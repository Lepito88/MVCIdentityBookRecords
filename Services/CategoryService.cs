using MVCIdentityBookRecords.Data;
using MVCIdentityBookRecords.Models;
using MVCIdentityBookRecords.Interfaces;
using MVCIdentityBookRecords.Responses.Books;
using MVCIdentityBookRecords.Responses.Categories;
using Microsoft.EntityFrameworkCore;

namespace MVCIdentityBookRecords.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetCategoriesResponse> GetCategoriesAsync()
        {
            var categories = await _context.Categories.ToListAsync();

            if (categories.Count == 0)
            {
                return new GetCategoriesResponse
                {
                    Success = false,
                    Error = "No categories found.",
                    ErrorCode = "C01"
                };
            }
            return new GetCategoriesResponse
            {
                Success = true,
                Categories = categories
            };
        }

        public async Task<CategoryResponse> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category is null)
            {
                return new CategoryResponse
                {
                    Success = false,
                    Error = "Category not found",
                    ErrorCode = "C02",
                };
            }

            return new CategoryResponse
            {
                Success = true,
                Idcategory = category.Idcategory,
                CategoryName = category.CategoryName
            };
        }
        public async Task<CategoryResponse> CreateCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);

            var createResponse = await _context.SaveChangesAsync();

            if (createResponse >= 0)
            {
                return new CategoryResponse
                {
                    Success = true,
                    Idcategory = category.Idcategory,
                    CategoryName=category.CategoryName
                };
            }
            return new CategoryResponse
            {
                Success = false,
                Error = "Unable to save Category",
                ErrorCode = "C05"
            };
        }

        public async Task<CategoryResponse> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return new CategoryResponse
                {
                    Success = false,
                    Error = "Category Not Found",
                    ErrorCode = "C02"
                };
            }

            _context.Categories.Remove(category);
            var deleteResponse = await _context.SaveChangesAsync();
            if (deleteResponse >= 0)
            {
                return new CategoryResponse
                {
                    Success = true,
                    Idcategory = category.Idcategory
                };
            }

            return new CategoryResponse
            {
                Success = false,
                Error = "Unable to delete category",
                ErrorCode = "C03"

            };
        }

        public async Task<CategoryResponse> UpdateCategoryAsync(int id, Category category)
        {
            if (id != category.Idcategory)
            {
                return new CategoryResponse
                {
                    Success = false,
                    Error = "Unable to update category",
                    ErrorCode = "C04"
                };
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
                    return new CategoryResponse
                    {
                        Success = false,
                        Error = "Category Not found",
                        ErrorCode = "C02",
                    };
                }
                else
                {
                    throw;
                }
            }

            return new CategoryResponse
            {
                Success = true,
                Idcategory = category.Idcategory,
                CategoryName = category.CategoryName,
            };
        }
        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Idcategory == id);
        }
    }
}
/*
 C01 :  No categories found.
 C02 :  Category not found
 C03 : Unable to delete category
 C04 : Unable to update category
 C05 : Unable to save Category
 
 */