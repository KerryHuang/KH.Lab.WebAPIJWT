using KH.Lab.WebAPIJWT.Data;
using KH.Lab.WebAPIJWT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presco.Utility.Caching;

namespace KH.Lab.WebAPIJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private readonly DbContextClass _context;
        private readonly ICache _cache;
        public CacheController(DbContextClass context, ICache cache)
        {
            _context = context;
            _cache = cache;
        }
        [HttpGet]
        [Route("ProductsList")]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            var productCache = new List<Product>();
            productCache = _cache.Get<List<Product>>("Product");
            if (productCache == null)
            {
                var product = await _context.Products.ToListAsync();
                if (product.Count > 0)
                {
                    productCache = product;
                    _cache.Set("Product", productCache);
                }
            }
            return productCache;
        }

        [HttpGet]
        [Route("ProductDetail")]
        public async Task<ActionResult<Product>> Get(int id)
        {
            var productCache = new Product();
            var productCacheList = new List<Product>();
            productCacheList = _cache.Get<List<Product>>("Product");
            productCache = productCacheList.Find(x => x.ProductId == id);
            if (productCache == null)
            {
                productCache = await _context.Products.FindAsync(id);
            }
            return productCache;
        }

        [HttpPost]
        [Route("CreateProduct")]
        public async Task<ActionResult<Product>> POST(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            _cache.Remove("Product");
            return CreatedAtAction(nameof(Get), new { id = product.ProductId }, product);
        }
        [HttpPost]
        [Route("DeleteProduct")]
        public async Task<ActionResult<IEnumerable<Product>>> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            _cache.Remove("Product");
            await _context.SaveChangesAsync();
            return await _context.Products.ToListAsync();
        }

        [HttpPost]
        [Route("UpdateProduct")]
        public async Task<ActionResult<IEnumerable<Product>>> Update(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }
            var productData = await _context.Products.FindAsync(id);
            if (productData == null)
            {
                return NotFound();
            }
            productData.ProductCost = product.ProductCost;
            productData.ProductDescription = product.ProductDescription;
            productData.ProductName = product.ProductName;
            productData.ProductStock = product.ProductStock;
            _cache.Remove("Product");
            await _context.SaveChangesAsync();
            return await _context.Products.ToListAsync();
        }
    }
}
