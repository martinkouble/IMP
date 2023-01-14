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
using System.Collections;

namespace IMP_reseni.ViewModels
{
    public class ModifyItemViewModel : INotifyPropertyChanged
    {
        public List<string> ListOfCategory { get; set; }
        public List<string> ListOfSupplier { get; set; }

        public ObservableCollection<string> ListOfSubCategory { get; set; }

        public ObservableCollection<string> ListOfItem { get; set; }

        //public ICommand AddCommand { get; set; }
        //public ICommand SubtractCommand { get; set; }
        public ICommand ModifyCommand { get; set; }

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
                        foreach (var item in App.saveholder.FindCategoryByName(value).GetSubCategoriesNames())
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
                if (value!=null && value!="")
                {
                    if (ListOfItem.Count != 0)
                    {
                        ListOfItem.Clear();
                    }
                    foreach (var item in App.saveholder.FindCategoryByName(SelectedCategory).FindSubCategoryByName(value).GetItemNames())
                    {
                        ListOfItem.Add(item);
                    }
                }
            }
            get { return _selectedSubCategory; }

        }

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
                    item =App.saveholder.FindCategoryByName(SelectedCategory).FindSubCategoryByName(SelectedSubCategory).FindItemByName(value);
                    Count = item.Stock.ToString();
                    BuyPrice=item.BuyCost.ToString();
                    SellPrice=item.SellCost.ToString();
                    DisableCheck = item.Disabled;
                    SorChecked = item.SoR;
                    SelectedSupplier = App.saveholder.FindSupplier(item.SupplierId).ToString();
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


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ModifyItemViewModel()
        {
            ListOfSubCategory = new ObservableCollection<string>();
            ListOfItem = new ObservableCollection<string>();
            //BindingBase.EnableCollectionSynchronization(ListOfSubCategory,null, ObservableCollectionCallback);
            //BindingBase.EnableCollectionSynchronization(ListOfItem, null, ObservableCollectionCallback);

            ListOfSupplier = new List<string>(App.saveholder.GetSupplierNames());
            DefaultedValues();
            List<string> list = new List<string>(App.saveholder.GetCategoriesNames());
            list.Sort();
            ListOfCategory = new List<string>(list);

            //void ObservableCollectionCallback(IEnumerable collection, object context, Action accessMethod, bool writeAccess)
            //{
            //    // `lock` ensures that only one thread access the collection at a time
            //    lock (collection)
            //    {
            //        accessMethod?.Invoke();
            //    }
            //}
            list =new List<string>(App.saveholder.GetSupplierNames());
            list.Sort();
            ListOfSupplier = new List<string>(list);
            ModifyCommand = new Command<string>(
          (string name) =>
          {
              SubCategory subCategory = new SubCategory();
              Category category = new Category();
              Items item=new Items();
              category = App.saveholder.FindCategoryByName(SelectedCategory);
              subCategory = category.FindSubCategoryByName(SelectedSubCategory);
              item = subCategory.FindItemByName(name);
              App.saveholder.ModifyItem(category, subCategory, item);
              App.saveholder.Save();
              Toast.Make("Položka změněna").Show();
              DefaultedValues();
              SelectedItem = null;
              list = new List<string>(App.saveholder.FindCategoryByName(SelectedCategory).FindSubCategoryByName(SelectedSubCategory).GetItemNames());
              list.Sort();
              ListOfItem.Clear();
              foreach (var Item in list)
              {
                  ListOfItem.Add(Item);
              }
          });
            /*
            AddCommand = new Command<string>(
                 canExecute: (string Count) =>
                 {
                     if (Convert.ToInt32(Count) >= MaxCount)
                     {
                         return false;
                     }
                     else
                     {
                         return true;
                     }
                 },
                 execute: (string Count) =>
                 {
                     this.Count = (Convert.ToInt32(Count) + 1).ToString();
                 });

            SubtractCommand = new Command<string>(
                canExecute: (string Count) =>
                {
                    if (Convert.ToInt32(Count) <= 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                },
                execute: (string Count) =>
                {
                    this.Count = (Convert.ToInt32(Count) - 1).ToString();
                });*/


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
