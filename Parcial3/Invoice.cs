using Parcial3.Interfaces;
using Parcial3.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3
{
    public class Invoice : IFactura
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public int AmountTotal { get; set; }
        public Client Client { get; set; }
        public List<Item> Items { get; set; }
        public int  ClientId { get; internal set; }



        public Invoice() { 
            Items = new List<Item>();
        }

        public Invoice(int id, string type, int number, DateTime date, int amountTotal, Client client, List<Item> items)
        {
            Id = id;
            Type = type;
            Number = number;
            Date = date;
            AmountTotal = amountTotal;
            Client = client;
            Items = new List<Item>();
        }

       

        public void ShowPreviewInvoice()
        {
            throw new NotImplementedException();
        }


        public static void Read()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            Console.WriteLine("Ingrese la ID");
            int id = int.Parse(Console.ReadLine());
            context.Invoices.Find(id);
        }

        public static void Register()
        {
            throw new NotImplementedException();
        }

        public string GetType() => this.Type;

        public int GetNumber() => this.Number;

        public DateTime GetDate() => this.Date;

        public int GetAmountTotal() => this.AmountTotal;

        public void SetType(string type)=> this.Type = type;

        public void SetNumber(int number)=> this.Number = number;

        public void SetDate(DateTime date)=> this.Date = date;

        public void SetAmountTotal(int amountTotal)=> this.AmountTotal = amountTotal;
    }
}
