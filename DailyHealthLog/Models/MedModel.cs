using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyHealthLog.Models
{
    public class MedModel
    {
        public int MedId { get; set; }
        public string MedName { get; set; }

        public override string ToString()
        {
            return MedName;
        }
    }
}
