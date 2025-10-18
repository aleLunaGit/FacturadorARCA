using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3.Interfaces
{
    public interface ICrud:ICrudFactura
    {
        void Delete();
        void Update();
        void List();
    }
    public interface ICrudFactura {
        void Register();
        void Read();

    }
}
