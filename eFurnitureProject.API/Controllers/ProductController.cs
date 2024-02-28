
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Dynamic;
using System.Net.Mime;
using System.Net;
using eFurnitureProject.Application.ViewModels.ProductDTO;

namespace eFurnitureProject.API.Controllers
{
    public class ProductController:BaseController
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
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

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePoduct(CreateProductDTO createProductDTO)
        {
            try
            {
                var result = await _productService.CreateProductByAdmin(createProductDTO);
                dynamic reponseObject = new ExpandoObject();
                reponseObject.StatusCode = 0;
                reponseObject.Result = result;
                if (result.isSuccess)
                {
                    reponseObject.StatusCode = HttpStatusCode.OK;
                    return Ok(reponseObject);
                }
                else
                {
                    reponseObject.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(reponseObject);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var result = await _productService.DeleteProduct(id);
            if (result.isSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet]
        public async Task<IActionResult> ViewAllProduct()
        {
            var result = await _productService.getAllProduct();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> ViewProductById(Guid id)
        {
            var result = await _productService.GetProductByID(id);
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductByAdmin(CreateProductDTO createProductDTO, Guid id)
        {
            var result = await _productService.UpdateProductByAdmin(createProductDTO, id);

            if (result.isSuccess)
            {

                return Ok(result);
            }
            return BadRequest(result);

        }
        [HttpGet("{name}")]
        public async Task<IActionResult> SearchByName(string name)
        {
            var result = await _productService.SearchProductByNameAsync(name);

            if (result.isSuccess)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpGet("{name}")]
        public async Task<IActionResult> SearchByCategoryName(string name)
        {
            var result = await _productService.SearchProductByCategoryNameAsync(name);

            if (result.isSuccess)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
