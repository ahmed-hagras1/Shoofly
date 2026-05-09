using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Shared.Enums
{
    public enum TransactionType
    {
        ClientPayment = 1,        // Client paid for an order
        ProviderEarnings = 2,     // Money added to worker's balance for finishing a job
        PlatformCommission = 3,   // The cut the platform takes
        Withdrawal = 4,           // Worker withdrew money to their bank/Vodafone Cash
        Refund = 5,               // Money returned to the client
        SalaryPayment = 6         // Monthly fixed salary paid to an employee
    }
}
