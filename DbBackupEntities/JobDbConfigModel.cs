using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbBackupEntities
{
    public class JobDbConfigModel
    {
        public DbJob dbJob { get; set; }
        public DbServer dbServer { get; set;}
        public DbBackupConfig dbBackupConfig { get; set; }
    }
}
