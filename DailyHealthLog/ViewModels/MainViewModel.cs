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
using DailyHealthLog.Config;

namespace DailyHealthLog.ViewModels
{
    public class MainViewModel : WorkspaceViewModel
    {
        public ICommand AddNewLog { get; set; }
        public ICommand DetailLog { get; set; }
        public ICommand EditLogCommand { get; set; }
        public ICommand DeleteLogCommand { get; set; }

        private ObservableCollection<WorkspaceViewModel> _workspaces;

        private HealthLogsModel _selectHealthLogItem;
        public HealthLogsModel SelectHealthLogItem
        {
            get { return _selectHealthLogItem; }
            set
            {
                if (_selectHealthLogItem != value)
                {
                    _selectHealthLogItem = value;
                    OnPropertyChanged("SelectHealthLogItem");
                }
            }
        }

        private ObservableCollection<HealthLogsModel> _healthLogs;
        public ObservableCollection<HealthLogsModel> HealthLogs
        {
            get { return _healthLogs; }
            set
            {
                if (_healthLogs != value)
                {
                    _healthLogs = value;
                    OnPropertyChanged("HealthLogs");
                }
            }
        }

        public MainViewModel(ObservableCollection<WorkspaceViewModel> workspaces)
        {
            _workspaces = workspaces;
            AddNewLog = new RelayCommand(ShowAddDailyLog);
            DetailLog = new RelayCommand(ShowLogDetails);
            EditLogCommand = new RelayCommand(ShowEditDailyLog);
            DeleteLogCommand = new RelayCommand(RemoveDailyLog);
            BuildHealthLogList();
        }

        private void BuildHealthLogList()
        {
            HealthLogs = new ObservableCollection<HealthLogsModel>();
            SQLiteConnection conn = new SQLiteConnection(Settings.DBPATH, SQLiteOpenFlags.ReadWrite, false);

            IEnumerable<DailyLogs> dailyLogs = conn.Table<DailyLogs>().Reverse();

            foreach (DailyLogs daily in dailyLogs)
            {
                HealthLogsModel healthModel = new HealthLogsModel();
                healthModel.HealthLogID = daily.DailyLogID;
                healthModel.LogDateTime = daily.LogDateTime;
                healthModel.TotalSleepTime = "--:--:--";
                string sleepTime = conn.Table<Sleep>().FirstOrDefault(x => x.SleepID == daily.SleepID)?.TotalSleepTime.ToString();
                if (sleepTime != "00:00:00")
                    healthModel.TotalSleepTime = sleepTime;
                healthModel.WeightAmount = $"{conn.Table<Weight>().FirstOrDefault(x => x.WeightID == daily.WeightID)?.WeightAmount} lbs";
                healthModel.BloodPressure = "--/--";
                healthModel.PulseRate = "-- bpm";
                List<BloodPressure> bps = conn.Table<BloodPressure>().Where(x => x.DailyLogID == daily.DailyLogID).ToList();
                if (bps.Any())
                {
                    healthModel.BloodPressure = $"{bps.LastOrDefault().BPSystolic}/{bps.LastOrDefault().BPDiastolic}";
                    healthModel.PulseRate = $"{bps.LastOrDefault().BPPulseRate} bpm";
                }
                HealthLogs.Add(healthModel);
            }
            conn.Close();
            conn.Dispose();
        }

        private void ShowEditDailyLog(object obj)
        {
            if (SelectHealthLogItem == null) return;

            int logId = SelectHealthLogItem.HealthLogID;
            _workspaces.Clear();
            EditDailyLogViewModel workspace = new EditDailyLogViewModel(_workspaces, logId);
            _workspaces.Add(workspace);
            SetActiveWorkspace(workspace);
        }

        private void ShowLogDetails(object obj)
        {
            if (SelectHealthLogItem == null) return;

            int logId = SelectHealthLogItem.HealthLogID;
            _workspaces.Clear();
            DetailLogViewModel workspace = new DetailLogViewModel(_workspaces, logId);
            _workspaces.Add(workspace);
            SetActiveWorkspace(workspace);
        }

        private void RemoveDailyLog(object obj)
        {

        }
        
        public void ShowAddDailyLog(object obj)
        {
            _workspaces.Clear();
            AddDailyLogViewModel workspace = new AddDailyLogViewModel(_workspaces);
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
