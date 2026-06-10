using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Shared.Enums
{
    /// <summary>Status of a DigitalOrder (freelance/technical flow).</summary>
    public enum DigitalOrderStatus
    {
        Pending = 1,       // Placed by client, waiting for provider to accept
        Accepted = 2,      // Provider accepted the project
        InProgress = 3,    // Work is underway
        UnderReview = 4,   // Provider submitted delivery; client is reviewing
        Completed = 5,     // Client approved — triggers DigitalReview creation
        Canceled = 6,
        Disputed = 7       // Client/provider raised a dispute
    }
}
