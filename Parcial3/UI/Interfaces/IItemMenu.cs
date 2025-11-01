using Parcial3.Domain.Implementations;

namespace Parcial3.UI.Interfaces
{
    public interface IItemMenu
    {
        void HandleAddItems(List<Item> itemsList);
        void HandleRemoveItem(List<Item> itemsList);
        void HandleUpdateItemInList(List<Item> itemsList);
    }
}