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
        /// <inheritdoc/>
        public override object CreateCursor()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override object CreateCursor(CursorType cursor)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override object CreateCursor(
            string cursorName,
            BitmapType type,
            int hotSpotX = 0,
            int hotSpotY = 0)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override object CreateCursor(Image image)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override bool CursorIsOk(Cursor cursor)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override PointI CursorGetHotSpot(Cursor cursor)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void CursorSetGlobal(Cursor? cursor)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void DisposeCursor(Cursor cursor)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override object CreateCaret()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override object CreateCaret(IControl control, int width, int height)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override int CaretGetBlinkTime()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void CaretSetBlinkTime(int value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override SizeI CaretGetSize(Caret caret)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void CaretSetSize(Caret caret, SizeI value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override PointI CaretGetPosition(Caret caret)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void CaretSetPosition(Caret caret, PointI value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override bool CaretIsOk(Caret caret)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override bool CaretGetVisible(Caret caret)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void CaretSetVisible(Caret caret, bool value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void DisposeCaret(Caret caret)
        {
            throw new NotImplementedException();
        }
    }
}
