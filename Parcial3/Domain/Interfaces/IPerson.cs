namespace Parcial3.Domain.Interfaces
{
    public interface IPerson
    {
        string ValidateCuitCuil(string value);
        string ValidateLegalName(string value);
        string ValidateAddress(string value);
    }
}

