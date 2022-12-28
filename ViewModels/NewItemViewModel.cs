using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using IMP_reseni.Models;
using CommunityToolkit.Maui.Alerts;
using System.Collections.ObjectModel;


namespace IMP_reseni.ViewModels
{
    public class NewItemViewModel:INotifyPropertyChanged
    {
        public List<string> ListOfCategory { get; set; }
        public List<string> ListOfSupplier { get; set; }

        public ObservableCollection<string> ListOfSubCategory { get; set; }

        public ICommand CreateCommand { get; set; }
        //public event EventHandler SelectedIndexChanged;
        
        //Picker
        private string _selecteCategory;
        public string SelectedCategory
        {
            set 
            {
                SetProperty(ref _selecteCategory, value);
                if (value != "")
                {
                    ListOfSubCategory.Clear();
                    foreach (var item in App.saveholder.FindCategoryByName(value).GetSubCategoriesNames())
                    {
                        ListOfSubCategory.Add(item);
                    }
                }
            }
            get { return _selecteCategory; }
        }

        private string _selecteSubCategory;
        public string SelectedSubCategory
        {
            set { SetProperty(ref _selecteSubCategory, value); }
            get { return _selecteSubCategory; }
        }

        private string _selectedSupplier;
        public string SelectedSupplier
        {
            get { return _selectedSupplier; }
            set { SetProperty(ref _selectedSupplier, value);}
        }


        //Entry
        private string _text;
        public string Text
        {
            set { SetProperty(ref _text, value); }
            get { return _text; }
        }

        private string _count;
        public string Count
        {
            get { return _count; }
            set { SetProperty(ref _count, value);}
        }

        private string _buyPrice;
        public string BuyPrice
        {
            get { return _buyPrice; }
            set { SetProperty(ref _buyPrice, value); }
        }

        private string _sellPrice;
        public string SellPrice
        {
            get { return _sellPrice; }
            set { SetProperty(ref _sellPrice, value); }
        }

        //CheckBox
        private bool _disableCheck;
        public bool DisableCheck
        {
            get { return _disableCheck; }
            set { SetProperty(ref _disableCheck, value); }
        }

        private bool _sorCheck;
        public bool SorCheck
        {
            get { return _sorCheck; }
            set { SetProperty(ref _sorCheck, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public NewItemViewModel()
        {
            ListOfCategory = new List<string>(App.saveholder.GetCategoriesNames());
            ListOfSubCategory = new ObservableCollection<string>();
            ListOfSupplier=new List<string>(App.saveholder.GetSupplierNames());
            Text = "";
            CreateCommand = new Command<string>(
            (string name) =>
            {
                Items newItem = new Items();
                //newItem.Name = name;
                newItem.Create(name, DisableCheck, Convert.ToDouble(BuyPrice), Convert.ToDouble(SellPrice), SorCheck, App.saveholder.FindSupplierByName(SelectedSupplier).Id);
                int categoryId = App.saveholder.FindCategoryByName(SelectedCategory).Id;
                int subCategoryId = App.saveholder.FindCategory(categoryId).FindSubCategoryByName(SelectedSubCategory).Id;
                newItem.SellCost = Convert.ToInt32(SellPrice);
                newItem.BuyCost = Convert.ToInt32(BuyPrice);
                newItem.SoR = SorCheck;
                newItem.Stock = Convert.ToInt32(Count);
                newItem.Disabled = DisableCheck;
                newItem.SupplierId = App.saveholder.FindSupplierByName(SelectedSupplier).Id;
                App.saveholder.AddItem(categoryId, subCategoryId, newItem);
                App.saveholder.Save();
                Toast.Make("Nová položka vytvořena").Show();
                Text = "";
            });
        }

  

        bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Object.Equals(storage, value))
                return false;

            storage = value;

            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
