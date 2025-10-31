using Parcial3.Domain.Implementations;
using Parcial3.Repositories.Implementations;
using Parcial3.Repositories.Interfaces;
using Parcial3.Services.Interfaces;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Parcial3.Services.Implementations
{
    public class ClientService : CrudService<Client>, IClientService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ClientService(IRepository<Client> entity, IUnitOfWork unitOfWork) : base(entity, unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public virtual Client FindClientByLegalName(string legalName, params Expression<Func<Client, object>>[] includes)
            => _repository.GetByProperty(nameof(Client.LegalName), legalName, includes);
        public Client FindByCuitCuil(string cuitCuil, params Expression<Func<Client, object>>[] includes)
            => _repository.GetByProperty(nameof(Client.CuitCuil), cuitCuil, includes);
        public Client FindByAddress(string address, params Expression<Func<Client, object>>[] includes)
            => _repository.GetByProperty(nameof(Client.Address), address, includes);
        public void RegisterNewClient(string cuitCuil, string legalName, string address)
        {
            Client newClient = new Client
            {
                CuitCuil = cuitCuil,
                Address = address,
                LegalName = legalName,
            };

            try
            {
                _repository.Add(newClient);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Falló el registro del cliente.");
            }

        }

        public void Update(Client entity, string changeToValue, int inputOption)
        {
            if (entity == null || changeToValue == null || inputOption == null) return;
            var modifiablePropertys = ListModifyableProperties(entity);
            foreach (var prop in modifiablePropertys)
            {
                Client entityFound = _repository.GetByProperty(prop.Name, changeToValue);
                if (entityFound != null)
                {
                    throw new Exception("Entidad encontrada con ese valor");
                }
            }
            var changeProperty = modifiablePropertys.ElementAt(inputOption - 1);
            var convertedValue = Convert.ChangeType(changeToValue, changeProperty.PropertyType);
            changeProperty.SetValue(entity, convertedValue);
            try
            {
                _repository.Update(entity);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar cliente.");
            }
        }
    }
}
