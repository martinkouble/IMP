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
    public class DeleteSubCategoryViewModel : INotifyPropertyChanged
    {
        public List<string> ListOfCategory { get; set; }
        public ObservableCollection<string> ListOfSubCategory { get; set; }


        public ICommand DeleteCommand { get; set; }

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
                if (value != "")
                {
                    Text = value;
                }
            }
        }

        private string _text;
        public string Text
        {
            set { SetProperty(ref _text, value); }
            get { return _text; }
        }
        private SaveHolder saveholder;

        public DeleteSubCategoryViewModel(SaveHolder saveholder, BasketHolder basketHolder)
        {
            this.saveholder = saveholder;
            Text = "";
            List<string> list = new List<string>(saveholder.GetCategoriesNames());
            list.Sort();
            ListOfCategory = new List<string>(list);
            ListOfSubCategory = new ObservableCollection<string>();
            DeleteCommand = new Command<string>(
           (string name) =>
           {
               SubCategory subCategory = new SubCategory();
               Category category = new Category();

               category = saveholder.FindCategoryByName(SelectedCategory);
               subCategory = category.FindSubCategoryByName(SelectedSubCategory);
               if (!basketHolder.ExistSubCategoryOfItem(subCategory.Id))
               {
                   saveholder.DeleteSubCategory(category, subCategory);
                   saveholder.Save();
                   Toast.Make("Podkategorie smazána").Show();
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
                   Toast.Make("Nelze smazat podkategorii položky v košíku").Show();
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
