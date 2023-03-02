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
using Mopups.Services;
using IMP_reseni.Controls;

namespace IMP_reseni.ViewModels
{
    public class ModifySubCategoryViewModel : INotifyPropertyChanged
    {
        public List<string> ListOfCategory { get; set; }
        public ObservableCollection<string> ListOfSubCategory { get; set; }


        public ICommand ModifyCommand { get; set; }
        public ICommand ChangePictureCommand { get; set; }
        public ICommand ShowPictureCommand { get; set; }
        private string _selectedCategory;

        public string SelectedCategory
        {
            set
            {
                SetProperty(ref _selectedCategory, value);
                if (value != "")
                {
                    ListOfSubCategory.Clear();
                    foreach (var item in saveholder.FindCategoryByName(value).GetSubCategoriesNames())
                    {
                        ListOfSubCategory.Add(item);
                    }
                }
            }

            get { return _selectedCategory; }
        }

        private string _selectedSubCategory;

        public string SelectedSubCategory 
        {
            get { return _selectedSubCategory; }
            set 
            { 
                SetProperty(ref _selectedSubCategory, value);
                if(value!= "") 
                {
                    Text=value;
                }
            }
        }

        private string _text;
        public string Text
        {
            set 
            {
                SetProperty(ref _text, value);
                if (previusName == null)
                {
                    previusName = value;                
                }
            }
            get { return _text; }
        }

        private string _pictureButtonText = "Změnit obrázek";
        public string PictureButtonText
        {
            set { SetProperty(ref _pictureButtonText, value); }
            get { return _pictureButtonText; }
        }

        private string ImageUrl;
        private SaveHolder saveholder;
        private string previusName = null;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ModifySubCategoryViewModel(SaveHolder saveholder)
        {
            this.saveholder = saveholder;
            Text = "";
            List<string> list = new List<string>(saveholder.GetCategoriesNames());
            list.Sort();
            ListOfCategory = new List<string>(list);
            ListOfSubCategory = new ObservableCollection<string>();
            ModifyCommand = new Command<string>(
           (string name) =>
            {
                SubCategory subCategory = new SubCategory();
                Category category = new Category();

                category = saveholder.FindCategoryByName(SelectedCategory);
                subCategory = category.FindSubCategoryByName(SelectedSubCategory);
                subCategory.Name = name;
                if (!category.ExistSubCategoryByName(name))
                {
                    saveholder.ModifySubCategory(category, subCategory);
                    saveholder.Save();
                    Toast.Make("Podkategorie změněna").Show();
                    Text = "";
                    //SelectedCategory = null;
                    SelectedSubCategory = null;
                    list = new List<string>(saveholder.FindCategoryByName(SelectedCategory).GetSubCategoriesNames());
                    list.Sort();
                    ListOfSubCategory.Clear();
                    foreach (var Item in list)
                    {
                        ListOfSubCategory.Add(Item);
                    }
                }
                else
                {
                    Toast.Make("Podkategorie s tímto jménem již existuje").Show();
                }

            });
            ShowPictureCommand = new Command<string>(
          canExecute: (string SelectedItem) =>
          {
              if (SelectedItem != null && SelectedItem != "")
              {
                  return true;
              }
              else
              {
                  return false;
              }
          },
          execute: async (string SelectedItem) =>
          {
              if (ImageUrl != null)
              {
                  await MopupService.Instance.PushAsync(new ShowPicturePopup(ImageUrl));
              }
              else 
              {
                  await Toast.Make("Obrázek nebyl nalezen").Show();
              }
          });

            ChangePictureCommand = new Command<string>(
             canExecute: (string SelectedItem) =>
             {
                 if (SelectedItem != null && SelectedItem != "")
                 {
                     return true;
                 }
                 else
                 {
                     return false;
                 }
             },
            execute: async (string SelectedItem) =>
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
