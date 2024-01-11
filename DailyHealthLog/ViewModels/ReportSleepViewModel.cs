using DailyHealthLog.Config;
using DailyHealthLog.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyHealthLog.ViewModels
{
    public class ReportSleepViewModel : WorkspaceViewModel
    {
        private ObservableCollection<Sleep> _sleepCollection;
        public ObservableCollection<Sleep> SleepCollection
        {
            get { return _sleepCollection; }
            set
            {
                if (_sleepCollection != value)
                {
                    _sleepCollection = value;
                    OnPropertyChanged("SleepCollection");
                }
            }
        }

        public ReportSleepViewModel(List<Sleep> sleeplist)
        {
            sleeplist = BuildReportSleepList();
            SleepCollection = new ObservableCollection<Sleep>(sleeplist);
        }

        private List<Sleep> BuildReportSleepList()
        {
            using (SQLiteConnection conn = new SQLiteConnection(Settings.DBPATH, SQLiteOpenFlags.ReadWrite))
            {
                List<Sleep> sleeplist = conn.Table<Sleep>().Reverse().ToList();
                TimeSpan day = new TimeSpan(24, 0, 0);
                sleeplist.ForEach(x =>
                {
                    x.TotalSleepTime = (day - x.TotalSleepTime);
                });
                return sleeplist;
            }
        }
    }
}
