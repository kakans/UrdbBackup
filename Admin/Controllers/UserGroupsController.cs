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
    public class UserGroupsController : Controller
    {
        //
        // GET: /UserGroups/
        GroupService groupService = new GroupService();
        PermissionService permissionService = new PermissionService();
        [ClaimsAuthorize(PermissionName.DB_USER_GROUP_VIEW, PermissionName.CLAIM_Y)]
        public ActionResult Index()
        {
            var userGroups = groupService.GetGroups();
            return View(userGroups);
        }

        [ClaimsAuthorize(PermissionName.DB_USER_GROUP_EDIT, PermissionName.CLAIM_Y)]
        public ActionResult Edit(int id)
        {
            var userGroups = groupService.GetGroup(id);
            return View(userGroups);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(PermissionName.DB_USER_GROUP_EDIT, PermissionName.CLAIM_Y)]
        public ActionResult Edit(int id, UserGroup model)
        {
            groupService.UpdateGroup(id,model);
            return RedirectToAction("Index");
        }

        [ClaimsAuthorize(PermissionName.DB_USER_GROUP_ADD, PermissionName.CLAIM_Y)]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(PermissionName.DB_USER_GROUP_ADD, PermissionName.CLAIM_Y)]
        public ActionResult Add(UserGroup model)
        {
            groupService.AddGroup(model);
            return RedirectToAction("Index");
        }

        [ClaimsAuthorize(PermissionName.DB_USER_GROUP_DELETE, PermissionName.CLAIM_Y)]
        public ActionResult Delete(int id)
        {
            var userGroups = groupService.GetGroup(id);
            return View(userGroups);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(PermissionName.DB_USER_GROUP_DELETE, PermissionName.CLAIM_Y)]
        public ActionResult Delete(int id,UserGroup model)
        {
            groupService.DeleteGroup(id);
            return RedirectToAction("Index");
        }

        [ClaimsAuthorize(PermissionName.DB_USER_GROUP_ASSIGNPERMISSION, PermissionName.CLAIM_Y)]
        public ActionResult Permission(int id)
        {
            var userGroups = groupService.GetGroup(id);
            ViewBag.GroupName = userGroups.GroupName;
            permissionService.SyncGroupPermission();
            var userPermission = permissionService.GetPermissionByGroup(id);
            
            List<CheckBoxModel> ChkItem = new List<CheckBoxModel>();
            foreach (var item in userPermission)
            {
                ChkItem.Add(new CheckBoxModel
                {
                    Value = item.PermissionId,
                    Text = item.PermissionDisplayName,
                    IsChecked = (item.HasAccess == "Y") ? true : false
                });
            }
            return View(ChkItem);    
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(PermissionName.DB_USER_GROUP_ASSIGNPERMISSION, PermissionName.CLAIM_Y)]
        public ActionResult Permission(int id, List<CheckBoxModel> list)
        {
            permissionService.UpdatePermissionByGroup(id, list);
            return RedirectToAction("Index");
        }

	}
}