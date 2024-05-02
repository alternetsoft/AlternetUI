using System;
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
    /// <see cref="ToolBar"/> as it is deprecated and has limited functionality.
    /// </remarks>
    [DefaultProperty("Items")]
    [ControlCategory("MenusAndToolbars")]
    public partial class ToolBar : NonVisualControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToolBar"/> class.
        /// </summary>
        public ToolBar()
        {
            Items.ItemInserted += Items_ItemInserted;
            Items.ItemRemoved += Items_ItemRemoved;
        }

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
        public override ControlTypeId ControlKind => ControlTypeId.Toolbar;

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
        public ImageToText ImageToText
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
        public Collection<ToolBarItem> Items { get; } = new() { ThrowOnNullAdd = true };

        /// <inheritdoc/>
        public override IReadOnlyList<FrameworkElement> ContentElements => Items;

        internal new ToolBarHandler Handler
        {
            get
            {
                CheckDisposed();
                return (ToolBarHandler)base.Handler;
            }
        }

        internal override bool IsDummy => true;

        /// <inheritdoc />
        protected override
            IEnumerable<FrameworkElement> LogicalChildrenCollection => Items;

        /// <summary>
        /// Returns suggested toolbar image size in pixels depending on the given DPI value.
        /// </summary>
        /// <param name="control">Control's <see cref="Control.GetDPI"/> method
        /// is used to get DPI settings.</param>
        public static SizeI GetDefaultImageSize(Control? control = null)
        {
            control ??= Application.FirstWindow();
            SizeD? dpi;
            if (control is null)
                dpi = Window.DefaultDPI ?? Display.Primary.DPI;
            else
                dpi = control.GetDPI();
            if (dpi is null)
                throw new ArgumentNullException(nameof(control));
            var result = ToolbarUtils.GetDefaultImageSize(dpi.Value);
            return result;
        }

        /// <summary>
        /// Returns suggested toolbar image size depending on the current DPI.
        /// </summary>
        public int GetDefaultImageSize()
        {
            SizeD deviceDpi = GetDPI();
            return ToolbarUtils.GetDefaultImageSize(deviceDpi.Width);
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
        internal override ControlHandler CreateHandler()
        {
            return GetEffectiveControlHandlerHactory().CreateToolbarHandler(this);
        }

        private void Items_ItemInserted(object? sender, int index, ToolBarItem item)
        {
            // This is required for data binding inheritance. ???
            // This commented out as added additional dummy toolbar items
            // Children.Add(e.Item);
        }

        private void Items_ItemRemoved(object? sender, int index, ToolBarItem item)
        {
            // Commented out as Children.Add(e.Item) was commented
            // Children.Remove(e.Item);
        }
    }
}