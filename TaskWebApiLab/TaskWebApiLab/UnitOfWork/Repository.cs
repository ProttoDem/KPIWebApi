using Microsoft.EntityFrameworkCore;
using TaskWebApiLab.Auth;

namespace TaskWebApiLab.UnitOfWork
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<T> _dbSet;
        public T GetById(object id)
        {
            return _dbSet.Find(id);
        }
        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }
        public IList<T> GetAll()
        {
            return _dbSet.ToList();
        }
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }
    }
}
