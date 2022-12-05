using System;

namespace Alternet.UI
{
    internal class NativeStatusBarPanelHandler : StatusBarPanelHandler
    {
        internal override Native.Control CreateNativeControl()
        {
            return new Native.StatusBarPanel();
        }

        public new Native.StatusBarPanel NativeControl => (Native.StatusBarPanel)base.NativeControl!;

        protected override void OnAttach()
        {
            base.OnAttach();

            ApplyText();

            Control.TextChanged += Control_TextChanged;
        }

        private void ApplyText()
        {
            NativeControl.Text = Control.Text;
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            Control.TextChanged -= Control_TextChanged;
        }

        private void Control_TextChanged(object? sender, EventArgs? e)
        {
            ApplyText();
        }
    }
}