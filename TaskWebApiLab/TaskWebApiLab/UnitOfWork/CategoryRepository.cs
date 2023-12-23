using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskWebApiLab.Auth;

namespace TaskWebApiLab.UnitOfWork
{
    public class CategoryRepository : ICategoryRepository, IDisposable
    {
        private ApplicationDbContext context;

        public CategoryRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Task<List<Category>> GetCategories()
        {
            return context.Categories.ToListAsync();
        }

        public Category GetCategoryByID(int id)
        {
            return context.Categories.Find(id);
        }

        public void InsertCategory(Category student)
        {
            context.Categories.Add(student);
        }

        public void DeleteCategory(int studentID)
        {
            Category student = context.Categories.Find(studentID);
            context.Categories.Remove(student);
        }

        public void UpdateCategory(Category student)
        {
            context.Entry(student).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
