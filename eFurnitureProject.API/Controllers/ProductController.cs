using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;

namespace eFurnitureProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public String Testing() {
            return "Hello world";
        }

        [HttpGet("{page}")]
        public async Task<IActionResult> GetProductsInPage(int page, int amount) {
            var result = await _productService.GetProductsInPageAsync(page, amount);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> FilterProduct(int page, int amount, string searchValue)
        {
            var result = await _productService.GetFilterProductsInPageAsync(page, amount, searchValue);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> ProductDetail(Guid productId)
        {
            var result = await _productService.GetProductDetail(productId);
            return Ok(result);
        }

    }
}
