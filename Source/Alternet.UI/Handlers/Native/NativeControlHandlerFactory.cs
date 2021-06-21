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
            CheckBox x => new NativeCheckBoxHandler(),
            RadioButton x => new NativeRadioButtonHandler(),
            TabControl x => new NativeTabControlHandler(),
            GroupBox x => new NativeGroupBoxHandler(),
            _ => new GenericControlHandler()
        };
    }
}