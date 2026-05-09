using Shoofly.Data.Entities.Identity;
using System.Collections.Generic;

namespace Shoofly.Data.Entities
{
    public class Coordinator : ApplicationUser
    {
        // 1. HR & Admin Information
        public bool IsActive { get; set; } = true;
        public decimal Salary { get; set; } = 7000m; // Example fixed salary
        public string? DocumentFileUrl { get; set; } // Contract, ID, or CV

        // 2. Performance Tracking (Optional but highly recommended)
        // You can increment this every time they successfully close an order
        public int DispatchedOrdersCount { get; set; } = 0;

        // ----------------------------------------------------
        // Navigation Properties
        // ----------------------------------------------------

        // The Coordinator is responsible for overseeing the Master Orders.
        // They look at these orders and assign workers to the specific OrderItems inside.
        public virtual ICollection<Order> ManagedOrders { get; set; } = new List<Order>();
    }
}