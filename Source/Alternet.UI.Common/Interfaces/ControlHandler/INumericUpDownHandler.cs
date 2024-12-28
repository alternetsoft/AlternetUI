using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to work with numeric up down control.
    /// </summary>
    public interface INumericUpDownHandler
    {
        /// <inheritdoc cref="NumericUpDown.HasBorder"/>
        bool HasBorder { get; set; }
    }
}
