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
    public partial class StdButton : GenericItemControl, INotifyPropertyChanged, IDialogButtonRoles
    {
        /// <summary>
        /// Gets or sets default value of the <see cref="IsBoldWhenIsd"/> property. It indicates whether the
        /// text is displayed in bold when <see cref="IsDefault"/> property of the control is set to true.
        /// </summary>
        public static bool DefaultIsBoldWhenIsd = false;

        /// <summary>
        /// Represents the default padding value which is applied to <see cref="StdButton"/> controls in the constructor.
        /// </summary>
        /// <remarks>This static field provides a standard padding size that is applied to <see cref="StdButton"/> controls
        /// to ensure consistent spacing across components.</remarks>
        public static Thickness DefaultPadding = 4;

        /// <summary>
        /// Represents the default minimum size for the component, expressed as a SizeD structure.
        /// </summary>
        public static SizeD DefaultMinSize = 32;

        /// <summary>
        /// Specifies the default corner radius,
        /// in device-independent pixels or in percentage, used for rendering the corners of button elements.
        /// Use of percentage is determined by the value of <see cref="DefaultCornerRadiusIsPercent"/> field.
        /// If percentage is used, the actual corner radius will be calculated as a percentage of the control's size.
        /// </summary>
        public static float DefaultCornerRadius = 35;

        /// <summary>
        /// Indicates whether the default corner radius is specified as a percentage of the element's dimensions.
        /// </summary>
        public static bool DefaultCornerRadiusIsPercent = true;

        private bool isDefault;
        private bool isCancel;
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
            WantTab = true;
            IsGraphicControl = false;
            IsTransparent = false;
            ParentBackColor = false;
            ParentForeColor = false;

            UniformBorderCornerRadius = DefaultCornerRadius;
            UniformBorderRadiusIsPercent = DefaultCornerRadiusIsPercent;

            Borders?.SetAllExceptNormal(
                (state) => Borders.Normal?.Clone(),
                VisualControlStates.Hovered | VisualControlStates.Pressed | VisualControlStates.Disabled | VisualControlStates.Focused);

            MinimumSize = DefaultMinSize;
            Padding = DefaultPadding;
            Item.HorizontalAlignment = HorizontalAlignment.Center;

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
        /// Specifies options for applying a color theme, allowing control over which aspects of the theme are modified.
        /// </summary>
        /// <remarks>This enumeration supports a bitwise combination of its member values to enable
        /// multiple options when applying a color theme. The default value is Default, which applies no specific
        /// options.</remarks>
        [Flags]
        public enum ColorThemeApplyOptions
        {
            /// <summary>
            /// No specific options are applied when this option is used. This is the default value.
            /// </summary>
            None = 0,

            /// <summary>
            /// When this option is used, the background color of the control is updated to match the color theme being applied.
            /// </summary>
            BackColor = 1,

            /// <summary>
            /// When this option is used, the foreground color of the control is updated to match the color theme being applied.
            /// </summary>
            ForeColor = 2,

            /// <summary>
            /// When this option is used, the border color of the control is updated to match the color theme being applied.
            /// </summary>
            BorderColor = 4,

            /// <summary>
            /// All parts of the color theme are applied when this option is used.
            /// </summary>
            All = BackColor | ForeColor | BorderColor,
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.Button;

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

                if (value)
                {
                    Item.IsBold = RealFont.IsBold || (IsBoldWhenIsd ?? DefaultIsBoldWhenIsd);
                }
                else
                {
                    Item.IsBold = RealFont.IsBold;
                    Item.Border = null;
                }

                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets whether the text is displayed in bold when <see cref="IsDefault"/> property of the control is set to true.
        /// If not set, the value of <see cref="DefaultIsBoldWhenIsd"/> static field is used. This property allows you to specify
        /// whether the button's text should be rendered in bold font style when the button is marked as the default action in a dialog or form.
        /// </summary>
        [Browsable(false)]
        public virtual bool? IsBoldWhenIsd { get; set; }

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
                value ??= GetEffectiveBackColor();
                base.BackgroundColor = value;
            }
        }

        /// <inheritdoc/>
        public override Color? ForegroundColor
        {
            get => base.ForegroundColor;
            set
            {
                if (DisposingOrDisposed)
                    return;
                if (ForegroundColor == value)
                    return;
                value ??= GetEffectiveForeColor();
                base.ForegroundColor = value;
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

        /// <inheritdoc/>
        public override void DrawBorderAndBackground(
            PaintEventArgs e,
            DrawDefaultBackgroundFlags flags = DrawDefaultBackgroundFlags.DrawBorderAndBackground)
        {
            base.DrawBorderAndBackground(e, flags);
        }

        /// <inheritdoc/>
        public override void DefaultPaint(PaintEventArgs e)
        {
            base.DefaultPaint(e);
        }

        /// <inheritdoc/>
        public override BorderSettings? GetBorderSettings(VisualControlState state)
        {
            return Borders?.GetObjectOrNormal(state);
        }

        /// <inheritdoc/>
        protected override bool IsDarkSvgImageRequired(VisualControlState state)
        {
            if (IsDefault)
            {
                var result = GetBackgroundColor(state)?.IsDark();
                result ??= IsDarkBackground;
                return result.Value;
            }

            return IsDarkBackground;
        }

        /// <inheritdoc/>
        protected override void UpdateItemImage()
        {
            base.UpdateItemImage();
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
        protected override bool GetDefaultHasBorder()
        {
            return true;
        }
    }
}