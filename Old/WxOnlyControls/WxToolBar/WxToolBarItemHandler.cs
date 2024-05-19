using System;

namespace Alternet.UI
{
    internal class WxToolBarItemHandler : WxControlHandler
    {
        private Menu? dropDownMenu;

        /// <summary>
        /// Gets a <see cref="WxToolBarItem"/> this handler provides the implementation for.
        /// </summary>
        public new WxToolBarItem Control => (WxToolBarItem)base.Control;

        public new Native.ToolbarItem NativeControl => (Native.ToolbarItem)base.NativeControl!;

        public Menu? DropDownMenu
        {
            get => dropDownMenu;
            set
            {
                dropDownMenu = value;

                if (value == null)
                    NativeControl.DropDownMenu = null;
                else
                    NativeControl.DropDownMenu = ((ContextMenuHandler)value.Handler).NativeControl;
            }
        }

        public bool IsCheckable { get => NativeControl.IsCheckable; set => NativeControl.IsCheckable = value; }

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

            NativeControl.Click = NativeControl_Click;
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            Control.CheckedChanged -= Control_CheckedChanged;
            Control.TextChanged -= Control_TextChanged;
            Control.ImageChanged -= Control_ImageChanged;

            NativeControl.Click = null;
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
            NativeControl.Image = (UI.Native.ImageSet?)(Control.Image?.Handler ?? null);
            NativeControl.DisabledImage =
                (UI.Native.ImageSet?)(Control.DisabledImage?.Handler ?? null);
        }

        private void NativeControl_Click()
        {
            Control.Checked = NativeControl.Checked;
            Control.RaiseClick(EventArgs.Empty);
        }

        private void Control_TextChanged(object? sender, EventArgs? e)
        {
            ApplyText();
        }
    }
}