using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class WxToolTipFactoryHandler : DisposableObject, IToolTipFactoryHandler
    {
        public bool SetReshow(long msecs)
        {
            Native.WxOtherFactory.ToolTipSetReshow(msecs);
            return true;
        }

        public bool SetEnabled(bool flag)
        {
            Native.WxOtherFactory.ToolTipEnable(flag);
            return true;
        }

        public bool SetAutoPop(long msecs)
        {
            Native.WxOtherFactory.ToolTipSetAutoPop(msecs);
            return true;
        }

        public bool SetDelay(long msecs)
        {
            Native.WxOtherFactory.ToolTipSetDelay(msecs);
            return true;
        }

        public bool SetMaxWidth(int width)
        {
            Native.WxOtherFactory.ToolTipSetMaxWidth(width);
            return true;
        }
    }
}
