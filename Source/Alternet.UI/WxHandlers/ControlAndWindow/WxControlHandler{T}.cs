namespace Alternet.UI
{
    /// <summary>
    /// Provides base functionality for implementing the behavior and appearance
    /// for a <see cref="Control"/> of a type specified by <c>TControl</c>.
    /// </summary>
    /// <typeparam name="T">Type of the <see cref="Control"/>
    /// descendant</typeparam>
    internal class WxControlHandler<T> : WxControlHandler
        where T : Control
    {
        /// <summary>
        /// Gets a <see cref="Control"/> this handler provides the implementation for.
        /// </summary>
        public new T? Control => (T?)base.Control;
    }
}