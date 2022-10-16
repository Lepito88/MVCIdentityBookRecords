using Microsoft.AspNetCore.Mvc;
using MVCIdentityBookRecords.Models;
using MVCIdentityBookRecords.Interfaces;
using MVCIdentityBookRecords.Responses.Categories;
using Microsoft.AspNetCore.Authorization;

namespace MVCIdentityBookRecords.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Basic,Admin")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class CategoriesController : BaseApiController
    {

        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var getCategoriesResponse = await _categoryService.GetCategoriesAsync();
            if (!getCategoriesResponse.Success)
            {
                return NotFound();
            }

            var categoriesResponse = getCategoriesResponse.Categories.ConvertAll(o => new CategoryResponse
            {
                CategoryId = o.CategoryId,
                CategoryName = o.CategoryName,

            });

            return Ok(categoriesResponse);

           
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var categoryResponse = await _categoryService.GetCategoryByIdAsync(id);

            if (!categoryResponse.Success)
            {
                return NotFound();
            }

            return Ok(new CategoryResponse
            {
                CategoryId = categoryResponse.CategoryId,
                CategoryName = categoryResponse.CategoryName,
            });
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.CategoryId)
            {
                return BadRequest();
            }

            var categoryResponse = await _categoryService.UpdateCategoryAsync(id, category);

            if (!categoryResponse.Success)
            {
                return BadRequest(category);
            }

            return NoContent();
        }

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostCategory(Category category)
        {
            var categoryResponse = await _categoryService.CreateCategoryAsync(category);

            if (!categoryResponse.Success)
            {
                return UnprocessableEntity(category);
            }

            return CreatedAtAction(nameof(GetCategory), new { id = category.CategoryId }, category);
          
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var categoryResponse = await _categoryService.DeleteCategoryAsync(id);
            if (!categoryResponse.Success)
            {
                return BadRequest();
            }

            return NoContent();
        }

      
    }
}
