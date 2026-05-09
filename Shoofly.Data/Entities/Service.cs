using Shoofly.Shared.Enums;
using System.Collections.Generic;

namespace Shoofly.Data.Entities
{
    public class Service
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal? ServiceAmountStart { get; set; }
        public decimal? HourlyRateStart { get; set; }
        public bool RequiresPhysicalAttendance { get; set; }
        public PricingType PricingType { get; set; }
        public string? CoverPhoto { get; set; }

        public bool RequiresTeam { get; set; } = false;

        // ----------------------------------------------------
        // Navigation Properties
        // ----------------------------------------------------

        // 1. Upward Link: Which SubCategory does this belong to?
        public int SubCategoryId { get; set; }
        public virtual SubCategory SubCategory { get; set; } = null!;

        // 2. Downward Link: Services are now requested inside specific OrderItems!
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        // 3. Downward Link (Many-to-Many): Which INDIVIDUAL workers have the skills to do this?
        public virtual ICollection<ServiceProvider> Providers { get; set; } = new List<ServiceProvider>();

        // 🟢 NEW: Downward Link (Many-to-Many): Which TEAMS have the skills to do this?
        public virtual ICollection<Team> Teams { get; set; } = new List<Team>();

        // 4. Cart Link: Users add this service to their cart before checking out
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}