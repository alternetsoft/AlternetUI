namespace Alternet.UI
{
    internal class NativeButtonHandler : ButtonHandler
    {
        internal override Native.Control CreateNativeControl()
        {
            return new Native.Button();
        }

        public new Native.Button NativeControl => (Native.Button)base.NativeControl!;

        public override bool IsDefault { get => NativeControl.IsDefault; set => NativeControl.IsDefault = value; }
        public override bool IsCancel { get => NativeControl.IsCancel; set => NativeControl.IsCancel = value; }

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