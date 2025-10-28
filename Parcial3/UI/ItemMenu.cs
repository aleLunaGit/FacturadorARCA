using Parcial3.Modules.Services.Parcial3.Modules.Services;
using Parcial3.Domain.Implementations;

namespace Parcial3.Modules
{
    public class ItemMenu
    {
        private readonly ItemService _itemService;

        public ItemMenu(ItemService itemService)
        {
            _itemService = itemService;
        }

        private Item HandleItemRegistration()
        {
            var newItem = new Item();
            while (true)
            {
                try
                {
                    string description = Reader.ReadString("Ingrese el nombre del producto");
                    newItem.Description = description;
                    break;
                }
                catch (ArgumentException ex)
                {
                    Presentator.WriteLine($"Error: {ex.Message} Intente de nuevo.");
                }
            }

            while (true)
            {
                try
                {
                    float quantity = Reader.ReadFloat("Ingrese la cantidad");
                    newItem.Quantity = quantity;
                    break;
                }
                catch (ArgumentException ex)
                {
                    Presentator.WriteLine($"Error: {ex.Message} Intente de nuevo.");
                }
            }

            while (true)
            {
                try
                {
                    float price = Reader.ReadFloat("Ingrese el precio del producto");
                    newItem.Price = price;
                    break;
                }
                catch (ArgumentException ex)
                {
                    Presentator.WriteLine($"Error: {ex.Message} Intente de nuevo.");
                }
            }
            return newItem;
        }

        public void HandleUpdateItemInList(List<Item> itemsList)
        {
            if (itemsList == null || itemsList.Count == 0)
            {
                Presentator.WriteLine("No hay items para modificar.");
                return;
            }

            Presentator.WriteLine("\n--- Items Disponibles ---");
            for (int i = 0; i < itemsList.Count; i++)
            {
                var item = itemsList[i];
                Presentator.WriteLine($"{i + 1}) {item.Description} - Cantidad: {item.Quantity} - Precio: ${item.Price:F2}");
            }

            int input = Reader.ReadInt("¿Qué item desea modificar?");
            int index = input - 1;

            if (index < 0 || index >= itemsList.Count)
            {
                Presentator.WriteLine("Índice inválido.");
                return;
            }
            Item itemToUpdate = itemsList[index];

            Presentator.WriteLine($"\n--- Modificar: {itemToUpdate.Description} ---");
            Presentator.WriteLine($"1) Descripción: {itemToUpdate.Description}");
            Presentator.WriteLine($"2) Cantidad: {itemToUpdate.Quantity}");
            Presentator.WriteLine($"3) Precio: ${itemToUpdate.Price:F2}");

            int option = Reader.ReadInt("¿Qué desea modificar?");

            bool success = false;
            while (!success)
            {
                try
                {
                    switch (option)
                    {
                        case 1:
                            string newDescription = Reader.ReadString("Ingrese el nuevo nombre del producto");
                            itemToUpdate.Description = newDescription;
                            break;

                        case 2:
                            float newQuantity = Reader.ReadFloat("Ingrese la nueva cantidad");
                            itemToUpdate.Quantity = newQuantity;
                            break;

                        case 3:
                            float newPrice = Reader.ReadFloat("Ingrese el nuevo precio");
                            itemToUpdate.Price = newPrice;
                            break;

                        default:
                            Presentator.WriteLine("Opción inválida.");
                            return;
                    }
                    success = true;
                    Presentator.WriteLine("Item actualizado correctamente.");
                }
                catch (ArgumentException ex)
                {
                    Presentator.WriteLine($"Error: {ex.Message}");
                }

                if (!success)
                {
                    char retry = Reader.ReadChar("¿Desea intentar nuevamente? (S/N)");
                    if (retry != 'S' && retry != 's')
                    {
                        return;
                    }
                }
            }
        }

        public void HandleAddItems(List<Item> itemsList)
        {
            Presentator.WriteLine("\n--- Agregar Productos ---");

            while (true)
            {
                var newItem = HandleItemRegistration();

                if (newItem != null)
                {
                    itemsList.Add(newItem);
                    Presentator.WriteLine("Producto agregado correctamente.");
                }

                char choice = Reader.ReadChar("\n¿Agregar otro producto? (S/N)");
                if (choice != 'S' && choice != 's')
                {
                    break;
                }
            }
        }

        public void HandleRemoveItem(List<Item> itemsList)
        {
            if (itemsList == null || itemsList.Count == 0)
            {
                Presentator.WriteLine("No hay items para eliminar.");
                return;
            }

            Presentator.WriteLine("\n--- Eliminar Producto ---");
            for (int i = 0; i < itemsList.Count; i++)
            {
                var item = itemsList[i];
                Presentator.WriteLine($"{i + 1}) {item.Description} - ${item.Price:F2} x {item.Quantity}");
            }

            int input = Reader.ReadInt("¿Qué item desea eliminar?");

            if (_itemService.RemoveItem(itemsList, input - 1))
            {
                Presentator.WriteLine("Producto eliminado correctamente.");
            }
            else
            {
                Presentator.WriteLine("Índice inválido.");
            }
        }
    }
}