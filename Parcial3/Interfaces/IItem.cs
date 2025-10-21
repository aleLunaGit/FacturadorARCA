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
        float GetQuantity();
        float GetPrice();
        void SetDescription(string description);
        void SetQuantity(float cuantity);
        void SetPrice(float Price);
    }
}
