using System;

namespace Alternet.UI
{
    internal class NativeRichTextBoxHandler : ControlHandler
    {
        public new Native.RichTextBox NativeControl => (Native.RichTextBox)base.NativeControl!;

        public new RichTextBox Control => (RichTextBox)base.Control;

        internal override Native.Control CreateNativeControl()
        {
            return new Native.RichTextBox();
        }

        protected override void OnAttach()
        {
            base.OnAttach();
            NativeControl.TextChanged = NativeControl_TextChanged;
            NativeControl.TextEnter = NativeControl_TextEnter;
            NativeControl.TextUrl = NativeControl_TextUrl;
        }

        protected override void OnDetach()
        {
            base.OnDetach();
            NativeControl.TextChanged = null;
            NativeControl.TextEnter = null;
            NativeControl.TextUrl = null;
        }

        private void NativeControl_TextUrl()
        {
            var url = NativeControl.ReportedUrl;
            Control.OnTextUrl(new UrlEventArgs(url));
        }

        private void NativeControl_TextEnter()
        {
            Control.OnEnterPressed(EventArgs.Empty);
        }

        private void NativeControl_TextChanged()
        {
            Control.RaiseTextChanged(EventArgs.Empty);
        }
    }
}