using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T GetByID(int Id);
        IEnumerable<T> GetAll();
        void Update(T entity);
        void Delete(T entity);
        void Add(T entity);
    }
}
