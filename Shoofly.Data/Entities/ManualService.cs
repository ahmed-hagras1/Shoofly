using Shoofly.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Data.Entities
{
    /// <summary>
    /// A bookable physical/manual service shown in the catalogue
    /// when a client browses a Manual SubCategory.
    ///
    /// Examples: "Fix Leaking Pipe", "Split AC Deep Cleaning",
    ///           "Full-Home Deep Clean" (team required).
    ///
    /// This replaces the old Service entity, scoped exclusively to Manual flow.
    /// The Technical flow has NO services catalogue — clients interact with
    /// DigitalServiceProvider profiles directly.
    /// </summary>
    public class ManualService
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? CoverPhoto { get; set; }

        public PricingType PricingType { get; set; }
        public decimal? ServiceAmountStart { get; set; }
        public decimal? HourlyRateStart { get; set; }
        public bool RequiresPhysicalAttendance { get; set; } = true;

        // When true, Coordinator must assign a ManualTeam (not a solo provider)
        public bool RequiresTeam { get; set; } = false;

        // ----------------------------------------------------
        // Parent
        // ----------------------------------------------------
        public int SubCategoryId { get; set; }
        public virtual SubCategory SubCategory { get; set; } = null!;

        // ----------------------------------------------------
        // Navigation Properties
        // ----------------------------------------------------

        // Individual providers qualified to perform this service (Many-to-Many)
        public virtual ICollection<ManualServiceProvider> QualifiedProviders { get; set; } = new List<ManualServiceProvider>();

        // Teams qualified to perform this service (Many-to-Many)
        public virtual ICollection<ManualTeam> QualifiedTeams { get; set; } = new List<ManualTeam>();

        // OrderItems that reference this service (placed orders)
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        // CartItems that reference this service (pre-checkout)
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
