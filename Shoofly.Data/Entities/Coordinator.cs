using Shoofly.Data.Entities.Identity;
using System.Collections.Generic;

namespace Shoofly.Data.Entities
{
    /// <summary>
    /// A company employee who reviews incoming manual orders
    /// and assigns ManualServiceProviders / ManualTeams to OrderItems.
    /// </summary>
    public class Coordinator : ApplicationUser
    {
        public Coordinator()
        {
            ManagedOrders = new HashSet<Order>();
        }

        public decimal Salary { get; set; } = 7000m;
        public string? DocumentFileUrl { get; set; }

        // Incremented each time the coordinator successfully closes an order
        public int DispatchedOrdersCount { get; set; } = 0;

        // ----------------------------------------------------
        // Navigation Properties
        // ----------------------------------------------------
        public virtual ICollection<Order> ManagedOrders { get; set; }
    }
}