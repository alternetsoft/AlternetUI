namespace Alternet.UI
{
    /// <summary>
    /// Implements an <see cref="IControlHandlerFactory"/> for the Generic visual theme.
    /// </summary>
    public class GenericControlHandlerFactory : IControlHandlerFactory
    {
        /// <inheritdoc/>
        public ControlHandler CreateControlHandler(Control control) => control switch
        {
            Button => new GenericButtonHandler(),
            Border => new GenericBorderHandler(),
            Label => new GenericLabelHandler(),
            TextBox => new GenericTextBoxHandler(),
            CheckBox => new GenericCheckBoxHandler(),
            _ => StockControlHandlerFactories.Native.CreateControlHandler(control)
        };
    }
}