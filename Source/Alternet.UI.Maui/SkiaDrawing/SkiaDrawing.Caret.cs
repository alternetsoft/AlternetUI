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

        public override bool CursorIsOk(Cursor cursor)
        {
            throw new NotImplementedException();
        }

        public override PointI CursorGetHotSpot(Cursor cursor)
        {
            throw new NotImplementedException();
        }

        public override void CursorSetGlobal(Cursor? cursor)
        {
            throw new NotImplementedException();
        }

        public override void DisposeCursor(Cursor cursor)
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

        public override SizeI CaretGetSize(Caret caret)
        {
            throw new NotImplementedException();
        }

        public override void CaretSetSize(Caret caret, SizeI value)
        {
            throw new NotImplementedException();
        }

        public override PointI CaretGetPosition(Caret caret)
        {
            throw new NotImplementedException();
        }

        public override void CaretSetPosition(Caret caret, PointI value)
        {
            throw new NotImplementedException();
        }

        public override bool CaretIsOk(Caret caret)
        {
            throw new NotImplementedException();
        }

        public override bool CaretGetVisible(Caret caret)
        {
            throw new NotImplementedException();
        }

        public override void CaretSetVisible(Caret caret, bool value)
        {
            throw new NotImplementedException();
        }

        public override void DisposeCaret(Caret caret)
        {
            throw new NotImplementedException();
        }
    }
}
