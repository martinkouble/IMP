using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP_reseni.Models
{
    public class SubCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Disabled { get; set; }
        public List<Items> Items { get; set; }

    }
}
