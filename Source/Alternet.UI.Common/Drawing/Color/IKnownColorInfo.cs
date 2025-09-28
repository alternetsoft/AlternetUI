using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// Represents information about a known color, including its category, label, value, and visibility.
    /// </summary>
    /// <remarks>This interface provides properties and methods to access and manage
    /// metadata about a known
    /// color. It includes details such as the color's category,
    /// its English and localized labels, the color value, and
    /// whether the color is visible to the end user in color editors.</remarks>
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

        /// <summary>
        /// Gets whether <see cref="Category"/> is in the list of categories.
        /// </summary>
        /// <param name="cats">List of categories.</param>
        /// <returns></returns>
        bool CategoryIs(params KnownColorCategory[] cats);
    }
}
