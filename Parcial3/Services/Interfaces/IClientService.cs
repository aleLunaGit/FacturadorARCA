using Parcial3.Domain.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3.Services.Interfaces
{
    public interface IClientService
    {
        Client FindClientByLegalName(string legalName, params Expression<Func<Client, object>>[] includes);
        Client FindByCuitCuil(string cuitCuil, params Expression<Func<Client, object>>[] includes);
        Client FindByAddress(string address, params Expression<Func<Client, object>>[] includes);
        void RegisterNewClient(Client newClient, List<string> listOfInputs);
    }
}
