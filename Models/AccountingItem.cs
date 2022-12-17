using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IMP_reseni.Models
{
    public class AccountingItem
    {
        public int supplierId;
        public int categoryId;
        public int subCategoryId;
        public int itemId;

        public int StartAmount = 0;
        public int SoldAmount = 0;
        public int Remaining = 0;
        public List<double> BuyCost = new List<double>();
        public List<double> SellCost = new List<double>();
        public double NetSale = 0;

        public string BuyCostText
        {
            get
            {
                string output = Math.Round(BuyCost[0], 2).ToString() + " Kč";
                for (int i = 1; i < BuyCost.Count; i++)
                    output += "\n" + Math.Round(BuyCost[i], 2) + " Kč";
                return output;
            }
        }
        public string SellCostText
        {
            get
            {
                string output = Math.Round(SellCost[0], 2).ToString() + " Kč";
                for (int i = 1; i < SellCost.Count; i++)
                    output += "\n" + Math.Round(SellCost[i], 2) + " Kč";
                return output;
            }
        }

        public Items Item { get { return App.saveholder.FindCategory(this.categoryId).FindSubCategory(this.subCategoryId).FindItem(this.itemId); } }
    }
}
