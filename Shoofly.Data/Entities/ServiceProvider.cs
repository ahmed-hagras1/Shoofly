using Shoofly.Data.Entities.Identity;
using System.Collections.Generic;

namespace Shoofly.Data.Entities
{
    public class ServiceProvider : ApplicationUser
    {
        // 1. Worker Profile Details
        public string? Bio { get; set; }
        public bool IsAvailable { get; set; } = true;

        // Cached property to keep your API fast
        public double AverageRating { get; set; }

        // 2. HR & Admin Information
        public string? DocumentFileUrl { get; set; }
        public decimal Salary { get; set; } = 5000m;

        // ----------------------------------------------------
        // Navigation Properties
        // ----------------------------------------------------

        // 3. What skills does this worker have? (e.g., Plumbing, Electrical)
        public virtual ICollection<Service> OfferedServices { get; set; } = new List<Service>();

        // 4. The specific Tasks (OrderItems) the Coordinator has assigned to them
        public virtual ICollection<OrderItem> AssignedTasks { get; set; } = new List<OrderItem>();

        // 🟢 NEW: Team Integration
        // Nullable because some workers are individuals, while others belong to a group.
        public int? TeamId { get; set; }
        public virtual Team? Team { get; set; }
    }
}