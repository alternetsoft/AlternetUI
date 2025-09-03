using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;

namespace Alternet.UI.Native
{
    internal partial class MainMenu : IMainMenuHandler
    {
        private Alternet.UI.MainMenu? control;

        public MainMenu(Alternet.UI.MainMenu control)
            : this()
        {
            this.control = control;
            OnAttach();
        }

        public override void OnPlatformEventHandleDestroyed()
        {
            base.OnPlatformEventHandleDestroyed();
        }

        public override void OnPlatformEventHandleCreated()
        {
            base.OnPlatformEventHandleCreated();
        }

        public Alternet.UI.MainMenu? Control
        {
            get { return control; }
        }

        protected void OnAttach()
        {
            if (Control is null)
                return;

            ApplyItems();

            Control.Items.ItemInserted += Items_ItemInserted;
            Control.Items.ItemRemoved += Items_ItemRemoved;
        }

        protected void OnDetach()
        {
            if (Control is null)
                return;

            Control.Items.ItemInserted -= Items_ItemInserted;
            Control.Items.ItemRemoved -= Items_ItemRemoved;

            foreach (var item in Control.Items)
                item.TextChanged -= Item_TextChanged;

            control = null;
        }

        protected override void DisposeManaged()
        {
            OnDetach();
            base.DisposeManaged();
        }

        private void ApplyItems()
        {
            if (Control is null)
                return;
            for (var i = 0; i < Control.Items.Count; i++)
                InsertItem(Control.Items[i], i);
        }

        private void InsertItem(Alternet.UI.MenuItem item, int index)
        {
            var host = Native.Menu.GetHostObject(item);

            var submenu = host.EnsureNativeSubmenuCreated() ?? throw new Exception();

            InsertItemAt(index, submenu, item.Text);

            item.TextChanged += Item_TextChanged;
        }

        private void Item_TextChanged(object? sender, EventArgs e)
        {
            if (Control is null)
                return;
            var item = (Alternet.UI.MenuItem)sender!;
            SetItemText(Control.Items.IndexOf(item), item.Text);
        }

        private void Items_ItemInserted(object? sender, int index, Alternet.UI.MenuItem item)
        {
            InsertItem(item, index);
        }

        private void Items_ItemRemoved(object? sender, int index, Alternet.UI.MenuItem item)
        {
            item.TextChanged -= Item_TextChanged;
            RemoveItemAt(index);
        }
    }
}
