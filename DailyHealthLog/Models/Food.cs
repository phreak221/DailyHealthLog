

using SQLite;

namespace DailyHealthLog.Models
{
    [Table("Foods")]
    public class Food
    {
        [Column("FoodID")]
        [PrimaryKey, Unique, AutoIncrement]
        public int FoodID { get; set; }
        
        [Column("FoodItem")]
        public string FoodItem { get; set; }

        [Column("DailyLogID")]
        public int DailyLogID { get; set; }
    }
}
