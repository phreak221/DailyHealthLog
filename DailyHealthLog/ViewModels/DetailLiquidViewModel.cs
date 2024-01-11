using DailyHealthLog.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyHealthLog.ViewModels
{
    public class DetailLiquidViewModel : WorkspaceViewModel
    {
        private ObservableCollection<Liquid> _liquidCollection;
        public ObservableCollection<Liquid> LiquidCollection
        {
            get { return _liquidCollection; }
            set
            {
                if (_liquidCollection != value)
                {
                    _liquidCollection = value;
                    OnPropertyChanged("LiquidCollection");
                }
            }
        }

        public DetailLiquidViewModel(List<Liquid> liquidlist)
        {
            LiquidCollection = new ObservableCollection<Liquid>(liquidlist);
        }
    }
}
