using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Parent class for all owner draw controls.
    /// </summary>
    [ControlCategory("Other")]
    public partial class UserControl : Control
    {
        private bool hasBorder;
        private RichTextBoxScrollBars scrollBars = RichTextBoxScrollBars.None;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserControl"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public UserControl(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserControl"/> class.
        /// </summary>
        public UserControl()
        {
            hasBorder = GetDefaultHasBorder();
            UserPaint = true;
        }

        /// <summary>
        /// Gets or sets different behavior and visualization options.
        /// </summary>
        [Browsable(false)]
        public virtual ControlRefreshOptions RefreshOptions { get; set; }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.UserPaintControl;

        /// <summary>
        /// Gets or sets <see cref="ContextMenu"/> which is shown when control is clicked.
        /// </summary>
        [Browsable(false)]
        public virtual ContextMenu? DropDownMenu { get; set; }

        /// <summary>
        /// Gets or sets the type of scroll bars displayed in the control.
        /// </summary>
        [Browsable(false)]
        public virtual RichTextBoxScrollBars ScrollBars
        {
            get
            {
                return scrollBars;
            }

            set
            {
                if (scrollBars == value)
                    return;
                scrollBars = value;

                var (horizontal, vertical) = ScrollBarUtils.AsHiddenOrVisible(value);
                VertScrollBarInfo = VertScrollBarInfo.WithVisibility(vertical);
                HorzScrollBarInfo = HorzScrollBarInfo.WithVisibility(horizontal);
            }
        }

        /// <summary>
        /// Gets or sets whether control wants to get all char/key events for all keys.
        /// </summary>
        /// <remarks>
        /// Use this to indicate that the control wants to get all char/key events for all keys
        /// - even for keys like TAB or ENTER which are usually used for dialog navigation and
        /// which wouldn't be generated without this style. If you need to use this style in
        /// order to get the arrows or etc., but would still like to have normal keyboard
        /// navigation take place, you should call Navigate in response to the key events
        /// for Tab and Shift-Tab.
        /// </remarks>
        [Browsable(false)]
        public virtual bool WantChars
        {
            get
            {
                return Handler.WantChars;
            }

            set
            {
                Handler.WantChars = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control has a border.
        /// </summary>
        [Browsable(true)]
        public override bool HasBorder
        {
            get
            {
                return hasBorder;
            }

            set
            {
                if (hasBorder == value)
                    return;
                hasBorder = value;
                Refresh();
            }
        }

        /// <summary>
        /// Sets <see cref="AbstractControl.StateObjects"/> colors and backgrounds
        /// for the state specified
        /// in the <paramref name="state"/> parameter to the
        /// colors from <paramref name="fontAndColor"/>.
        /// </summary>
        /// <param name="state">Affected control state.</param>
        /// <param name="fontAndColor">Colors.</param>
        public virtual void SetStateColors(
            VisualControlState state,
            IReadOnlyFontAndColor? fontAndColor)
        {
            if (fontAndColor is null && StateObjects?.Colors is null
                && StateObjects?.Backgrounds is null)
                return;
            StateObjects ??= new();
            StateObjects.Colors ??= new();
            StateObjects.Backgrounds ??= new();
            StateObjects.Colors.SetObject(fontAndColor, state);
            StateObjects.Backgrounds.SetObject(fontAndColor?.BackgroundColor?.AsBrush, state);
        }

        /// <inheritdoc/>
        public override Brush? GetBackground(VisualControlState state)
        {
            var overrideValue = Backgrounds?.GetObjectOrNormal(state);
            if (overrideValue is not null)
                return overrideValue;

            var result = GetDefaultTheme()?.DarkOrLight(IsDarkBackground);
            var brush = result?.Backgrounds?.GetObjectOrNormal(state);
            brush ??= BackgroundColor?.AsBrush;
            return brush;
        }

        /// <summary>
        /// Default painting method of the <see cref="UserControl"/>
        /// and its descendants.
        /// </summary>
        /// <param name="e">Paint arguments.</param>
        public virtual void DefaultPaint(PaintEventArgs e)
        {
        }

        /// <inheritdoc/>
        public override BorderSettings? GetBorderSettings(VisualControlState state)
        {
            var overrideValue = Borders?.GetObjectOrNull(state);
            if (overrideValue is not null)
                return overrideValue;

            var result = GetDefaultTheme()?.DarkOrLight(IsDarkBackground);
            return result?.Borders?.GetObjectOrNull(state);
        }

        /// <summary>
        /// Draw default background.
        /// </summary>
        /// <param name="e">Paint arguments.</param>
        public virtual void DrawDefaultBackground(PaintEventArgs e)
        {
            var state = VisualState;
            var brush = GetBackground(state);
            var border = GetBorderSettings(state);

            if (brush is null && (border is null || !HasBorder))
                return;

            var dc = e.Graphics;
            var rect = e.ClipRectangle;

            dc.FillBorderRectangle(
                rect,
                brush,
                border,
                HasBorder,
                this);
        }

        /// <summary>
        /// Gets default color and style settings.
        /// </summary>
        /// <returns></returns>
        protected virtual ControlColorAndStyle? GetDefaultTheme() => null;

        /// <inheritdoc/>
        protected override void OnMouseLeftButtonDown(MouseEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (!Enabled)
                return;
            RaiseClick(e);
            ShowDropDownMenu();
            Invalidate();
        }

        /// <summary>
        /// Shows attached drop down menu.
        /// </summary>
        protected virtual void ShowDropDownMenu()
        {
            PointD pt = (0, Bounds.Height);
            DropDownMenu?.Show(this, (pt.X, pt.Y));
        }

        /// <inheritdoc/>
        protected override void OnPaint(PaintEventArgs e)
        {
            DefaultPaint(e);
            DefaultPaintDebug(e);
        }

        /// <inheritdoc/>
        protected override void OnVisualStateChanged(EventArgs e)
        {
            base.OnVisualStateChanged(e);

            var options = RefreshOptions;

            if (options.HasFlag(ControlRefreshOptions.RefreshOnState))
            {
                Refresh();
                return;
            }

            var data = StateObjects;
            if (data is null)
                return;

            bool RefreshOnBorder() => options.HasFlag(ControlRefreshOptions.RefreshOnBorder) &&
                data.HasOtherBorders;
            bool RefreshOnImage() => options.HasFlag(ControlRefreshOptions.RefreshOnImage) &&
                data.HasOtherImages;
            bool RefreshOnColor() => options.HasFlag(ControlRefreshOptions.RefreshOnColor) &&
                data.HasOtherColors;
            bool RefreshOnBackground() => options.HasFlag(ControlRefreshOptions.RefreshOnBackground) &&
                data.HasOtherBackgrounds;

            if (RefreshOnBorder() || RefreshOnImage() || RefreshOnBackground()
                || RefreshOnColor())
                Refresh();
        }

        /// <summary>
        /// Gets default value for <see cref="HasBorder"/>.
        /// </summary>
        /// <returns></returns>
        protected virtual bool GetDefaultHasBorder() => true;

        /// <inheritdoc/>
        protected override void OnSystemColorsChanged(EventArgs e)
        {
            base.OnSystemColorsChanged(e);
            SystemSettings.ResetColors();
            Invalidate();
        }

        [Conditional("DEBUG")]
        private void DefaultPaintDebug(PaintEventArgs e)
        {
            if (DebugUtils.IsDebugDefined && ContainerControl.ShowDebugFocusRect)
            {
                if (FocusedControl?.Parent == this)
                {
                    e.Graphics.FillBorderRectangle(
                        FocusedControl.Bounds.Inflated(),
                        null,
                        BorderSettings.DebugBorder);
                }
            }
        }
    }
}
