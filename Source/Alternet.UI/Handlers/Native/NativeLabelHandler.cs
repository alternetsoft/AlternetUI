namespace Alternet.UI
{
    internal class NativeLabelHandler : NativeControlHandler<Label, Native.Label>
    {
        internal override Native.Control CreateNativeControl()
        {
            return new Native.Label() { Text = Control.Text };
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            NativeControl.Text = Control.Text;

            Control.TextChanged += Control_TextChanged;
        }

        protected override void OnDetach()
        {
            base.OnDetach();
            Control.TextChanged -= Control_TextChanged;
        }

        private void Control_TextChanged(object? sender, System.EventArgs? e)
        {
            if (e is null)
                throw new System.ArgumentNullException(nameof(e));

            if (NativeControl.Text != Control.Text)
                NativeControl.Text = Control.Text;
        }
    }
}