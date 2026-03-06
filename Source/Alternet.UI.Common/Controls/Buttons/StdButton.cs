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

        /// <summary>
        /// Represents the default minimum size for the component, expressed as a SizeD structure.
        /// </summary>
        public static SizeD DefaultMinSize = 32;

        private static LightDarkColor? defaultHotBorderColor;
        private static LightDarkColor? defaultBorderColor;
        private static LightDarkColor? defaultBorderColorIsd;
        private static LightDarkColor? defaultHoveredBorderColorIsd;
        private static LightDarkColor? defaultBackColorIsd;
        private static LightDarkColor? defaultForeColorIsd;
        private static LightDarkColor? defaultBackColor;
        private static LightDarkColor? defaultForeColor;

        private ControlStateImages? stateImages;
        private ControlStateSvgImages? stateSvgImages;
        private bool isDefault;
        private bool isCancel;
        private ElementContentAlign textAlign = ElementContentAlign.Default;
        private ElementContentAlign imageAlign = ElementContentAlign.Default;
        private Thickness? imageMargins;
        private bool exactFit = false;
        private int? svgSize;

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
            WantTab = true;
            IsGraphicControl = false;
            IsTransparent = false;

            var radius = MaterialDesign.CornerRadius.Button;

            UniformBorderCornerRadius = radius;

            if (Borders is not null)
            {
                Borders.Hovered = Borders.Normal?.Clone();
                Borders.Selected = Borders.Normal?.Clone();
                Borders.Pressed = Borders.Normal?.Clone();
                Borders.Disabled = Borders.Normal?.Clone();
            }

            UseControlColors(true);

            MinimumSize = DefaultMinSize;
            Padding = DefaultPadding;
            Item.HorizontalAlignment = HorizontalAlignment.Center;
            RefreshOptions = ControlRefreshOptions.RefreshOnBorder | ControlRefreshOptions.RefreshOnImage |
                ControlRefreshOptions.RefreshOnBackground | ControlRefreshOptions.RefreshOnColor |
                ControlRefreshOptions.RefreshOnState;

            SetColorTheme();
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
        /// Initializes a new instance of the <see cref="StdButton"/> class
        /// with the specified text and click action.
        /// </summary>
        public StdButton(string text, Action clickAction)
            : this()
        {
            Text = text;
            ClickAction = clickAction;
        }

        /// <summary>
        /// Gets or sets the default background color used for <see cref="StdButton"/> control when
        /// it's <see cref="IsDefault"/> property is set to true.
        /// </summary>
        public static LightDarkColor DefaultBackColorIsd
        {
            get => defaultBackColorIsd ?? DefaultBackColor;
            set => defaultBackColorIsd = value;
        }

        /// <summary>
        /// Gets or sets the default foreground color used for <see cref="StdButton"/> control when
        /// it's <see cref="IsDefault"/> property is set to true.
        /// </summary>
        public static LightDarkColor DefaultForeColorIsd
        {
            get => defaultForeColorIsd ?? DefaultForeColor;
            set => defaultForeColorIsd = value;
        }

        /// <summary>
        /// Gets or sets the default background color used for <see cref="StdButton"/> control.
        /// </summary>
        public static new LightDarkColor DefaultBackColor
        {
            get => defaultBackColor ?? DefaultColors.ControlBackColor;
            set => defaultBackColor = value;
        }

        /// <summary>
        /// Gets or sets the default foreground color used for <see cref="StdButton"/> control.
        /// </summary>
        public static new LightDarkColor DefaultForeColor
        {
            get => defaultForeColor ?? DefaultColors.ControlForeColor;
            set => defaultForeColor = value;
        }

        /// <summary>
        /// Gets or sets the default border color used for <see cref="StdButton"/> elements in a hot (hovered or active) state.
        /// </summary>
        /// <remarks>The default value is initialized to <see cref="LightDarkColors.Blue"/> if not previously set.</remarks>
        public static LightDarkColor DefaultHoveredBorderColor
        {
            get
            {
                return defaultHotBorderColor ?? DefaultColors.DefaultCheckBoxColor;
            }

            set
            {
                defaultHotBorderColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the default border color used for <see cref="StdButton"/> controls in the application.
        /// </summary>
        /// <remarks>If not explicitly set, the default border color is initialized to <see cref="DefaultColors.BorderColor"/>.</remarks>
        public static LightDarkColor DefaultBorderColor
        {
            get
            {
                return defaultBorderColor ?? DefaultColors.BorderColor;
            }

            set
            {
                defaultBorderColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the default border color used for <see cref="StdButton"/> controls in the application
        /// when <see cref="IsDefault"/> property of the control is set to true.
        /// </summary>
        public static LightDarkColor DefaultBorderColorIsd
        {
            get
            {
                return defaultBorderColorIsd ?? DefaultColors.DefaultCheckBoxColor;
            }

            set
            {
                defaultBorderColorIsd = value;
            }
        }

        /// <summary>
        /// Gets or sets the default hovered border color used for <see cref="StdButton"/> controls in the application
        /// when <see cref="IsDefault"/> property of the control is set to true.
        /// </summary>
        public static LightDarkColor DefaultHoveredBorderColorIsd
        {
            get
            {
                return defaultHoveredBorderColorIsd ?? DefaultColors.DefaultCheckBoxColor;
            }

            set
            {
                defaultHoveredBorderColorIsd = value;
            }
        }

        /// <summary>
        /// Specifies a set of images for different control states.
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

        /// <summary>
        /// Specifies a set of svg images for different control states.
        /// </summary>
        [Browsable(false)]
        public virtual ControlStateSvgImages StateSvgImages
        {
            get
            {
                if (stateSvgImages is null)
                {
                    stateSvgImages = new();
                    stateSvgImages.ChangedHandler = this;
                }

                return stateSvgImages;
            }

            set
            {
                if (stateSvgImages == value)
                    return;
                PerformLayoutAndInvalidate(() =>
                {
                    StateSvgImages.Assign(value);
                });
            }
        }

        /// <summary>
        /// Gets or sets the svg image size.
        /// </summary>
        [DefaultValue(null)]
        public virtual int? SvgSize
        {
            get
            {
                return svgSize;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                if (svgSize == value)
                    return;
                PerformLayoutAndInvalidate(() =>
                {
                    svgSize = value;
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
        /// Gets or sets the alignment of the image within the control.
        /// </summary>
        /// <remarks>The alignment determines how the image is positioned relative to the element's content.
        /// Changing this property affects the visual layout of the image in relation to other content.</remarks>
        public ElementContentAlign ImageAlign
        {
            get
            {
                return imageAlign;
            }

            set
            {
                SetImagePosition(value);
            }
        }

        /// <summary>
        /// Gets or sets an <see cref="Image"/> for hovered control state.
        /// </summary>
        [Browsable(false)]
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
        [Browsable(false)]
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
        [Browsable(false)]
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
        [Browsable(false)]
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

                if (AutoUpdateColors)
                {
                    SetColorTheme();
                }

                Invalidate();
            }
        }

        /// <inheritdoc/>
        public override Color? BackgroundColor
        {
            get => base.BackgroundColor;
            set
            {
                if (DisposingOrDisposed)
                    return;
                if (BackgroundColor == value)
                    return;
                if (value is null)
                {
                    if (SystemSettings.AppearanceIsDark)
                        value = DefaultColors.ControlBackColor.Dark;
                    else
                        value = DefaultColors.ControlBackColor.Light;
                }

                base.BackgroundColor = value;
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
        [Browsable(false)]
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
                UpdateTextAndImageAlignment();
                if (TextVisible)
                    PerformLayoutAndInvalidate();
            }
        }

        /// <summary>
        /// Sets the position at which the image is displayed.
        /// </summary>
        /// <param name="dir">New image position.</param>
        public virtual void SetImagePosition(ElementContentAlign dir)
        {
            if (DisposingOrDisposed)
                return;
            if (imageAlign == dir)
                return;
            imageAlign = dir;
            UpdateTextAndImageAlignment();
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
        public override void DrawBorderAndBackground(
            PaintEventArgs e,
            DrawDefaultBackgroundFlags flags = DrawDefaultBackgroundFlags.DrawBorderAndBackground)
        {
            if (IsDefault)
            {
            }

            base.DrawBorderAndBackground(e, flags);
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
            PerformLayoutAndInvalidate();
        }

        void IControlStateObjectChanged.NormalChanged(object? sender)
        {
            if (DisposingOrDisposed)
                return;
            PerformLayoutAndInvalidate();
        }

        void IControlStateObjectChanged.FocusedChanged(object? sender)
        {
            if (DisposingOrDisposed)
                return;
            PerformLayoutAndInvalidate();
        }

        void IControlStateObjectChanged.HoveredChanged(object? sender)
        {
            if (DisposingOrDisposed)
                return;
            PerformLayoutAndInvalidate();
        }

        void IControlStateObjectChanged.PressedChanged(object? sender)
        {
            if (DisposingOrDisposed)
                return;
            PerformLayoutAndInvalidate();
        }

        void IControlStateObjectChanged.SelectedChanged(object? sender)
        {
        }

        /// <summary>
        /// Sets the application's color theme to match the current system appearance setting.
        /// </summary>
        /// <remarks>This method applies either a dark or light color theme based on the system's appearance preference. </remarks>
        public virtual void SetColorTheme()
        {
            if (SystemSettings.AppearanceIsDark)
                SetColorThemeToDark();
            else
                SetColorThemeToLight();
        }

        /// <summary>
        /// Sets colors used in the control to the dark theme.
        /// </summary>
        public virtual void SetColorThemeToDark()
        {
            DoInsideUpdate(() =>
            {
                if (IsDefault)
                {
                    BackColor = DefaultBackColorIsd.Dark;
                    ForeColor = DefaultForeColorIsd.Dark;
                    this.Borders?.Normal?.SetColor(DefaultBorderColorIsd.Dark);
                    this.Borders?.Hovered?.SetColor(DefaultHoveredBorderColorIsd.Dark);
                }
                else
                {
                    BackColor = DefaultBackColor.Dark;
                    ForeColor = DefaultForeColor.Dark;
                    this.Borders?.Normal?.SetColor(DefaultBorderColor.Dark);
                    this.Borders?.Hovered?.SetColor(DefaultHoveredBorderColor.Dark);
                }
            });
        }

        /// <summary>
        /// Sets colors used in the control to the light theme.
        /// </summary>
        public virtual void SetColorThemeToLight()
        {
            DoInsideUpdate(() =>
            {
                if (IsDefault)
                {
                    BackColor = DefaultBackColorIsd.Light;
                    ForeColor = DefaultForeColorIsd.Light;
                    this.Borders?.Normal?.SetColor(DefaultBorderColorIsd.Light);
                    this.Borders?.Hovered?.SetColor(DefaultHoveredBorderColorIsd.Light);
                }
                else
                {
                    BackColor = DefaultBackColor.Light;
                    ForeColor = DefaultForeColor.Light;
                    this.Borders?.Normal?.SetColor(DefaultBorderColor.Light);
                    this.Borders?.Hovered?.SetColor(DefaultHoveredBorderColor.Light);
                }
            });
        }

        /// <summary>
        /// Recalculates the alignment of text and image elements within the control based on the current settings.
        /// </summary>
        /// <remarks>Override this method in a derived class to implement custom alignment logic for text and images. </remarks>
        protected virtual void UpdateTextAndImageAlignment()
        {
            Item.SetContentAlignment(TextAlign, ImageAlign);
        }

        /// <summary>
        /// Updates the button image according to the current visual state and images specified.
        /// </summary>
        protected virtual void UpdateItemImage()
        {
            if (DisposingOrDisposed)
                return;
            var state = VisualState;

            Image? image = null;

            var svgImage = StateSvgImages.GetObjectOrNull(state);

            if (svgImage is not null)
            {
                var imageSize = SvgSize ?? PixelFromDip(16);

                if(IsEnabled)
                    image = svgImage.AsNormal(imageSize, IsDarkBackground)?.AsImage();
                else
                    image = svgImage.AsDisabled(imageSize, IsDarkBackground)?.AsImage();
            }

            image ??= StateImages.GetObjectOrNormal(state);

            Item.Image = image;
        }

        /// <inheritdoc/>
        protected override void OnSystemColorsChanged(EventArgs e)
        {
            if (AutoUpdateColors)
            {
                SetColorTheme();
            }

            base.OnSystemColorsChanged(e);
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