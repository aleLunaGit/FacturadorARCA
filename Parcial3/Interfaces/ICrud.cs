using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3.Interfaces
{
    public interface ICrud:ICrudInvoice
    {
        static void Delete(int id) { }
        static void Update(int id){}
        static void List() { }
    }
    public interface ICrudInvoice {
        static void Register() { }
        static void Search() { }

    }
}
