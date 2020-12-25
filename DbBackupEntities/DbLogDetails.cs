using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbBackupEntities
{
    public class DbLogDetails
    {
        public int Id { get; set; }
        public int LogId { get; set; }
        public LogLevel LogLevel { get; set; }
        public DateTime LogTime { get; set; }
        public string Message { get; set; }
    }
}
