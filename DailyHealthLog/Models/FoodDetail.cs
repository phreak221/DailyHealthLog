using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyHealthLog.Models
{
    [Table("FoodDetail")]
    public class FoodDetail
    {
        [Column("FoodDetailID")]
        [PrimaryKey, Unique, AutoIncrement]
        public int FoodDetailID { get; set; }

        [Column("ItemCalories")]
        public int ItemCalories { get; set; }

        [Column("TimeAte")]
        public DateTime TimeAte { get; set; }
    }
}
