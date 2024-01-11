using DailyHealthLog.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyHealthLog.ViewModels
{
    public class DetailBloodPressureViewModel : WorkspaceViewModel
    {
        private ObservableCollection<BloodPressure> _bloodpressureCollection;
        public ObservableCollection<BloodPressure> BloodPressureCollection
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

        public DetailBloodPressureViewModel(List<BloodPressure> bplist)
        {
            BloodPressureCollection = new ObservableCollection<BloodPressure>(bplist);
        }
    }
}
