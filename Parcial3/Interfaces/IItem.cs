namespace Parcial3.Interfaces
{
    public interface IItem
    {
        string GetDescription();
        float GetQuantity();
        float GetPrice();
        void SetDescription(string description);
        void SetQuantity(float cuantity);
        void SetPrice(float Price);
    }
}
