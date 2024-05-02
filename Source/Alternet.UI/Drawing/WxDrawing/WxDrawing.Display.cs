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
            if (control is Control controlObj)
                return UI.Native.WxOtherFactory.DisplayGetFromWindow(controlObj.WxWidget);
            else
                throw new ArgumentException(nameof(control));
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

        public override string DisplayGetName(object nativeDisplay)
            => UI.Native.WxOtherFactory.DisplayGetName((IntPtr)nativeDisplay);

        public override SizeI DisplayGetDPI(object nativeDisplay)
            => UI.Native.WxOtherFactory.DisplayGetPPI((IntPtr)nativeDisplay);

        public override double DisplayGetScaleFactor(object nativeDisplay)
            => UI.Native.WxOtherFactory.DisplayGetScaleFactor((IntPtr)nativeDisplay);

        public override bool DisplayGetIsPrimary(object nativeDisplay)
            => UI.Native.WxOtherFactory.DisplayIsPrimary((IntPtr)nativeDisplay);

        public override RectI DisplayGetClientArea(object nativeDisplay)
            => UI.Native.WxOtherFactory.DisplayGetClientArea((IntPtr)nativeDisplay);

        public override RectI DisplayGetGeometry(object nativeDisplay)
            => UI.Native.WxOtherFactory.DisplayGetGeometry((IntPtr)nativeDisplay);

        public override int DisplayGetFromPoint(PointI pt) =>
            UI.Native.WxOtherFactory.DisplayGetFromPoint(pt);

        public override void DisposeDisplay(object nativeDisplay)
        {
            UI.Native.WxOtherFactory.DeleteDisplay((IntPtr)nativeDisplay);
        }
    }
}
