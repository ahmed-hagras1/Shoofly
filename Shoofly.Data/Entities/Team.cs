using System;
using System.Collections.Generic;

namespace Shoofly.Data.Entities
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        // Optional: Who is the boss of this team?
        public string? LeaderId { get; set; }

        // ----------------------------------------------------
        // Navigation Properties
        // ----------------------------------------------------

        // 1. The workers who belong to this team
        public virtual ICollection<ServiceProvider> Members { get; set; } = new List<ServiceProvider>();

        // 2. What services is this TEAM qualified to do together? (Many-to-Many)
        public virtual ICollection<Service> OfferedServices { get; set; } = new List<Service>();

        // 3. The specific tasks the Coordinator has assigned to the TEAM
        public virtual ICollection<OrderItem> AssignedTasks { get; set; } = new List<OrderItem>();
    }
}