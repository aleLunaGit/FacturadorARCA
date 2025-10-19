using Parcial3.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3
{
    public class Item : IItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public Invoice Invoice { get; set; }
        public object InvoiceId { get; internal set; }


        public Item()
        {

        }

        public Item(int id, string description, int quantity, int price, Invoice invoice) 
        {
            Id = id;
            Description = description;
            Quantity = quantity;
            Price = price;
            Invoice = invoice;
        }

        public void GetDescription()
        {
            throw new NotImplementedException();
        }

        public void GetCuantity()
        {
            throw new NotImplementedException();
        }

        public void GetAmount()
        {
            throw new NotImplementedException();
        }

        public void SetDescription(string description)
        {
            throw new NotImplementedException();
        }

        public void SetCuantity(string cuantity)
        {
            throw new NotImplementedException();
        }

        public void SetAmount(int amount)
        {
            throw new NotImplementedException();
        }
    }
}
