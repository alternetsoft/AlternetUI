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
    public interface IValueValidator : IDisposable
    {
        /// <summary>
        /// Object handle.
        /// </summary>
        IntPtr Handle { get; }
    }
}