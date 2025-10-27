using Parcial3.Domain.Implementations;
using Parcial3.Repositories.Interfaces;
using Parcial3.Services.Implementations;
using Parcial3.Services.Interfaces;

namespace Parcial3.Modules.Services
{
    namespace Parcial3.Modules.Services
    {
        public class ItemService : CrudService<Item>, IItemService
        {

            public ItemService(IRepository<Item> entity) : base(entity)
            {

            }

            // Crea un item con los datos proporcionados y lo valida
            public Item CreateItem()
            {
                return new Item();
            }
            public Item  CreateItem(string description, float quantity, float price)
            {
                var item = new Item
                {
                    Description = description,
                    Quantity = quantity,
                    Price = price
                };
                return item;
            }
            public void AddItem(Item item)
            {

            }

            // Actualiza un item existente con validación
            public void UpdateItemDescription(Item item, string newDescription)
            {

                    item.SetDescription(newDescription);

            }

            public void UpdateItemQuantity(Item item, float newQuantity)
            {

                    item.SetQuantity(newQuantity);

            }

            public void UpdateItemPrice(Item item, float newPrice)
            {
                    item.SetPrice(newPrice);

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
        }
        }
    }
