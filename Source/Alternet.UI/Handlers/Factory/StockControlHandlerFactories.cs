namespace Alternet.UI
{
    /// <summary>
    /// Control handler factories for all the stock visual themes.
    /// </summary>
    public static class StockControlHandlerFactories
    {
        /// <summary>
        /// Gets a stock <see cref="IControlHandlerFactory"/> object.
        /// </summary>
        public static IControlHandlerFactory Native { get; } = new NativeControlHandlerFactory();

        /// <summary>
        /// Gets a stock <see cref="IControlHandlerFactory"/> object.
        /// </summary>
        public static IControlHandlerFactory Generic { get; } = new GenericControlHandlerFactory();
    }
}