using System;
using System.Runtime.InteropServices;

namespace Alternet.UI
{
    internal class NativeTextBoxHandler : NativeControlHandler<TextBox, Native.TextBox>
    {
        private bool handlingNativeControlTextChanged;

        public bool IsRichEdit
        {
            get
            {
                return NativeControl.IsRichEdit;
            }

            set
            {
                NativeControl.IsRichEdit = value;
            }
        }

        internal override Native.Control CreateNativeControl()
        {
            return new Native.TextBox() {
                Text = Control.Text,
                EditControlOnly = !Control.HasBorder,
            };
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            ApplyMultiline();
            ApplyReadOnly();
            NativeControl.Text = Control.Text;

            Control.HasBorderChanged += Control_HasBorderChanged;
            Control.TextChanged += Control_TextChanged;
            Control.MultilineChanged += Control_MultilineChanged;
            Control.ReadOnlyChanged += Control_ReadOnlyChanged;
            NativeControl.TextChanged += NativeControl_TextChanged;
        }

        private void Control_ReadOnlyChanged(object? sender, System.EventArgs e)
        {
            ApplyReadOnly();
        }

        private void ApplyReadOnly()
        {
            NativeControl.ReadOnly = Control.ReadOnly;
        }

        private void ApplyMultiline()
        {
            NativeControl.Multiline = Control.Multiline;
        }

        private void Control_MultilineChanged(object? sender, System.EventArgs e)
        {
            ApplyMultiline();
        }

        private void Control_HasBorderChanged(object? sender, System.EventArgs? e)
        {
            NativeControl.EditControlOnly = !Control.HasBorder;
        }

        protected override void OnDetach()
        {
            base.OnDetach();
            Control.TextChanged -= Control_TextChanged;
            NativeControl.TextChanged -= NativeControl_TextChanged;
            Control.HasBorderChanged -= Control_HasBorderChanged;
            Control.MultilineChanged -= Control_MultilineChanged;
            Control.ReadOnlyChanged -= Control_ReadOnlyChanged;
        }

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