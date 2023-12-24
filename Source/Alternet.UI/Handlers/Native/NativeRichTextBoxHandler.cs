using System;

namespace Alternet.UI
{
    internal class NativeRichTextBoxHandler : ControlHandler
    {
        public new Native.RichTextBox NativeControl => (Native.RichTextBox)base.NativeControl!;

        public new RichTextBox Control => (RichTextBox)base.Control;

        /*/// <inheritdoc/>
        protected override bool VisualChildNeedsNativeControl => true;*/

        internal override Native.Control CreateNativeControl()
        {
            return new Native.RichTextBox();
        }

        protected override void OnAttach()
        {
            base.OnAttach();
            NativeControl.TextChanged += NativeControl_TextChanged;
            NativeControl.TextEnter += NativeControl_TextEnter;
            NativeControl.TextUrl += NativeControl_TextUrl;
        }

        protected override void OnDetach()
        {
            base.OnDetach();
            NativeControl.TextChanged -= NativeControl_TextChanged;
            NativeControl.TextEnter -= NativeControl_TextEnter;
            NativeControl.TextUrl -= NativeControl_TextUrl;
        }

        private void NativeControl_TextUrl(object? sender, EventArgs e)
        {
            var url = NativeControl.ReportedUrl;
            Control.OnTextUrl(new UrlEventArgs(url));
        }

        private void NativeControl_TextEnter(object? sender, EventArgs e)
        {
            Control.OnEnterPressed(e);
        }

        private void NativeControl_TextChanged(object? sender, EventArgs e)
        {
            Control.RaiseTextChanged(e);
        }
    }
}