using Shoofly.Data.Entities.Identity;
using Shoofly.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }

        // Address Details (Nullable because technical crafts might not need them)
        public string? Area { get; set; }
        public string? Street { get; set; }
        public string? Building { get; set; }
        public string? Floor { get; set; }
        public string? ApartmentNumber { get; set; }

        // Dates
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ServeDate { get; set; }

        // Financials & Settings
        public PaymentMethod PaymentMethod { get; set; }
        public int? HoursSelected { get; set; }

        // Derived Attribute: It is best to store this in the DB to lock in the price
        // TotalCost = HoursSelected * Service.HourlyRateStart (or fixed ServiceAmountStart)
        public decimal TotalCost { get; set; }

        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public string? Notes { get; set; }

        // Foreign Key and Navigation property to link the Demand/Service
        public int ServiceId { get; set; }
        public virtual Service Service { get; set; } = null!;

        // Add these to link the users!
        public string ClientId { get; set; } = string.Empty;
        public virtual ApplicationUser Client { get; set; } = null!; // Uncomment when Identity is ready
    }
}
