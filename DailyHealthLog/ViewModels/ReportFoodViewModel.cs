using DailyHealthLog.Config;
using DailyHealthLog.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DailyHealthLog.ViewModels
{
    public class ReportFoodViewModel : WorkspaceViewModel
    {
        private ObservableCollection<ReportFood> _foodCollection;
        public ObservableCollection<ReportFood> FoodCollection
        {
            get { return _foodCollection; }
            set
            {
                if (_foodCollection != value)
                {
                    _foodCollection = value;
                    OnPropertyChanged("FoodCollection");
                }
            }
        }

        public ReportFoodViewModel(List<ReportFood> foodCol)
        {
            foodCol = BuildReportFoodList();
            FoodCollection = new ObservableCollection<ReportFood>(foodCol);
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
    }
}
