namespace Alternet.UI
{
    internal abstract class NativeControlHandler<TControl, TNativeControl> : WxControlHandler
        where TControl : Control
        where TNativeControl : Native.Control
    {
        public new TControl Control => (TControl)base.Control;

        public new TNativeControl NativeControl => (TNativeControl)base.NativeControl!;
    }
}