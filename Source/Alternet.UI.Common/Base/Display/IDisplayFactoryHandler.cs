using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public interface IDisplayFactoryHandler : IDisposable
    {
        int GetFromControl(Control control);

        IDisplayHandler CreateDisplay();

        IDisplayHandler CreateDisplay(int index);

        int GetCount();

        SizeI GetDefaultDPI();

        int GetFromPoint(PointI pt);
    }
}
