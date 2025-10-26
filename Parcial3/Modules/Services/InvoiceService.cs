using Parcial3.Interfaces;
using Parcial3.Modules.Services;
using System.Net;


namespace Parcial3.Modules.Repositorys
{
    public class InvoiceService : CrudService<Invoice>, IInvoiceService<Invoice>
    {
        
        private readonly IRepository<Client> _clientRepository;
        private readonly ItemService _itemService;

        
        public InvoiceService( IRepository<Invoice> invoiceRepository, IRepository<Client> clientRepository,   ItemService itemService): base(invoiceRepository) 
        {

            _clientRepository = clientRepository;
            _itemService = itemService;
        }
        public void RegisterInvoice(Invoice newInvoice) 
            => _repository.Add(newInvoice);
        public void ChangeClient(Invoice modifyInvoice, int clientId)
        {
            var newClient = _clientRepository.GetByID(clientId);
            if (newClient != null)
            {
                modifyInvoice.Client = newClient;
                modifyInvoice.ClientId = newClient.Id;
            }
            else throw new ArgumentException($"Error: No se encontró ningún cliente con el ID: {clientId}.");
        }
        public void ChangeInvoiceType(Invoice modifyInvoice, string inputType) 
            => modifyInvoice.RegisterTypeFactura(inputType);
        public Invoice DraftInvoice(int clientId, string invoiceType, List<Item> items)
        {
            Invoice draftInvoice = new Invoice();
            Client client = _clientRepository.GetByID(clientId);
            if (client == null)
                throw new InvalidOperationException($"Cliente no encontrado con el ID: {clientId}");
            _itemService.AddItems(items);
            draftInvoice.Client = client;
            draftInvoice.ClientId = clientId;
            draftInvoice.Type = invoiceType;
            draftInvoice.Items = items;
            draftInvoice.Date = DateTime.Now;
            draftInvoice.NumberGenerator();
            draftInvoice.CalculateTotalAmount();
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

            // Verificamos que el cliente realmente exista en nuestra base de datos.
            var clientExists = _clientRepository.GetByID(draftInvoice.Client.Id);
            if (clientExists == null)
                throw new InvalidOperationException($"El cliente con ID {draftInvoice.Client.Id} no existe en la base de datos.");
            draftInvoice.CalculateTotalAmount();
            _repository.Add(draftInvoice);
        }

        // Calculates the total amount.

        // Add Items until users press something diff than x and associates to an invoice


        // Calculates the IVA and TotalAmount per separates
        /* private void TotalTypeA()
        {
            float IVA = AmountTotal / (1 + 21);
            float DiscriminatedTotal = AmountTotal - IVA;
            Console.WriteLine($"Total:{DiscriminatedTotal} | IVA: {IVA}");
        } */
        // Generate an Invoices Number who follow a determinated format

    }
}
