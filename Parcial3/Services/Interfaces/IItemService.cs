using Parcial3.Domain.Implementations;

namespace Parcial3.Services.Interfaces
{
    public interface IItemService
    {
<<<<<<< Updated upstream
        Item CreateItem();
        Item CreateItem(string description, float quantity, float price);
        bool RemoveItem(List<Item> itemsList, int index);
=======
>>>>>>> Stashed changes
        void UpdateItemDescription(Item item, string newDescription);
        void UpdateItemPrice(Item item, float newPrice);
        void UpdateItemQuantity(Item item, float newQuantity);
    }
}