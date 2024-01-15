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
    public class UserPaintControl : LayoutPanel
    {
        private bool hasBorder = true; // !! to border settings
        private RichTextBoxScrollBars scrollBars = RichTextBoxScrollBars.None;
        private Caret? caret;

#if DEBUG
        private bool drawDebugPointsBefore;
        private bool drawDebugPointsAfter;
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="UserPaintControl"/> class.
        /// </summary>
        public UserPaintControl()
            : base()
        {
            UserPaint = true;
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.UserPaintControl;

        /// <summary>
        /// Gets or sets whether to draw debug related points for the owner draw controls.
        /// </summary>
#if DEBUG
        [Browsable(false)]
#else
        [Browsable(false)]
#endif
        public bool DrawDebugPointsBefore
        {
            get
            {
#if DEBUG
                return drawDebugPointsBefore;
#else
                return false;
#endif
            }

            set
            {
#if DEBUG
                if (drawDebugPointsBefore == value)
                    return;
                drawDebugPointsBefore = value;
                Refresh();
#else
#endif
            }
        }

        /// <summary>
        /// Gets or sets <see cref="ContextMenu"/> which is shown when control is clicked.
        /// </summary>
        public ContextMenu? DropDownMenu { get; set; }

        /// <summary>
        /// Gets or sets the type of scroll bars displayed in the control.
        /// </summary>
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
        /// Gets or sets whether to draw debug related points for the owner draw controls.
        /// </summary>
#if DEBUG
        [Browsable(false)]
#else
        [Browsable(false)]
#endif
        public bool DrawDebugPointsAfter
        {
            get
            {
#if DEBUG
                return drawDebugPointsAfter;
#else
                return false;
#endif
            }

            set
            {
#if DEBUG
                if (drawDebugPointsAfter == value)
                    return;
                drawDebugPointsAfter = value;
                Refresh();
#else
#endif
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
        public virtual void SetStateColors(GenericControlState state, IReadOnlyFontAndColor? fontAndColor)
        {
            if (fontAndColor is null && StateObjects?.Colors is null && StateObjects?.Backgrounds is null)
                return;
            StateObjects ??= new();
            StateObjects.Colors ??= new();
            StateObjects.Backgrounds ??= new();
            StateObjects.Colors.SetObject(fontAndColor, state);
            StateObjects.Backgrounds.SetObject(fontAndColor?.BackgroundColor?.AsBrush, state);
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

            var border = Borders?.GetObjectOrNormal(state);

            var radius = border?.GetUniformCornerRadius(rect);

            if (radius is not null && brush is not null)
            {
                var color = border?.Color;
                if (border is null || color is null)
                {
                    dc.FillRoundedRectangle(brush, rect.InflatedBy(-1, -1), radius.Value);
                }
                else
                    dc.RoundedRectangle(color.AsPen, brush, rect.InflatedBy(-1, -1), radius.Value);
                return;
            }

            if (brush != null)
            {
                dc.FillRectangle(brush, rect);
            }

            if (HasBorder)
            {
                border?.Draw(dc, rect);
            }
        }

        internal virtual void BeforePaint(Graphics dc, RectD rect)
        {
#if DEBUG
            if (DrawDebugPointsBefore)
                dc.DrawDebugPoints(rect, Pens.Yellow);
#endif
        }

        internal virtual void AfterPaint(Graphics dc, RectD rect)
        {
#if DEBUG
            if (DrawDebugPointsAfter)
                dc.DrawDebugPoints(rect);
#endif
        }

        /// <inheritdoc/>
        protected override void OnMouseLeftButtonDown(MouseEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (e.Handled)
                return;
            RaiseClick(EventArgs.Empty);
            if (DropDownMenu is null)
                return;
            e.Handled = true;
            PointD pt = (0, Bounds.Height);
            this.ShowPopupMenu(DropDownMenu, pt.X, pt.Y);
            Invalidate();
        }

        /// <summary>
        /// Default painting method of the <see cref="UserPaintControl"/>
        /// and its descendants.
        /// </summary>
        /// <param name="dc">Drawing Context.</param>
        /// <param name="rect">Rectangle to draw in.</param>
        protected virtual void DefaultPaint(Graphics dc, RectD rect)
        {
        }

        /// <inheritdoc/>
        protected override void OnPaint(PaintEventArgs e)
        {
        }

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return new UserPaintHandler();
        }

        private void SetScrollbars(bool horz, bool vert, bool always)
        {
            NativeControl.ShowHorzScrollBar = horz;
            NativeControl.ShowVertScrollBar = vert;
            NativeControl.ScrollBarAlwaysVisible = always;
        }

        private class UserPaintHandler : NativeControlHandler<UserPaintControl, Native.Panel>
        {
            protected override bool NeedsPaint => true;

            public override void OnPaint(Graphics dc)
            {
                var r = DrawClientRectangle;
                Control.BeforePaint(dc, r);
                Control.DefaultPaint(dc, r);
                Control.AfterPaint(dc, r);
            }
        }
    }
}
