using SQLite;
using System;

namespace DailyHealthLog.Models
{
    [Table("Sleep")]
    public class Sleep
    {
        [Column("SleepID")]
        [PrimaryKey, Unique, AutoIncrement]
        public int SleepID { get; set; }

        [Column("WakeTime")]
        public DateTime WakeTime { get; set; }

        [Column("SleepTime")]
        public DateTime SleepTime { get; set; }

        [Column("TotalSleepTime")]
        public TimeSpan TotalSleepTime { get; set; }
    }
}
