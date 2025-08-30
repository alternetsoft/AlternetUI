using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;

namespace Alternet.UI.Native
{
    internal partial class Menu : IContextMenuHandler
    {
        private ContextMenu? control;

        private Action? onClose;
        internal Native.MenuItem? OwnerHandler;

        public Menu(Alternet.UI.ContextMenu contextMenu)
            : this()
        {
            control = contextMenu;
            OnAttach();
        }

        public Alternet.UI.ContextMenu? Control => control;

        public static Native.MenuItem GetHostObject(Alternet.UI.MenuItem item)
        {
            var host = item.GetHostObject<Native.MenuItem>();
            if (host is null)
            {
                host = new Native.MenuItem(item);
                item.AddHostObject(host);
            }
            return host;
        }

        public void OnPlatformEventOpened()
        {
            (OwnerHandler?.Control as UI.MenuItem)?.RaiseOpened();
        }

        public void OnPlatformEventClosed()
        {
            (OwnerHandler?.Control as UI.MenuItem)?.RaiseClosed();
            onClose?.Invoke();
        }

        void IContextMenuHandler.Show(AbstractControl control, Drawing.PointD? position, Action? onClose)
        {
            Opened = OnPlatformEventOpened;
            Closed = OnPlatformEventClosed;
            this.onClose = onClose;
            Show(control, position);
        }

        protected void OnAttach()
        {
            if (Control is null)
                return;

            ApplyItems();

            Control.Items.ItemInserted += Items_ItemInserted;
            Control.Items.ItemRemoved += Items_ItemRemoved;
        }

        override protected void DisposeManaged()
        {
            OnDetach();
            base.DisposeManaged();
        }

        protected void OnDetach()
        {
            if (Control is null)
                return;

            Control.Items.ItemInserted -= Items_ItemInserted;
            Control.Items.ItemRemoved -= Items_ItemRemoved;
            control = null;
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
            var host = GetHostObject(item);
            InsertItemAt(index, host);
        }

        private void Items_ItemInserted(object? sender, int index, Alternet.UI.MenuItem item)
        {
            InsertItem(item, index);
        }

        private void Items_ItemRemoved(object? sender, int index, Alternet.UI.MenuItem item)
        {
            RemoveItemAt(index);
        }

        private void Show(AbstractControl control, Alternet.Drawing.PointD? position = null)
        {
            if (position == null)
                ShowUnder();
            else
            {
                if (position != Alternet.Drawing.PointD.MinusOne)
                    ShowContextMenu(control, position.Value);
                else
                {
                    ((UI.Native.Control)control.NativeControl).ShowPopupMenu(
                        MenuHandle,
                        -1,
                        -1);
                }
            }

            void ShowUnder()
            {
                var window = control.ParentWindow;
                if (window == null)
                    return;
                var toolRect = control.Bounds;
                var pt = control.Parent!.ClientToScreen(toolRect.BottomLeft);
                pt = window.ScreenToClient(pt);
                ((UI.Native.Control)control.NativeControl).ShowPopupMenu(
                    MenuHandle,
                    (int)pt.X,
                    (int)pt.Y);
            }
        }

        /// <summary>
        /// Displays the shortcut menu at the specified position.
        /// </summary>
        /// <param name="control">A <see cref="AbstractControl"/> that specifies the control with which
        /// this shortcut menu is associated.</param>
        /// <param name="position">
        /// A <see cref="Alternet.Drawing.PointD"/> that specifies the
        /// coordinates at which to display the menu.
        /// These coordinates are specified relative
        /// to the client coordinates of the control specified in the control parameter.</param>
        private void ShowContextMenu(IControl control, Alternet.Drawing.PointD position)
        {
            if (control is null)
                throw new ArgumentNullException(nameof(control));

            ShowContextMenu(
                (UI.Native.Control)control.NativeControl ?? throw new Exception(), position);
        }
    }
}
