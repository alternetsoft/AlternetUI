using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class WxButtonHandler : WxControlHandler, IButtonHandler
    {
        public Image? NormalImage
        {
            set => NativeControl.NormalImage = (UI.Native.Image?)value?.Handler;
        }

        public Image? HoveredImage
        {
            set => NativeControl.HoveredImage = (UI.Native.Image?)value?.Handler;
        }

        public Image? PressedImage
        {
            set => NativeControl.PressedImage = (UI.Native.Image?)value?.Handler;
        }

        public Image? DisabledImage
        {
            set => NativeControl.DisabledImage = (UI.Native.Image?)value?.Handler;
        }

        public Image? FocusedImage
        {
            set => NativeControl.FocusedImage = (UI.Native.Image?)value?.Handler;
        }

        /// <summary>
        /// Gets a <see cref="Button"/> this handler provides the implementation for.
        /// </summary>
        public new Button? Control => (Button?)base.Control;

        public override bool HasBorder
        {
            get
            {
                return NativeControl.HasBorder;
            }

            set
            {
                NativeControl.HasBorder = value;
            }
        }

        public new Native.Button NativeControl => (Native.Button)base.NativeControl;

        public bool IsDefault
        {
            get => NativeControl.IsDefault;
            set => NativeControl.IsDefault = value;
        }

        public bool ExactFit
        {
            get => NativeControl.ExactFit;
            set => NativeControl.ExactFit = value;
        }

        public bool IsCancel
        {
            get => NativeControl.IsCancel;
            set => NativeControl.IsCancel = value;
        }

        public bool TextVisible
        {
            get => NativeControl.TextVisible;
            set => NativeControl.TextVisible = value;
        }

        public GenericDirection TextAlign
        {
            get => (GenericDirection)NativeControl.TextAlign;
            set => NativeControl.TextAlign = (int)value;
        }

        public void SetImagePosition(GenericDirection dir)
        {
            if(Control?.Image != null)
                NativeControl.SetImagePosition((int)dir);
        }

        public void SetImageMargins(double x, double y)
        {
            if (App.IsWindowsOS && Control?.Image != null)
            {
                var xPixels = PixelFromDip(x);
                var yPixels = PixelFromDip(y);
                NativeControl.SetImageMargins(xPixels, yPixels);
            }
        }

        protected override void OnAttach()
        {
            base.OnAttach();
            if (App.IsWindowsOS && Control is not null)
                Control.UserPaint = true;
        }

        internal override Native.Control CreateNativeControl()
        {
            return new Native.Button();
        }
    }
}