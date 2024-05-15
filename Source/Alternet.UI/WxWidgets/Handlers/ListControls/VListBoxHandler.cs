using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class VListBoxHandler : WxControlHandler, IVListBoxHandler
    {
        private bool receivingSelection;
        private bool applyingSelection;

        public VListBoxHandler()
        {
        }

        public int ItemsCount
        {
            get => NativeControl.ItemsCount;
            set => NativeControl.ItemsCount = value;
        }

        /// <summary>
        /// Gets a <see cref="VListBox"/> this handler provides the
        /// implementation for.
        /// </summary>
        public new VListBox Control => (VListBox)base.Control;

        /// <summary>
        /// Gets or sets a value indicating whether the control has a border.
        /// </summary>
        public bool HasBorder
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

        bool IVListBoxHandler.HScrollBarVisible
        {
            get => NativeControl.HScrollBarVisible;
            set => NativeControl.HScrollBarVisible = value;
        }

        bool IVListBoxHandler.VScrollBarVisible
        {
            get => NativeControl.VScrollBarVisible;
            set => NativeControl.VScrollBarVisible = value;
        }

        ListBoxSelectionMode IVListBoxHandler.SelectionMode
        {
            get => (ListBoxSelectionMode)NativeControl.SelectionMode;
            set => NativeControl.SelectionMode = (Native.ListBoxSelectionMode)value;
        }

        internal new Native.VListBox NativeControl => (Native.VListBox)base.NativeControl;

        public void EnsureVisible(int itemIndex)
        {
            if(itemIndex >= 0 && NativeControl.ItemsCount > 0)
                NativeControl.EnsureVisible(itemIndex);
        }

        public int? HitTest(PointD position)
        {
            int index = NativeControl.ItemHitTest(position);
            return index == -1 ? null : index;
        }

        RectD? IVListBoxHandler.GetItemRect(int index)
        {
            var resultI = NativeControl.GetItemRectI(index);
            if (resultI.SizeIsEmpty)
                return null;
            var resultD = Control.PixelToDip(resultI);
            return resultD;
        }

        bool IVListBoxHandler.ScrollRows(int rows)
        {
            return NativeControl.ScrollRows(rows);
        }

        bool IVListBoxHandler.ScrollRowPages(int pages)
        {
            return NativeControl.ScrollRowPages(pages);
        }

        void IVListBoxHandler.RefreshRow(int row)
        {
            NativeControl.RefreshRow(row);
        }

        void IVListBoxHandler.RefreshRows(int from, int to)
        {
            NativeControl.RefreshRows(from, to);
        }

        int IVListBoxHandler.GetVisibleEnd()
        {
            return NativeControl.GetVisibleEnd();
        }

        int IVListBoxHandler.GetVisibleBegin()
        {
            return NativeControl.GetVisibleBegin();
        }

        bool IVListBoxHandler.IsSelected(int line)
        {
            return NativeControl.IsSelected(line);
        }

        bool IVListBoxHandler.IsVisible(int line)
        {
            return NativeControl.IsVisible(line);
        }

        void IVListBoxHandler.ClearItems()
        {
            NativeControl.ClearItems();
        }

        void IVListBoxHandler.ClearSelected()
        {
            NativeControl.ClearSelected();
        }

        void IVListBoxHandler.SetSelected(int index, bool value)
        {
            NativeControl.SetSelected(index, value);
        }

        int IVListBoxHandler.GetFirstSelected()
        {
            return NativeControl.GetFirstSelected();
        }

        int IVListBoxHandler.GetNextSelected()
        {
            return NativeControl.GetNextSelected();
        }

        int IVListBoxHandler.GetSelectedCount()
        {
            return NativeControl.GetSelectedCount();
        }

        int IVListBoxHandler.GetSelection()
        {
            return NativeControl.GetSelection();
        }

        int IVListBoxHandler.ItemHitTest(PointD position)
        {
            return NativeControl.ItemHitTest(position);
        }

        void IVListBoxHandler.SetSelection(int selection)
        {
            NativeControl.SetSelection(selection);
        }

        void IVListBoxHandler.SetSelectionBackground(Color color)
        {
            NativeControl.SetSelectionBackground(color);
        }

        bool IVListBoxHandler.IsCurrent(int current)
        {
            return NativeControl.IsCurrent(current);
        }

        bool IVListBoxHandler.DoSetCurrent(int current)
        {
            return NativeControl.DoSetCurrent(current);
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
            Control.Invalidate();
        }

        private void Items_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
        }

        private void Items_ItemRemoved(object? sender, int index, object item)
        {
            NativeControl.ItemsCount = Control.Items.Count;
            Control.Invalidate();
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