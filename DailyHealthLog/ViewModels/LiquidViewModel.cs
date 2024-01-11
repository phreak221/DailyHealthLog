using DailyHealthLog.Config;
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
    public class LiquidViewModel : WorkspaceViewModel
    {
        public ICommand BtnEditDrink { get; set; }
        public ICommand BtnDeleteDrink { get; set; }
        public ICommand BtnSubmitDrink { get; set; }

        private ObservableCollection<Liquid> _liquids;
        public ObservableCollection<Liquid> Liquids
        {
            get { return _liquids; }
            set
            {
                if (_liquids != value)
                {
                    _liquids = value;
                    OnPropertyChanged("Liquids");
                }
            }
        }

        private Liquid _selectedDrinkItem;
        public Liquid SelectedDrinkItem
        {
            get { return _selectedDrinkItem; }
            set
            {
                if (_selectedDrinkItem != value)
                {
                    _selectedDrinkItem = value;
                    OnPropertyChanged("SelectedDrinkItem");
                }
            }
        }

        private int _drinkId;
        public int DrinkId
        {
            get { return _drinkId; }
            set
            {
                if (_drinkId != value)
                {
                    _drinkId = value;
                    OnPropertyChanged("DrinkId");
                }
            }
        }

        private string _drinkItem;
        public string DrinkItem
        {
            get { return _drinkItem; }
            set
            {
                if (_drinkItem != value)
                {
                    _drinkItem = value;
                    OnPropertyChanged("DrinkItem");
                }
            }
        }

        private string _drinkAmount;
        public string DrinkAmount
        {
            get { return _drinkAmount; }
            set
            {
                if (_drinkAmount != value)
                {
                    _drinkAmount = value;
                    OnPropertyChanged("DrinkAmount");
                }
            }
        }

        private int indexId = -1;
                
        public LiquidViewModel(List<Liquid> liquidlist)
        {
            Liquids = new ObservableCollection<Liquid>(liquidlist);
            BtnEditDrink = new RelayCommand(EditDrink);
            BtnDeleteDrink = new RelayCommand(DeleteDrink);
            BtnSubmitDrink = new RelayCommand(SubmitDrink);
        }
        
        private void EditDrink(object obj)
        {
            if (SelectedDrinkItem == null) return;

            DrinkId = SelectedDrinkItem.LiquidID;
            DrinkItem = SelectedDrinkItem.DrinkItem;
            DrinkAmount = SelectedDrinkItem.DrinkAmount.ToString();
        }

        private void DeleteDrink(object obj)
        {
            if (SelectedDrinkItem == null) return;
            Liquids.Remove(SelectedDrinkItem);
        }

        private void SubmitDrink(object obj)
        {
            int.TryParse(DrinkAmount, out int amount);
            Liquid lm = Liquids.FirstOrDefault(x => x.LiquidID == DrinkId);
            if (lm != null)
            {
                lm.DrinkItem = DrinkItem;                
                lm.DrinkAmount = amount;
                CollectionViewSource.GetDefaultView(Liquids).Refresh();

                DrinkId = 0;
                DrinkItem = string.Empty;
                DrinkAmount = string.Empty;
                return;
            }

            Liquid liquid = new Liquid()
            {
                LiquidID = indexId,
                DrinkItem = DrinkItem,
                DrinkAmount = amount
            };

            Liquids.Add(liquid);

            DrinkItem = string.Empty;
            DrinkAmount = string.Empty;
            DrinkId = 0;
            indexId--;
        }
    }
}
