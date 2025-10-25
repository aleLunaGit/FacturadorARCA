using Parcial3.Modules.Repositorys;
using Parcial3.Modules.Services;
using Parcial3.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3.Modules
{
    public class Presentator
    {
        // Declara las variables para guardar los servicios
        private readonly CrudService<Client> _crudServiceClient;
        private readonly InvoiceService _invoiceService;
        

        // El constructor PIDE los servicios como parámetros
        public Presentator(CrudService<Client> crudServiceClient, InvoiceService invoiceService)
        {
            // AQUÍ es donde la variable deja de ser NULL y recibe el objeto
            _crudServiceClient = crudServiceClient;
            _invoiceService = invoiceService;
        }
        public void Run()
        {
            // Bucle infinito para que el menú siempre vuelva a aparecer
            while (true)
            {
                Console.WriteLine("\n╔══════════════════════════════════╗");
                Console.WriteLine("║        SISTEMA DE GESTIÓN        ║");
                Console.WriteLine("╠══════════════════════════════════╣");
                Console.WriteLine("║ 1. Registrar Nuevo Cliente       ║");
                Console.WriteLine("║ 2. Modificar Cliente Existente   ║");
                Console.WriteLine("║ 3. Buscar Cliente por ID         ║");
                Console.WriteLine("║ 4. Registrar Nueva Factura       ║");
                Console.WriteLine("║ 5. Salir                         ║");
                Console.WriteLine("╚══════════════════════════════════╝");
                Console.Write("Seleccione una opción: ");

                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        _crudServiceClient.Register();
                        break;
                    case "2":
                        HandleUpdateClient(); // Llama al método auxiliar
                        break;
                    case "3":
                        HandleSearchClient(); // Llama al método auxiliar
                        break;
                    case "4":
                        _invoiceService.Register();
                        break;
                    case "5":
                        Console.WriteLine("Saliendo del sistema...");
                        return; // Termina el bucle y la aplicación
                    default:
                        Console.WriteLine("Opción no válida. Por favor, intente de nuevo.");
                        break;
                }
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
            }
        }

        // ... resto de tu clase ...
        private void HandleUpdateClient()
        {
            try
            {
                Console.Write("Ingrese el ID del cliente que desea modificar: ");
                int id = int.Parse(Console.ReadLine());
                _crudServiceClient.Update(id);
            }
            catch (FormatException)
            {
                Console.WriteLine("Error: El ID debe ser un número.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error inesperado: {ex.Message}");
            }
        }
        private void HandleSearchClient()
        {
            try
            {
                Console.Write("Ingrese el ID del cliente que desea buscar con sus facturas: ");
                int id = int.Parse(Console.ReadLine());

                // Llama al método Search y le dice que INCLUYA las facturas del cliente
                _crudServiceClient.Search(id, cliente => cliente.Invoices);
            }
            catch (FormatException)
            {
                Console.WriteLine("Error: El ID debe ser un número.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error inesperado: {ex.Message}");
            }
        }
        /*public void ShowMainMenu()
        {
            Console.WriteLine($"Bienvenido al Facturador Arca");
            IEnumerable<Client>totalClients = repositoryClient.GetAll();
            Console.WriteLine($"Trabajamos con {totalClients.Count()} clientes alrededor del pais");
        }*/
    }
}
