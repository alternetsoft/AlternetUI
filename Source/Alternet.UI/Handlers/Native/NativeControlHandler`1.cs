namespace Alternet.UI
{
    internal abstract class NativeControlHandler<TControl, TNativeControl> : ControlHandler
        where TControl : Control
        where TNativeControl : Native.Control
    {
        public new TControl Control => (TControl)base.Control;

        public new TNativeControl NativeControl => (TNativeControl)base.NativeControl!;

        protected override bool VisualChildNeedsNativeControl => true;
    }
}