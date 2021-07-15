
using System;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a list view control, which displays a collection of items that can be displayed using one of several different views.
    /// </summary>
    public class ListView : Control
    {
        /// <summary>
        /// Gets a collection containing all items in the control.
        /// </summary>
        public Collection<ListViewItem> Items { get; } = new Collection<ListViewItem> { ThrowOnNullItemAddition = true };

        private ListViewView view = ListViewView.List;

        /// <summary>
        /// Occurs when the <see cref="View"/> property value changes.
        /// </summary>
        public event EventHandler? ViewChanged;

        /// <summary>
        /// Gets or sets how items are displayed in the control.
        /// </summary>
        /// <value>One of the <see cref="ListViewView"/> values. The default is <see cref="ListViewView.List"/>.</value>
        public ListViewView View
        {
            get
            {
                CheckDisposed();
                return view;
            }

            set
            {
                CheckDisposed();

                if (view == value)
                    return;

                view = value;

                ViewChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}