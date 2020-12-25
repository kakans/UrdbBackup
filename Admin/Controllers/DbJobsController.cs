using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DbBackUpService;
using DbBackupEntities;
using Admin.Models;

namespace Admin.Controllers
{
    [Authorize]
    public class DbJobsController : Controller
    {
        //
        // GET: /DbJobs/
        [ClaimsAuthorize(PermissionName.DB_JOB_VIEW,PermissionName.CLAIM_Y)]
        public ActionResult Index()
        {
            DbJobService dbService = new DbJobService();
            List<DbJobDetail> dbBackupJobs = new List<DbJobDetail>();
            dbBackupJobs = dbService.GetJobDetails(active: 0);
            return View(dbBackupJobs);
        }
        [ClaimsAuthorize(PermissionName.DB_JOB_CREATE, PermissionName.CLAIM_Y)]
        public ActionResult Create()
        {
            DbJobDetail model = new DbJobDetail();
            model.DirForBackup = ConfigurationSettings.GetApplicationSetting("BackUpPath");
            model.LastRun = System.DateTime.Now;
            model.IsActive = true;
            model.DeleteOldBackups = true;
            model.KeepXBackups = 10;
            return View(model);
        }
        public ActionResult GetDatabaseList(DbJobDetail model)
        {
            DbJobService dbService = new DbJobService();
            var databaseList = dbService.GetDatabaseList(model);
            return Json(databaseList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTableList(FormCollection collection)
        {
            DbJobDetail model = new DbJobDetail();
            model.DbServerName = collection["hdnDbServerName"];
            model.DbUserName = collection["hdnDbUserName"];
            model.DbPassword = collection["hdnDbPassword"];
            model.DbName = collection["DbName"];
            
            DbJobService dbService = new DbJobService();
            var tableList = dbService.GetDatabaseTableList(model);
            return Json(tableList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult VerifyConnection(FormCollection collection)
        {
            DbJobDetail model = new DbJobDetail();
            model.DbServerName = collection["hdnDbServerName"];
            model.DbUserName = collection["hdnDbUserName"];
            model.DbPassword = collection["hdnDbPassword"];
            model.DbName = collection["DbName"];
            DbJobService dbService = new DbJobService();
            var tableList = dbService.GetDatabaseTableList(model);
            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        [ClaimsAuthorize(PermissionName.DB_JOB_RUNNOW, PermissionName.CLAIM_Y)]
        public ActionResult RunNow(string id)
        {
            DbJobService dbService = new DbJobService();
            dbService.RunNow(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ClaimsAuthorize(PermissionName.DB_JOB_CREATE, PermissionName.CLAIM_Y)]
        public ActionResult Create(FormCollection collection, DbJobDetail model)
        {
            model.DbServerName = collection["hdnDbServerNameTmp"];
            model.DbUserName = collection["hdnDbUserNameTmp"];
            model.DbPassword = collection["hdnDbPasswordTmp"];
            model.DbName = collection["hdnDbNameTmp"];
            model.FullDbBackup = Convert.ToBoolean(collection["hdnFullDbBackupTmp"]);
            model.DbTableList = collection["hdnDbTableListTmp"];
            
            DbJobService dbService = new DbJobService();
            List<DbJobDetail> dbBackupJobs = new List<DbJobDetail>();
            if( dbService.CreateJobDetails(model))
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [ClaimsAuthorize(PermissionName.DB_JOB_EDIT, PermissionName.CLAIM_Y)]
        public ActionResult Edit(int id)
        {
            DbJobDetail model = new DbJobDetail();
            DbJobService dbService = new DbJobService();
            model = dbService.GetJobDetailsById(id);
            return View(model);
        }
        
        [HttpPost]
        [ClaimsAuthorize(PermissionName.DB_JOB_EDIT, PermissionName.CLAIM_Y)]
        public ActionResult Edit(DbJobDetail model)
        {
            DbJobService dbService = new DbJobService();
            if (dbService.UpdateJobDetails(model))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }

        [ClaimsAuthorize(PermissionName.DB_JOB_DELETE, PermissionName.CLAIM_Y)]
        public ActionResult Delete(int id)
        {
            DbJobDetail model = new DbJobDetail();
            DbJobService dbService = new DbJobService();
            model = dbService.CheckJobDetailsForDelete(id);
            return View(model);
        }
        [HttpPost]
        [ClaimsAuthorize(PermissionName.DB_JOB_DELETE, PermissionName.CLAIM_Y)]
        public ActionResult Delete(DbJobDetail model)
        {
            DbJobService dbService = new DbJobService();
            if (dbService.DeleteJobDetailsById(model.JobId))
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }
	}
}