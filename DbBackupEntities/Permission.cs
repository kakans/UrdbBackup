using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbBackupEntities
{
    public class Permission
    {
        public int PermissionId { get; set; }

        [Required]
        [Display(Name = " Permission Name")]
        public string PermissionName { get; set; }

        [Required]
        [Display(Name = " Permission Display Name")]
        public string PermissionDisplayName { get; set; }

        public string HasAccess { get; set; }
    }
}
