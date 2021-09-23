namespace Alternet.UI
{
    /// <summary>
    /// Provides base functionality for implementing the behavior and appearance
    /// for a <see cref="Control"/> of a type specified by <c>TControl</c>.
    /// </summary>
    /// <typeparam name="TControl"></typeparam>
    public abstract class ControlHandler<TControl> : ControlHandler
        where TControl : Control
    {
        /// <summary>
        /// Gets a <see cref="Control"/> this handler provides the implementation for.
        /// </summary>
        public new TControl Control => (TControl)base.Control;
    }
}