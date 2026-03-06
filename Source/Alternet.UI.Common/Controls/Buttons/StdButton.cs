using System;
using System.ComponentModel;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a generic button control. This control is implemented inside the library and can be used in the
    /// same way as a regular native <see cref="Button"/> control. <see cref="StdButton"/> is used when you need
    /// to have the same code for all platforms. <see cref="StdButton"/> provides many additional features,
    /// which are not available in the native <see cref="Button"/> control.
    /// </summary>
    [ControlCategory("Common")]
    public partial class StdButton : GenericItemControl, IControlStateObjectChanged
    {
        /// <summary>
        /// Represents the default padding value which is applied to <see cref="StdButton"/> controls in the constructor.
        /// </summary>
        /// <remarks>This static field provides a standard padding size that is applied to <see cref="StdButton"/> controls
        /// to ensure consistent spacing across components.</remarks>
        public static Thickness DefaultPadding = 3;

        private ControlStateImages? stateImages;
        private bool isDefault;
        private bool isCancel;
        private bool textVisible = true;
        private ElementContentAlign textAlign = ElementContentAlign.Default;
        private ElementContentAlign imageAlign = ElementContentAlign.Default;
        private Thickness? imageMargins;
        private bool exactFit = false;

        static StdButton()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StdButton"/> class
        /// with the specified parent control.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public StdButton(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new <see cref="StdButton"/> instance.
        /// </summary>
        public StdButton()
        {
            UniformBorderCornerRadius = MaterialDesign.CornerRadius.Button;
            Padding = DefaultPadding;
            Item.HorizontalAlignment = HorizontalAlignment.Center;
            RefreshOptions = ControlRefreshOptions.RefreshOnBorder | ControlRefreshOptions.RefreshOnImage |
                ControlRefreshOptions.RefreshOnBackground | ControlRefreshOptions.RefreshOnColor |
                ControlRefreshOptions.RefreshOnState;
        }

        /// <summary>
        /// Initializes a new <see cref="StdButton"/> instance with the specified text.
        /// </summary>
        public StdButton(string text)
            : this()
        {
            Text = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StdButton"/> class
        /// with the specified text, click action and parent control.
        /// </summary>
        public StdButton(Control parent, string text, Action? clickAction = null)
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
        public StdButton(string text, Action clickAction)
            : this()
        {
            Text = text;
            ClickAction = clickAction;
        }

        /// <summary>
        /// Specifies a set of images for different <see cref="Button"/> states.
        /// </summary>
        [Browsable(false)]
        public virtual ControlStateImages StateImages
        {
            get
            {
                if (stateImages is null)
                {
                    stateImages = new();
                    stateImages.ChangedHandler = this;
                }

                return stateImages;
            }

            set
            {
                if (stateImages == value)
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
                if (DisposingOrDisposed)
                    return;
                if (isDefault == value)
                    return;
                isDefault = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets whether buttons are made of at least the standard button size, even
        /// if their contents is small enough to fit into a smaller size.
        /// Currently this property doesn't do anything and is added for compatibility.
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
                return exactFit;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                if (exactFit == value)
                    return;
                exactFit = value;
                PerformLayoutAndInvalidate();
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
                if (DisposingOrDisposed)
                    return;
                if (isCancel == value)
                    return;
                isCancel = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets visibility of the text.
        /// </summary>
        public virtual bool TextVisible
        {
            get
            {
                return textVisible;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                if (textVisible == value)
                    return;
                textVisible = value;
                PerformLayoutAndInvalidate();
            }
        }

        /// <summary>
        /// Sets the position at which the text is displayed.
        /// </summary>
        public virtual ElementContentAlign TextAlign
        {
            get
            {
                return textAlign;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                if (textAlign == value)
                    return;
                textAlign = value;
                if (TextVisible)
                    PerformLayoutAndInvalidate();
            }
        }

        /// <summary>
        /// Sets the position at which the image is displayed.
        /// </summary>
        /// <param name="dir">New image position (left, top, right, bottom).</param>
        public virtual void SetImagePosition(ElementContentAlign dir)
        {
            if (DisposingOrDisposed)
                return;
            if (imageAlign == dir)
                return;
            imageAlign = dir;
            if (Item.HasImageOrSvg)
                PerformLayoutAndInvalidate();
        }

        /// <summary>
        /// Sets the margins between the image and the text of the button.
        /// Value is in device-independent units.
        /// </summary>
        /// <remarks>
        /// If this method was not called, default margin is used around the image.
        /// </remarks>
        /// <param name="x">New horizontal margin.</param>
        /// <param name="y">New vertical margin.</param>
        public virtual void SetImageMargins(Coord x, Coord? y = null)
        {
            if (DisposingOrDisposed)
                return;
            y ??= x;

            var newMargins = new Thickness(x, y.Value);

            if (imageMargins == newMargins)
                return;
            imageMargins = newMargins;

            if (Item.HasImageOrSvg)
                PerformLayoutAndInvalidate();
        }

        /// <inheritdoc/>
        public override void DefaultPaint(PaintEventArgs e)
        {
            UpdateItemImage();
            base.DefaultPaint(e);
        }

        /// <inheritdoc/>
        public override SizeD GetPreferredSize(PreferredSizeContext context)
        {
            UpdateItemImage();
            return base.GetPreferredSize(context);
        }

        void IControlStateObjectChanged.DisabledChanged(object? sender)
        {
            if (DisposingOrDisposed)
                return;
            if (!Enabled)
                PerformLayoutAndInvalidate();
        }

        void IControlStateObjectChanged.NormalChanged(object? sender)
        {
            if (DisposingOrDisposed)
                return;
            if (VisualState == VisualControlState.Normal)
                PerformLayoutAndInvalidate();
        }

        void IControlStateObjectChanged.FocusedChanged(object? sender)
        {
            if (DisposingOrDisposed)
                return;
            if (VisualState == VisualControlState.Focused)
                PerformLayoutAndInvalidate();
        }

        void IControlStateObjectChanged.HoveredChanged(object? sender)
        {
            if (DisposingOrDisposed)
                return;
            if (VisualState == VisualControlState.Hovered)
                PerformLayoutAndInvalidate();
        }

        void IControlStateObjectChanged.PressedChanged(object? sender)
        {
            if (DisposingOrDisposed)
                return;
            if (VisualState == VisualControlState.Pressed)
                PerformLayoutAndInvalidate();
        }

        void IControlStateObjectChanged.SelectedChanged(object? sender)
        {
        }

        /// <summary>
        /// Updates the button image according to the current visual state and images specified.
        /// </summary>
        protected virtual void UpdateItemImage()
        {
            if (DisposingOrDisposed)
                return;
            var state = VisualState;

            if(state != VisualControlState.Normal)
            {
            }

            var image = StateImages.GetObjectOrNormal(state);

            Item.Image = image;
        }

        /// <inheritdoc/>
        protected override void OnVisualStateChanged(EventArgs e)
        {
            base.OnVisualStateChanged(e);
        }

        /// <inheritdoc/>
        protected override bool GetDefaultHasBorder()
        {
            return true;
        }
    }
}