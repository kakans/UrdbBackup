using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbBackupEntities
{
    public class BackupFile
    {
        public string FileName { get; set; }
        public string CreatedOn { get; set; }
        public string Size { get; set; }
        public DirectoryInfo DirInfo { get; set; }

    }
}
