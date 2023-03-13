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
using Microsoft.Maui.Controls.Handlers;

namespace IMP_reseni.ViewModels
{
    public class NewSubCategoryViewModel : INotifyPropertyChanged
    {
        public Page Page { get; set; }

        public List<string> ListOfCategory { get; set; }
        public ICommand CreateCommand { get; set; }
        public ICommand UploadOPictureCommand { get; set; }

        //public event EventHandler SelectedIndexChanged;

        private string _selectedItem;
        public string SelectedItem {
            set { SetProperty(ref _selectedItem, value); }
            get { return _selectedItem; }
        }

        string _text;
        public string Text
        {
            set { SetProperty(ref _text, value); }
            get { return _text; }
        }

        private string _pictureButtonText = "Nahrát obrázek";
        public string PictureButtonText
        {
            set { SetProperty(ref _pictureButtonText, value); }
            get { return _pictureButtonText; }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private string ImageUrl;
        public NewSubCategoryViewModel(SaveHolder saveholder, FileHandler fileHandler)
        {
            ListOfCategory = new List<string>(saveholder.GetCategoriesNames());

            //SelectedIndexChanged += (object sender, EventArgs e) => 
            //{

            //};

            Text = "";
            CreateCommand = new Command<bool>(
            canExecute:(bool CanBeEnabled) =>
            {
                return CanBeEnabled;
            },
            execute:async (bool CanBeEnabled) =>
            {
                string name = Text;
                if (ImageUrl == "" || ImageUrl == null)
                {
                    bool answer = await Page.DisplayAlert("Pozor", "Opravdu si přejete pokračovat bez přidaného obrázku?", "Ano", "Ne");
                    if (answer == false)
                    {
                        return;
                    }
                }
                SubCategory newSubCategory = new SubCategory();
                newSubCategory.Name = name;
                newSubCategory.ImageUrl = ImageUrl;
                Category category = saveholder.FindCategoryByName(SelectedItem);
                int categoryId= category.Id;
                if (!category.ExistSubCategoryByName(name))
                {
                    if (File.Exists(ImageUrl))
                    {
                        newSubCategory.ImageUrl = fileHandler.SaveImage(ImageUrl);
                    }
                    saveholder.AddSubCategory(categoryId, newSubCategory);
                    saveholder.Save();
                    await Toast.Make("Nová PodKategorie vytvořena").Show();
                    Text = "";
                }
                else
                {
                   await Toast.Make("PodKategorie s tímto jménem již existuje").Show();
                }

            });

            UploadOPictureCommand = new Command<bool>(
            canExecute: (bool CanBeEnabled) =>
            {
                return CanBeEnabled;
            },
            execute:async (bool CanBeEnabled) =>
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
