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

        [HttpGet]
        public async Task<ApiResponse<Pagination<CategoryViewModel>>> GetCategoryPagingsion(int pageIndex = 0, int pageSize = 10) => await _categoryService.GetCategoryPagingsionAsync(pageIndex, pageSize);

        [HttpPut]
        public async Task<ApiResponse<CategoryViewModel>> UpdateCategory(Guid categoryId, CreateCategoryViewModel updateCategory) => await _categoryService.UpdateCategoryAsync(categoryId, updateCategory);

        [HttpPut]
        public async Task<ApiResponse<CategoryViewModel>> SoftRemoveCategory(Guid categoryId) => await _categoryService.SoftRemoveCategoryAsync(categoryId);
    }
}
