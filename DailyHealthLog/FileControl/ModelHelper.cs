using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyHealthLog.FileControl
{
    public static class ModelHelper
    {
        public static Dictionary<string, Type> GetDailyLogsProperties()
        {
            return new Dictionary<string, Type>()
            {
                { "DailyLogID", typeof(int) },
                { "LogDateTime", typeof(DateTime) },
                { "DailyLogNotes", typeof(string) }
            };
        }

        public static Dictionary<string, Type> GetBloodPressureProperties()
        {
            return new Dictionary<string, Type>()
            {
                { "LogDateTime", typeof(DateTime) },
                { "BPTime", typeof(string) },
                { "BPSystolic", typeof(int) },
                { "BPDiastolic", typeof(int) },
                { "BPPulseRate", typeof(int) }
            };
        }

        public static Dictionary<string, Type> GetSleepProperties()
        {
            return new Dictionary<string, Type>()
            {
                { "WakeTime", typeof(DateTime) },
                { "SleepTime", typeof(DateTime) },
                { "TotalSleepTime", typeof(TimeSpan) }
            };
        }

        public static Dictionary<string, Type> GetLiquidProperties()
        {
            return new Dictionary<string, Type>()
            {
                { "DrinkItem", typeof(string) },
                { "DrinkAmount", typeof(int) }
            };
        }

        public static Dictionary<string, Type> GetMedicationProperties()
        {
            return new Dictionary<string, Type>()
            {
                { "MedicationTime", typeof(string) },
                { "MedicationName", typeof(string) },
                { "MedicationDose", typeof(string) }
            };
        }
    }
}
