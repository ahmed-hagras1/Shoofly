using Shoofly.Data.Entities.Identity;
using Shoofly.Shared.Enums;
using Shoofly.Shared.Enums;
using System;

namespace Shoofly.Data.Entities
{
    /// <summary>
    /// Records every money movement in the platform — client payments,
    /// provider payouts, refunds, salary payments, platform commissions, etc.
    ///
    /// Works for both the manual and digital flows.
    /// Only one of OrderId / DigitalOrderId will be set (or neither for
    /// salary payments and standalone withdrawals).
    /// </summary>
    public class Transaction
    {
        public int Id { get; set; }

        // ----------------------------------------------------
        // 1. Financial Details
        // ----------------------------------------------------
        public decimal Amount { get; set; }       // Positive = in, negative = out
        public TransactionType Type { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string? Notes { get; set; }

        // ----------------------------------------------------
        // 2. Whose ledger?
        // ----------------------------------------------------
        public string UserId { get; set; } = string.Empty;
        public virtual ApplicationUser User { get; set; } = null!;

        // ----------------------------------------------------
        // 3. Source (at most one will be set)
        // ----------------------------------------------------

        // Manual order payment / refund
        public int? OrderId { get; set; }
        public virtual Order? Order { get; set; }

        // Digital order payment / refund
        public int? DigitalOrderId { get; set; }
        public virtual DigitalOrder? DigitalOrder { get; set; }
    }
}