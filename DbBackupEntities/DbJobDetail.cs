using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbBackupEntities
{
    public class DbJobDetail
    {
        public int JobId { get; set; }

        [Required]
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        [Required]
        [Display(Name = "Frequency In Hrs")]
        public int FrequencyInHrs { get; set; }

        public int BackupSince { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy hh:mm:ss tt}")]
        public DateTime LastRun { get; set; }
         
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy hh:mm:ss tt}")]
        public DateTime NextRun { get; set; }

        [Display(Name = "Table List")]
        public string DbTableList { get; set; }

        [Required]
        [Display(Name = "Dir For Backup")]
        public string DirForBackup { get; set; }

        [Required]
        [Display(Name = "Backup Method")]
        public string BackupMethod { get; set; }

        [Required]
        [Display(Name = "Insert Method")]
        public string InsertMethod { get; set; }

        [Display(Name = "Save Structure")]
        public bool SaveStructure { get; set; }

        [Display(Name = "Compress To Zip")]
        public bool CompressToZip { get; set; }

        [Display(Name = "Delete Old Backups")]
        public bool DeleteOldBackups { get; set; }

        [Display(Name = "Keep X Backups")]
        public int KeepXBackups { get; set; }

        [Required]
        [Display(Name = "Database User Name")]
        public string DbUserName { get; set; }

        [Required]
        [Display(Name = "Database password")]
        public string DbPassword { get; set; }

        [Required]
        [Display(Name = "Database Server Name")]
        public string DbServerName { get; set; }

        [Required]
        [Display(Name = "Database Name")]
        public string DbName { get; set; }
        public bool IsActive { get; set; }

        [Display(Name="Full Database Backup")]
        public bool FullDbBackup { get; set; }
        public List<TableList> TableList { get; set; }

        public string JobStatus { get; set; }
    }
    public class TableList
    {
        public bool IsSelected { get; set; }
        public string TableName { get; set; }
    }
}
