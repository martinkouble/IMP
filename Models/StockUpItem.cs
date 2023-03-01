using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMP_reseni.Services;
namespace IMP_reseni.Models
{
    public class StockUpItem
    {
        public bool Completed = false;

        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int ItemId { get; set; }

        public int Amount { get; set; }
        public int FinalAmount { get; set; }
        public int ChangedSellCost { get; set; }
        public int ChangedBuyCost { get; set; }

        public DateTime StockUpCompletion { get; set; }

        public bool CompleteStockup(int SelectedStockUpCategoryId,int SelectedStockUpSubCategoryId, int SelectedStockUpItemId)
        {
            if (!Completed && CategoryId != default(int) && SubCategoryId != default(int) && ItemId != default(int))
            {
                SaveHolder saveHolder = App.saveholder;
                Items item = saveHolder.FindCategory(SelectedStockUpCategoryId).FindSubCategory(SelectedStockUpSubCategoryId).FindItem(SelectedStockUpItemId);
                item.Stock += Amount;
                this.FinalAmount = item.Stock;
                this.Completed = true;
                this.StockUpCompletion = DateTime.Now;
                saveHolder.AddToStockUpHistory(this);
            }
            else
                return false;
            return true;
        }
        public bool CompleteChangeCost(int SelectedStockUpCategoryId, int SelectedStockUpSubCategoryId, int SelectedStockUpItemId)
        {
            if (!Completed && CategoryId != default(int) && SubCategoryId != default(int) && ItemId != default(int))
            {
                SaveHolder saveHolder = App.saveholder;
                Items item = saveHolder.FindCategory(SelectedStockUpCategoryId).FindSubCategory(SelectedStockUpSubCategoryId).FindItem(SelectedStockUpItemId);
                item.SellCost = ChangedSellCost;
                item.BuyCost = ChangedBuyCost;
                this.Completed = true;
                this.StockUpCompletion = DateTime.Now;
                saveHolder.AddToStockUpHistory(this);
            }
            else
                return false;
            return true;
        }
        
        //public bool CompleteDeleting(int SelectedStockUpCategoryId, int SelectedStockUpSubCategoryId, int SelectedStockUpItemId)
        //{
        //    if (!Completed && CategoryId != default(int) && SubCategoryId != default(int) && ItemId != default(int))
        //    {
        //        SaveHolder saveHolder = App.saveholder;
        //        Items item = saveHolder.FindCategory(SelectedStockUpCategoryId).FindSubCategory(SelectedStockUpSubCategoryId).FindItem(SelectedStockUpItemId);
        //        this.Completed = true;
        //        saveHolder.RemoveToStockUpHistory(this);
        //    }
        //    else
        //        return false;
        //    return true;
        //}
    }
}
