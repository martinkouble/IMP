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
    public class ModifyCategoryViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> ListOfCategory { get; set; }

        public ICommand ModifyCommand { get; set; }
        public ICommand ChangePictureCommand { get; set; }
        public ICommand ShowPictureCommand { get; set; }

        private string _selectedCategory;

        public string SelectedCategory
        {
            set 
            { 
                SetProperty(ref _selectedCategory, value);
                if (value != "" && value !=null)
                {
                    Text = value;
                    ImageUrl = saveholder.FindCategoryByName(SelectedCategory).ImageUrl;
                }
            }

            get { return _selectedCategory; }
        }

        private string _text;
        public string Text
        {
            set 
            {
                if (previusName==null)
                {
                    previusName = value;
                }
                SetProperty(ref _text, value); 
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
        private string previusName = null;
        private SaveHolder saveholder;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ModifyCategoryViewModel(SaveHolder saveholder,FileHandler fileHandler)
        {
            this.saveholder = saveholder;

            //Text = "";
            List<string> list = new List<string>(saveholder.GetCategoriesNames());
            list.Sort();
            ListOfCategory =new ObservableCollection<string>(list);

            ModifyCommand = new Command<string>(
            canExecute: (string name) =>
            {
                if ((name == ""||name==null))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            },

            execute: (string name) =>
            {
                Category category = new Category();
                category = saveholder.FindCategoryByName(SelectedCategory);
                category.Name = name;
                if (!saveholder.ExistCategoryByName(name)||previusName==name)
                {
                    if (File.Exists(category.ImageUrl))
                    {
                        File.Delete(category.ImageUrl);
                    }
                    if (File.Exists(ImageUrl))
                    {
                        category.ImageUrl = fileHandler.SaveImage(ImageUrl);
                    }
                    saveholder.ModifyCategory(category);
                    saveholder.Save();
                    Toast.Make("kategorie změněna").Show();
                    Text = "";
                    SelectedCategory = null;
                    previusName = null;
                    List<string> list = new List<string>(saveholder.GetCategoriesNames());
                    list.Sort();
                    ListOfCategory.Clear();
                    foreach (var Item in list)
                    {
                        ListOfCategory.Add(Item);
                    }
                }
                else
                {
                    Toast.Make("Kategorie s tímto jménem již existujes").Show();
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
           execute: async (string SelectedSubCategory) =>
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
             canExecute: (string SelectedCategory) =>
             {
                 if (SelectedCategory != null && SelectedCategory != "")
                 {
                     return true;
                 }
                 else
                 {
                     return false;
                 }
             },
            execute: async (string SelectedCategory) =>
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
