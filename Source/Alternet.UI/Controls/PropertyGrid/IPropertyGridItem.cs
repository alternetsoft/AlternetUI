using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Item of the <see cref="PropertyGrid"/>.
    /// </summary>
    public interface IPropertyGridItem
    {
        /// <summary>
        /// Item handle.
        /// </summary>
        IntPtr Handle { get; }

        /// <summary>
        /// Item name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Item label.
        /// </summary>
        string Label { get; }

        /// <summary>
        /// Item default value.
        /// </summary>
        object? DefaultValue { get; }

        /// <summary>
        /// Item is category.
        /// </summary>
        bool IsCategory { get; }
    }
}
