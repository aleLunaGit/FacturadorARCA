using Parcial3.Interfaces;
using Parcial3.Validators;

namespace Parcial3.Modules.Services
{
    namespace Parcial3.Modules.Services
    {
        public class ItemService : IItemService
        {
            private readonly ItemValidator _validator;

            public ItemService()
            {
                _validator = new ItemValidator();
            }

            // Crea un item con los datos proporcionados y lo valida
            public (Item item, ValidationResult result) CreateItem(string description, float quantity, float price)
            {
                var item = new Item
                {
                    Description = description,
                    Quantity = quantity,
                    Price = price
                };

                var validationResult = _validator.ValidateItem(item);
                return (item, validationResult);
            }

            // Actualiza un item existente con validación
            public ValidationResult UpdateItemDescription(Item item, string newDescription)
            {
                var validationResult = _validator.ValidateDescription(newDescription);
                if (validationResult.IsValid)
                {
                    item.SetDescription(newDescription);
                }
                return validationResult;
            }

            public ValidationResult UpdateItemQuantity(Item item, float newQuantity)
            {
                var validationResult = _validator.ValidateQuantity(newQuantity);
                if (validationResult.IsValid)
                {
                    item.SetQuantity(newQuantity);
                }
                return validationResult;
            }

            public ValidationResult UpdateItemPrice(Item item, float newPrice)
            {
                var validationResult = _validator.ValidatePrice(newPrice);
                if (validationResult.IsValid)
                {
                    item.SetPrice(newPrice);
                }
                return validationResult;
            }

            // Agrega un item a una lista si es válido
            public (bool success, ValidationResult result) AddItemToList(List<Item> itemsList, Item item)
            {
                var validationResult = _validator.ValidateItem(item);
                if (validationResult.IsValid)
                {
                    itemsList.Add(item);
                    return (true, validationResult);
                }
                return (false, validationResult);
            }

            // Calcula el total de un item
            public float CalculateItemTotal(Item item)
            {
                return item.Price * item.Quantity;
            }

            // Calcula el total de todos los items
            public float CalculateTotalAmount(List<Item> items)
            {
                float total = 0;
                foreach (var item in items)
                {
                    total += CalculateItemTotal(item);
                }
                return total;
            }

            // Obtiene un item de la lista por índice
            public Item GetItemByIndex(List<Item> itemsList, int index)
            {
                if (index < 0 || index >= itemsList.Count)
                {
                    return null;
                }
                return itemsList[index];
            }

            // Elimina un item de la lista
            public bool RemoveItem(List<Item> itemsList, int index)
            {
                if (index < 0 || index >= itemsList.Count)
                {
                    return false;
                }
                itemsList.RemoveAt(index);
                return true;
            }

            (Item item, System.ComponentModel.DataAnnotations.ValidationResult result) IItemService.CreateItem(string description, float quantity, float price)
            {
                throw new NotImplementedException();
            }

            System.ComponentModel.DataAnnotations.ValidationResult IItemService.UpdateItemDescription(Item item, string newDescription)
            {
                throw new NotImplementedException();
            }

            System.ComponentModel.DataAnnotations.ValidationResult IItemService.UpdateItemQuantity(Item item, float newQuantity)
            {
                throw new NotImplementedException();
            }

            System.ComponentModel.DataAnnotations.ValidationResult IItemService.UpdateItemPrice(Item item, float newPrice)
            {
                throw new NotImplementedException();
            }

            (bool success, System.ComponentModel.DataAnnotations.ValidationResult result) IItemService.AddItemToList(List<Item> itemsList, Item item)
            {
                throw new NotImplementedException();
            }
        }
        }
    }
