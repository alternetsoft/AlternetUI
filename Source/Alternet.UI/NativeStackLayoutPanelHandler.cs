namespace Alternet.UI
{
    internal class NativeStackLayoutPanelHandler : NativeControlHandler<StackLayoutPanel, Native.StackLayoutPanel>
    {
        public NativeStackLayoutPanelHandler(StackLayoutPanel control) : base(control)
        {
        }

        protected override Native.Control CreateNativeControl()
        {
            return new Native.StackLayoutPanel();
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