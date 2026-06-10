using Shoofly.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Data.Entities
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // 🟢 NEW PROPERTY: Stores the calling code (e.g., "+20" for Egypt, "+966" for KSA)
        public string DialCode { get; set; } = string.Empty;

        // Navigation property: One Category has many SubCategories
        public virtual ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
    }
}
