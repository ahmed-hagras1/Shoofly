using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Shared.Enums
{
    public enum TransactionType
    {
        ClientPayment = 1,           // Client paid for a manual or digital order
        ProviderEarnings = 2,        // Payout to a ManualServiceProvider after job completion
        DigitalProviderEarnings = 3, // Payout to a DigitalServiceProvider after order completion
        PlatformCommission = 4,      // The platform's cut on any transaction
        Withdrawal = 5,              // Provider withdrew earnings to bank / Vodafone Cash
        Refund = 6,                  // Money returned to the client
        SalaryPayment = 7            // Fixed monthly salary paid to Coordinator / ManualServiceProvider
    }
}
