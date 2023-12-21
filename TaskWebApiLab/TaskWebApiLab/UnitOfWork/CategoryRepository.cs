using Data.Models;

namespace TaskWebApiLab.UnitOfWork
{
    public class CategoryRepository : IRepository<Category>
    {
        public Category GetById(object id)
        {
            throw new NotImplementedException();
        }

        public IList<Category> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Add(Category entity)
        {
            throw new NotImplementedException();
        }
    }
}
