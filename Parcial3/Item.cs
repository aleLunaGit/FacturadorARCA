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
        public int InvoiceId { get; internal set; }


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

        public string GetDescription()=> this.Description;

        public int GetCuantity()=> this.Quantity;

        public int GetPrice() => this.Price;

        public void SetDescription(string description) => this.Description = description;

        public void SetCuantity(int cuantity) => this.Quantity = cuantity;

        public void SetPrice(int Price)=> this.Price = Price;
    }
}
