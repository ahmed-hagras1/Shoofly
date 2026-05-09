using System;

namespace Shoofly.Data.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }

        // 1. Link back to the Master Order
        public int OrderId { get; set; }
        public virtual Order Order { get; set; } = null!;

        // 2. Link to the specific Service they requested
        public int ServiceId { get; set; }
        public virtual Service Service { get; set; } = null!;

        // 3. Financials (Snapshot of the price at the exact moment of checkout)
        public decimal UnitPrice { get; set; }
        public int? HoursSelected { get; set; }
        public decimal SubTotal { get; set; }

        // ----------------------------------------------------
        // 4. Assignment (Exclusive OR: Person vs. Team)
        // ----------------------------------------------------

        // Option A: Assigned to a single freelancer/worker
        public string? ProviderId { get; set; }
        public virtual ServiceProvider? Provider { get; set; }

        // 🟢 NEW: Option B: Assigned to an entire team for large jobs
        public int? TeamId { get; set; }
        public virtual Team? AssignedTeam { get; set; }

        // ----------------------------------------------------
        // 5. Status & Feedback
        // ----------------------------------------------------

        // Task Status (e.g., "Pending", "Assigned", "Completed", "Cancelled")
        public string ItemStatus { get; set; } = "Pending";

        // 6. The Review specifically for THIS task
        // (Nullable because it is only created after the ItemStatus becomes "Completed")
        public virtual Review? Review { get; set; }
    }
}