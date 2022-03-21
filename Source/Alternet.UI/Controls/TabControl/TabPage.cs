using System;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a single tab page in a <see cref="TabControl"/>.
    /// </summary>
    /// <remarks>
    /// <see cref="TabPage"/> controls represent the tabbed pages in a <see cref="TabControl"/> control.
    /// The order of tab pages in the <see cref="TabControl.Pages"/> collection reflects the order of tabs in the <see cref="TabControl"/> control.
    /// To change the order of tabs in the control, you must change their positions in the collection by removing them and inserting them at new indexes.
    /// The tabs in a <see cref="TabControl"/> are part of the <see cref="TabControl"/> but not parts of the individual <see cref="TabPage"/> controls.
    /// </remarks>
    public class TabPage : Control
    {
        private string title = "";

        /// <summary>
        /// Occurs when the value of the <see cref="Title"/> property changes.
        /// </summary>
        public event EventHandler? TitleChanged;

        /// <summary>
        /// Gets or sets the text to display on the tab.
        /// </summary>
        /// <value>The text to display on the tab.</value>
        public string Title
        {
            get
            {
                CheckDisposed();
                return title;
            }

            set
            {
                CheckDisposed();
                if (title == value)
                    return;

                title = value;
                RaiseTitleChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when the value of the <see cref="Title"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnTitleChanged(EventArgs e)
        {
        }

        /// <inheritdoc/>
        public override void BeginInit()
        {
#if NETCOREAPP
            // Workaround: on Linux, only the first tab is visible if the tab if PerformLayout is called from EndInit().
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
                return;
#endif
            base.BeginInit();
        }

        /// <inheritdoc/>
        public override void EndInit()
        {
#if NETCOREAPP
            // Workaround: on Linux, only the first tab is visible if the tab if PerformLayout is called from EndInit().
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
                return;
#endif
            base.EndInit();
        }

        /// <summary>
        /// Raises the <see cref="TitleChanged"/> event and calls <see cref="OnTitleChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        private void RaiseTitleChanged(EventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnTitleChanged(e);
            TitleChanged?.Invoke(this, e);
        }
    }
}