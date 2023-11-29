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
    public class UserPaintControl : Control
    {
        private bool hasBorder = true; // !! to border settings

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
        [Browsable(true)]
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
        /// Gets or sets whether to draw debug related points for the owner draw controls.
        /// </summary>
#if DEBUG
        [Browsable(true)]
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

        internal virtual void BeforePaint(DrawingContext dc, Rect rect)
        {
#if DEBUG
            if (DrawDebugPointsBefore)
                dc.DrawDebugPoints(rect, Pens.Yellow);
#endif
        }

        internal virtual void DefaultPaint(DrawingContext dc, Rect rect)
        {
        }

        internal virtual void AfterPaint(DrawingContext dc, Rect rect)
        {
#if DEBUG
            if (DrawDebugPointsAfter)
                dc.DrawDebugPoints(rect);
#endif
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

        private class UserPaintHandler : ControlHandler<UserPaintControl>
        {
            protected override bool NeedsPaint => true;

            public override void OnPaint(DrawingContext dc)
            {
                var r = DrawClientRectangle;
                Control.BeforePaint(dc, r);
                Control.DefaultPaint(dc, r);
                Control.AfterPaint(dc, r);
            }
        }
    }
}
