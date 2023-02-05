using IMP_reseni.Models;
using IMP_reseni.Services;
using IMP_reseni.Views;
using Microsoft.Maui.Controls.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IMP_reseni.ViewModels
{
    public class StockUpViewModel : INotifyPropertyChanged
    {
        private List<Category> source = App.saveholder.Inventory;

        public ICommand NavigateCommand { get; private set; }
        public ICommand ItemSelect { get; private set; }
        public ICommand NavigateCollectionCommand { get; private set; }
        public ICommand ItemSelectedCommand { get; private set; }
        public ObservableCollection<object> ItemsList { get; set; }

        private List<object> CurrentSelection = null;
        private object SelectedCategory = null;

        private object _selectedItem;
        public object SelectedItem
        {
            set { SetProperty(ref _selectedItem, value); }
            get { return _selectedItem; }
        }


        private string _typeOfItems;
        public string TypeOfItems
        {
            set { SetProperty(ref _typeOfItems, value); }
            get { return _typeOfItems; }
        }

        public StockUpViewModel(ContentPage _page) 
        {

            TypeOfItems = "Kategorie";

            ItemsList = new ObservableCollection<object>(App.saveholder.Inventory);

            NavigateCollectionCommand = new Command<string>(
            (string Direction) =>
            {
                if (Direction == "Back")
                {
                    if (CurrentSelection != null && CurrentSelection[0] != null)
                    {
                        var _category = (Category)SelectedCategory;
                        Type _type = CurrentSelection[0].GetType();
                        if (_type == typeof(Category))
                        {
                            addToList<Category>(source.ToList());
                            CurrentSelection[0] = null;
                            TypeOfItems = "Kategorie";
                        }
                        else if ((_type == typeof(SubCategory)) || (_type == typeof(Items)))
                        {
                            addToList<SubCategory>(_category.SubCategories);
                            CurrentSelection[0] = SelectedCategory;
                            TypeOfItems = "Podkategorie";
                        }
                        SelectedItem = null;                   
                    }
                }
            });

            ItemSelectedCommand = new Command<SelectionChangedEventArgs>(
            (SelectionChangedEventArgs e) =>
            {
                if (e.CurrentSelection.Count != 0)
                {    
               CurrentSelection = (List<object>)e.CurrentSelection;
                }
            });

            ItemSelect = new Command<object>(
            async (object SelectedItem) =>
            {
                if (SelectedItem != null)
                {

                    Type _type = SelectedItem.GetType();
                    if (_type == typeof(Category))
                    {
                        var _category = (Category)SelectedItem;
                        addToList(_category.SubCategories);
                        TypeOfItems = "Podkategorie";
                        SelectedCategory = SelectedItem;
                    }
                    else if (_type == typeof(SubCategory))
                    {
                        var _subCategory = (SubCategory)SelectedItem;
                        addToList(_subCategory.Items);
                        TypeOfItems = "Položky";

                    }
                    else if (_type == typeof(Items))
                    {
                        var Item = SelectedItem;
                        await _page.Navigation.PushAsync(new ItemStockUp((Items)SelectedItem));
                    }
                }
            });
        }
        private void Page_Resumed(object sender, EventArgs e)
        {
            TypeOfItems = "Kategorie";
            CurrentSelection = null;
            filter("");
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

        void filter(string Text)
        {
            Text = Text.ToLowerInvariant();
            var _filteredItems = source.Where(Item => Item.Name.ToLower().Contains(Text));
            if (string.IsNullOrEmpty(Text))
            {
                _filteredItems = source.ToList();
                ItemsList.Clear();
            }
            foreach (var Item in source)
            {
                if (!_filteredItems.Contains(Item))
                {
                    ItemsList.Remove(Item);
                }
                else if (!ItemsList.Contains(Item))
                {
                    ItemsList.Add(Item);
                }
            }
        }
        void addToList<T>(List<T> list)
        {
            if (list != null)
            {
                ItemsList.Clear();
                foreach (var Item in list)
                {
                    ItemsList.Add(Item);
                }
            }
        }
    }
}
