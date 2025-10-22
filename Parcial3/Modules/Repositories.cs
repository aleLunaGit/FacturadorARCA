using Microsoft.EntityFrameworkCore;
using Parcial3.Interfaces;
using Parcial3.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3.Modules
{
    public class Repositories<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext  _context;
        private readonly DbSet<T> _dbSet;

        public Repositories(ApplicationDbContext context){
            _context = context;
            _dbSet = context.Set<T>();
            }
        public T GetByID(int Id) => _dbSet.Find(Id);

        public IEnumerable<T> GetAll() => _dbSet.ToList();
        public void Add(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }
        public void Update(T entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }

    }
}
