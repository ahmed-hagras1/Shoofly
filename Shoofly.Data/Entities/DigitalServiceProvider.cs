using Shoofly.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Data.Entities
{
    /// <summary>
    /// A freelancer who offers technical/digital services remotely
    /// (web development, mobile, graphic design, digital marketing, etc.).
    ///
    /// Clients browse their profiles under a SubCategory (Technical flow)
    /// and place a DigitalOrder directly — similar to Khamsat / Mostakel.
    ///
    /// Each provider has their OWN profile, portfolio, and rating, even
    /// when they belong to a DigitalTeam.
    /// </summary>
    public class DigitalServiceProvider : ApplicationUser
    {
        public DigitalServiceProvider()
        {
            OfferedSubCategories = new HashSet<SubCategory>();
            DigitalOrders = new HashSet<DigitalOrder>();
        }

        public string? Bio { get; set; }
        public bool IsAvailable { get; set; } = true;

        // Cached average — recalculated after each completed DigitalOrder review
        public double AverageRating { get; set; } = 0;

        // Freelancer profile fields (shown on their public profile page)
        public string? ProfileTitle { get; set; }   // e.g. "Full-Stack .NET Developer"
        public string? PortfolioUrl { get; set; }
        public decimal? HourlyRate { get; set; }
        public bool IsVerified { get; set; } = false; // Verified badge

        // ----------------------------------------------------
        // Team membership (optional — each member keeps their own profile)
        // ----------------------------------------------------
        public int? TeamId { get; set; }
        public virtual DigitalTeam? Team { get; set; }

        // ----------------------------------------------------
        // Navigation Properties
        // ----------------------------------------------------

        // Which digital sub-categories does this provider list themselves under?
        // (e.g. "Web Development", "Mobile Apps") — (Many-to-Many)
        public virtual ICollection<SubCategory> OfferedSubCategories { get; set; }

        // All digital orders clients have placed with this provider
        public virtual ICollection<DigitalOrder> DigitalOrders { get; set; }
    }
}
