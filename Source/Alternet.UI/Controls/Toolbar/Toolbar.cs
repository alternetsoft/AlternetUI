using System.Collections.Generic;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a toolbar control.
    /// </summary>
    public class Toolbar : Control
    {
        /// <summary>
        /// Defines the default size of toolbar images for displays
        /// with 96 DPI.
        /// </summary>
        /// <remarks>
        /// The size is the width and height of the image in pixels.
        /// You can use it to change the default size of toolbar buttons
        /// for more comfortable use.
        /// </remarks>
        /// <remarks>
        /// DPI stands for dots per inch and affects on clarity and crispness
        /// of the display. DPI is determined by the size of the screen and
        /// its resolution.
        /// </remarks>
        /// <remarks>
        /// Default property value is 16. Suggested values are 16, 24, 32, 48.
        /// </remarks>
#pragma warning disable SA1401
        public static int DefaultImageSize96dpi = 16;
#pragma warning restore SA1401

        /// <summary>
        /// Defines the default size of toolbar images for displays
        /// with 144 DPI.
        /// </summary>
        /// <remarks>
        /// The size is the width and height of the image in pixels.
        /// You can use it to change the default size of toolbar buttons
        /// for more comfortable use.
        /// </remarks>
        /// <remarks>
        /// DPI stands for dots per inch and affects on clarity and crispness
        /// of the display. DPI is determined by the size of the screen and
        /// its resolution.
        /// </remarks>
        /// <remarks>
        /// Default property value is 24. Suggested values are 16, 24, 32, 48.
        /// </remarks>
#pragma warning disable SA1401
        public static int DefaultImageSize144dpi = 24;
#pragma warning restore SA1401

        /// <summary>
        /// Defines the default size of toolbar images for displays
        /// with 192 DPI.
        /// </summary>
        /// <remarks>
        /// The size is the width and height of the image in pixels.
        /// You can use it to change the default size of toolbar buttons
        /// for more comfortable use.
        /// </remarks>
        /// <remarks>
        /// DPI stands for dots per inch and affects on clarity and crispness
        /// of the display. DPI is determined by the size of the screen and
        /// its resolution.
        /// </remarks>
        /// <remarks>
        /// Default property value is 32. Suggested values are 16, 24, 32, 48.
        /// </remarks>
#pragma warning disable SA1401
        public static int DefaultImageSize192dpi = 32;
#pragma warning restore SA1401

        /// <summary>
        /// Defines the default size of toolbar images for displays
        /// with 288 DPI.
        /// </summary>
        /// <remarks>
        /// The size is the width and height of the image in pixels.
        /// You can use it to change the default size of toolbar buttons
        /// for more comfortable use.
        /// </remarks>
        /// <remarks>
        /// DPI stands for dots per inch and affects on clarity and crispness
        /// of the display. DPI is determined by the size of the screen and
        /// its resolution.
        /// </remarks>
        /// <remarks>
        /// Default property value is 48. Suggested values are 16, 24, 32, 48.
        /// </remarks>
#pragma warning disable SA1401
        public static int DefaultImageSize288dpi = 48;
#pragma warning restore SA1401

        /// <summary>
        /// Initializes a new instance of the <see cref="Toolbar"/> class.
        /// </summary>
        public Toolbar()
        {
            Items.ItemInserted += Items_ItemInserted;
            Items.ItemRemoved += Items_ItemRemoved;
        }

        internal new ToolbarHandler Handler
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

        public bool NoDivider
        {
            get
            {
                return Handler.NoDivider;
            }

            set
            {
                Handler.NoDivider = value;
            }
        }

        public bool IsVertical
        {
            get
            {
                return Handler.IsVertical;
            }

            set
            {
                Handler.IsVertical = value;
            }
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

        public int GetDefaultImageSize()
        {
            Size deviceDpi = GetDPI();
            return GetDefaultImageSize(deviceDpi.Width);
        }

        public void Realize()
        {
            Handler.Realize();
        }

        public static int GetDefaultImageSize(double deviceDpi)
        {
            decimal deviceDpiRatio = (decimal)deviceDpi / 96m;

            if (deviceDpi <= 96 || deviceDpiRatio < 1.5m)
                return DefaultImageSize96dpi;
            if (deviceDpiRatio < 2m)
                return DefaultImageSize144dpi;
            if (deviceDpiRatio < 3m)
                return DefaultImageSize192dpi;
            return DefaultImageSize288dpi;
        }

        /// <inheritdoc />
        protected override ControlHandler CreateHandler()
        {
            return new NativeToolbarHandler(true);
        }
    }
}