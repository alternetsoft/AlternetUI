using System;

namespace Alternet.UI
{
    internal class WxPanelHandler : WxControlHandler<Panel>
    {
        internal override Native.Control CreateNativeControl()
        {
            return new Native.Panel();
        }
    }
}