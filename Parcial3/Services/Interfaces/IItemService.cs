using Parcial3.Domain.Implementations;

namespace Parcial3.Services.Interfaces
{
    public interface IItemService
    {
        void UpdateItemDescription(Item item, string newDescription);
        void UpdateItemPrice(Item item, float newPrice);
        void UpdateItemQuantity(Item item, float newQuantity);
        bool RemoveItem(List<Item> itemsList, int index);
    }
}