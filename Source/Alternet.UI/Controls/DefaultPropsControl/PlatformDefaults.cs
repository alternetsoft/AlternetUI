using System;

namespace Alternet.UI
{
    /// <summary>
    /// Contains platform specific settings.
    /// </summary>
    public class PlatformDefaults
    {
        static PlatformDefaults()
        {
        }

        public bool AdjustTextBoxesHeight { get; set; } = false;

        /// <summary>
        /// Returns default property values for all controls in the library.
        /// </summary>
        public AllControlDefaults Controls { get; } = new();
    }
}