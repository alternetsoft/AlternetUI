using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI.Localization
{
    /// <summary>
    /// Defines localizations for property names.
    /// </summary>
    public class PropNameStrings
    {
        /// <summary>
        /// Current localizations for property names.
        /// </summary>
        public static PropNameStrings Default { get; set; } = new();

        /// <summary>
        /// Gets or sets localized property name.
        /// </summary>
        public string Left { get; set; } = "Left";

        /// <inheritdoc cref="Left"/>
        public string Top { get; set; } = "Top";

        /// <inheritdoc cref="Left"/>
        public string Right { get; set; } = "Right";

        /// <inheritdoc cref="Left"/>
        public string Bottom { get; set; } = "Bottom";

        /// <inheritdoc cref="Left"/>
        public string Width { get; set; } = "Width";

        /// <inheritdoc cref="Left"/>
        public string Height { get; set; } = "Height";

        /// <inheritdoc cref="Left"/>
        public string Center { get; set; } = "Center";

        /// <inheritdoc cref="Left"/>
        public string X { get; set; } = "X";

        /// <inheritdoc cref="Left"/>
        public string Y { get; set; } = "Y";
    }
}
