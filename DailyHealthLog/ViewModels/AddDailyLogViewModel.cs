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
    public class AddDailyLogViewModel : WorkspaceViewModel
    {
        private SQLiteConnection _conn;
        public ICommand BtnSetDateTime { get; set; }
        public ICommand BtnSetWakeTime { get; set; }
        public ICommand BtnSetBedTime { get; set; }
        public ICommand BtnSave { get; set; }
        public ICommand BtnCancel { get; set; }

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

        private string _logNotes;
        public string LogNotes
        {
            get { return _logNotes; }
            set
            {
                if (_logNotes != value)
                {
                    _logNotes = value;
                    OnPropertyChanged("LogNotes");
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

        private string _bedTime;
        public string BedTime
        {
            get { return _bedTime; }
            set
            {
                if (_bedTime != value)
                {
                    _bedTime = value;
                    OnPropertyChanged("BedTime");
                }
            }
        }
        
        private string _pounds;
        public string Pounds
        {
            get { return _pounds; }
            set
            {
                if (_pounds != value)
                {
                    _pounds = value;
                    OnPropertyChanged("Pounds");
                }
            }
        }
        
        private int _selectedTabIndex;
        public int SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            set
            {
                if (_selectedTabIndex != value)
                {
                    _selectedTabIndex = value;
                    OnPropertyChanged("SelectedTabIndex");
                }
            }
        }
        
        private DailyLogsModel _logsModel;
        public DailyLogsModel LogsModel
        {
            get { return _logsModel; }
            set
            {
                if (_logsModel != value)
                {
                    _logsModel = value;
                    OnPropertyChanged("LogsModel");
                }
            }
        }

        private ObservableCollection<object> _tabCollection = new ObservableCollection<object>();
        public ObservableCollection<object> TabCollection
        {
            get { return _tabCollection; }
        }

        private LiquidViewModel _liquidVM;
        private FoodViewModel _foodVM;
        private BloodPressureViewModel _bloodpressureVM;
        private MedicationViewModel _medicationVM;

        private ObservableCollection<WorkspaceViewModel> _workspaces;
        public AddDailyLogViewModel(ObservableCollection<WorkspaceViewModel> Workspaces)
        {
            _workspaces = Workspaces;
            _conn = new SQLiteConnection(Settings.DBPATH, SQLiteOpenFlags.ReadWrite, false);
            
            BtnSetDateTime = new RelayCommand(SetDateTime);
            BtnSetWakeTime = new RelayCommand(SetWakeTime);
            BtnSetBedTime = new RelayCommand(SetBedTime);            
            BtnSave = new RelayCommand(SaveLog);
            BtnCancel = new RelayCommand(CancelLog);

            BuildDailyLogModel();

            _liquidVM = new LiquidViewModel(LogsModel.Liquids);
            TabCollection.Add(_liquidVM);
            _foodVM = new FoodViewModel(LogsModel.Foods);
            TabCollection.Add(_foodVM);
            _bloodpressureVM = new BloodPressureViewModel(LogsModel.BloodPressures);
            TabCollection.Add(_bloodpressureVM);
            _medicationVM = new MedicationViewModel(LogsModel.Medications);
            TabCollection.Add(_medicationVM);
            SelectedTabIndex = 0;
        }

        private void BuildDailyLogModel()
        {
            LogsModel = new DailyLogsModel();
            LogsModel.Weight = new Weight();

            LogsModel.Liquids = new List<Liquid>();
            LogsModel.Foods = new List<Food>();
            LogsModel.BloodPressures = new List<BloodPressure>();
            LogsModel.Medications = new List<Medication>();
        }
        
        private void SetDateTime(object obj)
        {
            LogDateTime = String.Format("{0:g}", DateTime.Now);
        }

        private void SetWakeTime(object obj)
        {
            WakeTime = String.Format("{0:g}", DateTime.Now);
        }

        private void SetBedTime(object obj)
        {
            BedTime = String.Format("{0:g}", DateTime.Now);
        }
        
        private void SaveLog(object obj)
        {
            DailyLogs daily = new DailyLogs();
            daily.LogDateTime = Convert.ToDateTime(LogDateTime);
            daily.DailyLogNotes = LogNotes;
            daily.SleepID = AddSleepLog();
            daily.WeightID = AddWeightLog();
            _conn.Insert(daily);

            AddDrinksToLog(_liquidVM.Liquids, daily.DailyLogID);
            AddFoodToLog(_foodVM.Foods, daily.DailyLogID);
            AddReadingToLog(_bloodpressureVM.BloodPressures, daily.DailyLogID);
            AddMedicationToLog(_medicationVM.Medications, daily.DailyLogID);
            
            GoBackToMain();
        }

        private void AddDrinksToLog(ObservableCollection<Liquid> liqs, int dailyId)
        {
            foreach (Liquid l in liqs)
            {
                Liquid liq = new Liquid()
                {
                    DailyLogID = dailyId,
                    DrinkItem = l.DrinkItem,
                    DrinkAmount = l.DrinkAmount
                };
                _conn.Insert(liq);
            }
        }

        private void AddFoodToLog(ObservableCollection<Food> foodItems, int dailyId)
        {
            foreach(Food f in foodItems)
            {
                Food fnew = new Food()
                {
                    DailyLogID = dailyId,
                    FoodItem = f.FoodItem
                };
                _conn.Insert(fnew);
            }
        }

        private void AddReadingToLog(ObservableCollection<BloodPressure> bps, int dailyId)
        {
            foreach (BloodPressure bp in bps)
            {
                BloodPressure b = new BloodPressure()
                {
                    DailyLogID = dailyId,
                    BPTime = bp.BPTime,
                    BPSystolic = bp.BPSystolic,
                    BPDiastolic = bp.BPDiastolic,
                    BPPulseRate = bp.BPPulseRate,
                    BPStatus = bp.BPStatus
                };
                _conn.Insert(b);
            }
        }

        private void AddMedicationToLog(ObservableCollection<Medication> mds, int dailyId)
        {
            foreach (Medication m in mds)
            {
                Medication mnew = new Medication()
                {
                    DailyLogID = dailyId,
                    MedicationTime = m.MedicationTime,
                    MedicationName = m.MedicationName,
                    MedicationDose = m.MedicationDose
                };
                _conn.Insert(mnew);
            }
        }

        private int AddSleepLog()
        {
            Sleep sleep = new Sleep();
            if (!String.IsNullOrEmpty(WakeTime))
                sleep.WakeTime = Convert.ToDateTime(WakeTime);
            else
                sleep.WakeTime = DateTime.MinValue;

            if (!String.IsNullOrEmpty(BedTime))
            {
                sleep.SleepTime = Convert.ToDateTime(BedTime);
            }
            else
                sleep.SleepTime = DateTime.MinValue;

            if (sleep.WakeTime != DateTime.MinValue && sleep.SleepTime != DateTime.MinValue)
            {
                sleep.TotalSleepTime = (sleep.SleepTime - sleep.WakeTime);
            }
            
            _conn.Insert(sleep);
            return sleep.SleepID;
        }

        private int AddWeightLog()
        {
            Weight weight = new Weight();
            if (!String.IsNullOrEmpty(Pounds))
            {
                Int32.TryParse(Pounds, out int lbs);
                weight.WeightAmount = lbs;
            }
            else
                weight.WeightAmount = null;
            
            _conn.Insert(weight);
            return weight.WeightID;
        }

        private void CancelLog(object obj)
        {

        }
                
        private void GoBackToMain()
        {
            _conn.Close();

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
