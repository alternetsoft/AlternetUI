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
    /// Parent class for all owner draw controls.
    /// </summary>
    [ControlCategory("Other")]
    public partial class UserControl : Control
    {
        private bool hasBorder = true; // !! to border settings
        private RichTextBoxScrollBars scrollBars = RichTextBoxScrollBars.None;
        private Caret? caret;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserControl"/> class.
        /// </summary>
        public UserControl()
            : base()
        {
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
        public ContextMenu? DropDownMenu { get; set; }

        /// <summary>
        /// Gets or sets the type of scroll bars displayed in the control.
        /// </summary>
        [Browsable(false)]
        public RichTextBoxScrollBars ScrollBars
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
                switch (value)
                {
                    default:
                    case RichTextBoxScrollBars.None:
                        SetScrollbars(horz: false, vert: false, always: false);
                        break;
                    case RichTextBoxScrollBars.Horizontal:
                        SetScrollbars(horz: true, vert: false, always: false);
                        break;
                    case RichTextBoxScrollBars.Vertical:
                        SetScrollbars(horz: false, vert: true, always: false);
                        break;
                    case RichTextBoxScrollBars.Both:
                        SetScrollbars(horz: true, vert: true, always: false);
                        break;
                    case RichTextBoxScrollBars.ForcedHorizontal:
                        SetScrollbars(horz: true, vert: false, always: true);
                        break;
                    case RichTextBoxScrollBars.ForcedVertical:
                        SetScrollbars(horz: false, vert: true, always: true);
                        break;
                    case RichTextBoxScrollBars.ForcedBoth:
                        SetScrollbars(horz: true, vert: true, always: true);
                        break;
                }
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
        public bool WantChars
        {
            get
            {
                return NativeControl.WantChars;
            }

            set
            {
                NativeControl.WantChars = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control has a border.
        /// </summary>
        public bool HasBorder
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
        /// Gets or sets <see cref="Caret"/> associated with this control.
        /// </summary>
        [Browsable(false)]
        public Caret? Caret
        {
            get
            {
                return caret;
            }

            set
            {
                caret = value;
            }
        }

        internal new Native.Panel NativeControl => (Native.Panel)base.NativeControl;

        /// <summary>
        /// Sets <see cref="Control.StateObjects"/> colors and backgrounds for the state specified
        /// in the <paramref name="state"/> parameter to the
        /// colors from <paramref name="fontAndColor"/>.
        /// </summary>
        /// <param name="state">Affected control state.</param>
        /// <param name="fontAndColor">Colors.</param>
        public virtual void SetStateColors(
            GenericControlState state,
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
        public override Brush? GetBackground(GenericControlState state)
        {
            var overrideValue = Backgrounds?.GetObjectOrNull(state);
            if (overrideValue is not null)
                return overrideValue;

            var result = GetDefaultTheme()?.DarkOrLight(IsDarkBackground);
            return result?.Backgrounds?.GetObjectOrNull(state);
        }

        /// <summary>
        /// Default painting method of the <see cref="UserControl"/>
        /// and its descendants.
        /// </summary>
        /// <param name="dc">Drawing Context.</param>
        /// <param name="rect">Rectangle to draw in.</param>
        public virtual void DefaultPaint(Graphics dc, RectD rect)
        {
        }

        /// <inheritdoc/>
        public override BorderSettings? GetBorderSettings(GenericControlState state)
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
        /// <param name="dc">Drawing context.</param>
        /// <param name="rect">Ractangle.</param>
        public virtual void DrawDefaultBackground(Graphics dc, RectD rect)
        {
            var state = CurrentState;
            var brush = GetBackground(state);
            var border = GetBorderSettings(state);

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
            /*Application.Log($"{GetType()}.OnMouseLeftButtonDown");*/
            base.OnMouseLeftButtonDown(e);
            if (!Enabled)
                return;
            /*Application.Log($"{GetType()}.OnMouseLeftButtonDown 2");*/
            RaiseClick(EventArgs.Empty);
            ShowDropDownMenu();
            Invalidate();
        }

        /// <summary>
        /// Shows attached drop down menu.
        /// </summary>
        protected virtual void ShowDropDownMenu()
        {
            if (DropDownMenu is null)
                return;
            PointD pt = (0, Bounds.Height);
            this.ShowPopupMenu(DropDownMenu, pt.X, pt.Y);
        }

        /// <inheritdoc/>
        protected override void OnPaint(PaintEventArgs e)
        {
            DefaultPaint(e.Graphics, e.ClipRectangle);
        }

        /// <inheritdoc/>
        protected override void OnCurrentStateChanged(EventArgs e)
        {
            base.OnCurrentStateChanged(e);

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

        private void SetScrollbars(bool horz, bool vert, bool always)
        {
            NativeControl.ShowHorzScrollBar = horz;
            NativeControl.ShowVertScrollBar = vert;
            NativeControl.ScrollBarAlwaysVisible = always;
        }
    }
}
