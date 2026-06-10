using Shoofly.Data.Entities.Identity;
using Shoofly.Shared.Enums;
using System;

namespace Shoofly.Data.Entities
{
    /// <summary>
    /// A push/in-app notification delivered to any user type
    /// (Client, Coordinator, ManualServiceProvider, DigitalServiceProvider).
    ///
    /// The NotificationType tells the mobile app how to handle the tap action
    /// (navigate to an order, open a dispatch request, etc.).
    ///
    /// Only one of OrderId / DigitalOrderId / DispatchAttemptId will be set
    /// at a time — the others will be null.
    /// </summary>
    public class Notification
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public NotificationType Type { get; set; }

        // ----------------------------------------------------
        // Recipient
        // ----------------------------------------------------
        public string UserId { get; set; } = string.Empty;
        public virtual ApplicationUser User { get; set; } = null!;

        // ----------------------------------------------------
        // Deep-link targets (at most one will be set)
        // ----------------------------------------------------

        // For manual order status updates (client & coordinator)
        public int? OrderId { get; set; }

        // For digital order updates (client & provider)
        public int? DigitalOrderId { get; set; }

        // For dispatch requests sent to ManualServiceProviders
        // ("New job available — do you accept?")
        public int? DispatchAttemptId { get; set; }
    }
}