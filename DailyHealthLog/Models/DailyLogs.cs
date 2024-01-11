using SQLite;
using System;
using System.Collections.Generic;

namespace DailyHealthLog.Models
{
    [Table("DailyLogs")]
    public class DailyLogs
    {
        [Column("DailyLogID")]
        [PrimaryKey, AutoIncrement, Unique]
        public int DailyLogID { get; set; }

        [Column("LogDateTime")]
        public DateTime LogDateTime { get; set; }

        [Column("DailyLogNotes")]
        public string DailyLogNotes { get; set; }

        [Column("SleepID")]
        public int SleepID { get; set; }

        [Column("WeightID")]
        public int WeightID { get; set; }
    }
}
