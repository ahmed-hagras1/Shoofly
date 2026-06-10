using Shoofly.Shared.Enums;
using System;

namespace Shoofly.Data.Entities
{
    /// <summary>
    /// One specific service task inside a master Order.
    ///
    /// Lifecycle:
    ///   Pending → Dispatching (coordinator triggers dispatch)
    ///           → Assigned (a provider/team accepted)
    ///           → InProgress (work started)
    ///           → Completed → Review created
    ///
    /// Assignment is EXCLUSIVE: either a solo ManualServiceProvider
    /// OR a ManualTeam — never both.
    /// </summary>
    public class OrderItem
    {
        public int Id { get; set; }

        // ----------------------------------------------------
        // 1. Parent order
        // ----------------------------------------------------
        public int OrderId { get; set; }
        public virtual Order Order { get; set; } = null!;

        // ----------------------------------------------------
        // 2. Service requested
        // ----------------------------------------------------
        public int ManualServiceId { get; set; }
        public virtual ManualService ManualService { get; set; } = null!;

        // ----------------------------------------------------
        // 3. Price snapshot (locked at checkout time)
        // ----------------------------------------------------
        public decimal UnitPrice { get; set; }
        public int? HoursSelected { get; set; }
        public decimal SubTotal { get; set; }

        // ----------------------------------------------------
        // 4. Assignment — solo provider OR team (not both)
        // ----------------------------------------------------
        public string? ProviderId { get; set; }
        public virtual ManualServiceProvider? Provider { get; set; }

        public int? TeamId { get; set; }
        public virtual ManualTeam? AssignedTeam { get; set; }

        // ----------------------------------------------------
        // 5. Status
        // ----------------------------------------------------
        public OrderItemStatus ItemStatus { get; set; } = OrderItemStatus.Pending;

        // ----------------------------------------------------
        // 6. Dispatch history — every provider the system tried
        // ----------------------------------------------------
        public virtual ICollection<DispatchAttempt> DispatchAttempts { get; set; } = new List<DispatchAttempt>();

        // ----------------------------------------------------
        // 7. Review — created only after ItemStatus == Completed
        // ----------------------------------------------------
        public virtual Review? Review { get; set; }
    }
}