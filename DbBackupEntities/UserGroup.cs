using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbBackupEntities
{
    public class UserGroup
    {
        public int GroupId { get; set; }

        [Display(Name = "Group Name")]
        public string GroupName { get; set; }

        public bool Active { get; set; }
    }
}
