using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3.Interfaces
{
    public interface IItem
    {
        void GetDescription();
        void GetCuantity();
        void GetAmount();
        void SetDescription(string description);
        void SetCuantity(string cuantity);
        void SetAmount(int amount);
    }
}
