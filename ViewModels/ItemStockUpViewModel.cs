using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using IMP_reseni.Models;
using Microsoft.Maui.Platform;

namespace IMP_reseni.ViewModels
{
    public class ItemStockUpViewModel: INotifyPropertyChanged
    {
        public ICommand AddCommand { get; set; }
        public ICommand SubtractCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }
        public ICommand UnFocusCommand { get; set; }


        private bool _stockUpOrDown=false;
        //_stockUpOrDown=false => Naskladnit
        //_stockUpOrDown=true => Odskladnit
        public bool StockUpOrDown
        {
            get { return _stockUpOrDown; }
            set
            {
                SetProperty(ref _stockUpOrDown, value);
                (AddCommand as Command).ChangeCanExecute();
                (SubtractCommand as Command).ChangeCanExecute();
                Count = "0";
            }
        }

        private string _count;
        public string Count
        {
            set
            {
                SetProperty(ref _count, value);
            }
            get { return _count; }
        }
        private Items _item;
        public Items Item 
        {
            get { return _item; }
            set
            { 
                SetProperty(ref _item, value); 
            } 
        }
        public ItemStockUpViewModel(Items item)
        {
            //StockUpOrDown = false;
            Item = item;
            //Count = item.Stock.ToString();
            Count = "0";
            AddCommand = new Command<string>(
                canExecute: (string Count) =>
                {
                    if ((StockUpOrDown == true && Convert.ToInt32(Count) !=  Item.Stock ) ||  StockUpOrDown == false)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                },
                execute: (string Count) =>
                {
                    this.Count = (Convert.ToInt32(Count) + 1).ToString();
                });

            SubtractCommand = new Command<string>(
                canExecute: (string Count) =>
                {
                    if (Convert.ToInt32(Count) != 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                },
                execute: (string Count) =>
                {
                    this.Count = (Convert.ToInt32(Count) - 1).ToString();
                });
            ConfirmCommand = new Command<string>(
                canExecute: (string Count) =>
                {
                    if (Convert.ToInt32(Count) != 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                },
                execute: (string Count) =>
                {
                    AddItemToStockUp(item, Count);
                    OnPropertyChanged("Item");
                    this.Count = "0";
                });
            UnFocusCommand = new Command<Entry>(
            (Entry entry) =>
            {
#if ANDROID
                if(Platform.CurrentActivity.CurrentFocus!=null)
                    Platform.CurrentActivity.HideKeyboard(Platform.CurrentActivity.CurrentFocus);
#else
                entry.Unfocus();
#endif
            });
        }
        public void AddItemToStockUp(Items item, string Count)
        {
            //item.Stock += Convert.ToInt32(Count);
            StockUpItem stockUp = new StockUpItem();
            stockUp.CategoryId = item.CategoryId;
            stockUp.SubCategoryId = item.SubCategoryId;
            stockUp.ItemId = item.Id;
            if (_stockUpOrDown==true)
            {
                stockUp.Amount = -(Convert.ToInt32(Count));
            }
            else
            {
                stockUp.Amount = Convert.ToInt32(Count);
            }
            //stockUp.Amount += Convert.ToInt32(Count);
            stockUp.CompleteStockup(item.CategoryId, item.SubCategoryId, item.Id);
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
