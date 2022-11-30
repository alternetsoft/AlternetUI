using System;

namespace Alternet.UI
{
    internal class NativeToolbarItemHandler : NativeControlHandler<ToolbarItem, Native.ToolbarItem>
    {
        internal override Native.Control CreateNativeControl()
        {
            return new Native.ToolbarItem();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            ApplyText();
            ApplyChecked();
            ApplyImage();

            Control.TextChanged += Control_TextChanged;
            Control.ImageChanged += Control_ImageChanged;
            Control.CheckedChanged += Control_CheckedChanged;

            NativeControl.Click += NativeControl_Click;
        }

        private void Control_ImageChanged(object? sender, EventArgs e)
        {
            ApplyImage();
        }

        private void Control_CheckedChanged(object? sender, EventArgs e)
        {
            ApplyChecked();
        }

        private void ApplyText()
        {
            NativeControl.Text = Control.Text;
        }

        private void ApplyChecked()
        {
            NativeControl.Checked = Control.Checked;
        }

        private void ApplyImage()
        {
            NativeControl.Image = Control.Image?.NativeImageSet ?? null;
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            Control.CheckedChanged -= Control_CheckedChanged;
            Control.TextChanged -= Control_TextChanged;
            Control.ImageChanged -= Control_ImageChanged;

            NativeControl.Click -= NativeControl_Click;
        }

        private void NativeControl_Click(object? sender, EventArgs? e)
        {
            Control.Checked = NativeControl.Checked;
            Control.RaiseClick(e ?? throw new ArgumentNullException());
        }

        private void Control_TextChanged(object? sender, EventArgs? e)
        {
            ApplyText();
        }
    }
}