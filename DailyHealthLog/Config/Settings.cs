using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyHealthLog.Config
{
    public static class Settings
    {
        public readonly static string DBPATH = $"{AppDomain.CurrentDomain.BaseDirectory}healthlogs.sqlite";
        public readonly static string EDBPATH = $"{AppDomain.CurrentDomain.BaseDirectory}healthlogs.aes";
        public static string KEY = "GN2gkXLc86SsvAh9bLgnnw==";
        public static string AUTHKEY = "sdCCPUqbHS5Uebe38i5pNyseohUy57iXZpfD/d/8Ad4d4xprAF01uIKVBdlGaW+LC0dZKk1rqxMNUd7dLccmTQ==";
        public static int WRITEITERATIONS = 20;
    }
}
