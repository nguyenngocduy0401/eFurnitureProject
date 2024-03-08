
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Dynamic;
using System.Net.Mime;
using System.Net;
using eFurnitureProject.Application.ViewModels.ProductDTO;
using System.ComponentModel.DataAnnotations;

namespace eFurnitureProject.API.Controllers
{
    public class ProductController:BaseController
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<Pagination<ProductDTO>>>> FilterProducts2(
          int page,
         [FromQuery] Guid categoryId,
         string? productName,
           double minPrice, double maxPrice,
         int pageSize)
        {
            var response = await _productService.GetAll(page, categoryId, productName, minPrice, maxPrice, pageSize);
            if (response.isSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [HttpPost]
        
        public async Task<IActionResult> CreatePoduct(CreateProductDTO createProductDTO)
        {
            try
            {
                var result = await _productService.CreateProductByAdmin(createProductDTO);
                dynamic reponseObject = new ExpandoObject();
                reponseObject.StatusCode = 1;
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
        [HttpDelete]
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
        [HttpGet]
        public async Task<IActionResult> ViewAllProductNotDeleted()
        {
            var result = await _productService.getAllProductNotdeleted();
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> ViewProductById(Guid id)
        {
            var result = await _productService.GetProductByID(id);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProductByAdmin(CreateProductDTO createProductDTO, Guid id)
        {
            var result = await _productService.UpdateProductByAdmin(createProductDTO, id);

            if (result.isSuccess)
            {

                return Ok(result);
            }
            return BadRequest(result);

        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<int>>> GetTotalPages(int totalItemsCount, int pageSize)=>  await _productService.CalculateTotalPages(totalItemsCount, pageSize);
   

    }
}
