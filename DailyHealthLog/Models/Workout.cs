using SQLite;

namespace DailyHealthLog.Models
{
    [Table("Workout")]
    public class Workout
    {
        [Column("WorkoutID")]
        [PrimaryKey, Unique, AutoIncrement]
        public int WorkoutID { get; set; }

        [Column("WorkoutNote")]
        public string WorkoutNote { get; set; }

        [Column("DailyLogID")]
        public int DailyLogID { get; set; }
    }
}
