using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for an event that occurs when the visibility of the on-screen keyboard changes.
    /// </summary>
    public class KeyboardVisibleChangedEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardVisibleChangedEventArgs"/> class.
        /// </summary>
        public KeyboardVisibleChangedEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardVisibleChangedEventArgs"/> class with the specified visibility and height.
        /// </summary>
        /// <param name="isVisible">The visibility of the on-screen keyboard.</param>
        /// <param name="height">The height of the on-screen keyboard in device-independent units (DIPs).</param>
        public KeyboardVisibleChangedEventArgs(bool isVisible, double height)
        {
            IsVisible = isVisible;
            Height = height;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the on-screen keyboard is visible.
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets the height of the on-screen keyboard in device-independent units (DIPs).
        /// </summary>
        public double Height { get; set; }
    }
}
