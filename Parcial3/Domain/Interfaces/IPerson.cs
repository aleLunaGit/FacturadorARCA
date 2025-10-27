namespace Parcial3.Domain.Interfaces
{
    public interface IPerson
    {
        string GetCuilCuit();
        void SetCuilCuit(string CuitCuil);
        string GetLegalName();
        void SetLegalName(string LegalName);
        string GetAddress();
        void SetAddress(string Address);
    }
}

