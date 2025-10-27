using Parcial3.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parcial3.Domain.Implementations
{
    public class Client : IPerson
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
        public void ReadCuitCuil(string cuit)
        {
            // Validaciones para CUIT o CUIL
            string validado ="";
            CuitCuil = validado;

        }
        public string GetCuilCuit() => CuitCuil;

        public void SetCuilCuit(string cuilCuit)=> CuitCuil = cuilCuit;

        public string GetLegalName() => LegalName;

        public void SetLegalName(string legalName)=> LegalName = legalName;

        public string GetAddress() => Address;

        public void SetAddress(string Address)=> this.Address = Address;

    }
}
