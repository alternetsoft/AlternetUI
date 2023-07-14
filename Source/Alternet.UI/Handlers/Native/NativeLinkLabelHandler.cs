namespace Alternet.UI
{
    internal class NativeLinkLabelHandler : NativeControlHandler<LinkLabel, Native.LinkLabel>
    {
        internal override Native.Control CreateNativeControl()
        {
            return new Native.LinkLabel()
            {
                Text = Control.Text
            };
        }

        public string Url
        {
            get
            {
                return NativeControl.Url;
            }

            set
            {
                if (NativeControl.Url != value)
                    NativeControl.Url = value;
            }
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