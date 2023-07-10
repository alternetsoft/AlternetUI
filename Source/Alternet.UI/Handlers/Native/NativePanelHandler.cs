namespace Alternet.UI
{
    internal class NativePanelHandler : NativeControlHandler<Panel, Native.Panel>
    {
        internal override Native.Control CreateNativeControl()
        {
            return new Native.Panel();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

        }

        protected override void OnDetach()
        {
            base.OnDetach();

        }
    }
}