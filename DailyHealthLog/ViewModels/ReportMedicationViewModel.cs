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
    public class ReportMedicationViewModel : WorkspaceViewModel
    {
        private ObservableCollection<ReportMedication> _medCollection;
        public ObservableCollection<ReportMedication> MedCollection
        {
            get { return _medCollection; }
            set
            {
                if (_medCollection != value)
                {
                    _medCollection = value;
                    OnPropertyChanged("MedCollection");
                }
            }
        }

        public ReportMedicationViewModel(List<ReportMedication> medlist)
        {
            medlist = BuildReportMedicationList();
            MedCollection = new ObservableCollection<ReportMedication>(medlist);
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
    }
}
