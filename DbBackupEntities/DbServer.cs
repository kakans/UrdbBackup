using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbBackupEntities
{
    public class DbServer
    {
        public int Id { get; set; }
        public string DbUserName { get; set; }
        public string DbPassword { get; set; }
        public string DbServerName { get; set; }
        public string DbName { get; set; }
    }
}
