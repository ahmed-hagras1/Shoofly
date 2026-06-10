using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Data.Entities.Identity
{
    public class ApplicationUser : IdentityUser<string>
    {
        public ApplicationUser()
        {
            Id = Guid.NewGuid().ToString();
            UserRefreshTokens = new HashSet<UserRefreshToken>();
            Notifications = new HashSet<Notification>();
            Transactions = new HashSet<Transaction>();
        }

        public string FullName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public string PreferredLanguage { get; set; } = "en-US";

        public int CountryId { get; set; }
        public virtual Country Country { get; set; } = null!;

        // Navigation properties shared across ALL user types
        public virtual ICollection<UserRefreshToken> UserRefreshTokens { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
