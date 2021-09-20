using Alternet.Drawing;

namespace Alternet.UI
{
    internal class NativeCheckBoxHandler : NativeControlHandler<CheckBox, Native.CheckBox>
    {
        internal override Native.Control CreateNativeControl()
        {
            return new Native.CheckBox();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            NativeControl.Text = Control.Text;
            NativeControl.IsChecked = Control.IsChecked;

            Control.TextChanged += Control_TextChanged;
            Control.CheckedChanged += Control_CheckedChanged;
            NativeControl.CheckedChanged += NativeControl_CheckedChanged;
        }

        private void Control_CheckedChanged(object? sender, System.EventArgs? e)
        {
            if (e is null)
                throw new System.ArgumentNullException(nameof(e));

            NativeControl.IsChecked = Control.IsChecked;
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            Control.TextChanged -= Control_TextChanged;
            Control.CheckedChanged -= Control_CheckedChanged;
            NativeControl.CheckedChanged -= NativeControl_CheckedChanged;
        }

        private void NativeControl_CheckedChanged(object? sender, System.EventArgs? e)
        {
            if (e is null)
                throw new System.ArgumentNullException(nameof(e));

            Control.IsChecked = NativeControl.IsChecked;
        }

        private void Control_TextChanged(object? sender, System.EventArgs? e)
        {
            if (e is null)
                throw new System.ArgumentNullException(nameof(e));

            NativeControl.Text = Control.Text;
        }
    }
}