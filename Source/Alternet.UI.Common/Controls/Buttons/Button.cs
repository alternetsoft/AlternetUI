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
        private bool isDefault;
        private bool isCancel;

        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class
        /// with the specified parent control.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public Button(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

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
        /// Initializes a new instance of the <see cref="Button"/> class
        /// with the specified text, click action and parent control.
        /// </summary>
        public Button(Control parent, string text, Action? clickAction = null)
            : this()
        {
            Text = text;
            ClickAction = clickAction;
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class
        /// with the specified text and click action.
        /// </summary>
        public Button(string text, Action clickAction)
            : this()
        {
            Text = text;
            ClickAction = clickAction;
        }

        /// <summary>
        /// Gets or sets whether images in buttons are available.
        /// </summary>
        /// <remarks>
        /// By default images in buttons are not available under macOs due to not correct
        /// implementation in WxWidget native button control. On all other platforms
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
                if (DisposingOrDisposed)
                    return default;
                return base.Handler.HasBorder;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                base.Handler.HasBorder = value;
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
            get
            {
                return isDefault;
            }

            set
            {
                isDefault = value;
            }
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
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return Handler.ExactFit;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.ExactFit = value;
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether a <see cref="Button"/>
        /// is a 'Cancel' button. In a modal dialog, a
        /// user can activate the 'Cancel' button by pressing the ESC key.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the <see cref="Button"/> is a 'Cancel'
        /// button; otherwise, <see langword="false"/>.
        /// The default is <see langword="false"/>.
        /// </value>
        public virtual bool IsCancel
        {
            get
            {
                return isCancel;
            }

            set
            {
                isCancel = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the drop down menu
        /// is shown when the control is clicked. Default is <see langword="true"/>.
        /// </summary>
        [Browsable(true)]
        public virtual bool ShowDropDownMenuWhenClicked { get; set; } = true;

        /// <summary>
        /// Gets or sets <see cref="ContextMenu"/> which is shown when control is clicked.
        /// </summary>
        [Browsable(false)]
        public virtual ContextMenu? DropDownMenu { get; set; }

        /// <summary>
        /// Gets or sets visibility of the text in the bitmap.
        /// </summary>
        public virtual bool TextVisible
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return Handler.TextVisible;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.TextVisible = value;
            }
        }

        /// <summary>
        /// Gets a <see cref="IButtonHandler"/> associated with this class.
        /// </summary>
        [Browsable(false)]
        public new IButtonHandler Handler => (IButtonHandler)base.Handler;

        /// <summary>
        /// Sets the position at which the text is displayed.
        /// </summary>
        /// <remarks>
        /// Valid positions are left, top, right, bottom, default.
        /// </remarks>
        public virtual GenericDirection TextAlign
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return Handler.TextAlign;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.TextAlign = value;
            }
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
        /// Sets the position at which the image is displayed.
        /// </summary>
        /// <remarks>
        /// This method should only be called if the button does have
        /// an associated image.
        /// </remarks>
        /// <param name="dir">New image position (left, top, right, bottom).</param>
        public virtual void SetImagePosition(GenericDirection dir)
        {
            if (DisposingOrDisposed)
                return;
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
        public virtual void SetImageMargins(Coord x, Coord? y = null)
        {
            if (DisposingOrDisposed)
                return;
            y ??= x;
            Handler.SetImageMargins(x, y.Value);
        }

        void IControlStateObjectChanged.DisabledChanged(object? sender)
        {
            if (DisposingOrDisposed)
                return;
            Handler.DisabledImage = stateImages?.Disabled;
            PerformLayoutAndInvalidate();
        }

        void IControlStateObjectChanged.NormalChanged(object? sender)
        {
            if (DisposingOrDisposed)
                return;
            Handler.NormalImage = stateImages?.Normal;
            PerformLayoutAndInvalidate();
        }

        void IControlStateObjectChanged.FocusedChanged(object? sender)
        {
            if (DisposingOrDisposed)
                return;
            Handler.FocusedImage = stateImages?.Focused;
            PerformLayoutAndInvalidate();
        }

        void IControlStateObjectChanged.HoveredChanged(object? sender)
        {
            if (DisposingOrDisposed)
                return;
            Handler.HoveredImage = stateImages?.Hovered;
            PerformLayoutAndInvalidate();
        }

        void IControlStateObjectChanged.PressedChanged(object? sender)
        {
            if (DisposingOrDisposed)
                return;
            Handler.PressedImage = stateImages?.Pressed;
            PerformLayoutAndInvalidate();
        }

        void IControlStateObjectChanged.SelectedChanged(object? sender)
        {
        }

        /// <summary>
        /// Shows attached drop down menu.
        /// </summary>
        protected virtual void ShowDropDownMenu()
        {
            DropDownMenu?.ShowAsDropDown(this);
        }

        /// <inheritdoc/>
        protected override void OnMouseLeftButtonUp(MouseEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            if (!Enabled)
                return;
            if (ShowDropDownMenuWhenClicked)
                ShowDropDownMenu();
        }

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return ControlFactory.Handler.CreateButtonHandler(this);
        }
    }
}