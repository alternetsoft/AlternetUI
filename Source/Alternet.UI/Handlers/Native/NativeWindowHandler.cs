namespace Alternet.UI
{
    internal class NativeWindowHandler : NativeControlHandler<Window, Native.Window>
    {
        internal override Native.Control CreateNativeControl()
        {
            return new Native.Window();
        }

        protected override void OnAttach()
        {
            base.OnAttach();
            Control.TitleChanged += Control_TitleChanged;
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            Control.TitleChanged -= Control_TitleChanged;
        }

        private void Control_TitleChanged(object? sender, System.EventArgs? e)
        {
            if (e is null)
                throw new System.ArgumentNullException(nameof(e));

            NativeControl.Title = Control.Title;
        }
    }
}