using Shoofly.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Data.Entities
{
    /// <summary>
    /// Top-level category. The Type field is the key routing decision:
    ///
    ///   CategoryType.Manual    → SubCategories contain ManualServices
    ///                            Client flow: browse services → add to cart → checkout (Talabat style)
    ///
    ///   CategoryType.Technical → SubCategories list DigitalServiceProviders
    ///                            Client flow: browse provider profiles → place DigitalOrder directly (Khamsat style)
    /// </summary>
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? IconUrl { get; set; }

        // This single field drives which UI flow the client sees
        public CategoryType Type { get; set; }

        // ----------------------------------------------------
        // Navigation Properties
        // ----------------------------------------------------
        public virtual ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
    }
}
