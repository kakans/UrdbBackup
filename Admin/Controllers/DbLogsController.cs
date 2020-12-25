using DbBackUpService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DbBackupEntities;
using Admin.Models;
namespace Admin.Controllers
{
    [Authorize]
    public class DbLogsController : Controller
    {
        //
        // GET: /DbLogs/
        [ClaimsAuthorize(PermissionName.DB_LOGS_VIEW, PermissionName.CLAIM_Y)]
        public ActionResult Index()
        {
            DbJobService dbService = new DbJobService();
            List<DbLog> dbJobLog = new List<DbLog>();
            dbJobLog = dbService.GetJobLog();
            return View(dbJobLog);
        }
        
        [ClaimsAuthorize(PermissionName.DB_LOGS_DETAILS, PermissionName.CLAIM_Y)]
        public ActionResult Details(int id)
        {
            DbJobService dbService = new DbJobService();
            List<DbLogDetails> dbJobLogDetails = new List<DbLogDetails>();
            dbJobLogDetails = dbService.GetJobLogDetails(id);
            return View(dbJobLogDetails);
        }
	}
}