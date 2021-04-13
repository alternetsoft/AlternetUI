using System.Drawing;

namespace Alternet.UI
{
    internal class NativeButtonHandler : NativeControlHandler<Button, Native.Button>
    {
        public NativeButtonHandler(Button button) : base(button)
        {
            Control.TextChanged += Control_TextChanged;
            NativeControl.Click += NativeControl_Click;
        }

        internal override Native.Control CreateNativeControl()
        {
            return new Native.Button();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Control.TextChanged -= Control_TextChanged;
                NativeControl.Click -= NativeControl_Click;
            }

            base.Dispose(disposing);
        }

        private void NativeControl_Click(object? sender, System.EventArgs? e)
        {
            if (e is null)
                throw new System.ArgumentNullException(nameof(e));

            Control.InvokeClick(e);
        }

        private void Control_TextChanged(object? sender, System.EventArgs? e)
        {
            if (e is null)
                throw new System.ArgumentNullException(nameof(e));

            NativeControl.Text = Control.Text;
        }
    }
}