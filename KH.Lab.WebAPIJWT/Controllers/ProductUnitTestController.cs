using KH.Lab.WebAPIJWT.Models;
using KH.Lab.WebAPIJWT.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KH.Lab.WebAPIJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class ProductUnitTestController : ControllerBase
    {
        private readonly IProductService productService;
        public ProductUnitTestController(IProductService _productService)
        {
            productService = _productService;
        }
        [HttpGet("productlist")]
        public IEnumerable<Product> ProductList()
        {
            var productList = productService.GetProductList();
            return productList;
        }
        [HttpGet("getallproduct")]
        public IActionResult GetAllProducts()
        {
            var productList = productService.GetProductList();
            return Ok(productList);
        }

        [HttpGet("getproductbyid")]
        public Product GetProductById(int Id)
        {
            return productService.GetProductById(Id);
        }
        [HttpPost("addproduct")]
        public Product AddProduct(Product product)
        {
            return productService.AddProduct(product);
        }
        [HttpPut("updateproduct")]
        public Product UpdateProduct(Product product)
        {
            return productService.UpdateProduct(product);
        }
        [HttpDelete("deleteproduct")]
        public bool DeleteProduct(int Id)
        {
            return productService.DeleteProduct(Id);
        }
    }
}
