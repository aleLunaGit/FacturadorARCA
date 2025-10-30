using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parcial3.Repositories.Implementations;
using Parcial3.Services.Interfaces;

namespace Parcial3.Services.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context) {
            _context = context;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            //_context.Dispose(); Cuando escalemos el proyecto usarémos el dispose para contenedores DI automaticos
            //GC.SuppressFinalize(this); aún no entendí bien para qué sirve esto, mañana veré
        }
    }
}
