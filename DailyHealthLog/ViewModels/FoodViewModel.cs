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
    public class FoodViewModel : WorkspaceViewModel
    {
        private readonly SQLiteConnection _conn;

        public ICommand BtnEditFood { get; set; }
        public ICommand BtnDeleteFood { get; set; }
        public ICommand BtnAddFood { get; set; }

        private int _foodId;
        public int FoodId
        {
            get { return _foodId; }
            set
            {
                if (_foodId != value)
                {
                    _foodId = value;
                    OnPropertyChanged("FoodId");
                }
            }
        }

        private string _foodItem;
        public string FoodItem
        {
            get { return _foodItem; }
            set
            {
                if (_foodItem != value)
                {
                    _foodItem = value;
                    OnPropertyChanged("FoodItem");
                }
            }
        }

        private Food _selectedFoodItem;
        public Food SelectedFoodItem
        {
            get { return _selectedFoodItem; }
            set
            {
                if (_selectedFoodItem != value)
                {
                    _selectedFoodItem = value;
                    OnPropertyChanged("SelectedFoodItem");
                }
            }
        }

        private ObservableCollection<Food> _foods;
        public ObservableCollection<Food> Foods
        {
            get { return _foods; }
            set
            {
                if (_foods != value)
                {
                    _foods = value;
                    OnPropertyChanged("Foods");
                }
            }
        }

        private int indexId = -1;

        public FoodViewModel()
        {

        }

        public FoodViewModel(List<Food> foodList)
        {
            _conn = new SQLiteConnection(Settings.DBPATH, SQLiteOpenFlags.ReadWrite, false);
            Foods = new ObservableCollection<Food>(foodList);
            BtnEditFood = new RelayCommand(EditFood);
            BtnDeleteFood = new RelayCommand(DeleteFood);
            BtnAddFood = new RelayCommand(SubmitFood);
        }

        private void EditFood(object obj)
        {
            if (SelectedFoodItem == null) return;

            FoodId = SelectedFoodItem.FoodID;
            FoodItem = SelectedFoodItem.FoodItem;
        }

        private void DeleteFood(object obj)
        {
            if (SelectedFoodItem == null) return;
            Foods.Remove(SelectedFoodItem);
        }

        private void SubmitFood(object obj)
        {
            Food fm = Foods.FirstOrDefault(x => x.FoodID == FoodId);
            if (fm != null)
            {
                fm.FoodItem = FoodItem;
                CollectionViewSource.GetDefaultView(Foods).Refresh();
                FoodItem = string.Empty;
                FoodId = 0;
                return;
            }

            Food food = new Food()
            {
                FoodID = indexId,
                FoodItem = FoodItem
            };

            Foods.Add(food);
            FoodItem = string.Empty;
            FoodId = 0;
            indexId--;
        }
    }
}
