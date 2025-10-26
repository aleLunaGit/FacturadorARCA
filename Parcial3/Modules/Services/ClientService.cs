using Parcial3.Interfaces;


namespace Parcial3.Modules.Services
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
