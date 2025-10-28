using Parcial3.Domain.Implementations;
using Parcial3.Repositories.Implementations;
using Parcial3.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Parcial3.Services.Implementations
{
    public class ClientService : CrudService<Client>
    {

        public ClientService(IRepository<Client> entity, ApplicationDbContext context) : base(entity, context)
        {
        }
        // Recibe el DbContext para poder construir consultas complejas.

        public virtual Client FindClientByLegalName(string legalName, params Expression<Func<Client, object>>[] includes)
        {
            return _repository.GetByProperty(nameof(Client.LegalName), legalName, includes);
        }
        public Client CheckIfCuitOrCuilExists(string cuitCuil, params Expression<Func<Client, object>>[] includes)
        {
            return _repository.GetByProperty(nameof(Client.CuitCuil), cuitCuil, includes);
        }
        public Client CheckIfAddressExists(string address, params Expression<Func<Client, object>>[] includes)
        {
            return _repository.GetByProperty(nameof(Client.Address), address, includes);
        }
        
    }
}
