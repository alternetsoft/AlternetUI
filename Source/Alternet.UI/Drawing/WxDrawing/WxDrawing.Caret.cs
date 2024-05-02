using System;
using System.Collections.Generic;
using System.IO;
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

        public override object CreateCaret(IControl control, int width, int height)
        {
            if(control is Control controlObj)
                return UI.Native.WxOtherFactory.CreateCaret2(controlObj.WxWidget, width, height);
            throw new ArgumentException(nameof(control));
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

        public override object CreateCursor()
        {
            return UI.Native.WxOtherFactory.CreateCursor();
        }

        public override object CreateCursor(CursorType cursor)
        {
            return UI.Native.WxOtherFactory.CreateCursor2((int)cursor);
        }

        public override object CreateCursor(
            string cursorName,
            BitmapType type,
            int hotSpotX = 0,
            int hotSpotY = 0)
        {
            return UI.Native.WxOtherFactory.CreateCursor3(
                    cursorName,
                    (int)type,
                    hotSpotX,
                    hotSpotY);
        }

        public override object CreateCursor(Image image)
        {
            return UI.Native.WxOtherFactory.CreateCursor4((UI.Native.Image)image.NativeObject);
        }

        public override bool CursorIsOk(object nativeCursor)
        {
            return UI.Native.WxOtherFactory.CursorIsOk((IntPtr)nativeCursor);
        }

        public override PointI CursorGetHotSpot(object nativeCursor)
        {
            return UI.Native.WxOtherFactory.CursorGetHotSpot((IntPtr)nativeCursor);
        }

        public override void CursorSetGlobal(object nativeCursor)
        {
            UI.Native.WxOtherFactory.SetCursor((IntPtr)nativeCursor);
        }

        public override void DisposeCursor(object nativeCursor)
        {
            UI.Native.WxOtherFactory.DeleteCursor((IntPtr)nativeCursor);
        }
    }
}
