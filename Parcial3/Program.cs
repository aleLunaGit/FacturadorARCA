using Microsoft.EntityFrameworkCore;
using Parcial3.Interfaces;
using Parcial3.Modules;
using Parcial3.Modules.Repositorys;
using Parcial3.Server;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Parcial3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // --- 1. CONFIGURACIÓN ---
            var context = new ApplicationDbContext();
            var clientRepository = new Repositories<Client>(context);
            var invoiceRepository = new Repositories<Invoice>(context);

            // --- 2. CREACIÓN DE LOS SERVICIOS ---
            // Aquí creas la instancia del servicio para clientes
            var clientService = new CrudService<Client>(clientRepository);
            var invoiceService = new InvoiceService(invoiceRepository, clientRepository);

            // --- 3. CREACIÓN DEL PRESENTADOR ---
            // ¡Paso clave! Le pasas el objeto 'clientService' que acabas de crear
            // al constructor del Presentator.
            var presentator = new Presentator(clientService, invoiceService);

            // --- 4. EJECUCIÓN ---
            presentator.Run();
        }
    }
}
