namespace Alternet.UI
{
    public abstract class ControlHandler<TControl> : ControlHandler
        where TControl : Control
    {
        public new TControl Control => (TControl)base.Control;
    }
}