using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Data.Entities
{
    public class Cart
    {
        public int Id { get; set; }

        // Linked to the User
        public string ClientId { get; set; }
        // public ApplicationUser Client { get; set; } // If using Identity

        // Navigation property: A cart can have multiple services inside it
        public virtual ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
