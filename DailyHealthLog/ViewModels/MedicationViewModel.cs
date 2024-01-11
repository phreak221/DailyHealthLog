using DailyHealthLog.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace DailyHealthLog.ViewModels
{
    public class MedicationViewModel : WorkspaceViewModel
    {
        private readonly SQLiteConnection _conn;

        public ICommand BtnEditMed { get; set; }
        public ICommand BtnDeleteMed { get; set; }
        public ICommand BtnAddMedication { get; set; }
        public ICommand BtnSetMedicationTime { get; set; }

        private int _medId;
        public int MedId
        {
            get { return _medId; }
            set
            {
                if (_medId != value)
                {
                    _medId = value;
                    OnPropertyChanged("MedId");
                }
            }
        }

        private string _medTime;
        public string MedTime
        {
            get { return _medTime; }
            set
            {
                if (_medTime != value)
                {
                    _medTime = value;
                    OnPropertyChanged("MedTime");
                }
            }
        }

        private Medication _selectMedItem;
        public Medication SelectedMedItem
        {
            get { return _selectMedItem; }
            set
            {
                if (_selectMedItem != value)
                {
                    _selectMedItem = value;
                    OnPropertyChanged("SelectedMedItem");
                }
            }
        }

        private string _medName;
        public string MedName
        {
            get { return _medName; }
            set
            {
                if (_medName != value)
                {
                    _medName = value;
                    OnPropertyChanged("MedName");
                }
            }
        }

        private string _medDose;
        public string MedDose
        {
            get { return _medDose; }
            set
            {
                if (_medDose != value)
                {
                    _medDose = value;
                    OnPropertyChanged("MedDose");
                }
            }
        }

        private MedModel _medSelectedItem;
        public MedModel MedSelectedItem
        {
            get { return _medSelectedItem; }
            set
            {
                if (_medSelectedItem != value)
                {
                    _medSelectedItem = value;
                    OnPropertyChanged("MedSelectedItem");
                    SetMedicationValues();
                }
            }
        }

        private ObservableCollection<Medication> _medications;
        public ObservableCollection<Medication> Medications
        {
            get { return _medications; }
            set
            {
                if (_medications != value)
                {
                    _medications = value;
                    OnPropertyChanged("Medications");
                }
            }
        }

        private ObservableCollection<MedModel> _meds;
        public ObservableCollection<MedModel> Meds
        {
            get { return _meds; }
            set
            {
                if (_meds != value)
                {
                    _meds = value;
                    OnPropertyChanged("Meds");
                }
            }
        }

        private int indexId = -1;

        public MedicationViewModel()
        {
            
        }

        public MedicationViewModel(List<Medication> meds)
        {
            _conn = new SQLiteConnection("healthlogs.sqlite", SQLiteOpenFlags.ReadWrite, false);
            Medications = new ObservableCollection<Medication>(meds);
            Meds = new ObservableCollection<MedModel>();
            
            BtnEditMed = new RelayCommand(EditMedication);
            BtnDeleteMed = new RelayCommand(DeleteMedication);
            BtnSetMedicationTime = new RelayCommand(SetMedicationTime);
            BtnAddMedication = new RelayCommand(SubmitMedication);
            
            BuildMedCombo();
            MedSelectedItem = Meds.FirstOrDefault();
        }

        private void BuildMedCombo()
        {
            List<Medication> medList = _conn.Table<Medication>().ToList();
            foreach (Medication med in medList)
            {
                if (Meds.Select(x => x.MedName).Contains(med.MedicationName)) continue;

                MedModel mod = new MedModel();
                mod.MedId = med.MedicationID;
                mod.MedName = med.MedicationName;
                Meds.Add(mod);
            }
        }

        private void SetMedicationValues()
        {
            MedName = MedSelectedItem.MedName;
        }
        
        private void EditMedication(object obj)
        {
            if (SelectedMedItem == null) return;

            MedSelectedItem = Meds.FirstOrDefault(x => x.MedName == SelectedMedItem.MedicationName);
            MedId = SelectedMedItem.MedicationID;
            MedTime = SelectedMedItem.MedicationTime;
            MedDose = SelectedMedItem.MedicationDose;
        }

        private void DeleteMedication(object obj)
        {
            if (SelectedMedItem == null) return;

            Medications.Remove(SelectedMedItem);
        }

        private void SetMedicationTime(object obj)
        {
            MedTime = String.Format("{0:t}", DateTime.Now);
        }

        private void SubmitMedication(object obj)
        {
            //if (SelectedMedItem == null) return;

            //MedId = SelectedMedItem.MedicationID;
            Medication med = Medications.FirstOrDefault(x => x.MedicationID == MedId);
            if (med != null)
            {
                med.MedicationTime = MedTime;
                med.MedicationName = MedName;
                med.MedicationDose = MedDose;
                CollectionViewSource.GetDefaultView(Medications).Refresh();

                MedId = 0;
                MedName = string.Empty;
                MedDose = string.Empty;
                return;
            }

            Medication m = new Medication()
            {
                MedicationID = indexId,
                MedicationTime = MedTime,
                MedicationName = MedName,
                MedicationDose = MedDose
            };

            Medications.Add(m);
            MedId = 0;
            indexId--;
            MedName = string.Empty;
            MedDose = string.Empty;
        }
    }
}
