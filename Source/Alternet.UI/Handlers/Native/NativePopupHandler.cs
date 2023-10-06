using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class NativePopupHandler : PopupHandler
    {
        public new Native.Popup NativeControl => (Native.Popup)base.NativeControl!;

        /// <inheritdoc/>
        public override void SetSizeToContent(WindowSizeToContentMode mode)
        {
            if (mode == WindowSizeToContentMode.None)
                return;

            var newSize = GetChildrenMaxPreferredSizePadded(new Size(double.PositiveInfinity, double.PositiveInfinity));
            if (newSize != Size.Empty)
            {
                var currentSize = Control.ClientSize;
                switch (mode)
                {
                    case WindowSizeToContentMode.Width:
                        newSize.Height = currentSize.Height;
                        break;

                    case WindowSizeToContentMode.Height:
                        newSize.Width = currentSize.Width;
                        break;

                    case WindowSizeToContentMode.WidthAndHeight:
                        break;

                    default:
                        throw new Exception();
                }

                Control.ClientSize = newSize + new Size(1, 0);
                Control.ClientSize = newSize;
                Control.Refresh();
                NativeControl.SendSizeEvent();
            }
        }

        internal override Native.Control CreateNativeControl()
        {
            return new Native.Popup();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            ApplyOwner();

            Control.OwnerChanged += Control_OwnerChanged;
        }

        protected override void OnDetach()
        {
            Control.OwnerChanged -= Control_OwnerChanged;

            base.OnDetach();
        }

        private void ApplyOwner()
        {
            var newOwner = Control.Owner?.Handler?.NativeControl;
            var oldOwner = NativeControl.ParentRefCounted;
            if (newOwner == oldOwner)
                return;

            if (oldOwner != null)
                oldOwner.RemoveChild(NativeControl);

            if (newOwner == null)
                return;

            newOwner.AddChild(NativeControl);
        }

        private void Control_OwnerChanged(object? sender, EventArgs e)
        {
            ApplyOwner();
        }
    }
}