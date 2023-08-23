using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Choices used in <see cref="PropertyGrid"/> items for enum and flags properties.
    /// </summary>
    public interface IPropertyGridChoices
    {
        /// <summary>
        /// Gets <see cref="IPropertyGridChoices"/> object handle.
        /// </summary>
        IntPtr Handle { get; }

        /// <summary>
        /// Adds new item.
        /// </summary>
        /// <param name="text">Item title.</param>
        /// <param name="value">Item value.</param>
        /// <param name="bitmap">Item image.</param>
        public void Add(string text, int value, ImageSet? bitmap = null);
    }
}
