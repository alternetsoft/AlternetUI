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
            return UI.Native.WxOtherFactory.CreateCaret2(
                WxPlatform.WxWidget(control),
                width,
                height);
        }

        public override int CaretGetBlinkTime()
        {
            return UI.Native.WxOtherFactory.CaretGetBlinkTime();
        }

        public override void CaretSetBlinkTime(int value)
        {
            UI.Native.WxOtherFactory.CaretSetBlinkTime(value);
        }

        public override SizeI CaretGetSize(Caret caret)
        {
            return UI.Native.WxOtherFactory.CaretGetSize((IntPtr)caret.Handler);
        }

        public override void CaretSetSize(Caret caret, SizeI value)
        {
            UI.Native.WxOtherFactory.CaretSetSize((IntPtr)caret.Handler, value.Width, value.Height);
        }

        public override PointI CaretGetPosition(Caret caret)
        {
            return UI.Native.WxOtherFactory.CaretGetPosition((IntPtr)caret.Handler);
        }

        public override void CaretSetPosition(Caret caret, PointI value)
        {
            UI.Native.WxOtherFactory.CaretMove((IntPtr)caret.Handler, value.X, value.Y);
        }

        public override bool CaretIsOk(Caret caret)
        {
            return UI.Native.WxOtherFactory.CaretIsOk((IntPtr)caret.Handler);
        }

        public override bool CaretGetVisible(Caret caret)
        {
            return UI.Native.WxOtherFactory.CaretIsVisible((IntPtr)caret.Handler);
        }

        public override void CaretSetVisible(Caret caret, bool value)
        {
            UI.Native.WxOtherFactory.CaretShow((IntPtr)caret.Handler, value);
        }

        public override void DisposeCaret(Caret caret)
        {
            UI.Native.WxOtherFactory.DeleteCaret((IntPtr)caret.Handler);
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

        public override bool CursorIsOk(Cursor cursor)
        {
            return UI.Native.WxOtherFactory.CursorIsOk((IntPtr)cursor.Handler);
        }

        public override PointI CursorGetHotSpot(Cursor cursor)
        {
            return UI.Native.WxOtherFactory.CursorGetHotSpot((IntPtr)cursor.Handler);
        }

        public override void CursorSetGlobal(Cursor? cursor)
        {
            if (cursor is null)
                UI.Native.WxOtherFactory.SetCursor(default);
            else
                UI.Native.WxOtherFactory.SetCursor((IntPtr)cursor.Handler);
        }

        public override void DisposeCursor(Cursor cursor)
        {
            UI.Native.WxOtherFactory.DeleteCursor((IntPtr)cursor.Handler);
        }
    }
}
