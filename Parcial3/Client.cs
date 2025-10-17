using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3
{
    public class Client
    {
        public int Id { get; set; }
        public string CuitCuil { get; set; }
        public string LegalName { get; set; }
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
