using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyHealthLog.Models
{
    public class ReportBloodPressure
    {
        public BloodPressure BloodPressure { get; set; }
        public DateTime LogDateTime { get; set; }
        public string PressureStatus { get; set; }
    }
}
