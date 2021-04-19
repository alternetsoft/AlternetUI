namespace Alternet.UI
{
    internal abstract class ControlHandler<TControl> : ControlHandler
        where TControl : Control
    {
        public new TControl Control => (TControl)base.Control;
    }
}