using Admin.Models;
using DatabaseBackUpService;
using DbBackupEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers
{
    [Authorize]
    public class PermissionController : Controller
    {
        //
        // GET: /Permission/
        PermissionService permissionService = new PermissionService();
        [ClaimsAuthorize(PermissionName.DB_PERMISSIONS_VIEW, PermissionName.CLAIM_Y)]
        public ActionResult Index()
        {
            var userPermission = permissionService.GetPermission();
            return View(userPermission);
        }

        [ClaimsAuthorize(PermissionName.DB_PERMISSIONS_ADD, PermissionName.CLAIM_Y)]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(PermissionName.DB_PERMISSIONS_ADD, PermissionName.CLAIM_Y)]
        public ActionResult Add(Permission model)
        {
            permissionService.AddPermission(model);
            return RedirectToAction("Index");
        }

        [ClaimsAuthorize(PermissionName.DB_PERMISSIONS_EDIT, PermissionName.CLAIM_Y)]
        public ActionResult Edit(int id)
        {
            var userPermission = permissionService.GetPermission(id);
            return View(userPermission);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(PermissionName.DB_PERMISSIONS_EDIT, PermissionName.CLAIM_Y)]
        public ActionResult Edit(int id,Permission model )
        {
            permissionService.UpdatePermission(id,model);
            return RedirectToAction("Index");
        }

        [ClaimsAuthorize(PermissionName.DB_PERMISSIONS_DELETE, PermissionName.CLAIM_Y)]
        public ActionResult Delete(int id)
        {
            var userPermission = permissionService.GetPermission(id);
            return View(userPermission);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(PermissionName.DB_PERMISSIONS_DELETE, PermissionName.CLAIM_Y)]
        public ActionResult Delete(int id,Permission model)
        {
            permissionService.DeletePermission(id);
            return RedirectToAction("Index");
        }

	}
}