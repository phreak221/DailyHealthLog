using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyHealthLog.Models
{
    [Table("BloodPressures")]
    public class BloodPressure
    {
        [Column("BPID")]
        [PrimaryKey, Unique, AutoIncrement]
        public int BPID { get; set; }

        [Column("BPTime")]
        public string BPTime { get; set; }

        [Column("BPSystolic")]
        public int BPSystolic { get; set; }

        [Column("BPDiastolic")]
        public int BPDiastolic { get; set; }

        [Column("BPPulseRate")]
        public int BPPulseRate { get; set; }

        [Column("DailyLogID")]
        [NotNull]
        public int DailyLogID { get; set; }

        [Column("BPStatus")]
        public string BPStatus { get; set; }
    }
}
