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
    public class ModifyCategoryViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> ListOfCategory { get; set; }

        public ICommand ModifyCommand { get; set; }

        private string _selectedCategory;

        public string SelectedCategory
        {
            set 
            { 
                SetProperty(ref _selectedCategory, value);
                if (value != "")
                {
                    Text = value;
                }
            }

            get { return _selectedCategory; }
        }

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
        public ModifyCategoryViewModel(SaveHolder saveholder)
        {
            //Text = "";
            List<string> list = new List<string>(saveholder.GetCategoriesNames());
            list.Sort();
            ListOfCategory =new ObservableCollection<string>(list);

            ModifyCommand = new Command<string>(
            canExecute: (string name) =>
            {
                if (name == "")
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
                saveholder.ModifyCategory(category);
                saveholder.Save();
                Toast.Make("kategorie změněna").Show();
                Text = "";
                SelectedCategory = null;
                List<string> list = new List<string>(saveholder.GetCategoriesNames());
                list.Sort();
                ListOfCategory.Clear();
                foreach (var Item in list)
                {
                    ListOfCategory.Add(Item);
                }
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
