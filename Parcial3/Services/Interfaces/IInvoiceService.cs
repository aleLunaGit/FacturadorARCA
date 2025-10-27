using Parcial3.Domain.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3.Services.Interfaces
{
    internal interface IInvoiceService
    {
        Invoice DraftInvoice(int clientId, string invoiceType, List<Item> items);
        void CreateNewInvoice(Invoice draftInvoice);
    }
}
