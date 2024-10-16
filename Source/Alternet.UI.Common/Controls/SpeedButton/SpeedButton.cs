﻿using System;
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
    [ControlCategory("Other")]
    public partial class SpeedButton : GraphicControl
    {
        /// <summary>
        /// Gets or sets default image and label distance in the <see cref="SpeedButton"/>.
        /// </summary>
        public static Coord DefaultImageLabelDistance = 4;

        /// <summary>
        /// Gets ot sets default value of <see cref="UseTheme"/> property.
        /// </summary>
        public static KnownTheme DefaultUseTheme = KnownTheme.Default;

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
        /// which have <see cref="UseTheme"/> equal to <see cref="KnownTheme.Custom"/>.
        /// </summary>
        public static ControlColorAndStyle? DefaultCustomTheme = null;

        /// <summary>
        /// Gets or sets default color and style settings
        /// for all <see cref="SpeedButton"/> controls
        /// which have <see cref="UseTheme"/> equal to <see cref="KnownTheme.TabControl"/>.
        /// </summary>
        public static ControlColorAndStyle? TabControlTheme = DefaultTheme;

        private readonly PictureBox picture = new()
        {
            ImageStretch = false,
            Visible = false,
            Alignment = HVAlignment.Center,
        };

        private readonly AbstractControl spacer = new Spacer()
        {
            SuggestedSize = DefaultImageLabelDistance,
            Visible = false,
            Alignment = HVAlignment.Center,
        };

        private readonly GenericTextControl label = new()
        {
            Visible = false,
            Alignment = HVAlignment.Center,
        };

        private Action? clickAction;
        private bool sticky;
        private ShortcutInfo? shortcut;
        private bool textVisible = false;
        private bool imageVisible = true;
        private KnownTheme useTheme = DefaultUseTheme;
        private ControlColorAndStyle? customTheme;
        private bool isClickRepeated;
        private bool subscribedClickRepeated;

        static SpeedButton()
        {
            InitThemeLight(DefaultTheme.Light);
            InitThemeDark(DefaultTheme.Dark);
            StaticBorderTheme = DefaultTheme.Clone();
            StaticBorderTheme.NormalBorderAsHovered();
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
            Padding = 4;
            Layout = LayoutStyle.Horizontal;
            picture.Parent = this;
            picture.InputTransparent = true;
            spacer.Parent = this;
            label.Parent = this;
            label.InputTransparent = true;

            IsGraphicControl = true;
            RefreshOptions = ControlRefreshOptions.RefreshOnState;
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
        }

        /// <summary>
        /// Gets or sets default template for the shortcut when it is shown in the tooltip.
        /// </summary>
        /// <remarks>
        /// Default value is "({0})".
        /// </remarks>
        public static string DefaultShortcutToolTipTemplate { get; set; } = "({0})";

        /// <summary>
        /// Gets or sets whether mouse clicks are repeated continiously while left mouse
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
                if(value)
                    UseTheme = KnownTheme.Default;
                else
                    UseTheme = KnownTheme.None;
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
                PerformLayoutAndInvalidate();
            }
        }

        /// <inheritdoc/>
        public override IReadOnlyList<AbstractControl> AllChildrenInLayout
        {
            get
            {
                bool hasImage = (Image is not null) || (ImageSet is not null);
                var img = ImageVisible && hasImage;

                if (TextVisible)
                {
                    if (img)
                        return Children;
                    else
                        return new AbstractControl[] { Label };
                }
                else
                {
                    if (img)
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
                PerformLayoutAndInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets a value which specifies display modes for
        /// item image and text.
        /// </summary>
        public virtual ImageToText ImageToText
        {
            get
            {
                if (Layout == LayoutStyle.Horizontal)
                    return ImageToText.Horizontal;
                else
                    return ImageToText.Vertical;
            }

            set
            {
                if (value == ImageToText)
                    return;
                if (value == ImageToText.Horizontal)
                    Layout = LayoutStyle.Horizontal;
                else
                    Layout = LayoutStyle.Vertical;
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
                return PictureBox.Image;
            }

            set
            {
                PerformLayoutAndInvalidate(() =>
                {
                    PictureBox.Image = value;
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
                return PictureBox.DisabledImage;
            }

            set
            {
                PerformLayoutAndInvalidate(() =>
                {
                    PictureBox.DisabledImage = value;
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
                return PictureBox.DisabledImageSet;
            }

            set
            {
                PerformLayoutAndInvalidate(() =>
                {
                    PictureBox.DisabledImageSet = value;
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
                return PictureBox.ImageSet;
            }

            set
            {
                PerformLayoutAndInvalidate(() =>
                {
                    PictureBox.ImageSet = value;
                });
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
                        result = VisualControlState.Pressed;
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
                if (clickAction != null)
                    Click -= OnClickAction;
                clickAction = value;
                if (clickAction != null)
                    Click += OnClickAction;
            }
        }

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
        internal PictureBox PictureBox => picture;

        /// <summary>
        /// Gets inner <see cref="GenericLabel"/> control.
        /// </summary>
        [Browsable(false)]
        internal GenericTextControl Label => label;

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
            BorderSettings border = new()
            {
                Width = 1,
                UniformRadiusIsPercent = false,
                UniformCornerRadius = 3,
                Color = color,
            };

            ControlStateBorders borders = new();
            var hoveredBorder = border;
            var pressedBorder = hoveredBorder.Clone();
            borders.SetObject(hoveredBorder, VisualControlState.Hovered);
            borders.SetObject(pressedBorder, VisualControlState.Pressed);
            return borders;
        }

        /// <inheritdoc/>
        public override string? GetRealToolTip()
        {
            var s = ToolTip;

            if (string.IsNullOrWhiteSpace(s))
            {
                return null;
            }

            var template = ShortcutToolTipTemplate ?? DefaultShortcutToolTipTemplate;

            var filteredKeys = KeyInfo.FilterBackendOs(shortcut?.KeyInfo);
            if (filteredKeys is not null && filteredKeys.Length > 0)
            {
                s += " " + string.Format(template, filteredKeys[0]);
            }

            return s;
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

        /// <inheritdoc/>
        public override void DefaultPaint(Graphics dc, RectD rect)
        {
            DrawDefaultBackground(dc, rect);
            if(ImageVisible)
                PictureBox.DrawDefaultImage(dc, PictureBox.Bounds);
            if (TextVisible)
            {
                var state = VisualState;
                var foreColor = StateObjects?.Colors?.GetObjectOrNull(state)?.ForegroundColor;
                if(foreColor is null)
                {
                    var theme = GetDefaultTheme()?.DarkOrLight(IsDarkBackground);
                    foreColor ??= theme?.Colors?.GetObjectOrNull(state)?.ForegroundColor;
                }

                Label.ForegroundColor = foreColor;
                Label.VisualStateOverride = VisualState;
                RaisePaintRecursive(new(dc, rect), true, true, true);
            }
        }

        /// <inheritdoc/>
        protected override ControlColorAndStyle? GetDefaultTheme()
        {
            switch (UseTheme)
            {
                case KnownTheme.None:
                    return null;
                case KnownTheme.Custom:
                    return CustomTheme ?? DefaultCustomTheme;
                case KnownTheme.StaticBorder:
                    return StaticBorderTheme;
                case KnownTheme.Default:
                default:
                    return DefaultTheme;
            }
        }

        /// <inheritdoc/>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (HideToolTipOnClick)
                HideToolTip();
        }

        /// <inheritdoc/>
        protected override
            IReadOnlyList<(ShortcutInfo Shortcut, Action Action)>? GetShortcuts()
        {
            if (shortcut is null)
                return null;
            return new (ShortcutInfo Shortcut, Action action)[] { (shortcut, Fn) };

            void Fn()
            {
                RaiseClick();
                ShowDropDownMenu();
            }
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

        /// <inheritdoc/>
        protected override void ParentDisposed()
        {
            UnsubscribeClickRepeated();
            base.ParentDisposed();
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            UnsubscribeClickRepeated();
            base.DisposeManaged();
        }

        private void OnClickRepeatTimerEvent(object sender, EventArgs e)
        {
            if (VisualState != VisualControlState.Pressed)
                return;

            if (TimerUtils.LastClickLessThanRepeatInterval(this))
                return;
            RaiseClick();
        }

        private void UnsubscribeClickRepeated()
        {
            if (subscribedClickRepeated)
            {
                TimerUtils.ClickRepeated -= OnClickRepeatTimerEvent;
                subscribedClickRepeated = false;
            }
        }

        private void SubscribeClickRepeated()
        {
            if (!subscribedClickRepeated && IsClickRepeated)
            {
                TimerUtils.ClickRepeated += OnClickRepeatTimerEvent;
                subscribedClickRepeated = true;
            }
        }

        private void OnClickAction(object? sender, EventArgs? e)
        {
            clickAction?.Invoke();
        }
    }
}
