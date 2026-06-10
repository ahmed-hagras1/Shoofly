using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Data.Entities
{
    /// <summary>
    /// One ManualService inside the Client's cart.
    /// </summary>
    public class CartItem
    {
        public int Id { get; set; }

        public int CartId { get; set; }
        public virtual Cart Cart { get; set; } = null!;

        public int ManualServiceId { get; set; }
        public virtual ManualService ManualService { get; set; } = null!;

        public DateTime AddedDate { get; set; } = DateTime.UtcNow;
    }
}
