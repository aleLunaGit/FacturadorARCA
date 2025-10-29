using Parcial3.Services.Implementations;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using System;
using System.Linq;
using Parcial3.Domain.Implementations;

namespace Parcial3.Modules
{
    public class ClientMenu
    {
        private readonly ClientService _clientService;

        public ClientMenu(ClientService clientService)
        {
            _clientService = clientService;
        }
        public ClientService GetClientService() => _clientService;

        public void Run()
        {
            while (true)
            {
                Presentator.Clear();
                DisplayClientMenu();
                string option;

                try
                {
                    option = Reader.ReadString("Seleccione una opción del menú de clientes");
                }
                catch (OperationCanceledException)
                {
                    option = "7";
                }

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
                        HandleSearchClientByLegalName();
                        break;
                    case "5":
                        HandleListClients();
                        break;
                    case "6":
                        HandleDeleteClient();
                        break;
                    case "7":
                        return;
                    default:
                        Presentator.WriteLine("Opción no válida. Por favor, intente de nuevo.");
                        break;
                }

                Reader.WaitForKey("\nPresione cualquier tecla para volver al menú de clientes...");
            }
        }

        private void DisplayClientMenu()
        {
            Presentator.WriteLine("\n╔════════════════════════════════════╗");
            Presentator.WriteLine("║         GESTIÓN DE CLIENTES        ║");
            Presentator.WriteLine("╠════════════════════════════════════╣");
            Presentator.WriteLine("║ 1. Registrar Nuevo Cliente         ║");
            Presentator.WriteLine("║ 2. Modificar Cliente Existente     ║");
            Presentator.WriteLine("║ 3. Buscar Cliente por ID           ║");
            Presentator.WriteLine("║ 4. Buscar Cliente por Razón Social ║");
            Presentator.WriteLine("║ 5. Listar Clientes                 ║");
            Presentator.WriteLine("║ 6. Eliminar Cliente                ║");
            Presentator.WriteLine("║ 7. Volver al Menú Principal        ║");
            Presentator.WriteLine("╚════════════════════════════════════╝");
        }

        private string ReadAndValidateField(Func<string, string> validator, string prompt)
        {
            while (true)
            {
                try
                {
                    string input = Reader.ReadString(prompt);
                    validator(input);
                    return input;
                }
                catch (ArgumentException ex)
                {
                    Presentator.WriteLine(ex.Message);
                }
            }
        }

        public void HandleRegisterClient()
        {
            Presentator.Clear();
            Presentator.WriteLine("--- Registrar Nuevo Cliente ---");
            Presentator.WriteLine("(Presione 'Escape' en cualquier momento para cancelar)");

            try
            {
                Client newClient = new Client();

                string cuit = ReadAndValidateField(newClient.ValidateCuitCuil, "Ingresa su Cuit/Cuil");
                string legalName = ReadAndValidateField(newClient.ValidateLegalName, "Ingresa la Razón Social");
                string address = ReadAndValidateField(newClient.ValidateAddress, "Ingrese su Domicilio");

                _clientService.RegisterNewClient(cuit, legalName, address);
                Presentator.WriteLine("\n¡Cliente registrado exitosamente!");
            }
            catch (OperationCanceledException)
            {
                Presentator.WriteLine("\nRegistro cancelado.");
            }
            catch (Exception ex)
            {
                Presentator.WriteLine($"\nError al registrar cliente: {ex.Message}");
            }
        }

        public void HandleUpdateClient()
        {
            Presentator.Clear();
            Presentator.WriteLine("--- Modificar Cliente ---");
            Presentator.WriteLine("(Presione 'Escape' en cualquier momento para cancelar)");

            try
            {
                int clientId = Reader.ReadInt("Ingrese el ID del cliente que desea modificar");
                var updateClient = _clientService.Search(clientId);

                if (updateClient == null)
                {
                    Presentator.WriteLine($"No se encontró un cliente con ID {clientId}");
                    return;
                }

                ShowOptionsToUpdate(updateClient);
                int input = Reader.ReadInt("Ingrese una opción");
                var changeTo = Reader.ReadString("Ingrese el nuevo valor");

                _clientService.Update(updateClient, changeTo, input);
                Presentator.WriteLine("\n¡Cliente actualizado exitosamente!");
            }
            catch (OperationCanceledException)
            {
                Presentator.WriteLine("\nModificación cancelada.");
            }
            catch (Exception ex)
            {
                Presentator.WriteLine($"\nOcurrió un error: {ex.Message}");
            }
        }

        private void ShowOptionsToUpdate(Client entity)
        {
            if (_clientService == null) return;

            var updateProperty = _clientService.ListModifyableProperties(entity);
            int count = 1;

            Presentator.WriteLine("\n--- Propiedades Modificables ---");
            foreach (var property in updateProperty)
            {
                Presentator.WriteLine($"{count}) {property.Name}");
                count++;
            }
        }

        public void HandleSearchClientByLegalName()
        {
            Presentator.Clear();
            Presentator.WriteLine("--- Buscar Cliente por Razón Social ---");
            Presentator.WriteLine("(Presione 'Escape' en cualquier momento para cancelar)");

            try
            {
                string clientName = Reader.ReadString("Ingrese la Razón Social del Cliente");
                Client client = _clientService.FindClientByLegalName(clientName, c => c.Invoices);

                if (client == null)
                {
                    Presentator.WriteLine($"Error: No se encontró ningún cliente con el nombre '{clientName}'.");
                    return;
                }

                ShowClient(client);
            }
            catch (OperationCanceledException)
            {
                Presentator.WriteLine("\nBúsqueda cancelada.");
            }
            catch (Exception ex)
            {
                Presentator.WriteLine($"\nOcurrió un error: {ex.Message}");
            }
        }

        public void HandleSearchClient()
        {
            Presentator.Clear();
            Presentator.WriteLine("--- Buscar Cliente por ID ---");
            Presentator.WriteLine("(Presione 'Escape' en cualquier momento para cancelar)");

            try
            {
                int id = Reader.ReadInt("Ingrese el ID del cliente a buscar");
                Client entity = _clientService.SearchWhitIncludes(id, x => x.Invoices);

                if (entity == null)
                {
                    Presentator.WriteLine($"No se encontró un cliente con ID {id}");
                    return;
                }

                Presentator.WriteLine($"\n--- Detalles de {typeof(Client).Name} (ID: {id}) ---");
                ShowClient(entity, client => client.Invoices);
            }
            catch (OperationCanceledException)
            {
                Presentator.WriteLine("\nBúsqueda cancelada.");
            }
            catch (Exception ex)
            {
                Presentator.WriteLine($"\nOcurrió un error: {ex.Message}");
            }
        }

        private void ShowClient(Client entity, params Expression<Func<Client, object>>[] includes)
        {
            var properties = typeof(Client).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                _clientService.ShouldSkipPropertie(property);

                if (property.PropertyType.IsGenericType &&
                    property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    var itemList = property.GetValue(entity) as IEnumerable;
                    if (itemList != null)
                    {
                        Presentator.WriteLine($"\n{property.Name}:");

                        if (itemList.Cast<object>().Any())
                        {
                            foreach (var item in itemList)
                            {
                                Presentator.WriteLine("-------------------------------------");
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
                            Presentator.WriteLine("  (No hay elementos)");
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

        public void HandleListClients()
        {
            Presentator.Clear();
            var clientes = _clientService.GetAll();
            Presentator.WriteLine("\n--- LISTADO DE CLIENTES ---");

            if (clientes != null && clientes.Any())
            {
                foreach (var cliente in clientes)
                {
                    Presentator.WriteLine($"ID: {cliente.Id} | Nombre: {cliente.LegalName} | CUIT: {cliente.CuitCuil}");
                }
            }
            else
            {
                Presentator.WriteLine("No hay clientes registrados.");
            }
        }

        public void HandleDeleteClient()
        {
            Presentator.Clear();
            Presentator.WriteLine("--- Eliminar Cliente ---");

            try
            {
                int id = Reader.ReadInt("Ingrese el ID del cliente que desea ELIMINAR");

                Presentator.WriteLine($"\nEstás a punto de eliminar al cliente con ID {id}");
                Presentator.WriteLine("Esta acción eliminará todas las facturas asociadas a este cliente");
                Presentator.WriteLine("Esta acción no se puede deshacer");

                string confirmacion = Presentator.ReadLineWithCountDown(10);

                if (confirmacion != null && confirmacion.ToLower() == "si")
                {
                    _clientService.Delete(id);
                    Presentator.WriteLine("Cliente eliminado exitosamente.");
                }
                else if (confirmacion == null)
                {
                    Presentator.WriteLine("Eliminación cancelada por tiempo fuera de espera (10 segundos)");
                }
                else
                {
                    Presentator.WriteLine("Eliminación cancelada.");
                }
            }
            catch (OperationCanceledException)
            {
                Presentator.WriteLine("\nEliminación cancelada.");
            }
            catch (FormatException)
            {
                Presentator.WriteLine("Error: El ID debe ser un número.");
            }
            catch (Exception ex)
            {
                Presentator.WriteLine($"Ocurrió un error inesperado: {ex.Message}");
            }
        }
    }
}