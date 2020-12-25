using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbBackUpService;
using DbBackupEntities;
using System.IO;
using System.Diagnostics;
using System.Configuration;
using System.Threading;

namespace DatabaseBakup
{
    class Program
    {
        public static Queue<string> DumpRequestsQueue = new Queue<string>();
        static void Main(string[] args)
        {
            while (true)
            {
                ExecuteJob();
                Thread.Sleep(1*60*1000);
            }
        }
        public static void ExecuteJob()
        {
            object _lockObject = new object();

            //Console.WriteLine("#########################################");
            //Console.WriteLine("* SNK Database Backup Scheduler Started *");
            //Console.WriteLine("#########################################");
            
            Console.WriteLine("Job stated at :" + System.DateTime.Now.ToLocalTime());

            DbJobService dbService = new DbJobService();
            string sqlcmd = string.Empty;
            string myInsertMethod = string.Empty;
            int fileCount = 0;
            long logId = 0;
            List<string> dirForCompress = new List<string>();
            DateTime jobStartTime;
            //Step 1 : Get Active Jobs
            List<DbJobDetail> dbBackupJobs = new List<DbJobDetail>();
            dbBackupJobs = dbService.GetJobDetails(active: 1);
            if (dbBackupJobs.Count < 1)
            {
                Console.WriteLine("No Job Scheduled.");
            }
            else
            {
                //string shortNameDescription = "SNK";

                foreach (var job in dbBackupJobs)
                {
                    //Step 2 : Get Database and Table Names and backup configuration

                    //JobDbConfigModel jobDbConfigModel = dbService.GetDatabaseTableAndBackupConfig(job.JobId);

                    jobStartTime = System.DateTime.Now;

                    logId = addJobLog(new DbLog { JobId = job.JobId, JobName = job.JobTitle, Status = "Running", StartDate = jobStartTime,EndDate=jobStartTime});
                    updateJobStatus(new DbLog { JobId = job.JobId,Status = "Running"});
                    string currBackupDir = job.DirForBackup + "\\" + job.JobTitle.Trim();
                    string folderName = DateTime.Now.ToString("dd-MM-yyyy");
                    int dirCount = GetExitingDirCount(currBackupDir, folderName+"*");
                    currBackupDir += "\\" + folderName + "-" + dirCount.ToString();
                    dirForCompress.Add(currBackupDir);
                    if (!Directory.Exists(currBackupDir))
                    {
                        Directory.CreateDirectory(currBackupDir);
                        addJobDetailsLog(new DbLogDetails { 
                            LogId = Convert.ToInt32(logId), 
                            LogLevel = LogLevel.Info, 
                            LogTime = DateTime.Now, 
                            Message = "Backup Directory Created : "+currBackupDir});
                    }

                    //full database back up
                    fileCount = GetExitingFileCount(currBackupDir, job.DbName + "_FullDatabase_" + jobStartTime.ToString("yyyyMMdd"));

                    string resultFileFullDb = @"""" + currBackupDir + "\\" + job.DbName + "_FullDatabase_" + jobStartTime.ToString("yyyyMMdd") + "_" + fileCount.ToString() + @".sql""";


                    fileCount = GetExitingFileCount(currBackupDir, job.DbName + "_Alldata_" + jobStartTime.ToString("yyyyMMdd"));

                    string resultFileAllData = @"""" + currBackupDir + "\\" + job.DbName + "_Alldata_" + jobStartTime.ToString("yyyyMMdd") + "_" + fileCount.ToString() + @".sql""";

                    fileCount = GetExitingFileCount(currBackupDir, job.DbName + "_routines_" + jobStartTime.ToString("yyyyMMdd"));
                    string resultFileRoutines = @"""" + currBackupDir + "\\" + job.DbName + "_routines_" + jobStartTime.ToString("yyyyMMdd") + "_" + fileCount.ToString() + @".sql""";

                    if (job.InsertMethod == "InsertIgnore")
                    {
                        myInsertMethod = "--insert-ignore";
                    }
                    else
                    {
                        myInsertMethod = "--replace";
                    }

                    if (job.FullDbBackup)
                    {
                        sqlcmd = string.Format(" --opt --single-transaction --quick --lock-tables=false -u{0} -p{1} -h{2} --default-character-set=utf8 --databases {3} >{4} ", job.DbUserName, job.DbPassword, job.DbServerName, job.DbName, resultFileFullDb);
                        try
                        {
                            CreateAndExecuteDbBackupBatchFile(sqlcmd);
                            addJobDetailsLog(new DbLogDetails
                            {
                                LogId = Convert.ToInt32(logId),
                                LogLevel = LogLevel.Info,
                                LogTime = DateTime.Now,
                                Message = "Backup Created for : " + job.DbName
                            });
                        }
                        catch (Exception ex)
                        {
                            addJobDetailsLog(new DbLogDetails
                            {
                                LogId = Convert.ToInt32(logId),
                                LogLevel = LogLevel.Errors,
                                LogTime = DateTime.Now,
                                Message = "Error : " + ex.InnerException.ToString()
                            });
                        }
                    }
                    if (job.BackupMethod == "SaveInSeparateFiles" && !job.FullDbBackup)
                    {
                        List<string> myTempTables = job.DbTableList.Split(',').ToList();
                        foreach (string table in myTempTables)
                        {
                            fileCount = GetExitingFileCount(currBackupDir, job.DbName + table + "_" + jobStartTime.ToString("yyyyMMdd"));
                            string resultFileTableData = @"""" + currBackupDir + "\\" + job.DbName + "_" + table + "_" + jobStartTime.ToString("yyyyMMdd") + "_" + fileCount.ToString() + @".sql""";

                            sqlcmd = string.Format(" --opt --single-transaction --quick --lock-tables=false -u{0} -p{1} -h{2} " + myInsertMethod + " --default-character-set=utf8 {3} {4} > {5}", job.DbUserName, job.DbPassword, job.DbServerName, job.DbName, table, resultFileTableData);
                            try
                            {
                                CreateAndExecuteDbBackupBatchFile(sqlcmd);
                                //RunCmdProg(sqlcmd);
                                addJobDetailsLog(new DbLogDetails
                               {
                                   LogId = Convert.ToInt32(logId),
                                   LogLevel = LogLevel.Info,
                                   LogTime = DateTime.Now,
                                   Message = "Backup Created for : " + table
                               }); 
                            }
                            catch (Exception ex)
                            {
                                addJobDetailsLog(new DbLogDetails
                                {
                                    LogId = Convert.ToInt32(logId),
                                    LogLevel = LogLevel.Errors,
                                    LogTime = DateTime.Now,
                                    Message = "Error : " + ex.InnerException.ToString()
                                }); 
                            }
                        }
                    }
                    if (job.BackupMethod == "SaveInOneFile" && !job.FullDbBackup)
                    {
                        var tables = job.DbTableList.Replace(",", " ");
                        sqlcmd = string.Format(" --opt --single-transaction --quick --lock-tables=false -u{0} -p{1} -h{2} " + myInsertMethod + " --default-character-set=utf8 {3} {4} > {5}", job.DbUserName, job.DbPassword, job.DbServerName,job.DbName, tables, resultFileAllData);
                        try
                        {
                            CreateAndExecuteDbBackupBatchFile(sqlcmd);
                            //RunCmdProg(sqlcmd);
                            addJobDetailsLog(new DbLogDetails
                            {
                                LogId = Convert.ToInt32(logId),
                                LogLevel = LogLevel.Info,
                                LogTime = DateTime.Now,
                                Message = "Backup Created for SaveInOneFile."
                            }); 
                        }
                        catch (Exception ex)
                        {
                            addJobDetailsLog(new DbLogDetails
                            {
                                LogId = Convert.ToInt32(logId),
                                LogLevel = LogLevel.Errors,
                                LogTime = DateTime.Now,
                                Message = "Error SaveInOneFile : " + ex.InnerException
                            }); 
                        }
                    }
                    if (job.SaveStructure && !job.FullDbBackup)
                    {
                        sqlcmd = string.Format(" --opt --single-transaction --quick --lock-tables=false -u{0} -p{1} -h{2} --no-data --triggers --routines --dump-date {3} > {4}", job.DbUserName, job.DbPassword, job.DbServerName, job.DbName ,resultFileRoutines);
                        try
                        {
                            CreateAndExecuteDbBackupBatchFile(sqlcmd);
                            //RunCmdProg(sqlcmd);
                            addJobDetailsLog(new DbLogDetails
                            {
                                LogId = Convert.ToInt32(logId),
                                LogLevel = LogLevel.Info,
                                LogTime = DateTime.Now,
                                Message = "Backup Created for SaveStructure."
                            }); 
                        }
                        catch (Exception ex)
                        {
                            addJobDetailsLog(new DbLogDetails
                            {
                                LogId = Convert.ToInt32(logId),
                                LogLevel = LogLevel.Errors,
                                LogTime = DateTime.Now,
                                Message = "Error SaveStructure : " + ex.InnerException
                            }); 
                        }
                    }

                    KeepLastXBackup(job.DirForBackup + "\\" + job.JobTitle.Trim(), job.KeepXBackups);

                    updateJobLog(logId,"Success");
                    updateJobNextRun(job);
                }
                //Step 4 : Execute Queue
                //while (DumpRequestsQueue.Count > 0)
                //{
                //    RunCmdProg(DumpRequestsQueue.Dequeue());
                //}
                foreach (var job in dirForCompress)
                {
                    CompressFiles(job);
                }
                
            }
            //Step 5 : Send Notification
            Console.WriteLine("* SNK Database Backup Job Completed *");
            Console.WriteLine("Now Ideal State");
        }

        private static void CreateAndExecuteDbBackupBatchFile(string command)
        {
            Console.WriteLine("Full Backup started");
            StreamWriter sw = new StreamWriter("fullbackup.bat", false);
            string mySqlPath = ConfigurationManager.AppSettings["MYSQL_DUMP_LOCATION"].ToString();
 
            sw.WriteLine("\"{0}\"{1}", mySqlPath, command);
            sw.Close();

            ProcessStartInfo _proc = new ProcessStartInfo();
            _proc.CreateNoWindow = false;
            _proc.RedirectStandardOutput = true;
            _proc.UseShellExecute = false;
            _proc.FileName = "fullbackup.bat";
            Process proc = Process.Start(_proc);
            proc.WaitForExit();
            
            File.Delete("fullbackup.bat");
            Console.WriteLine("Full Backup completed");
        }

        private static int GetExitingFileCount(string path,string fileName)
        {
            return Directory.GetFiles(path,fileName + "_*.7z").Count() + 1;
        }

        private static void KeepLastXBackup(string path,int keep)
        {
            try
            {
                int folderCnt = Directory.GetDirectories(path).Count();
                var dir = Directory.GetDirectories(path);
                List<DirectoryInfo> dirInfo = new List<DirectoryInfo>();
                if (folderCnt > keep)
                {
                    foreach (var item in Directory.GetDirectories(path))
                    {
                        dirInfo.Add(new DirectoryInfo(item));
                    }

                    var dirPath = dirInfo.OrderByDescending(x => x.LastWriteTime).Skip(keep).Select(x => x);
                    if (dirPath != null)
                    {
                        foreach (var item in dirPath)
                        {
                            item.Delete(true);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
        private static int GetExitingDirCount(string path,string folderName)
        {
            if (Directory.Exists(path))
            {
                return Directory.GetDirectories(path, folderName).Count() + 1;
            }
            return 1;
        }
        private static void CompressFiles(string path)
        {
            try
            {
                StreamWriter sw = new StreamWriter("compress.bat", false);
                foreach (var files in Directory.GetFiles(path,"*.sql"))
                {
                    string sourceFile = files;
                    string targetFile = files.Replace(".sql", ".7z");
                    string compressPath = ConfigurationManager.AppSettings["COMPRESS_7Z_PATH"].ToString();
                    sw.WriteLine("\"{0}\" a -t7z \"{1}\" \"{2}\" -sdel", compressPath, targetFile, sourceFile);
                }
                sw.Close();
                Console.WriteLine("7Zip Compress started");
                ProcessStartInfo _proc = new ProcessStartInfo();
                _proc.CreateNoWindow = false;
                _proc.RedirectStandardOutput = true;
                _proc.UseShellExecute = false;
                _proc.FileName = "compress.bat";
                Process proc = Process.Start(_proc);
                proc.WaitForExit();

                File.Delete("compress.bat");
                Console.WriteLine("7Zip Compress completed");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        private static long addJobLog(DbLog logs)
        {
            DbJobService service = new DbJobService();
            return service.addJobLog(logs);
        }

        private static void updateJobStatus(DbLog logs)
        {
            DbJobService service = new DbJobService();
            service.updateJobStatus(logs);
        }
        private static void updateJobLog(long logId, string status)
        {
            DbJobService service = new DbJobService();
            service.updateJobLog(logId,status);
        }

        private static void addJobDetailsLog(DbLogDetails logDetails)
        {
            DbJobService service = new DbJobService();
            service.addJobDetailsLog(logDetails);
        }

        private static void updateJobNextRun(DbJobDetail job)
        {
            DbJobService service = new DbJobService();
            service.updateJobNextRun(job);
            service.updateJobNextRun(job);
        }

        public static void RunCmdProg(string cmdCommand)
        {
            //Process _proc = new Process();
            //_proc.EnableRaisingEvents = true;
            //_proc.StartInfo.CreateNoWindow = true;
            //_proc.StartInfo.RedirectStandardOutput = true;
            //_proc.StartInfo.UseShellExecute = false;
            //_proc.StartInfo.FileName = ConfigurationManager.AppSettings["MYSQL_DUMP_LOCATION"].ToString() +"\mysqldump.exe";
            //_proc.StartInfo.Arguments = cmdCommand;
            //_proc.Start();

            ProcessStartInfo _proc = new ProcessStartInfo();
            _proc.CreateNoWindow = true;
            _proc.RedirectStandardOutput = true;
            _proc.UseShellExecute = false;
            _proc.FileName = ConfigurationManager.AppSettings["MYSQL_DUMP_LOCATION"].ToString();
            _proc.Arguments = cmdCommand;
            Process proc = Process.Start(_proc);
            proc.WaitForExit();
        }
    }
}
