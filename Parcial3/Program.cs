using Parcial3.Domain.Implementations;
using Parcial3.Repositories.Implementations;
using Parcial3.Modules.Services.Parcial3.Modules.Services;
using Parcial3.Services.Implementations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parcial3.Modules;
namespace Parcial3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // --- 1. CONFIGURACIÓN ---
            var context = new ApplicationDbContext();

            // Creamos un repositorio para CADA entidad.
            var clientRepository = new Repository<Client>(context);
            var invoiceRepository = new Repository<Invoice>(context);
            var itemRepository = new Repository<Item>(context);

            // --- 2. CREACIÓN DE LOS SERVICIOS ---
            var clientService = new ClientService(clientRepository, context);
            var itemService = new ItemService(itemRepository, context);
            var invoiceService = new InvoiceService(invoiceRepository, clientRepository, context);

            // --- 3. CREACIÓN DEL PRESENTADOR ---
            var clientMenu = new ClientMenu(clientService);
            var itemMenu = new ItemMenu(itemService);
            var invoiceMenu = new InvoiceMenu(invoiceService, clientMenu, itemMenu);
            var mainMenu = new MainMenu(clientMenu, invoiceMenu);

            mainMenu.Run();

        }
    }
}
