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

        private string _cuitCuil;
        private string _legalName;
        private string _address;


        [Required]
        public string CuitCuil
        {
            get => _cuitCuil;
            set
            {
                ValidateCuitCuil(value);
                _cuitCuil = value;
            }
        }

        [Required]
        public string LegalName
        {
            get => _legalName;
            set
            {
                ValidateLegalName(value);
                _legalName = value;
            }
        }

        [Required]
        public string Address
        {
            get => _address;
            set
            {
                ValidateAddress(value);
                _address = value;
            }
        }

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

        private void ValidateCuitCuil(string value)
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
        }

        private void ValidateLegalName(string value)
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
        }

        private void ValidateAddress(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("ERROR: El Domicilio no puede estar vacío.");
            }
            if (value.Trim().Length < 5)
            {
                throw new ArgumentException("ERROR: El Domicilio debe tener al menos 5 caracteres.");
            }
        }

      

        public string GetCuilCuit() => CuitCuil;

        public void SetCuilCuit(string cuilCuit) => CuitCuil = cuilCuit;

        public string GetLegalName() => LegalName;

        public void SetLegalName(string legalName) => LegalName = legalName;

        public string GetAddress() => Address;

        public void SetAddress(string address) => Address = address;

        public List<Invoice> GetInvoices() => Invoices;
    }
}
