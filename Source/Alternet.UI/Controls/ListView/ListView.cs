using System;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a list view control, which displays a collection of items that can be displayed using one of several different views.
    /// </summary>
    public class ListView : Control
    {
        private ListViewView view = ListViewView.List;
        
        private ImageList? smallImageList = null;
        private ImageList? largeImageList = null;

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
        /// Occurs when the <see cref="SmallImageList"/> property value changes.
        /// </summary>
        public event EventHandler? SmallImageListChanged;

        /// <summary>
        /// Occurs when the <see cref="LargeImageList"/> property value changes.
        /// </summary>
        public event EventHandler? LargeImageListChanged;

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

        /// <summary>
        /// Gets or sets the <see cref="ImageList"/> to use when displaying items as small icons in the control.
        /// </summary>
        /// <value>An <see cref="ImageList"/> that contains the icons to use when the <see cref="View"/> property
        /// is set to any value other than <see cref="ListViewView.LargeIcon"/>. The default is <c>null</c>.</value>
        /// <remarks>
        /// <para>
        /// The <see cref="SmallImageList"/> property allows you to specify an <see cref="ImageList"/> object that contains icons to use when displaying
        /// items with small icons (when the <see cref="View"/> property is set to any value other than <see cref="ListViewView.LargeIcon"/>). The <see cref="ListView"/> control
        /// can accept any graphics format that the <see cref="ImageList"/> control supports when displaying icons. The <see cref="ListView"/> control
        /// is not limited to .ico files. Once an <see cref="ImageList"/> is assigned to the <see cref="SmallImageList"/> property, you can set the
        /// <see cref="ListViewItem.ImageIndex"/> property of each <see cref="ListViewItem"/> in the <see cref="ListView"/> control to the index position of the appropriate
        /// image in the <see cref="ImageList"/>. The size of the icons for the <see cref="SmallImageList"/> is specified by the <see cref="ImageList.PixelImageSize"/> property.
        /// </para>
        /// <para>
        /// Because only one index can be specified for the <see cref="ListViewItem.ImageIndex"/> property, the <see cref="ImageList"/> objects
        /// specified in the <see cref="LargeImageList"/> and <see cref="SmallImageList"/> properties should have the same index positions for the
        /// images to display. For example, if the <see cref="ListViewItem.ImageIndex"/> property of a <see cref="ListViewItem"/> is set to 0, the images to use
        /// for both small and large icons should be at the same index position in the <see cref="ImageList"/> objects specified in
        /// the <see cref="LargeImageList"/> and <see cref="SmallImageList"/> properties.
        /// </para>
        /// <para>
        /// To set the <see cref="ImageList"/> to use when displaying items with large icons (when the <see cref="View"/> property is set to <see cref="ListViewView.LargeIcon"/>),
        /// use the <see cref="LargeImageList"/> property.
        /// </para>
        /// </remarks>
        public ImageList? SmallImageList
        {
            get
            {
                CheckDisposed();
                return smallImageList;
            }

            set
            {
                CheckDisposed();

                if (smallImageList == value)
                    return;

                smallImageList = value;

                SmallImageListChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ImageList"/> to use when displaying items as large icons in the control.
        /// </summary>
        /// <value>
        /// An <see cref="ImageList"/> that contains the icons to use when the <see cref="View"/> property
        /// is set to <see cref="ListViewView.LargeIcon"/>. The default is <c>null</c>.
        /// </value>
        /// <remarks>
        /// <para>
        /// The <see cref="LargeImageList"/> property allows you to specify an <see cref="ImageList"/> object that contains icons to use when displaying
        /// items with large icons (when the <see cref="View"/> property is set to <see cref="ListViewView.LargeIcon"/>). The <see cref="ListView"/> control
        /// can accept any graphics format that the <see cref="ImageList"/> control supports when displaying icons. The <see cref="ListView"/> control
        /// is not limited to .ico files. Once an <see cref="ImageList"/> is assigned to the <see cref="LargeImageList"/> property, you can set the
        /// <see cref="ListViewItem.ImageIndex"/> property of each <see cref="ListViewItem"/> in the <see cref="ListView"/> control to the index position of the appropriate
        /// image in the <see cref="ImageList"/>. The size of the icons for the <see cref="LargeImageList"/> is specified by the <see cref="ImageList.PixelImageSize"/> property.
        /// </para>
        /// <para>
        /// Because only one index can be specified for the <see cref="ListViewItem.ImageIndex"/> property, the <see cref="ImageList"/> objects
        /// specified in the <see cref="LargeImageList"/> and <see cref="SmallImageList"/> properties should have the same index positions for the
        /// images to display. For example, if the <see cref="ListViewItem.ImageIndex"/> property of a <see cref="ListViewItem"/> is set to 0, the images to use
        /// for both small and large icons should be at the same index position in the <see cref="ImageList"/> objects specified in
        /// the <see cref="LargeImageList"/> and <see cref="SmallImageList"/> properties.
        /// </para>
        /// <para>
        /// To set the <see cref="ImageList"/> to use when displaying items with small icons (when the <see cref="View"/>
        /// property is set to any value other than <see cref="ListViewView.LargeIcon"/>),
        /// use the <see cref="SmallImageList"/> property.
        /// </para>
        /// </remarks>
        public ImageList? LargeImageList
        {
            get
            {
                CheckDisposed();
                return largeImageList;
            }

            set
            {
                CheckDisposed();

                if (largeImageList == value)
                    return;

                smallImageList = value;

                LargeImageListChanged?.Invoke(this, EventArgs.Empty);
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