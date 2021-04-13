namespace Alternet.UI
{
    public class GenericControlHandlerFactory : IControlHandlerFactory
    {
        public ControlHandler CreateControlHandler(Control control) => control switch
        {
            Button => new GenericButtonHandler(),
            Border => new GenericBorderHandler(),
            TextBlock => new GenericTextBlockHandler(),
            _ => StockControlHandlerFactories.Native.CreateControlHandler(control)
        };
    }
}