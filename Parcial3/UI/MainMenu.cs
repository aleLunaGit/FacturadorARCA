
using Parcial3.UI.Implementations;
using Parcial3.UI.Interfaces;

namespace Parcial3.Modules
{
    public class MainMenu
    {
        private readonly IClientMenu _clientMenu;
        private readonly IInvoiceMenu _invoiceMenu;

        public MainMenu(IClientMenu clientMenu, IInvoiceMenu invoiceMenu)
        {
            _clientMenu = clientMenu;
            _invoiceMenu = invoiceMenu;
        }

        public void Run()
        {
            while (true)
            {
                DisplayMenu();

                Presentator.Write("Seleccione una opción: ");
                var option = Reader.ReadString("");

                switch (option)
                {
                    case "1":
                        _clientMenu.HandleRegisterClient();
                        break;
                    case "2":
                        _clientMenu.HandleUpdateClient();
                        break;
                    case "3":
                        _clientMenu.HandleSearchClient();
                        break;
                    case "4":
                        _clientMenu.HandleSearchClientByLegalName();
                        break;
                    case "5":
                        _clientMenu.HandleListClients();
                        break;
                    case "6":
                        _clientMenu.HandleDeleteClient();
                        break;
                    case "7":
                        _invoiceMenu.HandleRegisterInvoice();
                        break;
                    case "8":
                        _invoiceMenu.HandleSearchInvoice();
                        break;
                    case "9":
                        Presentator.WriteLine("Saliendo del sistema...");
                        return;
                    default:
                        Presentator.WriteLine("Opción no válida. Por favor, intente de nuevo.");
                        break;
                }
                Reader.ReadChar("\nPresione cualquier tecla para volver al menú...");
            }
        }

        private void DisplayMenu()
        {
            Presentator.WriteLine("\n╔════════════════════════════════════╗");
            Presentator.WriteLine("║        SISTEMA DE GESTIÓN          ║");
            Presentator.WriteLine("╠════════════════════════════════════╣");
            Presentator.WriteLine("║CLIENTES                            ║");
            Presentator.WriteLine("╠════════════════════════════════════╣");
            Presentator.WriteLine("║ 1. Registrar Nuevo Cliente         ║");
            Presentator.WriteLine("║ 2. Modificar Cliente Existente     ║");
            Presentator.WriteLine("║ 3. Buscar Cliente por ID           ║");
            Presentator.WriteLine("║ 4. Buscar Cliente por Razon Social ║");
            Presentator.WriteLine("║ 5. Listar Clientes                 ║");
            Presentator.WriteLine("║ 6. Eliminar Cliente                ║");
            Presentator.WriteLine("╠════════════════════════════════════╣");
            Presentator.WriteLine("║ FACTURAS                           ║");
            Presentator.WriteLine("╠════════════════════════════════════╣");
            Presentator.WriteLine("║ 7. Registrar Nueva Factura         ║");
            Presentator.WriteLine("║ 8. Consultar Factura por ID        ║");
            Presentator.WriteLine("╠════════════════════════════════════╣");
            Presentator.WriteLine("║ 9. Salir                           ║");
            Presentator.WriteLine("╚════════════════════════════════════╝");
        }
    }
}