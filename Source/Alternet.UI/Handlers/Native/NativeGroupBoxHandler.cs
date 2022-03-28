using Alternet.Drawing;
using System;

namespace Alternet.UI
{
    internal class NativeGroupBoxHandler : NativeControlHandler<GroupBox, Native.GroupBox>
    {
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

        public override Size GetPreferredSize(Size availableSize)
        {
            // Ensure the group box label is included in the size.
            var nativeControlSize = GetNativeControlSize(availableSize);
            var calculatedSize = base.GetPreferredSize(availableSize);

            return new Size(Math.Max(nativeControlSize.Width, calculatedSize.Width), Math.Max(nativeControlSize.Height, calculatedSize.Height));
        }

        private void Control_TitleChanged(object? sender, System.EventArgs? e)
        {
            if (e is null)
                throw new System.ArgumentNullException(nameof(e));

            NativeControl.Title = Control.Title;
        }
    }
}