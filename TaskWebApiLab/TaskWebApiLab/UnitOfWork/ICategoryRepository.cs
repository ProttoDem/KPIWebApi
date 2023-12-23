using Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace TaskWebApiLab.UnitOfWork
{
    public interface ICategoryRepository : IDisposable
    {
        Task<List<Category>> GetCategories();
        Category GetCategoryByID(int categoryId);
        void InsertCategory(Category category);
        void DeleteCategory(int categoryId);
        void UpdateCategory(Category category);
        void Save();
    }
}
