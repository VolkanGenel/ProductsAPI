using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.DTO;
using ProductsAPI.Models;

namespace ProductsAPI.Controllers;

// localhost:5000/api/products
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    // private static List<Product>? _products;
    private readonly ProductsContext _context;

    public ProductsController(ProductsContext context)
    {
        _context = context;
        // _products = new List<Product> 
        //     {
        //         new() {ProductId = 1, ProductName = "Iphone 15", Price = 100000, IsActive = true},
        //         new Product() {ProductId = 2, ProductName = "Iphone 16", Price = 110000, IsActive = true},
        //         new Product() {ProductId = 3, ProductName = "Iphone 17", Price = 120000, IsActive = true},
        //         new Product() {ProductId = 4, ProductName = "Iphone 18", Price = 130000, IsActive = true},
        //     };
    }
    
    //  'http://localhost:5238/api/products => GET
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        // if (_products is null)
        // {
        //     return NotFound();
        // }
        // return Ok(_products); //  Aynı şey return _products == null ? new List<Product>() : _products;
        // var products = await _context.Products.Where(i => i.IsActive).Select(p => new
        // {
        //     p.ProductId,
        //     p.ProductName,
        //     p.Price
        // }).ToListAsync();
        var products = await _context.Products.Where(i => i.IsActive).Select(p => MapProductDTO(p)).ToListAsync();
        return Ok(products);
    }
    // http://localhost:5238/api/products/0
    // Product yerine IActionResult gönderirsek durum kodu da gönderme imkanımız olur.
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int? id)
    {
        if (id == null)
        {
            //return StatusCode(404); Aynı şey
            return NotFound();
        }
        var product = await _context.Products.Select(p => MapProductDTO(p)).FirstOrDefaultAsync(i => i.ProductId == id);
        // var product = await _context.Products.FindAsync(id);  // sadece id kullanıldığı için soldaki sorgu kullanıldı var product = await _context.Products.FirstOrDefaultAsync(i => i.ProductId == id);
        // Product? product = _products?.FirstOrDefault(x => x.ProductId == id); // _products? ==> sonuna koyulan ? sayesinde FirstOrDefault _products eğer null değilse çalışır.
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(Product product)
    {
        if (product == null)
        {
            return BadRequest();
        }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product); // GetProduct metotuna git ürünün id bilgisini veriyorum. Eklenen ürünü bana gönder diyorum.
            //CreatedAtAction 201 Http kodlu ürün kaydedildi dönüşü için kullanılır.
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, Product entity)
    {
        if (id != entity.ProductId)
        {
            return BadRequest();
        }
        
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound();
        }
        product.ProductName = entity.ProductName;
        product.Price = entity.Price;
        product.IsActive = entity.IsActive;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return NotFound(e);
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        _context.Products.Remove(product);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return NotFound(e);
        }
        return NoContent();
    }

    private static ProductDTO MapProductDTO(Product product)
    {
        return new ProductDTO
        {
            ProductId = product.ProductId,
            ProductName = product.ProductName,
            Price = product.Price,
        };

    }
}