using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyHealthLog.Models
{
    public class ReportMedication
    {
        public Medication Medication { get; set; }
        public DateTime LogDateTime { get; set; }
    }
}
