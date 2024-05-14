using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    internal partial class WxDrawing
    {
        public override int DisplayGetFromControl(IControl control)
        {
            return UI.Native.WxOtherFactory.DisplayGetFromWindow(WxPlatform.WxWidget(control));
        }

        public override object CreateDisplay()
        {
            return UI.Native.WxOtherFactory.CreateDisplay();
        }

        public override object CreateDisplay(int index)
        {
            return UI.Native.WxOtherFactory.CreateDisplay2((uint)index);
        }

        public override int DisplayGetCount()
            => (int)UI.Native.WxOtherFactory.DisplayGetCount();

        public override int DisplayGetDefaultDPIValue()
            => UI.Native.WxOtherFactory.DisplayGetStdPPIValue();

        public override SizeI DisplayGetDefaultDPI()
            => UI.Native.WxOtherFactory.DisplayGetStdPPI();

        public override string DisplayGetName(Display display)
            => UI.Native.WxOtherFactory.DisplayGetName((IntPtr)display.Handler);

        public override SizeI DisplayGetDPI(Display display)
            => UI.Native.WxOtherFactory.DisplayGetPPI((IntPtr)display.Handler);

        public override double DisplayGetScaleFactor(Display display)
            => UI.Native.WxOtherFactory.DisplayGetScaleFactor((IntPtr)display.Handler);

        public override bool DisplayGetIsPrimary(Display display)
            => UI.Native.WxOtherFactory.DisplayIsPrimary((IntPtr)display.Handler);

        public override RectI DisplayGetClientArea(Display display)
            => UI.Native.WxOtherFactory.DisplayGetClientArea((IntPtr)display.Handler);

        public override RectI DisplayGetGeometry(Display display)
            => UI.Native.WxOtherFactory.DisplayGetGeometry((IntPtr)display.Handler);

        public override int DisplayGetFromPoint(PointI pt) =>
            UI.Native.WxOtherFactory.DisplayGetFromPoint(pt);

        public override void DisposeDisplay(Display display)
        {
            UI.Native.WxOtherFactory.DeleteDisplay((IntPtr)display.Handler);
        }
    }
}
