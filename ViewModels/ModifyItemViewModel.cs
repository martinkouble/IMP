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
using IMP_reseni.Services;
using Mopups.Services;
using IMP_reseni.Controls;

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
        public ICommand ChangePictureCommand { get; set; }
        public ICommand ShowPictureCommand { get; set; }

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
                if (value!=null && value!="")
                {
                    if (ListOfItem.Count != 0)
                    {
                        ListOfItem.Clear();
                    }
                    foreach (var item in saveholder.FindCategoryByName(SelectedCategory).FindSubCategoryByName(value).GetItemNames(true))
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
                    if(previusName==null)
                    {
                        previusName = value;
                    }
                    Text = value;
                    Items item = new Items();              
                    item =saveholder.FindCategoryByName(SelectedCategory).FindSubCategoryByName(SelectedSubCategory).FindItemByName(value);
                    itemId = item.Id;
                    Count = item.Stock.ToString();
                    BuyPrice=item.BuyCost.ToString();
                    SellPrice=item.SellCost.ToString();
                    DisableCheck = item.Disabled;
                    SorChecked = item.SoR;
                    if (saveholder.ExistSupplier(item.SupplierId))
                    {
                        SelectedSupplier = saveholder.FindSupplier(item.SupplierId).Name;
                    }
                    else
                    {
                        SelectedSupplier = null;
                    }
                    ImageUrl = item.ImageUrl;
                    originalSellCost = item.SellCost;
                    originalBuyCost = item.BuyCost;
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

        private double originalSellCost;
        private double originalBuyCost;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _pictureButtonText = "Změnit obrázek";
        public string PictureButtonText
        {
            set { SetProperty(ref _pictureButtonText, value); }
            get { return _pictureButtonText; }
        }
        private string ImageUrl;
        private SaveHolder saveholder;
        private BasketHolder basketHolder;
        private string previusName=null;
        public ModifyItemViewModel(SaveHolder Saveholder,BasketHolder basketHolder,FileHandler fileHandler)
        {
            saveholder = Saveholder;
            this.basketHolder = basketHolder;
            ListOfSubCategory = new ObservableCollection<string>();
            ListOfItem = new ObservableCollection<string>();
            //BindingBase.EnableCollectionSynchronization(ListOfSubCategory,null, ObservableCollectionCallback);
            //BindingBase.EnableCollectionSynchronization(ListOfItem, null, ObservableCollectionCallback);

            ListOfSupplier = new List<string>(saveholder.GetSupplierNames());
            DefaultedValues();
            List<string> list = new List<string>(saveholder.GetCategoriesNames());
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
            list =new List<string>(saveholder.GetSupplierNames());
            list.Sort();
            ListOfSupplier = new List<string>(list);
            ModifyCommand = new Command<bool>(
          canExecute:(bool CanBeEnabled) => 
          {
              return CanBeEnabled;
          },
          execute:(bool CanBeEnabled) =>
          {
              string name = Text;
              SubCategory subCategory = new SubCategory();
              Category category = new Category();
              Items item=new Items();
              category = saveholder.FindCategoryByName(SelectedCategory);
              subCategory = category.FindSubCategoryByName(SelectedSubCategory);
              item = subCategory.FindItemByName(name);
              if (!basketHolder.ExistItem(item))
              {
                  if (!subCategory.ExistItemByName(name) || name == previusName)
                  {
                      if (File.Exists(item.ImageUrl))
                      {
                          File.Delete(item.ImageUrl);
                      }
                      item.Id = itemId;
                      if (File.Exists(ImageUrl))
                      {
                          item.ImageUrl = fileHandler.SaveImage(ImageUrl);
                      }

                      item.CreateWithStock(name, DisableCheck, Convert.ToDouble(BuyPrice), Convert.ToDouble(SellPrice),item.Stock, SorChecked, saveholder.FindSupplierByName(SelectedSupplier).Id, category.Id, subCategory.Id);

                      if (originalSellCost != Convert.ToDouble(SellPrice) || originalBuyCost != Convert.ToDouble(BuyPrice))
                      {
                          StockUpItem stockUp = new StockUpItem();
                          stockUp.CategoryId = category.Id;
                          stockUp.SubCategoryId = subCategory.Id;
                          stockUp.ItemId = itemId;
                          stockUp.ChangedSellCost = Convert.ToInt32(SellPrice);
                          stockUp.ChangedBuyCost = Convert.ToInt32(BuyPrice);
                          stockUp.CompleteChangeCost(category.Id, subCategory.Id, itemId);
                      }

                      saveholder.ModifyItem(category, subCategory, item);
                      saveholder.Save();
                      Toast.Make("Položka změněna").Show();
                      DefaultedValues();
                      SelectedItem = null;
                      previusName = null;
                      //list = new List<string>(App.saveholder.FindCategoryByName(SelectedCategory).FindSubCategoryByName(SelectedSubCategory).GetItemNames());
                      //list.Sort();
                      //ListOfItem.Clear();
                      //foreach (var Item in list)
                      //{
                      //    ListOfItem.Add(Item);
                      //}
                  }
                  else
                  {
                      Toast.Make("Položka s tímto jménem již existuje").Show();
                  }
              }
              else
              {
                  Toast.Make("Nelze měnit položku v košíku").Show();
              }


          });

            ShowPictureCommand = new Command<string>(
            canExecute: (string SelectedSubCategory) =>
            {
                if (SelectedSubCategory != null && SelectedSubCategory != "")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            },
            execute: async(string SelectedSubCategory) =>
            {
                if (ImageUrl!=null)
                {
                    await MopupService.Instance.PushAsync(new ShowPicturePopup(ImageUrl));
                }
                else
                {
                    await Toast.Make("Obrázek nebyl nalezen").Show();
                }
            });

            ChangePictureCommand = new Command<string>(
             canExecute: (string SelectedSubCategory) =>
             {
                 if (SelectedSubCategory != null && SelectedSubCategory != "")
                 {
                     return true;
                 }
                 else
                 {
                     return false;
                 }
             },
            execute: async (string SelectedSubCategory) =>
            {
                ImageUrl = await PickAndShow(PickOptions.Images);
                if (ImageUrl != null)
                {
                    PictureButtonText = "Změněno";
                }
                else
                {
                    PictureButtonText = "Změnit obrázek";
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
        public async Task<string> PickAndShow(PickOptions options)
        {
            try
            {
                var result = await FilePicker.Default.PickAsync(options);
                return result.FullPath;
            }
            catch (Exception)
            {
                await Toast.Make("Obrázek nebyl vybrán").Show();
            }
            return null;
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
