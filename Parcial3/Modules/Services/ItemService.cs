using Parcial3.Interfaces;

namespace Parcial3.Modules.Services
{
    namespace Parcial3.Modules.Services
    {
        public class ItemService : IItemService
        {
            public Item Item { get; set; }
            public ItemService() { }

            public Item ItemRegister()
            {
                Item item = null;
                bool itemCreated = false;

                while (!itemCreated)
                {
                    try
                    {
                        string description = Reader.ReadString("Ingrese el nombre del producto");
                        float quantity = Reader.ReadFloat("Ingrese la cantidad que compró");
                        float price = Reader.ReadFloat("Ingrese el precio del producto");

                        // El constructor de Item valida automáticamente
                        item = new Item(description, quantity, price);

                        itemCreated = true;
                        Presentator.WriteLine("✓ Item creado exitosamente.");
                    }
                    catch (ArgumentException ex)
                    {
                        // Captura las validaciones lanzadas por Item
                        Presentator.WriteLine(ex.Message);
                        char retry = Reader.ReadChar("\n¿Desea intentar nuevamente? (S/N)");
                        if (retry != 'S' && retry != 's')
                        {
                            throw new OperationCanceledException("Operación cancelada por el usuario.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Presentator.WriteLine($"ERROR inesperado: {ex.Message}");
                        char retry = Reader.ReadChar("\n¿Desea intentar nuevamente? (S/N)");
                        if (retry != 'S' && retry != 's')
                        {
                            throw;
                        }
                    }
                }

                return item;
            }

            public List<Item> UpdateItem(List<Item> itemsList)
            {
                if (itemsList == null || !itemsList.Any())
                {
                    Presentator.WriteLine("ERROR: No hay items para modificar.");
                    return itemsList;
                }

                int count = 1;
                Presentator.WriteLine("\n--- Items disponibles ---");
                foreach (Item item in itemsList)
                {
                    Presentator.WriteLine($"{count}) {item.Description} | Cantidad: {item.Quantity} | Precio: ${item.Price}");
                    count++;
                }

                bool validSelection = false;
                Item itemToUpdate = null;

                while (!validSelection)
                {
                    int itemIndex = Reader.ReadInt("¿Qué item desea modificar?");

                    if (itemIndex < 1 || itemIndex > itemsList.Count)
                    {
                        Presentator.WriteLine($"ERROR: Debe seleccionar un número entre 1 y {itemsList.Count}.");
                    }
                    else
                    {
                        itemToUpdate = itemsList[itemIndex - 1];
                        validSelection = true;
                    }
                }

                Presentator.WriteLine($"\n--- Modificando: {itemToUpdate.Description} ---");
                Presentator.WriteLine($"1) Descripción: {itemToUpdate.Description}");
                Presentator.WriteLine($"2) Cantidad: {itemToUpdate.Quantity}");
                Presentator.WriteLine($"3) Precio: ${itemToUpdate.Price}");

                validSelection = false;
                int optionToUpdate = 0;

                while (!validSelection)
                {
                    optionToUpdate = Reader.ReadInt("¿Qué desea modificar?");

                    if (optionToUpdate < 1 || optionToUpdate > 3)
                    {
                        Presentator.WriteLine("ERROR: Debe seleccionar una opción entre 1 y 3.");
                    }
                    else
                    {
                        validSelection = true;
                    }
                }

                bool updateSuccess = false;
                while (!updateSuccess)
                {
                    try
                    {
                        switch (optionToUpdate)
                        {
                            case 1:
                                var description = Reader.ReadString("Ingrese la nueva descripción");
                                itemToUpdate.SetDescription(description); // Valida automáticamente
                                Presentator.WriteLine("✓ Descripción actualizada correctamente.");
                                break;

                            case 2:
                                float quantity = Reader.ReadFloat("Ingrese la nueva cantidad");
                                itemToUpdate.SetQuantity(quantity); // Valida automáticamente
                                Presentator.WriteLine("✓ Cantidad actualizada correctamente.");
                                break;

                            case 3:
                                float price = Reader.ReadFloat("Ingrese el nuevo precio");
                                itemToUpdate.SetPrice(price); // Valida automáticamente
                                Presentator.WriteLine("✓ Precio actualizado correctamente.");
                                break;
                        }
                        updateSuccess = true;
                    }
                    catch (ArgumentException ex)
                    {
                        // Captura las validaciones de Item
                        Presentator.WriteLine(ex.Message);
                        char retry = Reader.ReadChar("\n¿Desea intentar nuevamente? (S/N)");
                        if (retry != 'S' && retry != 's')
                        {
                            Presentator.WriteLine("Actualización cancelada.");
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Presentator.WriteLine($"ERROR inesperado: {ex.Message}");
                        break;
                    }
                }

                return itemsList;
            }

            public List<Item> AddItems(List<Item> itemsList)
            {
                if (itemsList == null)
                {
                    itemsList = new List<Item>();
                }

                while (true)
                {
                    try
                    {
                        Presentator.WriteLine("\n--- Agregar nuevo item ---");
                        var item = ItemRegister();

                        if (item != null)
                        {
                            itemsList.Add(item);
                            Presentator.WriteLine($"✓ Item '{item.Description}' agregado exitosamente.");
                        }

                        char choice = Reader.ReadChar("\n¿Desea agregar otro item? (X = Sí / Cualquier otra tecla = No)");
                        if (choice != 'X' && choice != 'x')
                        {
                            break;
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        Presentator.WriteLine("Operación de agregar items cancelada.");
                        break;
                    }
                    catch (Exception ex)
                    {
                        Presentator.WriteLine($"ERROR al agregar el item: {ex.Message}");
                        char retry = Reader.ReadChar("¿Desea intentar nuevamente? (S/N)");
                        if (retry != 'S' && retry != 's')
                        {
                            break;
                        }
                    }
                }

                if (itemsList.Any())
                {
                    Presentator.WriteLine($"\n✓ Total de items: {itemsList.Count}");
                }
                else
                {
                    Presentator.WriteLine("\nADVERTENCIA: No se agregaron items.");
                }

                return itemsList;
            }
        }
    }
}