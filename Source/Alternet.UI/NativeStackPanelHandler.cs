namespace Alternet.UI
{
    internal class NativeStackPanelHandler : NativeControlHandler<StackPanel, Native.Panel>
    {
        public NativeStackPanelHandler(StackPanel control) : base(control)
        {
        }

        protected override Native.Control CreateNativeControl()
        {
            return new Native.Panel();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }

            base.Dispose(disposing);
        }
    }
}