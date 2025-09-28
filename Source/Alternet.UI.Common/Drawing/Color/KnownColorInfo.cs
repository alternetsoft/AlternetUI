using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// Represents information about a known color, including its category,
    /// label, localized label, and value.
    /// </summary>
    /// <remarks>This class provides metadata and utility methods for working with known colors,
    /// such as their
    /// category and visibility. It is designed to encapsulate details
    /// about a specific <see cref="KnownColor"/> and
    /// expose them through properties and methods.</remarks>
    public class KnownColorInfo : IKnownColorInfo
    {
        private readonly KnownColor knownColor;
        private readonly string label;
        private readonly Color value;
        private KnownColorCategory category;
        private string labelLocalized;

        /// <summary>
        /// Initializes a new instance of the <see cref="KnownColorInfo"/>
        /// class with the specified known color.
        /// </summary>
        /// <param name="knownColor">The <see cref="KnownColor"/>
        /// value representing the color to initialize.</param>
        public KnownColorInfo(KnownColor knownColor)
        {
            this.knownColor = knownColor;
            this.category = KnownColorCategory.Other;
            label = knownColor.ToString();
            labelLocalized = label;
            value = Color.FromKnownColor(knownColor);
        }

        /// <inheritdoc/>
        public virtual bool Visible { get; set; } = true;

        /// <inheritdoc/>
        public virtual KnownColor KnownColor { get => knownColor; }

        /// <inheritdoc/>
        public virtual KnownColorCategory Category { get => category; set => category = value; }

        /// <inheritdoc/>
        public virtual string Label { get => label; }

        /// <inheritdoc/>
        public virtual string LabelLocalized { get => labelLocalized; set => labelLocalized = value; }

        /// <inheritdoc/>
        public virtual Color Value { get => value; }

        /// <inheritdoc/>
        public virtual bool CategoryIs(params KnownColorCategory[] cats)
        {
            foreach(var item in cats)
            {
                if (item == category)
                    return true;
            }

            return false;
        }
    }
}
