using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class ComboBoxHandler : WxControlHandler, IComboBoxHandler
    {
        private ComboBoxItemPaintEventArgs? paintEventArgs;

        [Flags]
        private enum DrawItemFlags
        {
            // when set, we are painting the selected item in control,
            // not in the popup
            PaintingControl = 0x0001,

            // when set, we are painting an item which should have
            // focus rectangle painted in the background. Text colour
            // and clipping region are then appropriately set in
            // the default OnDrawBackground implementation.
            PaintingSelected = 0x0002,
        }

        /// <summary>
        /// Returns <see cref="ComboBox"/> instance with which this handler
        /// is associated.
        /// </summary>
        public new ComboBox Control => (ComboBox)base.Control;

        public int TextSelectionStart => NativeControl.TextSelectionStart;

        public int TextSelectionLength => NativeControl.TextSelectionLength;

        public ComboBox.OwnerDrawFlags OwnerDrawStyle
        {
            get
            {
                return (ComboBox.OwnerDrawFlags)NativeControl.OwnerDrawStyle;
            }

            set
            {
                if (OwnerDrawStyle == value)
                    return;
                NativeControl.OwnerDrawStyle = (int)value;
            }
        }

        public virtual string? EmptyTextHint
        {
            get
            {
                return NativeControl.EmptyTextHint;
            }

            set
            {
                value ??= string.Empty;
                NativeControl.EmptyTextHint = value;
            }
        }

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

        public PointI TextMargins => NativeControl.TextMargins;

        internal new Native.ComboBox NativeControl =>
            (Native.ComboBox)base.NativeControl!;

        public void DefaultOnDrawBackground() => NativeControl.DefaultOnDrawBackground();

        public void DefaultOnDrawItem() => NativeControl.DefaultOnDrawItem();

        public void SelectTextRange(int start, int length) =>
            NativeControl.SelectTextRange(start, length);

        public void SelectAllText() => NativeControl.SelectAllText();

        internal override Native.Control CreateNativeControl()
        {
            return new Native.ComboBox();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            if (App.IsWindowsOS)
                UserPaint = true;

            ApplyItems();
            ApplyIsEditable();
            ApplySelectedItem();
 
            Control.Items.ItemRangeAdditionFinished +=
                Items_ItemRangeAdditionFinished;
            Control.Items.ItemInserted += Items_ItemInserted;
            Control.Items.ItemRemoved += Items_ItemRemoved;
            Control.Items.CollectionChanged += Items_CollectionChanged;
            Control.IsEditableChanged += Control_IsEditableChanged;
            Control.SelectedItemChanged += Control_SelectedItemChanged;

            NativeControl.SelectedItemChanged = NativeControl_SelectedItemChanged;
            NativeControl.DrawItem = NativeControl_DrawItem;
            NativeControl.DrawItemBackground = NativeControl_DrawItemBackground;
            NativeControl.MeasureItemWidth = NativeControl_MeasureItemWidth;
            NativeControl.MeasureItem = NativeControl_MeasureItem;
        }

        protected override void OnDetach()
        {
            Control.Items.ItemRangeAdditionFinished -=
                Items_ItemRangeAdditionFinished;
            Control.Items.CollectionChanged -= Items_CollectionChanged;
            Control.Items.ItemInserted -= Items_ItemInserted;
            Control.Items.ItemRemoved -= Items_ItemRemoved;
            Control.IsEditableChanged -= Control_IsEditableChanged;
            Control.SelectedItemChanged -= Control_SelectedItemChanged;

            NativeControl.SelectedItemChanged = null;
            NativeControl.DrawItem = null;
            NativeControl.DrawItemBackground = null;
            NativeControl.MeasureItemWidth = null;
            NativeControl.MeasureItem = null;

            base.OnDetach();
        }

        private void Items_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                var item = e.NewItems?[0];
                var index = e.NewStartingIndex;
                var text = Control.GetItemText(item);
                NativeControl.SetItem(index, text);
            }
        }

        private void NativeControl_MeasureItemWidth()
        {
            if (Control.ItemPainter is null)
                return;
            var defaultWidthPixels = NativeControl.DefaultOnMeasureItemWidth();
            var defaultWidth = Control.PixelToDip(defaultWidthPixels);
            var result = Control.ItemPainter.GetWidth(Control, NativeControl.EventItem, defaultWidth);
            if (result >= 0)
            {
                NativeControl.EventResultInt = Control.PixelFromDip(result);
                NativeControl.EventCalled = true;
            }
        }

        private void NativeControl_MeasureItem()
        {
            if (Control.ItemPainter is null)
                return;
            var defaultHeightPixels = NativeControl.DefaultOnMeasureItem();
            var defaultHeight = Control.PixelToDip(defaultHeightPixels);
            var result = Control.ItemPainter.GetHeight(Control, NativeControl.EventItem, defaultHeight);
            if(result >= 0)
            {
                NativeControl.EventResultInt = Control.PixelFromDip(result);
                NativeControl.EventCalled = true;
            }
        }

        private void DrawItem(ComboBoxItemPaintEventArgs prm)
        {
            Control.ItemPainter?.Paint(Control, prm);
        }

        private void DrawItem(bool drawBackground)
        {
            var flags = (DrawItemFlags)NativeControl.EventFlags;
            var isPaintingControl = flags.HasFlag(DrawItemFlags.PaintingControl);

            var ptr = Native.Control.OpenDrawingContextForDC(NativeControl.EventDc, false);
            var dc = new WxGraphics(ptr);

            var rect = Control.PixelToDip(NativeControl.EventRect);

            if (paintEventArgs is null)
            {
                paintEventArgs = new ComboBoxItemPaintEventArgs(Control, dc, rect);
            }
            else
            {
                paintEventArgs.Graphics = dc;
                paintEventArgs.ClipRectangle = rect;
            }

            const int ItemIndexNotFound = -1;
            paintEventArgs.IsSelected = flags.HasFlag(DrawItemFlags.PaintingSelected);
            paintEventArgs.IsPaintingControl = isPaintingControl;
            paintEventArgs.IsIndexNotFound = NativeControl.EventItem == ItemIndexNotFound;
            paintEventArgs.ItemIndex = NativeControl.EventItem;
            paintEventArgs.IsPaintingBackground = drawBackground;
            DrawItem(paintEventArgs);
            paintEventArgs.Graphics = null!;
            dc.Dispose();
        }

        private void NativeControl_DrawItem()
        {
            if (Control.ItemPainter is null)
                return;
            DrawItem(false);
            NativeControl.EventCalled = true;
        }

        private void NativeControl_DrawItemBackground()
        {
            if (Control.ItemPainter is null)
                return;
            DrawItem(true);
            NativeControl.EventCalled = true;
        }

        private void NativeControl_SelectedItemChanged()
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

        private void ReceiveSelectedItem()
        {
            var selectedIndex = NativeControl.SelectedIndex;
            Control.SelectedIndex = selectedIndex == -1 ? null : selectedIndex;
        }

        private void Items_ItemInserted(object? sender, int index, object item)
        {
            if (!Control.Items.RangeOpInProgress)
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