using System;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class CheckListBoxHandler : ListBoxHandler, ICheckListBoxHandler
    {
        private bool recreateCalled = false;
        private bool applyingCheckedIndices;
        private bool receivingCheckedIndices;
        private bool receivingSelection;
        private bool applyingSelection;

        public CheckListBoxHandler()
        {
        }

        /// <summary>
        /// Gets a <see cref="CheckListBox"/> this handler provides
        /// the implementation for.
        /// </summary>
        public new CheckListBox Control => (CheckListBox)base.Control;

        /// <summary>
        /// Gets or sets a value indicating whether the control has a border.
        /// </summary>
        public override bool HasBorder
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

        public new Native.CheckListBox NativeControl => (Native.CheckListBox)base.NativeControl;

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
            return new Native.CheckListBox();
        }

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
            Control.CheckedChanged += Control_CheckedChanged;

            NativeControl.SelectionChanged = NativeControl_SelectionChanged;
            NativeControl.CheckedChanged = NativeControl_CheckedChanged;
        }

        protected override void OnDetach()
        {
            Control.Items.ItemInserted -= Items_ItemInserted;
            Control.Items.ItemRemoved -= Items_ItemRemoved;
            Control.SelectionModeChanged -= Control_SelectionModeChanged;
            Control.SelectionChanged -= Control_SelectionChanged;
            Control.CheckedChanged -= Control_CheckedChanged;

            NativeControl.SelectionChanged = null;
            NativeControl.CheckedChanged = null;

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

        private void NativeControl_CheckedChanged()
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
            NativeControl.SelectionMode = Control.SelectionMode;
        }

        private void ApplyItems()
        {
            var nativeControl = NativeControl;
            nativeControl.ClearItems();

            var control = Control;
            var items = control.Items;

            for (var i = 0; i < items.Count; i++)
                NativeControl.InsertItem(i, Control.GetItemText(control.Items[i], false));
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

        private void Items_ItemInserted(object? sender, int index, object item)
        {
            // Do not comment this or items will not be painted properly
            if (!recreateCalled)
            {
                recreateCalled = true;
                NativeControl.RecreateWindow();
            }

            NativeControl.InsertItem(index, Control.GetItemText(item, false));
        }

        private void Items_ItemRemoved(object? sender, int index, object item)
        {
            NativeControl.RemoveItemAt(index);
        }
     }
}