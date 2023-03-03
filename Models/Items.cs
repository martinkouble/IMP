using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP_reseni.Models
{
    public class Items
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public bool Disabled { get; set; }

        public double BuyCost { get; set; }
        public double SellCost { get; set; }
        public bool SoR { get; set; }

        public int Stock { get; set; }
        public int SupplierId { get; set; }

        //

        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }

        public Items MakeCopy()
        {
            Items output = new Items();
            output.Id = this.Id;
            output.Name = this.Name;
            output.ImageUrl = this.ImageUrl;
            output.Disabled = this.Disabled;
            output.BuyCost = this.BuyCost;
            output.SellCost = this.SellCost;
            output.SoR = this.SoR;
            output.Stock = this.Stock;
            output.SupplierId = this.SupplierId;
            //output.bitmap = this.bitmap;

            //
            output.CategoryId = this.CategoryId;
            output.SubCategoryId = this.SubCategoryId;
            return output;
        }

        public void Create(string name, bool disabled, double buyCost, double sellCost, bool sor, int supplierId,int categoryId,int subcategoryId)
        {
            this.Name = name;
            this.Disabled = disabled;
            this.BuyCost = buyCost;
            this.SellCost = sellCost;
            this.SoR = sor;
            this.Stock = 0;
            this.SupplierId = supplierId;
            this.CategoryId = categoryId;
            this.SubCategoryId = subcategoryId;
        }
    }



}
