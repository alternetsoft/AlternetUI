using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a scrollable area that can contain other visible elements.
    /// </summary>
    [ControlCategory("Containers")]
    public partial class ScrollViewer : Control
    {
        private bool settingLayoutOffset;
        private bool scrollInfoValid;
        private SizeD layoutOffset;

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.ScrollViewer;

        /// <inheritdoc/>
        public override void OnLayout()
        {
            LayoutCore();

            if (!settingLayoutOffset && !scrollInfoValid)
            {
                SetScrollInfo();
                LayoutCore();
            }
        }

        /// <inheritdoc/>
        internal override ControlHandler CreateHandler()
        {
            return GetEffectiveControlHandlerHactory().
                CreateScrollViewerHandler(this);
        }

        private void LayoutCore()
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
                    LayoutFactory.AlignHorizontal(
                        childrenLayoutBounds,
                        control,
                        boundedPreferredSize,
                        horizontalAlignment);
                var verticalPosition =
                    LayoutFactory.AlignVertical(
                        childrenLayoutBounds,
                        control,
                        boundedPreferredSize,
                        verticalAlignment);

                var offset = layoutOffset;

                control.Handler.Bounds = new RectD(
                    horizontalPosition.Origin + offset.Width,
                    verticalPosition.Origin + offset.Height,
                    horizontalPosition.Size,
                    verticalPosition.Size);
            }
        }

        private void SetScrollInfo()
        {
            var preferredSize = Handler.GetChildrenMaxPreferredSizePadded(
                new SizeD(double.PositiveInfinity, double.PositiveInfinity));
            var size = ClientRectangle.Size;

            if (preferredSize.Width <= size.Width)
                NativeControl.SetScrollBar(Native.ScrollBarOrientation.Horizontal, false, 0, 0, 0);
            else
            {
                NativeControl.SetScrollBar(
                    Native.ScrollBarOrientation.Horizontal,
                    true,
                    0,
                    (int)size.Width,
                    (int)preferredSize.Width);
            }

            if (preferredSize.Height <= size.Height)
                NativeControl.SetScrollBar(Native.ScrollBarOrientation.Vertical, false, 0, 0, 0);
            else
            {
                NativeControl.SetScrollBar(
                    Native.ScrollBarOrientation.Vertical,
                    true,
                    0,
                    (int)size.Height,
                    (int)preferredSize.Height);
            }

            layoutOffset = SizeD.Empty;

            scrollInfoValid = true;
        }

        internal class NativeScrollViewerHandler : ScrollViewerHandler
        {
            public new Native.Panel NativeControl => (Native.Panel)base.NativeControl!;

            internal override Native.Control CreateNativeControl()
            {
                return new Native.Panel();
            }

            protected internal override void OnLayoutChanged()
            {
                base.OnLayoutChanged();
                Control.scrollInfoValid = false;
            }

            protected override void OnAttach()
            {
                Control.scrollInfoValid = false;

                base.OnAttach();

                NativeControl.IsScrollable = true;
                NativeControl.VerticalScrollBarValueChanged =
                    NativeControl_VerticalScrollBarValueChanged;
                NativeControl.HorizontalScrollBarValueChanged =
                    NativeControl_HorizontalScrollBarValueChanged;
            }

            protected override void OnDetach()
            {
                base.OnDetach();

                NativeControl.VerticalScrollBarValueChanged = null;
                NativeControl.HorizontalScrollBarValueChanged = null;
            }

            private void NativeControl_HorizontalScrollBarValueChanged()
            {
                Control.settingLayoutOffset = true;
                Control.layoutOffset = new SizeD(
                    -NativeControl.GetScrollBarValue(Native.ScrollBarOrientation.Horizontal),
                    Control.layoutOffset.Height);
                Control.PerformLayout();
                Control.settingLayoutOffset = false;
            }

            private void NativeControl_VerticalScrollBarValueChanged()
            {
                Control.settingLayoutOffset = true;
                Control.layoutOffset = new SizeD(
                    Control.layoutOffset.Width,
                    -NativeControl.GetScrollBarValue(Native.ScrollBarOrientation.Vertical));
                Control.PerformLayout();
                Control.settingLayoutOffset = false;
            }
        }
    }
}