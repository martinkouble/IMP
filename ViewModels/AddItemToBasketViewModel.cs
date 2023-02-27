using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Maui.Alerts;
using IMP_reseni.Models;
using IMP_reseni.Services;

namespace IMP_reseni.ViewModels
{
   public class AddItemToBasketViewModel: INotifyPropertyChanged
    {
        private double ItemsPriceWithDPH;
        private double ItemsPriceWithoutDPH;
        private int MaxCount;

        public ICommand AddCommand { get; set; }

        public ICommand SubtractCommand { get; set; }
        public ICommand AddToBasketCommand { get; set; }


        private string _name;
        public string Name
        {
            set { SetProperty(ref _name, value); }
            get { return _name; }
        }

        private string _count;
        public string Count
        {
            set 
            { 
                SetProperty(ref _count, value);
                PriceWithDPH= Math.Round((Convert.ToInt32(value)*ItemsPriceWithDPH),2).ToString()+" KČ";
                PriceWithoutDPH ="Cena bez DPH "+ Math.Round((Convert.ToInt32(value) * ItemsPriceWithoutDPH),2).ToString()+" KČ";
            }
            get { return _count; }
        }

        private string _imageUrl;
        public string ImageUrl
        {
            set { SetProperty(ref _imageUrl, value); }
            get { return _imageUrl; }
        }

        private string _priceWithDPH;
        public string PriceWithDPH
        {
            set { SetProperty(ref _priceWithDPH, value); }
            get { return _priceWithDPH; }
        }

        private string _priceWithoutDPH;
        public string PriceWithoutDPH
        {
            set { SetProperty(ref _priceWithoutDPH, value); }
            get { return _priceWithoutDPH; }
        }


        public AddItemToBasketViewModel(Items item,Page _page)
        {
            /*
            bool CanAdd = false;
            if (App.basketHolder.Items.Any(x => x.ItemId == item.Id && x.SubCategoryId == item.SubCategoryId && x.CategoryId == item.SubCategoryId))
            {
                OrderItem BasketItem = (OrderItem)App.basketHolder.Items.Where(x => x.ItemId == item.Id && x.SubCategoryId == item.SubCategoryId && x.CategoryId == item.SubCategoryId);
                if ((BasketItem.Amount - item.Stock) == 0)
                {
                    CanAdd = true;
                }
            }
            */

            Count = "0";
            PriceWithoutDPH = "Cena bez DPH 0 KČ";
            PriceWithDPH = "0 KČ";
            ImageUrl= item.ImageUrl;
            Name= item.Name;
            ItemsPriceWithDPH = item.SellCost;
            ItemsPriceWithoutDPH= item.SellCost*0.85;
            MaxCount = item.Stock;
            AddCommand = new Command<string>(
                canExecute: (string Count) =>
                {
                    if (Convert.ToInt32(Count) >= MaxCount)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                },
                execute: (string Count) =>
                {
                    this.Count = (Convert.ToInt32(Count) + 1).ToString();
                });

            SubtractCommand = new Command<string>(
                canExecute: (string Count) =>
                {
                    if (Convert.ToInt32(Count) <= 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                },
                execute: (string Count) =>
                {
                    this.Count = (Convert.ToInt32(Count) - 1).ToString();
                });

            AddToBasketCommand = new Command<string>(
                canExecute: (string Count) =>
                {
                    if (Convert.ToInt32(Count) == 0 )
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                },
                execute: (string Count) =>
                {
                    AddItemToBasket(item);
                    _page.Navigation.PopAsync();
                });
        }
        public void AddItemToBasket(Items item)
        {
            OrderItem order = new OrderItem();
            order.ItemId = item.Id;
            order.Amount = Convert.ToInt32(Count);
            order.BuyCostPerPiece=item.BuyCost;
            order.SellCostPerPiece = item.SellCost;

            order.CategoryId=item.CategoryId;
            order.SubCategoryId = item.SubCategoryId;

            App.basketHolder.AddItemToBasket(order);
            //App.basketHolder.CompleteOrder();
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
