using DailyHealthLog.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyHealthLog.ViewModels
{
    public class DetailMedicationViewModel : WorkspaceViewModel
    {
        private ObservableCollection<Medication> _medicationCollection;
        public ObservableCollection<Medication> MedicationCollection
        {
            get { return _medicationCollection; }
            set
            {
                if (_medicationCollection != value)
                {
                    _medicationCollection = value;
                    OnPropertyChanged("MedicationCollection");
                }
            }
        }

        public DetailMedicationViewModel(List<Medication> medlist)
        {
            MedicationCollection = new ObservableCollection<Medication>(medlist);
        }
    }
}
