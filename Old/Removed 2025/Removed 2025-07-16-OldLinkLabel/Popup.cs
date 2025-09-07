#pragma warning disable
using System;
using Alternet.Drawing;
using ApiCommon;

namespace NativeApi.Api
{
    // https://docs.wxwidgets.org/3.2/classwx_popup_window.html
    public class Popup : Control
    {
        public bool IsTransient { get; set; }
        public bool PuContainsControls { get; set; }

        public void DoPopup(IntPtr focus) { }
        public void Dismiss() { }

        public void Position(PointI ptOrigin, SizeI sizePopup) { }
    }
}







