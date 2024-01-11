using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyHealthLog.Models
{
    [Table("WeightDetail")]
    public class WeightDetail
    {
        [Column("WeightDetailID")]
        [PrimaryKey, Unique, AutoIncrement]
        public int WeightDetailID { get; set; }

        [Column("WeightID")]
        public int WeightID { get; set; }

        [Column("WeightGoal")]
        public int WeightGoal { get; set; }

        [Column("BodyFat")]
        public double BodyFat { get; set; }

        [Column("WaterPercent")]
        public double WaterPercent { get; set; }

        [Column("Bmi")]
        public double Bmi { get; set; }

        [Column("Dci")]
        public int Dci { get; set; }

        [Column("WeeksToGoal")]
        public int WeeksToGoal { get; set; }
    }
}
