namespace Alternet.UI
{
    internal abstract class NativeControlHandler<TControl, TNativeControl> : NativeControlHandler
        where TControl : Control
        where TNativeControl : Native.Control
    {
        protected NativeControlHandler(TControl control) : base(control)
        {
        }

        public new TControl Control => (TControl)base.Control;

        public new TNativeControl NativeControl => (TNativeControl)base.NativeControl!;
    }
}