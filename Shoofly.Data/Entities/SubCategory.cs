using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Data.Entities
{
    /// <summary>
    /// A sub-category under a parent Category.
    ///
    /// Behavior depends on parent Category.Type:
    ///
    ///   Manual    → SubCategory has ManualServices
    ///               (e.g. "Plumbing" → "Fix Leaking Pipe", "Install Faucet" …)
    ///
    ///   Technical → SubCategory lists DigitalServiceProviders
    ///               (e.g. "Web Development" → profiles of web developers)
    /// </summary>
    public class SubCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? IconUrl { get; set; }

        // ----------------------------------------------------
        // Parent
        // ----------------------------------------------------
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;

        // ----------------------------------------------------
        // Navigation Properties
        // ----------------------------------------------------

        // Manual flow: bookable services under this sub-category
        public virtual ICollection<ManualService> ManualServices { get; set; } = new List<ManualService>();

        // Technical flow: freelancers who list themselves under this sub-category
        public virtual ICollection<DigitalServiceProvider> DigitalProviders { get; set; } = new List<DigitalServiceProvider>();

        // Technical flow: digital orders categorized here
        public virtual ICollection<DigitalOrder> DigitalOrders { get; set; } = new List<DigitalOrder>();
    }
}
