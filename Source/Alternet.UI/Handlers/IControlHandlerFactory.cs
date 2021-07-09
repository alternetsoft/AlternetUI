namespace Alternet.UI
{
    /// <summary>
    /// Represents an interface that can be implemented by classes providing creating <see cref="ControlHandler"/> instances for specified controls.
    /// </summary>
    public interface IControlHandlerFactory
    {
        /// <summary>
        /// Creates a <see cref="ControlHandler"/> for specified control.
        /// </summary>
        ControlHandler CreateControlHandler(Control control);
    }
}