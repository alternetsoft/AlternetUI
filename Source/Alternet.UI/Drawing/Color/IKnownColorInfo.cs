using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// Contains information related to <see cref="KnownColor"/>.
    /// </summary>
    public interface IKnownColorInfo
    {
        /// <summary>
        /// Gets <see cref="KnownColor"/> for which item is created.
        /// </summary>
        KnownColor KnownColor { get; }

        /// <summary>
        /// Gets or sets color category.
        /// </summary>
        KnownColorCategory Category { get; set; }

        /// <summary>
        /// Gets color label in English.
        /// </summary>
        string Label { get; }

        /// <summary>
        /// Gets or sets localized version of the color label.
        /// </summary>
        string LabelLocalized { get; set; }

        /// <summary>
        /// Gets color value.
        /// </summary>
        Color Value { get; }

        /// <summary>
        /// Gets or sets whether color is visible for the end user.
        /// </summary>
        bool Visible { get; set; }
    }
}
