using Parcial3.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3.Modules
{
    public class Presentator
    {
        private readonly Repositories<Invoice> repositoryInvoice; 
        private readonly Repositories<Client> repositoryClient;
        private readonly Repositories<Item> repositoryItem;
         public Presentator(Repositories<Invoice> repoInvoice, Repositories<Client> repoClient, Repositories<Item> repoItem) { 
            this.repositoryInvoice = repoInvoice;
            this.repositoryClient = repoClient;
            this.repositoryItem = repoItem;
        }
        public void ShowMainMenu()
        {
            Console.WriteLine($"Bienvenido al Facturador Arca");
            IEnumerable<Client>totalClients = repositoryClient.GetAll();
            Console.WriteLine($"Trabajamos con {totalClients.Count()} clientes alrededor del pais");
        }
    }
}
