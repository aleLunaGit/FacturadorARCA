using Parcial3.Modules.Services.Parcial3.Modules.Services;
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

        public void HandleRegisterClient()
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
                Presentator.WriteLine("Cliente registrado exitosamente.");
            }
            catch (Exception ex)
            {
                Presentator.WriteLine($"Error al registrar cliente: {ex.Message}");
            }
        }

        public void HandleUpdateClient()
        {
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
                Presentator.WriteLine("Cliente actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                Presentator.WriteLine($"Ocurrió un error: {ex.Message}");
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
            catch (Exception ex)
            {
                Presentator.WriteLine($"Ocurrió un error: {ex.Message}");
            }
        }

        public void HandleSearchClient()
        {
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
            catch (Exception ex)
            {
                Presentator.WriteLine($"Ocurrió un error: {ex.Message}");
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