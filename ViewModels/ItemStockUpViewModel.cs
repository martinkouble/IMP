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
    public class ItemStockUpViewModel: BaseViewModel, INotifyPropertyChanged
    {
        public ICommand AddCommand { get; set; }
        public ICommand SubtractCommand { get; set; }
        public ICommand StockUpCommand { get; set; }
        public ICommand UnFocusCommand { get; set; }

        private string _count;
        public string Count
        {
            set
            {
                SetProperty(ref _count, value);
            }
            get { return _count; }
        }
        public Items Item;
        public ItemStockUpViewModel(Items item)
        {
            Item= item;
            Count = item.Stock.ToString();
            AddCommand = new Command<string>(
                (string Count) =>
                {
                  this.Count = (Convert.ToInt32(Count) + 1).ToString();
                });

            SubtractCommand = new Command<string>(
                canExecute: (string Count) =>
                {
                    if (Convert.ToInt32(Count) >= -Item.Stock)
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
            StockUpCommand = new Command<string>(
                canExecute: (string Count) =>
                {
                    if (Convert.ToInt32(Count) == item.Stock)
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
                    AddItemToStockUp(item);
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
        public void AddItemToStockUp(Items item)
        {
            //item.Stock += Convert.ToInt32(Count);
            StockUpItem stockUp = new StockUpItem();
            stockUp.CategoryId = item.CategoryId;
            stockUp.SubCategoryId = item.SubCategoryId;
            stockUp.ItemId = item.Id;
            stockUp.Amount = Convert.ToInt32(Count);
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
