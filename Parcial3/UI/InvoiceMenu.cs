using Parcial3.Modules.Services.Parcial3.Modules.Services;
using Parcial3.Services.Implementations;
using System.Collections.Generic;
using System;
using System.Linq;
using Parcial3.Domain.Implementations;

namespace Parcial3.Modules
{
    public class InvoiceMenu
    {
        private readonly InvoiceService _invoiceService;
        private readonly ClientMenu _clientMenu;
        private readonly ItemMenu _itemMenu;

        public InvoiceMenu(InvoiceService invoiceService, ClientMenu clientMenu, ItemMenu itemMenu)
        {
            _invoiceService = invoiceService;
            _clientMenu = clientMenu;
            _itemMenu = itemMenu;
        }

        public void Run()
        {
            while (true)
            {
                Presentator.Clear();
                DisplayInvoiceMenu();
                string option;

                try
                {
                    option = Reader.ReadString("Seleccione una opción del menú de facturas");
                }
                catch (OperationCanceledException)
                {
                    option = "3";
                }

                switch (option)
                {
                    case "1":
                        HandleRegisterInvoice();
                        break;
                    case "2":
                        HandleSearchInvoice();
                        break;
                    case "3":
                        return;
                    default:
                        Presentator.WriteLine("Opción no válida. Por favor, intente de nuevo.");
                        break;
                }

                Reader.WaitForKey("\nPresione cualquier tecla para volver al menú de facturas...");
            }
        }

        private void DisplayInvoiceMenu()
        {
            Presentator.WriteLine("\n╔════════════════════════════════════╗");
            Presentator.WriteLine("║        GESTIÓN DE FACTURAS         ║");
            Presentator.WriteLine("╠════════════════════════════════════╣");
            Presentator.WriteLine("║ 1. Registrar Nueva Factura         ║");
            Presentator.WriteLine("║ 2. Consultar Factura por ID Cliente║");
            Presentator.WriteLine("║ 3. Volver al Menú Principal        ║");
            Presentator.WriteLine("╚════════════════════════════════════╝");
        }

        public void HandleSearchInvoice()
        {
            Presentator.Clear();
            Presentator.WriteLine("--- Consultar Factura ---");
            Presentator.WriteLine("(Presione 'Escape' en cualquier momento para cancelar)");

            try
            {
                int id = Reader.ReadInt("Ingrese el ID del cliente a buscar");
                Client entity = _clientMenu.GetClientService().SearchWhitIncludes(id, x => x.Invoices);

                if (entity == null)
                {
                    Presentator.WriteLine($"No se encontró un cliente con ID {id}");
                    return;
                }

                if (entity.Invoices == null || !entity.Invoices.Any())
                {
                    Presentator.WriteLine($"El cliente con ID {id} no tiene facturas asociadas.");
                    return;
                }

                foreach (var invoiceInfo in entity.Invoices)
                {
                    Presentator.WriteLine($"- Factura ID: {invoiceInfo.Id} | Número: {invoiceInfo.Number} | Tipo: {invoiceInfo.Type} | Monto Total: ${invoiceInfo.AmountTotal:F2}");
                }

                int invoiceId = Reader.ReadInt("Ingrese el ID de la factura que desea ver en detalle");
                Invoice invoice = _invoiceService.SearchWhitIncludes(invoiceId, x => x.Items);

                if (invoice == null || invoice.ClientId != id)
                {
                    Presentator.WriteLine($"No se encontró una factura con ID {invoiceId} para el cliente con ID {id}");
                    return;
                }

                Presentator.WriteLine($"\n--- Detalles de la Factura (ID: {invoiceId}) ---");
                ShowPreviewInvoice(invoice, false);
            }
            catch (OperationCanceledException)
            {
                Presentator.WriteLine("\nConsulta cancelada.");
            }
            catch (Exception ex)
            {
                Presentator.WriteLine($"\nOcurrió un error: {ex.Message}");
            }
        }

        public void HandleRegisterInvoice()
        {
            Presentator.Clear();
            Presentator.WriteLine("--- Registrar Nueva Factura ---");
            Presentator.WriteLine("(Presione 'Escape' en cualquier momento para cancelar)");

            try
            {
                int clientId = Reader.ReadInt("Ingrese el ID del Cliente al que le crearemos la factura");
                string invoiceType = Reader.ReadChar("Ingrese el tipo de factura (A/B/C/E)").ToString().ToUpper();
                List<Item> items = new List<Item>();

                Invoice draftInvoice = _invoiceService.DraftInvoice(clientId, invoiceType, items);

                _itemMenu.HandleAddItems(draftInvoice.Items);
                _invoiceService.CalculateTotalAmount(draftInvoice);

                bool invoiceCancelled = false;
                while (true)
                {
                    Presentator.Clear();
                    ShowPreviewInvoice(draftInvoice);
                    Presentator.WriteLine("\n--- Opciones de Factura ---");
                    Presentator.WriteLine("1. Guardar Factura Permanentemente");
                    Presentator.WriteLine("2. Modificar Factura Actual");
                    Presentator.WriteLine("3. Cancelar y Volver al Menú Principal");

                    int decisionInput;
                    try
                    {
                        decisionInput = Reader.ReadInt("Seleccione una opción", 1, 3);
                    }
                    catch (OperationCanceledException)
                    {
                        decisionInput = 3;
                    }

                    if (decisionInput == 1)
                    {
                        break;
                    }
                    else if (decisionInput == 2)
                    {
                        if (HandleUpdateInvoice(draftInvoice))
                        {
                            invoiceCancelled = true;
                            break;
                        }
                    }
                    else if (decisionInput == 3)
                    {
                        invoiceCancelled = true;
                        break;
                    }
                }

                if (invoiceCancelled)
                {
                    Presentator.WriteLine("\nCreación de factura cancelada.");
                    return;
                }

                Presentator.WriteLine("\nCerrando y guardando factura permanentemente...");
                _invoiceService.CreateNewInvoice(draftInvoice);
                Presentator.WriteLine("¡Factura registrada exitosamente!");
            }
            catch (OperationCanceledException)
            {
                Presentator.WriteLine("\nCreación de factura cancelada.");
            }
            catch (Exception ex)
            {
                Presentator.WriteLine($"\nError al registrar la factura: {ex.Message}");
            }
        }

        private bool HandleUpdateInvoice(Invoice draftInvoice)
        {
            if (draftInvoice == null)
            {
                Presentator.WriteLine("Error: Se intentó modificar una factura nula.");
                return false;
            }

            Presentator.Clear();
            Presentator.WriteLine("\n--- Editando Borrador de Factura ---");
            Presentator.WriteLine($"1) Cliente: {draftInvoice.Client.LegalName}");
            Presentator.WriteLine($"2) Tipo: {draftInvoice.Type}");
            Presentator.WriteLine($"3) Editar Ítems ({draftInvoice.Items.Count} actuales)");
            Presentator.WriteLine("0) Volver a la revisión");
            Presentator.WriteLine("(Presione 'Escape' para volver a la revisión)");

            int input;
            try
            {
                input = Reader.ReadInt("Ingrese una opción");
            }
            catch (OperationCanceledException)
            {
                return false;
            }

            switch (input)
            {
                case 1:
                    int clientId = Reader.ReadInt("Ingrese el ID del Cliente");
                    var newClient = _clientMenu.GetClientService().Search(clientId);
                    if (newClient != null)
                    {
                        draftInvoice.Client = newClient;
                        draftInvoice.ClientId = newClient.Id;
                        Presentator.WriteLine("Cliente actualizado.");
                    }
                    else
                    {
                        Presentator.WriteLine($"No se encontró ningún cliente con el ID: {clientId}.");
                    }
                    Reader.WaitForKey("Presione una tecla para continuar...");
                    break;
                case 2:
                    string inputType = Reader.ReadChar("Ingrese el tipo de factura (A/B/C/E)").ToString();
                    draftInvoice.RegisterTypeFactura(inputType);
                    break;
                case 3:
                    Presentator.WriteLine("\n--- Gestión de Productos ---");
                    Presentator.WriteLine("1) Modificar producto existente");
                    Presentator.WriteLine("2) Agregar nuevo producto");
                    Presentator.WriteLine("3) Eliminar producto");

                    int itemOption = Reader.ReadInt("Ingrese una opción");
                    switch (itemOption)
                    {
                        case 1:
                            _itemMenu.HandleUpdateItemInList(draftInvoice.Items);
                            break;
                        case 2:
                            _itemMenu.HandleAddItems(draftInvoice.Items);
                            break;
                        case 3:
                            _itemMenu.HandleRemoveItem(draftInvoice.Items);
                            break;
                        default:
                            Presentator.WriteLine("Opción inválida.");
                            break;
                    }
                    _invoiceService.CalculateTotalAmount(draftInvoice);
                    Presentator.WriteLine($"Total actualizado: ${draftInvoice.AmountTotal:F2}");
                    Reader.WaitForKey("Presione una tecla para continuar...");
                    break;
                case 0:
                    return false;
                default:
                    Presentator.WriteLine("Opción inválida.");
                    Reader.WaitForKey("Presione una tecla para continuar...");
                    break;
            }
            return false;
        }

        private void ShowPreviewInvoice(Invoice invoice, bool isPreview = true)
        {
            Presentator.WriteLine("\n══════════════════════════════════════════════════════════════════════════");
            if (isPreview)
                Presentator.WriteLine("                         VISTA PREVIA DE FACTURA");
            else
                Presentator.WriteLine("                                FACTURA");
            Presentator.WriteLine("══════════════════════════════════════════════════════════════════════════");
            Presentator.WriteLine($"Fecha: {invoice.Date:dd/MM/yyyy HH:mm} | Número: {invoice.Number} | Tipo: {invoice.Type}");
            Presentator.WriteLine("──────────────────────────────────────────────────────────────────────────");

            if (invoice.Client != null)
            {
                Presentator.WriteLine($"Razón Social: {invoice.Client.LegalName}");
                Presentator.WriteLine($"CUIT/CUIL:    {invoice.Client.CuitCuil}");
                Presentator.WriteLine($"Domicilio:    {invoice.Client.Address}");
            }
            else
            {
                Presentator.WriteLine($"ID Cliente: {invoice.ClientId} (Cargando datos...)");
            }

            Presentator.WriteLine("──────────────────────────────────────────────────────────────────────────");
            Presentator.WriteLine("PRODUCTOS:");

            if (invoice.Items == null || invoice.Items.Count == 0)
            {
                Presentator.WriteLine("  (Sin productos agregados)");
            }
            else
            {
                foreach (var item in invoice.Items)
                {
                    Presentator.WriteLine($"  • {item.Description}");
                    Presentator.WriteLine($"    Precio: ${item.Price:F2} | Cantidad: {item.Quantity} | Subtotal: ${item.Price * item.Quantity:F2}");
                }
            }

            Presentator.WriteLine("──────────────────────────────────────────────────────────────────────────");
            switch (invoice.Type)
            {
                case "A":
                    ShowTotalOfTypeA(invoice);
                    break;
                case "B":
                    ShowTotalOfTypeB(invoice);
                    break;
                case "C":
                    ShowTotalOfTypeC(invoice);
                    break;
                case "E":
                    ShowTotalOfTypeE(invoice);
                    break;
                default:
                    Presentator.WriteLine($"Total: ${invoice.AmountTotal:F2}");
                    break;
            }
            Presentator.WriteLine("══════════════════════════════════════════════════════════════════════════");
        }

        private void ShowTotalOfTypeA(Invoice invoice)
        {
            double iva = _invoiceService.GetDiscriminatedIva(invoice);
            double discriminatedTotal = _invoiceService.GetDiscriminatedTotal(invoice);

            Presentator.WriteLine($"Subtotal: ${discriminatedTotal:F2}");
            Presentator.WriteLine($"IVA 21%: ${iva:F2}");
            Presentator.WriteLine($"Total: ${invoice.AmountTotal:F2}");
        }

        private void ShowTotalOfTypeE(Invoice invoice)
        {
            Presentator.WriteLine($"Total (Exportación): ${invoice.AmountTotal :F2}");
        }

        private void ShowTotalOfTypeB(Invoice invoice)
        {
            Presentator.WriteLine($"Total: ${invoice.AmountTotal:F2}");
        }

        private void ShowTotalOfTypeC(Invoice invoice)
        {
            Presentator.WriteLine($"Total: ${invoice.AmountTotal:F2}");
        }
    }
}