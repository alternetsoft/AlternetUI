using System;
using System.Collections.Specialized;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class VListBoxHandler : ListBoxHandler
    {
        private bool receivingSelection;
        private bool applyingSelection;

        /// <summary>
        /// Gets a <see cref="VListBox"/> this handler provides the
        /// implementation for.
        /// </summary>
        public new VListBox Control => (VListBox)base.Control;

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

        internal new Native.VListBox NativeControl =>
            (Native.VListBox)base.NativeControl!;

        /// <inheritdoc/>
        public override void EnsureVisible(int itemIndex)
        {
            if(itemIndex >= 0 && NativeControl.ItemsCount > 0)
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
            return new NativeVListBox();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            ApplySelectionMode();
            NativeControl.ItemsCount = Control.Items.Count;
            ApplySelection();

            Control.Items.ItemInserted += Items_ItemInserted;
            Control.Items.ItemRemoved += Items_ItemRemoved;
            Control.Items.CollectionChanged += Items_CollectionChanged;
            Control.SelectionModeChanged += Control_SelectionModeChanged;
            Control.SelectionChanged += Control_SelectionChanged;

            NativeControl.SelectionChanged = NativeControl_SelectionChanged;
            NativeControl.MeasureItem = NativeControl_MeasureItem;
        }

        protected override void OnDetach()
        {
            Control.Items.ItemInserted -= Items_ItemInserted;
            Control.Items.ItemRemoved -= Items_ItemRemoved;
            Control.SelectionModeChanged -= Control_SelectionModeChanged;
            Control.Items.CollectionChanged -= Items_CollectionChanged;
            Control.SelectionChanged -= Control_SelectionChanged;
            NativeControl.MeasureItem = null;
            NativeControl.SelectionChanged = null;

            base.OnDetach();
        }

        private void NativeControl_MeasureItem()
        {
            var itemIndex = NativeControl.EventItem;
            var heightDip = Control.MeasureItemSize(itemIndex).Height;
            var height = Control.PixelFromDip(heightDip);
            NativeControl.EventHeight = height;
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

        private void ApplySelection()
        {
            if (Control.SelectionMode == ListBoxSelectionMode.Single)
            {
                var indices = Control.SelectedIndices;
                if(indices.Count > 0)
                    NativeControl.SetSelection(indices[0]);
                else
                    NativeControl.SetSelection(-1);
            }

            applyingSelection = true;

            try
            {
                NativeControl.ClearSelected();

                var indices = Control.SelectedIndices;

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
                if (Control.SelectionMode == ListBoxSelectionMode.Single)
                {
                    Control.SelectedIndices = new int[] { NativeControl.GetSelection() };
                    return;
                }

                var selCount = NativeControl.GetSelectedCount();

                if (selCount == 0)
                {
                    Control.SelectedIndices = Array.Empty<int>();
                    return;
                }

                var result = new List<int>(selCount + 1);
                var firstSelected = NativeControl.GetFirstSelected();
                result.Add(firstSelected);

                while (true)
                {
                    var selected = NativeControl.GetNextSelected();
                    if (selected < 0)
                        break;
                    result.Add(selected);
                }

                Control.SelectedIndices = result;
            }
            finally
            {
                receivingSelection = false;
            }
        }

        private void Items_ItemInserted(object? sender, int index, object item)
        {
            NativeControl.ItemsCount = Control.Items.Count;
        }

        private void Items_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
        }

        private void Items_ItemRemoved(object? sender, int index, object item)
        {
            NativeControl.ItemsCount = Control.Items.Count;
        }

        private class NativeVListBox : Native.VListBox
        {
            public NativeVListBox()
            {
                SetNativePointer(NativeApi.VListBox_CreateEx_(1));
            }

            public NativeVListBox(IntPtr nativePointer)
                : base(nativePointer)
            {
            }
        }
    }
}