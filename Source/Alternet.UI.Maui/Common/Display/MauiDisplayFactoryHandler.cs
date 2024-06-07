using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

using Microsoft.Maui.Devices;

namespace Alternet.UI
{
    public class MauiDisplayFactoryHandler : DisposableObject, IDisplayFactoryHandler
    {
        public IDisplayHandler CreateDisplay()
        {
            return new MauiDisplayHandler();
        }

        public IDisplayHandler CreateDisplay(int index)
        {
            return new MauiDisplayHandler(index);
        }

        public int GetCount()
        {
            return 1;
        }

        public SizeI GetDefaultDPI()
        {
            if (App.IsIOS || App.IsMacOS)
                return 72;
            return 96;
        }

        public int GetFromControl(Control control)
        {
            return 1;
        }

        public int GetFromPoint(PointI pt)
        {
            return 1;
        }
    }
}
