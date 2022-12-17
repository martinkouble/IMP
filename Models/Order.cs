using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP_reseni.Models
{
    public class Order
    {
        public List<OrderItem> Items { get; set; }

        public DateTime OrderCompletion { get; set; }

        public void AddItemToOrder(OrderItem item)
        {
            Items.Add(item);
        }
        public Order()
        {
            if (Items == null)
                Items = new List<OrderItem>();
        }
    }
}
