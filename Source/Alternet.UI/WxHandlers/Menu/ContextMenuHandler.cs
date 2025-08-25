using System;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class ContextMenuHandler : WxControlHandler<ContextMenu, Native.Menu>, IContextMenuHandler
    {
        public ContextMenuHandler()
        {
        }

        public void Show(AbstractControl control, PointD? position = null)
        {
            if (Control is null)
                return;

            if (position == null)
                ShowUnder();
            else
            {
                if (position != PointD.MinusOne)
                    ShowContextMenu(control, (PointD)position);
                else
                {
                    ((UI.Native.Control)control.NativeControl).ShowPopupMenu(
                        MenuItemHandler.GetMenuHandle(Control),
                        -1,
                        -1);
                }
            }

            void ShowUnder()
            {
                var window = control.ParentWindow;
                if (window == null)
                    return;
                RectD toolRect = control.Bounds;
                PointD pt = control.Parent!.ClientToScreen(toolRect.BottomLeft);
                pt = window.ScreenToClient(pt);
                ((UI.Native.Control)control.NativeControl).ShowPopupMenu(
                    MenuItemHandler.GetMenuHandle(Control),
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
        /// A <see cref="PointD"/> that specifies the coordinates at which to display the menu.
        /// These coordinates are specified relative
        /// to the client coordinates of the control specified in the control parameter.</param>
        public void ShowContextMenu(IControl control, PointD position)
        {
            if (control is null)
                throw new ArgumentNullException(nameof(control));

            NativeControl.ShowContextMenu(
                (UI.Native.Control)control.NativeControl ?? throw new Exception(), position);
        }

        internal override Native.Control CreateNativeControl()
        {
            return new Native.Menu();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            if (Control is null)
                return;

            ApplyItems();

            Control.Items.ItemInserted += Items_ItemInserted;
            Control.Items.ItemRemoved += Items_ItemRemoved;
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            if (Control is null)
                return;

            Control.Items.ItemInserted -= Items_ItemInserted;
            Control.Items.ItemRemoved -= Items_ItemRemoved;
        }

        private void ApplyItems()
        {
            if (Control is null)
                return;

            for (var i = 0; i < Control.Items.Count; i++)
                InsertItem(Control.Items[i], i);
        }

        private void InsertItem(MenuItem item, int index)
        {
            if (item.Handler is not MenuItemHandler handler)
                throw new InvalidOperationException();

            NativeControl.InsertItemAt(index, handler.NativeControl);
        }

        private void Items_ItemInserted(object? sender, int index, MenuItem item)
        {
            InsertItem(item, index);
        }

        private void Items_ItemRemoved(object? sender, int index, MenuItem item)
        {
            NativeControl.RemoveItemAt(index);
        }
    }
}