using System;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class NativeContextMenuHandler : NativeControlHandler<ContextMenu, Native.Menu>
    {
        /// <summary>
        /// Displays the shortcut menu at the specified position.
        /// </summary>
        /// <param name="control">A <see cref="Control"/> that specifies the control with which
        /// this shortcut menu is associated.</param>
        /// <param name="position">
        /// A <see cref="Point"/> that specifies the coordinates at which to display the menu.
        /// These coordinates are specified relative
        /// to the client coordinates of the control specified in the control parameter.</param>
        public void Show(Control control, Point position)
        {
            if (control is null)
                throw new ArgumentNullException(nameof(control));

            NativeControl.ShowContextMenu(
                control.Handler.NativeControl ?? throw new Exception(), position);
        }

        internal override Native.Control CreateNativeControl()
        {
            return new Native.Menu();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            ApplyItems();

            Control.Items.ItemInserted += Items_ItemInserted;
            Control.Items.ItemRemoved += Items_ItemRemoved;
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            Control.Items.ItemInserted -= Items_ItemInserted;
            Control.Items.ItemRemoved -= Items_ItemRemoved;
        }

        private void ApplyItems()
        {
            for (var i = 0; i < Control.Items.Count; i++)
                InsertItem(Control.Items[i], i);
        }

        private void InsertItem(MenuItem item, int index)
        {
            if (item.Handler is not NativeMenuItemHandler handler)
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