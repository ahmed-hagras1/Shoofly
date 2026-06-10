using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Data.Entities
{
    /// <summary>
    /// A crew of ManualServiceProviders who work together on large physical jobs
    /// (e.g. full-home deep clean, moving, large AC installation).
    ///
    /// The Coordinator can assign an entire ManualTeam to a single OrderItem
    /// when that service has RequiresTeam = true.
    /// </summary>
    public class ManualTeam
    {
        public ManualTeam()
        {
            Members = new HashSet<ManualServiceProvider>();
            QualifiedServices = new HashSet<ManualService>();
            AssignedTasks = new HashSet<OrderItem>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // UserId of the team leader (a ManualServiceProvider)
        public string? LeaderId { get; set; }

        // ----------------------------------------------------
        // Navigation Properties
        // ----------------------------------------------------

        // Workers who belong to this team
        public virtual ICollection<ManualServiceProvider> Members { get; set; }

        // Which manual services is this team qualified to perform? (Many-to-Many)
        public virtual ICollection<ManualService> QualifiedServices { get; set; }

        // OrderItems the Coordinator has assigned to this team
        public virtual ICollection<OrderItem> AssignedTasks { get; set; }
    }
}
