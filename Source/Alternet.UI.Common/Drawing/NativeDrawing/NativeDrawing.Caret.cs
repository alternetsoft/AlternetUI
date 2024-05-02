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

        public abstract object CreateCaret(object control, int width, int height);

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
