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

        public abstract string DisplayGetName(Display display);

        public abstract SizeI DisplayGetDPI(Display display);

        public abstract double DisplayGetScaleFactor(Display display);

        public abstract bool DisplayGetIsPrimary(Display display);

        public abstract RectI DisplayGetClientArea(Display display);

        public abstract RectI DisplayGetGeometry(Display display);

        public abstract int DisplayGetFromPoint(PointI pt);

        public abstract void DisposeDisplay(Display display);
    }
}
