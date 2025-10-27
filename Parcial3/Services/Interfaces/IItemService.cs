using Parcial3.Domain.Implementations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3.Services.Interfaces
{
    public interface IItemService : ICrudService<Item>
    {
        Item CreateItem(string description, float quantity, float price);
        void UpdateItemDescription(Item item, string newDescription);
        void UpdateItemPrice(Item item, float newPrice);
        void UpdateItemQuantity(Item item, float newQuantity);
    }
}
