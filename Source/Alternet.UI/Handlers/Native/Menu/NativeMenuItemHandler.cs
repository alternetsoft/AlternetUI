using Alternet.Base.Collections;
using System;

namespace Alternet.UI
{
    internal class NativeMenuItemHandler : NativeControlHandler<MenuItem, Native.MenuItem>
    {
        internal override Native.Control CreateNativeControl()
        {
            return new Native.MenuItem();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            ApplyText();
            ApplyChecked();
            ApplyItems();
            ApplyShortcut();
            ApplyRole();

            Control.TextChanged += Control_TextChanged;
            Control.CheckedChanged += Control_CheckedChanged;
            Control.ShortcutChanged += Control_ShortcutChanged;
            Control.RoleChanged += Control_RoleChanged;

            NativeControl.Click += NativeControl_Click;

            Control.Items.ItemInserted += Items_ItemInserted;
            Control.Items.ItemRemoved += Items_ItemRemoved;
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

        protected override void OnDetach()
        {
            base.OnDetach();

            Control.CheckedChanged -= Control_CheckedChanged;
            Control.TextChanged -= Control_TextChanged;
            Control.ShortcutChanged -= Control_ShortcutChanged;
            Control.RoleChanged -= Control_RoleChanged;

            NativeControl.Click -= NativeControl_Click;

            Control.Items.ItemInserted -= Items_ItemInserted;
            Control.Items.ItemRemoved -= Items_ItemRemoved;
        }

        private void Items_ItemInserted(object? sender, CollectionChangeEventArgs<MenuItem> e)
        {
            InsertItem(e.Item, e.Index);
        }

        private void Items_ItemRemoved(object? sender, CollectionChangeEventArgs<MenuItem> e)
        {
            (NativeControl.Submenu ?? throw new Exception()).RemoveItemAt(e.Index);
        }

        void InsertItem(MenuItem item, int index)
        {
            var handler = item.Handler as NativeMenuItemHandler;
            if (handler == null)
                throw new InvalidOperationException();

            EnsureNativeSubmenuCreated().InsertItemAt(index, handler.NativeControl);
        }

        internal Native.Menu EnsureNativeSubmenuCreated()
        {
            if (NativeControl.Submenu == null)
                NativeControl.Submenu = new Native.Menu();

            return NativeControl.Submenu;
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