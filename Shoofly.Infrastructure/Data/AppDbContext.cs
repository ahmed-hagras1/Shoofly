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

        // --------------------------------------------------------
        // DbSets (Your Tables)
        // --------------------------------------------------------
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Service> Services { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Review> Reviews { get; set; }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Country> Countries { get; set; }

        // Employee and Team Entities
        public DbSet<ServiceProvider> ServiceProviders { get; set; }
        public DbSet<Coordinator> Coordinators { get; set; }
        public DbSet<Team> Teams { get; set; }

        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --------------------------------------------------------
            // 1. Core Platform Relationships
            // --------------------------------------------------------
            modelBuilder.Entity<SubCategory>()
                .HasOne(sc => sc.Category)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(sc => sc.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Service>()
                .HasOne(s => s.SubCategory)
                .WithMany(sc => sc.Services)
                .HasForeignKey(s => s.SubCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // --------------------------------------------------------
            // 2. Order & Checkout Relationships
            // --------------------------------------------------------
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Client)
                .WithMany()
                .HasForeignKey(o => o.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Coordinator)
                .WithMany(c => c.ManagedOrders)
                .HasForeignKey(o => o.CoordinatorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Service)
                .WithMany(s => s.OrderItems)
                .HasForeignKey(oi => oi.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Provider)
                .WithMany(p => p.AssignedTasks)
                .HasForeignKey(oi => oi.ProviderId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.OrderItem)
                .WithOne(oi => oi.Review)
                .HasForeignKey<Review>(r => r.OrderItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // --------------------------------------------------------
            // 3. Cart Relationships
            // --------------------------------------------------------
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.Items)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Service)
                .WithMany(s => s.CartItems)
                .HasForeignKey(ci => ci.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // --------------------------------------------------------
            // 4. Financial & System Relationships
            // --------------------------------------------------------
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Order)
                .WithMany()
                .HasForeignKey(t => t.OrderId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // --------------------------------------------------------
            // 5. Team Relationships
            // --------------------------------------------------------
            modelBuilder.Entity<ServiceProvider>()
                .HasOne(sp => sp.Team)
                .WithMany(t => t.Members)
                .HasForeignKey(sp => sp.TeamId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.AssignedTeam)
                .WithMany(t => t.AssignedTasks)
                .HasForeignKey(oi => oi.TeamId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Team>()
                .HasMany(t => t.OfferedServices)
                .WithMany(s => s.Teams);

            // --------------------------------------------------------
            // 6. Decimal Precision Configuration (Avoids SQL Warnings)
            // --------------------------------------------------------
            modelBuilder.Entity<Service>().Property(s => s.ServiceAmountStart).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Service>().Property(s => s.HourlyRateStart).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Order>().Property(o => o.TotalCost).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<OrderItem>().Property(oi => oi.UnitPrice).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<OrderItem>().Property(oi => oi.SubTotal).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Transaction>().Property(t => t.Amount).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<ServiceProvider>().Property(sp => sp.Salary).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Coordinator>().Property(c => c.Salary).HasColumnType("decimal(18,2)");

            // --------------------------------------------------------
            // 7. 🟢 Enum to String Conversions (Best Practice)
            // --------------------------------------------------------

            // Order Enums
            modelBuilder.Entity<Order>()
                .Property(o => o.OrderStatus)
                .HasConversion<string>();

            modelBuilder.Entity<Order>()
                .Property(o => o.PaymentMethod)
                .HasConversion<string>();

            // Service Enum
            modelBuilder.Entity<Service>()
                .Property(s => s.PricingType)
                .HasConversion<string>();

            // 🟢 NEW: Transaction Enum
            modelBuilder.Entity<Transaction>()
                .Property(t => t.Type) // Assuming your property is named TransactionType
                .HasConversion<string>();
        }
    }
}