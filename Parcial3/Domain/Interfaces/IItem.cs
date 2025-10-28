namespace Parcial3.Domain.Interfaces
{
    public interface IItem
    {
        string ValidateDescription(string value);
        float ValidateQuantity(float value);
        float ValidatePrice(float value);
    }
}
