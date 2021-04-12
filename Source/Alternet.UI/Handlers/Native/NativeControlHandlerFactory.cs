namespace Alternet.UI
{
    public class NativeControlHandlerFactory : IControlHandlerFactory
    {
        public ControlHandler CreateControlHandler(Control control) => control switch
        {
            Button x => new NativeButtonHandler(x),
            Window x => new NativeWindowHandler(x),
            StackPanel x => new NativeStackPanelHandler(x),
            TextBox x => new NativeTextBoxHandler(x),
            _ => new NativeControlHandler(control)
        };
    }
}