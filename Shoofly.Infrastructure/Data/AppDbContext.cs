using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shoofly.Data.Entities;
using Shoofly.Data.Entities.Identity;

namespace Shoofly.Infrastructure.Data
{
    // Inherit from IdentityDbContext if you are using ASP.NET Core Identity for users
    public class AppDbContext : IdentityDbContext<
        ApplicationUser,                // 1. TUser: Your custom user class
        ApplicationRole,                // 2. TRole: Your custom role class
        string,                         // 3. TKey: The primary key type (string for GUID)
        IdentityUserClaim<string>,      // 4. TUserClaim
        IdentityUserRole<string>,       // 5. TUserRole
        IdentityUserLogin<string>,      // 6. TUserLogin
        IdentityRoleClaim<string>,      // 7. TRoleClaim
        IdentityUserToken<string>       // 8. TUserToken
        >
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DbSets (Your Tables)
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        // Added the UserRefreshTokens table for your JWT logic
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // If using IdentityDbContext, you MUST call the base method first
            base.OnModelCreating(modelBuilder);

            // Configure the relationships for the User -> Orders
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Client)
                .WithMany(u => u.ClientOrders)
                .HasForeignKey(o => o.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // 1. Category -> SubCategory (1 to Many)
            modelBuilder.Entity<SubCategory>()
                .HasOne(sc => sc.Category)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(sc => sc.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting a category if it has subcategories

            // 2. SubCategory -> Service (1 to Many)
            modelBuilder.Entity<Service>()
                .HasOne(s => s.SubCategory)
                .WithMany(sc => sc.Services)
                .HasForeignKey(s => s.SubCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // 3. Service -> Order (1 to Many)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Service)
                .WithMany(s => s.Orders)
                .HasForeignKey(o => o.ServiceId)
                .OnDelete(DeleteBehavior.Restrict); // CRITICAL: Never delete financial orders even if the service is deleted

            // 4. Cart -> CartItem (1 to Many)
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.Items)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade); // Safe to cascade: if the cart is cleared, delete the items

            // 5. Service -> CartItem (1 to Many)
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Service)
                .WithMany()
                .HasForeignKey(ci => ci.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // 6. Decimal Precision Configuration
            // EF Core requires you to define the precision for decimal columns to avoid truncation warnings
            modelBuilder.Entity<Service>()
                .Property(s => s.ServiceAmountStart)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Service>()
                .Property(s => s.HourlyRateStart)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalCost)
                .HasColumnType("decimal(18,2)");
        }
    }
}