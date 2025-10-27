using Parcial3.Domain.Implementations;
using Parcial3.Domain.Validators;  
using Parcial3.Repositories.Interfaces;
using Parcial3.Services.Implementations;
using System;                         
using System.Collections.Generic;    
using System.Reflection;

namespace Parcial3.Modules.Services
{
    public class ClientService : CrudService<Client>
    {
        public ClientService(IRepository<Client> entity) : base(entity)
        {
        }

        
        public override void Register(Client entity, List<string> listOfInputs)
        {
            ConvertValues(entity, listOfInputs);

            var validationResult = ClientValidator.Validate(entity);

            if (!validationResult.IsValid)
            {
                throw new Exception(validationResult.ErrorMessage);
            }

            base.Register(entity);
        }
    }
}