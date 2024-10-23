using System;
using System.Collections.Specialized;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class ListBoxHandler : WxControlHandler, IListBoxHandler
    {
        private bool receivingSelection;
        private bool applyingSelection;

        public ListBoxHandler()
        {
        }

        /// <summary>
        /// Gets a <see cref="ListBox"/> this handler provides the
        /// implementation for.
        /// </summary>
        public new ListBox Control => (ListBox)base.Control;

        /// <summary>
        /// Gets or sets a value indicating whether the control has a border.
        /// </summary>
        public virtual bool HasBorder
        {
            get
            {
                return NativeControl.HasBorder;
            }

            set
            {
                if (HasBorder == value)
                    return;
                NativeControl.HasBorder = value;
            }
        }

        internal new Native.ListBox NativeControl => (Native.ListBox)base.NativeControl;

        public virtual void EnsureVisible(int itemIndex)
        {
            NativeControl.EnsureVisible(itemIndex);
        }

        public virtual int? HitTest(PointD position)
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

            if (App.IsWindowsOS)
                UserPaint = true;

            bool? value = AllPlatformDefaults.GetHasBorderOverride(Control.ControlKind);
            if (value is not null)
                HasBorder = value.Value;

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

        private void Items_ItemInserted(object? sender, int index, object item)
        {
            NativeControl.InsertItem(index, Control.GetItemText(item, false));
        }

        private void Items_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == NotifyCollectionChangedAction.Replace)
            {
                var item = e.NewItems?[0];
                var index = e.NewStartingIndex;
                var text = Control.GetItemText(item, false);
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