using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Data.Entities
{
    /// <summary>
    /// Each Client has exactly one Cart.
    /// They add ManualServices to it before checking out as a single Order.
    ///
    /// NOTE: There is NO cart for the digital/technical flow.
    /// Digital orders are placed directly on a provider's profile.
    /// </summary>
    public class Cart
    {
        public int Id { get; set; }

        public string ClientId { get; set; } = string.Empty;
        public virtual Client Client { get; set; } = null!;

        public virtual ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }

    
}
