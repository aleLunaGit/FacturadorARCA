using Parcial3.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Parcial3.Domain.Implementations
{
    public class Client : IPerson
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string CuitCuil {  get; internal set; }

        [Required]
        public string LegalName { get; internal set; }

        [Required]
        public string Address { get; internal set; }

        public List<Invoice> Invoices { get; set; }

        public Client()
        {
            Invoices = new List<Invoice>();
        }

        public Client(string cuitCuil, string legalName, string address)
        {
            CuitCuil = cuitCuil;
            LegalName = legalName;
            Address = address;
            Invoices = new List<Invoice>();
        }

        public string ValidateCuitCuil(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("ERROR: El CUIT/CUIL no puede estar vacío.");
            }

            string cleanCuit = Regex.Replace(value, @"[-\s]", "");

            if (!cleanCuit.All(char.IsDigit))
            {
                throw new ArgumentException("ERROR: El CUIT/CUIL debe contener solo números.");
            }

            if (cleanCuit.Length != 11)
            {
                throw new ArgumentException("ERROR: El CUIT/CUIL debe tener exactamente 11 dígitos.");
            }
            return value;
        }

        public string ValidateLegalName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("ERROR: La Razón Social no puede estar vacía.");
            }
            if (value.Trim().Length < 3)
            {
                throw new ArgumentException("ERROR: La Razón Social debe tener al menos 3 caracteres.");
            }
            if (value.Length > 200)
            {
                throw new ArgumentException("ERROR: La Razón Social no puede exceder los 200 caracteres.");
            }
            return value;
        }

        public string ValidateAddress(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("ERROR: El Domicilio no puede estar vacío.");
            }
            if (value.Trim().Length < 5)
            {
                throw new ArgumentException("ERROR: El Domicilio debe tener al menos 5 caracteres.");
            }
            return value;
        }

        public string GetCuilCuit() => CuitCuil;

        public void SetCuilCuit(string cuilCuit) => CuitCuil = ValidateCuitCuil(cuilCuit);

        public string GetLegalName() => LegalName;

        public void SetLegalName(string legalName) => LegalName = ValidateLegalName(legalName);

        public string GetAddress() => Address;

        public void SetAddress(string address) => Address = ValidateAddress(address);

        public List<Invoice> GetInvoices() => Invoices;
    }
}
