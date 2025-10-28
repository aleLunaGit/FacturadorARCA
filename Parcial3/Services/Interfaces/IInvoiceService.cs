using Parcial3.Domain.Implementations;
using System.Linq.Expressions;

namespace Parcial3.Services.Interfaces
{
    internal interface IInvoiceService
    {
        Invoice DraftInvoice(int clientId, string invoiceType, List<Item> items);
        void CreateNewInvoice(Invoice draftInvoice);
        Invoice SearchWhitIncludes(int id, params Expression<Func<Invoice, object>>[] includes);
        void CalculateTotalAmount(Invoice invoice);
        string NumberGenerator();
    }
}
