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
    [ControlCategory("Other")]
    public partial class SpeedButton : GraphicControl
    {
        /// <summary>
        /// Gets or sets default image and label distance in the <see cref="SpeedButton"/>.
        /// </summary>
        public static double DefaultImageLabelDistance = 4;

        /// <summary>
        /// Gets or sets default color and style settings
        /// for all <see cref="SpeedButton"/> controls.
        /// </summary>
        public static ControlColorAndStyle DefaultColorAndStyle = new();

        private readonly PictureBox picture = new()
        {
            ImageStretch = false,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
        };

        private readonly Control spacer = new()
        {
            SuggestedSize = DefaultImageLabelDistance,
            Visible = false,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
        };

        private readonly GenericLabel label = new()
        {
            Visible = false,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
        };

        private Action? clickAction;
        private bool sticky;
        private KeyInfo[]? keys;
        private bool textVisible = false;
        private bool imageVisible = true;

        static SpeedButton()
        {
            InitSchemeLight();
            InitSchemeDark();

            void InitSchemeLight()
            {
                DefaultColorAndStyle.Light.Borders =
                    CreateBorders(Color.FromRgb(214, 214, 214));
                DefaultColorAndStyle.Light.Colors = CreateColorsDark();
            }

            void InitSchemeDark()
            {
                DefaultColorAndStyle.Dark.Borders =
                    CreateBorders(Color.FromRgb(112, 112, 112));
                DefaultColorAndStyle.Dark.Colors = CreateColorsDark();
            }

            ControlStateColors CreateColorsDark()
            {
                AllStateColors colors = new()
                {
                    NormalForeColor = Color.FromRgb(214, 214, 214),
                    NormalBackColor = Color.FromRgb(31, 31, 31),

                    DisabledForeColor = Color.FromRgb(109, 109, 109),
                    DisabledBackColor = Color.FromRgb(31, 31, 31),

                    HoveredForeColor = Color.FromRgb(250, 250, 250),
                    HoveredBackColor = Color.FromRgb(61, 61, 61),

                    PressedForeColor = Color.FromRgb(214, 214, 214),
                    PressedBackColor = Color.FromRgb(34, 34, 34),
                };

                return colors.AllStates;
            }

            ControlStateBorders CreateBorders(Color color)
            {
                ControlStateBorders borders = new();
                var hoveredBorder = CreateBorder(color);
                var pressedBorder = hoveredBorder.Clone();
                borders.SetObject(hoveredBorder, GenericControlState.Hovered);
                borders.SetObject(pressedBorder, GenericControlState.Pressed);
                return borders;
            }

            BorderSettings CreateBorder(Color color)
            {
                BorderSettings result = new()
                {
                    Width = 1,
                    UniformRadiusIsPercent = false,
                    UniformCornerRadius = 3,
                    Color = color,
                };

                return result;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeedButton"/> class.
        /// </summary>
        public SpeedButton()
        {
            Padding = 4;
            Layout = LayoutStyle.Horizontal;
            picture.Parent = this;
            picture.BubbleMouse = true;
            spacer.Parent = this;
            label.Visible = false;
            label.Parent = this;
            label.BubbleMouse = true;

            AcceptsFocusAll = false;
            RefreshOptions = ControlRefreshOptions.RefreshOnState;
        }

        /// <summary>
        /// Gets or sets default template for the shortcut when it is shown in the tooltip.
        /// </summary>
        /// <remarks>
        /// Default value is "({0})".
        /// </remarks>
        public static string DefaultShortcutToolTipTemplate { get; set; } = "({0})";

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
                PerformLayoutAndInvalidate(() =>
                {
                    Label.Visible = value;
                    spacer.Visible = Label.Visible && PictureBox.Visible;
                });
            }
        }

        /// <inheritdoc/>
        public override IReadOnlyList<Control> AllChildrenInLayout
        {
            get
            {
                if (TextVisible)
                {
                    if (ImageVisible)
                        return Children;
                    else
                        return new Control[] { Label };
                }
                else
                {
                    if (ImageVisible)
                        return new Control[] { PictureBox };
                    else
                        return Array.Empty<Control>();
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
                PerformLayoutAndInvalidate(() =>
                {
                    PictureBox.Visible = value;
                    spacer.Visible = Label.Visible && PictureBox.Visible;
                });
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
        public string? ShortcutToolTipTemplate { get; set; }

        /// <summary>
        /// Gets or sets whether control is sticky.
        /// </summary>
        /// <remarks>
        /// When this property is true, control painted as pressed if it is not disabled.
        /// </remarks>
        public bool Sticky
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
                PictureBox.Image = value;
            }
        }

        /// <inheritdoc/>
        [DefaultValue("")]
        [Localizability(LocalizationCategory.Text)]
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
                RaiseTextChanged(EventArgs.Empty);
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
                PictureBox.DisabledImage = value;
            }
        }

        /// <summary>
        /// Gets or sets the disabled <see cref="ImageSet"/> that is displayed by the control.
        /// </summary>
        [Browsable(false)]
        public ImageSet? DisabledImageSet
        {
            get
            {
                return PictureBox.DisabledImageSet;
            }

            set
            {
                PictureBox.DisabledImageSet = value;
            }
        }

        /// <summary>
        /// Gets or sets <see cref="ImageSet"/> that is displayed by the control.
        /// </summary>
        [Browsable(false)]
        public ImageSet? ImageSet
        {
            get
            {
                return PictureBox.ImageSet;
            }

            set
            {
                PictureBox.ImageSet = value;
            }
        }

        /// <summary>
        /// Gets or sets the shortcut keys associated with the control.
        /// </summary>
        /// <returns>
        /// One of the <see cref="Keys" /> values.
        /// The default is <see cref="Keys.None" />.</returns>
        [Localizable(true)]
        [DefaultValue(Keys.None)]
        [Browsable(false)]
        public Keys ShortcutKeys
        {
            get
            {
                if (Shortcut is null)
                    return Keys.None;
                var result = Shortcut.Key.ToKeys(Shortcut.Modifiers);
                return result;
            }

            set
            {
                var key = value.ToKey();
                var modifiers = value.ToModifiers();
                Shortcut = new(key, modifiers);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the shortcut key associated with
        /// the control.
        /// </summary>
        [Browsable(false)]
        public KeyGesture? Shortcut
        {
            get
            {
                if (keys is null || keys.Length == 0)
                    return null;
                return new(keys[0].Key, keys[0].Modifiers);
            }

            set
            {
                if (value is null)
                    ShortcutKeyInfo = null;
                else
                    ShortcutKeyInfo = new KeyInfo[] { new(value.Key, value.Modifiers) };
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the shortcut key associated with
        /// the control.
        /// </summary>
        [Browsable(false)]
        public KeyInfo[]? ShortcutKeyInfo
        {
            get
            {
                return keys;
            }

            set
            {
                if (keys == value)
                    return;

                keys = value;
                var s = ToolTip;
                ToolTip = null;
                ToolTip = s;
            }
        }

        /// <inheritdoc/>
        [Browsable(false)]
        public override GenericControlState CurrentState
        {
            get
            {
                var result = base.CurrentState;
                if (sticky)
                {
                    if (result == GenericControlState.Normal
                        || result == GenericControlState.Focused)
                        result = GenericControlState.Pressed;
                }

                return result;
            }
        }

        /// <summary>
        /// Gets or sets <see cref="Action"/> which will be executed when
        /// this control is clicked by the user.
        /// </summary>
        [Browsable(false)]
        public Action? ClickAction
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

        /// <summary>
        /// Gets inner <see cref="PictureBox"/> control.
        /// </summary>
        [Browsable(false)]
        internal PictureBox PictureBox => picture;

        /// <summary>
        /// Gets inner <see cref="GenericLabel"/> control.
        /// </summary>
        [Browsable(false)]
        internal GenericLabel Label => label;

        /// <inheritdoc/>
        public override string? GetRealToolTip()
        {
            var s = ToolTip;

            if (string.IsNullOrWhiteSpace(s))
            {
                return null;
            }

            var template = ShortcutToolTipTemplate ?? DefaultShortcutToolTipTemplate;

            var filteredKeys = KeyInfo.FilterBackendOs(keys);
            if (filteredKeys is not null && filteredKeys.Length > 0)
            {
                s += " " + string.Format(template, filteredKeys[0]);
            }

            return s;
        }

        /// <summary>
        /// Initializes solid border in the normal state.
        /// Also border width in hovered and pressed states
        /// is made larger than in the normal state.
        /// </summary>
        public virtual void InitSolidBorder()
        {
            Borders ??= new();
            Borders.Normal = new(Borders.Hovered ?? BorderSettings.Default);
            Borders.Disabled = Borders.Normal;
            BorderSettings doubleBorder = new(Borders.Hovered ?? BorderSettings.Default);
            doubleBorder.SetWidth(doubleBorder.Top.Width + 1);
            Borders.Pressed = doubleBorder;
            Borders.Hovered = doubleBorder;
        }

        /// <inheritdoc/>
        public override void DefaultPaint(Graphics dc, RectD rect)
        {
            DrawDefaultBackground(dc, rect);
            if(ImageVisible)
                PictureBox.DrawDefaultImage(dc, PictureBox.Bounds);
            if(TextVisible)
                Label.DrawDefaultText(dc, Label.Bounds);
        }

        /// <inheritdoc/>
        protected override ControlColorAndStyle? GetDefaultColorAndStyle() => DefaultColorAndStyle;

        private void OnClickAction(object? sender, EventArgs? e)
        {
            clickAction?.Invoke();
        }
    }
}
