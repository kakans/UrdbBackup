using DbBackupEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbBackupDataModel;

namespace DbBackUpService
{
    public class DbJobService
    {

        public List<DbJobDetail> GetJobDetails(int active=1)
        {
            List<DbJobDetail> Jobs = new List<DbJobDetail>();
            DbJobDataModel dataModel = new DbJobDataModel();
            return dataModel.GetJobDetails(active);
        }

        public JobDbConfigModel GetDatabaseTableAndBackupConfig(int jobId)
        {
            DbJobDataModel dataModel = new DbJobDataModel();
            return dataModel.GetDatabaseTableAndBackupConfig(jobId);
        }

        public bool CreateJobDetails(DbJobDetail model)
        {
            DbJobDataModel dataModel = new DbJobDataModel();
            return dataModel.CreateJobDetails(model);
        }

        public DbJobDetail GetJobDetailsById(int id)
        {
            DbJobDataModel dataModel = new DbJobDataModel();
            return dataModel.GetJobDetailsById(id);
        }

        public bool UpdateJobDetails(DbJobDetail model)
        {
            DbJobDataModel dataModel = new DbJobDataModel();
            return dataModel.UpdateJobDetails(model);
        }

        public bool DeleteJobDetailsById(int id)
        {
            DbJobDataModel dataModel = new DbJobDataModel();
            return dataModel.DeleteJobDetailsById(id);
        }

        public bool updateJobNextRun(DbJobDetail job)
        {
            DbJobDataModel dataModel = new DbJobDataModel();
            return dataModel.updateJobNextRun(job);
        }

        public void updateJobStatus(DbLog logs)
        {
            DbJobDataModel dataModel = new DbJobDataModel();
            dataModel.updateJobStatus(logs);
        }

        public long addJobLog(DbLog logs)
        {
            DbJobDataModel dataModel = new DbJobDataModel();
            return dataModel.addJobLog(logs);
        }

        public void updateJobLog(long logId, string status)
        {
            DbJobDataModel dataModel = new DbJobDataModel();
            dataModel.updateJobLog(logId, status);
        }

        public bool addJobDetailsLog(DbLogDetails logDetails)
        {
            DbJobDataModel dataModel = new DbJobDataModel();
            return dataModel.addJobDetailsLog(logDetails);
        }
        
        public List<DbLog> GetJobLog()
        {
            List<DbLog> Jobs = new List<DbLog>();
            DbJobDataModel dataModel = new DbJobDataModel();
            return dataModel.GetJobLog();
        }

        public List<DbLogDetails> GetJobLogDetails(int id)
        {
            List<DbLogDetails> Jobs = new List<DbLogDetails>();
            DbJobDataModel dataModel = new DbJobDataModel();
            return dataModel.GetJobLogDetails(id);
        }

        public List<string> GetDatabaseList(DbJobDetail model)
        {
            DbJobDataModel dataModel = new DbJobDataModel();
            return dataModel.GetDatabaseList(model);
        }

        public List<string> GetDatabaseTableList(DbJobDetail model)
        {
            DbJobDataModel dataModel = new DbJobDataModel();
            return dataModel.GetDatabaseTableList(model);
        }

        public void RunNow(string id)
        {
            DbJobDataModel dataModel = new DbJobDataModel();
            dataModel.RunNow(id);
        }

        public DbJobDetail CheckJobDetailsForDelete(int id)
        {
            DbJobDataModel dataModel = new DbJobDataModel();
            return dataModel.CheckJobDetailsForDelete(id);
        }

        public List<DBJobListModel> GetJobList()
        {
            DbJobDataModel dataModel = new DbJobDataModel();
            return dataModel.GetJobList();
        }
        public List<int> GetAssignedJobList(int id)
        {
            DbJobDataModel dataModel = new DbJobDataModel();
            return dataModel.GetAssignedJobList(id);
        }
    }
}
