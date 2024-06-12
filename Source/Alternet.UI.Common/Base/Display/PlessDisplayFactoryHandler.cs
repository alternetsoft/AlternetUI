using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public abstract class PlessDisplayFactoryHandler : DisposableObject, IDisplayFactoryHandler
    {
        public abstract IDisplayHandler CreateDisplay();

        public virtual IDisplayHandler CreateDisplay(int index)
        {
            return CreateDisplay();
        }

        public virtual int GetCount()
        {
            return 1;
        }

        public virtual SizeI GetDefaultDPI()
        {
            if (App.IsIOS || App.IsMacOS)
                return 72;
            return 96;
        }

        public virtual int GetFromControl(Control control)
        {
            return 0;
        }

        public virtual int GetFromPoint(PointI pt)
        {
            return 0;
        }
    }
}
