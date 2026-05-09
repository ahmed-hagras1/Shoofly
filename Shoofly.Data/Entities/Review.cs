using System;

namespace Shoofly.Data.Entities
{
    public class Review
    {
        public int Id { get; set; }

        // The star rating (Usually 1 to 5)
        public int Rating { get; set; }

        // Optional text feedback from the client
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // ----------------------------------------------------
        // Navigation Properties
        // ----------------------------------------------------

        // 🟢 THE FIX: Strictly linked to the specific OrderItem!
        // This guarantees that if a Client rates the Plumber 5 stars, 
        // it doesn't accidentally give the Electrician 5 stars too.
        public int OrderItemId { get; set; }
        public virtual OrderItem OrderItem { get; set; } = null!;
    }
}