using System;

namespace Alternet.UI
{
    public class StackLayoutPanel : Control
    {
        protected override ControlHandler CreateHandler()
        {
            return new NativeStackLayoutPanelHandler(this);
        }
    }
}