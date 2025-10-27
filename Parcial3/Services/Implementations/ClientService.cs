using Parcial3.Domain.Implementations;
using Parcial3.Repositories.Interfaces;
using System.Reflection;


namespace Parcial3.Services.Implementations
{
    public class ClientService : CrudService<Client>
    {
        private readonly IRepository<Client> repositories;

        public ClientService(IRepository<Client> entity) : base(entity)
        {
        }
        // Recibe el DbContext para poder construir consultas complejas.
    }
}
