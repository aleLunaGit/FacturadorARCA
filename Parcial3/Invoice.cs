using Microsoft.Identity.Client;
using Parcial3.Interfaces;
using Parcial3.Modules;
using Parcial3.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3
{
    public class Invoice
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
        [NotMapped]
        public Enum TyposFactura {  get; set; }

        public Invoice() { 
            Items = new List<Item>();
        }

        public Invoice(int id, string type, Client client)
        {
            Id = id;
            Type = type.ToUpper(); ;
            //Number = NumberGenerator();
            Date = DateTime.Now;
            //CalculateTotalAmount();
            Client = client;
            //Items = AddItems();
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
