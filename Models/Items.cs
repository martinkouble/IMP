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
    }
}
