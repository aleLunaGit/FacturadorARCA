using Parcial3.Modules.Repositorys;

namespace Parcial3.Modules
{
    public class Presentator
    {
        private readonly CrudService<Client> _clientService;
        private readonly InvoiceService _invoiceService;

        public Presentator(CrudService<Client> clientService, InvoiceService invoiceService)
        {
            _clientService = clientService;
            _invoiceService = invoiceService;
        }

        public void Run()
        {
            while (true)
            {
                WriteLine("\n╔══════════════════════════════════╗");
                WriteLine("║        SISTEMA DE GESTIÓN        ║");
                WriteLine("╠══════════════════════════════════╣");
                WriteLine("║        -- CLIENTES --            ║");
                WriteLine("║ 1. Registrar Nuevo Cliente       ║");
                WriteLine("║ 2. Modificar Cliente Existente   ║");
                WriteLine("║ 3. Buscar Cliente por ID         ║");
                WriteLine("║ 4. Listar Todos los Clientes     ║");
                WriteLine("║        -- FACTURAS --            ║");
                WriteLine("║ 5. Registrar Nueva Factura       ║");
                WriteLine("║                                  ║");
                WriteLine("║ 6. Salir                         ║");
                WriteLine("╚══════════════════════════════════╝");
                Write("Seleccione una opción: ");

                var option = Reader.ReadString("");

                switch (option)
                {
                    case "1":
                        _clientService.Register();
                        break;
                    case "2":
                        HandleUpdateClient();
                        break;
                    case "3":
                        HandleSearchClient();
                        break;
                    case "4":
                        HandleListClients();
                        break;
                    case "5":
                        _invoiceService.Register();
                        break;
                    case "6":
                        WriteLine("Saliendo del sistema...");
                        return;
                    default:
                        WriteLine("Opción no válida. Por favor, intente de nuevo.");
                        break;
                }
                Reader.ReadChar("\nPresione cualquier tecla para volver al menú...");
            }
        }
        public static void WriteLine(string msg) {
            Console.WriteLine($"{msg}");
        }
        public static void Write(string msg)
        {
            Console.Write(msg);
        }
        private void HandleUpdateClient()
        {
            try
            {
                int id = Reader.ReadInt("Ingrese el ID del cliente que desea modificar");
                _clientService.Update(id);
            }
            catch (Exception ex)
            {
                WriteLine($"Ocurrió un error: {ex.Message}");
            }
        }

        private void HandleSearchClient()
        {
            try
            {
                int id = Reader.ReadInt("Ingrese el ID del cliente a buscar");
                // Llama a Search y le dice que incluya la lista de facturas
                 _clientService.Search(id, c => c.Invoices);
                WriteLine(""); // Añade un espacio para legibilidad
                char choice = Reader.ReadChar("¿Desea crear una nueva factura para este cliente? (S/N)");
                WriteLine(""); // Salto de línea después de leer el carácter

                if (choice == 'S' || choice == 's')
                {
                    // Llama al método Register sobrecargado, pasándole el cliente que ya encontramos.
                    _invoiceService.Register(id);
                }
            }
            catch (Exception ex)
            {
                WriteLine($"Ocurrió un error: {ex.Message}");
            }
        }

        private void HandleListClients()
        {
            var clientes = _clientService.GetAll();
            WriteLine("\n--- LISTADO DE CLIENTES ---");
            if (clientes != null && clientes.Any())
            {
                foreach (var cliente in clientes)
                {
                    WriteLine($"ID: {cliente.Id} | Nombre: {cliente.LegalName} | CUIT: {cliente.CuitCuil}");
                }
            }
            else
            {
                WriteLine("No hay clientes registrados.");
            }
        }
    }
}