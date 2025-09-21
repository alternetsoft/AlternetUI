using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Implements speed button control.
    /// </summary>
    [ControlCategory("MenusAndToolbars")]
    public partial class SpeedButton : GraphicControl, ICommandSource
    {
        /// <summary>
        /// Represents the default border radius used for rounding corners.
        /// This value is used in the <see cref="KnownTheme.RoundBorder"/> theme.
        /// </summary>
        public static Coord DefaultRoundBorderRadius = 25;

        /// <summary>
        /// Indicates whether the default round border radius is expressed as a percentage.
        /// This value is used in the <see cref="KnownTheme.RoundBorder"/> theme.
        /// </summary>
        public static bool DefaultRoundBorderRadiusIsPercent = true;

        /// <summary>
        /// Gets or sets default click repeat delay used when
        /// <see cref="SpeedButton.IsClickRepeated"/> is True.
        /// </summary>
        /// <remarks>
        /// The default value of delay is 50 milliseconds. This means
        /// the first event is initiated after 250 milliseconds (5 times the specified value).
        /// Each subsequent event is initiated after 50 milliseconds delay.
        /// </remarks>
        public static int DefaultClickRepeatDelay = 50;

        /// <summary>
        /// Gets or sets default image and label distance in the <see cref="SpeedButton"/>.
        /// </summary>
        public static Coord DefaultImageLabelDistance = 4;

        /// <summary>
        /// Gets or sets default padding in the <see cref="SpeedButton"/>.
        /// </summary>
        public static Coord DefaultPadding = 4;

        /// <summary>
        /// Gets or sets default border width of the <see cref="SpeedButton"/> in the sticky state.
        /// </summary>
        public static Coord DefaultStickyBorderWidth = 1;

        /// <summary>
        /// Gets ot sets default value of <see cref="UseTheme"/> property.
        /// </summary>
        public static KnownTheme DefaultUseTheme = KnownTheme.Default;

        /// <summary>
        /// Gets ot sets default value of <see cref="UseThemeForSticky"/> property.
        /// </summary>
        public static KnownTheme DefaultUseThemeForSticky = KnownTheme.StickyBorder;

        /// <summary>
        /// Gets or sets default color and style settings
        /// for all <see cref="SpeedButton"/> controls
        /// which have <see cref="UseTheme"/> equal to <see cref="KnownTheme.Default"/>.
        /// </summary>
        public static ControlColorAndStyle DefaultTheme = new();

        /// <summary>
        /// Gets or sets default color and style settings
        /// for all <see cref="SpeedButton"/> controls
        /// which have <see cref="UseTheme"/> equal to <see cref="KnownTheme.StaticBorder"/>.
        /// </summary>
        public static ControlColorAndStyle StaticBorderTheme;

        /// <summary>
        /// Gets or sets default color and style settings
        /// for all <see cref="SpeedButton"/> controls
        /// which have <see cref="UseTheme"/> equal to <see cref="KnownTheme.RoundBorder"/>.
        /// </summary>
        public static ControlColorAndStyle RoundBorderTheme;

        /// <summary>
        /// Gets or sets default color and style settings
        /// for all <see cref="SpeedButton"/> controls
        /// which have <see cref="UseTheme"/> equal to <see cref="KnownTheme.StickyBorder"/>.
        /// </summary>
        public static ControlColorAndStyle StickyBorderTheme;

        /// <summary>
        /// Gets or sets default color and style settings
        /// for all <see cref="SpeedButton"/> controls
        /// which have <see cref="UseTheme"/> equal to <see cref="KnownTheme.CheckBorder"/>.
        /// </summary>
        public static ControlColorAndStyle CheckBorderTheme;

        /// <summary>
        /// Gets or sets default color and style settings
        /// for all <see cref="SpeedButton"/> controls
        /// which have <see cref="UseTheme"/> equal to <see cref="KnownTheme.NoBorder"/>.
        /// </summary>
        public static ControlColorAndStyle NoBorderTheme = new();

        /// <summary>
        /// Gets or sets default color and style settings
        /// for all <see cref="SpeedButton"/> controls
        /// which have <see cref="UseTheme"/> equal to <see cref="KnownTheme.PushButton"/>.
        /// </summary>
        public static ControlColorAndStyle PushButtonTheme = new();

        /// <summary>
        /// Gets or sets default color and style settings
        /// for all <see cref="SpeedButton"/> controls
        /// which have <see cref="UseTheme"/> equal to <see cref="KnownTheme.PushButtonHovered"/>.
        /// </summary>
        public static ControlColorAndStyle PushButtonHoveredTheme = new();

        /// <summary>
        /// Gets or sets default color and style settings
        /// for all <see cref="SpeedButton"/> controls
        /// which have <see cref="UseTheme"/> equal to <see cref="KnownTheme.PushButtonPressed"/>.
        /// </summary>
        public static ControlColorAndStyle PushButtonPressedTheme = new();

        /// <summary>
        /// Gets or sets default color and style settings
        /// for all <see cref="SpeedButton"/> controls
        /// which have <see cref="UseTheme"/> equal to <see cref="KnownTheme.Custom"/>.
        /// </summary>
        public static ControlColorAndStyle? DefaultCustomTheme = null;

        /// <summary>
        /// Gets or sets default color and style settings
        /// for all <see cref="SpeedButton"/> controls
        /// which have <see cref="UseTheme"/> equal to <see cref="KnownTheme.TabControl"/>.
        /// </summary>
        public static ControlColorAndStyle? TabControlTheme = DefaultTheme;

        private readonly Spacer pictureSpacer = new()
        {
            Visible = false,
            Alignment = HVAlignment.Center,
        };

        private readonly Spacer spacer = new()
        {
            SuggestedSize = DefaultImageLabelDistance,
            Visible = false,
            Alignment = HVAlignment.Center,
        };

        private readonly Label label = new()
        {
            Visible = false,
            Alignment = HVAlignment.Center,
        };

        private readonly ImageDrawable drawable = new();

        private bool sticky;
        private bool stickyToggleOnClick;
        private bool textVisible = false;
        private bool imageVisible = true;
        private bool isClickRepeated;
        private bool subscribedClickRepeated;

        private Action? clickAction;
        private ShortcutInfo? shortcut;
        private KnownTheme useTheme = DefaultUseTheme;
        private KnownTheme useThemeForSticky = DefaultUseThemeForSticky;
        private ControlColorAndStyle? customTheme;
        private VisualControlState stickyVisualState = VisualControlState.Normal;
        private Color? borderColor;
        private Timer? repeatTimer;
        private Timer? firstClickTimer;
        private int clickRepeatDelay = DefaultClickRepeatDelay;
        private CommandSourceStruct commandSource;
        private ImageToText imageToText = ImageToText.Horizontal;
        private CheckedSpreadMode stickySpreadMode;
        private RightSideElementKind rightSideElementKind;
        private bool isToolTipEnabled = true;
        private Coord minRightSideWidth;

        static SpeedButton()
        {
            var borderColor = DefaultColors.BorderColor;

            InitThemeLight(DefaultTheme.Light);
            InitThemeDark(DefaultTheme.Dark);

            StaticBorderTheme = DefaultTheme.Clone();
            StaticBorderTheme.NormalBorderAsHovered();
            StaticBorderTheme.DisabledBorderAsHovered();
            StaticBorderTheme.SetBorderColor(borderColor);

            RoundBorderTheme = StaticBorderTheme.Clone();

            RoundBorderTheme.SetCornerRadius(
                DefaultRoundBorderRadius,
                DefaultRoundBorderRadiusIsPercent);

            StickyBorderTheme = CreateBordered(borderColor);
            CheckBorderTheme = CreateBordered(DefaultColors.DefaultCheckBoxColor);

            ControlColorAndStyle CreateBordered(LightDarkColor? color)
            {
                var result = DefaultTheme.Clone();
                result
                    .SetBorderFromBorder(
                    stateToChange: VisualControlState.Normal,
                    assignFromState: VisualControlState.Hovered);
                result.SetBorderWidth(DefaultStickyBorderWidth);
                result.SetBorderColor(color);
                return result;
            }

            PushButtonTheme = CreatePushButtonTheme(null);
            PushButtonHoveredTheme = CreatePushButtonTheme(DrawingUtils.DrawPushButtonHovered);
            PushButtonPressedTheme = CreatePushButtonTheme(DrawingUtils.DrawPushButtonPressed);

            ControlColorAndStyle CreatePushButtonTheme(PaintEventHandler? normalStateOverride)
            {
                var result = DefaultTheme.Clone();
                result.SetAsPushButton();
                if (normalStateOverride is null)
                    return result;
                result.Light.BackgroundActions!.Normal = normalStateOverride;
                result.Dark.BackgroundActions!.Normal = normalStateOverride;
                return result;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeedButton"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public SpeedButton(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeedButton"/> class.
        /// </summary>
        public SpeedButton()
        {
            commandSource = new(this);

            ParentBackColor = true;
            ParentForeColor = true;
            Padding = DefaultPadding;
            Layout = LayoutStyle.Horizontal;

            IsGraphicControl = true;
            RefreshOptions = ControlRefreshOptions.RefreshOnState;

            commandSource.Changed = () =>
            {
                Enabled = commandSource.CanExecute;
            };

            drawable.CenterHorz = true;
            drawable.CenterVert = true;
            drawable.Stretch = false;

            label.BeforeDrawText += OnBeforeDrawLabel;
        }

        /// <summary>
        /// Occurs when <see cref="Sticky"/> property is changed.
        /// </summary>
        public event EventHandler? StickyChanged;

        /// <summary>
        /// Specifies the kind of text displayed on the right side of the control.
        /// </summary>
        public enum RightSideElementKind
        {
            /// <summary>
            /// No right-side text is displayed.
            /// </summary>
            None,

            /// <summary>
            /// A key gesture (such as a keyboard shortcut) is displayed on the right side.
            /// </summary>
            KeyGesture,

            /// <summary>
            /// A formatted key gesture (such as a keyboard shortcut) is displayed on the right side.
            /// </summary>
            KeyGestureFormatted,

            /// <summary>
            /// A value of <see cref="RightSideText"/> property is displayed on the right side.
            /// </summary>
            Text,

            /// <summary>
            /// A value of <see cref="RightSideImage"/> property is displayed on the right side.
            /// </summary>
            Image,
        }

        /// <summary>
        /// Enumerates known color and style themes for the <see cref="SpeedButton"/>.
        /// </summary>
        public enum KnownTheme
        {
            /// <summary>
            /// An empty theme. Settings from <see cref="AbstractControl.StateObjects"/> are used.
            /// </summary>
            None,

            /// <summary>
            /// Theme <see cref="DefaultTheme"/> is used.
            /// </summary>
            Default,

            /// <summary>
            /// Theme <see cref="CustomTheme"/> is used.
            /// </summary>
            Custom,

            /// <summary>
            /// Theme <see cref="TabControlTheme"/> is used.
            /// </summary>
            TabControl,

            /// <summary>
            /// Theme <see cref="StaticBorderTheme"/> is used.
            /// </summary>
            StaticBorder,

            /// <summary>
            /// Theme <see cref="StickyBorderTheme"/> is used.
            /// </summary>
            StickyBorder,

            /// <summary>
            /// Theme <see cref="NoBorderTheme"/> is used.
            /// </summary>
            NoBorder,

            /// <summary>
            /// Theme <see cref="CheckBorderTheme"/> is used.
            /// </summary>
            CheckBorder,

            /// <summary>
            /// Theme <see cref="PushButtonTheme"/> is used.
            /// </summary>
            PushButton,

            /// <summary>
            /// Theme <see cref="PushButtonHoveredTheme"/> is used.
            /// </summary>
            PushButtonHovered,

            /// <summary>
            /// Theme <see cref="PushButtonPressedTheme"/> is used.
            /// </summary>
            PushButtonPressed,

            /// <summary>
            /// Theme <see cref="RoundBorderTheme"/> is used.
            /// </summary>
            RoundBorder,
        }

        /// <summary>
        /// Gets or sets default template for the shortcut when it is shown in the tooltip.
        /// </summary>
        /// <remarks>
        /// Default value is "({0})".
        /// </remarks>
        public static string DefaultShortcutToolTipTemplate { get; set; } = "({0})";

        /// <summary>
        /// Gets or sets a value indicating whether the drop-down menu
        /// is displayed when the user hovers over the associated control.
        /// </summary>
        public virtual bool ShowDropDownMenuWhenHovered { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether tooltip is shown
        /// when mouse hovers over the control.
        /// </summary>
        public virtual bool IsToolTipEnabled
        {
            get => isToolTipEnabled;

            set
            {
                if (isToolTipEnabled == value)
                    return;
                isToolTipEnabled = value;
                UpdateToolTip();
            }
        }

        /// <summary>
        /// Gets or sets distance between image and label.
        /// </summary>
        public virtual Coord ImageLabelDistance
        {
            get
            {
                return spacer.SuggestedWidth;
            }

            set
            {
                if (ImageLabelDistance == value)
                    return;
                spacer.SuggestedSize = value;
                if (HasVisibleImage && TextVisible)
                    PerformLayoutAndInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the text should be rendered vertically.
        /// </summary>
        /// <remarks>
        /// When this property is set, the layout is refreshed to reflect
        /// the vertical text orientation.
        /// </remarks>
        public virtual bool IsVerticalText
        {
            get => Label.IsVerticalText;
            set
            {
                if (IsVerticalText == value)
                    return;
                DoInsideLayout(() =>
                {
                    Label.IsVerticalText = value;
                });
            }
        }

        /// <summary>
        /// Gets or sets the text displayed on the right side of the control.
        /// </summary>
        public virtual object? RightSideText { get; set; }

        /// <summary>
        /// Gets or sets the image displayed on the right side of the control.
        /// </summary>
        [Browsable(false)]
        public virtual Image? RightSideImage { get; set; }

        /// <summary>
        /// Gets or sets the image displayed on the right side of the control when it is disabled.
        /// </summary>
        [Browsable(false)]
        public virtual Image? RightSideDisabledImage { get; set; }

        /// <summary>
        /// Gets or sets the type of element displayed on the right side of the control.
        /// </summary>
        /// <remarks>Changing this property triggers a layout update and invalidates the control for
        /// redrawing.</remarks>
        public virtual RightSideElementKind RightSideElement
        {
            get
            {
                return rightSideElementKind;
            }

            set
            {
                if (rightSideElementKind == value)
                    return;
                rightSideElementKind = value;
                PerformLayoutAndInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets the mode that defines how the <see cref="Sticky"/> state
        /// is propagated to sibling <see cref="SpeedButton"/> controls
        /// when this control becomes sticky.
        /// </summary>
        /// <remarks>
        /// This property determines whether other <see cref="SpeedButton"/> controls
        /// in the same group or hierarchy
        /// should be influenced when this control becomes sticky
        /// (<c>IsSticky</c> is <c>true</c>).
        /// </remarks>
        public virtual CheckedSpreadMode StickySpreadMode
        {
            get => stickySpreadMode;
            set
            {
                if (stickySpreadMode == value)
                    return;
                stickySpreadMode = value;
                UpdateSiblingStickyState();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the image should be stretched
        /// to fill its container. Default is <c>false</c>.
        /// </summary>
        /// <remarks>When set to <see langword="true"/>, the image will scale to fit
        /// the dimensions of its
        /// container, potentially altering its aspect ratio. When set to <see langword="false"/>,
        /// the image will retain  its original size and aspect ratio.</remarks>
        public virtual bool ImageStretch
        {
            get
            {
                return drawable.Stretch;
            }

            set
            {
                if (drawable.Stretch == value)
                    return;
                drawable.Stretch = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the label has an associated image.
        /// </summary>
        [Browsable(false)]
        public virtual bool HasLabelImage
        {
            get
            {
                return label.HasVisibleImage;
            }
        }

        /// <summary>
        /// Gets or sets the minimum width of the right side element in dips.
        /// </summary>
        public virtual Coord MinRightSideWidth
        {
            get => minRightSideWidth;
            set
            {
                if (minRightSideWidth == value)
                    return;
                minRightSideWidth = value;
                if (RightSideElement != RightSideElementKind.None)
                    PerformLayoutAndInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets horizontal alignment of the image.
        /// </summary>
        public virtual HorizontalAlignment ImageHorizontalAlignment
        {
            get
            {
                return pictureSpacer.HorizontalAlignment;
            }

            set
            {
                if (ImageHorizontalAlignment == value)
                    return;
                pictureSpacer.HorizontalAlignment = value;
                PerformLayoutAndInvalidate(null, false);
            }
        }

        /// <summary>
        /// Gets or sets vertical alignment of the image.
        /// </summary>
        public virtual VerticalAlignment ImageVerticalAlignment
        {
            get
            {
                return pictureSpacer.VerticalAlignment;
            }

            set
            {
                if (ImageVerticalAlignment == value)
                    return;
                pictureSpacer.VerticalAlignment = value;
                PerformLayoutAndInvalidate(null, false);
            }
        }

        /// <summary>
        /// Gets or sets horizontal alignment of the spacer.
        /// </summary>
        public virtual HorizontalAlignment SpacerHorizontalAlignment
        {
            get
            {
                return spacer.HorizontalAlignment;
            }

            set
            {
                if (SpacerHorizontalAlignment == value)
                    return;
                spacer.HorizontalAlignment = value;
                PerformLayoutAndInvalidate(null, false);
            }
        }

        /// <summary>
        /// Gets or sets vertical alignment of the spacer.
        /// </summary>
        public virtual VerticalAlignment SpacerVerticalAlignment
        {
            get
            {
                return spacer.VerticalAlignment;
            }

            set
            {
                if (SpacerVerticalAlignment == value)
                    return;
                spacer.VerticalAlignment = value;
                PerformLayoutAndInvalidate(null, false);
            }
        }

        /// <summary>
        /// Gets or sets horizontal alignment of the label.
        /// </summary>
        public virtual HorizontalAlignment LabelHorizontalAlignment
        {
            get
            {
                return label.HorizontalAlignment;
            }

            set
            {
                if (LabelHorizontalAlignment == value)
                    return;
                label.HorizontalAlignment = value;
                PerformLayoutAndInvalidate(null, false);
            }
        }

        /// <summary>
        /// Gets or sets vertical alignment of the label.
        /// </summary>
        public virtual VerticalAlignment LabelVerticalAlignment
        {
            get
            {
                return label.VerticalAlignment;
            }

            set
            {
                if (LabelVerticalAlignment == value)
                    return;
                label.VerticalAlignment = value;
                PerformLayoutAndInvalidate(null, false);
            }
        }

        /// <summary>
        /// Gets or sets click repeat delay used when
        /// <see cref="SpeedButton.IsClickRepeated"/> is True.
        /// </summary>
        /// <remarks>
        /// The default value of delay is 50 milliseconds. This means
        /// the first event is initiated after 250 milliseconds (5 times the specified value).
        /// Each subsequent event is initiated after 50 milliseconds delay.
        /// </remarks>
        public virtual int ClickRepeatDelay
        {
            get
            {
                return clickRepeatDelay;
            }

            set
            {
                if (clickRepeatDelay == value)
                    return;
                clickRepeatDelay = value;
            }
        }

        /// <summary>
        /// Gets or sets whether mouse clicks are repeated continuously while left mouse
        /// button is pressed.
        /// </summary>
        public virtual bool IsClickRepeated
        {
            get
            {
                return isClickRepeated;
            }

            set
            {
                if (isClickRepeated == value)
                    return;
                isClickRepeated = value;
            }
        }

        /// <inheritdoc/>
        public override Font? Font
        {
            get => base.Font;
            set => base.Font = value;
        }

        /// <inheritdoc/>
        public override bool IsBold
        {
            get => base.IsBold;
            set => base.IsBold = value;
        }

        /// <summary>
        /// Gets or sets border color override. Currently is not implemented.
        /// Added for the compatibility with legacy code.
        /// </summary>
        [Browsable(false)]
        public virtual Color? BorderColor
        {
            get
            {
                return borderColor;
            }

            set
            {
                if (borderColor == value)
                    return;
                borderColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets default color and style settings
        /// for all <see cref="SpeedButton"/> controls
        /// which have <see cref="UseTheme"/> equal to <see cref="KnownTheme.Custom"/>.
        /// </summary>
        [Browsable(false)]
        public virtual ControlColorAndStyle? CustomTheme
        {
            get
            {
                return customTheme;
            }

            set
            {
                if (customTheme == value)
                    return;
                customTheme = value;
                if (UseTheme == KnownTheme.Custom)
                    Invalidate();
            }
        }

        /// <inheritdoc/>
        public override bool Enabled
        {
            get => base.Enabled;
            set
            {
                base.Enabled = value;
                PictureBox.Enabled = value;
                Label.Enabled = value;
            }
        }

        /// <inheritdoc/>
        public virtual ICommand? Command
        {
            get
            {
                return commandSource.Command;
            }

            set
            {
                commandSource.Command = value;
            }
        }

        /// <inheritdoc/>
        public virtual object? CommandParameter
        {
            get
            {
                return commandSource.CommandParameter;
            }

            set
            {
                commandSource.CommandParameter = value;
            }
        }

        /// <inheritdoc/>
        [Browsable(false)]
        public virtual object? CommandTarget
        {
            get
            {
                return commandSource.CommandParameter;
            }

            set
            {
                commandSource.CommandParameter = value;
            }
        }

        /// <summary>
        /// Gets or sets the associated shortcut keys.
        /// </summary>
        /// <returns>
        /// One of the <see cref="Keys" /> values.
        /// The default is <see cref="Keys.None" />.</returns>
        [Localizable(true)]
        [DefaultValue(Keys.None)]
        [Browsable(false)]
        public virtual Keys ShortcutKeys
        {
            get
            {
                return shortcut;
            }

            set
            {
                if (ShortcutKeys == value)
                    return;
                shortcut = value;
                UpdateToolTip();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the associated shortcut key.
        /// </summary>
        [Browsable(false)]
        public virtual KeyGesture? Shortcut
        {
            get
            {
                return shortcut;
            }

            set
            {
                if (Shortcut == value)
                    return;
                shortcut = value;
                UpdateToolTip();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the associated shortcut key.
        /// </summary>
        [Browsable(false)]
        public virtual ShortcutInfo? ShortcutInfo
        {
            get
            {
                return shortcut;
            }

            set
            {
                if (shortcut == value)
                    return;
                shortcut = value;
                UpdateToolTip();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the associated shortcut key.
        /// </summary>
        [Browsable(false)]
        public virtual KeyInfo[]? ShortcutKeyInfo
        {
            get
            {
                return shortcut;
            }

            set
            {
                shortcut = value;
                UpdateToolTip();
            }
        }

        /// <summary>
        /// Gets or sets whether <see cref="AbstractControl.ToolTip"/> will be hidden
        /// when control is clicked. Default is <c>true</c>.
        /// </summary>
        public virtual bool HideToolTipOnClick { get; set; } = true;

        /// <summary>
        /// Gets or sets whether to use <see cref="DefaultTheme"/>.
        /// </summary>
        [Browsable(false)]
        public virtual bool UseDefaultTheme
        {
            get => UseTheme == KnownTheme.Default;
            set
            {
                if (UseDefaultTheme == value)
                    return;
                if (value)
                    UseTheme = KnownTheme.Default;
                else
                    UseTheme = KnownTheme.None;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets color theme when <see cref="Sticky"/> is True.
        /// </summary>
        public virtual KnownTheme UseThemeForSticky
        {
            get => useThemeForSticky;
            set
            {
                if (useThemeForSticky == value)
                    return;
                useThemeForSticky = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets whether to use <see cref="DefaultTheme"/>.
        /// </summary>
        public virtual KnownTheme UseTheme
        {
            get => useTheme;
            set
            {
                if (useTheme == value)
                    return;
                useTheme = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets whether to display text in the control.
        /// </summary>
        public virtual bool TextVisible
        {
            get
            {
                return textVisible;
            }

            set
            {
                if (textVisible == value)
                    return;
                textVisible = value;
                PerformLayoutAndInvalidate(OnImageToTextChanged);
            }
        }

        /// <summary>
        /// Gets a value indicating whether <see cref="Image"/> or <see cref="ImageSet"/>
        /// is associated with the current instance.
        /// </summary>
        [Browsable(false)]
        public virtual bool HasImage
        {
            get
            {
                return (Image is not null) || (ImageSet is not null);
            }
        }

        /// <summary>
        /// Gets whether control has image and it is visible.
        /// </summary>
        [Browsable(false)]
        public virtual bool HasVisibleImage
        {
            get
            {
                return ImageVisible && HasImage;
            }
        }

        /// <inheritdoc/>
        public override IReadOnlyList<AbstractControl> AllChildrenInLayout
        {
            get
            {
                if (TextVisible)
                {
                    if (HasVisibleImage)
                    {
                        if (ImageToText == ImageToText.Horizontal)
                        {
                            if (PictureBox.HorizontalAlignment == HorizontalAlignment.Right)
                            {
                                return new AbstractControl[] { Label, spacer, PictureBox };
                            }
                            else
                            {
                                return new AbstractControl[] { PictureBox, spacer, Label };
                            }
                        }
                        else
                        {
                            if (PictureBox.VerticalAlignment == VerticalAlignment.Bottom)
                            {
                                return new AbstractControl[] { Label, spacer, PictureBox };
                            }
                            else
                            {
                                return new AbstractControl[] { PictureBox, spacer, Label };
                            }
                        }
                    }
                    else
                        return new AbstractControl[] { Label };
                }
                else
                {
                    if (HasVisibleImage)
                        return new AbstractControl[] { PictureBox };
                    else
                        return Array.Empty<AbstractControl>();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to draw image.
        /// </summary>
        public virtual bool ImageVisible
        {
            get
            {
                return imageVisible;
            }

            set
            {
                if (imageVisible == value)
                    return;
                imageVisible = value;
                PerformLayoutAndInvalidate(OnImageToTextChanged);
            }
        }

        /// <summary>
        /// Gets or sets the layout relationship between the image and text content
        /// within the control.
        /// </summary>
        /// <remarks>
        /// This property defines how text is aligned relative to the associated image
        /// (vertical or horizontal alignment).
        /// </remarks>
        public virtual ImageToText ImageToText
        {
            get
            {
                return imageToText;
            }

            set
            {
                if (value == imageToText)
                    return;
                imageToText = value;
                OnImageToTextChanged();
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets template for the shortcut when it is shown in the tooltip.
        /// </summary>
        /// <remarks>
        /// When this property is null (default), <see cref="DefaultShortcutToolTipTemplate"/>
        /// is used to get the template.
        /// </remarks>
        [Browsable(false)]
        public virtual string? ShortcutToolTipTemplate { get; set; }

        /// <summary>
        /// Gets or sets whether control is sticky.
        /// </summary>
        /// <remarks>
        /// When this property is true, control painted as pressed if it is not disabled.
        /// </remarks>
        public virtual bool Sticky
        {
            get
            {
                return sticky;
            }

            set
            {
                if (sticky == value)
                    return;
                sticky = value;
                Invalidate();
                UpdateSiblingStickyState();
                RaiseStickyChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets whether <see cref="Sticky"/> is toggled
        /// when control is clicked.
        /// </summary>
        public virtual bool StickyToggleOnClick
        {
            get
            {
                return stickyToggleOnClick;
            }

            set
            {
                if (stickyToggleOnClick == value)
                    return;
                stickyToggleOnClick = value;
            }
        }

        /// <summary>
        /// Gets or sets the image that is displayed by the control's label.
        /// This is an additional image that can be displayed
        /// alongside the main <see cref="Image"/>.
        /// </summary>
        public virtual Image? LabelImage
        {
            get
            {
                return Label.Image;
            }

            set
            {
                if (LabelImage == value)
                    return;
                PerformLayoutAndInvalidate(() =>
                {
                    Label.Image = value;
                });
            }
        }

        /// <summary>
        /// Gets or sets the image that is displayed by the control.
        /// </summary>
        [DefaultValue(null)]
        public virtual Image? Image
        {
            get
            {
                return drawable.Image;
            }

            set
            {
                PerformLayoutAndInvalidate(() =>
                {
                    drawable.Image = value;
                    PictureSizeChanged();
                });
            }
        }

        /// <inheritdoc/>
        [DefaultValue("")]
        public override string Text
        {
            get
            {
                return base.Text;
            }

            set
            {
                if (base.Text == value)
                    return;
                base.Text = value;
                Label.Text = value;
                if (TextVisible)
                    PerformLayoutAndInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets the disabled image that is displayed by the control.
        /// </summary>
        public virtual Image? DisabledImage
        {
            get
            {
                return drawable.DisabledImage;
            }

            set
            {
                PerformLayoutAndInvalidate(() =>
                {
                    drawable.DisabledImage = value;
                    PictureSizeChanged();
                });
            }
        }

        /// <summary>
        /// Gets or sets the disabled <see cref="ImageSet"/> that is displayed by the control.
        /// </summary>
        [Browsable(false)]
        public virtual ImageSet? DisabledImageSet
        {
            get
            {
                return drawable.DisabledImageSet;
            }

            set
            {
                PerformLayoutAndInvalidate(() =>
                {
                    drawable.DisabledImageSet = value;
                    PictureSizeChanged();
                });
            }
        }

        /// <summary>
        /// Gets or sets <see cref="ImageSet"/> that is displayed by the control.
        /// </summary>
        [Browsable(false)]
        public virtual ImageSet? ImageSet
        {
            get
            {
                return drawable.ImageSet;
            }

            set
            {
                PerformLayoutAndInvalidate(() =>
                {
                    drawable.ImageSet = value;
                    PictureSizeChanged();
                });
            }
        }

        /// <summary>
        /// Gets or sets override for <see cref="VisualState"/>
        /// when <see cref="Sticky"/> is True.
        /// </summary>
        [Browsable(false)]
        public virtual VisualControlState StickyVisualStateOverride
        {
            get => stickyVisualState;
            set
            {
                if (stickyVisualState == value)
                    return;
                stickyVisualState = value;
                Invalidate();
            }
        }

        /// <inheritdoc/>
        [Browsable(false)]
        public override VisualControlState VisualState
        {
            get
            {
                var result = base.VisualState;
                if (sticky)
                {
                    if (result == VisualControlState.Normal
                        || result == VisualControlState.Focused)
                        result = StickyVisualStateOverride;
                }

                return result;
            }
        }

        /// <summary>
        /// Gets or sets <see cref="Action"/> which will be executed when
        /// this control is clicked by the user.
        /// </summary>
        [Browsable(false)]
        public virtual Action? ClickAction
        {
            get => clickAction;
            set
            {
                clickAction = value;
            }
        }

        /// <summary>
        /// Gets inner <see cref="Label"/> control.
        /// </summary>
        [Browsable(false)]
        public Label Label => label;

        internal new LayoutStyle? Layout
        {
            get => base.Layout;
            set => base.Layout = value;
        }

        [Browsable(false)]
        internal new string Title
        {
            get => base.Title;
            set => base.Title = value;
        }

        /// <summary>
        /// Gets inner <see cref="PictureBox"/> control.
        /// </summary>
        [Browsable(false)]
        internal Spacer PictureBox => pictureSpacer;

        /// <summary>
        /// Initializes default colors and styles for the <see cref="SpeedButton"/>
        /// using 'Light' color theme.
        /// </summary>
        /// <param name="theme"><see cref="ControlStateSettings"/> to initialize.</param>
        public static void InitThemeLight(ControlStateSettings theme)
        {
            AllStateColors colors = new()
            {
                HoveredForeColor = (0, 0, 0),
                HoveredBackColor = (229, 243, 255),

                DisabledForeColor = SystemColors.GrayText,

                PressedForeColor = (0, 0, 0),
                PressedBackColor = (204, 228, 247),
            };

            theme.Borders = CreateBorders((204, 232, 255));
            theme.Colors = colors.AllStates;
            theme.Backgrounds = theme.Colors;
        }

        /// <summary>
        /// Initializes default colors and styles for the <see cref="SpeedButton"/>
        /// using 'Dark' color theme.
        /// </summary>
        /// <param name="theme"><see cref="ControlStateSettings"/> to initialize.</param>
        public static void InitThemeDark(ControlStateSettings theme)
        {
            AllStateColors colors = new()
            {
                HoveredForeColor = (250, 250, 250),
                HoveredBackColor = (61, 61, 61),

                DisabledForeColor = SystemColors.GrayText,

                PressedForeColor = (214, 214, 214),
                PressedBackColor = (34, 34, 34),
            };

            theme.Borders = CreateBorders((112, 112, 112));
            theme.Colors = colors.AllStates;
            theme.Backgrounds = theme.Colors;
        }

        /// <summary>
        /// Creates borders for the <see cref="SpeedButton"/>
        /// using default border width
        /// and using specified <paramref name="color"/>.
        /// </summary>
        /// <param name="color">Border color.</param>
        /// <returns></returns>
        public static ControlStateBorders CreateBorders(Color color)
        {
            BorderSettings hoveredBorder = new()
            {
                Width = 1,
                Color = color,
            };

            ControlStateBorders borders = new();
            var pressedBorder = hoveredBorder.Clone();
            borders.SetObject(hoveredBorder, VisualControlState.Hovered);
            borders.SetObject(pressedBorder, VisualControlState.Pressed);
            return borders;
        }

        /// <inheritdoc/>
        public override void RaiseFontChanged(EventArgs e)
        {
            Label.Font = RealFont;
            base.RaiseFontChanged(e);
        }

        /// <inheritdoc/>
        public override object? GetRealToolTip()
        {
            if (!IsToolTipEnabled)
                return null;

            var s = ToolTip;

            if (string.IsNullOrWhiteSpace(s))
            {
                return null;
            }

            var shortcutText = GetShortcutText(true);

            if (shortcutText is not null)
            {
                s += " " + shortcutText;
            }

            return s;
        }

        /// <summary>
        /// Retrieves the text representation of the shortcut key,
        /// optionally formatted using a template.
        /// </summary>
        /// <remarks>The method filters the shortcut key information based
        /// on the backend operating system before generating the text representation.
        /// If multiple keys are available, only the first key is used.</remarks>
        /// <param name="useTemplate">A value indicating whether to format the shortcut key
        /// using a predefined template. If <see langword="true"/>, the shortcut key is
        /// formatted using the template specified by  <c>ShortcutToolTipTemplate</c>
        /// or a default template. If <see langword="false"/>, the raw string
        /// representation of the shortcut key is returned.</param>
        /// <param name="forUser">The <see langword="forUser"/> parameter determines whether
        /// the shortcut key is formatted for display to the user.</param>
        /// <returns>A string representing the shortcut key. If no shortcut key
        /// is available or the key is null, the method
        /// returns <see langword="null"/>.</returns>
        public virtual string? GetShortcutText(bool useTemplate, bool forUser = true)
        {
            if(shortcut is null)
                return null;

            ShortcutInfo.FormatOptions options = new()
            {
                UseTemplate = useTemplate,
                ForUser = forUser,
                Template = ShortcutToolTipTemplate ?? DefaultShortcutToolTipTemplate,
            };

            var result = shortcut.ToString(options);
            return result;
        }

        /// <summary>
        /// Resets the image and disabled image to null.
        /// </summary>
        public virtual void ResetImages()
        {
            SetImages(null, null, null, null);
        }

        /// <summary>
        /// Sets the images for the control.
        /// </summary>
        /// <param name="image">The normal image to display.</param>
        /// <param name="disabledImage">The image to display when disabled.</param>
        public void SetImages(Image? image, Image? disabledImage)
        {
            SetImages(image, disabledImage, null, null);
        }

        /// <summary>
        /// Sets the images for the control.
        /// </summary>
        /// <param name="image">The set of images to use.</param>
        /// <param name="disabledImage">The set of images to use when disabled.</param>
        public void SetImageSets(ImageSet? image, ImageSet? disabledImage)
        {
            SetImages(null, null, image, disabledImage);
        }

        /// <summary>
        /// Raises <see cref="StickyChanged"/> event and <see cref="OnStickyChanged"/> method.
        /// </summary>
        /// <param name="e"></param>
        public void RaiseStickyChanged(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnStickyChanged(e);
            StickyChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Sets the images for the control.
        /// </summary>
        /// <param name="image">The normal image to display.</param>
        /// <param name="disabledImage">The image to display when disabled.</param>
        /// <param name="imageSet">The set of images to use.</param>
        /// <param name="disabledImageSet">The set of images to use when disabled.</param>
        public virtual void SetImages(
            Image? image,
            Image? disabledImage,
            ImageSet? imageSet,
            ImageSet? disabledImageSet)
        {
            PerformLayoutAndInvalidate(() =>
            {
                drawable.Image = image;
                drawable.DisabledImage = disabledImage;
                drawable.ImageSet = imageSet;
                drawable.DisabledImageSet = disabledImageSet;
                PictureSizeChanged();
            });
        }

        /// <summary>
        /// Sets the default combo box image for the control.
        /// </summary>
        public virtual void SetDefaultComboBoxImage(int? size = null)
        {
            SetSvgImage(
                ControlAndButton.DefaultBtnComboBoxSvg,
                KnownButton.TextBoxCombo,
                size);
        }

        /// <summary>
        /// Sets the SVG image for the control.
        /// </summary>
        /// <param name="svg">The SVG image to be set. If null, known
        /// button image will be used.</param>
        /// <param name="btn">The known button type. If null, svg image should be specified.</param>
        /// <param name="size">The optional size for the image.
        /// If not specified, the default size for the SVG image would be used.</param>
        public virtual void SetSvgImage(
            SvgImage? svg,
            KnownButton? btn,
            int? size = null)
        {
            var (normalImage, disabledImage)
                = ToolBarUtils.GetNormalAndDisabledSvg(svg, btn, this, size);

            SetImageSets(normalImage, disabledImage);
        }

        /// <inheritdoc/>
        public override SizeD GetPreferredSize(SizeD availableSize)
        {
            return base.GetPreferredSize(availableSize);
        }

        /// <summary>
        /// Loads normal and disabled image from the specified file or resource url.
        /// Loaded images assigned to <see cref="ImageSet"/> and
        /// <see cref="DisabledImageSet"/> properties.
        /// </summary>
        /// <param name="url">The file or embedded resource url with Svg data used
        /// to load the image.
        /// </param>
        /// <param name="imageSize">Image size in pixels.</param>
        /// <remarks>
        /// <paramref name="url"/> can include assembly name. Example:
        /// "embres:Alternet.UI.Resources.Svg.ImageName.svg?assembly=Alternet.UI"
        /// </remarks>
        /// <remarks>
        /// This method updates Svg default fill colors using
        /// <see cref="AbstractControl.GetSvgColor"/>.
        /// If you need to load Svg without updating its colors, use
        /// <see cref="ImageSet.FromSvgUrl(string, int, int, Color?)"/> without
        /// defining the last parameter.
        /// </remarks>
        public virtual void LoadSvg(string url, SizeI imageSize)
        {
            var (normalImage, disabledImage) =
                ToolBarUtils.GetNormalAndDisabledSvg(url, this, imageSize);
            ImageSet = normalImage;
            DisabledImageSet = disabledImage;
        }

        /// <summary>
        /// Sets the label's image using the specified SVG image, button type, alignment, and size.
        /// </summary>
        /// <remarks>This method updates the label's image and alignment properties.
        /// The image is aligned
        /// to the right horizontally and centered vertically by default.</remarks>
        /// <param name="svg">The SVG image to be displayed on the label.
        /// Can be <see langword="null"/> to remove the current image.</param>
        /// <param name="btn">The button type that determines the style or context of the image.
        /// Can be <see langword="null"/> for default behavior.</param>
        /// <param name="align">The horizontal and vertical alignment of the image
        /// relative to the label. Defaults to <see langword="null"/>, which uses the
        /// label's default alignment.</param>
        /// <param name="size">The size of the image in pixels.
        /// Can be <see langword="null"/> to use the default size.</param>
        public virtual void SetLabelImage(
            SvgImage? svg,
            KnownButton? btn = null,
            HVAlignment? align = null,
            CoordAndUnit? size = null)
        {
            DoInsideLayout(() =>
            {
                Label.ImageHorizontalAlignment = align?.Horizontal ?? HorizontalAlignment.Right;
                Label.ImageVerticalAlignment = align?.Vertical ?? VerticalAlignment.Center;
                Label.SetSvgImage(svg, btn, size);
            });
        }

        /// <summary>
        /// Sets the right-side image for the toolbar button using
        /// the specified SVG image and optional parameters.
        /// </summary>
        public virtual void SetRightSideImage(KnownButtonImage knownButtonAndImage)
        {
            var (normalImage, disabledImage) = ToolBarUtils.GetNormalAndDisabledSvgImages(
                    knownButtonAndImage.SvgImage,
                    knownButtonAndImage.KnownButton,
                    this,
                    knownButtonAndImage.Size);

            RightSideImage = normalImage;
            RightSideDisabledImage = disabledImage;
        }

        /// <summary>
        /// Sets the label image for a button using the specified predefined image,
        /// button type, alignment, and size.
        /// </summary>
        /// <remarks>This method simplifies the process of setting a label image by encapsulating the
        /// required parameters in a single object.</remarks>
        /// <param name="knownButtonAndImage">An object containing the image parameters.</param>
        public virtual void SetLabelImage(KnownButtonImage knownButtonAndImage)
        {
            SetLabelImage(
                knownButtonAndImage.SvgImage,
                knownButtonAndImage.KnownButton,
                knownButtonAndImage.Alignment,
                knownButtonAndImage.Size);
        }

        /// <summary>
        /// Sets the SVG image for a toolbar button, along with its corresponding disabled state image.
        /// </summary>
        /// <remarks>This method generates both the normal and disabled state images for
        /// the specified toolbar button based on the provided SVG image.
        /// If <paramref name="svg"/> is <see langword="null"/>, the
        /// images for the button will be cleared.</remarks>
        /// <param name="svg">The <see cref="SvgImage"/> to use as the base image.
        /// Can be <see langword="null"/> to clear the current image.</param>
        /// <param name="btn">The <see cref="KnownButton"/> representing the toolbar button
        /// to associate with the image. Can be <see langword="null"/>
        /// if no specific button is targeted.</param>
        /// <param name="size">An optional <see cref="CoordAndUnit"/> specifying
        /// the desired size of the image. If <see langword="null"/>,
        /// the default size is used.</param>
        public virtual void SetSvg(
            SvgImage? svg,
            KnownButton? btn,
            CoordAndUnit? size)
        {
            var (normalImage, disabledImage)
                = ToolBarUtils.GetNormalAndDisabledSvgImages(svg, btn, this, size);

            SetImages(normalImage, disabledImage);
        }

        /// <summary>
        /// Sets the image and alignment properties for a button based on the specified configuration.
        /// </summary>
        /// <remarks>This method updates the horizontal and vertical alignment of the image if alignment
        /// values are provided in the <paramref name="knownButtonAndImage"/> parameter.
        /// It then sets the SVG image, button type, and size using the specified configuration.</remarks>
        /// <param name="knownButtonAndImage">An object containing the image, button, alignment,
        /// and size configuration to apply.</param>
        public virtual void SetImage(KnownButtonImage knownButtonAndImage)
        {
            DoInsideLayout(() =>
            {
                if(knownButtonAndImage.Alignment is not null)
                {
                    ImageHorizontalAlignment = knownButtonAndImage.Alignment.Value.Horizontal;
                    ImageVerticalAlignment = knownButtonAndImage.Alignment.Value.Vertical;
                }

                SetSvg(
                    knownButtonAndImage.SvgImage,
                    knownButtonAndImage.KnownButton,
                    knownButtonAndImage.Size);
            });
        }

        /// <summary>
        /// Sets the label image to a default arrow image, typically used to indicate
        /// a submenu or navigation direction.
        /// </summary>
        public virtual void SetLabelImageAsMenuArrow()
        {
            SetLabelImage(MenuItem.DefaultMenuArrowImage);
        }

        /// <summary>
        /// Configures the label image to represent a combo box control.
        /// </summary>
        /// <remarks>This method sets the label image to a predefined combo box representation,
        /// using optional parameters to customize its alignment and size.</remarks>
        /// <param name="align">The alignment of the label image.
        /// If <see langword="null"/>, a default alignment is used.</param>
        /// <param name="size">The size of the label image.
        /// If <see langword="null"/>, a default size is used.</param>
        public virtual void SetLabelImageAsComboBox(
            HVAlignment? align = null,
            CoordAndUnit? size = null)
        {
            SetLabelImage(
                ControlAndButton.DefaultBtnComboBoxSvg,
                KnownButton.TextBoxCombo,
                align,
                size);
        }

        /// <summary>
        /// Sets the horizontal alignment of the content elements within the control.
        /// </summary>
        /// <remarks>This method updates the horizontal alignment of the content elements,
        /// including the picture, spacer, and label, to the specified alignment.
        /// After updating the alignment, the layout is
        /// recalculated and the control is invalidated.</remarks>
        /// <param name="alignment">The desired horizontal alignment for the content
        /// elements. Valid values are defined by the
        /// <see cref="HorizontalAlignment"/> enumeration.</param>
        public virtual void SetContentHorizontalAlignment(HorizontalAlignment alignment)
        {
            pictureSpacer.HorizontalAlignment = alignment;
            spacer.HorizontalAlignment = alignment;
            label.HorizontalAlignment = alignment;
            PerformLayoutAndInvalidate(null, false);
        }

        /// <summary>
        /// Sets the horizontal alignment for the content elements within the control.
        /// </summary>
        /// <param name="pictureAlignment">The desired horizontal alignment for the picture.</param>
        /// <param name="labelAlignment">The desired horizontal alignment for the label.</param>
        /// <param name="spacerAlignment">The desired horizontal alignment for the spacer.</param>
        public virtual void SetContentHorizontalAlignment(
            HorizontalAlignment pictureAlignment,
            HorizontalAlignment labelAlignment,
            HorizontalAlignment spacerAlignment)
        {
            pictureSpacer.HorizontalAlignment = pictureAlignment;
            spacer.HorizontalAlignment = spacerAlignment;
            label.HorizontalAlignment = labelAlignment;
            PerformLayoutAndInvalidate(null, false);
        }

        /// <summary>
        /// Shows default combo box image at the right side of the control.
        /// </summary>
        public virtual void ShowComboBoxImageAtRight()
        {
            SetDefaultComboBoxImage();
            ImageVisible = true;
            SetContentHorizontalAlignment(
                        pictureAlignment: HorizontalAlignment.Right,
                        labelAlignment: HorizontalAlignment.Fill,
                        spacerAlignment: HorizontalAlignment.Right);
        }

        /// <summary>
        /// Sets the vertical alignment for the content elements within the control.
        /// </summary>
        /// <remarks>This method updates the vertical alignment of the picture,
        /// spacer, and label elements
        /// within the control. After applying the alignment,
        /// the layout is recalculated and the control is
        /// invalidated.</remarks>
        /// <param name="alignment">The vertical alignment to apply
        /// to the content elements. Valid values are defined
        /// by the <see cref="VerticalAlignment"/> enumeration.</param>
        public virtual void SetContentVerticalAlignment(VerticalAlignment alignment)
        {
            pictureSpacer.VerticalAlignment = alignment;
            spacer.VerticalAlignment = alignment;
            label.VerticalAlignment = alignment;
            PerformLayoutAndInvalidate(null, false);
        }

        /// <summary>
        /// Configures the content to match the appearance of a menu item.
        /// </summary>
        /// <remarks>This method adjusts content layout, alignment and other properties
        /// to match the typical appearance and behavior of a menu item.</remarks>
        public virtual void ConfigureAsMenuItem()
        {
            DoInsideLayout(() =>
            {
                TextVisible = true;
                HorizontalAlignment = HorizontalAlignment.Fill;
                IsToolTipEnabled = false;
                SuggestedSize = (Coord.NaN, SuggestedHeight);
                SetContentHorizontalAlignment(HorizontalAlignment.Left);
                Label.HorizontalAlignment = HorizontalAlignment.Fill;
                RightSideElement = SpeedButton.RightSideElementKind.KeyGesture;
                ShowDropDownMenuWhenHovered = true;

                if(!App.IsMaui)
                    UseInternalDropDownMenu = true;

                if (DropDownMenu is null)
                {
                    SetLabelImage(null);
                }
                else
                {
                    SetLabelImage(MenuItem.DefaultMenuArrowImage);
                }
            });
        }

        /// <inheritdoc/>
        public override void RaiseClick(EventArgs e)
        {
            if (TimerUtils.LastClickLessThanRepeatInterval(this))
            {
            }
            else
            {
                base.RaiseClick(e);
            }
        }

        /// <summary>
        /// Assigns the properties of the specified menu item to the current instance.
        /// </summary>
        /// <remarks>This method updates the current instance's properties, such as images,
        /// visibility,  enabled state, and shortcut, based on the values provided
        /// by the <paramref name="menuItem"/>. If the
        /// <paramref name="menuItem"/> has an SVG image, it is set using
        /// the <c>SetSvgImage</c> method;  otherwise, the
        /// standard and disabled images are assigned directly.</remarks>
        /// <param name="menuItem">The menu item whose properties are to be assigned.
        /// Cannot be <see langword="null"/>.</param>
        public virtual void Assign(IMenuItemProperties menuItem)
        {
            Text = menuItem.Text;

            if (menuItem.SvgImage is not null)
            {
                SetSvgImage(menuItem.SvgImage, null);
            }
            else
            {
                ImageSet = menuItem.Image;
                DisabledImageSet = menuItem.DisabledImage;
            }

            Visible = menuItem.Visible;
            Enabled = menuItem.Enabled;
            Shortcut = menuItem.Shortcut;

            if (menuItem.Count == 0)
            {
                DropDownMenu = null;
                RightSideElement = RightSideElementKind.KeyGesture;
            }
            else
            {
                RightSideElement = RightSideElementKind.Image;
                DropDownMenu ??= new ContextMenu();
                DropDownMenu.Assign(menuItem);
                SetRightSideImage(MenuItem.DefaultMenuArrowImage);
            }
        }

        /// <inheritdoc/>
        public override void DefaultPaint(PaintEventArgs e)
        {
            var state = VisualState;

            DrawDefaultBackground(e, DrawDefaultBackgroundFlags.DrawBackground);

            if (TextVisible)
            {
                var foreColor = StateObjects?.Colors?.GetObjectOrNull(state)?.ForegroundColor;
                if (foreColor is null)
                {
                    var theme = GetDefaultTheme()?.DarkOrLight(IsDarkBackground);
                    foreColor ??= theme?.Colors?.GetObjectOrNull(state)?.ForegroundColor;
                }

                Label.ForegroundColor = foreColor;
                TemplateUtils.RaisePaintRecursive(Label, e.Graphics, Label.Location);
            }

            if (HasVisibleImage)
            {
                if (ImageToText == ImageToText.Horizontal && TextVisible
                    && ImageHorizontalAlignment == HorizontalAlignment.Right)
                {
                    var newRect = new RectD(
                        spacer.Location.X,
                        0,
                        ClientRectangle.Width - spacer.Location.X,
                        ClientRectangle.Height);
                    DrawDefaultBackground(
                        e.WithRect(newRect),
                        DrawDefaultBackgroundFlags.DrawBackground);
                }

                drawable.VisualState = Enabled
                    ? VisualControlState.Normal : VisualControlState.Disabled;

                drawable.Bounds = PictureBox.Bounds;
                drawable.Draw(this, e.Graphics);
            }

            DrawDefaultBackground(e, DrawDefaultBackgroundFlags.DrawBorder);
        }

        /// <inheritdoc/>
        public override ControlColorAndStyle? GetDefaultTheme()
        {
            var theme = UseTheme;

            if (Sticky)
                theme = UseThemeForSticky;

            switch (theme)
            {
                case KnownTheme.None:
                    return null;
                case KnownTheme.Custom:
                    return CustomTheme ?? DefaultCustomTheme;
                case KnownTheme.StaticBorder:
                    return StaticBorderTheme;
                case KnownTheme.StickyBorder:
                    return StickyBorderTheme;
                case KnownTheme.NoBorder:
                    return NoBorderTheme;
                case KnownTheme.CheckBorder:
                    return CheckBorderTheme;
                case KnownTheme.PushButton:
                    return PushButtonTheme;
                case KnownTheme.PushButtonHovered:
                    return PushButtonHoveredTheme;
                case KnownTheme.PushButtonPressed:
                    return PushButtonPressedTheme;
                case KnownTheme.RoundBorder:
                    return RoundBorderTheme;
                case KnownTheme.Default:
                default:
                    return DefaultTheme;
            }
        }

        /// <summary>
        /// Retrieves the element displayed on the right side based on the current configuration.
        /// </summary>
        /// <remarks>Override this method in a derived class to customize
        /// the behavior for determining the right-side element.</remarks>
        /// <returns>An object representing the right-side element.
        /// Returns <see langword="null"/> if no element is configured
        /// or if the configuration is unrecognized. Possible return values include:
        /// <list type="bullet">
        /// <item><description><see langword="null"/> if <see cref="RightSideElementKind"/>
        /// is <see
        /// cref="RightSideElementKind.None"/>.</description></item>
        /// <item><description>A string representing the
        /// shortcut text if <see cref="RightSideElementKind"/> is <see
        /// cref="RightSideElementKind.KeyGesture"/>.</description></item>
        /// <item><description>The value of <see
        /// cref="RightSideText"/> if <see cref="RightSideElementKind"/> is <see
        /// cref="RightSideElementKind.Text"/>.</description></item>
        /// </list></returns>
        protected virtual object? GetRightSideElement()
        {
            return rightSideElementKind switch
            {
                RightSideElementKind.None => null,
                RightSideElementKind.KeyGesture
                    => GetShortcutText(useTemplate: false) ?? string.Empty,
                RightSideElementKind.KeyGestureFormatted
                    => GetShortcutText(useTemplate: true) ?? string.Empty,
                RightSideElementKind.Text => RightSideText ?? string.Empty,
                RightSideElementKind.Image
                    => IsEnabled ? RightSideImage : RightSideDisabledImage ?? RightSideImage,
                _ => null,
            };
        }

        /// <summary>
        /// Invoked before a label is drawn, allowing for custom handling or modifications.
        /// </summary>
        /// <remarks>This method is intended to be overridden in a derived class to provide custom
        /// behavior before a label is drawn.</remarks>
        /// <param name="sender">The source of the event, typically the object raising the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnBeforeDrawLabel(object? sender, EventArgs e)
        {
            if (ImageToText != ImageToText.Horizontal || !TextVisible)
                return;

            label.UpdateDrawLabelParams(UpdateLabelParams);

            void UpdateLabelParams(ref Graphics.DrawLabelParams prm)
            {
                var elements = GetRightSideElements(ref prm);
                if (elements is null || elements.Length == 0)
                    return;
                prm.SuffixElements = elements;
            }
        }

        /// <summary>
        /// Generates an array of drawing parameters for the right-side elements of a label.
        /// </summary>
        /// <remarks>This method determines the right-side elements based on the result of
        /// <c>GetRightSideElement</c>. If the right-side element is a string,
        /// it creates a text element. If it is an image, it creates an image element.
        /// A spacer element is also included to ensure proper alignment. The
        /// elements are aligned to the right horizontally.</remarks>
        /// <param name="prm">A reference to the <see cref="Graphics.DrawLabelParams"/> structure
        /// that provides the context for drawing
        /// the label, including layout and style information.</param>
        /// <returns>An array of <see cref="Graphics.DrawElementParams"/> representing
        /// the right-side elements to be drawn, or
        /// <see langword="null"/> if no right-side elements are available.</returns>
        protected virtual Graphics.DrawElementParams[]? GetRightSideElements(
            ref Graphics.DrawLabelParams prm)
        {
            var rightSide = GetRightSideElement();

            if (rightSide is null)
                return null;

            Graphics.DrawElementParams element;

            if (rightSide is string s)
            {
                element = Graphics.DrawElementParams.CreateTextElement(ref prm, s);
            }
            else
            if (rightSide is Image img)
            {
                element = Graphics.DrawElementParams.CreateImageElement(ref prm, img);
            }
            else
                return null;

            element.MinWidth = MinRightSideWidth;

            var spacerElement = Graphics.DrawElementParams.CreateSpacerElement(spacer.Size);
            element.Alignment = element.Alignment.WithHorizontal(HorizontalAlignment.Right);

            return [spacerElement, element];
        }

        /// <inheritdoc/>
        protected override void ShowDropDownMenu(Action? afterShow = null)
        {
            if (!Enabled || DropDownMenu is null)
                return;

            if(VisualState == VisualControlState.Normal)
            {
                Internal(afterShow);
            }
            else
            {
                VisualStateOverride = VisualControlState.Normal;
                Refresh();

                Internal(AfterShow);
            }

            void AfterShow()
            {
                VisualStateOverride = null;
                afterShow?.Invoke();
            }

            void Internal(Action? afterShow = null)
            {
                base.ShowDropDownMenu(afterShow);
            }
        }

        /// <inheritdoc/>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (StickyToggleOnClick && e.ChangedButton == MouseButton.Left)
            {
                Sticky = !Sticky;
            }

            base.OnMouseDown(e);
            if (HideToolTipOnClick)
                HideToolTip();
        }

        /// <inheritdoc/>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
        }

        /// <inheritdoc/>
        protected override IReadOnlyList<ShortcutAndAction>? GetShortcuts()
        {
            if (shortcut is null)
                return null;
            return new ShortcutAndAction[] { new(shortcut, Fn) };

            void Fn()
            {
                RaiseClick(EventArgs.Empty);
                ShowDropDownMenu();
            }
        }

        /// <summary>
        /// Called when <see cref="ImageToText"/> property is changed.
        /// </summary>
        protected virtual void OnImageToTextChanged()
        {
            if (ImageToText == ImageToText.Horizontal || !TextVisible || !ImageVisible)
                Layout = LayoutStyle.Horizontal;
            else
                Layout = LayoutStyle.Vertical;
        }

        /// <inheritdoc/>
        protected override void OnVisualStateChanged(EventArgs e)
        {
            base.OnVisualStateChanged(e);
            if (VisualState == VisualControlState.Pressed)
                SubscribeClickRepeated();
            else
                UnsubscribeClickRepeated();
        }

        /// <summary>
        /// Called when <see cref="Sticky"/> property is changed.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnStickyChanged(EventArgs e)
        {
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            commandSource.Changed = null;
            UnsubscribeClickRepeated();
            base.DisposeManaged();
        }

        /// <inheritdoc />
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            clickAction?.Invoke();
            commandSource.Execute();
        }

        /// <summary>
        /// Updates the sticky state of sibling <c>SpeedButton</c> controls
        /// based on the current button’s
        /// <c>IsSticky</c> setting and <see cref="StickySpreadMode"/>.
        /// </summary>
        /// <remarks>
        /// This method is invoked when the current button becomes sticky and may
        /// modify sibling buttons within the same container or logical group to
        /// enforce exclusive selection,
        /// group-wide activation, or other propagation behavior
        /// as defined by <see cref="CheckedSpreadMode"/>.
        /// Override this method to customize spread logic or integrate group rules.
        /// </remarks>
        protected virtual void UpdateSiblingStickyState()
        {
            if (Parent is null || !Sticky)
                return;

            switch (StickySpreadMode)
            {
                case CheckedSpreadMode.None:
                default:
                    return;
                case CheckedSpreadMode.SingleSibling:
                    Parent.ForEachVisibleChild(Uncheck, recursive: false);
                    break;
                case CheckedSpreadMode.SingleInGroup:
                    var group = Parent.GetMembersOfGroups(GroupIndexes, false);
                    group.ForEachVisible(Uncheck);
                    break;
            }

            void Uncheck(AbstractControl control)
            {
                if (control is not SpeedButton button || button == this)
                    return;
                button.Sticky = false;
            }
        }

        /// <inheritdoc/>
        protected override void OnDataContextChanged(object? oldValue, object? newValue)
        {
            if (oldValue is IMenuItemProperties oldProperties)
            {
                oldProperties.Changed -= OnMenuItemChanged;
            }

            if (newValue is IMenuItemProperties newProperties)
            {
                newProperties.Changed += OnMenuItemChanged;
                Assign(newProperties);
            }
        }

        /// <inheritdoc/>
        protected override void OnMouseHover(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            if (ShowDropDownMenuWhenHovered && DropDownMenu is not null)
            {
                if (DropDownMenu.IsShownInHostControl)
                    return;

                ShowDropDownMenu();
            }
        }

        /// <summary>
        /// Invoked when attached menu item change event occurs.
        /// </summary>
        /// <remarks>This method is called to handle menu item change events
        /// and can be overridden in a derived class to provide custom handling logic.</remarks>
        /// <param name="sender">The source of the event, which may be <see langword="null"/>.</param>
        /// <param name="e">An event argument of type <see cref="BaseEventArgs{T}"/>
        /// containing the details of the menu item change, including the kind of change.</param>
        protected virtual void OnMenuItemChanged(object? sender, BaseEventArgs<MenuChangeKind> e)
        {
            if(sender is not IMenuItemProperties menuItem)
                return;
            Assign(menuItem);
        }

        /// <summary>
        /// Invoked periodically while the control remains in the pressed state.
        /// This method is called only when <see cref="IsClickRepeated"/>
        /// is set to <see langword="true"/>.
        /// </summary>
        /// <remarks>This method is called to handle repeated click events when
        /// the control is in the <see cref="VisualControlState.Pressed"/> state.
        /// If the control is no longer in the pressed state, the
        /// method exits without raising the click event.</remarks>
        protected virtual void OnClickRepeatTimerEvent()
        {
            if (VisualState != VisualControlState.Pressed)
                return;
            RaiseClick(EventArgs.Empty);
        }

        /// <summary>
        /// Unsubscribes from repeated click events and releases associated resources.
        /// </summary>
        /// <remarks>This method stops any active timers used for handling repeated click events
        /// and ensures that resources are properly disposed.
        /// After calling this method, repeated click events will no
        /// longer be processed.</remarks>
        protected virtual void UnsubscribeClickRepeated()
        {
            if (subscribedClickRepeated)
            {
                SafeDispose(ref repeatTimer);
                SafeDispose(ref firstClickTimer);
                subscribedClickRepeated = false;
            }
        }

        /// <summary>
        /// Handles changes to the picture size.
        /// </summary>
        /// <remarks>
        /// Derived classes can override this method to provide custom behavior
        /// when the picture size changes.</remarks>
        protected virtual void PictureSizeChanged()
        {
            PictureBox.SuggestedSize = drawable.GetPreferredSize(this);
        }

        /// <summary>
        /// Subscribes to the click repeated event, initializing and starting timers
        /// to handle repeated click actions.
        /// </summary>
        /// <remarks>This method sets up timers to manage repeated click events. It ensures that the
        /// timers are properly initialized  and started only if the click repeated
        /// functionality is enabled and has not
        /// already been subscribed. The first timer introduces a delay before repeated
        /// actions begin, while the second timer handles the repeated actions
        /// at regular intervals.</remarks>
        protected virtual void SubscribeClickRepeated()
        {
            if (!subscribedClickRepeated && IsClickRepeated)
            {
                SafeDispose(ref repeatTimer);
                firstClickTimer ??= new();
                firstClickTimer.Stop();
                firstClickTimer.Interval = ClickRepeatDelay * 5;
                firstClickTimer.AutoReset = false;
                firstClickTimer.TickAction = () =>
                {
                    repeatTimer ??= new();
                    repeatTimer.Stop();
                    repeatTimer.Interval = ClickRepeatDelay;
                    repeatTimer.AutoReset = true;
                    repeatTimer.TickAction = OnClickRepeatTimerEvent;
                    repeatTimer.Start();
                };
                firstClickTimer.Start();
                subscribedClickRepeated = true;
            }
        }
    }
}
