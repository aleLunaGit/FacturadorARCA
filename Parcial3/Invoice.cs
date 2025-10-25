using Parcial3.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parcial3
{
    public class Invoice : IInvoice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public float AmountTotal { get; set; }
        public Client Client { get; set; }
        public List<Item> Items { get; set; }
        public int  ClientId { get; internal set; }


        public Invoice()
        {
            Items = new List<Item>();
        }

       public void Validacion()
        {

        }


        

        // Setters / Getters 
        public string GetType() => this.Type;

        public string GetNumber() => this.Number;

        public DateTime GetDate() => this.Date;

        public float GetAmountTotal() => this.AmountTotal;

        public void SetType(string type)=> this.Type = type;

        // public void SetNumber()=> this.Number = NumberGenerator();

        public void SetDate(DateTime date)=> this.Date = date;

        public void SetAmountTotal(float amountTotal)=> this.AmountTotal = amountTotal;
    }
}
