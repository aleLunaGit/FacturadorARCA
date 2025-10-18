using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3
{
    public class Client
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]

        public string CuitCuil { get; set; }
        [Required]
        public string LegalName { get; set; }
        [Required]
        public string Address { get; set; }
        public List<Invoice> Invoices { get; set; }
        public Client() 
        {
            Invoices = new List<Invoice>();
        }
        public Client (int id, string cuitCuil, string legalName, string address)
        {
            Id = id;
            CuitCuil = cuitCuil;
            LegalName = legalName;
            Address = address;
            Invoices = new List<Invoice>();
        }
    }
}
