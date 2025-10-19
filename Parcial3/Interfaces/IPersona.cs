using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3.Interfaces
{
    public interface IPerson : ICrud
    {
        string GetCuilCuit();
        void SetCuilCuit(string CuitCuil);
        string GetLegalName();
        void SetLegalName(string LegalName);
        string GetAddress();
        void SetAddress(string Address);
    }
}
