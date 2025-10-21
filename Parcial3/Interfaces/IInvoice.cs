using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3.Interfaces
{
    public interface IInvoice : ICrudInvoice
    {
        string GetType();
        string GetNumber();
        DateTime GetDate();
        float GetAmountTotal();
        void SetType(string type);
        void SetNumber();
        void SetDate(DateTime date);
        void SetAmountTotal(float amountTotal);
        void ShowPreviewInvoice();
    }
}
