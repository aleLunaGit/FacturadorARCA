using Parcial3.Modules.Repositorys;
using Parcial3.Modules.Services;

namespace Parcial3.Modules
{
    public class Presentator
    {
        private readonly CrudService<Client> _clientService;
        private readonly InvoiceService _invoiceService;
        private readonly ItemService _itemService;

        public Presentator(CrudService<Client> clientService, InvoiceService invoiceService, ItemService itemService)
        {
            _clientService = clientService;
            _invoiceService = invoiceService;
            _itemService = itemService;
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
                        HandleRegisterInvoice();
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
        // Handle Client Services:
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

        // Handle Invoice Services:
        private void HandleRegisterInvoice() {
            // Creamos las instancias necesarias y le cargamos datos
            // Borrador de Invoice
            Invoice draftInvoice = new Invoice();
            int clientId= Reader.ReadInt("Ingrese el ID del Cliente al que le crearemos la factura: ");
            string invoiceType = Reader.ReadChar("\"Enter the invoice type \\n This can be: A, B or C\"").ToString();
            List<Item> items = new List<Item>();
            
            // Usamos estos datos para preparar el registro
            draftInvoice = _invoiceService.DraftInvoice(clientId, invoiceType, items);
            while (true)
            {
                ShowPreviewInvoice(draftInvoice);
                char decitionInput = Reader.ReadChar("\nDesea modificar la factura antes de cerrarla permanentemente? S/N");
                if (decitionInput == 'S')
                {
                    HandleUpdateInvoice(draftInvoice);
                }
                else break;
            }
            
            try
            {
                Console.WriteLine("\nCerrando y guardando factura permanentemente...");

                // El servicio toma el objeto borrador final, ejecuta su "receta maestra"
                // (validar, enriquecer, calcular, guardar) y lo persiste.
                _invoiceService.CreateNewInvoice(draftInvoice);

                Console.WriteLine("¡Factura registrada exitosamente!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al registrar la factura: {ex.Message}");
            }
        }
        private void HandleUpdateInvoice(Invoice draftInvoice)
        {
            Client comprador = draftInvoice.Client;
            WriteLine("\n--- Editando Borrador de Factura ---");
            WriteLine($"1) Cliente: {draftInvoice.Client.LegalName}");
            WriteLine($"2) Tipo: {draftInvoice.Type}");
            WriteLine($"3) Editar Ítems ({draftInvoice.Items.Count} actuales)");
            WriteLine("0) Volver a la revisión");
            int input = Reader.ReadInt("Ingrese una opcion");
            if (draftInvoice == null)
            {
                throw new ArgumentNullException($"No se encontro una factura con esa ID");
            }
            switch (input)
            {
                case 1:
                    {
                        int clientId = Reader.ReadInt("Ingrese el ID del Cliente para asignarle esta factura");
                        var newClient = _clientService.Search(clientId);
                        if (newClient != null)
                        {
                            draftInvoice.Client = newClient;
                            draftInvoice.ClientId = newClient.Id;
                        }
                        else throw new ArgumentException($"Error: No se encontró ningún cliente con el ID: {clientId}.");
                    }
                    break;
                case 2:
                    {
                        string inputType = Reader.ReadChar("Ingrese el tipo de factura").ToString();
                        draftInvoice.RegisterTypeFactura(inputType);
                    }
                    break;
                case 3:
                    {
                        _itemService.UpdateItem(draftInvoice.Items);
                        draftInvoice.CalculateTotalAmount();
                    }
                    break;
            }
        }
        private void ShowPreviewInvoice(Invoice invoice)
        {
            WriteLine("--------------------------Vista Previa de Factura--------------------------");
            WriteLine($"Fecha: {invoice.Date} | Numero de Factura: {invoice.Number}");
            WriteLine($"Razon Social: {invoice.Client.LegalName}");
            WriteLine($"Cuit/Cuil: {invoice.Client.CuitCuil}");
            WriteLine($"Domicilio: {invoice.Client.Address}");
            WriteLine($"Productos:");
            foreach (var item in invoice.Items)
            {
            WriteLine("---------------------------------------------------------------------------");
                WriteLine($"Nombre: {item.Description}\nPrecio: ${item.Price} | Cantidad: {item.Quantity}");
                WriteLine($"Total: ${item.Price*item.Quantity}");
            }
            WriteLine("---------------------------------------------------------------------------");
            WriteLine($"Monto Total: ${invoice.AmountTotal}");
        }

    }
}