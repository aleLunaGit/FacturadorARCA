using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Parcial3.Modules.Repositorys;
using Parcial3.Modules.Services;
using Parcial3.Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading; // Necesario para tu contador
using System.Threading.Tasks;
using System.Xml;

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
                Console.WriteLine("\n╔══════════════════════════════════╗");
                Console.WriteLine("║        SISTEMA DE GESTIÓN        ║");
                Console.WriteLine("╠══════════════════════════════════╣");
                Console.WriteLine("║ 1. Registrar Nuevo Cliente       ║");
                Console.WriteLine("║ 2. Modificar Cliente Existente   ║");
                Console.WriteLine("║ 3. Buscar Cliente por ID         ║");
                Console.WriteLine("║ 4. Listar Clientes               ║"); 
                Console.WriteLine("║ 5. Eliminar Cliente              ║"); 
                Console.WriteLine("║ 6. Registrar Nueva Factura       ║"); 
                Console.WriteLine("║ 7. Salir                         ║"); 
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
                        WriteLine("Saliendo del sistema...");
                        return;
                    default:
                        WriteLine("Opción no válida. Por favor, intente de nuevo.");
                        break;
                }
                Reader.ReadChar("\nPresione cualquier tecla para volver al menú...");
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
            Client newClient = new Client();
            List<string> listOfInputs = new List<string>();
            string legalName = Reader.ReadString("Ingresa la Razon Social:");
            string address = Reader.ReadString("Ingrese su Domicilio:");
            string cuit = Reader.ReadString("Ingresa su Cuit/Cuil:");
            listOfInputs.Add(cuit);
            listOfInputs.Add(legalName);
            listOfInputs.Add(address);
            _clientService.Register(newClient, listOfInputs);
        }
        private void HandleUpdateClient()
        {
            try
            {
                int clientId = Reader.ReadInt("Ingrese el ID del cliente que desea modificar");
                var updateClient = _clientService.Search(clientId);
                ShowOptionsToUpdate(updateClient);
                int input = Reader.ReadInt("Ingrese una opcion:");
                var changeTo = Reader.ReadString("Ingrese el nuevo valor");

                _clientService.Update(updateClient, changeTo, input);
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
            foreach (var property in updateProperty)
            {
                WriteLine($"{count}) {property.Name}");
                count++;
            }
        }
        private void ShowClient(Client entity, params Expression<Func<Client, object>>[] includes)
        {
            var properties = _clientService.ListModifyableProperties(entity);
            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType.IsGenericType &&
                    property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    var itemList = property.GetValue(entity) as IEnumerable;
                    foreach (var item in itemList)
                    {
                        Presentator.WriteLine($"-------------------------------------");
                        var itemProperties = item.GetType().GetProperties();
                        foreach (var itemProperty in itemProperties)
                        {
                            if (!itemProperty.PropertyType.IsClass || itemProperty.PropertyType == typeof(string))
                            {
                                Presentator.WriteLine($"    - {itemProperty.Name}: {itemProperty.GetValue(item)}");
                            }
                        }
                    }
                }
                else
                {
                    if (!property.PropertyType.IsClass || property.PropertyType == typeof(string))
                    {
                        Presentator.WriteLine($"- {property.Name}: {property.GetValue(entity)}");
                    }
                }
            }
        }

    
        private void HandleDeleteClient()
        {
            try
            {
            
                int id = Reader.ReadInt("Ingrese el ID del cliente que desea ELIMINAR: ");

             
                WriteLine($"Estas a punto de eliminar al cliente con ID {id}");
                WriteLine("Esta accion eliminara todas las facturas asociadas a este cliente");
                WriteLine("Esta accion no se puede deshacer");

         
                string confirmacion = ReadLineWithCountDown(10);
                if (confirmacion != null && confirmacion.ToLower() == "si")
                {
                    _clientService.Delete(id); 
                }
                else if (confirmacion == null)
                {
                    WriteLine("Eliminacion cancelada por tiempo fuera de espera (10 segundos)");
                }
                else
                {
                    WriteLine("Eliminacion Cancelada");
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

        private void HandleSearchClient()
        {
            try
            {
                int id = Reader.ReadInt("Ingrese el ID del cliente a buscar");
                Presentator.WriteLine($"\n--- Detalles de {typeof(Client).Name} (ID: {id}) ---");
                Client entity = _clientService.Search(id);
                ShowClient(entity, client => client.Invoices);
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

        private void HandleRegisterInvoice()
        {
            Invoice draftInvoice = new Invoice();
            int clientId = Reader.ReadInt("Ingrese el ID del Cliente al que le crearemos la factura: ");
            string invoiceType = Reader.ReadChar("\"Enter the invoice type \\n This can be: A, B or C\"").ToString();
            List<Item> items = new List<Item>();

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
                WriteLine("\nCerrando y guardando factura permanentemente...");
                _invoiceService.CreateNewInvoice(draftInvoice);
                WriteLine("¡Factura registrada exitosamente!");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al registrar la factura: {ex.Message}");
            }
        }

        private void HandleUpdateInvoice(Invoice draftInvoice)
        {
            if (draftInvoice == null)
            {
                Console.WriteLine("Error: Se intentó modificar una factura nula.");
                return;
            }
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
                        WriteLine("1) Modificar Producto");
                        WriteLine("2) Agregar Producto");
                        int inputItems = Reader.ReadInt("Ingrese una opcion:");
                        if (inputItems == 1)
                        {
                            _itemService.UpdateItem(draftInvoice.Items);
                        }
                        if (inputItems == 2)
                        {
                            _itemService.AddItems(draftInvoice.Items);
                        }
                        draftInvoice.CalculateTotalAmount();
                    }
                    break;
                case 0:
                    return;
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
                WriteLine($"Total: ${item.Price * item.Quantity}");
            }
            WriteLine("---------------------------------------------------------------------------");
            WriteLine($"Monto Total: ${invoice.AmountTotal}");
        }

        private string ReadLineWithCountDown(int seconds)
        {
            WriteLine($"Escriba si para confirmar (tiene {seconds} segundos para elegir)");
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
                    Thread.Sleep(100); //espera de 100 milisegundos
                }
                if (inputDetected)
                {
                    break;
                }
            }

            Write("\r" + new string(' ', 40) + "\r");
            if (inputDetected)
            {
                Write("Confirmacion : ");
                return Console.ReadLine(); 
            }
            else
            {
                WriteLine("\n Tiempo Agotado.");
                return null; //se retorna null para indicar tiempo
            }
        }
    }
}