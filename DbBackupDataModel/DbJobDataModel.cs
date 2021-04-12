using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbBackupEntities;
using System.Data;
using MySql.Data.MySqlClient;
using System.Globalization;

namespace DbBackupDataModel
{
    public class DbJobDataModel
    {
        public List<DbJobDetail> GetJobDetails(int active = 1)
        {
            List<DbJobDetail> jobs = new List<DbJobDetail>();
            MySqlDatabase db = new MySqlDatabase();
            DataSet ds = new DataSet();

            string sql = string.Format(@"SELECT id,JobName,FrequencyInHrs,LastRun,NextRun,DirForBackUp,
                                        BackupMethod,InsertMethod,SaveStructure,CompressToZip,DeleteOldBackups,
                                        KeepXBackups,DbUserName,DbPassword,DbServer,DbName,DbTableList,IsActive,
                                        FullDbBackup, TIMESTAMPDIFF(HOUR, LastRun, NOW()) AS BackupSince,Status 
                                        FROM DbJobDetails");
            if (active == 1)
            {
                sql = sql + " where isactive = 1 and NextRun < now() ";
            }
            sql = sql + " ORDER BY JobName";
            ds = db.GetDataSet(sql, CommandType.Text);
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                jobs.Add(new DbJobDetail
                {
                    JobId = Convert.ToInt32(ds.Tables[0].Rows[j]["id"]),
                    JobTitle = Convert.ToString(ds.Tables[0].Rows[j]["JobName"]),
                    FrequencyInHrs = Convert.ToInt32(ds.Tables[0].Rows[j]["FrequencyInHrs"]),
                    LastRun = Convert.ToDateTime(ds.Tables[0].Rows[j]["LastRun"]),
                    NextRun = Convert.ToDateTime(ds.Tables[0].Rows[j]["NextRun"]),
                    DirForBackup = Convert.ToString(ds.Tables[0].Rows[j]["DirForBackUp"]),
                    BackupMethod = Convert.ToString(ds.Tables[0].Rows[j]["BackupMethod"]),
                    InsertMethod = Convert.ToString(ds.Tables[0].Rows[j]["InsertMethod"]),
                    SaveStructure = Convert.ToBoolean(ds.Tables[0].Rows[j]["SaveStructure"]),
                    CompressToZip = Convert.ToBoolean(ds.Tables[0].Rows[j]["CompressToZip"]),
                    DeleteOldBackups = Convert.ToBoolean(ds.Tables[0].Rows[j]["DeleteOldBackups"]),
                    KeepXBackups = Convert.ToInt32(ds.Tables[0].Rows[j]["KeepXBackups"]),
                    DbUserName = Convert.ToString(ds.Tables[0].Rows[j]["DbUserName"]),
                    DbPassword = Convert.ToString(ds.Tables[0].Rows[j]["DbPassword"]),
                    DbServerName = Convert.ToString(ds.Tables[0].Rows[j]["DbServer"]),
                    DbName = Convert.ToString(ds.Tables[0].Rows[j]["DbName"]),
                    DbTableList = Convert.ToString(ds.Tables[0].Rows[j]["dbtablelist"]),
                    IsActive = Convert.ToBoolean(ds.Tables[0].Rows[j]["isactive"]),
                    FullDbBackup = Convert.ToBoolean(ds.Tables[0].Rows[j]["FullDbBackup"]),
                    BackupSince = Convert.ToInt32(ds.Tables[0].Rows[j]["BackupSince"]),
                    JobStatus = Convert.ToString(ds.Tables[0].Rows[j]["Status"])
                });
            }
            return jobs;
        }

        public JobDbConfigModel GetDatabaseTableAndBackupConfig(int jobId)
        {
            JobDbConfigModel model = new JobDbConfigModel();
            DbJob dbJob = new DbJob();
            DbServer dbServer = new DbServer();
            DbBackupConfig dbBackupConfig = new DbBackupConfig();

            MySqlDatabase db = new MySqlDatabase();
            DataSet ds = new DataSet();
            string sqlQuery = string.Format("select job.Name,job.FrequencyInHrs,job.LastRun,job.NextRun,job.DbTableList, config.DirForBackup,config.SaveInSeparateFiles,config.SaveInOneFile,config.InsertIgnore,config.ReplaceInsert, config.SaveStructure,config.CompressToZip,config.DeleteOldBackups,config.KeepXBackups, dbs.DbUserName,dbs.DbPassword,dbs.DbServer,dbs.DbName from DbBackupJob job join DbBackupConfig config on job.DbConfigId = config.Id and job.IsActive = 1 join DbServerDetails dbs on job.DbServerId = dbs.Id where job.Id={0}", jobId);
            ds = db.GetDataSet(sqlQuery, CommandType.Text);
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                dbJob.JobTitle = Convert.ToString(ds.Tables[0].Rows[j]["Name"]);
                dbJob.FrequencyInHrs = Convert.ToInt32(ds.Tables[0].Rows[j]["FrequencyInHrs"]);
                dbJob.LastRun = Convert.ToDateTime(ds.Tables[0].Rows[j]["LastRun"]);
                dbJob.NextRun = Convert.ToDateTime(ds.Tables[0].Rows[j]["NextRun"]);
                dbJob.DbTableList = Convert.ToString(ds.Tables[0].Rows[j]["DbTableList"]);
                dbBackupConfig.DirForBackup = Convert.ToString(ds.Tables[0].Rows[j]["DirForBackup"]);
                dbBackupConfig.SaveInSeparateFiles = (ds.Tables[0].Rows[j]["SaveInSeparateFiles"] == DBNull.Value) ? false : (Convert.ToInt32(ds.Tables[0].Rows[j]["SaveInSeparateFiles"]) == 1) ? true : false;
                dbBackupConfig.SaveInOneFile = (ds.Tables[0].Rows[j]["SaveInOneFile"] == DBNull.Value) ? false : (Convert.ToInt32(ds.Tables[0].Rows[j]["SaveInOneFile"]) == 1) ? true : false;
                dbBackupConfig.InsertIgnore = (ds.Tables[0].Rows[j]["InsertIgnore"] == DBNull.Value) ? false : (Convert.ToInt32(ds.Tables[0].Rows[j]["InsertIgnore"]) == 1) ? true : false;
                dbBackupConfig.ReplaceInsert = (ds.Tables[0].Rows[j]["ReplaceInsert"] == DBNull.Value) ? false : (Convert.ToInt32(ds.Tables[0].Rows[j]["ReplaceInsert"]) == 1) ? true : false;
                dbBackupConfig.SaveStructure = (ds.Tables[0].Rows[j]["SaveStructure"] == DBNull.Value) ? false : (Convert.ToInt32(ds.Tables[0].Rows[j]["SaveStructure"]) == 1) ? true : false;
                dbBackupConfig.CompressToZip = (ds.Tables[0].Rows[j]["CompressToZip"] == DBNull.Value) ? false : (Convert.ToInt32(ds.Tables[0].Rows[j]["CompressToZip"]) == 1) ? true : false;
                dbBackupConfig.DeleteOldBackups = (ds.Tables[0].Rows[j]["DeleteOldBackups"] == DBNull.Value) ? false : (Convert.ToInt32(ds.Tables[0].Rows[j]["DeleteOldBackups"]) == 1) ? true : false;
                dbBackupConfig.KeepXBackups = Convert.ToInt32(ds.Tables[0].Rows[j]["KeepXBackups"]);
                dbServer.DbUserName = Convert.ToString(ds.Tables[0].Rows[j]["DbUserName"]);
                dbServer.DbPassword = Convert.ToString(ds.Tables[0].Rows[j]["DbPassword"]);
                dbServer.DbServerName = Convert.ToString(ds.Tables[0].Rows[j]["DbServer"]);
                dbServer.DbName = Convert.ToString(ds.Tables[0].Rows[j]["DbName"]);
                model.dbJob = dbJob;
                model.dbServer = dbServer;
                model.dbBackupConfig = dbBackupConfig;
            }
            return model;
        }

        public List<int> GetAssignedJobList(int id)
        {
            List<int> jobs = new List<int>();
            MySqlDatabase db = new MySqlDatabase();
            DataSet ds = new DataSet();

            string sql = string.Format(@"SELECT UserJobId
                                        FROM UserJob
                                        WHERE IsActive=1 and UserId=@userId");

            MySql.Data.MySqlClient.MySqlParameter[] param = new MySql.Data.MySqlClient.MySqlParameter[1];
            param[0] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "userId", DbType = DbType.Int16, Value = id };

            ds = db.GetDataSet(sql, CommandType.Text,param);
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                jobs.Add(Convert.ToInt32(ds.Tables[0].Rows[j]["UserJobId"]));
            }
            return jobs;
        }

        public List<DBJobListModel> GetJobList()
        {
            List<DBJobListModel> jobs = new List<DBJobListModel>();
            MySqlDatabase db = new MySqlDatabase();
            DataSet ds = new DataSet();

            string sql = string.Format(@"SELECT Id, JobName,DBName,
                                        CONCAT(FLOOR(TIMESTAMPDIFF(MINUTE, now(), NextRun)/60),'h ',MOD(TIMESTAMPDIFF(MINUTE, now(), NextRun),60),'m') 'RunIn',
                                        Status,IsActive
                                        FROM DbJobDetails
                                        WHERE FLOOR(TIMESTAMPDIFF(MINUTE, now(), NextRun)/60) < 1 
                                        ");
            
            ds = db.GetDataSet(sql, CommandType.Text);
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                jobs.Add(new DBJobListModel
                {
                    JobId = Convert.ToInt32(ds.Tables[0].Rows[j]["Id"]),
                    JobTitle = Convert.ToString(ds.Tables[0].Rows[j]["JobName"]),
                    RunIn = Convert.ToString(ds.Tables[0].Rows[j]["RunIn"]),
                    DbName = Convert.ToString(ds.Tables[0].Rows[j]["DBName"]),
                    Status = Convert.ToString(ds.Tables[0].Rows[j]["Status"]),
                    IsActive = Convert.ToBoolean(ds.Tables[0].Rows[j]["IsActive"])
                });
            }
            return jobs;
        }

        public bool CreateJobDetails(DbJobDetail model)
        {
            try
            {
                MySqlDatabase db = new MySqlDatabase();
                DataSet ds = new DataSet();
                string sqlQuery = "INSERT INTO DbJobDetails(JobName,FrequencyInHrs,LastRun,NextRun,DirForBackUp,BackupMethod,InsertMethod,SaveStructure,CompressToZip,DeleteOldBackups,KeepXBackups,DbUserName,DbPassword,DbServer,DbName,DbTableList,IsActive,FullDbBackup)VALUES(@JobName,@FrequencyInHrs,@LastRun,DATE_ADD(@LastRun, INTERVAL @FrequencyInHrs HOUR),@DirForBackUp,@BackupMethod,@InsertMethod,@SaveStructure,@CompressToZip,@DeleteOldBackups,@KeepXBackups,@DbUserName,@DbPassword,@DbServer,@DbName,@DbTableList,@IsActive,@FullDbBackup)";
                MySql.Data.MySqlClient.MySqlParameter[] param = new MySql.Data.MySqlClient.MySqlParameter[17];
                param[0] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "JobName", DbType = DbType.String, Size = 45, Value = model.JobTitle };
                param[1] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "FrequencyInHrs", DbType = DbType.Int32, Value = model.FrequencyInHrs };
                param[2] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "LastRun", DbType = DbType.DateTime, Value = model.LastRun };
                //param[3] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "NextRun", DbType = DbType.DateTime, Value = model.NextRun };
                param[3] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "DirForBackUp", DbType = DbType.String, Size = 250, Value = model.DirForBackup };
                param[4] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "BackupMethod", DbType = DbType.String, Size = 25, Value = model.BackupMethod };
                param[5] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "InsertMethod", DbType = DbType.String, Size = 25, Value = model.InsertMethod };
                param[6] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "SaveStructure", DbType = DbType.Boolean, Size = 1, Value = model.SaveStructure };
                param[7] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "CompressToZip", DbType = DbType.Boolean, Size = 1, Value = model.CompressToZip };
                param[8] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "DeleteOldBackups", DbType = DbType.Boolean, Size = 1, Value = model.DeleteOldBackups };
                param[9] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "KeepXBackups", DbType = DbType.Int32, Value = model.KeepXBackups };
                param[10] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "DbUserName", DbType = DbType.String, Size = 250, Value = model.DbUserName };
                param[11] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "DbPassword", DbType = DbType.String, Size = 45, Value = model.DbPassword };
                param[12] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "DbServer", DbType = DbType.String, Size = 250, Value = model.DbServerName };
                param[13] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "DbName", DbType = DbType.String, Size = 45, Value = model.DbName };
                param[14] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "DbTableList", DbType = DbType.String, Size = 1000, Value = model.DbTableList };
                param[15] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "IsActive", DbType = DbType.Boolean, Size = 1, Value = 1 };
                param[16] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "FullDbBackup", DbType = DbType.Boolean, Size = 1, Value = model.FullDbBackup };
                db.ExecuteNonQuery(sqlQuery, CommandType.Text, param);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public DbJobDetail GetJobDetailsById(int id)
        {
            DbJobDetail model = new DbJobDetail();
            MySqlDatabase db = new MySqlDatabase();
            DataSet ds = new DataSet();

            string sql = string.Format("SELECT id,JobName,FrequencyInHrs,LastRun,NextRun,DirForBackUp,BackupMethod,InsertMethod,SaveStructure,CompressToZip,DeleteOldBackups,KeepXBackups,DbUserName,DbPassword,DbServer,DbName,DbTableList,IsActive,FullDbBackup FROM DbJobDetails");
            sql = sql + string.Format(" where id = {0} ", id);

            ds = db.GetDataSet(sql, CommandType.Text);
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                model.JobId = Convert.ToInt32(ds.Tables[0].Rows[j]["id"]);
                model.JobTitle = Convert.ToString(ds.Tables[0].Rows[j]["JobName"]);
                model.FrequencyInHrs = Convert.ToInt32(ds.Tables[0].Rows[j]["FrequencyInHrs"]);
                //model.LastRun = DateTime.ParseExact(ds.Tables[0].Rows[j]["LastRun"].ToString(), "MM/dd/yyyy HH:mm:ss tt", CultureInfo.InvariantCulture);
                model.LastRun =   Convert.ToDateTime(ds.Tables[0].Rows[j]["LastRun"],CultureInfo.InvariantCulture);
                //model.NextRun = DateTime.ParseExact(ds.Tables[0].Rows[j]["NextRun"].ToString(), "MM/dd/yyyy HH:mm:ss tt", CultureInfo.InvariantCulture);
                model.NextRun = Convert.ToDateTime(ds.Tables[0].Rows[j]["NextRun"],CultureInfo.InvariantCulture);
                model.DirForBackup = Convert.ToString(ds.Tables[0].Rows[j]["DirForBackUp"]);
                model.BackupMethod = Convert.ToString(ds.Tables[0].Rows[j]["BackupMethod"]);
                model.InsertMethod = Convert.ToString(ds.Tables[0].Rows[j]["InsertMethod"]);
                model.SaveStructure = Convert.ToBoolean(ds.Tables[0].Rows[j]["SaveStructure"]);
                model.CompressToZip = Convert.ToBoolean(ds.Tables[0].Rows[j]["CompressToZip"]);
                model.DeleteOldBackups = Convert.ToBoolean(ds.Tables[0].Rows[j]["DeleteOldBackups"]);
                model.KeepXBackups = Convert.ToInt32(ds.Tables[0].Rows[j]["KeepXBackups"]);
                model.DbUserName = Convert.ToString(ds.Tables[0].Rows[j]["DbUserName"]);
                model.DbPassword = Convert.ToString(ds.Tables[0].Rows[j]["DbPassword"]);
                model.DbServerName = Convert.ToString(ds.Tables[0].Rows[j]["DbServer"]);
                model.DbName = Convert.ToString(ds.Tables[0].Rows[j]["DbName"]);
                model.DbTableList = Convert.ToString(ds.Tables[0].Rows[j]["dbtablelist"]);
                model.IsActive = Convert.ToBoolean(ds.Tables[0].Rows[j]["isactive"]);
                model.FullDbBackup = Convert.ToBoolean(ds.Tables[0].Rows[j]["FullDbBackup"]);
            }
            var tableList = GetDatabaseTableList(
                new DbJobDetail 
                { 
                    DbServerName = model.DbServerName, 
                    DbUserName = model.DbUserName, 
                    DbPassword = model.DbPassword, 
                    DbName = model.DbName
                });
            model.TableList = new List<TableList>();
            foreach (var table in tableList)
	        {
                model.TableList.Add(new TableList
                {
                    IsSelected = model.DbTableList.Contains(table) ? true : false,
                    TableName = table
                });
	        }
            return model;
        }

        public bool UpdateJobDetails(DbJobDetail model)
        {
            try
            {
                MySqlDatabase db = new MySqlDatabase();
                DataSet ds = new DataSet();

                string sqlQuery = string.Format("Update DbJobDetails set JobName=@JobName,FrequencyInHrs=@FrequencyInHrs,DirForBackUp=@DirForBackUp,BackupMethod=@BackupMethod,InsertMethod=@InsertMethod,SaveStructure=@SaveStructure,CompressToZip=@CompressToZip,DeleteOldBackups=@DeleteOldBackups,KeepXBackups=@KeepXBackups,DbUserName=@DbUserName,DbPassword=@DbPassword,DbServer=@DbServer,DbName=@DbName,DbTableList=@DbTableList,IsActive=@IsActive,FullDbBackup=@FullDbBackup where id =@id");

                MySql.Data.MySqlClient.MySqlParameter[] param = new MySql.Data.MySqlClient.MySqlParameter[17];
                param[0] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "JobName", DbType = DbType.String, Size = 45, Value = model.JobTitle };
                param[1] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "FrequencyInHrs", DbType = DbType.Int32, Value = model.FrequencyInHrs };
                //param[2] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "LastRun", DbType = DbType.DateTime, Value = model.LastRun };
                //param[3] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "NextRun", DbType = DbType.DateTime, Value = model.NextRun };
                param[2] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "DirForBackUp", DbType = DbType.String, Size = 250, Value = model.DirForBackup };
                param[3] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "BackupMethod", DbType = DbType.String, Size = 25, Value = model.BackupMethod };
                param[4] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "InsertMethod", DbType = DbType.String, Size = 25, Value = model.InsertMethod };
                param[5] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "SaveStructure", DbType = DbType.Boolean, Size = 1, Value = model.SaveStructure };
                param[6] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "CompressToZip", DbType = DbType.Boolean, Size = 1, Value = model.CompressToZip };
                param[7] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "DeleteOldBackups", DbType = DbType.Boolean, Size = 1, Value = model.DeleteOldBackups };
                param[8] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "KeepXBackups", DbType = DbType.Int32, Value = model.KeepXBackups };
                param[9] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "DbUserName", DbType = DbType.String, Size = 250, Value = model.DbUserName };
                param[10] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "DbPassword", DbType = DbType.String, Size = 45, Value = model.DbPassword };
                param[11] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "DbServer", DbType = DbType.String, Size = 250, Value = model.DbServerName };
                param[12] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "DbName", DbType = DbType.String, Size = 45, Value = model.DbName };
                param[13] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "DbTableList", DbType = DbType.String, Size = 1000, Value = model.DbTableList };
                param[14] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "IsActive", DbType = DbType.Boolean, Size = 1, Value = model.IsActive };
                param[15] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "FullDbBackup", DbType = DbType.Boolean, Size = 1, Value = model.FullDbBackup };
                param[16] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "Id", DbType = DbType.Int32, Value = model.JobId };
                db.ExecuteNonQuery(sqlQuery, CommandType.Text, param);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public DbJobDetail CheckJobDetailsForDelete(int id)
        {
            DbJobDetail model = new DbJobDetail();
            MySqlDatabase db = new MySqlDatabase();
            DataSet ds = new DataSet();

            string sql = string.Format("SELECT id,JobName,FrequencyInHrs,LastRun,NextRun,DirForBackUp,BackupMethod,InsertMethod,SaveStructure,CompressToZip,DeleteOldBackups,KeepXBackups,DbUserName,DbPassword,DbServer,DbName,DbTableList,IsActive,FullDbBackup FROM DbJobDetails");
            sql = sql + string.Format(" where id = {0} ", id);

            ds = db.GetDataSet(sql, CommandType.Text);
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                model.JobId = Convert.ToInt32(ds.Tables[0].Rows[j]["id"]);
                model.JobTitle = Convert.ToString(ds.Tables[0].Rows[j]["JobName"]);
                model.FrequencyInHrs = Convert.ToInt32(ds.Tables[0].Rows[j]["FrequencyInHrs"]);
                //model.LastRun = DateTime.ParseExact(ds.Tables[0].Rows[j]["LastRun"].ToString(), "MM/dd/yyyy HH:mm:ss tt", CultureInfo.InvariantCulture);
                model.LastRun = Convert.ToDateTime(ds.Tables[0].Rows[j]["LastRun"], CultureInfo.InvariantCulture);
                //model.NextRun = DateTime.ParseExact(ds.Tables[0].Rows[j]["NextRun"].ToString(), "MM/dd/yyyy HH:mm:ss tt", CultureInfo.InvariantCulture);
                model.NextRun = Convert.ToDateTime(ds.Tables[0].Rows[j]["NextRun"], CultureInfo.InvariantCulture);
                model.DirForBackup = Convert.ToString(ds.Tables[0].Rows[j]["DirForBackUp"]);
                model.BackupMethod = Convert.ToString(ds.Tables[0].Rows[j]["BackupMethod"]);
                model.InsertMethod = Convert.ToString(ds.Tables[0].Rows[j]["InsertMethod"]);
                model.SaveStructure = Convert.ToBoolean(ds.Tables[0].Rows[j]["SaveStructure"]);
                model.CompressToZip = Convert.ToBoolean(ds.Tables[0].Rows[j]["CompressToZip"]);
                model.DeleteOldBackups = Convert.ToBoolean(ds.Tables[0].Rows[j]["DeleteOldBackups"]);
                model.KeepXBackups = Convert.ToInt32(ds.Tables[0].Rows[j]["KeepXBackups"]);
                model.DbUserName = Convert.ToString(ds.Tables[0].Rows[j]["DbUserName"]);
                model.DbPassword = Convert.ToString(ds.Tables[0].Rows[j]["DbPassword"]);
                model.DbServerName = Convert.ToString(ds.Tables[0].Rows[j]["DbServer"]);
                model.DbName = Convert.ToString(ds.Tables[0].Rows[j]["DbName"]);
                model.DbTableList = Convert.ToString(ds.Tables[0].Rows[j]["dbtablelist"]);
                model.IsActive = Convert.ToBoolean(ds.Tables[0].Rows[j]["isactive"]);
                model.FullDbBackup = Convert.ToBoolean(ds.Tables[0].Rows[j]["FullDbBackup"]);
            }

            model.TableList = new List<TableList>();
            return model;
        }

        public bool DeleteJobDetailsById(int id)
        {
            try
            {
                MySqlDatabase db = new MySqlDatabase();
                DataSet ds = new DataSet();

                string sqlQuery = string.Format("Delete from DbJobDetails where id =@id");

                MySql.Data.MySqlClient.MySqlParameter[] param = new MySql.Data.MySqlClient.MySqlParameter[1];
                param[0] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "Id", DbType = DbType.Int32, Value = id };
                db.ExecuteNonQuery(sqlQuery, CommandType.Text, param);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool updateJobNextRun(DbJobDetail job)
        {
            try
            {
                MySqlDatabase db = new MySqlDatabase();
                DataSet ds = new DataSet();

                string sqlQuery = string.Format("UPDATE DbJobDetails set Status='Success', LastRun=now(), NextRun=DATE_ADD(now(), INTERVAL FrequencyInHrs HOUR) where id =@id");

                MySql.Data.MySqlClient.MySqlParameter[] param = new MySql.Data.MySqlClient.MySqlParameter[1];
                param[0] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "Id", DbType = DbType.Int32, Value = job.JobId };
                db.ExecuteNonQuery(sqlQuery, CommandType.Text, param);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public long addJobLog(DbLog logs)
        {
            long LastInsertedId;
            try
            {
                MySqlDatabase db = new MySqlDatabase();
                string sqlQuery = "INSERT INTO DbBackupLogs(JobId,JobName,Status,JobStartTime,JobEndTime)VALUES(@JobId,@JobName,@Status,@JobStartTime,@JobEndTime)";
                MySql.Data.MySqlClient.MySqlParameter[] param = new MySql.Data.MySqlClient.MySqlParameter[5];
                param[0] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "JobId", DbType = DbType.Int32, Value = logs.JobId };
                param[1] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "JobName", DbType = DbType.String, Size = 250, Value = logs.JobName };
                param[2] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "Status", DbType = DbType.String, Size = 250, Value = logs.Status };
                param[3] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "JobStartTime", DbType = DbType.String, Value = logs.StartDate.ToString("yyyy-MM-dd H:mm:ss") };
                param[4] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "JobEndTime", DbType = DbType.String, Value = logs.EndDate.ToString("yyyy-MM-dd H:mm:ss") };
                db.ExecuteNonQuery(sqlQuery, CommandType.Text, param, out LastInsertedId);
                return LastInsertedId;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public void updateJobStatus(DbLog logs)
        {
            try
            {
                MySqlDatabase db = new MySqlDatabase();
                string sqlQueryJob = "UPDATE DbJobDetails SET Status=@Status WHERE Id= @Id";
                MySql.Data.MySqlClient.MySqlParameter[] param = new MySql.Data.MySqlClient.MySqlParameter[2];
                param[0] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "Status", DbType = DbType.String, Size = 250, Value = logs.Status };
                param[1] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "Id", DbType = DbType.Int32,  Value = logs.JobId };
                db.ExecuteNonQuery(sqlQueryJob, CommandType.Text, param);
            }

            catch (Exception)
            {

                throw;
            }
        }

        public void updateJobLog(long logId, string status)
        {
            try
            {
                MySqlDatabase db = new MySqlDatabase();
                string sqlQuery = "UPDATE DbBackupLogs set Status=@Status,JobEndTime=@JobEndTime where Id=@LogId";
                MySql.Data.MySqlClient.MySqlParameter[] param = new MySql.Data.MySqlClient.MySqlParameter[3];
                param[0] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "Status", DbType = DbType.String, Size = 250, Value = status };
                param[1] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "JobEndTime", DbType = DbType.DateTime, Value = DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") };
                param[2] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "LogId", DbType = DbType.Int32, Value = Convert.ToInt32(logId) };
                db.ExecuteNonQuery(sqlQuery, CommandType.Text, param);
            }
            catch (Exception)
            {
                
                throw;
            }

        }

        public bool addJobDetailsLog(DbLogDetails logDetails)
        {
            try
            {
                MySqlDatabase db = new MySqlDatabase();
                DataSet ds = new DataSet();
                string sqlQuery = "INSERT INTO LogDetails(LogId,Level,LogTime,Message)VALUES(@LogId,@Level,@LogTime,@Message)";
                MySql.Data.MySqlClient.MySqlParameter[] param = new MySql.Data.MySqlClient.MySqlParameter[4];
                param[0] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "LogId", DbType = DbType.Int32, Value = logDetails.LogId };
                param[1] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "Level", DbType = DbType.String, Size = 250, Value = logDetails.LogLevel };
                param[2] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "LogTime", DbType = DbType.DateTime, Value = logDetails.LogTime.ToString("yyyy-MM-dd H:mm:ss") };
                param[3] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "Message", DbType = DbType.String, Size=255, Value = logDetails.Message};
                db.ExecuteNonQuery(sqlQuery, CommandType.Text, param);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<DbLog> GetJobLog()
        {
            List<DbLog> jobLogs = new List<DbLog>();
            MySqlDatabase db = new MySqlDatabase();
            DataSet ds = new DataSet();

            string sql = string.Format("SELECT id,JobId,JobName,Status,JobStartTime,JobEndTime FROM DbBackupLogs  order by id desc limit 100");
            ds = db.GetDataSet(sql, CommandType.Text);
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                jobLogs.Add(new DbLog
                {
                    Id = Convert.ToInt32(ds.Tables[0].Rows[j]["id"]),
                    JobId = Convert.ToInt32(ds.Tables[0].Rows[j]["JobId"]),
                    JobName = Convert.ToString(ds.Tables[0].Rows[j]["JobName"]),
                    Status = Convert.ToString(ds.Tables[0].Rows[j]["Status"]),
                    StartDate = Convert.ToDateTime(ds.Tables[0].Rows[j]["JobStartTime"]),
                    EndDate = Convert.ToDateTime(ds.Tables[0].Rows[j]["JobEndTime"])
                });
            }
            return jobLogs;  
        }



        public List<DbLogDetails> GetJobLogDetails(int id)
        {
            List<DbLogDetails> jobLogDetails = new List<DbLogDetails>();
            MySqlDatabase db = new MySqlDatabase();
            DataSet ds = new DataSet();

            string sql = string.Format("SELECT Level,LogTime,Message FROM LogDetails where LogId=@LogId order by id desc");
            MySql.Data.MySqlClient.MySqlParameter[] param = new MySql.Data.MySqlClient.MySqlParameter[1];
            param[0] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "LogId", DbType = DbType.Int32, Value = id };

            ds = db.GetDataSet(sql, CommandType.Text, param);
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                jobLogDetails.Add(new DbLogDetails
                {
                    LogLevel = LogLevel.Info,
                    LogTime = Convert.ToDateTime(ds.Tables[0].Rows[j]["LogTime"]),
                    Message = Convert.ToString(ds.Tables[0].Rows[j]["Message"])
                });
            }
            return jobLogDetails;  
        }

        public List<string> GetDatabaseList(DbJobDetail model)
        {
            MySqlDatabase db = new MySqlDatabase(model.DbServerName, model.DbUserName, model.DbPassword);
            DataSet ds = new DataSet();
            List<string> lstDatabases = new List<string>();

            string sql = string.Format("SHOW Databases");
            ds = db.GetDataSet(sql, CommandType.Text);
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                lstDatabases.Add(ds.Tables[0].Rows[j]["Database"].ToString());
            }
            return lstDatabases;
        }

        public List<string> GetDatabaseTableList(DbJobDetail model)
        {
            MySqlDatabase db = new MySqlDatabase(model.DbServerName, model.DbUserName, model.DbPassword);
            DataSet ds = new DataSet();
            List<string> lstDatabaseTables = new List<string>();

            string sql = string.Format("show tables from {0}",model.DbName);
            ds = db.GetDataSet(sql, CommandType.Text);
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                lstDatabaseTables.Add(ds.Tables[0].Rows[j][0].ToString());
            }
            return lstDatabaseTables;
        }

        public void RunNow(string id)
        {
            MySqlDatabase db = new MySqlDatabase();
            DataSet ds = new DataSet();

            string sqlQuery = string.Format("UPDATE DbJobDetails set NextRun=now(), Status ='Queued' where id =@id");

            MySql.Data.MySqlClient.MySqlParameter[] param = new MySql.Data.MySqlClient.MySqlParameter[1];
            param[0] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "Id", DbType = DbType.Int32, Value = id };
            db.ExecuteNonQuery(sqlQuery, CommandType.Text, param);
        }
    }
}
