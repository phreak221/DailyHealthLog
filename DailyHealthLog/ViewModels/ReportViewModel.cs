using DailyHealthLog.Config;
using DailyHealthLog.FileControl;
using DailyHealthLog.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace DailyHealthLog.ViewModels
{
    public class ReportViewModel : WorkspaceViewModel
    {
        public ICommand BtnSubmit { get; set; }
        public ICommand BtnCancel { get; set; }

        ObservableCollection<WorkspaceViewModel> _workspaces;
        private Dictionary<string, Type> _classProperties;

        private ObservableCollection<object> _logObj;
        public ObservableCollection<object> LogObj
        {
            get { return _logObj; }
            set
            {
                if (_logObj != value)
                {
                    _logObj = value;
                    OnPropertyChanged("LogObj");
                }
            }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    OnPropertyChanged("Message");
                }
            }
        }

        private List<string> _startDateList;
        public List<string> StartDateList
        {
            get { return _startDateList; }
            set
            {
                if (_startDateList != value)
                {
                    _startDateList = value;
                    OnPropertyChanged("StartDateList");
                }
            }
        }

        private string _selectedStartDateItem;
        public string SelectedStartDateItem
        {
            get { return _selectedStartDateItem; }
            set
            {
                if (_selectedStartDateItem != value)
                {
                    _selectedStartDateItem = value;
                    OnPropertyChanged("SelectedStartDateItem");
                }
            }
        }
        
        private List<string> _endDateList;
        public List<string> EndDateList
        {
            get { return _endDateList; }
            set
            {
                if (_endDateList != value)
                {
                    _endDateList = value;
                    OnPropertyChanged("EndDateList");
                }
            }
        }

        private string _selectedEndDateItem;
        public string SelectedEndDateItem
        {
            get { return _selectedEndDateItem; }
            set
            {
                if (_selectedEndDateItem != value)
                {
                    _selectedEndDateItem = value;
                    OnPropertyChanged("SelectedEndDateItem");
                }
            }
        }

        private string _maxRecordCount;
        public string MaxRecordCount
        {
            get { return _maxRecordCount; }
            set
            {
                if (_maxRecordCount != value)
                {
                    _maxRecordCount = value;
                    OnPropertyChanged("MaxRecordCount");
                }
            }
        }

        private bool _isBloodPressure;
        public bool IsBloodPressure
        {
            get { return _isBloodPressure; }
            set
            {
                if (_isBloodPressure != value)
                {
                    _isBloodPressure = value;
                    OnPropertyChanged("IsBloodPressure");
                }
            }
        }

        private bool _isSleep;
        public bool IsSleep
        {
            get { return _isSleep; }
            set
            {
                if (_isSleep != value)
                {
                    _isSleep = value;
                    OnPropertyChanged("IsSleep");
                }
            }
        }
        
        private bool _isLiquids;
        public bool IsLiquids
        {
            get { return _isLiquids; }
            set
            {
                if (_isLiquids != value)
                {
                    _isLiquids = value;
                    OnPropertyChanged("IsLiquids");
                }
            }
        }
        
        private bool _isMedication;
        public bool IsMedication
        {
            get { return _isMedication; }
            set
            {
                if (_isMedication != value)
                {
                    _isMedication = value;
                    OnPropertyChanged("IsMedication");
                }
            }
        }
        
        public ReportViewModel(ObservableCollection<WorkspaceViewModel> workspaces)
        {
            _workspaces = workspaces;
            LogObj = new ObservableCollection<object>();
            BtnSubmit = new RelayCommand(RunFilterSearch);
            BtnCancel = new RelayCommand(CancelReports);
            BuildComboDateList();

            IsBloodPressure = false;
            IsSleep = false;
            IsLiquids = false;
            IsMedication = false;
        }

        private void BuildComboDateList()
        {
            using (SQLiteConnection conn = new SQLiteConnection(Settings.DBPATH, SQLiteOpenFlags.ReadWrite))
            {
                IEnumerable<DailyLogs> logs = conn.Table<DailyLogs>();
                List<string> datelist = logs.Select(x => x.LogDateTime.ToString("MM/dd/yyyy")).ToList();

                StartDateList = datelist;
                SelectedStartDateItem = StartDateList.LastOrDefault();
                EndDateList = datelist;
                SelectedEndDateItem = EndDateList.LastOrDefault();
            }
        }

        private void RunFilterSearch(object obj)
        {
            bool isDates = false;
            if (SelectedStartDateItem != null)
            {
                if (SelectedEndDateItem == null)
                {
                    Message = "End Date Must be set";
                }
                else
                    isDates = true;
            }
            LogObj.Clear();
            using (SQLiteConnection conn = new SQLiteConnection(Settings.DBPATH, SQLiteOpenFlags.ReadWrite))
            {
                IQueryable<DailyLogs> dailyLogs = conn.Table<DailyLogs>().AsQueryable();
                //IQueryable<Sleep> sleep = conn.Table<Sleep>().AsQueryable();
                //IQueryable<Liquid> liquids = conn.Table<Liquid>().AsQueryable();
                IQueryable<BloodPressure> bpReadings = conn.Table<BloodPressure>().AsQueryable();
                //IQueryable<Medication> meds = conn.Table<Medication>().AsQueryable();

                if (isDates)
                {
                    int month = (Convert.ToDateTime(SelectedStartDateItem).Month);
                    dailyLogs = dailyLogs.Where(x => x.LogDateTime.Month == month);
                }
                IList<int> dailyLogIds = dailyLogs.Select(x => x.DailyLogID).ToList();

                foreach (int logId in dailyLogIds)
                {
                    object log = CreateDynamicClass();
                    DailyLogs dl = dailyLogs.FirstOrDefault(x => x.DailyLogID == logId);
                    
                    if (IsBloodPressure)
                    {
                        List<BloodPressure> bps = conn.Table<BloodPressure>().Where(x => x.DailyLogID == logId).ToList();
                        foreach(BloodPressure bp in bps)
                        {
                            log.GetType().GetProperty("LogDateTime").SetValue(log, dl.LogDateTime);
                            log.GetType().GetProperty("BPTime").SetValue(log, bp.BPTime);
                            log.GetType().GetProperty("BPSystolic").SetValue(log, bp.BPSystolic);
                            log.GetType().GetProperty("BPDiastolic").SetValue(log, bp.BPDiastolic);
                            log.GetType().GetProperty("BPPulseRate").SetValue(log, bp.BPPulseRate);
                        }
                    }
                    if (IsSleep)
                    {
                        //Sleep s = sleep.FirstOrDefault(x => x.SleepID == dl.SleepID);
                        //log.GetType().GetProperty("WakeTime").SetValue(log, s.WakeTime);
                        //log.GetType().GetProperty("SleepTime").SetValue(log, s.SleepTime);
                        //log.GetType().GetProperty("TotalSleepTime").SetValue(log, s.TotalSleepTime);
                    }
                    if (IsLiquids)
                    {
                        //reportLog.Liquids = new List<Liquid>();
                        //reportLog.Liquids = liquids.Where(x => x.DailyLogID == logId).ToList();
                    }
                    if (IsMedication)
                    {
                        //reportLog.Medications = new List<Medication>();
                        //reportLog.Medications = meds.Where(x => x.DailyLogID == logId).ToList();
                    }
                    LogObj.Add(log);
                }
            }
        }

        private object CreateDynamicClass()
        {
            ClassBuilder cb = new ClassBuilder("RemoteLog");
            _classProperties = new Dictionary<string, Type>();
            Dictionary<string, Type> objProp = new Dictionary<string, Type>();

            if (IsBloodPressure)
            {
                objProp = ModelHelper.GetBloodPressureProperties();
                foreach (KeyValuePair<string, Type> prop in objProp)
                    _classProperties.Add(prop.Key, prop.Value);
            }
            if (IsSleep)
            {
                objProp = ModelHelper.GetSleepProperties();
                foreach (KeyValuePair<string, Type> prop in objProp)
                    _classProperties.Add(prop.Key, prop.Value);
            }
            if (IsLiquids)
            {
                objProp = ModelHelper.GetLiquidProperties();
                foreach (KeyValuePair<string, Type> prop in objProp)
                    _classProperties.Add(prop.Key, prop.Value);
            }
            if (IsMedication)
            {
                objProp = ModelHelper.GetMedicationProperties();
                foreach (KeyValuePair<string, Type> prop in objProp)
                    _classProperties.Add(prop.Key, prop.Value);
            }

            return cb.CreateObject(_classProperties);
        }

        private void CancelReports(object obj)
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
