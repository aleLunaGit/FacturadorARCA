using Parcial3.Domain.Implementations;
using Parcial3.Repositories.Implementations;
using Parcial3.Repositories.Interfaces;
using Parcial3.Services.Interfaces;
using System.Linq.Expressions;

namespace Parcial3.Services.Implementations
{
    public class ClientService : CrudService<Client>, IClientService
    {

        public ClientService(IRepository<Client> entity, ApplicationDbContext context) : base(entity, context)
        {
        }
        public virtual Client FindClientByLegalName(string legalName, params Expression<Func<Client, object>>[] includes)
            => _repository.GetByProperty(nameof(Client.LegalName), legalName, includes);
        public Client FindByCuitCuil(string cuitCuil, params Expression<Func<Client, object>>[] includes)
            => _repository.GetByProperty(nameof(Client.CuitCuil), cuitCuil, includes);
        public Client FindByAddress(string address, params Expression<Func<Client, object>>[] includes)
            => _repository.GetByProperty(nameof(Client.Address), address, includes);
        public void RegisterNewClient(Client newClient, List<string> listOfInputs)
        {
            ConvertValues(newClient, listOfInputs);
            if (newClient == null) throw new ArgumentNullException(nameof(newClient));

            var existingClient = FindByCuitCuil(newClient.CuitCuil);
            if (existingClient != null) 
                throw new InvalidOperationException($"Ya existe un cliente registrado con el CUIT/CUIL: {newClient.CuitCuil}");
            existingClient = FindByAddress(newClient.Address);

            if (existingClient != null)
                throw new InvalidOperationException($"Ya existe un cliente registrado con el Domicilio: {newClient.Address}");

            existingClient = FindClientByLegalName(newClient.LegalName);
            if (existingClient != null)
                throw new InvalidOperationException($"Ya existe un cliente registrado con esta Razon Social: {newClient.LegalName}");

            _repository.Add(newClient);
            _context.SaveChanges();
        }

    }
}
