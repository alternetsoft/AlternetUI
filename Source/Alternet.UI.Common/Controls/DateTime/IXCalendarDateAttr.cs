using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Defines the interface for calendar date attributes. You need to invalidate the control after changing
    /// attributes if you want to see the changes immediately.
    /// </summary>
    public interface IXCalendarDateAttr : IDisposableObject
    {
        /// <summary>
        /// Gets whether this object is immutable (properties are readonly).
        /// </summary>
        bool Immutable { get; }

        /// <summary>
        /// Gets or sets the text color assigned for the calendar date.
        /// </summary>
        Color? TextColor { get; set; }

        /// <summary>
        /// Gets a value indicating whether text color is assigned for the calendar date.
        /// </summary>
        bool HasTextColor { get; }
    }
}