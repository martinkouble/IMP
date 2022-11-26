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


        public ObservableCollection<object> ItemsList { get;  set; }

        //
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainPageViewModel()
        {

        }
        public MainPageViewModel(ContentPage _page)
        {
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

            ItemSelect=new Command<object>(
             (object SelectedItem) =>
            {
                Type _type = SelectedItem.GetType();
                if (_type == typeof(Category))
                {
                    var category = (Category)SelectedItem;
                    addToList(category.SubCategories);
                }
                else if(_type == typeof(SubCategory))
                {
                    var SubCategory = (SubCategory)SelectedItem;
                    addToList(SubCategory.Items);

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
