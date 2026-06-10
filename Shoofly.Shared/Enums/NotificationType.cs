using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Shared.Enums
{
    /// <summary>
    /// Tells the mobile app how to handle a notification tap action.
    /// </summary>
    public enum NotificationType
    {
        /// <summary>Sent to a ManualServiceProvider: "New job available — accept?"</summary>
        DispatchRequest = 1,

        /// <summary>Sent to Client/Coordinator about a manual Order or OrderItem status change.</summary>
        ManualOrderUpdate = 2,

        /// <summary>Sent to Client or DigitalServiceProvider about a DigitalOrder status change.</summary>
        DigitalOrderUpdate = 3,

        /// <summary>Payment received, payout processed, refund issued.</summary>
        Payment = 4,

        /// <summary>General system/admin message.</summary>
        General = 5
    }
}
