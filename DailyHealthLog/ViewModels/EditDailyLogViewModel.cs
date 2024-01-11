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
    public class EditDailyLogViewModel : WorkspaceViewModel
    {
        public ICommand BtnSetWakeTime { get; set; }
        public ICommand BtnSetBedTime { get; set; }
        public ICommand BtnSave { get; set; }
        public ICommand BtnCancel { get; set; }

        private ObservableCollection<WorkspaceViewModel> _workspaces;

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
                    UpdateWakeText();
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
                    UpdateSleepText();
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

        private LiquidViewModel _liquidVM;
        private FoodViewModel _foodVM;
        private BloodPressureViewModel _bloodPressureVM;
        private MedicationViewModel _medicationVM;

        public EditDailyLogViewModel(ObservableCollection<WorkspaceViewModel> workspaces, int logId)
        {
            _workspaces = workspaces;
            BtnSetWakeTime = new RelayCommand(SetWakeTime);
            BtnSetBedTime = new RelayCommand(SetBedTime);
            BtnSave = new RelayCommand(SaveChanges);
            BtnCancel = new RelayCommand(CancelChanges);

            DailyLogsModel = new DailyLogsModel();
            BuildDailyLogModel(logId);

            _liquidVM = new LiquidViewModel(DailyLogsModel.Liquids);
            TabCollection.Add(_liquidVM);
            _foodVM = new FoodViewModel(DailyLogsModel.Foods);
            TabCollection.Add(_foodVM);
            _bloodPressureVM = new BloodPressureViewModel(DailyLogsModel.BloodPressures);
            TabCollection.Add(_bloodPressureVM);
            _medicationVM = new MedicationViewModel(DailyLogsModel.Medications);
            TabCollection.Add(_medicationVM);
            SelectedTabIndex = 0;
        }

        private void BuildDailyLogModel(int logId)
        {
            //Logs = _conn.GetWithChildren<DailyLogs>(logId);
            
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

        private void UpdateWakeText()
        {
            DateTime.TryParse(WakeTime, out DateTime wake);
            if (wake != null)
            {
                DailyLogsModel.Sleep.WakeTime = wake;
                CalculateTotalSLeepTime();
            }
        }

        private void UpdateSleepText()
        {
            DateTime.TryParse(SleepTime, out DateTime sleep);
            if (sleep != null)
            {
                DailyLogsModel.Sleep.SleepTime = sleep;
                CalculateTotalSLeepTime();
            }
        }

        private void SetWakeTime(object obj)
        {
            WakeTime = String.Format("{0:g}", DateTime.Now);
            DailyLogsModel.Sleep.WakeTime = DateTime.Now;
            CalculateTotalSLeepTime();
        }

        private void SetBedTime(object obj)
        {
            SleepTime = String.Format("{0:g}", DateTime.Now);
            DailyLogsModel.Sleep.SleepTime = DateTime.Now;
            CalculateTotalSLeepTime();
        }

        private void CalculateTotalSLeepTime()
        {
            if (DailyLogsModel.Sleep.SleepTime != DateTime.MinValue && DailyLogsModel.Sleep.WakeTime != DateTime.MinValue)
            {
                DailyLogsModel.Sleep.TotalSleepTime = (DailyLogsModel.Sleep.SleepTime - DailyLogsModel.Sleep.WakeTime);
                TotalSleepTime = DailyLogsModel.Sleep.TotalSleepTime.ToString();
            }
        }

        private void SaveChanges(object obj)
        {
            int.TryParse(WeightAmount, out int amount);
            DailyLogsModel.Weight.WeightAmount = amount;
            DailyLogsModel.Logs.DailyLogNotes = DailyLogNotes;

            using (SQLiteConnection conn = new SQLiteConnection(Settings.DBPATH, SQLiteOpenFlags.ReadWrite))
            {
                conn.Update(DailyLogsModel.Logs);
                if (DailyLogsModel.Sleep.SleepID > 0)
                    conn.Update(DailyLogsModel.Sleep);
                else
                    conn.Insert(DailyLogsModel.Sleep);

                conn.Update(DailyLogsModel.Weight);

                conn.Close();
                conn.Dispose();
            }

            AddDrinksToLog(_liquidVM.Liquids, DailyLogsModel.Logs.DailyLogID);
            AddFoodToLog(_foodVM.Foods, DailyLogsModel.Logs.DailyLogID);
            AddBloodPressureToLog(_bloodPressureVM.BloodPressures, DailyLogsModel.Logs.DailyLogID);
            AddMedicationToLog(_medicationVM.Medications, DailyLogsModel.Logs.DailyLogID);
            
            GoBackToMain();
        }

        private void AddDrinksToLog(ObservableCollection<Liquid> liqs, int dailyLogId)
        {
            using (SQLiteConnection conn = new SQLiteConnection(Settings.DBPATH, SQLiteOpenFlags.ReadWrite))
            {
                foreach (Liquid l in liqs)
                {
                    if (l.LiquidID > 0)
                    {
                        Liquid dbLiquid = DailyLogsModel.Liquids.FirstOrDefault(x => x.LiquidID == l.LiquidID);
                        if (dbLiquid == null) continue;

                        dbLiquid.DrinkItem = l.DrinkItem;
                        dbLiquid.DrinkAmount = l.DrinkAmount;
                        conn.Update(dbLiquid);
                        continue;
                    }
                    l.DailyLogID = dailyLogId;
                    conn.Insert(l);
                }

                conn.Close();
                conn.Dispose();
            }
        }

        private void AddFoodToLog(ObservableCollection<Food> foods, int dailyLogId)
        {
            using (SQLiteConnection conn = new SQLiteConnection(Settings.DBPATH, SQLiteOpenFlags.ReadWrite))
            {
                foreach (Food f in foods)
                {
                    if (f.FoodID > 0)
                    {
                        Food dbFood = DailyLogsModel.Foods.FirstOrDefault(x => x.FoodID == f.FoodID);
                        if (dbFood == null) continue;

                        dbFood.FoodItem = f.FoodItem;
                        conn.Update(dbFood);
                        continue;
                    }
                    f.DailyLogID = dailyLogId;
                    conn.Insert(f);
                }
                conn.Close();
                conn.Dispose();
            }
        }

        private void AddBloodPressureToLog(ObservableCollection<BloodPressure> bps, int dailyLogId)
        {
            using (SQLiteConnection conn = new SQLiteConnection(Settings.DBPATH, SQLiteOpenFlags.ReadWrite))
            {
                foreach (BloodPressure bp in bps)
                {
                    if (bp.BPID > 0)
                    {
                        BloodPressure dbBp = DailyLogsModel.BloodPressures.FirstOrDefault(x => x.BPID == bp.BPID);
                        if (dbBp == null) continue;

                        dbBp.BPTime = bp.BPTime;
                        dbBp.BPSystolic = bp.BPSystolic;
                        dbBp.BPDiastolic = bp.BPDiastolic;
                        dbBp.BPPulseRate = bp.BPPulseRate;
                        dbBp.BPStatus = bp.BPStatus;
                        conn.Update(dbBp);
                        continue;
                    }
                    bp.DailyLogID = dailyLogId;
                    conn.Insert(bp);
                }
                conn.Close();
                conn.Dispose();
            }
        }

        private void AddMedicationToLog(ObservableCollection<Medication> meds, int dailyLogId)
        {
            using (SQLiteConnection conn = new SQLiteConnection(Settings.DBPATH, SQLiteOpenFlags.ReadWrite))
            {
                foreach (Medication med in meds)
                {
                    if (med.MedicationID > 0)
                    {
                        Medication dbMed = DailyLogsModel.Medications.FirstOrDefault(x => x.MedicationID == med.MedicationID);
                        if (dbMed == null) return;

                        dbMed.MedicationTime = med.MedicationTime;
                        dbMed.MedicationName = med.MedicationName;
                        dbMed.MedicationDose = med.MedicationDose;
                        conn.Update(dbMed);
                        continue;
                    }
                    med.DailyLogID = dailyLogId;
                    conn.Insert(med);
                }
                conn.Close();
                conn.Dispose();
            }
        }

        private void GoBackToMain()
        {
            _workspaces.Clear();
            MainViewModel workspace = new MainViewModel(_workspaces);
            _workspaces.Add(workspace);
            SetActiveWorkspace(workspace);
        }

        private void CancelChanges(object obj)
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
