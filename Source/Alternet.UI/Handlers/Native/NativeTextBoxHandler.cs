namespace Alternet.UI
{
    internal class NativeTextBoxHandler : NativeControlHandler<TextBox, Native.TextBox>
    {
        public NativeTextBoxHandler(TextBox textBox) : base(textBox)
        {
            Control.TextChanged += Control_TextChanged;
            NativeControl.TextChanged += NativeControl_TextChanged;
        }

        internal override Native.Control CreateNativeControl()
        {
            return new Native.TextBox();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Control.TextChanged -= Control_TextChanged;
                NativeControl.TextChanged -= NativeControl_TextChanged;
            }

            base.Dispose(disposing);
        }

        bool handlingNativeControlTextChanged;

        private void NativeControl_TextChanged(object? sender, System.EventArgs? e)
        {
            if (e is null)
                throw new System.ArgumentNullException(nameof(e));

            handlingNativeControlTextChanged = true;
            try
            {
                Control.Text = NativeControl.Text!;
            }
            finally
            {
                handlingNativeControlTextChanged = false;
            }
        }

        private void Control_TextChanged(object? sender, System.EventArgs? e)
        {
            if (e is null)
                throw new System.ArgumentNullException(nameof(e));

            if (!handlingNativeControlTextChanged)
            {
                if (NativeControl.Text != Control.Text)
                    NativeControl.Text = Control.Text;
            }
        }
    }
}