using Parcial3.Domain.Implementations;
using Parcial3.Repositories.Implementations;
using Parcial3.Modules.Services.Parcial3.Modules.Services;
using Parcial3.Services.Implementations;
using Parcial3.UI;
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
            var itemRepository = new Repository<Item>(context); // Es buena práctica tenerlo listo.

            // --- 2. CREACIÓN DE LOS SERVICIOS ---
            var clientService = new CrudService<Client>(clientRepository);
            var itemService = new ItemService();
            var invoiceService = new InvoiceService(invoiceRepository, clientRepository, itemService);

            // --- 3. CREACIÓN DEL PRESENTADOR ---
            var presentator = new Presentator(clientService, invoiceService, itemService);

            // --- 4. EJECUCIÓN ---
            presentator.Run();
        }
    }
}
