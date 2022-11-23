using System.ComponentModel;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for <see cref="TabControl.SelectedPageChanging"/> event.
    /// </summary>
    public class SelectedTabPageChangingEventArgs : CancelEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectedTabPageChangingEventArgs"/> class.
        /// </summary>
        public SelectedTabPageChangingEventArgs(TabPage? oldValue, TabPage? newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        /// <summary>
        /// Old selected tab page.
        /// </summary>
        public TabPage? OldValue { get; }

        /// <summary>
        /// New selected tab page.
        /// </summary>
        public TabPage? NewValue { get; }
    }
}