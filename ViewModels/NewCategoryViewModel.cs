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


namespace IMP_reseni.ViewModels
{
    public class NewCategoryViewModel: INotifyPropertyChanged
    {

        public ICommand CreateCommand { get; set; }
        public ICommand UploadOPictureCommand { get; set; }

        private string _text;
        public string Text
        {
            set { SetProperty(ref _text, value); }
            get { return _text; }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public NewCategoryViewModel() 
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

                App.saveholder.AddCategory(newCategory);
                App.saveholder.Save();
                Toast.Make("Nová kategorie vytvořena").Show();
                Text = "";
            });

            UploadOPictureCommand = new Command(
            async () =>
                {
                    await PickAndShow(PickOptions.Images);
                });

        }

        public async Task<string> PickAndShow(PickOptions options)
        {
            try
            {
                var result = await FilePicker.Default.PickAsync(options);
                return result.FullPath;
            }
            catch (Exception ex)
            {
                // The user canceled or something went wrong
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
