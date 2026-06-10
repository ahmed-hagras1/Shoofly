using Shoofly.Data.Entities.Identity;
using Shoofly.Shared.Enums;
using System;
using System.Collections.Generic;

namespace Shoofly.Data.Entities
{
    /// <summary>
    /// A master order placed by a Client for one or more ManualServices.
    /// Think: one Talabat checkout containing multiple items.
    ///
    /// After creation (Status = Pending), the Coordinator reviews it
    /// and triggers dispatch for each OrderItem individually.
    /// </summary>
    public class Order
    {
        public int Id { get; set; }

        // ----------------------------------------------------
        // 1. Service address (captured at checkout — may differ from default)
        // ----------------------------------------------------
        public string? Area { get; set; }
        public string? Street { get; set; }
        public string? Building { get; set; }
        public string? Floor { get; set; }
        public string? ApartmentNumber { get; set; }

        // ----------------------------------------------------
        // 2. Timing & Notes
        // ----------------------------------------------------
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ServeDate { get; set; }
        public string? Notes { get; set; }

        // ----------------------------------------------------
        // 3. Financials & Status
        // ----------------------------------------------------
        public PaymentMethod PaymentMethod { get; set; }
        public decimal TotalCost { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;

        // ----------------------------------------------------
        // 4. People Involved
        // ----------------------------------------------------
        public string ClientId { get; set; } = string.Empty;
        public virtual Client Client { get; set; } = null!;

        public string? CoordinatorId { get; set; }
        public virtual Coordinator? Coordinator { get; set; }

        // ----------------------------------------------------
        // 5. Contents & Financials
        // ----------------------------------------------------
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}