namespace Alternet.UI
{
    internal abstract class NativeControlHandler : ControlHandler
    {
        protected NativeControlHandler(Control control) : base(control)
        {
            NativeControl = CreateNativeControl();
        }

        public Native.Control NativeControl { get; }

        protected abstract Native.Control CreateNativeControl();
    }
}