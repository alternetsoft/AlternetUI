using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Arranges child controls into a single line that can be oriented
    /// horizontally or vertically.
    /// </summary>
    [ControlCategory("Containers")]
    public partial class StackPanel : Control
    {
        private StackPanelOrientation orientation;

        /// <summary>
        /// Occurs when the value of the <see cref="Orientation"/> property changes.
        /// </summary>
        public event EventHandler? OrientationChanged;

        /// <summary>
        /// Gets or sets a value that indicates the dimension by which child
        /// controls are stacked.
        /// </summary>
        public virtual StackPanelOrientation Orientation
        {
            get => orientation;

            set
            {
                if (orientation == value)
                    return;

                orientation = value;

                OnOrientationChanged(EventArgs.Empty);
                PerformLayout();
                RaiseLayoutChanged();
            }
        }

        /// <summary>
        /// Gets or sets whether <see cref="Orientation"/> is horizontal.
        /// </summary>
        [Browsable(false)]
        public bool IsHorizontal
        {
            get => Orientation == StackPanelOrientation.Horizontal;
            set => Orientation = StackPanelOrientation.Horizontal;
        }

        /// <summary>
        /// Gets or sets whether <see cref="Orientation"/> is vertical.
        /// </summary>
        [Browsable(false)]
        public bool IsVertical
        {
            get => Orientation == StackPanelOrientation.Vertical;
            set => Orientation = StackPanelOrientation.Vertical;
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.StackPanel;

        internal static SizeD GetPreferredSizeHorizontalStackPanel(Control container, SizeD availableSize)
        {
            var stackPanelPadding = container.Padding;

            double width = 0;
            double maxHeight = 0;
            foreach (var control in container.AllChildrenInLayout)
            {
                var margin = control.Margin;
                var preferredSize = control.GetPreferredSizeLimited(
                    new SizeD(availableSize.Width - width, availableSize.Height));
                width += preferredSize.Width + margin.Horizontal;
                maxHeight = Math.Max(maxHeight, preferredSize.Height + margin.Vertical);
            }

            var isNan = double.IsNaN(container.SuggestedHeight);

            return new SizeD(
                width + stackPanelPadding.Horizontal,
                isNan ? maxHeight + stackPanelPadding.Vertical : container.SuggestedHeight);
        }

        internal static SizeD GetPreferredSizeStackPanel(
            Control container,
            SizeD availableSize,
            bool isVertical)
        {
            if (isVertical)
                return StackPanel.GetPreferredSizeVerticalStackPanel(container, availableSize);
            else
                return StackPanel.GetPreferredSizeHorizontalStackPanel(container, availableSize);
        }

        internal static SizeD GetPreferredSizeVerticalStackPanel(
            Control container,
            SizeD availableSize)
        {
            var padding = container.Padding;

            double maxWidth = 0;
            double height = 0;
            foreach (var control in container.AllChildrenInLayout)
            {
                var margin = control.Margin;
                var preferredSize = control.GetPreferredSizeLimited(
                    new SizeD(availableSize.Width, availableSize.Height - height));
                maxWidth = Math.Max(maxWidth, preferredSize.Width + margin.Horizontal);
                height += preferredSize.Height + margin.Vertical;
            }

            var isNan = double.IsNaN(container.SuggestedWidth);
            var newWidth = isNan ? maxWidth + padding.Horizontal : container.SuggestedWidth;
            return new SizeD(newWidth, height + padding.Vertical);
        }

        internal static void LayoutHorizontalStackPanel(Control container)
        {
            var controls = container.AllChildrenInLayout;
            var childrenLayoutBounds = container.ChildrenLayoutBounds;

            double x = 0;
            double w = 0;

            Stack<Control> rightControls = new();
            List<(Control Control, double Top, SizeD Size)>? centerControls = null;

            foreach (var control in controls)
            {
                bool isRight = control.HorizontalAlignment == UI.HorizontalAlignment.Right;
                if (isRight)
                    rightControls.Push(control);
                else
                    DoAlignControl(control);
            }

            foreach (var control in rightControls)
            {
                DoAlignControl(control);
            }

            if (centerControls is not null)
                AlignCenterControls();

            void AlignCenterControls()
            {
                double totalWidth = 0;
                foreach(var item in centerControls)
                    totalWidth += item.Size.Width;
                var offset = (childrenLayoutBounds.Width - totalWidth) / 2;

                foreach (var item in centerControls)
                {
                    var margin = item.Control.Margin;
                    item.Control.Bounds =
                        new RectD(
                            childrenLayoutBounds.Left + margin.Left + offset,
                            item.Top,
                            item.Size.Width,
                            item.Size.Height);
                    offset += item.Size.Width + margin.Horizontal;
                }
            }

            void DoAlignControl(Control control)
            {
                var margin = control.Margin;
                var horizontalMargin = margin.Horizontal;

                var preferredSize = control.GetPreferredSizeLimited(
                    new SizeD(
                        childrenLayoutBounds.Width - x - horizontalMargin - w,
                        childrenLayoutBounds.Height));
                var alignedPosition =
                    LayoutFactory.AlignVertical(
                        childrenLayoutBounds,
                        control,
                        preferredSize,
                        control.VerticalAlignment);

                switch (control.HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                    case HorizontalAlignment.Fill:
                    case HorizontalAlignment.Stretch:
                    default:
                        control.Bounds =
                            new RectD(
                                childrenLayoutBounds.Left + x + margin.Left,
                                alignedPosition.Origin,
                                preferredSize.Width,
                                alignedPosition.Size);
                        x += preferredSize.Width + horizontalMargin;
                        break;
                    case HorizontalAlignment.Center:
                        centerControls ??= new();
                        var item = (
                            control,
                            alignedPosition.Origin,
                            (preferredSize.Width, alignedPosition.Size));
                        centerControls.Add(item);
                        break;
                    case HorizontalAlignment.Right:
                        control.Bounds =
                            new RectD(
                                childrenLayoutBounds.Right - w - margin.Right - preferredSize.Width,
                                alignedPosition.Origin,
                                preferredSize.Width,
                                alignedPosition.Size);
                        w += preferredSize.Width + horizontalMargin;
                        break;
                }
            }
        }

        internal static void LayoutStackPanel(Control container, bool isVertical)
        {
            if (isVertical)
                StackPanel.LayoutVerticalStackPanel(container);
            else
                StackPanel.LayoutHorizontalStackPanel(container);
        }

        internal static void LayoutVerticalStackPanel(Control container)
        {
            var lBounds = container.ChildrenLayoutBounds;
            var items = container.AllChildrenInLayout;

            double stretchedSize = 0;

            if (items.Count > 0)
            {
                foreach (var control in items)
                {
                    if (control.VerticalAlignment == UI.VerticalAlignment.Fill)
                        continue;

                    var verticalMargin = control.Margin.Vertical;
                    var freeSize = new SizeD(
                            lBounds.Width,
                            lBounds.Height - stretchedSize - verticalMargin);
                    var preferredSize = control.GetPreferredSizeLimited(freeSize);
                    stretchedSize += preferredSize.Height + verticalMargin;
                }
            }

            stretchedSize = lBounds.Height - stretchedSize;

            double y = 0;
            double h = 0;
            int bottomCount = 0;

            foreach (var control in items)
            {
                if (control.VerticalAlignment == UI.VerticalAlignment.Bottom)
                {
                    bottomCount++;
                    continue;
                }

                DoAlignControl(control);
            }

            if (bottomCount > 0)
            {
                for (int i = items.Count - 1; i >= 0; i--)
                {
                    var control = items[i];
                    if (control.VerticalAlignment != UI.VerticalAlignment.Bottom)
                        continue;
                    DoAlignControl(control);
                    bottomCount--;
                    if (bottomCount == 0)
                        break;
                }
            }

            void DoAlignControl(Control control)
            {
                var stretch = control.VerticalAlignment == UI.VerticalAlignment.Fill;

                bool bottom = control.VerticalAlignment == UI.VerticalAlignment.Bottom;

                var margin = control.Margin;
                var vertMargin = margin.Vertical;

                var freeSize = new SizeD(
                    lBounds.Width,
                    lBounds.Height - y - vertMargin - h);
                var preferSize = control.GetPreferredSizeLimited(freeSize);
                if (stretch)
                    preferSize.Height = stretchedSize - vertMargin;
                var alignedPos = LayoutFactory.AlignHorizontal(
                    lBounds,
                    control,
                    preferSize,
                    control.HorizontalAlignment);
                if (bottom)
                {
                    control.Handler.Bounds =
                        new RectD(
                            alignedPos.Origin,
                            lBounds.Bottom - h - margin.Bottom - preferSize.Height,
                            alignedPos.Size,
                            preferSize.Height);
                    h += preferSize.Height + vertMargin;
                }
                else
                {
                    control.Handler.Bounds =
                        new RectD(
                            alignedPos.Origin,
                            lBounds.Top + y + margin.Top,
                            alignedPos.Size,
                            preferSize.Height);
                    y += preferSize.Height + vertMargin;
                }
            }
        }

        /// <inheritdoc/>
        protected override LayoutStyle GetDefaultLayout()
        {
            if (IsVertical)
                return LayoutStyle.Vertical;
            else
                return LayoutStyle.Horizontal;
        }

        /// <summary>
        /// Called when the value of the <see cref="Orientation"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnOrientationChanged(EventArgs e) =>
            OrientationChanged?.Invoke(this, e);
    }
}