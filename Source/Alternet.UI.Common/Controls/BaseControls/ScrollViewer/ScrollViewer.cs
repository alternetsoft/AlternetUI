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

            var offset = GetScrollBarInfo(e.IsVertical).Position;

            if (e.IsVertical)
                LayoutOffset = new PointD(layoutOffset.X, -offset);
            else
                LayoutOffset = new PointD(-offset, layoutOffset.Y);
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
                new SizeD(Coord.PositiveInfinity, Coord.PositiveInfinity));
            var size = ClientRectangle.Size;

            if (preferredSize.Width <= size.Width)
                HorzScrollBarInfo = HorzScrollBarInfo.WithVisibility(HiddenOrVisible.Hidden);
            else
            {
                ScrollBarInfo horz = new()
                {
                    Visibility = HiddenOrVisible.Auto,
                    Range = (int)preferredSize.Width,
                    PageSize = (int)size.Width,
                    Position = GetScrollBarValue(false),
                };

                HorzScrollBarInfo = horz;
            }

            if (preferredSize.Height <= size.Height)
                VertScrollBarInfo = VertScrollBarInfo.WithVisibility(HiddenOrVisible.Hidden);
            else
            {
                ScrollBarInfo vert = new()
                {
                    Visibility = HiddenOrVisible.Auto,
                    Range = (int)preferredSize.Height,
                    PageSize = (int)size.Height,
                    Position = GetScrollBarValue(true),
                };

                VertScrollBarInfo = vert;
            }
        }
    }
}