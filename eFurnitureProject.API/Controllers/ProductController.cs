
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

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductDTO>>>> FilterProducts2(
          int page,
         [FromQuery] List<Guid> categoryId,
         string? productName,
          int amount,
         int pageSize)
        {
            var response = await _productService.GetAll(page, categoryId, productName, amount, pageSize);
            if (response.isSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
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
       
       
    }
}
