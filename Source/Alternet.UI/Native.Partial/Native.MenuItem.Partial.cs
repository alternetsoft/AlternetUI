using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;

namespace Alternet.UI.Native
{
    internal partial class MenuItem : IMenuItemHandler
    {
        private Alternet.UI.MenuItem? control;

        public MenuItem(Alternet.UI.MenuItem control)
            : this()
        {
            this.control = control;
            OnAttach();
            Click = OnPlatformEventClick;
            Highlight = OnPlatformEventHighlight;
            Opened = OnPlatformEventOpened;
            Closed = OnPlatformEventClosed;
        }

        public Alternet.UI.MenuItem? Control => control;

        public void OnPlatformEventClick()
        {
            (Control as UI.MenuItem)?.RaiseClick();
        }

        public void OnPlatformEventHighlight()
        {
            (Control as UI.MenuItem)?.RaiseHighlighted();
        }

        public void OnPlatformEventOpened()
        {
            (Control as UI.MenuItem)?.RaiseOpened();
        }

        public void OnPlatformEventClosed()
        {
            (Control as UI.MenuItem)?.RaiseClosed();
        }

        internal Native.Menu EnsureNativeSubmenuCreated()
        {
            if (Submenu == null)
            {
                Submenu = new Native.Menu();
                Submenu.OwnerHandler = this;
            }

            return Submenu;
        }

        protected void OnDetach()
        {
            if (Control is null)
                return;
            Control.CheckedChanged -= Control_CheckedChanged;
            Control.TextChanged -= Control_TextChanged;
            Control.ShortcutChanged -= Control_ShortcutChanged;
            Control.RoleChanged -= Control_RoleChanged;
            Control.ImageChanged -= Control_ImageChanged;
            Control.DisabledImageChanged -= Control_DisabledImageChanged;
            Control.Items.ItemInserted -= Items_ItemInserted;
            Control.Items.ItemRemoved -= Items_ItemRemoved;
            control = null;
        }

        protected override void DisposeManaged()
        {
            OnDetach();
            base.DisposeManaged();
        }

        protected void OnAttach()
        {
            if (Control is null)
                return;
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

            Control.Items.ItemInserted += Items_ItemInserted;
            Control.Items.ItemRemoved += Items_ItemRemoved;
        }

        private void ApplyDisabledImage()
        {
            if (Control is null)
                return;
            var image = Control.GetRealImage(VisualControlState.Disabled);
            DisabledImage = (UI.Native.ImageSet?)image?.Handler;
        }

        private void ApplyImage()
        {
            if (Control is null)
                return;
            var image = Control.GetRealImage(VisualControlState.Normal);
            NormalImage = (UI.Native.ImageSet?)image?.Handler;
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
            if (Control is null)
                return;
            for (var i = 0; i < Control.Items.Count; i++)
                InsertItem(Control.Items[i], i);
        }

        private void ApplyText()
        {
            if (Control is null)
                return;
            Text = Control.Text;
        }

        private void ApplyShortcut()
        {
            if (Control is null)
                return;
            if (!Control.IsShortcutEnabled)
            {
                SetShortcut(Key.None, ModifierKeys.None);
                return;
            }

            var shortcut = Control.Shortcut;

            Key key;
            ModifierKeys modifierKeys;

            if (shortcut == null)
            {
                key = Key.None;
                modifierKeys = ModifierKeys.None;
            }
            else
            {
                key = shortcut.Key;
                modifierKeys = shortcut.Modifiers;
            }

            SetShortcut(key, modifierKeys);
        }

        private void ApplyRole()
        {
            if (Control is null)
                return;
            Role = Control.Role?.Name ?? string.Empty;
        }

        private void ApplyChecked()
        {
            if (Control is null)
                return;
            Checked = Control.Checked;
        }

        private void Items_ItemInserted(object? sender, int index, Alternet.UI.MenuItem item)
        {
            InsertItem(item, index);
        }

        private void Items_ItemRemoved(object? sender, int index, Alternet.UI.MenuItem item)
        {
            (Submenu ?? throw new Exception()).RemoveItemAt(index);
        }

        private void InsertItem(Alternet.UI.MenuItem item, int index)
        {
            var host = Native.Menu.GetHostObject(item);

            EnsureNativeSubmenuCreated().InsertItemAt(index, host);
        }

        private void Control_TextChanged(object? sender, EventArgs e)
        {
            ApplyText();
        }
    }
}
