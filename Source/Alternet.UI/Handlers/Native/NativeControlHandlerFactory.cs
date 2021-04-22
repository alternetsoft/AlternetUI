using System;

namespace Alternet.UI
{
    public class NativeControlHandlerFactory : IControlHandlerFactory
    {
        public ControlHandler CreateControlHandler(Control control) => control switch
        {
            Button x => new NativeButtonHandler(),
            StackPanel x => new StackPanelHandler(),
            Border => new GenericBorderHandler(),
            TextBlock => new GenericTextBlockHandler(),
            TextBox x => new NativeTextBoxHandler(),
            _ => new GenericControlHandler()
        };
    }
}