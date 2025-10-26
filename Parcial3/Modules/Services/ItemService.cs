using Parcial3.Interfaces;

namespace Parcial3.Modules.Services
{
    public class ItemService : IItemService
    {
        public Item Item { get; set; }
        public ItemService() { }
        public Item ItemRegister() {
            var item = new Item
            {
                Description = Reader.ReadString("Ingrese el nombre del producto"),
                Quantity = Reader.ReadFloat("Ingrese la Cantidad que compro"),
                Price = Reader.ReadFloat("Ingrese el Precio del Producto")
            };
            return item;
        }
        public List<Item> UpdateItem(List<Item> itemsList)
        {
            int input; 
            int count = 1;
            
            foreach(Item item in itemsList)
            {
                Presentator.WriteLine($"{count}) {item.Description}");
                count++;
            }
            input = Reader.ReadInt("Que item desea modificar?");
            Item gonnaUpdateThisItem=itemsList.ElementAt(input - 1);
            Presentator.WriteLine($"1) {gonnaUpdateThisItem.Description}\n2) {gonnaUpdateThisItem.Quantity}\n3) {gonnaUpdateThisItem.Price}");
            input = Reader.ReadInt("Que desea modificar de este item?");
            switch (input)
            {
                case 1:
                    var description = Reader.ReadString("Ingrese el nuevo nombre del producto");
                    gonnaUpdateThisItem.SetDescription(description);
                    break;
                case 2:
                    float quantity = Reader.ReadFloat("Ingrese la cantidad del producto");
                    gonnaUpdateThisItem.SetQuantity(quantity);
                    break;
                case 3:
                    float price= Reader.ReadFloat("Ingrese el precio del producto");
                    gonnaUpdateThisItem.SetPrice(price);
                    break;
                default: 
                    break;
            }
            return itemsList;
        }
        public List<Item> AddItems(List<Item> itemsList)
        {
            float totalAmount = 0;
            while (true)
            {
                var item = ItemRegister();
                itemsList.Add(item);

                char choice = Reader.ReadChar("Presione X para agregar mas | Cualquier otra tecla para finalizar");
                if (choice != 'X' && choice != 'x')
                {
                    break; // Salimos del bucle
                }
            }
            return itemsList;
        }

    }
}
