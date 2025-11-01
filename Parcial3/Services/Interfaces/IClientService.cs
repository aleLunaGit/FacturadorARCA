using Parcial3.Domain.Implementations;
using System.Linq.Expressions;

namespace Parcial3.Services.Interfaces
{
    public interface IClientService : ICrudService<Client>
    {
        Client FindByAddress(string address, params Expression<Func<Client, object>>[] includes);
        Client FindByCuitCuil(string cuitCuil, params Expression<Func<Client, object>>[] includes);
        Client FindClientByLegalName(string legalName, params Expression<Func<Client, object>>[] includes);
        void RegisterNewClient(string cuitCuil, string legalName, string address);
        void Update(Client entity, string changeToValue, int inputOption);
        IEnumerable<Client> FindClientsByLegalName(string partialName, params Expression<Func<Client, object>>[] includes);
    }
}