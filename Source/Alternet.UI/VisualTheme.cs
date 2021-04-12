namespace Alternet.UI
{
    public class VisualTheme
    {
        public VisualTheme(IControlHandlerFactory controlHandlerFactory)
        {
            ControlHandlerFactory = controlHandlerFactory;
        }

        public IControlHandlerFactory ControlHandlerFactory { get; }
    }
}