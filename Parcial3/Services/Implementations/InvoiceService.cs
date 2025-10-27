using Parcial3.Domain.Implementations;
using Parcial3.Modules.Services.Parcial3.Modules.Services;
using Parcial3.Repositories.Interfaces;
using Parcial3.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Parcial3.Services.Implementations
{
    public class InvoiceService : CrudService<Invoice>, IInvoiceService<Invoice>
    {
        private readonly IRepository<Client> _clientRepository;
        private ItemService itemService;

        public InvoiceService(IRepository<Invoice> invoiceRepository, IRepository<Client> clientRepository)
            : base(invoiceRepository)
        {
            _clientRepository = clientRepository;
        }

        public InvoiceService(IRepository<Invoice> invoiceRepository, IRepository<Client> clientRepository, ItemService itemService) : this(invoiceRepository, clientRepository)
        {
            this.itemService = itemService;
        }

        // Crea un borrador de factura (sin items, se agregan después desde el menú)
        public Invoice DraftInvoice(int clientId, string invoiceType, List<Item> items)
        {
            Invoice draftInvoice = new Invoice();
            Client client = _clientRepository.GetByID(clientId);

            if (client == null)
                throw new InvalidOperationException($"Cliente no encontrado con el ID: {clientId}");

            // NO llamamos a AddItems aquí, eso lo maneja el ItemMenu
            draftInvoice.Client = client;
            draftInvoice.ClientId = clientId;
            draftInvoice.Type = invoiceType;
            draftInvoice.Items = items; // Asignamos la lista vacía
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

            // Verificamos que el cliente realmente exista en nuestra base de datos
            var clientExists = _clientRepository.GetByID(draftInvoice.Client.Id);
            if (clientExists == null)
                throw new InvalidOperationException($"El cliente con ID {draftInvoice.Client.Id} no existe en la base de datos.");

            _repository.Add(draftInvoice);
        }
    }
}   