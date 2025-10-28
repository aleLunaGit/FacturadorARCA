namespace Parcial3.Domain.Interfaces
{
    public interface IInvoice
    {
        void RegisterTypeFactura(string inputType);
        string GetType();
        string GetNumber();
        DateTime GetDate();
        float GetAmountTotal();
        void SetType(string type);
        void SetDate(DateTime date);
        void SetAmountTotal(float amountTotal);
    }
}
