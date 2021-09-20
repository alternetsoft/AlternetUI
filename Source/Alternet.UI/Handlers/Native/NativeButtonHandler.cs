using Alternet.Drawing;

namespace Alternet.UI
{
    internal class NativeButtonHandler : NativeControlHandler<Button, Native.Button>
    {
        internal override Native.Control CreateNativeControl()
        {
            return new Native.Button();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            NativeControl.Text = Control.Text;

            Control.TextChanged += Control_TextChanged;
            NativeControl.Click += NativeControl_Click;
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            Control.TextChanged -= Control_TextChanged;
            NativeControl.Click -= NativeControl_Click;
        }

        private void NativeControl_Click(object? sender, System.EventArgs? e)
        {
            if (e is null)
                throw new System.ArgumentNullException(nameof(e));

            Control.RaiseClick(e);
        }

        private void Control_TextChanged(object? sender, System.EventArgs? e)
        {
            if (e is null)
                throw new System.ArgumentNullException(nameof(e));

            NativeControl.Text = Control.Text;
        }
    }
}