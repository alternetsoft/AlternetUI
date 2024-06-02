using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class Control
    {
        /// <summary>
        /// Enumerates known handler types.
        /// </summary>
        public enum HandlerType
        {
            /// <summary>
            /// Native handler type.
            /// </summary>
            Native,

            /// <summary>
            /// Generic type.
            /// </summary>
            Generic,
        }

        public static AlignedPosition AlignHorizontal(
                    RectD layoutBounds,
                    Control childControl,
                    SizeD childPreferredSize,
                    HorizontalAlignment alignment)
        {
            switch (alignment)
            {
                case HorizontalAlignment.Left:
                    return new AlignedPosition(
                        layoutBounds.Left + childControl.Margin.Left,
                        childPreferredSize.Width);
                case HorizontalAlignment.Center:
                    return new AlignedPosition(
                        layoutBounds.Left +
                        ((layoutBounds.Width
                        - (childPreferredSize.Width + childControl.Margin.Horizontal)) / 2) +
                        childControl.Margin.Left,
                        childPreferredSize.Width);
                case HorizontalAlignment.Right:
                    return new AlignedPosition(
                        layoutBounds.Right -
                        childControl.Margin.Right - childPreferredSize.Width,
                        childPreferredSize.Width);
                case HorizontalAlignment.Stretch:
                default:
                    return new AlignedPosition(
                        layoutBounds.Left + childControl.Margin.Left,
                        layoutBounds.Width - childControl.Margin.Horizontal);
            }
        }

        public static AlignedPosition AlignVertical(
            RectD layoutBounds,
            Control control,
            SizeD childPreferredSize,
            VerticalAlignment alignment)
        {
            switch (alignment)
            {
                case VerticalAlignment.Top:
                    return new AlignedPosition(
                        layoutBounds.Top + control.Margin.Top,
                        childPreferredSize.Height);
                case VerticalAlignment.Center:
                    return new AlignedPosition(
                        layoutBounds.Top +
                        ((layoutBounds.Height
                        - (childPreferredSize.Height + control.Margin.Vertical)) / 2) +
                        control.Margin.Top,
                        childPreferredSize.Height);
                case VerticalAlignment.Bottom:
                    return new AlignedPosition(
                        layoutBounds.Bottom - control.Margin.Bottom - childPreferredSize.Height,
                        childPreferredSize.Height);
                case VerticalAlignment.Stretch:
                default:
                    return new AlignedPosition(
                        layoutBounds.Top + control.Margin.Top,
                        layoutBounds.Height - control.Margin.Vertical);
            }
        }

        internal static void NotifyCaptureLost()
        {
            App.Handler.NotifyCaptureLost();
        }

        internal static void PerformDefaultLayout(
            Control container,
            RectD childrenLayoutBounds,
            IReadOnlyList<Control> childs)
        {
            foreach (var control in childs)
            {
                var preferredSize = control.GetPreferredSizeLimited(childrenLayoutBounds.Size);

                var horizontalPosition =
                    AlignHorizontal(
                        childrenLayoutBounds,
                        control,
                        preferredSize,
                        control.HorizontalAlignment);
                var verticalPosition =
                    AlignVertical(
                        childrenLayoutBounds,
                        control,
                        preferredSize,
                        control.VerticalAlignment);

                control.Bounds = new RectD(
                    horizontalPosition.Origin,
                    verticalPosition.Origin,
                    horizontalPosition.Size,
                    verticalPosition.Size);
            }
        }

        internal static SizeD GetPreferredSizeDefaultLayout(Control container, SizeD availableSize)
        {
            if (container.HasChildren)
                return container.GetSpecifiedOrChildrenPreferredSize(availableSize);
            return container.GetNativeControlSize(availableSize);
        }

        internal static Color GetClassDefaultAttributesBgColor(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            return SystemSettings.Handler.GetClassDefaultAttributesBgColor(controlType, renderSize);
        }

        internal static Color GetClassDefaultAttributesFgColor(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            return SystemSettings.Handler.GetClassDefaultAttributesFgColor(controlType, renderSize);
        }

        internal static Font? GetClassDefaultAttributesFont(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            return SystemSettings.Handler.GetClassDefaultAttributesFont(controlType, renderSize);
        }

        /// <summary>
        /// Forces the re-creation of the handler for the control.
        /// </summary>
        /// <remarks>
        /// The <see cref="RecreateHandler"/> method is called whenever
        /// re-execution of handler creation logic is needed.
        /// For example, this may happen when visual theme changes.
        /// </remarks>
        internal void RecreateHandler()
        {
            if (handler != null)
                DetachHandler();

            Invalidate();
        }

        internal void RaiseProcessException(ControlExceptionEventArgs e)
        {
            OnProcessException(e);
            ProcessException?.Invoke(this, e);
        }

        internal void SetParentInternal(Control? value)
        {
            parent = value;
            LogicalParent = value;
        }

        internal virtual SizeD GetPreferredSizeLimited(SizeD availableSize)
        {
            var result = GetPreferredSize(availableSize);
            var minSize = MinimumSize;
            var maxSize = MaximumSize;
            var preferredSize = result.ApplyMinMax(minSize, maxSize);
            return preferredSize;
        }

        internal void SendMouseDownEvent(int x, int y)
        {
            Handler.SendMouseDownEvent(x, y);
        }

        internal void SendMouseUpEvent(int x, int y)
        {
            Handler.SendMouseUpEvent(x, y);
        }

        internal bool BeginRepositioningChildren()
        {
            return Handler.BeginRepositioningChildren();
        }

        internal void EndRepositioningChildren()
        {
            Handler.EndRepositioningChildren();
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
                ScrollOrientation = ScrollOrientation.HorizontalScroll,
                NewValue = Handler.GetScrollBarEvtPosition(),
                Type = Handler.GetScrollBarEvtKind(),
            };
            RaiseScroll(args);
        }

        internal virtual void OnHandlerVerticalScrollBarValueChanged()
        {
            var args = new ScrollEventArgs
            {
                ScrollOrientation = ScrollOrientation.VerticalScroll,
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

        internal void DoInsideRepositioningChildren(Action action)
        {
            var repositioning = BeginRepositioningChildren();
            if (repositioning)
            {
                try
                {
                    action();
                }
                finally
                {
                    EndRepositioningChildren();
                }
            }
            else
                action();
        }

        public class AlignedPosition
        {
            public AlignedPosition(double origin, double size)
            {
                Origin = origin;
                Size = size;
            }

            public double Origin { get; }

            public double Size { get; }
        }
    }
}
