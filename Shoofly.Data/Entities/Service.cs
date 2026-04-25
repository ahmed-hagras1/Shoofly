using Shoofly.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Data.Entities
{
    public class Service
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal? ServiceAmountStart { get; set; }
        public decimal? HourlyRateStart { get; set; }
        public bool RequiresPhysicalAttendance { get; set; }
        public PricingType PricingType { get; set; }
        public string? CoverPhoto { get; set; }

        // Foreign Key and Navigation property back to SubCategory
        public int SubCategoryId { get; set; }
        public virtual SubCategory SubCategory { get; set; } = null!;

        // Navigation property: One Service can be requested in many Orders
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
