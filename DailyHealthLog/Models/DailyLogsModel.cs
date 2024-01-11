using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyHealthLog.Models
{
    public class DailyLogsModel
    {
        public DailyLogs Logs { get; set; }
        public Sleep Sleep { get; set; }
        public Weight Weight { get; set; }
        public List<Liquid> Liquids { get; set; }
        public List<Food> Foods { get; set; }
        public List<BloodPressure> BloodPressures { get; set; }
        public List<Medication> Medications { get; set; }
    }
}
