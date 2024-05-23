using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    public partial class NativeDrawing
    {
        public abstract object CreateCursor();

        public abstract object CreateCursor(CursorType cursor);

        public abstract object CreateCursor(
            string cursorName,
            BitmapType type,
            int hotSpotX = 0,
            int hotSpotY = 0);

        public abstract object CreateCursor(Image image);

        public abstract bool CursorIsOk(Cursor cursor);

        public abstract PointI CursorGetHotSpot(Cursor cursor);

        public abstract void CursorSetGlobal(Cursor? cursor);

        public abstract void DisposeCursor(Cursor cursor);

        public abstract object CreateCaret();

        public abstract object CreateCaret(IControl control, int width, int height);

        public abstract int CaretGetBlinkTime();

        public abstract void CaretSetBlinkTime(int value);

        public abstract SizeI CaretGetSize(Caret caret);

        public abstract void CaretSetSize(Caret caret, SizeI value);

        public abstract PointI CaretGetPosition(Caret caret);

        public abstract void CaretSetPosition(Caret caret, PointI value);

        public abstract bool CaretIsOk(Caret caret);

        public abstract bool CaretGetVisible(Caret caret);

        public abstract void CaretSetVisible(Caret caret, bool value);

        public abstract void DisposeCaret(Caret caret);
    }
}
