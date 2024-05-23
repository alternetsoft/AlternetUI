namespace Alternet.UI
{
    /// <summary>
    /// Control handler factories for all the stock visual themes.
    /// </summary>
    internal static class StockControlHandlerFactories
    {
        /// <summary>
        /// Gets a stock <see cref="IControlHandlerFactory"/> object.
        /// </summary>
        public static IControlHandlerFactory Native { get; set; } = new NativeControlHandlerFactory();

        /// <summary>
        /// Gets a stock <see cref="IControlHandlerFactory"/> object.
        /// </summary>
        public static IControlHandlerFactory Generic { get; set; } = new GenericControlHandlerFactory();
    }
}