namespace Alternet.UI
{
    internal class NativeRadioButtonHandler : NativeControlHandler<RadioButton, Native.RadioButton>
    {
        internal override Native.Control CreateNativeControl()
        {
            return new Native.RadioButton();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            NativeControl.Text = Control.Text;
            NativeControl.IsChecked = Control.IsChecked;

            Control.TextChanged += Control_TextChanged;
            Control.CheckedChanged += Control_CheckedChanged;
            NativeControl.CheckedChanged = NativeControl_CheckedChanged;
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            Control.TextChanged -= Control_TextChanged;
            Control.CheckedChanged -= Control_CheckedChanged;
            NativeControl.CheckedChanged = null;
        }

        private void Control_CheckedChanged(object? sender, EventArgs e)
        {
            NativeControl.IsChecked = Control.IsChecked;
        }

        private void NativeControl_CheckedChanged()
        {
            Control.IsChecked = NativeControl.IsChecked;
        }

        private void Control_TextChanged(object? sender, EventArgs e)
        {
            NativeControl.Text = Control.Text;
        }
    }
}