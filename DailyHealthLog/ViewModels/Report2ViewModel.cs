using DailyHealthLog.Config;
using DailyHealthLog.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace DailyHealthLog.ViewModels
{
    public class Report2ViewModel : WorkspaceViewModel
    {
        public ICommand BtnDetails { get; set; }
        public ICommand BtnCancel { get; set; }

        private ObservableCollection<WorkspaceViewModel> _workspaces;

        private ObservableCollection<object> _tabCollection = new ObservableCollection<object>();
        public ObservableCollection<object> TabCollection
        {
            get { return _tabCollection; }
        }

        public string Setting { get; private set; }

        public Report2ViewModel(ObservableCollection<WorkspaceViewModel> workspaces)
        {
            _workspaces = workspaces;
            BtnDetails = new RelayCommand(ShowLogDetails);
            BtnCancel = new RelayCommand(BackToMain);
            //List<Sleep> sleeplist = BuildReportSleepList();
            List<Sleep> sleeplist = new List<Sleep>();
            TabCollection.Add(new ReportSleepViewModel(sleeplist));
            //List<ReportLiquid> liquidlist = BuildReportDrinkList();
            List<ReportLiquid> liquidlist = new List<ReportLiquid>();
            TabCollection.Add(new ReportLiquidViewModel(liquidlist));
            //List<ReportFood> reportfoodlist = BuildReportFoodList();
            List<ReportFood> reportfoodlist = new List<ReportFood>();
            TabCollection.Add(new ReportFoodViewModel(reportfoodlist));
            //List<ReportBloodPressure> reportbplist = BuildReportBloodPressureList();
            List<ReportBloodPressure> reportbplist = new List<ReportBloodPressure>();
            TabCollection.Add(new ReportBloodPressureViewModel(reportbplist));
            //List<ReportMedication> reportmedlist = BuildReportMedicationList();
            List<ReportMedication> reportmedlist = new List<ReportMedication>();
            TabCollection.Add(new ReportMedicationViewModel(reportmedlist));
        }

        private List<ReportFood> BuildReportFoodList()
        {
            using (SQLiteConnection conn = new SQLiteConnection(Settings.DBPATH, SQLiteOpenFlags.ReadWrite))
            {
                List<Food> foodlist = conn.Table<Food>().ToList();
                List<DailyLogs> dailylist = conn.Table<DailyLogs>().ToList();

                return (from f in foodlist
                        join dl in dailylist on f.DailyLogID equals dl.DailyLogID
                        select new ReportFood
                        {
                            Foods = f,
                            LogDateTime = dl.LogDateTime
                        }).Reverse().ToList();
            }
        }

        private List<Sleep> BuildReportSleepList()
        {
            using (SQLiteConnection conn = new SQLiteConnection(Settings.DBPATH, SQLiteOpenFlags.ReadWrite))
            {
                List<Sleep> sleeplist = conn.Table<Sleep>().ToList();
                TimeSpan day = new TimeSpan(24, 0, 0);
                sleeplist.ForEach(x =>
                {
                    x.TotalSleepTime = (day - x.TotalSleepTime);
                });
                return sleeplist;
            }
        }

        private List<ReportLiquid> BuildReportDrinkList()
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

        private List<ReportBloodPressure> BuildReportBloodPressureList()
        {
            using (SQLiteConnection conn = new SQLiteConnection(Settings.DBPATH, SQLiteOpenFlags.ReadWrite))
            {
                List<BloodPressure> bplist = conn.Table<BloodPressure>().ToList();
                List<DailyLogs> dailylist = conn.Table<DailyLogs>().ToList();

                return (from bp in bplist
                        join dl in dailylist on bp.DailyLogID equals dl.DailyLogID
                        select new ReportBloodPressure
                        {
                            BloodPressure = bp,
                            LogDateTime = dl.LogDateTime
                        }).Reverse().ToList();
            }
        }

        private List<ReportMedication> BuildReportMedicationList()
        {
            using (SQLiteConnection conn = new SQLiteConnection(Settings.DBPATH, SQLiteOpenFlags.ReadWrite))
            {
                List<Medication> medlist = conn.Table<Medication>().ToList();
                List<DailyLogs> dailylist = conn.Table<DailyLogs>().ToList();

                return (from m in medlist
                        join dl in dailylist on m.DailyLogID equals dl.DailyLogID
                        select new ReportMedication
                        {
                            Medication = m,
                            LogDateTime = dl.LogDateTime
                        }).Reverse().ToList();
            }
        }

        private void ShowLogDetails(object obj)
        {

        }

        private void BackToMain(object obj)
        {
            _workspaces.Clear();
            MainViewModel workspace = new MainViewModel(_workspaces);
            _workspaces.Add(workspace);
            SetActiveWorkspace(workspace);
        }

        public void SetActiveWorkspace(WorkspaceViewModel workspace)
        {
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(_workspaces);
            collectionView?.MoveCurrentTo(workspace);
        }
    }
}
