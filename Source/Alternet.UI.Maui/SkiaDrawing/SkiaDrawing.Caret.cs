using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing
{
    public partial class SkiaDrawing
    {
        public override object CreateCursor()
        {
            throw new NotImplementedException();
        }

        public override object CreateCursor(CursorType cursor)
        {
            throw new NotImplementedException();
        }

        public override object CreateCursor(
            string cursorName,
            BitmapType type,
            int hotSpotX = 0,
            int hotSpotY = 0)
        {
            throw new NotImplementedException();
        }

        public override object CreateCursor(Image image)
        {
            throw new NotImplementedException();
        }

        public override bool CursorIsOk(object nativeCursor)
        {
            throw new NotImplementedException();
        }

        public override PointI CursorGetHotSpot(object nativeCursor)
        {
            throw new NotImplementedException();
        }

        public override void CursorSetGlobal(object nativeCursor)
        {
            throw new NotImplementedException();
        }

        public override void DisposeCursor(object nativeCursor)
        {
            throw new NotImplementedException();
        }

        public override object CreateCaret()
        {
            throw new NotImplementedException();
        }

        public override object CreateCaret(IControl control, int width, int height)
        {
            throw new NotImplementedException();
        }

        public override int CaretGetBlinkTime()
        {
            throw new NotImplementedException();
        }

        public override void CaretSetBlinkTime(int value)
        {
            throw new NotImplementedException();
        }

        public override SizeI CaretGetSize(object nativeCaret)
        {
            throw new NotImplementedException();
        }

        public override void CaretSetSize(object nativeCaret, SizeI value)
        {
            throw new NotImplementedException();
        }

        public override PointI CaretGetPosition(object nativeCaret)
        {
            throw new NotImplementedException();
        }

        public override void CaretSetPosition(object nativeCaret, PointI value)
        {
            throw new NotImplementedException();
        }

        public override bool CaretIsOk(object nativeCaret)
        {
            throw new NotImplementedException();
        }

        public override bool CaretGetVisible(object nativeCaret)
        {
            throw new NotImplementedException();
        }

        public override void CaretSetVisible(object nativeCaret, bool value)
        {
            throw new NotImplementedException();
        }

        public override void DisposeCaret(object nativeCaret)
        {
            throw new NotImplementedException();
        }
    }
}
