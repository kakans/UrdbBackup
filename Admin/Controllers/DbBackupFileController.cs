using Admin.Models;
using DatabaseBackUpService;
using DbBackupEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace Admin.Controllers
{
    [Authorize]
    public class DbBackupFileController : Controller
    {
        string backupPath = ConfigurationSettings.GetApplicationSetting("BackUpPath");
        string emailTo = ConfigurationSettings.GetApplicationSetting("Notification_Email_TO");
        string emailCC = ConfigurationSettings.GetApplicationSetting("Notification_Email_CC");
        string emailBcc = ConfigurationSettings.GetApplicationSetting("Notification_Email_BCC");

        //
        // GET: /DbBackupFile/
        [ClaimsAuthorize(PermissionName.DB_BACKUP_VIEW, PermissionName.CLAIM_Y)]
        public ActionResult Index()
        {
            BackupFileService fileService = new BackupFileService();
            List<Folder> folders = new List<Folder>();
            folders = fileService.getBackupFolders(backupPath, true);
            return View(folders);
        }

        public ActionResult FolderList(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                BackupFileService fileService = new BackupFileService();
                var folders = fileService.getBackupFolders(string.Format("{0}\\{1}", backupPath, id), false);
                return View(folders);
            }
            return View();
        }

        [ClaimsAuthorize(PermissionName.DB_BACKUP_DELETE, PermissionName.CLAIM_Y)]
        public ActionResult DeleteBackup(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                BackupFileService fileService = new BackupFileService();
                try
                {
                    Directory.Delete(string.Format("{0}\\{1}", backupPath, id), true);
                    EmailSender email = new EmailSender();
                    email.sendEmail(emailTo, emailCC, emailBcc, false, "Database Backup Deleted", "Database Backup Deleted for "+ string.Format("{0}\\{1}", backupPath, id));
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return RedirectToAction("Index");
        }

        [ClaimsAuthorize(PermissionName.DB_BACKUP_DETAILS, PermissionName.CLAIM_Y)]
        public ActionResult Details(string id,string name)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                BackupFileService fileService = new BackupFileService();

                var backupFiles = fileService.GetBackupFile(string.Format("{0}\\{1}\\{2}", backupPath, name, id));

                //backupFiles = fileService.GetBackupFile(string.Format("{0}\\{1}", backupPath, name));
                return View(backupFiles);
            }
            return View();
        }

        [ClaimsAuthorize(PermissionName.DB_BACKUP_DOWNLOAD, PermissionName.CLAIM_Y)]
        public ActionResult DownloadFile(string path)
        {
            //string filePath = string.Format("{0}\\{1}", backupPath, path);
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            string fileName = path.Split('\\').Last();
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
	}
}