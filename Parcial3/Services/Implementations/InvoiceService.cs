using Parcial3.Domain.Implementations;
using Parcial3.Domain.Interfaces;
using Parcial3.Repositories.Implementations;
using Parcial3.Repositories.Interfaces;
using Parcial3.Services.Interfaces;
using System.Linq.Expressions;

namespace Parcial3.Services.Implementations
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IRepository<Invoice> _invoiceRepository;
        private readonly IRepository<Client> _clientRepository;
        private readonly IUnitOfWork _unitOfWork;

        public InvoiceService(
            IRepository<Invoice> invoiceRepository,
            IRepository<Client> clientRepository,
            IUnitOfWork unitOfWork)
        {
            _invoiceRepository = invoiceRepository;
            _clientRepository = clientRepository;
            _unitOfWork = unitOfWork;
        }

        public Invoice DraftInvoice(int clientId, string invoiceType, List<Item> items)
        {
            
            Client client = _clientRepository.GetByID(clientId);

            if (client == null)
                throw new InvalidOperationException($"Cliente no encontrado con el ID: {clientId}");

            Invoice draftInvoice = new Invoice {
                Client = client,
                Items = items
            };
            draftInvoice.RegisterTypeFactura(invoiceType);
            draftInvoice.Date = DateTime.Now;
            draftInvoice.Number = NumberGenerator();
            CalculateTotalAmount(draftInvoice);

            return draftInvoice;
        }

        public void CreateNewInvoice(Invoice draftInvoice)
        {
            if (draftInvoice == null)
                throw new ArgumentNullException(nameof(draftInvoice), "La información de la factura no puede ser nula.");

            if (draftInvoice.Client == null || draftInvoice.Client.Id <= 0)
                throw new InvalidOperationException("La factura debe estar asociada a un cliente válido.");

            if (string.IsNullOrWhiteSpace(draftInvoice.Type))
                throw new InvalidOperationException("El tipo de factura (A, B, C) es requerido.");

            if (draftInvoice.Items == null || !draftInvoice.Items.Any())
                throw new InvalidOperationException("La factura debe contener al menos un ítem.");

            var clientExists = _clientRepository.GetByID(draftInvoice.Client.Id);
            if (clientExists == null)
                throw new InvalidOperationException($"El cliente con ID {draftInvoice.Client.Id} no existe en la base de datos.");

            _invoiceRepository.Add(draftInvoice);
            _unitOfWork.Save();
        }
        public Invoice SearchWhitIncludes(int id, params Expression<Func<Invoice, object>>[] includes)
        {
            return _invoiceRepository.GetByIdWithIncludes(id, includes);
        }
        public string NumberGenerator()
        {
            string NumberGenerated = "";
            Random rnd = new Random();
            NumberGenerated = DateTime.Now.ToString("ddd") + DateTime.Now.Year + "-" + rnd.Next(100000, 999999);
            return NumberGenerated;
        }
        public void CalculateTotalAmount(Invoice invoice)
        {
            float total = 0;
            foreach (var item in invoice.Items)
            {
                float price = item.Price;
                float quantity = item.Quantity;
                total = total + item.Price * item.Quantity;
            }
            invoice.AmountTotal = total;
        }

        public double GetDiscriminatedIva(Invoice invoice)
        {
            float total = invoice.AmountTotal;
            double iva = 0.21;
            double discriminatedIva = total * iva;
            return discriminatedIva;
        }
        public double GetDiscriminatedTotal(Invoice invoice)
        {
            float total = invoice.AmountTotal;
            double iva = 0.21;
            double discrimantedTotal = total * (1 + iva);
            return discrimantedTotal;
        }
    }
}   