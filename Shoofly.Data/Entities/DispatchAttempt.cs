using Shoofly.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Data.Entities
{
    /// <summary>
    /// Records every attempt to assign a ManualServiceProvider to an OrderItem.
    ///
    /// DISPATCH FLOW:
    ///   1. Coordinator triggers dispatch on an OrderItem.
    ///   2. System queries ManualServiceProviders filtered by:
    ///        - Region matches the order's Area
    ///        - OfferedServices contains the ManualService
    ///        - IsAvailable == true
    ///        - No existing Accepted DispatchAttempt for this OrderItem
    ///   3. Best match is selected → DispatchAttempt created (Status = Sent).
    ///   4. Push notification sent to the provider.
    ///
    ///   5a. Provider accepts  → Status = Accepted
    ///                         → OrderItem.ProviderId = this.ProviderId
    ///                         → OrderItem.ItemStatus = Assigned
    ///                         → provider.IsAvailable = false
    ///
    ///   5b. Provider declines → Status = Declined
    ///                         → System picks next candidate → new DispatchAttempt
    ///
    ///   5c. No response in timeout window → Status = Expired
    ///                                     → System picks next candidate → new DispatchAttempt
    /// </summary>
    public class DispatchAttempt
    {
        public int Id { get; set; }

        // ----------------------------------------------------
        // Which task is being dispatched?
        // ----------------------------------------------------
        public int OrderItemId { get; set; }
        public virtual OrderItem OrderItem { get; set; } = null!;

        // ----------------------------------------------------
        // Which provider was notified?
        // ----------------------------------------------------
        public string ProviderId { get; set; } = string.Empty;
        public virtual ManualServiceProvider Provider { get; set; } = null!;

        // ----------------------------------------------------
        // Timing & Outcome
        // ----------------------------------------------------
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public DateTime? RespondedAt { get; set; }

        public DispatchStatus Status { get; set; } = DispatchStatus.Sent;

        // Optional: provider can specify a reason when declining
        public string? DeclineReason { get; set; }
    }
}
