using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Container for the property values in the <see cref="PropertyGrid"/>.
    /// </summary>
    public interface IPropertyGridVariant : IDisposable
    {
        /// <summary>
        /// Gets instance handle.
        /// </summary>
        IntPtr Handle { get; }

        /// <summary>
        /// Gets or sets variant value as <see cref="object"/>.
        /// </summary>
        /// <remarks>
        /// Use <see cref="GetCompatibleValue"/> if you want to assign variant
        /// to object property using <see cref="PropertyInfo.SetValue(object, object)"/>
        /// or similar methods.
        /// </remarks>
        object? AsObject { get; set; }

        /// <summary>
        /// Gets or sets variant value as <see cref="double"/>.
        /// </summary>
        double AsDouble { get; set; }

        /// <summary>
        /// Gets or sets variant value as <see cref="bool"/>.
        /// </summary>
        bool AsBool { get; set; }

        /// <summary>
        /// Gets or sets variant value as <see cref="long"/>.
        /// </summary>
        long AsLong { get; set; }

        /// <summary>
        /// Gets or sets variant value as <see cref="ulong"/>.
        /// </summary>
        ulong AsULong { get; set; }

        /// <summary>
        /// Gets or sets variant value as <see cref="DateTime"/>.
        /// </summary>
        DateTime AsDateTime { get; set; }

        /// <summary>
        /// Gets or sets variant value as <see cref="Color"/>.
        /// </summary>
        Color AsColor { get; set; }

        /// <summary>
        /// Gets or sets variant value as <see cref="string"/>.
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
        /// Gets value in the compatible format which can be assigned to the property
        /// using <see cref="PropertyInfo.SetValue(object, object)"/>.
        /// </summary>
        /// <param name="p">Property information.</param>
        public object? GetCompatibleValue(PropertyInfo p);

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
