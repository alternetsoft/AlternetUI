using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a scrollable area that can contain other visible elements.
    /// </summary>
    [ControlCategory("Containers")]
    public partial class ScrollViewer : ContainerControl
    {
        private PointD layoutOffset;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollViewer"/> class.
        /// </summary>
        public ScrollViewer()
        {
            IsScrollable = true;
        }

        /// <summary>
        /// Gets or sets offset of the layout.
        /// </summary>
        public PointD LayoutOffset
        {
            get => layoutOffset;

            set
            {
                if (layoutOffset == value)
                    return;
                layoutOffset = value;
                PerformLayout();
            }
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.ScrollViewer;

        /// <inheritdoc/>
        public override void OnLayout()
        {
            SetScrollInfo();
            LayoutCore();
        }

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return ControlFactory.Handler.CreateScrollViewerHandler(this);
        }

        /// <inheritdoc/>
        protected override void OnScroll(ScrollEventArgs e)
        {
            base.OnScroll(e);
            if (e.IsVertical)
                LayoutOffset = new PointD(layoutOffset.X, -GetScrollBarValue(isVertical: true));
            else
                LayoutOffset = new PointD(-GetScrollBarValue(isVertical: false), layoutOffset.Y);
        }

        /// <summary>
        /// Core layout method.
        /// </summary>
        protected virtual void LayoutCore()
        {
            var childrenLayoutBounds = ChildrenLayoutBounds;
            foreach (var control in AllChildrenInLayout)
            {
                var boundedPreferredSize = control.GetPreferredSize(childrenLayoutBounds.Size);
                var unboundedPreferredSize =
                    control.GetPreferredSize(
                        new SizeD(double.PositiveInfinity, double.PositiveInfinity));

                var verticalAlignment = control.VerticalAlignment;
                var horizontalAlignment = control.HorizontalAlignment;

                if (unboundedPreferredSize.Width > childrenLayoutBounds.Width)
                {
                    horizontalAlignment = UI.HorizontalAlignment.Left;
                }

                if (unboundedPreferredSize.Height > childrenLayoutBounds.Height)
                {
                    verticalAlignment = UI.VerticalAlignment.Top;
                }

                var horizontalPosition =
                    AlignHorizontal(
                        childrenLayoutBounds,
                        control,
                        boundedPreferredSize,
                        horizontalAlignment);
                var verticalPosition =
                    AlignVertical(
                        childrenLayoutBounds,
                        control,
                        boundedPreferredSize,
                        verticalAlignment);

                control.Bounds = new RectD(
                    horizontalPosition.Origin + layoutOffset.X,
                    verticalPosition.Origin + layoutOffset.Y,
                    horizontalPosition.Size,
                    verticalPosition.Size);
            }
        }

        /// <summary>
        /// Updates scrollbars.
        /// </summary>
        protected virtual void SetScrollInfo()
        {
            var preferredSize = GetChildrenMaxPreferredSizePadded(
                new SizeD(double.PositiveInfinity, double.PositiveInfinity));
            var size = ClientRectangle.Size;

            if (preferredSize.Width <= size.Width)
                SetScrollBar(isVertical: false, false, 0, 0, 0);
            else
            {
                SetScrollBar(
                    isVertical: false,
                    visible: true,
                    GetScrollBarValue(false),
                    (int)size.Width,
                    (int)preferredSize.Width);
            }

            if (preferredSize.Height <= size.Height)
                SetScrollBar(isVertical: true, false, 0, 0, 0);
            else
            {
                SetScrollBar(
                    isVertical: true,
                    visible: true,
                    value: GetScrollBarValue(true),
                    largeChange: (int)size.Height,
                    maximum: (int)preferredSize.Height);
            }
        }
    }
}