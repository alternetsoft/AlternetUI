namespace Alternet.UI
{
    public class GenericControlHandlerFactory : IControlHandlerFactory
    {
        public ControlHandler CreateControlHandler(Control control) => control switch
        {
            Button => new GenericButtonHandler(),
            Border => new GenericBorderHandler(),
            _ => StockControlHandlerFactories.Native.CreateControlHandler(control)
        };
    }
}