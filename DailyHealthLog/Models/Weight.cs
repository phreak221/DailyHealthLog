using SQLite;

namespace DailyHealthLog.Models
{
    [Table("Weight")]
    public class Weight
    {
        [Column("WeightID")]
        [PrimaryKey, Unique, AutoIncrement]
        public int WeightID { get; set; }
        
        [Column("WeightAmount")]
        public int? WeightAmount { get; set; }
    }
}
