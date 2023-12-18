using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class NativeGroupBoxHandler : NativeControlHandler<GroupBox, Native.GroupBox>
    {
        public override SizeD GetPreferredSize(SizeD availableSize)
        {
            // Ensure the group box label is included in the size.
            var nativeControlSize = GetNativeControlSize(availableSize);
            var calculatedSize = base.GetPreferredSize(availableSize);

            return new SizeD(
                Math.Max(nativeControlSize.Width, calculatedSize.Width),
                Math.Max(nativeControlSize.Height, calculatedSize.Height));
        }

        internal override Native.Control CreateNativeControl()
        {
            return new Native.GroupBox();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            NativeControl.Title = Control.Title;

            Control.TitleChanged += Control_TitleChanged;
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            Control.TitleChanged -= Control_TitleChanged;
        }

        private void Control_TitleChanged(object? sender, EventArgs e)
        {
            NativeControl.Title = Control.Title;
        }
    }
}