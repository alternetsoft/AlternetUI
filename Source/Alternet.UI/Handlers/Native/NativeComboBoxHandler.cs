using System;
using System.Collections.Generic;
using Alternet.Base.Collections;

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
            ApplySelectedItem();
            ApplyText();

            Control.Items.ItemInserted += Items_ItemInserted;
            Control.Items.ItemRemoved += Items_ItemRemoved;
            Control.IsEditableChanged += Control_IsEditableChanged;
            Control.TextChanged += Control_TextChanged;
            Control.SelectedItemChanged += Control_SelectedItemChanged;

            NativeControl.SelectedItemChanged += NativeControl_SelectedItemChanged;
            NativeControl.TextChanged += NativeControl_TextChanged;
        }

        private void Control_IsEditableChanged(object? sender, EventArgs e)
        {
            ApplyIsEditable();
            
            if (Control.Parent != null)
                Control.Parent.PerformLayout(); // preferred size may change, so relayout parent.
        }

        protected override void OnDetach()
        {
            Control.Items.ItemInserted -= Items_ItemInserted;
            Control.Items.ItemRemoved -= Items_ItemRemoved;
            Control.IsEditableChanged -= Control_IsEditableChanged;

            Control.SelectedItemChanged -= Control_SelectedItemChanged;
            NativeControl.SelectedItemChanged -= NativeControl_SelectedItemChanged;

            base.OnDetach();
        }

        private void NativeControl_SelectedItemChanged(object? sender, EventArgs e)
        {
            ReceiveSelectedItem();
        }

        private void Control_SelectedItemChanged(object? sender, EventArgs e)
        {
            ApplySelectedItem();
        }

        private void NativeControl_TextChanged(object? sender, EventArgs e)
        {
            ReceiveText();
        }

        private void Control_TextChanged(object? sender, EventArgs e)
        {
            ApplyText();
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

        private void ApplySelectedItem()
        {
            NativeControl.SelectedIndex = Control.SelectedIndex ?? -1;
        }

        private void ApplyText()
        {
            NativeControl.Text = Control.Text;
        }

        private void ReceiveSelectedItem()
        {
            var selectedIndex = NativeControl.SelectedIndex;
            Control.SelectedIndex = selectedIndex == -1 ? null : selectedIndex;
        }

        private void ReceiveText()
        {
            Control.Text = NativeControl.Text;
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