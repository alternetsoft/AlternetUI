using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public partial class Control
    {
        internal virtual void OnHandlerDpiChanged()
        {
            var oldDpi = Handler.EventOldDpi;
            var newDpi = Handler.EventNewDpi;

            var e = new DpiChangedEventArgs(oldDpi, newDpi);
            RaiseDpiChanged(e);
        }

        internal virtual void OnHandlerVerticalScrollBarValueChanged()
        {
            var args = new ScrollEventArgs
            {
                ScrollOrientation = ScrollBarOrientation.Vertical,
                NewValue = Handler.GetScrollBarEvtPosition(),
                Type = Handler.GetScrollBarEvtKind(),
            };
            RaiseScroll(args);
        }

        internal virtual void OnHandlerVisibleChanged()
        {
            bool visible = Handler.Visible;
            Visible = visible;

            if (App.IsLinuxOS && visible)
            {
                // todo: this is a workaround for a problem on Linux when
                // ClientSize is not reported correctly until the window is shown
                // So we need to relayout all after the proper client size is available
                // This should be changed later in respect to RedrawOnResize functionality.
                // Also we may need to do this for top-level windows.
                // Doing this on Windows results in strange glitches like disappearing
                // tab controls' tab.
                // See https://forums.wxwidgets.org/viewtopic.php?f=1&t=47439
                PerformLayout();
            }
        }

        internal virtual void OnHandlerHorizontalScrollBarValueChanged()
        {
            var args = new ScrollEventArgs
            {
                ScrollOrientation = ScrollBarOrientation.Horizontal,
                NewValue = Handler.GetScrollBarEvtPosition(),
                Type = Handler.GetScrollBarEvtKind(),
            };
            RaiseScroll(args);
        }

        internal virtual void OnHandlerPaint()
        {
            if (!UserPaint)
                return;

            using var dc = Handler.OpenPaintDrawingContext();

            RaisePaint(new PaintEventArgs(dc, ClientRectangle));
        }
    }
}
