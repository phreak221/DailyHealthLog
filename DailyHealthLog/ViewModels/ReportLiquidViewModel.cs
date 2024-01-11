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
    public class ReportLiquidViewModel : WorkspaceViewModel
    {
        private ObservableCollection<ReportLiquid> _liquidCollection;
        public ObservableCollection<ReportLiquid> LiquidCollection
        {
            get { return _liquidCollection; }
            set
            {
                if (_liquidCollection != value)
                {
                    _liquidCollection = value;
                    OnPropertyChanged("LiquidCollection");
                }
            }
        }

        public ReportLiquidViewModel(List<ReportLiquid> drinklist)
        {
            drinklist = BuildReportLiquidList();
            LiquidCollection = new ObservableCollection<ReportLiquid>(drinklist);
        }

        private List<ReportLiquid> BuildReportLiquidList()
        {
            using (SQLiteConnection conn = new SQLiteConnection(Settings.DBPATH, SQLiteOpenFlags.ReadWrite))
            {
                List<Liquid> liquidlist = conn.Table<Liquid>().ToList();
                List<DailyLogs> dailylist = conn.Table<DailyLogs>().ToList();

                return (from l in liquidlist
                        join dl in dailylist on l.DailyLogID equals dl.DailyLogID
                        select new ReportLiquid
                        {
                            Liquid = l,
                            LogDateTime = dl.LogDateTime
                        }).Reverse().ToList();
            }
        }
    }
}
