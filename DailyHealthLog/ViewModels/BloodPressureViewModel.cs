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
    public class BloodPressureViewModel : WorkspaceViewModel
    {
        public ICommand BtnEditReading { get; set; }
        public ICommand BtnDeleteReading { get; set; }
        public ICommand BtnAddReading { get; set; }
        public ICommand BtnSetBpTime { get; set; }

        private int _bpId;
        public int BpId
        {
            get { return _bpId; }
            set
            {
                if (_bpId != value)
                {
                    _bpId = value;
                    OnPropertyChanged("BpId");
                }
            }
        }

        private string _bpTime;
        public string BpTime
        {
            get { return _bpTime; }
            set
            {
                if (_bpTime != value)
                {
                    _bpTime = value;
                    OnPropertyChanged("BpTime");
                }
            }
        }

        private string _bpSystolic;
        public string BpSystolic
        {
            get { return _bpSystolic; }
            set
            {
                if (_bpSystolic != value)
                {
                    _bpSystolic = value;
                    OnPropertyChanged("BpSystolic");
                }
            }
        }

        private string _bpDiastolic;
        public string BpDiastolic
        {
            get { return _bpDiastolic; }
            set
            {
                if (_bpDiastolic != value)
                {
                    _bpDiastolic = value;
                    OnPropertyChanged("BpDiastolic");
                }
            }
        }

        private string _bpPulseRate;
        public string BpPulseRate
        {
            get { return _bpPulseRate; }
            set
            {
                if (_bpPulseRate != value)
                {
                    _bpPulseRate = value;
                    OnPropertyChanged("BpPulseRate");
                }
            }
        }

        private BloodPressure _selectedBloodPressure;
        public BloodPressure SelectedBloodPressure
        {
            get { return _selectedBloodPressure; }
            set
            {
                if (_selectedBloodPressure != value)
                {
                    _selectedBloodPressure = value;
                    OnPropertyChanged("SelectedBloodPressure");
                }
            }
        }

        private ObservableCollection<BloodPressure> _bloodPressures;
        public ObservableCollection<BloodPressure> BloodPressures
        {
            get { return _bloodPressures; }
            set
            {
                if (_bloodPressures != value)
                {
                    _bloodPressures = value;
                    OnPropertyChanged("BloodPressures");
                }
            }
        }

        private int indexId = -1;

        public BloodPressureViewModel()
        {

        }

        public BloodPressureViewModel(List<BloodPressure> bpReadings)
        {
            BloodPressures = new ObservableCollection<BloodPressure>(bpReadings);
            BtnEditReading = new RelayCommand(EditReading);
            BtnDeleteReading = new RelayCommand(DeleteReading);
            BtnAddReading = new RelayCommand(SubmitReading);
            BtnSetBpTime = new RelayCommand(SetBpTime);
        }

        private void SubmitReading(object obj)
        {
            int.TryParse(BpSystolic, out int systolic);
            int.TryParse(BpDiastolic, out int diastolic);
            int.TryParse(BpPulseRate, out int pulse);

            BloodPressure bpm = BloodPressures.FirstOrDefault(x => x.BPID == BpId);
            if (bpm != null)
            {
                bpm.BPTime = BpTime;
                bpm.BPSystolic = systolic;
                bpm.BPDiastolic = diastolic;
                bpm.BPPulseRate = pulse;
                bpm.BPStatus = GetPressureStatus(systolic, diastolic);
                CollectionViewSource.GetDefaultView(BloodPressures).Refresh();

                ClearTextboxes();
                return;
            }

            BloodPressure BloodPressure = new BloodPressure()
            {
                BPID = indexId,
                BPTime = BpTime,
                BPSystolic = systolic,
                BPDiastolic = diastolic,
                BPPulseRate = pulse,
                BPStatus = GetPressureStatus(systolic, diastolic)
            };
            BloodPressures.Add(BloodPressure);
            ClearTextboxes();
            indexId--;
        }

        private string GetPressureStatus(int systolic, int diastolic)
        {
            if (systolic < 140 && diastolic < 90)
            {
                return "Goal";
            }
            if (systolic > 140 && diastolic < 90)
            {
                return "Systolic High";
            }
            if (systolic < 140 && diastolic >= 90)
            {
                return "Diastolic High";
            }
            if (systolic < 90 && diastolic < 60)
            {
                return "Low Crisis";
            }
            if (systolic > 140 && diastolic >= 90)
            {
                return "High Crisis";
            }
            //if (systolic < 120 && diastolic < 80)
            //{
            //    return "Normal";
            //}
            //else if ((systolic >= 120 && systolic <= 129) && diastolic < 80)
            //{
            //    return "Elevated";
            //}
            //else if ((systolic >= 130 && systolic <= 139) || (diastolic >= 80 && diastolic <= 89))
            //{
            //    return "Stage1";
            //}
            //else if ((systolic >= 140) || (diastolic >= 90))
            //{
            //    return "Stage2";
            //}
            //else if (((systolic > 180) && (diastolic > 120)) || (systolic > 180) || (diastolic > 120))
            //{
            //    return "Crisis";
            //}
            return string.Empty;
        }

        private void EditReading(object obj)
        {
            if (SelectedBloodPressure == null) return;

            BpId = SelectedBloodPressure.BPID;
            BpTime = SelectedBloodPressure.BPTime;
            BpSystolic = SelectedBloodPressure.BPSystolic.ToString();
            BpDiastolic = SelectedBloodPressure.BPDiastolic.ToString();
            BpPulseRate = SelectedBloodPressure.BPPulseRate.ToString();
        }

        private void DeleteReading(object obj)
        {
            if (SelectedBloodPressure == null) return;
            BloodPressures.Remove(SelectedBloodPressure);
        }

        private void SetBpTime(object obj)
        {
            BpTime = String.Format("{0:t}", DateTime.Now);
        }

        private void ClearTextboxes()
        {
            BpId = 0;
            BpTime = string.Empty;
            BpSystolic = string.Empty;
            BpDiastolic = string.Empty;
            BpPulseRate = string.Empty;
        }
    }
}
