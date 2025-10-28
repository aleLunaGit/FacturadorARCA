using Parcial3.Domain.Implementations;
using Parcial3.Repositories.Interfaces;
using System.Linq.Expressions;
using System.Reflection;


namespace Parcial3.Services.Implementations
{
    public class ClientService : CrudService<Client>
    {

        public ClientService(IRepository<Client> entity) : base(entity)
        {
        }
        // Recibe el DbContext para poder construir consultas complejas.

        public virtual Client FindClientByLegalName(string legalName, params Expression<Func<Client, object>>[] includes)
        {
            // TODO: Arreglar el principio SRP del Search   
            return _repository.GetByProperty(nameof(Client.LegalName), legalName, includes);
        }
    }
}
