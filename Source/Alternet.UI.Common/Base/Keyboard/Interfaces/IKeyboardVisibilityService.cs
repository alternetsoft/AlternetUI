using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties to monitor the visibility of the on-screen keyboard.
    /// </summary>
    public interface IKeyboardVisibilityService : IDisposable
    {
        /// <summary>
        /// Occurs when the visibility of the on-screen keyboard changes.
        /// </summary>
        event EventHandler<KeyboardVisibleChangedEventArgs>? KeyboardVisibleChanged;

        /// <summary>
        /// Gets a value indicating whether the on-screen keyboard is visible.
        /// </summary>
        bool IsVisible { get; }

        /// <summary>
        /// Gets the height of the on-screen keyboard in device-independent units (DIPs).
        /// </summary>
        double Height { get; }
    }
}
