using System;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    internal class NativeListBoxHandler : NativeControlHandler<ListBox, Native.ListBox>
    {
        private bool receivingSelection;

        private bool applyingSelection;

        internal override Native.Control CreateNativeControl()
        {
            return new Native.ListBox();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            ApplyItems();
            ApplySelectionMode();
            ApplySelection();

            Control.Items.ItemInserted += Items_ItemInserted;
            Control.Items.ItemRemoved += Items_ItemRemoved;
            Control.SelectionModeChanged += Control_SelectionModeChanged;

            Control.SelectionChanged += Control_SelectionChanged;
            NativeControl.SelectionChanged += NativeControl_SelectionChanged;
        }

        protected override void OnDetach()
        {
            Control.Items.ItemInserted -= Items_ItemInserted;
            Control.Items.ItemRemoved -= Items_ItemRemoved;
            Control.SelectionModeChanged -= Control_SelectionModeChanged;

            Control.SelectionChanged -= Control_SelectionChanged;
            NativeControl.SelectionChanged -= NativeControl_SelectionChanged;

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