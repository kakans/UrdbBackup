
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbBackupEntities
{
    public class DbBackupConfig
    {
        public int Id { get; set; }
        public string DirForBackup { get; set; }
        public bool SaveInSeparateFiles { get; set; }
        public bool SaveInOneFile { get; set; }
        public bool InsertIgnore { get; set; }
        public bool ReplaceInsert { get; set; }
        public bool SaveStructure { get; set; }
        public bool CompressToZip { get; set; }
        public bool DeleteOldBackups { get; set; }
        public int KeepXBackups { get; set; }
    }
}
