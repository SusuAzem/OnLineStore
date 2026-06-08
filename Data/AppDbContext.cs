using Core;

using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Data
{
    public class AppDbContext : DbContext, IDataProtectionKeyContext
    {
        public AppDbContext()
        {
        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
        }        

        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }
        public DbSet<ShoppingCartLine> ShoppingCartLines { get; set; }
        public DbSet<Payment> Payments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(p => {
                p.HasOne(p => p.ProductType).WithMany(pt => pt.Products)
                    .HasForeignKey(p => p.TypeId).IsRequired();
                p.Property(p=>p.Name).HasMaxLength(150);
                p.Property(p => p.TypeId).HasDefaultValue(0);
                p.Property(p => p.Price).HasPrecision(10, 2);
            });

            modelBuilder.Entity<OrderHeader>(o => { 
                o.HasOne(o => o.User).WithMany(u => u.OrderHeaders)
                    .HasForeignKey(o => o.UserNameIdentifier).IsRequired();

                o.HasMany(o => o.OrderItems).WithOne(i => i.OrderHeader)
                .HasForeignKey(o => o.OrderHeaderId).IsRequired();

                o.HasOne(o=>o.Payment).WithOne(p=>p.OrderHeader)
                    .HasForeignKey<OrderHeader>(o => o.PaymentId);
            });

            modelBuilder.Entity<OrderItem>(o => { 
                o.Property(o=>o.OrderHeaderId).IsRequired();
                o.HasOne(o => o.Product).WithMany().HasForeignKey(o => o.ProductId);              
            });

            modelBuilder.Entity<ShoppingCartLine>(s => {
                s.Property(s=>s.ProductId).IsRequired();
                s.HasOne(s => s.Product).WithMany().HasForeignKey(o => o.ProductId);

                s.Property(s => s.UserNameIdentifier).IsRequired();
                s.HasOne(s => s.User).WithMany().HasForeignKey(o => o.UserNameIdentifier);
            });

            base.OnModelCreating(modelBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
        }
    }
}
