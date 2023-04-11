using System;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class NativeCheckListBoxHandler : CheckListBoxHandler
    {
        private bool applyingCheckedIndices;
        private bool receivingCheckedIndices;
        private bool receivingSelection;
        private bool applyingSelection;

        internal override Native.Control CreateNativeControl()
        {
            return new Native.CheckListBox();
        }

        public new Native.CheckListBox NativeControl => (Native.CheckListBox)base.NativeControl!;
        
        public new CheckListBox Control => (CheckListBox)base.Control;

        protected override void OnAttach()
        {
            base.OnAttach();

            ApplyItems();
            ApplySelectionMode();
            ApplySelection();
            ApplyCheckedIndices();

            Control.Items.ItemInserted += Items_ItemInserted;
            Control.Items.ItemRemoved += Items_ItemRemoved;
            Control.SelectionModeChanged += Control_SelectionModeChanged;

            Control.SelectionChanged += Control_SelectionChanged;
            NativeControl.SelectionChanged += NativeControl_SelectionChanged;

            Control.CheckedChanged += Control_CheckedChanged;
            NativeControl.CheckedChanged += NativeControl_CheckedChanged;
        }

        protected override void OnDetach()
        {
            Control.Items.ItemInserted -= Items_ItemInserted;
            Control.Items.ItemRemoved -= Items_ItemRemoved;
            Control.SelectionModeChanged -= Control_SelectionModeChanged;

            Control.SelectionChanged -= Control_SelectionChanged;
            NativeControl.SelectionChanged -= NativeControl_SelectionChanged;

            Control.CheckedChanged -= Control_CheckedChanged;
            NativeControl.CheckedChanged -= NativeControl_CheckedChanged;

            base.OnDetach();
        }

        private void NativeControl_SelectionChanged(object? sender, EventArgs e)
        {
            if (applyingSelection)
                return;

            ReceiveSelection();
        }

        private void Control_SelectionChanged(object? sender, EventArgs e)
        {
            if (receivingSelection)
                return;

            ApplySelection();
        }

        private void Control_SelectionModeChanged(object? sender, EventArgs e)
        {
            ApplySelectionMode();
        }

        private void NativeControl_CheckedChanged(object? sender, EventArgs e)
        {
            if (applyingCheckedIndices)
                return;

            ReceiveCheckedIndices();
        }

        private void Control_CheckedChanged(object? sender, EventArgs e)
        {
            if (receivingCheckedIndices)
                return;

            ApplyCheckedIndices();
        }


        private void ApplySelectionMode()
        {
            NativeControl.SelectionMode = (Native.ListBoxSelectionMode)Control.SelectionMode;
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
            applyingSelection = true;

            try
            {
                var nativeControl = NativeControl;
                nativeControl.ClearSelected();

                var control = Control;
                var indices = control.SelectedIndices;

                for (var i = 0; i < indices.Count; i++)
                    NativeControl.SetSelected(indices[i], true);
            }
            finally
            {
                applyingSelection = false;
            }
        }

        private void ReceiveSelection()
        {
            receivingSelection = true;

            try
            {
                Control.SelectedIndices = NativeControl.SelectedIndices;
            }
            finally
            {
                receivingSelection = false;
            }
        }

        private void ApplyCheckedIndices()
        {
            applyingCheckedIndices = true;

            try
            {
                var nativeControl = NativeControl;
                nativeControl.ClearChecked();

                var control = Control;
                var indices = control.CheckedIndices;

                for (var i = 0; i < indices.Count; i++)
                    NativeControl.SetChecked(indices[i], true);
            }
            finally
            {
                applyingCheckedIndices = false;
            }
        }

        private void ReceiveCheckedIndices()
        {
            receivingCheckedIndices = true;

            try
            {
                Control.CheckedIndices = NativeControl.CheckedIndices;
            }
            finally
            {
                receivingCheckedIndices = false;
            }
        }
        private void Items_ItemInserted(object? sender, CollectionChangeEventArgs<object> e)
        {
            NativeControl.InsertItem(e.Index, Control.GetItemText(e.Item));
        }

        private void Items_ItemRemoved(object? sender, CollectionChangeEventArgs<object> e)
        {
            NativeControl.RemoveItemAt(e.Index);
        }

        /// <inheritdoc/>
        public override void EnsureVisible(int itemIndex)
        {
            NativeControl.EnsureVisible(itemIndex);
        }

        /// <inheritdoc/>
        public override int? HitTest(Point position)
        {
            int index = NativeControl.ItemHitTest(position);
            return index == -1 ? null : index;
        }
    }
}