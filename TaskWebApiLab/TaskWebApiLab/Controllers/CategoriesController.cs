using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using TaskWebApiLab.ApiModels;
using TaskWebApiLab.Auth;
using TaskWebApiLab.UnitOfWork;

namespace TaskWebApiLab.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        //private readonly ApplicationDbContext _context;
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
           // _context = context;
           // _categoryRepository = categoryRepository;
            //this._categoryRepository = new CategoryRepository(context);
            this._categoryRepository = categoryRepository;
        }

        
        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {

            return await _categoryRepository.GetCategories();
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = _categoryRepository.GetCategoryByID(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<CategoryModel>> PostCategory([FromBody]CategoryModel category)
        {
            var CategoryData = new Category
            {
                Description = category.Description,
                Goals = new List<Goal>(),
                Title = category.Title
            };
            _categoryRepository.InsertCategory(CategoryData);
            _categoryRepository.Save();

            return CreatedAtAction("GetCategory", new { id = CategoryData.Id }, category);
        }
    }
}
