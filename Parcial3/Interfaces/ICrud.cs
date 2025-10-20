using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3.Interfaces
{
    public interface ICrud:ICrudFactura
    {
        static void Delete(int id) { }
        static void Update(int id){}
        static void List() { }
    }
    public interface ICrudFactura {
        static void Register() { }
        static void Read() { }

    }
}
