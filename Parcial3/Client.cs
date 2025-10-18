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
        public Client (int id, string cuitCuil, string legalName, string address)
        {
            Id = id;
            CuitCuil = cuitCuil;
            LegalName = legalName;
            Address = address;
            Invoices = new List<Invoice>();
        }

        public void GetCuilCuit()
        {
            throw new NotImplementedException();
        }

        public void SetCuilCuit(int cuilCuit)
        {
            throw new NotImplementedException();
        }

        public void GetRazonSocial()
        {
            throw new NotImplementedException();
        }

        public void SetRazonSocial(int razonSocial)
        {
            throw new NotImplementedException();
        }

        public void GetDomicilio()
        {
            throw new NotImplementedException();
        }

        public void SetDomicilio(int domicilio)
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public void List()
        {
            throw new NotImplementedException();
        }

        public void Register()
        {
            throw new NotImplementedException();
        }

        public void Read()
        {
            throw new NotImplementedException();
        }
    }
}
