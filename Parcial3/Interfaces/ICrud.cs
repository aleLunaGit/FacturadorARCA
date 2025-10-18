using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3.Interfaces
{
    public interface ICrud:ICrudFactura
    {
        void Down();
        void Update();
        void List();
    }
    public interface ICrudFactura {
        void Up();
        void Read();

    }
}
