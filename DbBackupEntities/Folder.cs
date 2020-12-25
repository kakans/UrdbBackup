using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbBackupEntities
{
    public class Folder
    {
        public string Name { get; set; }
        public DirectoryInfo DirInfo { get; set; }
        public string Size { get; set; }

    }
}
