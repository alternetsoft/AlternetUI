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

        public abstract bool CursorIsOk(object nativeCursor);

        public abstract PointI CursorGetHotSpot(object nativeCursor);

        public abstract void CursorSetGlobal(object nativeCursor);

        public abstract void DisposeCursor(object nativeCursor);

        public abstract object CreateCaret();

        public abstract object CreateCaret(IControl control, int width, int height);

        public abstract int CaretGetBlinkTime();

        public abstract void CaretSetBlinkTime(int value);

        public abstract SizeI CaretGetSize(object nativeCaret);

        public abstract void CaretSetSize(object nativeCaret, SizeI value);

        public abstract PointI CaretGetPosition(object nativeCaret);

        public abstract void CaretSetPosition(object nativeCaret, PointI value);

        public abstract bool CaretIsOk(object nativeCaret);

        public abstract bool CaretGetVisible(object nativeCaret);

        public abstract void CaretSetVisible(object nativeCaret, bool value);

        public abstract void DisposeCaret(object nativeCaret);
    }
}
