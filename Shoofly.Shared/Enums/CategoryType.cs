using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Shared.Enums
{
    /// <summary>
    /// Drives which UI flow the client sees after selecting a category.
    /// </summary>
    public enum CategoryType
    {
        /// <summary>
        /// Physical on-site services (plumbing, cleaning, electrical…).
        /// Sub-categories contain ManualServices. Client uses cart + checkout.
        /// </summary>
        Manual = 1,

        /// <summary>
        /// Remote digital/technical services (web dev, mobile, design…).
        /// Sub-categories list DigitalServiceProviders. Client places DigitalOrder directly.
        /// </summary>
        Technical = 2
    }
}
