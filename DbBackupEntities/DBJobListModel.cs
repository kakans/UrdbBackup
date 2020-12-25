using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbBackupEntities
{
    public class DBJobListModel
    {
        public int JobId { get; set; }
        public string JobTitle { get; set; }
        public string DbName { get; set; }
        public string RunIn { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
    }
}