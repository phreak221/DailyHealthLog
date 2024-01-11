using DailyHealthLog.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyHealthLog.ViewModels
{
    public class DetailFoodViewModel : WorkspaceViewModel
    {
        private ObservableCollection<Food> _foodCollection;
        public ObservableCollection<Food> FoodCollection
        {
            get { return _foodCollection; }
            set
            {
                if (_foodCollection != value)
                {
                    _foodCollection = value;
                    OnPropertyChanged("FoodCollection");
                }
            }
        }

        public DetailFoodViewModel(List<Food> foodlist)
        {
            FoodCollection = new ObservableCollection<Food>(foodlist);
        }
    }
}
