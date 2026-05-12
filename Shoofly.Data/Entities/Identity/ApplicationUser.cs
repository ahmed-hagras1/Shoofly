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
            // Initialize the GUID
            Id = Guid.NewGuid().ToString();

            // Initialize the collections
            UserRefreshTokens = new HashSet<UserRefreshToken>();
            ClientOrders = new HashSet<Order>();
        }

        public string FullName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        // You can also bring over Address and Country if Shoofly users need them!
        //public string? Address { get; set; }
        //public string? Country { get; set; }

        // ADD THIS: Store the ISO culture code directly on the user
        // Default to English (or Arabic, depending on your primary audience)
        public string PreferredLanguage { get; set; } = "en-US";

        public int CountryId { get; set; }
        public virtual Country Country { get; set; } = null!;

        // Navigation properties
        public virtual ICollection<UserRefreshToken> UserRefreshTokens { get; set; }
        public virtual ICollection<Order> ClientOrders { get; set; }
    }
}
