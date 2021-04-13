namespace Alternet.UI
{
    internal abstract class GenericControlHandler<TControl> : GenericControlHandler
        where TControl : Control
    {
        public new TControl Control => (TControl)base.Control;
    }
}