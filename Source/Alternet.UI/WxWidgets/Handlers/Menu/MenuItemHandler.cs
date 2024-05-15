using System;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    internal class MenuItemHandler
        : NativeControlHandler<MenuItem, Native.MenuItem>, IMenuItemHandler
    {
        public MenuItemHandler()
        {
        }

        internal static IntPtr GetMenuHandle(IControl control)
        {
            var ncontrol = (UI.Native.Menu)control.NativeControl;
            return ncontrol.MenuHandle;
        }

        internal Native.Menu EnsureNativeSubmenuCreated()
        {
            if (NativeControl.Submenu == null)
                NativeControl.Submenu = new Native.Menu();

            return NativeControl.Submenu;
        }

        internal override Native.Control CreateNativeControl()
        {
            return new Native.MenuItem();
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            Control.CheckedChanged -= Control_CheckedChanged;
            Control.TextChanged -= Control_TextChanged;
            Control.ShortcutChanged -= Control_ShortcutChanged;
            Control.RoleChanged -= Control_RoleChanged;
            Control.ImageChanged -= Control_ImageChanged;
            Control.DisabledImageChanged -= Control_DisabledImageChanged;

            NativeControl.Click = null;

            Control.Items.ItemInserted -= Items_ItemInserted;
            Control.Items.ItemRemoved -= Items_ItemRemoved;
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            ApplyText();
            ApplyChecked();
            ApplyItems();
            ApplyShortcut();
            ApplyRole();
            ApplyImage();
            ApplyDisabledImage();

            Control.TextChanged += Control_TextChanged;
            Control.CheckedChanged += Control_CheckedChanged;
            Control.ShortcutChanged += Control_ShortcutChanged;
            Control.RoleChanged += Control_RoleChanged;
            Control.ImageChanged += Control_ImageChanged;
            Control.DisabledImageChanged += Control_DisabledImageChanged;

            NativeControl.Click = NativeControl_Click;

            Control.Items.ItemInserted += Items_ItemInserted;
            Control.Items.ItemRemoved += Items_ItemRemoved;
        }

        private void ApplyDisabledImage()
        {
            NativeControl.DisabledImage = (UI.Native.ImageSet?)Control.DisabledImage?.Handler;
        }

        private void ApplyImage()
        {
            NativeControl.NormalImage = (UI.Native.ImageSet?)Control.Image?.Handler;
        }

        private void Control_DisabledImageChanged(object? sender, EventArgs e)
        {
            ApplyDisabledImage();
        }

        private void Control_ImageChanged(object? sender, EventArgs e)
        {
            ApplyImage();
        }

        private void Control_RoleChanged(object? sender, EventArgs e)
        {
            ApplyRole();
        }

        private void Control_ShortcutChanged(object? sender, EventArgs e)
        {
            ApplyShortcut();
        }

        private void Control_CheckedChanged(object? sender, EventArgs e)
        {
            ApplyChecked();
        }

        private void ApplyItems()
        {
            for (var i = 0; i < Control.Items.Count; i++)
                InsertItem(Control.Items[i], i);
        }

        private void ApplyText()
        {
            NativeControl.Text = Control.Text;
        }

        private void ApplyShortcut()
        {
            var shortcut = Control.Shortcut;

            Native.Key key;
            Native.ModifierKeys modifierKeys;

            if (shortcut == null)
            {
                key = Native.Key.None;
                modifierKeys = Native.ModifierKeys.None;
            }
            else
            {
                key = (Native.Key)shortcut.Key;
                modifierKeys = (Native.ModifierKeys)shortcut.Modifiers;
            }

            NativeControl.SetShortcut(key, modifierKeys);
        }

        private void ApplyRole()
        {
            NativeControl.Role = Control.Role?.Name ?? string.Empty;
        }

        private void ApplyChecked()
        {
            NativeControl.Checked = Control.Checked;
        }

        private void Items_ItemInserted(object? sender, int index, MenuItem item)
        {
            InsertItem(item, index);
        }

        private void Items_ItemRemoved(object? sender, int index, MenuItem item)
        {
            (NativeControl.Submenu ?? throw new Exception()).RemoveItemAt(index);
        }

        private void InsertItem(MenuItem item, int index)
        {
            if (item.Handler is not MenuItemHandler handler)
                throw new InvalidOperationException();

            EnsureNativeSubmenuCreated().InsertItemAt(index, handler.NativeControl);
        }

        private void NativeControl_Click()
        {
            Control.Checked = NativeControl.Checked;
            Control.RaiseClick(EventArgs.Empty);
        }

        private void Control_TextChanged(object? sender, EventArgs e)
        {
            ApplyText();
        }
    }
}