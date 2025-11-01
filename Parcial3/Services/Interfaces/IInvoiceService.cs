using Parcial3.Domain.Implementations;
using System.Linq.Expressions;

namespace Parcial3.Services.Interfaces
{
    public interface IInvoiceService
    {
        void CalculateTotalAmount(Invoice invoice);
        void CreateNewInvoice(Invoice draftInvoice);
        Invoice DraftInvoice(int clientId, string invoiceType, List<Item> items);
        double GetDiscriminatedIva(Invoice invoice);
        double GetDiscriminatedTotal(Invoice invoice);
        string NumberGenerator();
        Invoice SearchWhitIncludes(int id, params Expression<Func<Invoice, object>>[] includes);
    }
}