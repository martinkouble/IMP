using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMP_reseni.Services;
using IMP_reseni.Models;

namespace IMP_reseni.Services
{
    public class BasketHolder
    {
        public Order Order { get; set; }
        private List<OrderItem> Items { get { return Order.Items; } }

        public bool Empty
        {
            get
            {
                if (Items.Count <= 0)
                    return true;
                else
                    return false;
            }
        }
        public void CompleteOrder()
        {
            foreach (OrderItem i in Items)
            {
                Items item = App.saveholder.FindCategory(i.CategoryId).FindSubCategory(i.SubCategoryId).FindItem(i.ItemId);
                item.Stock -= i.Amount;
            }
            this.Order.OrderCompletion = DateTime.Now;
            App.saveholder.AddToOrderHistory(this.Order);
            /*
            MainActivity.main.TryShowBasketButton(false);
            MyApplication.myapp.Basket = new BasketHolder();*/
        }
        public void AddItemToBasket(OrderItem item)
        {
            Order.AddItemToOrder(item);
        }
        public void RemoveItemFromBasket(int categoryId, int subCategoryId, int itemId)
        {
            for (int i = 0; i < Items.Count; i++)
                if (Items[i].CategoryId == categoryId && Items[i].SubCategoryId == subCategoryId && Items[i].ItemId == itemId)
                {
                    Items.RemoveAt(i);
                    break;
                }
        }
        public int ItemAmountInBasket(int categoryId, int subCategoryId, int itemId)
        {
            int output = 0;
            List<OrderItem> found = Items.FindAll(f => f.CategoryId == categoryId && f.SubCategoryId == subCategoryId && f.ItemId == subCategoryId);
            foreach (OrderItem o in found)
                output += o.Amount;
            return output;
        }
        public BasketHolder()
        {
            if (Order == null)
                Order = new Order();
        }
    }
}
