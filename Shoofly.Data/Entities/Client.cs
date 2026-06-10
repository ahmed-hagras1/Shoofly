using Shoofly.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Data.Entities
{
    /// <summary>
    /// A customer who browses services and places orders (both manual and digital).
    /// Inherits all identity/auth fields from ApplicationUser.
    /// </summary>
    public class Client : ApplicationUser
    {
        public Client()
        {
            ManualOrders = new HashSet<Order>();
            DigitalOrders = new HashSet<DigitalOrder>();
        }

        // Optional saved address for faster manual-order checkout
        public string? DefaultArea { get; set; }
        public string? DefaultStreet { get; set; }
        public string? DefaultBuilding { get; set; }

        // ----------------------------------------------------
        // Navigation Properties
        // ----------------------------------------------------

        public virtual Cart? Cart { get; set; }
        public virtual ICollection<Order> ManualOrders { get; set; }
        public virtual ICollection<DigitalOrder> DigitalOrders { get; set; }
    }
}
