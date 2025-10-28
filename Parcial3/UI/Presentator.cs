using Parcial3.Domain.Implementations;
using Parcial3.Modules;
using Parcial3.Modules.Services.Parcial3.Modules.Services;
using Parcial3.Services.Implementations;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace Parcial3.UI
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
                Console.WriteLine("\n╔══════════════════════════════════╗");
                Console.WriteLine("║        SISTEMA DE GESTIÓN        ║");
                Console.WriteLine("╠══════════════════════════════════╣");
                Console.WriteLine("║ 1. Registrar Nuevo Cliente       ║");
                Console.WriteLine("║ 2. Modificar Cliente Existente   ║");
                Console.WriteLine("║ 3. Buscar Cliente por ID         ║");
                Console.WriteLine("║ 4. Listar Clientes               ║");
                Console.WriteLine("║ 5. Eliminar Cliente              ║");
                Console.WriteLine("║ 6. Registrar Nueva Factura       ║");
                Console.WriteLine("║ 7. Consultar Factura por ID      ║");
                Console.WriteLine("║ 8. Salir                         ║");
                Console.WriteLine("╚══════════════════════════════════╝");
                Console.Write("Seleccione una opción: ");

                var option = Reader.ReadString("");

                switch (option)
                {
                    case "1":
                        HandleRegisterClient();
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
                        HandleDeleteClient();
                        break;
                    case "6":
                        HandleRegisterInvoice();
                        break;
                    case "7":
                        HandleSearchInvoice();
                        break;
                    case "8":
                        WriteLine("Saliendo del sistema...");
                        return;
                    default:
                        WriteLine("Opción no válida. Por favor, intente de nuevo.");
                        break;
                }
                Reader.ReadChar("\nPresione cualquier tecla para volver al menú...");
            }
        }

        private void HandleSearchInvoice()
        {
            try
            {
                int id = Reader.ReadInt("Ingrese el ID del cliente a buscar");
                Client entity = _clientService.SearchWhitIncludes(id, x => x.Invoices);

                if (entity == null)
                {
                    WriteLine($"No se encontró un cliente con ID {id}");
                    return;
                }

                foreach (var invoiceInfo in entity.GetInvoices())
                {
                    WriteLine($"- Factura ID: {invoiceInfo.Id} | Número: {invoiceInfo.Number} | Tipo: {invoiceInfo.Type} | Monto Total: ${invoiceInfo.AmountTotal:F2}");
                }

                int invoiceId = Reader.ReadInt("Ingrese el ID de la factura a buscar");
                Invoice invoice = _invoiceService.SearchWhitIncludes(invoiceId, x => x.Items);

                if (invoice == null)
                {
                    WriteLine($"No se encontró una factura con ID {invoiceId} para el cliente con ID {id}");
                    return;
                }

                WriteLine($"\n--- Detalles de la Factura (ID: {invoiceId}) ---");
                ShowInvoice(invoice);
            }
            catch (Exception ex)
            {
                WriteLine($"Ocurrió un error: {ex.Message}");
            }
        }

        private void ShowInvoice(Invoice invoice)
        {
            WriteLine($"Número: {invoice.Number}");
            WriteLine($"Tipo: {invoice.Type}");
            WriteLine($"Fecha: {invoice.Date:dd/MM/yyyy}");
            WriteLine($"Monto Total: ${invoice.AmountTotal:F2}");
            WriteLine("Productos:");
            foreach (Item item in invoice.Items)
            {
                WriteLine($"  - {item.Description}: ${item.Price:F2} x {item.Quantity} = ${item.Price * item.Quantity:F2}");
            }
        }

        public static void WriteLine(string msg)
        {
            Console.WriteLine($"{msg}");
        }

        public static void Write(string msg)
        {
            Console.Write(msg);
        }


        private void HandleRegisterClient()
        {
            if (_clientService == null) return;

            try
            {
                Client newClient = new Client();
                List<string> listOfInputs = new List<string>();

                string cuit = Reader.ReadString("Ingresa su Cuit/Cuil");
                string legalName = Reader.ReadString("Ingresa la Razón Social");
                string address = Reader.ReadString("Ingrese su Domicilio");

                listOfInputs.Add(cuit);
                listOfInputs.Add(legalName);
                listOfInputs.Add(address);

                _clientService.Register(newClient, listOfInputs);
                WriteLine("✓ Cliente registrado exitosamente.");
            }
            catch (Exception ex)
            {
                WriteLine($"✗ Error al registrar cliente: {ex.Message}");
            }
        }

        private void HandleUpdateClient()
        {
            try
            {
                int clientId = Reader.ReadInt("Ingrese el ID del cliente que desea modificar");
                var updateClient = _clientService.Search(clientId);

                if (updateClient == null)
                {
                    WriteLine($"No se encontró un cliente con ID {clientId}");
                    return;
                }

                ShowOptionsToUpdate(updateClient);
                int input = Reader.ReadInt("Ingrese una opción");
                var changeTo = Reader.ReadString("Ingrese el nuevo valor");

                _clientService.Update(updateClient, changeTo, input);
                WriteLine("Cliente actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                WriteLine($"Ocurrió un error: {ex.Message}");
            }
        }

        private void ShowOptionsToUpdate(Client entity)
        {
            if (_clientService == null) return;

            var updateProperty = _clientService.ListModifyableProperties(entity);
            int count = 1;

            WriteLine("\n--- Propiedades Modificables ---");
            foreach (var property in updateProperty)
            {
                WriteLine($"{count}) {property.Name}");
                count++;
            }
        }

        private void HandleSearchClient()
        {
            try
            {
                int id = Reader.ReadInt("Ingrese el ID del cliente a buscar");
                Client entity = _clientService.SearchWhitIncludes(id, x => x.Invoices);

                if (entity == null)
                {
                    WriteLine($"No se encontró un cliente con ID {id}");
                    return;
                }

                WriteLine($"\n--- Detalles de {typeof(Client).Name} (ID: {id}) ---");
                ShowClient(entity, client => client.Invoices);
            }
            catch (Exception ex)
            {
                WriteLine($"Ocurrió un error: {ex.Message}");
            }
        }

        private void ShowClient(Client entity, params Expression<Func<Client, object>>[] includes)
        {
            var properties = typeof(Client).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType.IsGenericType &&
                    property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    var itemList = property.GetValue(entity) as IEnumerable;
                    if (itemList != null)
                    {
                        WriteLine($"\n{property.Name}:");
                        foreach (var item in itemList)
                        {
                            WriteLine("-------------------------------------");
                            var itemProperties = item.GetType().GetProperties();
                            foreach (var itemProperty in itemProperties)
                            {
                                if (!itemProperty.PropertyType.IsClass || itemProperty.PropertyType == typeof(string))
                                {
                                    WriteLine($"    - {itemProperty.Name}: {itemProperty.GetValue(item)}");
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (!property.PropertyType.IsClass || property.PropertyType == typeof(string))
                    {
                        WriteLine($"- {property.Name}: {property.GetValue(entity)}");
                    }
                }
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

        private void HandleDeleteClient()
        {
            try
            {
                int id = Reader.ReadInt("Ingrese el ID del cliente que desea ELIMINAR");

                WriteLine($"\nEstás a punto de eliminar al cliente con ID {id}");
                WriteLine("Esta acción eliminará todas las facturas asociadas a este cliente");
                WriteLine("Esta acción no se puede deshacer");

                string confirmacion = ReadLineWithCountDown(10);

                if (confirmacion != null && confirmacion.ToLower() == "si")
                {
                    _clientService.Delete(id);
                    WriteLine("Cliente eliminado exitosamente.");
                }
                else if (confirmacion == null)
                {
                    WriteLine("Eliminación cancelada por tiempo fuera de espera (10 segundos)");
                }
                else
                {
                    WriteLine("Eliminación cancelada.");
                }
            }
            catch (FormatException)
            {
                WriteLine("Error: El ID debe ser un número.");
            }
            catch (Exception ex)
            {
                WriteLine($"Ocurrió un error inesperado: {ex.Message}");
            }
        }

        private void HandleRegisterInvoice()
        {
            try
            {
                int clientId = Reader.ReadInt("Ingrese el ID del Cliente al que le crearemos la factura");
                string invoiceType = Reader.ReadChar("Ingrese el tipo de factura (A/B/C)").ToString();
                List<Item> items = new List<Item>();

                Invoice draftInvoice = _invoiceService.DraftInvoice(clientId, invoiceType, items);

                WriteLine("\n--- Agregar Productos a la Factura ---");
                HandleAddItems(draftInvoice.Items);

                draftInvoice.CalculateTotalAmount();

                while (true)
                {
                    ShowPreviewInvoice(draftInvoice);
                    char decisionInput = Reader.ReadChar("\n¿Desea modificar la factura antes de cerrarla permanentemente? (S/N)");

                    if (decisionInput == 'S' || decisionInput == 's')
                    {
                        HandleUpdateInvoice(draftInvoice);
                    }
                    else
                    {
                        break;
                    }
                }

                WriteLine("\nCerrando y guardando factura permanentemente...");
                _invoiceService.CreateNewInvoice(draftInvoice);
                WriteLine("✓ ¡Factura registrada exitosamente!");
            }
            catch (Exception ex)
            {
                WriteLine($"✗ Error al registrar la factura: {ex.Message}");
            }
        }

        private void HandleUpdateInvoice(Invoice draftInvoice)
        {
            if (draftInvoice == null)
            {
                WriteLine("✗ Error: Se intentó modificar una factura nula.");
                return;
            }

            WriteLine("\n--- Editando Borrador de Factura ---");
            WriteLine($"1) Cliente: {draftInvoice.Client.LegalName}");
            WriteLine($"2) Tipo: {draftInvoice.Type}");
            WriteLine($"3) Editar Ítems ({draftInvoice.Items.Count} actuales)");
            WriteLine("0) Volver a la revisión");

            int input = Reader.ReadInt("Ingrese una opción");

            switch (input)
            {
                case 1:
                    int clientId = Reader.ReadInt("Ingrese el ID del Cliente para asignarle esta factura");
                    var newClient = _clientService.Search(clientId);
                    if (newClient != null)
                    {
                        draftInvoice.Client = newClient;
                        draftInvoice.ClientId = newClient.Id;
                        WriteLine("✓ Cliente actualizado.");
                    }
                    else
                    {
                        WriteLine($"✗ No se encontró ningún cliente con el ID: {clientId}.");
                    }
                    break;

                case 2:
                    string inputType = Reader.ReadChar("Ingrese el tipo de factura (A/B/C)").ToString();
                    draftInvoice.RegisterTypeFactura(inputType);
                    break;

                case 3:
                    WriteLine("\n--- Gestión de Productos ---");
                    WriteLine("1) Modificar producto existente");
                    WriteLine("2) Agregar nuevo producto");
                    WriteLine("3) Eliminar producto");

                    int itemOption = Reader.ReadInt("Ingrese una opción");

                    switch (itemOption)
                    {
                        case 1:
                            HandleUpdateItemInList(draftInvoice.Items);
                            break;
                        case 2:
                            HandleAddItems(draftInvoice.Items);
                            break;
                        case 3:
                            HandleRemoveItem(draftInvoice.Items);
                            break;
                        default:
                            WriteLine("✗ Opción inválida.");
                            break;
                    }

                    draftInvoice.CalculateTotalAmount();
                    WriteLine($"✓ Total actualizado: ${draftInvoice.AmountTotal:F2}");
                    break;

                case 0:
                    return;

                default:
                    WriteLine("✗ Opción inválida.");
                    break;
            }
        }

        private void ShowPreviewInvoice(Invoice invoice)
        {
            WriteLine("\n══════════════════════════════════════════════════════════════════════════");
            WriteLine("                         VISTA PREVIA DE FACTURA");
            WriteLine("══════════════════════════════════════════════════════════════════════════");
            WriteLine($"Fecha: {invoice.Date:dd/MM/yyyy HH:mm} | Número: {invoice.Number} | Tipo: {invoice.Type}");
            WriteLine("──────────────────────────────────────────────────────────────────────────");
            WriteLine($"Razón Social: {invoice.Client.LegalName}");
            WriteLine($"CUIT/CUIL:    {invoice.Client.CuitCuil}");
            WriteLine($"Domicilio:    {invoice.Client.Address}");
            WriteLine("──────────────────────────────────────────────────────────────────────────");
            WriteLine("PRODUCTOS:");

            if (invoice.Items.Count == 0)
            {
                WriteLine("  (Sin productos agregados)");
            }
            else
            {
                foreach (var item in invoice.Items)
                {
                    WriteLine($"  • {item.Description}");
                    WriteLine($"    Precio: ${item.Price:F2} | Cantidad: {item.Quantity} | Subtotal: ${item.Price * item.Quantity:F2}");
                }
            }

            WriteLine("──────────────────────────────────────────────────────────────────────────");
            WriteLine($"MONTO TOTAL: ${invoice.AmountTotal:F2}");
            WriteLine("══════════════════════════════════════════════════════════════════════════");
        }

        private void HandleAddItems(List<Item> itemsList)
        {
            WriteLine("\n--- Agregar Productos ---");

            while (true)
            {
                var newItem = HandleItemRegistration();

                itemsList.Add(newItem);
                WriteLine("✓ Producto agregado correctamente.");

                char choice = Reader.ReadChar("\n¿Agregar otro producto? (S/N)");
                if (choice != 'S' && choice != 's')
                {
                    break;
                }
            }
        }
        private Item HandleItemRegistration()
        {
            var newItem = new Item();

            while (true)
            {
                try
                {
                    string description = Reader.ReadString("Ingrese el nombre del producto");
                    newItem.Description = description; 
                    break;
                }
                catch (ArgumentException ex)
                {
                    WriteLine($"✗ Error: {ex.Message} Intente de nuevo.");
                }
            }

            while (true)
            {
                try
                {
                    float quantity = Reader.ReadFloat("Ingrese la cantidad");
                    newItem.Quantity = quantity;
                    break; 
                }
                catch (ArgumentException ex)
                {
                    WriteLine($"✗ Error: {ex.Message} Intente de nuevo.");
                }
            }

            while (true)
            {
                try
                {
                    float price = Reader.ReadFloat("Ingrese el precio del producto");
                    newItem.Price = price; 
                    break; 
                }
                catch (ArgumentException ex)
                {
                    WriteLine($"✗ Error: {ex.Message} Intente de nuevo.");
                }
            }
            return newItem;
        }

        private void HandleUpdateItemInList(List<Item> itemsList)
        {
            if (itemsList == null || itemsList.Count == 0)
            {
                WriteLine("✗ No hay items para modificar.");
                return;
            }

            WriteLine("\n--- Items Disponibles ---");
            for (int i = 0; i < itemsList.Count; i++)
            {
                var item = itemsList[i];
                WriteLine($"{i + 1}) {item.Description} - Cantidad: {item.Quantity} - Precio: ${item.Price:F2}");
            }

            int input = Reader.ReadInt("¿Qué item desea modificar?");
            int index = input - 1; 

            if (index < 0 || index >= itemsList.Count)
            {
                WriteLine("✗ Índice inválido.");
                return;
            }

            Item itemToUpdate = itemsList[index];

            WriteLine($"\n--- Modificar: {itemToUpdate.Description} ---");
            WriteLine($"1) Descripción: {itemToUpdate.Description}");
            WriteLine($"2) Cantidad: {itemToUpdate.Quantity}");
            WriteLine($"3) Precio: ${itemToUpdate.Price:F2}");

            int option = Reader.ReadInt("¿Qué desea modificar?");

            bool success = false;
            while (!success)
            {

                switch (option)
                {
                    case 1:
                        string newDescription = Reader.ReadString("Ingrese el nuevo nombre del producto");
                        _itemService.UpdateItemDescription(itemToUpdate, newDescription);
   
                        break;

                    case 2:
                        float newQuantity = Reader.ReadFloat("Ingrese la nueva cantidad");
                        _itemService.UpdateItemQuantity(itemToUpdate, newQuantity);

                        break;

                    case 3:
                        float newPrice = Reader.ReadFloat("Ingrese el nuevo precio");
                        _itemService.UpdateItemPrice(itemToUpdate, newPrice);
                        
                        break;

                    default:
                        WriteLine("✗ Opción inválida.");
                        return;
                }

                    char retry = Reader.ReadChar("¿Desea intentar nuevamente? (S/N)");
                    if (retry != 'S' && retry != 's')
                    {
                        return;
                    }
            }
        }

        private void HandleRemoveItem(List<Item> itemsList)
        {
            if (itemsList == null || itemsList.Count == 0)
            {
                WriteLine("✗ No hay items para eliminar.");
                return;
            }

            WriteLine("\n--- Eliminar Producto ---");
            for (int i = 0; i < itemsList.Count; i++)
            {
                var item = itemsList[i];
                WriteLine($"{i + 1}) {item.Description} - ${item.Price:F2} x {item.Quantity}");
            }

            int input = Reader.ReadInt("¿Qué item desea eliminar?");

            if (_itemService.RemoveItem(itemsList, input - 1))
            {
                WriteLine("✓ Producto eliminado correctamente.");
            }
            else
            {
                WriteLine("✗ Índice inválido.");
            }
        }

        // ========== UTILIDADES ==========
        private string ReadLineWithCountDown(int seconds)
        {
            WriteLine($"Escriba 'si' para confirmar (tiene {seconds} segundos para elegir)");
            bool inputDetected = false;

            for (int i = seconds; i > 0; i--)
            {
                Write($"\rTiempo restante: {i}s...");

                for (int j = 0; j < 10; j++)
                {
                    if (Console.KeyAvailable)
                    {
                        inputDetected = true;
                        break;
                    }
                    Thread.Sleep(100);
                }

                if (inputDetected)
                {
                    break;
                }
            }

            Write("\r" + new string(' ', 40) + "\r");

            if (inputDetected)
            {
                Write("Confirmación: ");
                return Console.ReadLine();
            }
            else
            {
                WriteLine("\nTiempo agotado.");
                return null;
            }
        }
    }
}