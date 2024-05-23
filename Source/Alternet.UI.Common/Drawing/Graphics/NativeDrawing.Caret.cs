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
