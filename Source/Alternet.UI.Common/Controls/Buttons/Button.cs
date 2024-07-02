using System;
using System.ComponentModel;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a button control.
    /// </summary>
    [ControlCategory("Common")]
    public partial class Button : ButtonBase, IControlStateObjectChanged
    {
        private static bool imagesEnabled = true;

        private ControlStateImages? stateImages;

        /// <summary>
        /// Initializes a new <see cref="Button"/> instance.
        /// </summary>
        public Button()
        {
        }

        /// <summary>
        /// Initializes a new <see cref="Button"/> instance with the specified text.
        /// </summary>
        public Button(string text)
            : this()
        {
            Text = text;
        }

        /// <summary>
        /// Gets or sets whether images in buttons are available.
        /// </summary>
        /// <remarks>
        /// By default under Macos images in buttons not available due to not correct
        /// implementation in WxWidget native button control. On all other operating systems
        /// images in buttons are available.
        /// </remarks>
        public static bool ImagesEnabled
        {
            get
            {
                return imagesEnabled;
            }

            set
            {
                imagesEnabled = value;
            }
        }

        /// <summary>
        /// Specifies a set of images for different <see cref="Button"/> states.
        /// </summary>
        [Browsable(false)]
        public virtual ControlStateImages StateImages
        {
            get
            {
                if(stateImages is null)
                {
                    stateImages = new();
                    stateImages.ChangedHandler = this;
                }

                return stateImages;
            }

            set
            {
                if (!imagesEnabled || stateImages == value)
                    return;
                PerformLayoutAndInvalidate(() =>
                {
                    StateImages.Assign(value);
                });
            }
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.Button;

        /// <summary>
        /// Gets or sets a value indicating whether the control has a border.
        /// </summary>
        [Browsable(true)]
        public override bool HasBorder
        {
            get
            {
                return Handler.HasBorder;
            }

            set
            {
                Handler.HasBorder = value;
            }
        }

        /// <summary>
        /// Gets or sets the image that is displayed on a button control.
        /// </summary>
        /// <value>
        /// The <see cref="Image"/> displayed on the button control. The default
        /// value is <see langword="null"/>.
        /// </value>
        public virtual Image? Image
        {
            get
            {
                return StateImages.Normal;
            }

            set
            {
                if (!imagesEnabled)
                    return;
                StateImages.Normal = value;
            }
        }

        /// <summary>
        /// Gets or sets an <see cref="Image"/> for hovered control state.
        /// </summary>
        public virtual Image? HoveredImage
        {
            get => StateImages.Hovered;

            set
            {
                if (!imagesEnabled)
                    return;
                StateImages.Hovered = value;
            }
        }

        /// <summary>
        /// Gets or sets an <see cref="Image"/> for focused control state.
        /// </summary>
        public virtual Image? FocusedImage
        {
            get => StateImages.Focused;

            set
            {
                if (!imagesEnabled)
                    return;
                StateImages.Focused = value;
            }
        }

        /// <summary>
        /// Gets or sets an <see cref="Image"/> for pressed control state.
        /// </summary>
        public virtual Image? PressedImage
        {
            get => StateImages.Pressed;

            set
            {
                if (!imagesEnabled)
                    return;
                StateImages.Pressed = value;
            }
        }

        /// <summary>
        /// Gets or sets an <see cref="Image"/> for disabled control state.
        /// </summary>
        public virtual Image? DisabledImage
        {
            get => StateImages.Disabled;

            set
            {
                if (!imagesEnabled)
                    return;
                StateImages.Disabled = value;
            }
        }

        /// <summary>
        /// Gets or sets a value that determines if the background is drawn using visual styles,
        /// if supported.</summary>
        /// <returns>
        /// <see langword="true" /> if the background is drawn using visual styles;
        /// otherwise, <see langword="false" />.</returns>
        /// <remarks>
        /// Currently this property doesn't do anything and is added for compatibility.
        /// </remarks>
        [Category("Appearance")]
        [Browsable(false)]
        public virtual bool UseVisualStyleBackColor { get; set; } = true;

        /// <summary>
        /// Gets or sets a value that indicates whether a <see cref="Button"/> is
        /// the default button. In a modal dialog,
        /// a user invokes the default button by pressing the ENTER key.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the <see cref="Button"/> is the default
        /// button; otherwise, <see
        /// langword="false"/>. The default is <see langword="false"/>.
        /// </value>
        public virtual bool IsDefault
        {
            get => Handler.IsDefault;
            set => Handler.IsDefault = value;
        }

        /// <summary>
        /// Gets or sets whether buttons are made of at least the standard button size, even
        /// if their contents is small enough to fit into a smaller size.
        /// </summary>
        /// <remarks>
        /// Standard button size is used
        /// for consistency as most platforms use buttons of the same size in the native
        /// dialogs. If this flag is specified, the
        /// button will be made just big enough for its contents. Notice that under MSW
        /// the button will still have at least the standard height, even with this style,
        /// if it has a non-empty label.
        /// </remarks>
        public virtual bool ExactFit
        {
            get => Handler.ExactFit;
            set => Handler.ExactFit = value;
        }

        /// <summary>
        /// Gets or sets a value that indicates whether a <see cref="Button"/>
        /// is a Cancel button. In a modal dialog, a
        /// user can activate the Cancel button by pressing the ESC key.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the <see cref="Button"/> is a Cancel
        /// button; otherwise, <see langword="false"/>.
        /// The default is <see langword="false"/>.
        /// </value>
        public virtual bool IsCancel
        {
            get => Handler.IsCancel;
            set => Handler.IsCancel = value;
        }

        /// <summary>
        /// Gets or sets visibility of the text in the bitmap.
        /// </summary>
        public virtual bool TextVisible
        {
            get => Handler.TextVisible;
            set => Handler.TextVisible = value;
        }

        /// <summary>
        /// Sets the position at which the text is displayed.
        /// </summary>
        /// <remarks>
        /// Valid positions are left, top, right, bottom, default.
        /// </remarks>
        public virtual GenericDirection TextAlign
        {
            get => Handler.TextAlign;
            set => Handler.TextAlign = value;
        }

        [Browsable(false)]
        internal new LayoutStyle? Layout
        {
            get => base.Layout;
        }

        [Browsable(false)]
        internal new string Title
        {
            get => base.Title;
            set => base.Title = value;
        }

        [Browsable(false)]
        internal new Thickness Padding
        {
            get => base.Padding;
        }

        /// <summary>
        /// Gets a <see cref="IButtonHandler"/> associated with this class.
        /// </summary>
        [Browsable(false)]
        internal new IButtonHandler Handler => (IButtonHandler)base.Handler;

        /// <summary>
        /// Sets the position at which the image is displayed.
        /// </summary>
        /// <remarks>
        /// This method should only be called if the button does have
        /// an associated image.
        /// </remarks>
        /// <param name="dir">New image position (left, top, right, bottom).</param>
        public virtual void SetImagePosition(GenericDirection dir)
        {
            if (dir == GenericDirection.Left || dir == GenericDirection.Right ||
                dir == GenericDirection.Top || dir == GenericDirection.Bottom)
                Handler.SetImagePosition(dir);
        }

        /// <summary>
        /// Sets the margins between the image and the text of the button.
        /// Value is in device-independent units.
        /// </summary>
        /// <remarks>
        /// This method is currently only implemented under Windows.
        /// If it is not called, default margin is used around the image.
        /// </remarks>
        /// <param name="x">New horizontal margin.</param>
        /// <param name="y">New vertical margin.</param>
        public virtual void SetImageMargins(double x, double? y = null)
        {
            y ??= x;
            Handler.SetImageMargins(x, y.Value);
        }

        void IControlStateObjectChanged.DisabledChanged(object? sender)
        {
            Handler.DisabledImage = stateImages?.Disabled;
            PerformLayoutAndInvalidate();
        }

        void IControlStateObjectChanged.NormalChanged(object? sender)
        {
            Handler.NormalImage = stateImages?.Normal;
            PerformLayoutAndInvalidate();
        }

        void IControlStateObjectChanged.FocusedChanged(object? sender)
        {
            Handler.FocusedImage = stateImages?.Focused;
            PerformLayoutAndInvalidate();
        }

        void IControlStateObjectChanged.HoveredChanged(object? sender)
        {
            Handler.HoveredImage = stateImages?.Hovered;
            PerformLayoutAndInvalidate();
        }

        void IControlStateObjectChanged.PressedChanged(object? sender)
        {
            Handler.PressedImage = stateImages?.Pressed;
            PerformLayoutAndInvalidate();
        }

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return ControlFactory.Handler.CreateButtonHandler(this);
        }

        /// <inheritdoc/>
        protected override void BindHandlerEvents()
        {
            base.BindHandlerEvents();
            Handler.Click = RaiseClick;
        }

        /// <inheritdoc/>
        protected override void UnbindHandlerEvents()
        {
            base.UnbindHandlerEvents();
            Handler.Click = null;
        }
    }
}