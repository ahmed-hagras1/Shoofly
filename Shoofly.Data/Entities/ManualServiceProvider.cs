using Shoofly.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Data.Entities
{
    /// <summary>
    /// A field worker who performs physical/manual services on-site
    /// (plumbing, electrical, cleaning, AC, etc.).
    ///
    /// Orders are dispatched to them automatically via DispatchAttempt
    /// based on Region, skills (OfferedServices), and IsAvailable.
    /// </summary>
    public class ManualServiceProvider : ApplicationUser
    {
        public ManualServiceProvider()
        {
            OfferedServices = new HashSet<ManualService>();
            AssignedTasks = new HashSet<OrderItem>();
            DispatchAttempts = new HashSet<DispatchAttempt>();
        }

        public string? Bio { get; set; }
        public bool IsAvailable { get; set; } = true;

        // Cached average — recalculated after each completed OrderItem review
        public double AverageRating { get; set; } = 0;

        // HR / admin
        public decimal Salary { get; set; } = 5000m;
        public string? DocumentFileUrl { get; set; }

        // Used to filter eligible providers at dispatch time
        // Example values: "Cairo - Nasr City", "Alex - Smouha", "Giza - Haram"
        public string? Region { get; set; }

        // ----------------------------------------------------
        // Team membership (optional — some providers work solo)
        // ----------------------------------------------------
        public int? TeamId { get; set; }
        public virtual ManualTeam? Team { get; set; }

        // ----------------------------------------------------
        // Navigation Properties
        // ----------------------------------------------------

        // Which manual services can this provider perform? (Many-to-Many)
        public virtual ICollection<ManualService> OfferedServices { get; set; }

        // OrderItems assigned directly to this provider by a Coordinator
        public virtual ICollection<OrderItem> AssignedTasks { get; set; }

        // Every dispatch notification sent to this provider
        public virtual ICollection<DispatchAttempt> DispatchAttempts { get; set; }
    }
}
