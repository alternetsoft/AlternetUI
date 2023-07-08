using System.Collections.Generic;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a toolbar control.
    /// </summary>
    public class Toolbar : Control
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Toolbar"/> class.
        /// </summary>
        public Toolbar()
        {
            Items.ItemInserted += Items_ItemInserted;
            Items.ItemRemoved += Items_ItemRemoved;
        }

        public new ToolbarHandler Handler
        {
            get
            {
                CheckDisposed();
                return (ToolbarHandler)base.Handler;
            }
        }

        /// <summary>
        /// Gets or sets a value which specifies display modes for toolbar item image and text.
        /// </summary>
        public ToolbarItemImageToTextDisplayMode ImageToTextDisplayMode
        {
            get => Handler.ImageToTextDisplayMode;
            set => Handler.ImageToTextDisplayMode = value;
        }

        /// <summary>
        /// Gets or sets a boolean value indicating whether this toolbar item text is visible.
        /// </summary>
        public bool ItemTextVisible { get => Handler.ItemTextVisible; set => Handler.ItemTextVisible = value; }

        /// <summary>
        /// Gets or sets a boolean value indicating whether this toolbar item images are visible.
        /// </summary>
        public bool ItemImagesVisible { get => Handler.ItemImagesVisible; set => Handler.ItemImagesVisible = value; }

        private void Items_ItemInserted(object? sender, CollectionChangeEventArgs<ToolbarItem> e)
        {
            // This is required for data binding inheritance. ???
            // This commented out as added additional dummy toolbar items
            // Children.Add(e.Item);
        }

        private void Items_ItemRemoved(object? sender, CollectionChangeEventArgs<ToolbarItem> e)
        {
            // Commented out as Children.Add(e.Item) was commented
            // Children.Remove(e.Item);
        }

        /// <summary>
        /// Gets a collection of <see cref="MenuItem"/> objects associated with the menu.
        /// </summary>
        [Content]
        public Collection<ToolbarItem> Items { get; } = new Collection<ToolbarItem>();

        /// <inheritdoc/>
        public override IReadOnlyList<FrameworkElement> ContentElements => Items;

        /// <inheritdoc />
        protected override IEnumerable<FrameworkElement> LogicalChildrenCollection => Items;
    }
}