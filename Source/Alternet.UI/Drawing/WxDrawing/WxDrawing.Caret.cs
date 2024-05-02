using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    internal partial class WxDrawing
    {
        public override object CreateCaret()
        {
            return UI.Native.WxOtherFactory.CreateCaret();
        }

        public override object CreateCaret(object control, int width, int height)
        {
            return UI.Native.WxOtherFactory.CreateCaret2(((Control)control).WxWidget, width, height);
        }

        public override int CaretGetBlinkTime()
        {
            return UI.Native.WxOtherFactory.CaretGetBlinkTime();
        }

        public override void CaretSetBlinkTime(int value)
        {
            UI.Native.WxOtherFactory.CaretSetBlinkTime(value);
        }

        public override SizeI CaretGetSize(object nativeCaret)
        {
            return UI.Native.WxOtherFactory.CaretGetSize((IntPtr)nativeCaret);
        }

        public override void CaretSetSize(object nativeCaret, SizeI value)
        {
            UI.Native.WxOtherFactory.CaretSetSize((IntPtr)nativeCaret, value.Width, value.Height);
        }

        public override PointI CaretGetPosition(object nativeCaret)
        {
            return UI.Native.WxOtherFactory.CaretGetPosition((IntPtr)nativeCaret);
        }

        public override void CaretSetPosition(object nativeCaret, PointI value)
        {
            UI.Native.WxOtherFactory.CaretMove((IntPtr)nativeCaret, value.X, value.Y);
        }

        public override bool CaretIsOk(object nativeCaret)
        {
            return UI.Native.WxOtherFactory.CaretIsOk((IntPtr)nativeCaret);
        }

        public override bool CaretGetVisible(object nativeCaret)
        {
            return UI.Native.WxOtherFactory.CaretIsVisible((IntPtr)nativeCaret);
        }

        public override void CaretSetVisible(object nativeCaret, bool value)
        {
            UI.Native.WxOtherFactory.CaretShow((IntPtr)nativeCaret, value);
        }

        public override void DisposeCaret(object nativeCaret)
        {
            UI.Native.WxOtherFactory.DeleteCaret((IntPtr)nativeCaret);
        }
    }
}
