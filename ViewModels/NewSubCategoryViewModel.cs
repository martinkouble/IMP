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
    public class NewSubCategoryViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public List<string> ListOfCategory { get; set; }
        public ICommand CreateCommand { get; set; }
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
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public NewSubCategoryViewModel()

        {
            ListOfCategory = new List<string>(App.saveholder.GetCategoriesNames());

            //SelectedIndexChanged += (object sender, EventArgs e) => 
            //{

            //};

            Text = "";
            CreateCommand = new Command<string>(
            (string name) =>
            {
                SubCategory newSubCategory = new SubCategory();
                newSubCategory.Name = name;
                int categoryId= App.saveholder.FindCategoryByName(SelectedItem).Id;
                App.saveholder.AddSubCategory(categoryId, newSubCategory);
                App.saveholder.Save();
                Toast.Make("Nová PodKategorie vytvořena").Show();
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
