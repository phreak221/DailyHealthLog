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
    public class ReportBloodPressureViewModel : WorkspaceViewModel
    {
        private ObservableCollection<ReportBloodPressure> _bloodpressureCollection;
        public ObservableCollection<ReportBloodPressure> BloodPressureCollection
        {
            get { return _bloodpressureCollection; }
            set
            {
                if (_bloodpressureCollection != value)
                {
                    _bloodpressureCollection = value;
                    OnPropertyChanged("BloodPressureCollection");
                }
            }
        }

        public ReportBloodPressureViewModel(List<ReportBloodPressure> bplist)
        {
            bplist = BuildReportBloodPressureList();
            BloodPressureCollection = new ObservableCollection<ReportBloodPressure>(bplist);
        }

        private List<ReportBloodPressure> BuildReportBloodPressureList()
        {
            using (SQLiteConnection conn = new SQLiteConnection(Settings.DBPATH, SQLiteOpenFlags.ReadWrite))
            {
                List<BloodPressure> bplist = conn.Table<BloodPressure>().ToList();
                List<DailyLogs> dailylist = conn.Table<DailyLogs>().ToList();

                List<ReportBloodPressure> reportBpList = (from bp in bplist
                        join dl in dailylist on bp.DailyLogID equals dl.DailyLogID
                        select new ReportBloodPressure
                        {
                            BloodPressure = bp,
                            LogDateTime = dl.LogDateTime
                        }).Reverse().ToList();

                foreach (ReportBloodPressure rbp in reportBpList)
                {
                    int systolic = rbp.BloodPressure.BPSystolic;
                    int diastolic = rbp.BloodPressure.BPDiastolic;
                    if (systolic < 140 && diastolic < 90)
                        rbp.PressureStatus = "Goal";
                    if (systolic > 140 && diastolic < 90)
                        rbp.PressureStatus = "Systolic High";
                    if (systolic < 140 && diastolic > 90)
                        rbp.PressureStatus = "Diastolic High";
                    if (systolic < 90 && diastolic < 60)
                        rbp.PressureStatus = "Low Crisis";
                    if (systolic > 140 && diastolic > 90)
                        rbp.PressureStatus = "High Crisis";

                    //if (systolic < 120 && diastolic < 80)
                    //{
                    //    rbp.PressureStatus = "Normal";
                    //}
                    //else if ((systolic >= 120 && systolic <= 129) && diastolic < 80)
                    //{
                    //    rbp.PressureStatus = "Elevated";
                    //}
                    //else if ((systolic >= 130 && systolic <= 139) || (diastolic >= 80 && diastolic <= 89))
                    //{
                    //    rbp.PressureStatus = "Stage1";
                    //}
                    //else if ((systolic >= 140) || (diastolic >= 90))
                    //{
                    //    rbp.PressureStatus = "Stage2";
                    //}
                    //else if (((systolic > 180) && (diastolic > 120)) || (systolic > 180) || (diastolic > 120))
                    //{
                    //    rbp.PressureStatus = "Crisis";
                    //}
                }
                return reportBpList;
            }

            //using (SQLiteConnection conn = new SQLiteConnection(Settings.DBPATH, SQLiteOpenFlags.ReadWrite))
            //{
            //    List<BloodPressure> bplist = conn.Table<BloodPressure>().ToList();
            //    List<DailyLogs> dailylist = conn.Table<DailyLogs>().ToList();

            //    List<ReportBloodPressure> reportBpList = (from bp in bplist
            //            join dl in dailylist on bp.DailyLogID equals dl.DailyLogID
            //            select new ReportBloodPressure
            //            {
            //                BloodPressure = bp,
            //                LogDateTime = dl.LogDateTime
            //            }).Reverse().ToList();

            //    //foreach (ReportBloodPressure rbp in reportBpList)
            //    //{
            //    //    int systolic = rbp.BloodPressure.BPSystolic;
            //    //    int diastolic = rbp.BloodPressure.BPDiastolic;

            //    //    if (systolic < 120 && diastolic < 80)
            //    //    {
            //    //        rbp.PressureStatus = "Normal";
            //    //        rbp.BloodPressure.BPStatus = "Normal";
            //    //    }
            //    //    else if ((systolic >= 120 && systolic <= 129) && diastolic < 80)
            //    //    {
            //    //        rbp.PressureStatus = "Elevated";
            //    //        rbp.BloodPressure.BPStatus = "Elevated";
            //    //    }
            //    //    else if ((systolic >= 130 && systolic <= 139) || (diastolic >= 80 && diastolic <= 89))
            //    //    {
            //    //        rbp.PressureStatus = "Stage1";
            //    //        rbp.BloodPressure.BPStatus = "Stage1";
            //    //    }
            //    //    else if ((systolic >= 140) || (diastolic >= 90))
            //    //    {
            //    //        rbp.PressureStatus = "Stage2";
            //    //        rbp.BloodPressure.BPStatus = "Stage2";
            //    //    }
            //    //    else if (((systolic > 180) && (diastolic > 120)) || (systolic > 180) || (diastolic > 120))
            //    //    {
            //    //        rbp.PressureStatus = "Crisis";
            //    //        rbp.BloodPressure.BPStatus = "Crisis";
            //    //    }
            //    //    conn.Update(rbp.BloodPressure);
            //    //}
            //    return reportBpList;
            //}
        }
    }
}
