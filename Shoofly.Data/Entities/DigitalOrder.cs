using Shoofly.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Data.Entities
{
    /// <summary>
    /// A direct order between a Client and a DigitalServiceProvider (or DigitalTeam)
    /// for a technical/digital service.
    ///
    /// This is the Khamsat / Mostakel flow:
    ///   Client browses providers under a Technical SubCategory
    ///   → selects a provider → describes the project → places DigitalOrder
    ///   → provider accepts → works → delivers → client reviews
    ///
    /// There is NO cart for digital orders. Each DigitalOrder is a standalone
    /// negotiated agreement between the client and a specific provider or team.
    /// </summary>
    public class DigitalOrder
    {
        public int Id { get; set; }

        // ----------------------------------------------------
        // 1. What is being requested?
        // ----------------------------------------------------
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Which sub-category this order falls under (e.g. "Web Development")
        public int SubCategoryId { get; set; }
        public virtual SubCategory SubCategory { get; set; } = null!;

        // ----------------------------------------------------
        // 2. Financials & Delivery
        // ----------------------------------------------------
        public decimal AgreedPrice { get; set; }
        public int DeliveryDays { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        // ----------------------------------------------------
        // 3. Status & Timing
        // ----------------------------------------------------
        public DigitalOrderStatus Status { get; set; } = DigitalOrderStatus.Pending;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedDate { get; set; }

        // ----------------------------------------------------
        // 4. People — client + (solo provider OR team, not both)
        // ----------------------------------------------------
        public string ClientId { get; set; } = string.Empty;
        public virtual Client Client { get; set; } = null!;

        public string? ProviderId { get; set; }
        public virtual DigitalServiceProvider? Provider { get; set; }

        public int? TeamId { get; set; }
        public virtual DigitalTeam? Team { get; set; }

        // ----------------------------------------------------
        // 5. Files
        // ----------------------------------------------------
        public string? AttachmentUrl { get; set; }   // Client uploads brief/assets
        public string? DeliverableUrl { get; set; }  // Provider uploads final delivery

        // ----------------------------------------------------
        // 6. Feedback & Transactions
        // ----------------------------------------------------
        public virtual DigitalReview? Review { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
