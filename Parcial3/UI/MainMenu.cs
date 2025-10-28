
namespace Parcial3.Modules
{
    public class MainMenu
    {
        private readonly ClientMenu _clientMenu;
        private readonly InvoiceMenu _invoiceMenu;

        public MainMenu(ClientMenu clientMenu, InvoiceMenu invoiceMenu)
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
                        _clientMenu.HandleListClients();
                        break;
                    case "5":
                        _clientMenu.HandleDeleteClient();
                        break;
                    case "6":
                        _invoiceMenu.HandleRegisterInvoice();
                        break;
                    case "7":
                        _invoiceMenu.HandleSearchInvoice();
                        break;
                    case "8":
                        _clientMenu.HandleSearchClientByLegalName();
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
            // Replicamos el menú original exactamente
            Presentator.WriteLine("\n╔════════════════════════════════════╗");
            Presentator.WriteLine("║        SISTEMA DE GESTIÓN          ║");
            Presentator.WriteLine("╠════════════════════════════════════╣");
            Presentator.WriteLine("║ 1. Registrar Nuevo Cliente         ║");
            Presentator.WriteLine("║ 2. Modificar Cliente Existente     ║");
            Presentator.WriteLine("║ 3. Buscar Cliente por ID           ║");
            Presentator.WriteLine("║ 4. Listar Clientes                 ║");
            Presentator.WriteLine("║ 5. Eliminar Cliente                ║");
            Presentator.WriteLine("║ 6. Registrar Nueva Factura         ║");
            Presentator.WriteLine("║ 7. Consultar Factura por ID        ║");
            Presentator.WriteLine("║ 8. Buscar Cliente por Razon Social ║");
            Presentator.WriteLine("║ 9. Salir                           ║");
            Presentator.WriteLine("╚════════════════════════════════════╝");
        }
    }
}