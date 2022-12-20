using System;

namespace Alternet.UI
{
    internal class NativeScrollViewerHandler : ScrollViewerHandler
    {
        internal override Native.Control CreateNativeControl()
        {
            return new Native.Panel();
        }

        public new Native.Panel NativeControl => (Native.Panel)base.NativeControl!;

        protected override void OnAttach()
        {
            base.OnAttach();

            NativeControl.SetScrollBar(Native.ScrollBarOrientation.Vertical, true, 10, 100, 1000);
            NativeControl.SetScrollBar(Native.ScrollBarOrientation.Horizontal, true, 10, 100, 1000);

            NativeControl.VerticalScrollBarValueChanged += NativeControl_VerticalScrollBarValueChanged;
            NativeControl.HorizontalScrollBarValueChanged += NativeControl_HorizontalScrollBarValueChanged;
        }

        private void NativeControl_HorizontalScrollBarValueChanged(object? sender, EventArgs e)
        {
        }

        private void NativeControl_VerticalScrollBarValueChanged(object? sender, EventArgs e)
        {
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            NativeControl.VerticalScrollBarValueChanged -= NativeControl_VerticalScrollBarValueChanged;
            NativeControl.HorizontalScrollBarValueChanged -= NativeControl_HorizontalScrollBarValueChanged;
        }
    }
}