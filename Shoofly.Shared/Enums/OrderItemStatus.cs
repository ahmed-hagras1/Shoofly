using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Shared.Enums
{
    public enum OrderItemStatus
    {
        Pending = 1,        // Created, no provider assigned yet
        Dispatching = 2,    // System is actively trying to find a provider
        Assigned = 3,       // Provider/team accepted
        InProgress = 4,     // Provider has started the work
        Completed = 5,
        Cancelled = 6
    }
}
