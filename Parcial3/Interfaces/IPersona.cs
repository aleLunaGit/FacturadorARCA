using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3.Interfaces
{
    public interface IPersona
    {
        void GetCuilCuit();
        void SetCuilCuit(int cuilCuit);
        void GetRazonSocial();
        void SetRazonSocial(int razonSocial);
        void GetDomicilio();
        void SetDomicilio(int domicilio);
    }
}
