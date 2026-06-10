using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Shared.Enums
{
    public enum PricingType
    {
        FixedAmount = 1,  // Total fixed price (e.g. logo design, AC installation)
        PerHour = 2,      // Billed by the hour (e.g. cleaning, hourly tech support)
        Custom = 3        // Price quoted after initial assessment
    }
}
