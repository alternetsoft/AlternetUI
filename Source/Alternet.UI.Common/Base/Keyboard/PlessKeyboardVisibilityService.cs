using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Implements dummy <see cref="IKeyboardVisibilityService"/> for testing or other purposes.
    /// </summary>
    public partial class PlessKeyboardVisibilityService : DisposableObject, IKeyboardVisibilityService, IDisposable
    {
        /// <inheritdoc/>
        public event EventHandler<KeyboardVisibleChangedEventArgs>? KeyboardVisibleChanged;

        /// <inheritdoc/>
        public bool IsVisible { get; private set; }

        /// <inheritdoc/>
        public double Height { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlessKeyboardVisibilityService"/> class.
        /// </summary>
        public PlessKeyboardVisibilityService()
        {
        }

        /// <summary>
        /// Raises the event indicating that the on-screen keyboard has been shown and updates the keyboard height.
        /// </summary>
        /// <param name="height">The height, in device-independent units (DIPs),
        /// of the visible on-screen keyboard. Must be greater than or
        /// equal to zero.</param>
        public void RaiseKeyboardShown(double height)
        {
            IsVisible = true;
            Height = height;
            KeyboardVisibleChanged?.Invoke(this, new KeyboardVisibleChangedEventArgs(true, height));
        }

        /// <summary>
        /// Notifies subscribers that the software keyboard has been hidden and updates the keyboard visibility state
        /// accordingly.
        /// </summary>
        /// <remarks>This method sets the keyboard visibility to hidden and resets the keyboard height to
        /// zero. It then raises the KeyboardVisibleChanged event to inform listeners of the change. Call this method
        /// when the keyboard is dismissed to ensure that the application state remains consistent.</remarks>
        public void RaiseKeyboardHidden()
        {
            IsVisible = false;
            Height = 0;
            KeyboardVisibleChanged?.Invoke(this, new KeyboardVisibleChangedEventArgs(false, 0));
        }
    }
}