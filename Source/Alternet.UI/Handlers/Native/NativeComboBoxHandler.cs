using System;
using System.Collections.Generic;

namespace Alternet.UI
{
    internal class NativeComboBoxHandler : NativeControlHandler<ComboBox, Native.ComboBox>
    {
        internal override Native.Control CreateNativeControl()
        {
            return new Native.ComboBox();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            ApplyItems();
            ApplyIsEditable();
            ApplySelection();

            Control.Items.ItemInserted += Items_ItemInserted;
            Control.Items.ItemRemoved += Items_ItemRemoved;
            Control.IsEditableChanged += Control_IsEditableChanged;

            Control.SelectionChanged += Control_SelectionChanged;
            NativeControl.SelectionChanged += NativeControl_SelectionChanged;
        }

        private void Control_IsEditableChanged(object? sender, EventArgs e)
        {
            ApplyIsEditable();
        }

        protected override void OnDetach()
        {
            Control.Items.ItemInserted -= Items_ItemInserted;
            Control.Items.ItemRemoved -= Items_ItemRemoved;
            Control.IsEditableChanged -= Control_IsEditableChanged;

            Control.SelectionChanged -= Control_SelectionChanged;
            NativeControl.SelectionChanged -= NativeControl_SelectionChanged;

            base.OnDetach();
        }

        private void NativeControl_SelectionChanged(object? sender, EventArgs e)
        {
            ReceiveSelection();
        }

        private void Control_SelectionChanged(object? sender, EventArgs e)
        {
            ApplySelection();
        }

        private void Control_SelectionModeChanged(object? sender, EventArgs e)
        {
            ApplyIsEditable();
        }

        private void ApplyIsEditable()
        {
            NativeControl.IsEditable = Control.IsEditable;
        }

        private void ApplyItems()
        {
            var nativeControl = NativeControl;
            nativeControl.ClearItems();

            var control = Control;
            var items = control.Items;

            for (var i = 0; i < items.Count; i++)
                NativeControl.InsertItem(i, Control.GetItemText(control.Items[i]));
        }

        private void ApplySelection()
        {
            NativeControl.SelectedIndex = Control.SelectedIndex ?? -1;
        }

        private void ReceiveSelection()
        {
            var selectedIndex = NativeControl.SelectedIndex;
            Control.SelectedIndex = selectedIndex == -1 ? null : selectedIndex;
        }

        private void Items_ItemInserted(object? sender, CollectionChangeEventArgs<object> e)
        {
            NativeControl.InsertItem(e.Index, Control.GetItemText(e.Item));
        }

        private void Items_ItemRemoved(object? sender, CollectionChangeEventArgs<object> e)
        {
            NativeControl.RemoveItemAt(e.Index);
        }
    }
}