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

        public bool ExactFit
        {
            get => NativeControl.ExactFit;
            set => NativeControl.ExactFit = value;
        }

        public bool TextVisible
        {
            get => NativeControl.TextVisible;
            set => NativeControl.TextVisible = value;
        }

        public ElementContentAlign TextAlign
        {
            get => (ElementContentAlign)NativeControl.TextAlign;
            set => NativeControl.TextAlign = (int)value;
        }

        public void SetImagePosition(ElementContentAlign dir)
        {
            if(Control?.Image != null)
                NativeControl.SetImagePosition((int)dir);
        }

        public void SetImageMargins(Coord x, Coord y)
        {
            if (App.IsWindowsOS && Control?.Image != null)
            {
                var xPixels = PixelFromDip(x);
                var yPixels = PixelFromDip(y);
                NativeControl.SetImageMargins(xPixels, yPixels);
            }
        }

        public override void OnSystemColorsChanged()
        {
            base.OnSystemColorsChanged();

            if(App.IsWindowsOS)
                RecreateWindow();
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