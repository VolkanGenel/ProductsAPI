using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProductsAPI.Models;

public class ProductsContext:IdentityDbContext<AppUser,AppRole,int>
// IdentityDbContext<AppUser,AppRole,int> en sondaki int Primary keyin tipini soruyor. SppUser ve AppRole için int ayarladığımdan int yazdım. Defaultta string bu yüzden ide hata verir.
{
    public DbSet<Product> Products { get; set; }
    
    //Dışarıdan context string göndermek için constructor inject yapıldı.
    public ProductsContext(DbContextOptions<ProductsContext> options): base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Product>().HasData(new Product()  {ProductId = 1, ProductName = "Iphone 15", Price = 100000, IsActive = true });
        modelBuilder.Entity<Product>().HasData(new Product()   { ProductId = 2, ProductName = "Iphone 16", Price = 110000, IsActive = true });
        modelBuilder.Entity<Product>().HasData(new Product()  { ProductId = 3, ProductName = "Iphone 17", Price = 120000, IsActive = true });
        modelBuilder.Entity<Product>().HasData(new Product()   { ProductId = 4, ProductName = "Iphone 18", Price = 130000, IsActive = false });
    }
}