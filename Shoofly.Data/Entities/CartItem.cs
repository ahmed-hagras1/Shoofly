using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Data.Entities
{
    public class CartItem
    {
        public int Id { get; set; }

        // Linked to the Cart
        public int CartId { get; set; }
        public virtual Cart Cart { get; set; } = null!;

        // Linked to the Service they selected
        public int ServiceId { get; set; }
        public virtual Service Service { get; set; } = null!;

        // You can optionally add a timestamp to clear old items automatically
        public DateTime AddedDate { get; set; } = DateTime.UtcNow;
    }
}
