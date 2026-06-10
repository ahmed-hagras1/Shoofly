using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shoofly.Data.Entities;
using Shoofly.Data.Entities.Identity;

namespace Shoofly.Infrastructure.Data
{
    // Inherit from IdentityDbContext if you are using ASP.NET Core Identity for users
    public class AppDbContext : IdentityDbContext<
        ApplicationUser,
        ApplicationRole,
        string,
        IdentityUserClaim<string>,
        IdentityUserRole<string>,
        IdentityUserLogin<string>,
        IdentityRoleClaim<string>,
        IdentityUserToken<string>
    >
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // -------------------------------------------------------
        // DbSets
        // -------------------------------------------------------

        // Identity
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
        public DbSet<Country> Countries { get; set; }

        // User subtypes (TPH — all stored in AspNetUsers with a Discriminator column)
        public DbSet<Client> Clients { get; set; }
        public DbSet<Coordinator> Coordinators { get; set; }
        public DbSet<ManualServiceProvider> ManualServiceProviders { get; set; }
        public DbSet<DigitalServiceProvider> DigitalServiceProviders { get; set; }

        // Catalogue
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<ManualService> ManualServices { get; set; }

        // Manual flow
        public DbSet<ManualTeam> ManualTeams { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<DispatchAttempt> DispatchAttempts { get; set; }
        public DbSet<Review> Reviews { get; set; }

        // Digital flow
        public DbSet<DigitalTeam> DigitalTeams { get; set; }
        public DbSet<DigitalOrder> DigitalOrders { get; set; }
        public DbSet<DigitalReview> DigitalReviews { get; set; }

        // Shared
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        // -------------------------------------------------------
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===================================================
            // 1. IDENTITY & COUNTRY
            // ===================================================

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.Country)
                .WithMany(c => c.Users)
                .HasForeignKey(u => u.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            // ===================================================
            // 2. CATALOGUE — Category → SubCategory → ManualService
            // ===================================================

            modelBuilder.Entity<SubCategory>()
                .HasOne(sc => sc.Category)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(sc => sc.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ManualService>()
                .HasOne(s => s.SubCategory)
                .WithMany(sc => sc.ManualServices)
                .HasForeignKey(s => s.SubCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Many-to-Many: ManualService ↔ ManualServiceProvider
            modelBuilder.Entity<ManualService>()
                .HasMany(s => s.QualifiedProviders)
                .WithMany(p => p.OfferedServices)
                .UsingEntity(j => j.ToTable("ManualServiceProviderServices"));

            // Many-to-Many: ManualService ↔ ManualTeam
            modelBuilder.Entity<ManualService>()
                .HasMany(s => s.QualifiedTeams)
                .WithMany(t => t.QualifiedServices)
                .UsingEntity(j => j.ToTable("ManualTeamServices"));

            // ===================================================
            // 3. MANUAL TEAMS
            // ===================================================

            modelBuilder.Entity<ManualServiceProvider>()
                .HasOne(p => p.Team)
                .WithMany(t => t.Members)
                .HasForeignKey(p => p.TeamId)
                .OnDelete(DeleteBehavior.SetNull);

            // ===================================================
            // 4. CART (Manual flow only)
            // ===================================================

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Client)
                .WithOne(cl => cl.Cart)
                .HasForeignKey<Cart>(c => c.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.Items)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.ManualService)
                .WithMany(s => s.CartItems)
                .HasForeignKey(ci => ci.ManualServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // ===================================================
            // 5. MANUAL ORDERS
            // ===================================================

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Client)
                .WithMany(cl => cl.ManualOrders)
                .HasForeignKey(o => o.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Coordinator)
                .WithMany(c => c.ManagedOrders)
                .HasForeignKey(o => o.CoordinatorId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.ManualService)
                .WithMany(s => s.OrderItems)
                .HasForeignKey(oi => oi.ManualServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Provider)
                .WithMany(p => p.AssignedTasks)
                .HasForeignKey(oi => oi.ProviderId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.AssignedTeam)
                .WithMany(t => t.AssignedTasks)
                .HasForeignKey(oi => oi.TeamId)
                .OnDelete(DeleteBehavior.SetNull);

            // ===================================================
            // 6. DISPATCH
            // ===================================================

            modelBuilder.Entity<DispatchAttempt>()
                .HasOne(d => d.OrderItem)
                .WithMany(oi => oi.DispatchAttempts)
                .HasForeignKey(d => d.OrderItemId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DispatchAttempt>()
                .HasOne(d => d.Provider)
                .WithMany(p => p.DispatchAttempts)
                .HasForeignKey(d => d.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);

            // ===================================================
            // 7. REVIEWS (Manual)
            // ===================================================

            modelBuilder.Entity<Review>()
                .HasOne(r => r.OrderItem)
                .WithOne(oi => oi.Review)
                .HasForeignKey<Review>(r => r.OrderItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // ===================================================
            // 8. DIGITAL TEAMS
            // ===================================================

            modelBuilder.Entity<DigitalServiceProvider>()
                .HasOne(p => p.Team)
                .WithMany(t => t.Members)
                .HasForeignKey(p => p.TeamId)
                .OnDelete(DeleteBehavior.SetNull);

            // Many-to-Many: DigitalServiceProvider ↔ SubCategory
            modelBuilder.Entity<DigitalServiceProvider>()
                .HasMany(p => p.OfferedSubCategories)
                .WithMany(sc => sc.DigitalProviders)
                .UsingEntity(j => j.ToTable("DigitalProviderSubCategories"));

            // ===================================================
            // 9. DIGITAL ORDERS
            // ===================================================

            modelBuilder.Entity<DigitalOrder>()
                .HasOne(o => o.Client)
                .WithMany(cl => cl.DigitalOrders)
                .HasForeignKey(o => o.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DigitalOrder>()
                .HasOne(o => o.Provider)
                .WithMany(p => p.DigitalOrders)
                .HasForeignKey(o => o.ProviderId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<DigitalOrder>()
                .HasOne(o => o.Team)
                .WithMany(t => t.DigitalOrders)
                .HasForeignKey(o => o.TeamId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<DigitalOrder>()
                .HasOne(o => o.SubCategory)
                .WithMany(sc => sc.DigitalOrders)
                .HasForeignKey(o => o.SubCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // ===================================================
            // 10. REVIEWS (Digital)
            // ===================================================

            modelBuilder.Entity<DigitalReview>()
                .HasOne(r => r.DigitalOrder)
                .WithOne(o => o.Review)
                .HasForeignKey<DigitalReview>(r => r.DigitalOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // ===================================================
            // 11. NOTIFICATIONS
            // ===================================================

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ===================================================
            // 12. TRANSACTIONS
            // ===================================================

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Order)
                .WithMany(o => o.Transactions)
                .HasForeignKey(t => t.OrderId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.DigitalOrder)
                .WithMany(o => o.Transactions)
                .HasForeignKey(t => t.DigitalOrderId)
                .OnDelete(DeleteBehavior.SetNull);

            // ===================================================
            // 13. DECIMAL PRECISION
            // ===================================================

            modelBuilder.Entity<ManualService>().Property(s => s.ServiceAmountStart).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<ManualService>().Property(s => s.HourlyRateStart).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Order>().Property(o => o.TotalCost).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<OrderItem>().Property(oi => oi.UnitPrice).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<OrderItem>().Property(oi => oi.SubTotal).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<DigitalOrder>().Property(o => o.AgreedPrice).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Transaction>().Property(t => t.Amount).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<ManualServiceProvider>().Property(p => p.Salary).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<DigitalServiceProvider>().Property(p => p.HourlyRate).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Coordinator>().Property(c => c.Salary).HasColumnType("decimal(18,2)");

            // ===================================================
            // 14. ENUM → STRING CONVERSIONS
            // ===================================================

            modelBuilder.Entity<Category>().Property(c => c.Type).HasConversion<string>();
            modelBuilder.Entity<ManualService>().Property(s => s.PricingType).HasConversion<string>();
            modelBuilder.Entity<Order>().Property(o => o.OrderStatus).HasConversion<string>();
            modelBuilder.Entity<Order>().Property(o => o.PaymentMethod).HasConversion<string>();
            modelBuilder.Entity<OrderItem>().Property(oi => oi.ItemStatus).HasConversion<string>();
            modelBuilder.Entity<DispatchAttempt>().Property(d => d.Status).HasConversion<string>();
            modelBuilder.Entity<DigitalOrder>().Property(o => o.Status).HasConversion<string>();
            modelBuilder.Entity<DigitalOrder>().Property(o => o.PaymentMethod).HasConversion<string>();
            modelBuilder.Entity<Transaction>().Property(t => t.Type).HasConversion<string>();
            modelBuilder.Entity<Transaction>().Property(t => t.PaymentMethod).HasConversion<string>();
            modelBuilder.Entity<Notification>().Property(n => n.Type).HasConversion<string>();
        }
    }
}