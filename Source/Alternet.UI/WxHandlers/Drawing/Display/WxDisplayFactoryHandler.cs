using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal class WxDisplayFactoryHandler : DisposableObject, IDisplayFactoryHandler
    {
        public int GetFromControl(Control control)
        {
            return UI.Native.WxOtherFactory.DisplayGetFromWindow(WxApplicationHandler.WxWidget(control));
        }

        public IDisplayHandler CreateDisplay()
        {
            return new WxDisplayHandler();
        }

        public IDisplayHandler CreateDisplay(int index)
        {
            return new WxDisplayHandler(index);
        }

        public int GetCount()
        {
            return (int)UI.Native.WxOtherFactory.DisplayGetCount();
        }

        public SizeI GetDefaultDPI()
        {
            return UI.Native.WxOtherFactory.DisplayGetStdPPI();
        }

        public int GetFromPoint(PointI pt)
        {
            return UI.Native.WxOtherFactory.DisplayGetFromPoint(pt);
        }
    }
}