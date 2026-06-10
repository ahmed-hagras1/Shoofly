using System;

namespace Shoofly.Data.Entities
{
    /// <summary>
    /// A client's star rating + comment for a completed manual service task.
    ///
    /// Strictly tied to one OrderItem — so rating "the plumber" in a
    /// mixed order does NOT accidentally rate "the electrician" too.
    /// Created only after OrderItem.ItemStatus == Completed.
    /// </summary>
    public class Review
    {
        public int Id { get; set; }

        public int Rating { get; set; }        // 1–5
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // One-to-one with OrderItem
        public int OrderItemId { get; set; }
        public virtual OrderItem OrderItem { get; set; } = null!;
    }

    /// <summary>
    /// A client's star rating + comment for a completed digital/freelance order.
    /// One-to-one with DigitalOrder (created only after Status == Completed).
    /// </summary>
    public class DigitalReview
    {
        public int Id { get; set; }

        public int Rating { get; set; }        // 1–5
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // One-to-one with DigitalOrder
        public int DigitalOrderId { get; set; }
        public virtual DigitalOrder DigitalOrder { get; set; } = null!;
    }
}