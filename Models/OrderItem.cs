using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMP_reseni.Services;

namespace IMP_reseni.Models
{
    public class OrderItem
    {
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int ItemId { get; set; }

        public int Amount { get; set; }
        public double BuyCostPerPiece { get; set; }
        public double SellCostPerPiece { get; set; }

        

        //public Items Item
        //{
        //    get
        //    {
        //        SaveHolder saveholder = App.saveholder;
        //        if (saveholder.Inventory.Count > 0)
        //            return saveholder.FindCategory(CategoryId).FindSubCategory(SubCategoryId).FindItem(ItemId);
        //        else
        //            return null;
        //    }
        //}
    }
}
