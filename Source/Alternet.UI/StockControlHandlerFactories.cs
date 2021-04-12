namespace Alternet.UI
{
    public static class StockControlHandlerFactories
    {
        public static IControlHandlerFactory Native { get; } = new NativeControlHandlerFactory();

        //public static IControlHandlerFactory Generic { get; }
    }
}