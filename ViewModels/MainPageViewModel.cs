﻿using IMP_reseni.Models;
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
using IMP_reseni.MyPermissions;
namespace IMP_reseni.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        //        await Navigation.PushAsync(new Views.Login());
        //public IList<Items> Items { get; private set; }
        //private List<Category> source=App.saveholder.Inventory;
        private List<Category> source;
        //private Category[] source = new[] {
        //    new Category
        //        {
        //            Name = "A",
        //            ImageUrl = "https://img.darkoviny.cz/images/6657.jpg",
        //            SubCategories=new List<SubCategory>(){ new SubCategory { Name="SS1"} }
        //        },
        //    new Category
        //        {
        //            Name = "B",
        //            ImageUrl = "https://img.darkoviny.cz/images/6657.jpg",
        //            SubCategories=new List<SubCategory>(){ new SubCategory { Name="SS2"} }

        //        },
        //     new Category
        //        {
        //            Name = "C",
        //            ImageUrl = "https://img.darkoviny.cz/images/6657.jpg",
        //            SubCategories=new List<SubCategory>(){ new SubCategory { Name="SS3"} }
        //        },

        //};
        public ICommand NavigateCommand { get; private set; }
        public ICommand NavigateToBasketCommand { get; private set; }
        public ICommand PerformSearch { get; private set; }
        public ICommand ItemSelect { get; private set; }
        public ICommand NavigateCollectionCommand { get; private set; }
        public ICommand ItemSelectedCommand { get; private set; }
        public ICommand CheckedBoxChangeCommand { get; private set; }

        private List<object> CurrentSelection=null;
        //private List<object> PreviousSelection=null;
        private object SelectedCategory=null;


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

        private string _searchText;
        public string SearchText
        {
            set { SetProperty(ref _searchText, value); }
            get { return _searchText; }
        }

        //private bool _SubCategoryCheckBoxIsChecked;
        //public bool SubCategoryCheckBoxIsChecked
        //{
        //    set { SetProperty(ref _SubCategoryCheckBoxIsChecked, value); }
        //    get { return _SubCategoryCheckBoxIsChecked; }
        //}


        //private bool _CategoryCheckBoxIsChecked;
        //public bool CategoryCheckBoxIsChecked
        //{
        //    set { SetProperty(ref _CategoryCheckBoxIsChecked, value); }
        //    get { return _CategoryCheckBoxIsChecked; }
        //}

        //private bool _ItemCheckBoxIsChecked;
        //public bool ItemCheckBoxIsChecked
        //{
        //    set { SetProperty(ref _ItemCheckBoxIsChecked, value); }
        //    get { return _ItemCheckBoxIsChecked; }
        //}


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
        private SaveHolder saveholder;
        private BasketHolder basketholder;
        private ContentPage _page;

        private bool CanSearch = true;
        public MainPageViewModel()
        {
        }
        public MainPageViewModel(ContentPage _page, SaveHolder saveholder, BasketHolder basketholder)
        {
            this._page = _page;
            //SaveHolder h = App.saveholder;
            //source = h.Inventory;
            //h.Save();
            //h.Load();

            //source = saveholder.Inventory;
            //source = saveholder.Inventory.Where(x => x.Disabled == false).ToList();
            source = saveholder.Inventory;

            this.basketholder = basketholder;
            this.saveholder = saveholder;
            TypeOfItems = "Kategorie";

            ItemsList = new ObservableCollection<object>(saveholder.Inventory);

            NavigateCommand = new Command(
            async () =>
            {
                //Page page = (Page)Activator.CreateInstance(pageType);
                await _page.Navigation.PushAsync(new Login());

            });

            PerformSearch = new Command<string>(
            (string Text) =>
            {
                if (Text == "" && CanSearch == true)
                {
                    TypeOfItems = "Kategorie";
                    filter(Text);
                }
                else if (Text != "" && Text != null)
                {
                    TypeOfItems = "Vše";
                    GetAllNames(Text);
                }
                else
                {
                    CanSearch = true;
                }
            });

            NavigateCollectionCommand = new Command<string>(
            canExecute: (string TypeOfItems) =>
            {
                if (TypeOfItems=="Kategorie")
                {
                    return false;
                }
                else if(TypeOfItems!=null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            },
            execute:(string TypeText) =>
            {
                //if(true)
                //{
                if (CurrentSelection != null && CurrentSelection[0] != null)
                {
                    CanSearch=false;
                    SearchText = "";
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
                        if (_category == source.First(x => x.SubCategories.Any(x=>x == CurrentSelection[0])))
                        {
                            addToList<SubCategory>(_category.SubCategories);
                        }
                        else
                        {
                            Category trueCategory = source.First(x => x.SubCategories.Any(x=>x == CurrentSelection[0]));
                            addToList<SubCategory>(trueCategory.SubCategories);
                        }
                        CurrentSelection[0] = SelectedCategory;
                        TypeOfItems = "Podkategorie";
                    }
                    //backArrowClicked = true;
                    //SearchText = "";
                    SelectedItem = null;
                    /*
                    switch (_type)
                    {
                        case typeof(Category):
                            addToList<Category>(source.ToList());
                            CurrentSelection = null;
                            break;

                        case "SubCategory":
                            //addToList<SubCategory>(source.ToList());
                            CurrentSelection = null;
                            //
                            break;
                        case "Items":
                            //
                            break;
                    }*/
                    //}
                    //backArrowClicked = true;
                }
                else
                {
                    addToList<Category>(source.ToList());
                    CurrentSelection = null;
                    TypeOfItems = "Kategorie";
                    SelectedItem = null;
                    SearchText = "";
                }
                //else if(Direction == "Forward")
                //{

                //}


            });

            ItemSelectedCommand = new Command<SelectionChangedEventArgs>(
           (SelectionChangedEventArgs e) =>
           {
               if (e.CurrentSelection.Count!=0)
               {
                   CurrentSelection = (List<object>)e.CurrentSelection;
               }
               //PreviousSelection = (List<object>)e.PreviousSelection;
           });
           
            ItemSelect =new Command<object>(
            async (object SelectedItem) =>
            {
                if (SelectedItem != null) 
                {
                    CanSearch = false;
                    SearchText = "";

                Type _type = SelectedItem.GetType();
                if (_type == typeof(Category))
                {
                    var _category = (Category)SelectedItem;
                    addToList(_category.SubCategories);
                    TypeOfItems = "Podkategorie";
                    SelectedCategory = SelectedItem;
                }
                else if(_type == typeof(SubCategory))
                {
                    var _subCategory = (SubCategory)SelectedItem;
                    //addToList(_subCategory.Items);
                    addItemsToList(_subCategory.Items);
                    TypeOfItems = "Položky";

                }
                else if(_type == typeof(Items))
                {
                    var Item = SelectedItem;
                    await _page.Navigation.PushAsync(new AddItemToBasket((Items)SelectedItem));
                }
                }
            });
            NavigateToBasketCommand = new Command(
            async ()=>
            {
                await _page.Navigation.PushAsync<Basket>();
            });

            _page.Appearing += Page_Resumed;
        }

        private async void Page_Resumed(object sender, EventArgs e)
        {
            source = saveholder.Inventory.Where(x=>x.Disabled==false).ToList();
            TypeOfItems = "Kategorie";
            CurrentSelection = null;
            SelectedItem=null;
            filter("");
            //if (basketholder.Empty==false)
            //{

            //}
            await Permissions.RequestAsync<Permissions.StorageWrite>();
            await Permissions.RequestAsync<Permissions.StorageRead>();
           
        }

        private void filter(string Text)
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
            SelectedItem = null;

        }


        private void GetAllNames(string Text)
        {
            if (string.IsNullOrEmpty(Text))
            {
                var _source = source.ToList();
                ItemsList.Clear();
                foreach (var Item in _source)
                { ItemsList.Add(Item); }
            }
            else
            {
                ItemsList.Clear();
                Text = Text.ToLowerInvariant();
                List<object> _filteredCategories = new List<object>(source
                    .Where(Item => Item.Name.ToLower().Contains(Text)));
                List<object> _filteredSubCategories = new List<object>(source
                    .SelectMany(x => x.SubCategories
                    .Where(x => x.Name.ToLower()
                    .Contains(Text))));
                List<object> _filteredItems = new List<object>(source
                    .SelectMany(x => x.SubCategories
                    .SelectMany(x => x.Items.Where(x => x.Name
                    .ToLower().Contains(Text)))));
                foreach (var Item in _filteredCategories)
                { ItemsList.Add(Item);}

                foreach (var Item in _filteredSubCategories)
                {ItemsList.Add(Item);}

                foreach (var Item in _filteredItems)
                {ItemsList.Add(Item);}
                SelectedItem = null;
            }
        }
        private void addToList<T>(List<T> list)
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
        private void addItemsToList(List<Items> list)
        {
            if(list!=null)
            {
                ItemsList.Clear();
                foreach(var Item in list)
                {
                    if (Item.Disabled!=true)
                    {
                        ItemsList.Add(Item);
                    }
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
