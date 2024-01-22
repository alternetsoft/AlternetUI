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
    public partial class SpeedButton : PictureBox
    {
        private static SpeedButton? defaults;
        private Action? clickAction;
        private bool sticky;
        private KeyInfo[]? keys;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeedButton"/> class.
        /// </summary>
        public SpeedButton()
        {
            AcceptsFocusAll = false;
            ImageStretch = false;
            Borders ??= new();

            var defaultsButton = GetButtonDefaults();

            if(defaultsButton is null)
                InitBorderAndColors();
            else
                AssignBorderAndColors(defaultsButton);
        }

        /// <summary>
        /// Gets or sets default hovered state colors.
        /// </summary>
        public static IReadOnlyFontAndColor? DefaultHoveredColors { get; set; }

        /// <summary>
        /// Gets or sets default pressed state colors.
        /// </summary>
        public static IReadOnlyFontAndColor? DefaultPressedColors { get; set; }

        /// <summary>
        /// Gets or sets default hovered state colors.
        /// </summary>
        public static IReadOnlyFontAndColor? DefaultNormalColors { get; set; }

        /// <summary>
        /// Gets or sets default hovered state colors.
        /// </summary>
        public static IReadOnlyFontAndColor? DefaultDisabledColors { get; set; }

        /// <summary>
        /// Gets or sets default hovered state colors.
        /// </summary>
        public static IReadOnlyFontAndColor? DefaultFocusedColors { get; set; }

        /// <summary>
        /// Gets or sets default template for the shortcut when it is shown in the tooltip.
        /// </summary>
        /// <remarks>
        /// Default value is "({0})".
        /// </remarks>
        public static string DefaultShortcutToolTipTemplate { get; set; } = "({0})";

        /// <summary>
        /// Gets or sets default settings for the <see cref="SpeedButton"/>.
        /// </summary>
        /// <remarks>
        /// Create instance of the <see cref="SpeedButton"/> and assign to this property.
        /// You can specify border and background settings and all new <see cref="SpeedButton"/>
        /// controls will inherit them.
        /// </remarks>
        public static SpeedButton? Defaults
        {
            get
            {
                return defaults;
            }

            set
            {
                defaults = value;
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
        /// Gets or sets the shortcut keys associated with the control.
        /// </summary>
        /// <returns>
        /// One of the <see cref="Keys" /> values. The default is <see cref="Keys.None" />.</returns>
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
                    ShortcutKeyInfo = [new KeyInfo(value.Key, value.Modifiers)];
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
                    if (result == GenericControlState.Normal || result == GenericControlState.Focused)
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
        /// Assigns borders and colors from other <see cref="SpeedButton"/> instance.
        /// </summary>
        /// <param name="button"></param>
        protected virtual void AssignBorderAndColors(SpeedButton button)
        {
            Borders ??= new();
            Borders.Assign(button.Borders);
            Backgrounds = button.Backgrounds;
            StateObjects!.Colors = button.StateObjects?.Colors;
            Padding = button.Padding;
        }

        /// <summary>
        /// Initializes borders and colors with default settings.
        /// </summary>
        protected virtual void InitBorderAndColors()
        {
            Borders ??= new();

            var hoveredBorder = BorderSettings.Default.Clone();
            hoveredBorder.UniformRadiusIsPercent = true;
            hoveredBorder.UniformCornerRadius = 25;
            Borders.SetObject(hoveredBorder, GenericControlState.Hovered);

            var pressedBorder = hoveredBorder.Clone();

            Borders.SetObject(pressedBorder, GenericControlState.Pressed);

            Padding = 4;
            SetStateColors(GenericControlState.Normal, GetNormalColors());
            SetStateColors(GenericControlState.Hovered, GetHoveredColors());
            SetStateColors(GenericControlState.Pressed, GetPressedColors());
            SetStateColors(GenericControlState.Disabled, GetDisabledColors());
            SetStateColors(GenericControlState.Focused, GetFocusedColors());
        }

        /// <summary>
        /// Gets control colors for the pressed state.
        /// </summary>
        /// <returns></returns>
        protected virtual IReadOnlyFontAndColor? GetPressedColors()
        {
            return DefaultPressedColors;
        }

        /// <summary>
        /// Gets control colors for the hovered state.
        /// </summary>
        /// <returns></returns>
        protected virtual IReadOnlyFontAndColor? GetHoveredColors()
        {
            return DefaultHoveredColors;
        }

        /// <summary>
        /// Gets control colors for the focused state.
        /// </summary>
        /// <returns></returns>
        protected virtual IReadOnlyFontAndColor? GetFocusedColors()
        {
            return DefaultFocusedColors;
        }

        /// <summary>
        /// Gets control colors for the disabled state.
        /// </summary>
        /// <returns></returns>
        protected virtual IReadOnlyFontAndColor? GetDisabledColors()
        {
            return DefaultDisabledColors;
        }

        /// <summary>
        /// Gets control colors for the normal state.
        /// </summary>
        /// <returns></returns>
        protected virtual IReadOnlyFontAndColor? GetNormalColors()
        {
            return DefaultNormalColors;
        }

        /// <summary>
        /// Gets default <see cref="SpeedButton"/> instance from which border and color
        /// settings are assigned.
        /// </summary>
        /// <returns></returns>
        protected virtual SpeedButton? GetButtonDefaults()
        {
            return defaults;
        }

        private void OnClickAction(object? sender, EventArgs? e)
        {
            clickAction?.Invoke();
        }
    }
}
