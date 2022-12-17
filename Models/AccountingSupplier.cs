using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP_reseni.Models
{
    public class AccountingSupplier
    {
        public Supplier Supplier;
        public List<AccountingItem> Items = new List<AccountingItem>();
        public double TotalNetSale
        {
            get
            {
                double output = 0;
                foreach (AccountingItem i in this.Items)
                    output += i.NetSale;
                return output;
            }
        }
    }
}
