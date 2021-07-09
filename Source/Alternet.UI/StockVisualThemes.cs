namespace Alternet.UI
{
    /// <summary>
    /// <see cref="VisualTheme"/> objects for all the stock visual themes.
    /// </summary>
    public static class StockVisualThemes
    {
        /// <summary>
        /// Gets a stock <see cref="VisualTheme"/> object.
        /// </summary>
        public static VisualTheme Native { get; } = new VisualTheme(StockControlHandlerFactories.Native);

        /// <summary>
        /// Gets a stock <see cref="VisualTheme"/> object.
        /// </summary>
        public static VisualTheme GenericLight { get; } = new VisualTheme(StockControlHandlerFactories.Generic);
    }
}