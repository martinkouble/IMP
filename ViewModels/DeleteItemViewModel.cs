using CommunityToolkit.Maui.Alerts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using IMP_reseni.Services;

namespace IMP_reseni.ViewModels
{
    public class DeleteItemViewModel : INotifyPropertyChanged
    {
        public List<string> ListOfCategory { get; set; }
        public List<string> ListOfSupplier { get; set; }

        public ObservableCollection<string> ListOfSubCategory { get; set; }

        public ObservableCollection<string> ListOfItem { get; set; }

        public ICommand DeleteCommand { get; set; }

        //Pickers' selected items
        private string _selectedCategory;
        public string SelectedCategory
        {
            set
            {
                SetProperty(ref _selectedCategory, value);
                if (value != null && value != "")
                {
                    if (ListOfSubCategory.Count != 0)
                    {
                        ListOfSubCategory.Clear();
                    }
                    if (ListOfSubCategory.Count == 0)
                    {
                        foreach (var item in saveholder.FindCategoryByName(value).GetSubCategoriesNames())
                        {
                            ListOfSubCategory.Add(item);
                        }
                    }
                }

            }

            get { return _selectedCategory; }
        }

        private string _selectedSubCategory;
        public string SelectedSubCategory
        {
            set
            {
                SetProperty(ref _selectedSubCategory, value);
                if (value != null && value != "")
                {
                    if (ListOfItem.Count != 0)
                    {
                        ListOfItem.Clear();
                    }
                    foreach (var item in saveholder.FindCategoryByName(SelectedCategory).FindSubCategoryByName(value).GetItemNames())
                    {
                        ListOfItem.Add(item);
                    }
                }
            }
            get { return _selectedSubCategory; }

        }

        private int itemId;

        private string _selectedItem;
        public string SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                SetProperty(ref _selectedItem, value);
                if (value != null && value != "")
                {
                    Text = value;
                    Items item = new Items();
                    item = saveholder.FindCategoryByName(SelectedCategory).FindSubCategoryByName(SelectedSubCategory).FindItemByName(value);
                    itemId = item.Id;
                    Count = item.Stock.ToString();
                    BuyPrice = item.BuyCost.ToString();
                    SellPrice = item.SellCost.ToString();
                    DisableCheck = item.Disabled;
                    SorChecked = item.SoR;
                    SelectedSupplier = saveholder.FindSupplier(item.SupplierId).Name;
                }
            }
        }

        private string _selectedSupplier;
        public string SelectedSupplier
        {
            get { return _selectedSupplier; }
            set { SetProperty(ref _selectedSupplier, value); }
        }

        //Entries texts
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
            set { SetProperty(ref _count, value); }
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

        //CheckBoxs
        private bool _disableCheck;
        public bool DisableCheck
        {
            get { return _disableCheck; }
            set { SetProperty(ref _disableCheck, value); }
        }

        private bool _sorChecked;
        public bool SorChecked
        {
            get { return _sorChecked; }
            set { SetProperty(ref _sorChecked, value); }
        }
        private SaveHolder saveholder;
        public DeleteItemViewModel(SaveHolder Saveholder, BasketHolder basketHolder)
        {
            saveholder = Saveholder;
            ListOfSubCategory = new ObservableCollection<string>();
            ListOfItem = new ObservableCollection<string>();
            ListOfSupplier = new List<string>(saveholder.GetSupplierNames());
            DefaultedValues();
            List<string> list = new List<string>(saveholder.GetCategoriesNames());
            list.Sort();
            ListOfCategory = new List<string>(list);
            list = new List<string>(saveholder.GetSupplierNames());
            list.Sort();
            ListOfSupplier = new List<string>(list);
            DeleteCommand = new Command<string>(
          (string name) =>
          {
              SubCategory subCategory = new SubCategory();
              Category category = new Category();
              Items item = new Items();
              category = saveholder.FindCategoryByName(SelectedCategory);
              subCategory = category.FindSubCategoryByName(SelectedSubCategory);
              item.Id = itemId;
              if (!basketHolder.ExistItem(item))
              {
                  saveholder.DeleteItem(category, subCategory, item);
                  saveholder.Save();
                  Toast.Make("Smazáno").Show();
                  DefaultedValues();
                  SelectedItem = null;
              }
              else
              {
                  Toast.Make("Nelze smazat položku v košíku").Show();
              }
              //item.Create(name, DisableCheck, Convert.ToDouble(BuyPrice), Convert.ToDouble(SellPrice), SorChecked, saveholder.FindSupplierByName(SelectedSupplier).Id, category.Id, subCategory.Id);


              //saveholder.RemoveFromOrderHistory()


          });
        }
        private void DefaultedValues()
        {
            SelectedSubCategory = "";
            SelectedCategory = "";
            SelectedItem = "";
            SelectedSupplier = "";

            Text = "";
            Count = "";
            BuyPrice = "";
            SellPrice = "";
            DisableCheck = false;
            SorChecked = false;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
