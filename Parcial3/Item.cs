using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3
{
    public class Item
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public Invoice Invoice { get; set; }
        public Item(int id, string description, int quantity, int price, Invoice invoice) 
        {
            Id = id;
            Description = description;
            Quantity = quantity;
            Price = price;
            Invoice = invoice;
        }

    }
}
