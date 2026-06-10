using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Data.Entities
{
    /// <summary>
    /// A collaborative team of DigitalServiceProviders (e.g. a small agency:
    /// backend dev + mobile dev + designer).
    ///
    /// IMPORTANT: Every member must still have their own DigitalServiceProvider
    /// record with their own profile, portfolio, and individual rating.
    /// The DigitalTeam is just a grouping layer for collaborative projects.
    ///
    /// A client can place a DigitalOrder with the team as a whole (via DigitalOrder.TeamId)
    /// rather than targeting a single provider.
    /// </summary>
    public class DigitalTeam
    {
        public DigitalTeam()
        {
            Members = new HashSet<DigitalServiceProvider>();
            DigitalOrders = new HashSet<DigitalOrder>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // UserId of the team leader (a DigitalServiceProvider)
        public string? LeaderId { get; set; }

        // ----------------------------------------------------
        // Navigation Properties
        // ----------------------------------------------------

        // Members — each has their own independent provider profile
        public virtual ICollection<DigitalServiceProvider> Members { get; set; }

        // Digital orders placed with this team
        public virtual ICollection<DigitalOrder> DigitalOrders { get; set; }
    }
}
