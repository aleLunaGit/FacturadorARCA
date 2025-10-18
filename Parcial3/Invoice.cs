using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
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
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public int AmountTotal { get; set; }
        public Client Client { get; set; }
        public List<Item> Items { get; set; }
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
    }
}
