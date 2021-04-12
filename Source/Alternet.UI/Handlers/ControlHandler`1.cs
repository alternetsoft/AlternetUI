namespace Alternet.UI
{
    public abstract class ControlHandler<TControl> : ControlHandler where TControl : Control
    {
        protected ControlHandler(TControl control) : base(control)
        {
        }

        public new TControl Control => (TControl)base.Control;
    }
}