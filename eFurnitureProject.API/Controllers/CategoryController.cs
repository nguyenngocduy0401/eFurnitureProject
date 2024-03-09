using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.CategoryViewModels;
using Microsoft.AspNetCore.Mvc;

namespace eFurnitureProject.API.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<ApiResponse<CategoryViewModel>> CreateCategory(CreateCategoryViewModel createCategoryViewModel) => await _categoryService.CreateCategoryAsync(createCategoryViewModel);

        [HttpPut]
        public async Task<ApiResponse<CategoryViewModel>> UpdateCategory(string categoryId, CreateCategoryViewModel updateCategory) => await _categoryService.UpdateCategoryAsync(categoryId, updateCategory);

        [HttpPut]
        public async Task<ApiResponse<CategoryViewModel>> SoftRemoveCategory(string categoryId) => await _categoryService.SoftRemoveCategoryAsync(categoryId);

        [HttpGet]
        public async Task<ApiResponse<List<CategoryViewModel>>> GetCategories() => await _categoryService.GetAllCategoryAsync();
    }
}
