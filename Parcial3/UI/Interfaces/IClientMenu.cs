using Parcial3.Services.Interfaces;

namespace Parcial3.UI.Interfaces
{
    public interface IClientMenu
    {
        IClientService GetClientService();
        void HandleDeleteClient();
        void HandleListClients();
        void HandleRegisterClient();
        void HandleSearchClient();
        void HandleSearchClientByLegalName();
        void HandleUpdateClient();
        void Run();
        string ValidateAddress();
        string ValidateCuit();
        string ValidateLegalName();
    }
}