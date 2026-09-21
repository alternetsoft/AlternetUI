using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the type of the window settings change event.
    /// </summary>
    public enum WindowSettingsChangedEventType
    {
        /// <summary>
        /// Indicates that no specific window settings change event has occurred.
        /// </summary>
        None,
    }

    /// <summary>
    /// Provides data for the window settings changed event.
    /// </summary>
    public class WindowSettingsChangedEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WindowSettingsChangedEventArgs"/> class with the specified event type.
        /// </summary>
        /// <param name="eventType">The type of the window settings change event.</param>
        public WindowSettingsChangedEventArgs(WindowSettingsChangedEventType eventType)
        {
            EventType = eventType;
        }

        /// <summary>
        /// Gets the type of the window settings change event.
        /// </summary>
        public WindowSettingsChangedEventType EventType { get; set; }
    }
}
