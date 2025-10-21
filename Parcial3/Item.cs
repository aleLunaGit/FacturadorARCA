using Parcial3.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3
{
    public class Item : IItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Description { get; set; }
        public float Quantity { get; set; }
        public float Price { get; set; }
        public Invoice Invoice { get; set; }
        public int InvoiceId { get; internal set; }


        public Item()
        {

        }

        public Item(int id, string description, float quantity, float price, Invoice invoice) 
        {
            Id = id;
            Description = description;
            Quantity = quantity;
            Price = price;
            Invoice = invoice;
        }

        public string GetDescription()=> this.Description;

        public float GetQuantity()=> this.Quantity;

        public float GetPrice() => this.Price;

        public void SetDescription(string description) => this.Description = description;

        public void SetQuantity(float cuantity) => this.Quantity = cuantity;

        public void SetPrice(float Price)=> this.Price = Price;
    }
}
