using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3.Services.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
        void Dispose();
    }
}
