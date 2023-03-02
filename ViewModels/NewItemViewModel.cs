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
using IMP_reseni.Services;

namespace IMP_reseni.ViewModels
{
    public class NewItemViewModel: INotifyPropertyChanged
    {
        public List<string> ListOfCategory { get; set; }
        public List<string> ListOfSupplier { get; set; }

        public ObservableCollection<string> ListOfSubCategory { get; set; }

        public ICommand UploadOPictureCommand { get; set; }
        public ICommand CreateCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand SubtractCommand { get; set; }

    //public event EventHandler SelectedIndexChanged;

    //Picker
    private string _selecteCategory;
        public string SelectedCategory
        {
            set 
            {
                SetProperty(ref _selecteCategory, value);
                if (value != "" && value !=null)
                {
                    ListOfSubCategory.Clear();
                    foreach (var item in saveholder.FindCategoryByName(value).GetSubCategoriesNames())
                    {
                        ListOfSubCategory.Add(item);
                    }
                }
                else
                {
                    ListOfSubCategory.Clear();
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
            set 
            {            
                SetProperty(ref _count, value);        
            }
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

        private string _pictureButtonText = "Nahrát obrázek";
        public string PictureButtonText
        {
            set { SetProperty(ref _pictureButtonText, value); }
            get { return _pictureButtonText; }
        }
        private string ImageUrl;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private SaveHolder saveholder;
        public NewItemViewModel(SaveHolder saveholder)
        {
            this.saveholder = saveholder;
            ListOfCategory = new List<string>(saveholder.GetCategoriesNames());
            ListOfSubCategory = new ObservableCollection<string>();
            ListOfSupplier=new List<string>(saveholder.GetSupplierNames());
            Text = "";
            AddCommand = new Command<string>(
            (string count) =>
            {
                if (count!="")
                {
                    Count = (Convert.ToInt32(count) + 1).ToString();
                }
                else
                {
                    Count = "0";
                }
            });

            SubtractCommand = new Command<string>(
            canExecute: (string count) =>
            {
                if (count !="" && Convert.ToInt32(count)<=0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            },
            execute: (string count) =>
            {
                if (count != "")
                {
                    Count = (Convert.ToInt32(count) - 1).ToString();
                }
            });

            UploadOPictureCommand = new Command(
            async () =>
            {
                ImageUrl=await PickAndShow(PickOptions.Images);
                if (ImageUrl!=null)
                {
                    PictureButtonText = "Nahráno";
                }
                else
                {
                    PictureButtonText = "Nahrát obrázek";
                }
            });

            CreateCommand = new Command<string>(
            (string name) =>
            {
                Items newItem = new Items();
                //newItem.Name = name;
                Category category = saveholder.FindCategoryByName(SelectedCategory);
                int categoryId = category.Id;
                SubCategory subCategory = saveholder.FindCategory(categoryId).FindSubCategoryByName(SelectedSubCategory);
                int subCategoryId = subCategory.Id;
                if (!subCategory.ExistItemByName(name))
                {
                    newItem.Create(name, DisableCheck, Convert.ToDouble(BuyPrice), Convert.ToDouble(SellPrice), SorCheck, saveholder.FindSupplierByName(SelectedSupplier).Id, categoryId, subCategoryId);
                    newItem.SellCost = Convert.ToInt32(SellPrice);
                    newItem.BuyCost = Convert.ToInt32(BuyPrice);
                    newItem.ImageUrl = ImageUrl;
                    newItem.SoR = SorCheck;
                    newItem.Stock = Convert.ToInt32(Count);
                    newItem.Disabled = DisableCheck;
                    newItem.SupplierId = saveholder.FindSupplierByName(SelectedSupplier).Id;

                    //newItem.CategoryId = categoryId;
                    //newItem.SubCategoryId = subCategoryId;

                    saveholder.AddItem(categoryId, subCategoryId, newItem);
                    saveholder.Save();
                    Toast.Make("Nová položka vytvořena").Show();
                    DefaultedValues();
                }
                else
                {
                    Toast.Make("Položka s tímto jménem již existuje").Show();
                }

            });
        }

        private void DefaultedValues()
        {
            SelectedSupplier = "";
            SelectedSubCategory = "";
            SelectedCategory = "";
            Text = "";
            Count = "";
            BuyPrice = "";
            SellPrice = "";
            DisableCheck = false;
            SorCheck = false;
            PictureButtonText = "Nahrát obrázek";
            ImageUrl = null;
        }
        public async Task<string> PickAndShow(PickOptions options)
        {
            try
            {
                var result = await FilePicker.Default.PickAsync(options);
                return result.FullPath;
            }
            catch(Exception)
            {
                await Toast.Make("Obrázek nebyl vybrán").Show();
            }
            return null;
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
