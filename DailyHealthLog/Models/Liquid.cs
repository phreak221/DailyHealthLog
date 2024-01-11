
using SQLite;

namespace DailyHealthLog.Models
{
    [Table("Liquids")]
    public class Liquid
    {
        [Column("LiquidID")]
        [PrimaryKey, Unique, AutoIncrement]
        public int LiquidID { get; set; }

        [Column("DrinkItem")]
        public string DrinkItem { get; set; }

        [Column("DrinkAmount")]
        public int DrinkAmount { get; set; }

        [Column("DailyLogID")]
        public int DailyLogID { get; set; }
    }
}
