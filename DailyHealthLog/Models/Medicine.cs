using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyHealthLog.Models
{
    [Table("Medicine")]
    public class Medicine
    {
        [Column("MedicineID")]
        [PrimaryKey, Unique, AutoIncrement]
        public int MedicineID { get; set; }

        [Column("MedicineName")]
        public string MedicineName { get; set; }

        [Column("MedicineDose")]
        public string MedicineDose { get; set; }
    }
}
