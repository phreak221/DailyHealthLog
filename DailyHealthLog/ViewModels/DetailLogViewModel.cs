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
    public class DetailLogViewModel : WorkspaceViewModel
    {
        public ICommand BtnEdit { get; set; }
        public ICommand BtnCancel { get; set; }

        private ObservableCollection<WorkspaceViewModel> _workspaces;

        private DailyLogsModel _dailyLogsModel;
        public DailyLogsModel DailyLogsModel
        {
            get { return _dailyLogsModel; }
            set
            {
                if (_dailyLogsModel != value)
                {
                    _dailyLogsModel = value;
                    OnPropertyChanged("DailyLogsModel");
                }
            }
        }
        
        private string _logDateTime;
        public string LogDateTime
        {
            get { return _logDateTime; }
            set
            {
                if (_logDateTime != value)
                {
                    _logDateTime = value;
                    OnPropertyChanged("LogDateTime");
                }
            }
        }

        private string _wakeTime;
        public string WakeTime
        {
            get { return _wakeTime; }
            set
            {
                if (_wakeTime != value)
                {
                    _wakeTime = value;
                    OnPropertyChanged("WakeTime");
                }
            }
        }

        private string _sleepTime;
        public string SleepTime
        {
            get { return _sleepTime; }
            set
            {
                if (_sleepTime != value)
                {
                    _sleepTime = value;
                    OnPropertyChanged("SleepTime");
                }
            }
        }

        private string _totalSleepTime;
        public string TotalSleepTime
        {
            get { return _totalSleepTime; }
            set
            {
                if (_totalSleepTime != value)
                {
                    _totalSleepTime = value;
                    OnPropertyChanged("TotalSleepTime");
                }
            }
        }

        private string _weightAmount;
        public string WeightAmount
        {
            get { return _weightAmount; }
            set
            {
                if (_weightAmount != value)
                {
                    _weightAmount = value;
                    OnPropertyChanged("WeightAmount");
                }
            }
        }

        private string _dailyLogNotes;
        public string DailyLogNotes
        {
            get { return _dailyLogNotes; }
            set
            {
                if (_dailyLogNotes != value)
                {
                    _dailyLogNotes = value;
                    OnPropertyChanged("DailyLogNotes");
                }
            }
        }

        private ObservableCollection<object> _tabCollection = new ObservableCollection<object>();
        public ObservableCollection<object> TabCollection
        {
            get { return _tabCollection; }
        }

        private int _logId;
        private DetailLiquidViewModel _liquidVM;
        private DetailFoodViewModel _foodVM;
        private DetailBloodPressureViewModel _bloodpressureVM;
        private DetailMedicationViewModel _medicationVM;

        public DetailLogViewModel(ObservableCollection<WorkspaceViewModel> workspaces, int logId)
        {
            _workspaces = workspaces;
            _logId = logId;
            BtnEdit = new RelayCommand(ShowEditDailyLog);
            BtnCancel = new RelayCommand(BackToMain);
            DailyLogsModel = new DailyLogsModel();
            BuildDetailDailyLogModel(logId);

            _liquidVM = new DetailLiquidViewModel(DailyLogsModel.Liquids);
            TabCollection.Add(_liquidVM);
            _foodVM = new DetailFoodViewModel(DailyLogsModel.Foods);
            TabCollection.Add(_foodVM);
            _bloodpressureVM = new DetailBloodPressureViewModel(DailyLogsModel.BloodPressures);
            TabCollection.Add(_bloodpressureVM);
            _medicationVM = new DetailMedicationViewModel(DailyLogsModel.Medications);
            TabCollection.Add(_medicationVM);
        }

        private void BuildDetailDailyLogModel(int logId)
        {
            DailyLogsModel.Liquids = new List<Liquid>();
            DailyLogsModel.Foods = new List<Food>();
            DailyLogsModel.BloodPressures = new List<BloodPressure>();
            DailyLogsModel.Medications = new List<Medication>();

            using (SQLiteConnection conn = new SQLiteConnection(Settings.DBPATH, SQLiteOpenFlags.ReadWrite))
            {
                DailyLogsModel.Logs = conn.Table<DailyLogs>().FirstOrDefault(x => x.DailyLogID == logId);
                if (DailyLogsModel.Logs == null) return;

                Sleep sleep = conn.Table<Sleep>().FirstOrDefault(x => x.SleepID == DailyLogsModel.Logs.SleepID);
                Weight weight = conn.Table<Weight>().FirstOrDefault(x => x.WeightID == DailyLogsModel.Logs.WeightID);
                DailyLogsModel.Sleep = sleep ?? new Sleep();
                DailyLogsModel.Weight = weight ?? new Weight();

                DailyLogsModel.Liquids = conn.Table<Liquid>().Where(x => x.DailyLogID == DailyLogsModel.Logs.DailyLogID).ToList();
                DailyLogsModel.Foods = conn.Table<Food>().Where(x => x.DailyLogID == DailyLogsModel.Logs.DailyLogID).ToList();
                DailyLogsModel.BloodPressures = conn.Table<BloodPressure>().Where(x => x.DailyLogID == DailyLogsModel.Logs.DailyLogID).ToList();
                DailyLogsModel.Medications = conn.Table<Medication>().Where(x => x.DailyLogID == DailyLogsModel.Logs.DailyLogID).ToList();

                LogDateTime = $"{DailyLogsModel.Logs.LogDateTime}";
                WakeTime = String.Format("{0:g}", sleep?.WakeTime);
                SleepTime = String.Format("{0:g}", sleep?.SleepTime);
                TotalSleepTime = sleep?.TotalSleepTime.ToString();
                WeightAmount = $"{weight.WeightAmount}";
                DailyLogNotes = DailyLogsModel.Logs.DailyLogNotes;

                conn.Close();
                conn.Dispose();
            }
        }

        private void ShowEditDailyLog(object obj)
        {
            _workspaces.Clear();
            EditDailyLogViewModel workspace = new EditDailyLogViewModel(_workspaces, _logId);
            _workspaces.Add(workspace);
            SetActiveWorkspace(workspace);
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
