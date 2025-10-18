using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3.Interfaces
{
    public interface IFactura : ICrudFactura
    {
        void GetType();
        void GetNumber();
        void GetDate();
        void GetAmountTotal();
        void SetType(string type);
        void SetNumber(int number);
        void SetDate(DateTime date);
        void SetAmountTotal(int amountTotal);
        void ShowPreviewInvoice();
        void ReadInvoice(int id);
    }
}
