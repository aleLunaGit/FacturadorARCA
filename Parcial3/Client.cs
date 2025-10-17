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
        public Client() 
        {

        }
        public Client (int id, string cuitCuil, string legalName, string address)
        {
            Id = id;
            CuitCuil = cuitCuil;
            LegalName = legalName;
            Address = address;
        }
    }
}
