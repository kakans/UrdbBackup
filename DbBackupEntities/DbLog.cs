using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbBackupEntities
{
    public class DbLog
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public string JobName { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
