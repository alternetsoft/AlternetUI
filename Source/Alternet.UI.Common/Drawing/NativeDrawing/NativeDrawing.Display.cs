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
        public abstract int DisplayGetFromControl(IControl control);

        public abstract object CreateDisplay();

        public abstract object CreateDisplay(int index);

        public abstract int DisplayGetCount();

        public abstract int DisplayGetDefaultDPIValue();

        public abstract SizeI DisplayGetDefaultDPI();

        public abstract string DisplayGetName(object nativeDisplay);

        public abstract SizeI DisplayGetDPI(object nativeDisplay);

        public abstract double DisplayGetScaleFactor(object nativeDisplay);

        public abstract bool DisplayGetIsPrimary(object nativeDisplay);

        public abstract RectI DisplayGetClientArea(object nativeDisplay);

        public abstract RectI DisplayGetGeometry(object nativeDisplay);

        public abstract int DisplayGetFromPoint(PointI pt);

        public abstract void DisposeDisplay(object nativeDisplay);
    }
}
