using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a scrollable area that can contain other visible elements.
    /// </summary>
    [ControlCategory("Containers")]
    public class ScrollViewer : Control
    {
        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.ScrollViewer;

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return GetEffectiveControlHandlerHactory().
                CreateScrollViewerHandler(this);
        }

        internal class NativeScrollViewerHandler : ScrollViewerHandler
        {
            private bool settingLayoutOffset;
            private bool scrollInfoValid;

            public new Native.Panel NativeControl => (Native.Panel)base.NativeControl!;

            private SizeD LayoutOffset { get; set; }

            public override void OnLayout()
            {
                LayoutCore();

                if (!settingLayoutOffset && !scrollInfoValid)
                {
                    SetScrollInfo();
                    LayoutCore();
                }
            }

            internal override Native.Control CreateNativeControl()
            {
                return new Native.Panel();
            }

            protected override void OnAttach()
            {
                scrollInfoValid = false;

                base.OnAttach();

                NativeControl.IsScrollable = true;
                NativeControl.VerticalScrollBarValueChanged +=
                    NativeControl_VerticalScrollBarValueChanged;
                NativeControl.HorizontalScrollBarValueChanged +=
                    NativeControl_HorizontalScrollBarValueChanged;
            }

            protected override void OnDetach()
            {
                base.OnDetach();

                NativeControl.VerticalScrollBarValueChanged -=
                    NativeControl_VerticalScrollBarValueChanged;
                NativeControl.HorizontalScrollBarValueChanged -=
                    NativeControl_HorizontalScrollBarValueChanged;
            }

            protected override void OnLayoutChanged()
            {
                base.OnLayoutChanged();
                scrollInfoValid = false;
            }

            private void LayoutCore()
            {
                var childrenLayoutBounds = Control.ChildrenLayoutBounds;
                foreach (var control in AllChildrenIncludedInLayout)
                {
                    var boundedPreferredSize = control.GetPreferredSize(childrenLayoutBounds.Size);
                    var unboundedPreferredSize =
                        control.GetPreferredSize(
                            new SizeD(double.PositiveInfinity, double.PositiveInfinity));

                    var verticalAlignment = control.VerticalAlignment;
                    var horizontalAlignment = control.HorizontalAlignment;

                    if (unboundedPreferredSize.Width > childrenLayoutBounds.Width)
                    {
                        horizontalAlignment = HorizontalAlignment.Left;
                    }

                    if (unboundedPreferredSize.Height > childrenLayoutBounds.Height)
                    {
                        verticalAlignment = VerticalAlignment.Top;
                    }

                    var horizontalPosition =
                        AlignedLayout.AlignHorizontal(
                            childrenLayoutBounds,
                            control,
                            boundedPreferredSize,
                            horizontalAlignment);
                    var verticalPosition =
                        AlignedLayout.AlignVertical(
                            childrenLayoutBounds,
                            control,
                            boundedPreferredSize,
                            verticalAlignment);

                    var offset = LayoutOffset;

                    control.Handler.Bounds = new RectD(
                        horizontalPosition.Origin + offset.Width,
                        verticalPosition.Origin + offset.Height,
                        horizontalPosition.Size,
                        verticalPosition.Size);
                }
            }

            private void SetScrollInfo()
            {
                var preferredSize = GetChildrenMaxPreferredSizePadded(
                    new SizeD(double.PositiveInfinity, double.PositiveInfinity));
                var size = Control.ClientRectangle.Size;

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

                LayoutOffset = SizeD.Empty;

                scrollInfoValid = true;
            }

            private void NativeControl_HorizontalScrollBarValueChanged(object? sender, EventArgs e)
            {
                settingLayoutOffset = true;
                LayoutOffset = new SizeD(
                    -NativeControl.GetScrollBarValue(Native.ScrollBarOrientation.Horizontal),
                    LayoutOffset.Height);
                Control.PerformLayout();
                settingLayoutOffset = false;
            }

            private void NativeControl_VerticalScrollBarValueChanged(object? sender, EventArgs e)
            {
                settingLayoutOffset = true;
                LayoutOffset = new SizeD(
                    LayoutOffset.Width,
                    -NativeControl.GetScrollBarValue(Native.ScrollBarOrientation.Vertical));
                Control.PerformLayout();
                settingLayoutOffset = false;
            }
        }
    }
}