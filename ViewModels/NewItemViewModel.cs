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
    public class NewItemViewModel:INotifyPropertyChanged
    {
        public List<string> ListOfCategory { get; set; }
        public ObservableCollection<string> ListOfSubCategory { get; set; }

        public ICommand CreateCommand { get; set; }
        //public event EventHandler SelectedIndexChanged;

        private string _selecteCategory;

        public string SelectedCategory
        {
            set 
            {
                SetProperty(ref _selecteCategory, value);
                if (value != "")
                {
                    ListOfSubCategory.Clear();
                    foreach (var item in App.saveholder.FindCategoryByName(value).GetSubCategoriesNames())
                    {
                        ListOfSubCategory.Add(item);
                    }
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
        public NewItemViewModel()
        {
            ListOfCategory = new List<string>(App.saveholder.GetCategoriesNames());
            ListOfSubCategory = new ObservableCollection<string>();
            Text = "";
            CreateCommand = new Command<string>(
            (string name) =>
            {
                Items newItem = new Items();
                newItem.Name = name;
                int categoryId = App.saveholder.FindCategoryByName(SelectedCategory).Id;
                int subCategoryId = App.saveholder.FindCategory(categoryId).FindSubCategoryByName(SelectedSubCategory).Id;
                App.saveholder.AddItem(categoryId, subCategoryId, newItem);
                App.saveholder.Save();
                Toast.Make("Nová položka vytvořena").Show();
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
