using Microsoft.EntityFrameworkCore;
using Parcial3.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3.Modules.Services
{
    public class ClientService
    {
        private readonly ApplicationDbContext _context;

        // Recibe el DbContext para poder construir consultas complejas.
        public ClientService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Este es el método que hace la magia.
        public Client GetClientWithAllDetails(int clientId)
        {
            // Construimos la consulta usando Include y ThenInclude
            var client = _context.Clients
                .Include(c => c.Invoices)       // 1. INCLUYE la lista de Facturas
                    .ThenInclude(i => i.Items)  // 2. LUEGO, para cada Factura, INCLUYE su lista de Items
                .FirstOrDefault(c => c.Id == clientId); // Busca el cliente por su ID

            return client;
        }
    }
}
