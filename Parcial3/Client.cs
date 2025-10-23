using Parcial3.Interfaces;
using Parcial3.Server;
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
        public int Id { get; private set; }
        [Required]

        public string CuitCuil { get; private set; }
        [Required]
        public string LegalName { get; private set; }
        [Required]
        public string Address { get; private set; }
        public List<Invoice> Invoices { get; private set; }
        [NotMapped]
        public static ApplicationDbContext context { get; set; }
        public Client()
        {
            Invoices = new List<Invoice>();
        }
       /* public Client() 
        {
            Invoices = new List<Invoice>();
        } */
        public Client (int id, string cuitCuil, string legalName, string address)
        {
            Id = id;
            CuitCuil = cuitCuil;
            LegalName = legalName;
            Address = address;
            Invoices = new List<Invoice>();
        }

        public static void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public static void Update(int id)
        {
            throw new NotImplementedException();
        }

        public static void List()
        {
            
        }

        public static void Register()
        {
            Console.WriteLine("Registro de Cliente");
            Console.WriteLine("Ingrese la RazonSocial");
            string legalName = Console.ReadLine();
            Console.WriteLine("Ingrese el Cuit/Cuil del cliente");
            string cuitCuil = Console.ReadLine();
            Console.WriteLine("Ingrese la direccion del cliente");
            string address = Console.ReadLine();
            Client registerClient = new Client
            {
                LegalName = legalName,
                CuitCuil = cuitCuil,
                Address = address
            };
            context.Clients.Add(registerClient);
            context.SaveChanges();
        }

        public static void Search()
        {
            Console.WriteLine("Buscar al cliente por ID");
            Console.WriteLine("Ingrese la ID del cliente");
            int id = int.Parse(Console.ReadLine());
            Client cliente =context.Clients.Find(id);
            if (cliente != null)
            {
                Console.WriteLine($"{cliente.Id}\nRazon Social: {cliente.LegalName}\nDireccion: {cliente.Address}\nCuit/Cuil: {cliente.CuitCuil}");
            }
            else
            {
                Console.WriteLine($"No se encontró ningún cliente con el ID: {id}");
            }
        }
        public string GetCuilCuit() => this.CuitCuil;

        public void SetCuilCuit(string cuilCuit)=> this.CuitCuil = cuilCuit;

        public string GetLegalName() => this.LegalName;

        public void SetLegalName(string legalName)=> this.LegalName = legalName;

        public string GetAddress() => this.Address;

        public void SetAddress(string Address)=> this.Address = Address;

    }
}
