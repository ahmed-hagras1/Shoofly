using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Shared.Enums
{
    public enum OrderStatus
    {
        Pending = 1,        // Just placed, waiting for Coordinator review
        Confirmed = 2,      // Coordinator confirmed it
        InProgress = 3,     // At least one OrderItem is being worked on
        Completed = 4,      // All OrderItems completed
        Cancelled = 5
    }
}
