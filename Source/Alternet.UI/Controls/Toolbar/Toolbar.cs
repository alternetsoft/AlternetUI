using System.Collections.Generic;
using System.ComponentModel;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a toolbar control.
    /// </summary>
    /// <remarks>
    /// Please use <see cref="AuiManager"/> and <see cref="AuiToolbar"/> instead of
    /// <see cref="Toolbar"/> as it is deprecated and has limited functionality.
    /// </remarks>
    [DefaultProperty("Items")]
    public class Toolbar : NonVisualControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Toolbar"/> class.
        /// </summary>
        public Toolbar()
        {
            Items.ItemInserted += Items_ItemInserted;
            Items.ItemRemoved += Items_ItemRemoved;
        }

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
        public static int DefaultImageSize96dpi { get; set; } = 24;

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
        public static int DefaultImageSize144dpi { get; set; } = 32;

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
        public static int DefaultImageSize192dpi { get; set; } = 32;

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
        public static int DefaultImageSize288dpi { get; set; } = 48;

        /// <summary>
        /// Gets or sets a value indicating whether to align the toolbar at
        /// the bottom of parent window.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if toolbar is aligned at the bottom; otherwise,
        /// <see langword="false"/>. The default is <see langword="false"/>.
        /// </value>
        /// <remarks>This property affects only horizontal toolbars.</remarks>
        public bool IsBottom
        {
            get
            {
                return Handler.IsBottom;
            }

            set
            {
                Handler.IsBottom = value;
            }
        }

        /// <inheritdoc/>
        public override ControlId ControlKind => ControlId.Toolbar;

        /// <summary>
        /// Gets or sets a value indicating whether to align the toolbar at
        /// the right side of parent window.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if toolbar is aligned at the right side; otherwise,
        /// <see langword="false"/>. The default is <see langword="false"/>.
        /// </value>
        /// <remarks>This property affects only vertical toolbars.</remarks>
        public bool IsRight
        {
            get
            {
                return Handler.IsRight;
            }

            set
            {
                Handler.IsRight = value;
            }
        }

        /// <summary>
        /// Gets or sets a value which specifies display modes for
        /// toolbar item image and text.
        /// </summary>
        public ToolbarImageToText ImageToText
        {
            get => Handler.ImageToText;
            set => Handler.ImageToText = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to draw divider (border)
        /// above the toolbar (Windows only).
        /// </summary>
        /// <value>
        /// <see langword="true"/> if toolbar border is shown; otherwise,
        /// <see langword="false"/>. The default is <see langword="true"/>.
        /// </value>
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

        /// <summary>
        /// Gets or sets a value indicating whether to align the toolbar vertically.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if toolbar is aligned vertically; otherwise,
        /// <see langword="false"/>. The default is <see langword="false"/>.
        /// </value>
        /// <remarks>Vertical toolbars are aligned at the left or right side of
        /// parent window.</remarks>
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
        /// Gets or sets a boolean value indicating whether this toolbar item
        /// text is visible.
        /// </summary>
        public bool ItemTextVisible
        {
            get => Handler.ItemTextVisible;
            set => Handler.ItemTextVisible = value;
        }

        /// <summary>
        /// Gets or sets a boolean value indicating whether this toolbar item
        /// images are visible.
        /// </summary>
        public bool ItemImagesVisible
        {
            get => Handler.ItemImagesVisible;
            set => Handler.ItemImagesVisible = value;
        }

        /// <summary>
        /// Gets a collection of <see cref="MenuItem"/> objects associated
        /// with the menu.
        /// </summary>
        [Content]
        public Collection<ToolbarItem> Items { get; } = new() { ThrowOnNullAdd = true };

        /// <inheritdoc/>
        public override IReadOnlyList<FrameworkElement> ContentElements => Items;

        internal new ToolbarHandler Handler
        {
            get
            {
                CheckDisposed();
                return (ToolbarHandler)base.Handler;
            }
        }

        internal override bool IsDummy => true;

        /// <inheritdoc />
        protected override
            IEnumerable<FrameworkElement> LogicalChildrenCollection => Items;

        /// <summary>
        /// Returns suggested toolbar image size depending on the given DPI value.
        /// </summary>
        /// <param name="deviceDpi">DPI for which default image size is
        /// returned.</param>
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

        /// <inheritdoc cref="GetDefaultImageSize(double)"/>
        public static Int32Size GetDefaultImageSize(Size deviceDpi)
        {
            var width = GetDefaultImageSize(deviceDpi.Width);
            var height = GetDefaultImageSize(deviceDpi.Height);
            return new(width, height);
        }

        /// <summary>
        /// Returns suggested toolbar image size depending on the given DPI value.
        /// </summary>
        /// <param name="control">Control's <see cref="Control.GetDPI"/> method
        /// is used to get DPI settings.</param>
        public static Int32Size GetDefaultImageSize(Control control)
        {
            var result = GetDefaultImageSize(control.GetDPI());
            return result;
        }

        /// <summary>
        /// Returns suggested toolbar image size depending on the current DPI.
        /// </summary>
        public int GetDefaultImageSize()
        {
            Size deviceDpi = GetDPI();
            return GetDefaultImageSize(deviceDpi.Width);
        }

        /// <summary>
        /// Updates visual representation of the toolbar after it's items where
        /// added, deleted or updated. In most cases this method is called
        /// automatically, so you don't need to call it.
        /// </summary>
        public void Realize()
        {
            Handler.Realize();
        }

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return GetEffectiveControlHandlerHactory().CreateToolbarHandler(this);
        }

        private void Items_ItemInserted(object? sender, int index, ToolbarItem item)
        {
            // This is required for data binding inheritance. ???
            // This commented out as added additional dummy toolbar items
            // Children.Add(e.Item);
        }

        private void Items_ItemRemoved(object? sender, int index, ToolbarItem item)
        {
            // Commented out as Children.Add(e.Item) was commented
            // Children.Remove(e.Item);
        }
    }
}