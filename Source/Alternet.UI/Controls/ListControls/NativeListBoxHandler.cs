using System;
using System.Collections.Specialized;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class NativeListBoxHandler : ListBoxHandler
    {
        private bool receivingSelection;

        private bool applyingSelection;

        internal new Native.ListBox NativeControl =>
            (Native.ListBox)base.NativeControl!;

        /// <inheritdoc/>
        public override void EnsureVisible(int itemIndex)
        {
            NativeControl.EnsureVisible(itemIndex);
        }

        /// <inheritdoc/>
        public override int? HitTest(PointD position)
        {
            int index = NativeControl.ItemHitTest(position);
            return index == -1 ? null : index;
        }

        internal override Native.Control CreateNativeControl()
        {
            return new NativeListBox();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            ApplyItems();
            ApplySelectionMode();
            ApplySelection();

            Control.Items.ItemInserted += Items_ItemInserted;
            Control.Items.ItemRemoved += Items_ItemRemoved;
            Control.Items.CollectionChanged += Items_CollectionChanged;
            Control.SelectionModeChanged += Control_SelectionModeChanged;

            Control.SelectionChanged += Control_SelectionChanged;
            NativeControl.SelectionChanged = NativeControl_SelectionChanged;
        }

        protected override void OnDetach()
        {
            Control.Items.ItemInserted -= Items_ItemInserted;
            Control.Items.ItemRemoved -= Items_ItemRemoved;
            Control.SelectionModeChanged -= Control_SelectionModeChanged;
            Control.Items.CollectionChanged -= Items_CollectionChanged;

            Control.SelectionChanged -= Control_SelectionChanged;
            NativeControl.SelectionChanged = null;

            base.OnDetach();
        }

        private void NativeControl_SelectionChanged()
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

        private void Items_ItemInserted(object? sender, int index, object item)
        {
            NativeControl.InsertItem(index, Control.GetItemText(item));
        }

        private void Items_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == NotifyCollectionChangedAction.Replace)
            {
                var item = e.NewItems?[0];
                var index = e.NewStartingIndex;
                var text = Control.GetItemText(item);
                NativeControl.SetItem(index, text);
            }
        }

        private void Items_ItemRemoved(object? sender, int index, object item)
        {
            NativeControl.RemoveItemAt(index);
        }

        private class NativeListBox : Native.ListBox
        {
            public NativeListBox()
            {
                SetNativePointer(NativeApi.ListBox_CreateEx_(1));
            }

            public NativeListBox(IntPtr nativePointer)
                : base(nativePointer)
            {
            }
        }
    }
}