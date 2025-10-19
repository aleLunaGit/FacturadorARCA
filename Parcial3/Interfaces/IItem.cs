using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3.Interfaces
{
    public interface IItem
    {
        string GetDescription();
        int GetCuantity();
        int GetPrice();
        void SetDescription(string description);
        void SetCuantity(int cuantity);
        void SetPrice(int Price);
    }
}
