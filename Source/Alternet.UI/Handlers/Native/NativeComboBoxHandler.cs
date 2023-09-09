using System;
using System.Collections.Generic;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    internal class NativeComboBoxHandler : ComboBoxHandler
    {
        /// <inheritdoc/>
        public override int TextSelectionStart => NativeControl.TextSelectionStart;

        /// <inheritdoc/>
        public override int TextSelectionLength => NativeControl.TextSelectionLength;

        internal new Native.ComboBox NativeControl =>
            (Native.ComboBox)base.NativeControl!;

        internal override bool HasBorder
        {
            get
            {
                return NativeControl.HasBorder;
            }

            set
            {
                NativeControl.HasBorder = value;
            }
        }

        /// <inheritdoc/>
        public override void SelectTextRange(int start, int length) =>
            NativeControl.SelectTextRange(start, length);

        /// <inheritdoc/>
        public override void SelectAllText() => NativeControl.SelectAllText();

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

            Control.Items.ItemRangeAdditionFinished +=
                Items_ItemRangeAdditionFinished;
            Control.Items.ItemInserted += Items_ItemInserted;
            Control.Items.ItemRemoved += Items_ItemRemoved;
            Control.IsEditableChanged += Control_IsEditableChanged;
            Control.TextChanged += Control_TextChanged;
            Control.SelectedItemChanged += Control_SelectedItemChanged;

            NativeControl.SelectedItemChanged += NativeControl_SelectedItemChanged;
            NativeControl.TextChanged += NativeControl_TextChanged;
        }

        protected override void OnDetach()
        {
            Control.Items.ItemRangeAdditionFinished -=
                Items_ItemRangeAdditionFinished;
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

        private void Control_IsEditableChanged(object? sender, EventArgs e)
        {
            ApplyIsEditable();

            // preferred size may change, so relayout parent.
            Control.Parent?.PerformLayout();
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

            var insertion = NativeControl.CreateItemsInsertion();
            for (var i = 0; i < items.Count; i++)
            {
                NativeControl.AddItemToInsertion(
                    insertion,
                    Control.GetItemText(control.Items[i]));
            }

            NativeControl.CommitItemsInsertion(insertion, 0);
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

        private void Items_ItemInserted(object? sender, int index, object item)
        {
            if (!Control.Items.RangeOperationInProgress)
                NativeControl.InsertItem(index, Control.GetItemText(item));
        }

        private void Items_ItemRemoved(object? sender, int index, object item)
        {
            NativeControl.RemoveItemAt(index);
        }

        private void Items_ItemRangeAdditionFinished(
            object? sender,
            int index,
            IEnumerable<object> items)
        {
            var insertion = NativeControl.CreateItemsInsertion();
            foreach (var item in items)
            {
                NativeControl.AddItemToInsertion(
                    insertion,
                    Control.GetItemText(item));
            }

            NativeControl.CommitItemsInsertion(insertion, index);
        }
    }
}