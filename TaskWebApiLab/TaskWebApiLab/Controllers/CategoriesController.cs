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
        /*
        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }*/

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
            //_context.Categories.Add(CategoryData);
            //await _context.SaveChangesAsync();
            _categoryRepository.Save();

            return CreatedAtAction("GetCategory", new { id = CategoryData.Id }, category);
        }

        /*// DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }*/
    }
}
