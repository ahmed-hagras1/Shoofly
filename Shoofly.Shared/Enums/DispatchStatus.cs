using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Shared.Enums
{
    public enum DispatchStatus
    {
        Sent = 1,           // Notification sent, waiting for response
        Accepted = 2,       // Provider accepted
        Declined = 3,       // Provider declined
        Expired = 4         // Provider didn't respond within the timeout window
    }
}
