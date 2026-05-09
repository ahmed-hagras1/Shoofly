using Shoofly.Data.Entities.Identity;
using Shoofly.Shared.Enums;
using System;
using System.Collections.Generic;

namespace Shoofly.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }

        // ----------------------------------------------------
        // 1. Checkout Details (Where and When)
        // ----------------------------------------------------
        public string? Area { get; set; }
        public string? Street { get; set; }
        public string? Building { get; set; }
        public string? Floor { get; set; }
        public string? ApartmentNumber { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ServeDate { get; set; }
        public string? Notes { get; set; }

        // ----------------------------------------------------
        // 2. Financials & Status
        // ----------------------------------------------------
        public PaymentMethod PaymentMethod { get; set; }

        // This is the GRAND TOTAL (The sum of all OrderItem SubTotals)
        public decimal TotalCost { get; set; }

        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;

        // ----------------------------------------------------
        // 3. Navigation Properties (The People Involved)
        // ----------------------------------------------------

        // Who requested this order?
        public string ClientId { get; set; } = string.Empty;
        public virtual ApplicationUser Client { get; set; } = null!;

        // Who is managing this order from the company side?
        public string? CoordinatorId { get; set; }
        public virtual Coordinator? Coordinator { get; set; }

        // ----------------------------------------------------
        // 4. The Contents of the Order
        // ----------------------------------------------------

        // The list of specific tasks/services requested in this checkout
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}