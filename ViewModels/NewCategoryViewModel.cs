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
using IMP_reseni.Services;

namespace IMP_reseni.ViewModels
{
    public class NewCategoryViewModel: INotifyPropertyChanged
    {

        public ICommand CreateCommand { get; set; }
        public ICommand UploadOPictureCommand { get; set; }

        private string _pictureButtonText= "Nahrát obrázek";
        public string PictureButtonText
        {
            set { SetProperty(ref _pictureButtonText, value); }
            get { return _pictureButtonText; }
        }

        private string _text;
        public string Text
        {
            set { SetProperty(ref _text, value); }
            get { return _text; }
        }
        private string ImageUrl;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public NewCategoryViewModel(SaveHolder saveholder) 
        {
            Text = "";
            CreateCommand = new Command<string>(
            canExecute:(string name) =>
            {
                if(name=="")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            },

            execute:(string name) =>
            {
                Category newCategory = new Category();
                newCategory.Name = name;
                if(!saveholder.ExistCategoryByName(name))
                {
                    newCategory.ImageUrl = ImageUrl;
                    saveholder.AddCategory(newCategory);
                    saveholder.Save();
                    Toast.Make("Nová kategorie vytvořena").Show();
                    Text = "";
                    PictureButtonText = "Nahrát obrázek";
                }
                else
                {
                    Toast.Make("Kategorie s tímto jménem již existuje").Show();
                }

            });

            UploadOPictureCommand = new Command<string>(
            canExecute: (string Text) =>
            {
                if (Text!=null && Text!="")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            },
            execute: async (string CanBeEnabled) =>
            {
                ImageUrl = await PickAndShow(PickOptions.Images);
                if (ImageUrl != null)
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
