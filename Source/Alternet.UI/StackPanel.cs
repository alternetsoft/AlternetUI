using System;

namespace Alternet.UI
{
    public class StackPanel : Control
    {
        protected override ControlHandler CreateHandler()
        {
            return new NativeStackPanelHandler(this);
        }
    }
}