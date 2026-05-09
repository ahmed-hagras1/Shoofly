using Shoofly.Data.Entities.Identity;
using System;

namespace Shoofly.Data.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Who is receiving the notification? (Client, Worker, or Coordinator)
        public string UserId { get; set; } = string.Empty;
        public virtual ApplicationUser User { get; set; } = null!;

        // Optional: If the notification is about a specific order, link it!
        public int? OrderId { get; set; }
    }
}