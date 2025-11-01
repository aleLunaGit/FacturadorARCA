using Parcial3.Modules;
using Parcial3.UI.Interfaces;

namespace Parcial3.UI.Implementations
{
    public class MainMenu : IMainMenu
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
                Presentator.Clear();
                DisplayMenu();
                string option;

                try
                {
                    option = Reader.ReadString("Seleccione una opción");
                }
                catch (OperationCanceledException)
                {
                    option = "3";
                }

                switch (option)
                {
                    case "1":
                        _clientMenu.Run();
                        break;
                    case "2":
                        _invoiceMenu.Run();
                        break;
                    case "3":
                        Presentator.WriteLine("Saliendo del sistema...");
                        return;
                    default:
                        Presentator.WriteLine("Opción no válida. Por favor, intente de nuevo.");
                        Reader.WaitForKey("\nPresione cualquier tecla para continuar...");
                        break;
                }
            }
        }

        private void DisplayMenu()
        {
            Presentator.WriteLine("\n╔════════════════════════════════════╗");
            Presentator.WriteLine("║        SISTEMA DE GESTIÓN          ║");
            Presentator.WriteLine("╠════════════════════════════════════╣");
            Presentator.WriteLine("║ 1. Gestionar Clientes              ║");
            Presentator.WriteLine("║ 2. Gestionar Facturas              ║");
            Presentator.WriteLine("║ 3. Salir                           ║");
            Presentator.WriteLine("╚════════════════════════════════════╝");
        }
    }
}