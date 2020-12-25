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
    public class ManageUsersController : Controller
    {
        //
        // GET: /ManageUser/
        [ClaimsAuthorize(PermissionName.DB_USER_VIEW, PermissionName.CLAIM_Y)]
        public ActionResult Index()
        {
            UserService userService = new UserService();
            var userPermission = userService.GetAllUsers();
            return View(userPermission);
        }
        
        [ClaimsAuthorize(PermissionName.DB_USER_ADD, PermissionName.CLAIM_Y)]
        public ActionResult AddUser()
        {
            UserEntity user = new UserEntity();
            List<ListItem> lst = new List<ListItem>();
            UserService userService = new UserService();
            var lstGroups = userService.GetUserGroups();
            user.GroupItems = lstGroups;
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(PermissionName.DB_USER_ADD, PermissionName.CLAIM_Y)]
        public ActionResult AddUser(UserEntity model)
        {
            UserService userService = new UserService();
            userService.AddUser(model);
            return RedirectToAction("Index");
        }

        [ClaimsAuthorize(PermissionName.DB_USER_EDIT, PermissionName.CLAIM_Y)]
        public ActionResult Edit(int id)
        {
            UserEntity model = new UserEntity();
            UserService userService = new UserService();
            model = userService.GetUser(id);
            var lstGroups = userService.GetUserGroups();
            model.GroupItems = lstGroups;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(PermissionName.DB_USER_EDIT, PermissionName.CLAIM_Y)]
        public ActionResult Edit(int id, UserEntity model)
        {
            UserService userService = new UserService();
            userService.UpdateUser(id, model);
            return RedirectToAction("Index");
        }
    }
}