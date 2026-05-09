using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Data.Entities
{
    public class SubCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Foreign Key and Navigation property back to Category
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;

        // Navigation property: One SubCategory has many Services
        public virtual ICollection<Service> Services { get; set; } = new List<Service>();
    }
}
