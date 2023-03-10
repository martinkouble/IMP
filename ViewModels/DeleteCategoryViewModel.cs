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
    public class DeleteCategoryViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> ListOfCategory { get; set; }

        public ICommand DeleteCommand { get; set; }

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
        public DeleteCategoryViewModel(SaveHolder saveholder, BasketHolder basketHolder)
        {
            List<string> list = new List<string>(saveholder.GetCategoriesNames());
            list.Sort();
            ListOfCategory = new ObservableCollection<string>(list);

            DeleteCommand = new Command<string>(
            canExecute: (string name) =>
            {
                if (name == "" || name==null)
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
                if (!basketHolder.ExistCategoryOfItem(category.Id))
                {
                    if (File.Exists(category.ImageUrl))
                    {
                        File.Delete(category.ImageUrl);
                    }
                    saveholder.DeleteCategory(category);
                    saveholder.Save();
                    Toast.Make("kategorie smazána").Show();
                    Text = "";
                    SelectedCategory = null;
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
                    Toast.Make("Nelze smazat kategorii položky v košíku").Show();
                }

            });
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
