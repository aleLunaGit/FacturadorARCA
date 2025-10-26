using Parcial3.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parcial3
{
    public class Client : IPerson
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }
        [Required]

        public string CuitCuil { get; private set; }
        [Required]
        public string LegalName { get; private set; }
        [Required]
        public string Address { get; private set; }
        public List<Invoice> Invoices { get; private set; }
        public Client()
        {
            Invoices = new List<Invoice>();
        }
       /* public Client() 
        {
            Invoices = new List<Invoice>();
        } */
        public Client (string cuitCuil, string legalName, string address)
        {
            CuitCuil = cuitCuil;
            LegalName = legalName;
            Address = address;
            Invoices = new List<Invoice>();
        }
        public string GetCuilCuit() => this.CuitCuil;

        public void SetCuilCuit(string cuilCuit)=> this.CuitCuil = cuilCuit;

        public string GetLegalName() => this.LegalName;

        public void SetLegalName(string legalName)=> this.LegalName = legalName;

        public string GetAddress() => this.Address;

        public void SetAddress(string Address)=> this.Address = Address;

    }
}
