
using System.ComponentModel.DataAnnotations;

namespace Parcial3.Interfaces
{
    public interface IItemService
    {
        // Creación y validación
        (Item item, ValidationResult result) CreateItem(string description, float quantity, float price);

        // Actualización con validación
        ValidationResult UpdateItemDescription(Item item, string newDescription);
        ValidationResult UpdateItemQuantity(Item item, float newQuantity);
        ValidationResult UpdateItemPrice(Item item, float newPrice);

        // Gestión de listas
        (bool success, ValidationResult result) AddItemToList(List<Item> itemsList, Item item);
        Item GetItemByIndex(List<Item> itemsList, int index);
        bool RemoveItem(List<Item> itemsList, int index);

        // Cálculos
        float CalculateItemTotal(Item item);
        float CalculateTotalAmount(List<Item> items);
    }
}
