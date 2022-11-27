using IMP_reseni.Models;
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
    public class MainPageViewModel : INotifyPropertyChanged
    {
        //        await Navigation.PushAsync(new Views.Login());
        //public IList<Items> Items { get; private set; }
        private Category[] source = new[] {
            new Category
                {
                    Name = "A",
                    ImageUrl = "https://img.darkoviny.cz/images/6657.jpg",
                    SubCategories=new List<SubCategory>(){ new SubCategory { Name="SS1"} }
                },
            new Category
                {
                    Name = "B",
                    ImageUrl = "https://img.darkoviny.cz/images/6657.jpg",
                    SubCategories=new List<SubCategory>(){ new SubCategory { Name="SS2"} }

                },
             new Category
                {
                    Name = "C",
                    ImageUrl = "https://img.darkoviny.cz/images/6657.jpg",
                    SubCategories=new List<SubCategory>(){ new SubCategory { Name="SS3"} }
                },

        };
        public ICommand NavigateCommand { get; private set; }
        public ICommand PerformSearch { get; private set; }
        public ICommand ItemSelect { get; private set; }
        public ICommand NavigateCollectionCommand { get; private set; }
        public ICommand ItemSelectedCommand { get; private set; }


        private string _typeOfItems;
        public string TypeOfItems
        {
            set { SetProperty(ref _typeOfItems, value); }
            get { return _typeOfItems; }
        }

        public ObservableCollection<object> ItemsList { get;  set; }

        //
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
        public MainPageViewModel()
        {

        }
        public MainPageViewModel(ContentPage _page)
        {
            TypeOfItems="Kategorie";

            ItemsList = new ObservableCollection<object>(source);

            NavigateCommand = new Command(
            async () =>
            {
                //Page page = (Page)Activator.CreateInstance(pageType);
                await _page.Navigation.PushAsync(new Login());

            });

            PerformSearch = new Command<string>(
            (string Text) =>
            {
                filter(Text);      
            });

            NavigateCollectionCommand = new Command<string>(
            (string Direction) =>
            {
                if(Direction == "Back")
                {

                }
                else if(Direction == "Forward")
                {

                }
            });

            ItemSelectedCommand = new Command<object>(
           (object Direction) =>
           {

           });
            ItemSelect =new Command<object>(
             (object SelectedItem) =>
            {
                Type _type = SelectedItem.GetType();
                if (_type == typeof(Category))
                {
                    var _category = (Category)SelectedItem;
                    addToList(_category.SubCategories);
                    TypeOfItems = "Podkategorie";
                }
                else if(_type == typeof(SubCategory))
                {
                    var _subCategory = (SubCategory)SelectedItem;
                    addToList(_subCategory.Items);
                    TypeOfItems = "Položky";

                }
                else if(_type == typeof(Items))
                {
                    var Item = SelectedItem;

                }
            });

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
            if(list!=null)
            {
                ItemsList.Clear();
                foreach(var Item in list)
                {
                    ItemsList.Add(Item);
                }
            }
        }
        /*
            Items = new ObservableCollection<Items>();
            for (int i = 0; i <= 20; i++)
            {
                Items.Add(new Items
                {
                    Name = "NIC",
                    ImageUrl = "https://img.darkoviny.cz/images/6657.jpg",
                });
            }
            */



    }
}
