using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DailyHealthLog.Models
{
    public class HealthLogsModel
    {
        public int HealthLogID { get; set; }
        public DateTime LogDateTime { get; set; }
        public string TotalSleepTime { get; set; }
        public string WeightAmount { get; set; }
        public string BloodPressure { get; set; }
        public string PulseRate { get; set; }
        public string DidWorkout { get; set; }
    }
}
