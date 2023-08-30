using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Container for the property values in the <see cref="PropertyGrid"/>.
    /// </summary>
    public interface IPropertyGridVariant
    {
        /// <summary>
        /// Gets or sets variasnt value as <see cref="object"/>.
        /// </summary>
        object? AsObject { get; set; }

        /// <summary>
        /// Gets or sets variasnt value as <see cref="double"/>.
        /// </summary>
        double AsDouble { get; set; }

        /// <summary>
        /// Gets or sets variasnt value as <see cref="bool"/>.
        /// </summary>
        bool AsBool { get; set; }

        /// <summary>
        /// Gets or sets variasnt value as <see cref="long"/>.
        /// </summary>
        long AsLong { get; set; }

        /// <summary>
        /// Gets or sets variasnt value as <see cref="DateTime"/>.
        /// </summary>
        DateTime AsDateTime { get; set; }

        /// <summary>
        /// Gets or sets variasnt value as <see cref="Color"/>.
        /// </summary>
        Color AsColor { get; set; }

        /// <summary>
        /// Gets or sets variasnt value as <see cref="string"/>.
        /// </summary>
        string AsString { get; set; }

        /// <summary>
        /// Clears variant.
        /// </summary>
        void Clear();

        /// <summary>
        /// Gets whether value is null.
        /// </summary>
        bool IsNull();

        /// <summary>
        /// Returns value type as <see cref="string"/>.
        /// </summary>
        string GetValueType();

        /// <summary>
        /// Returns value as string for any type of variant.
        /// </summary>
        string ToString();
    }
}
