using Parcial3.Domain.Implementations;
using Parcial3.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3.Interfaces
{
    public interface IItemService : ICrudService<Item>
    {
        (bool success, ValidationResult result) AddItemToList(List<Item> itemsList, Item item);
        (Item item, ValidationResult result) CreateItem(string description, float quantity, float price);
        ValidationResult UpdateItemDescription(Item item, string newDescription);
        ValidationResult UpdateItemPrice(Item item, float newPrice);
        ValidationResult UpdateItemQuantity(Item item, float newQuantity);
    }
}
