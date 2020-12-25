using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DbBackupEntities
{
    public class DbJob
    {
        public int JobId { get; set; }

        [Required]
        [Display(Name ="Job Title")]
        public string JobTitle { get; set; }
        public int DbConfigId { get; set; }
        [Required]
        [Display(Name = "Frequency In Hrs")]
        public int FrequencyInHrs { get; set; }
        public int DbServerId { get; set; }
        public string DbTableList { get; set; }
        public int IsActive { get; set; }
        public DateTime LastRun { get; set; }
        public DateTime NextRun { get; set; }
    }
}
