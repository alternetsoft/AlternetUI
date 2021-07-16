using System;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a list view control, which displays a collection of items that can be displayed using one of several different views.
    /// </summary>
    public class ListView : Control
    {
        private ListViewView view = ListViewView.List;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListView"/> class.
        /// </summary>
        public ListView()
        {
            Columns.ItemInserted += Columns_ItemInserted;
            Columns.ItemRemoved += Columns_ItemRemoved;
        }

        /// <summary>
        /// Occurs when the <see cref="View"/> property value changes.
        /// </summary>
        public event EventHandler? ViewChanged;

        /// <summary>
        /// Gets a collection containing all items in the control.
        /// </summary>
        public Collection<ListViewItem> Items { get; } = new Collection<ListViewItem> { ThrowOnNullItemAddition = true };

        /// <summary>
        /// Gets the collection of all columns that appear in the control in the <see cref="ListViewView.Details"/> <see cref="View"/>..
        /// </summary>
        /// <value>A collection that represents the columns that appear when the <see cref="View"/> property is set to <see cref="ListViewView.Details"/>.</value>
        public Collection<ListViewColumn> Columns { get; } = new Collection<ListViewColumn> { ThrowOnNullItemAddition = true };

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

        private void Columns_ItemInserted(object? sender, CollectionChangeEventArgs<ListViewColumn> e)
        {
            e.Item.ListView = this;
            e.Item.Index = e.Index;
        }

        private void Columns_ItemRemoved(object? sender, CollectionChangeEventArgs<ListViewColumn> e)
        {
            e.Item.ListView = null;
            e.Item.Index = null;
        }
    }
}