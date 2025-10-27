using Microsoft.EntityFrameworkCore;
using Parcial3.Interfaces;
using Parcial3.Server;
using System.Linq.Expressions;

namespace Parcial3.Modules.Repositorys
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext  _context;
        private readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context){
            _context = context;
            _dbSet = context.Set<T>();
            }
        public T GetByID(int Id) { 
            
            return _dbSet.Find(Id); 
        }
        public T GetByIdWithIncludes(int id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            // Agrega todos los includes a la consulta
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            // Busca la entidad por su clave primaria. Usamos una expresión lambda genérica.
            // Esto es un poco más complejo pero necesario para que funcione con cualquier entidad.
            var parameter = Expression.Parameter(typeof(T), "e");
            var property = Expression.Property(parameter, "Id");
            var equal = Expression.Equal(property, Expression.Constant(id));
            var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);

            return query.FirstOrDefault(lambda);
        }

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
        public void Delete(int id)
        {
            _dbSet.Remove(_dbSet.Find(id));
            _context.SaveChanges();
        }
    }
}
