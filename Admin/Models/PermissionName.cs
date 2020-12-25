using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models
{
    sealed class PermissionName
    {
        public const string CLAIM_Y = "Y";
        public const string DB_JOB_VIEW = "DB_JOB_VIEW";
        public const string DB_JOB_CREATE = "DB_JOB_CREATE";
        public const string DB_JOB_RUNNOW = "DB_JOB_RUNNOW";
        public const string DB_JOB_EDIT = "DB_JOB_EDIT";
        public const string DB_JOB_DELETE = "DB_JOB_DELETE";

        public const string DB_LOGS_VIEW = "DB_LOGS_VIEW";
        public const string DB_LOGS_DETAILS = "DB_LOGS_DETAILS";

        public const string DB_BACKUP_VIEW = "DB_BACKUP_VIEW";
        public const string DB_BACKUP_DETAILS = "DB_BACKUP_DETAILS";
        public const string DB_BACKUP_DELETE = "DB_BACKUP_DELETE";
        public const string DB_BACKUP_DOWNLOAD = "DB_BACKUP_DOWNLOAD";

        public const string DB_USER_VIEW = "DB_USER_VIEW";
        public const string DB_USER_ADD = "DB_USER_ADD";
        public const string DB_USER_EDIT = "DB_USER_EDIT";

        public const string DB_USER_GROUP_VIEW = "DB_USER_GROUP_VIEW";
        public const string DB_USER_GROUP_ADD = "DB_USER_GROUP_ADD";
        public const string DB_USER_GROUP_EDIT = "DB_USER_GROUP_EDIT";
        public const string DB_USER_GROUP_DELETE = "DB_USER_GROUP_DELETE";
        public const string DB_USER_GROUP_ASSIGNPERMISSION = "DB_USER_GROUP_ASSIGNPERMISSION";

        public const string DB_PERMISSIONS_VIEW = "DB_PERMISSIONS_VIEW";
        public const string DB_PERMISSIONS_ADD = "DB_PERMISSIONS_ADD";
        public const string DB_PERMISSIONS_EDIT = "DB_PERMISSIONS_EDIT";
        public const string DB_PERMISSIONS_DELETE = "DB_PERMISSIONS_DELETE";
    }
}