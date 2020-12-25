using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DbBackupEntities;
using DbBackUpService;

namespace Admin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            DbJobService jobservice = new DbJobService();
            List<DBJobListModel> model = new List<DBJobListModel>();
            model = jobservice.GetJobList();
            return View(model);
        }

        public ActionResult About()
        { 
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}