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
        



        public Invoice() { }

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

        void IFactura.GetType()
        {
            throw new NotImplementedException();
        }

        public void GetNumber()
        {
            throw new NotImplementedException();
        }

        public void GetDate()
        {
            throw new NotImplementedException();
        }

        public void GetAmountTotal()
        {
            throw new NotImplementedException();
        }

        public void SetType(string type)
        {
            throw new NotImplementedException();
        }

        public void SetNumber(int number)
        {
            throw new NotImplementedException();
        }

        public void SetDate(DateTime date)
        {
            throw new NotImplementedException();
        }

        public void SetAmountTotal(int amountTotal)
        {
            throw new NotImplementedException();
        }

        public void ShowPreviewInvoice()
        {
            throw new NotImplementedException();
        }


        public void Read()
        {
            throw new NotImplementedException();
        }

        public void Register()
        {
            throw new NotImplementedException();
        }
    }
}
