using System;

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

        protected override void OnDetach()
        {
            base.OnDetach();

            Control.TextChanged -= Control_TextChanged;
            Control.CheckedChanged -= Control_CheckedChanged;
            NativeControl.CheckedChanged -= NativeControl_CheckedChanged;
        }

        private void Control_CheckedChanged(object? sender, System.EventArgs? e)
        {
        }

        private void NativeControl_CheckedChanged(object? sender, System.EventArgs? e)
        {
            Control.RaiseCheckedChanged(EventArgs.Empty);
        }

        private void Control_TextChanged(object? sender, System.EventArgs? e)
        {
            NativeControl.Text = Control.Text;
        }
    }
}