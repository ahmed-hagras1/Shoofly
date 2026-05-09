using Shoofly.Data.Entities.Identity;
using Shoofly.Shared.Enums;
using Shoofly.Shared.Enums;
using System;

namespace Shoofly.Data.Entities
{
    public class Transaction
    {
        public int Id { get; set; }

        // ----------------------------------------------------
        // 1. Financial Details
        // ----------------------------------------------------

        // The amount of money moved. 
        // (Tip: Use positive numbers for adding money, negative for deducting)
        public decimal Amount { get; set; }

        public TransactionType Type { get; set; }

        // e.g., "Cash", "Visa", "Wallet" - You can reuse your PaymentMethod enum here!
        public PaymentMethod PaymentMethod { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        // Optional: The Coordinator can write "Refunded due to broken AC"
        public string? Notes { get; set; }

        // ----------------------------------------------------
        // 2. Navigation Properties
        // ----------------------------------------------------

        // 🟢 REQUIRED: Whose wallet/account is this transaction for?
        public string UserId { get; set; } = string.Empty;
        public virtual ApplicationUser User { get; set; } = null!;

        // 🟢 OPTIONAL: If this transaction was caused by a specific Order, link it!
        // (Nullable because a Salary Payment or Bank Withdrawal isn't linked to one specific order)
        public int? OrderId { get; set; }
        public virtual Order? Order { get; set; }
    }
}