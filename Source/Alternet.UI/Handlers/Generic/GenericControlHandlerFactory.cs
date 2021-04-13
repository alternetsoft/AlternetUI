namespace Alternet.UI
{
    public class GenericControlHandlerFactory : IControlHandlerFactory
    {
        public ControlHandler CreateControlHandler(Control control) => control switch
        {
            Button x => new GenericButtonHandler(),
            _ => StockControlHandlerFactories.Native.CreateControlHandler(control)
        };
    }
}