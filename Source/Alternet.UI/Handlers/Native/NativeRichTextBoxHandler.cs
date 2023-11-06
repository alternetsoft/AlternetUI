using System;

namespace Alternet.UI
{
    internal class NativeRichTextBoxHandler : ControlHandler
    {
        public new Native.RichTextBox NativeControl => (Native.RichTextBox)base.NativeControl!;

        public new RichTextBox Control => (RichTextBox)base.Control;

        /// <inheritdoc/>
        protected override bool VisualChildNeedsNativeControl => true;

        internal override Native.Control CreateNativeControl()
        {
            return new Native.RichTextBox();
        }

        protected override void OnAttach()
        {
            base.OnAttach();
        }

        protected override void OnDetach()
        {
            base.OnDetach();
        }
    }
}