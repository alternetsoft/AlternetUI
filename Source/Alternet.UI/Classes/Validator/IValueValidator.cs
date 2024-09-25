using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Mediates between controls and application data.
    /// </summary>
    /// <remarks>
    /// A validator has three major roles. It transfers data from a variable or own storage
    /// to (and from) a control. It validates data in a control, and shows an appropriate
    /// error message. It filters events (such as keystrokes), thereby changing the behaviour
    /// of the associated control. Validators can be plugged into controls dynamically.
    /// </remarks>
    internal interface IValueValidator : IDisposable
    {
        /// <summary>
        /// Object handle.
        /// </summary>
        IntPtr Handle { get; }

        /// <summary>
        /// Gets or sets the identifying name of the object.
        /// </summary>
        /// <value>The name of the object. The default is <c>null</c>.</value>
        string? Name { get; set; }

        /// <summary>
        /// Gets or sets data related to the object.
        /// </summary>
        /// <value>An <see cref="object"/> that contains data.
        /// The default is <c>null</c>.</value>
        /// <remarks>
        /// Any type derived from the <see cref="object"/> class can be assigned
        /// to this property.
        /// </remarks>
        object? Tag { get; set; }
    }
}