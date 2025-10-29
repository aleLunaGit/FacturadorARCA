using Parcial3.Modules.Services.Parcial3.Modules.Services;
using Parcial3.Services.Implementations;
using System.Collections.Generic;
using System;
using System.Linq;
using Parcial3.Domain.Implementations;
using System.Security.Principal;

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

        public void HandleSearchInvoice()
        {
            try
            {
                int id = Reader.ReadInt("Ingrese el ID del cliente a buscar");
                Client entity = _clientMenu.GetClientService().SearchWhitIncludes(id, x => x.Invoices);

                if (entity == null)
                {
                    Presentator.WriteLine($"No se encontró un cliente con ID {id}");
                    return;
                }

                foreach (var invoiceInfo in entity.Invoices)
                {
                    Presentator.WriteLine($"- Factura ID: {invoiceInfo.Id} | Número: {invoiceInfo.Number} | Tipo: {invoiceInfo.Type} | Monto Total: ${invoiceInfo.AmountTotal:F2}");
                }

                int invoiceId = Reader.ReadInt("Ingrese el ID de la factura a buscar");
                Invoice invoice = _invoiceService.SearchWhitIncludes(invoiceId, x => x.Items);

                if (invoice == null)
                {
                    Presentator.WriteLine($"No se encontró una factura con ID {invoiceId} para el cliente con ID {id}");
                    return;
                }

                Presentator.WriteLine($"\n--- Detalles de la Factura (ID: {invoiceId}) ---");
                ShowPreviewInvoice(invoice, false);
            }
            catch (Exception ex)
            {
                Presentator.WriteLine($"Ocurrió un error: {ex.Message}");
            }
        }

        public void HandleRegisterInvoice()
        {
            try
            {
                int clientId = Reader.ReadInt("Ingrese el ID del Cliente al que le crearemos la factura");
                string invoiceType = Reader.ReadChar("Ingrese el tipo de factura (A/B/C/E)").ToString().ToUpper();
                
                List<Item> items = new List<Item>();

                Invoice draftInvoice = _invoiceService.DraftInvoice(clientId, invoiceType, items);

                Presentator.WriteLine("\n--- Agregar Productos a la Factura ---");
                _itemMenu.HandleAddItems(draftInvoice.Items);

            _invoiceService.CalculateTotalAmount(draftInvoice);

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

                Presentator.WriteLine("\nCerrando y guardando factura permanentemente...");
                _invoiceService.CreateNewInvoice(draftInvoice);
                Presentator.WriteLine("¡Factura registrada exitosamente!");
            }
            catch (Exception ex)
            {
                Presentator.WriteLine($"Error al registrar la factura: {ex.Message}");
            }
        }

        private void HandleUpdateInvoice(Invoice draftInvoice)
        {
            if (draftInvoice == null)
            {
                Presentator.WriteLine("Error: Se intentó modificar una factura nula.");
                return;
            }

            Presentator.WriteLine("\n--- Editando Borrador de Factura ---");
            Presentator.WriteLine($"1) Cliente: {draftInvoice.Client.LegalName}");
            Presentator.WriteLine($"2) Tipo: {draftInvoice.Type}");
            Presentator.WriteLine($"3) Editar Ítems ({draftInvoice.Items.Count} actuales)");
            Presentator.WriteLine("0) Volver a la revisión");

            int input = Reader.ReadInt("Ingrese una opción");

            switch (input)
            {
                case 1:
                    int clientId = Reader.ReadInt("Ingrese el ID del Cliente para asignarle esta factura");
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
                    break;

                case 0:
                    return;

                default:
                    Presentator.WriteLine("Opción inválida.");
                    break;
            }
        }

        private void ShowPreviewInvoice(Invoice invoice, bool isPreview = true)
        {
            Presentator.WriteLine("\n══════════════════════════════════════════════════════════════════════════");
            if (isPreview == true) Presentator.WriteLine("                         VISTA PREVIA DE FACTURA");
            else Presentator.WriteLine("                                FACTURA");
                Presentator.WriteLine("══════════════════════════════════════════════════════════════════════════");
            Presentator.WriteLine($"Fecha: {invoice.Date:dd/MM/yyyy HH:mm} | Número: {invoice.Number} | Tipo: {invoice.Type}");
            Presentator.WriteLine("──────────────────────────────────────────────────────────────────────────");
            Presentator.WriteLine($"Razón Social: {invoice.Client.LegalName}");
            Presentator.WriteLine($"CUIT/CUIL:    {invoice.Client.CuitCuil}");
            Presentator.WriteLine($"Domicilio:    {invoice.Client.Address}");
            Presentator.WriteLine("──────────────────────────────────────────────────────────────────────────");
            Presentator.WriteLine("PRODUCTOS:");

            if (invoice.Items.Count == 0)
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
            => Presentator.WriteLine($"Subtotal: ${_invoiceService.GetDiscriminatedTotal(invoice):F2}" +
                                  $"\nIVA %21: ${_invoiceService.GetDiscriminatedIva(invoice):F2}" +
                                  $"\nTotal: ${invoice.AmountTotal:F2}");
        private void ShowTotalOfTypeE(Invoice invoice)
            => Presentator.WriteLine($"Total sin IVA: ${_invoiceService.GetDiscriminatedTotal(invoice):F2}");
        private void ShowTotalOfTypeB(Invoice invoice)
            => Presentator.WriteLine($"Total: ${invoice.AmountTotal:F2}");
        private void ShowTotalOfTypeC(Invoice invoice)
            => Presentator.WriteLine($"Total: ${invoice.AmountTotal:F2}");
    }
}