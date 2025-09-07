#pragma warning disable
using ApiCommon;
using System;

namespace NativeApi.Api
{
    // https://docs.wxwidgets.org/3.2/classwx_static_text.html
    public class Label : Control
    {
        public bool IsEllipsized() => false;

        public void Wrap(int width) { }
    }
}