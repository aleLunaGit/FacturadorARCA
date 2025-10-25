using Parcial3.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public Item() { }
        public Item(string description, float quantity, float price) 
        {
            Description = description;
            Quantity = quantity;
            Price = price;
        }

        public string GetDescription()=> this.Description;

        public float GetQuantity()=> this.Quantity;

        public float GetPrice() => this.Price;

        public void SetDescription(string description) => this.Description = description;

        public void SetQuantity(float cuantity) => this.Quantity = cuantity;

        public void SetPrice(float Price)=> this.Price = Price;
    }
}
