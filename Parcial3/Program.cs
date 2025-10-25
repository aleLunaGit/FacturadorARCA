using Parcial3.Modules;
using Parcial3.Modules.Repositorys;
using Parcial3.Modules.Services;
using Parcial3.Server;

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

            // Primero, crea la instancia de ItemService.
            var itemService = new ItemService();

            // ¡CORRECCIÓN! Ahora le pasamos las TRES dependencias que necesita.
            var invoiceService = new InvoiceService(invoiceRepository, clientRepository, itemService);

            // --- 3. CREACIÓN DEL PRESENTADOR ---
            var presentator = new Presentator(clientService, invoiceService);

            // --- 4. EJECUCIÓN ---
            presentator.Run();
        }
    }
}
