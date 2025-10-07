using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class WxComboBoxHandler : WxControlHandler, IComboBoxHandler
    {
        /// <summary>
        /// Returns <see cref="ComboBox"/> instance with which this handler
        /// is associated.
        /// </summary>
        public new ComboBox? Control => (ComboBox?)base.Control;

        public int TextSelectionStart => NativeControl.TextSelectionStart;

        public int TextSelectionLength => NativeControl.TextSelectionLength;

        public bool AllowMouseWheel
        {
            set
            {
                NativeControl.AllowMouseWheel = value;
            }
        }

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

        public PointI TextMargins => NativeControl.TextMargins;

        internal new Native.ComboBox NativeControl =>
            (Native.ComboBox)base.NativeControl!;

        public void DismissPopup()
        {
            NativeControl.DismissPopup();
        }

        public void ShowPopup()
        {
            NativeControl.ShowPopup();
        }

        public void DefaultOnDrawBackground() => NativeControl.DefaultOnDrawBackground();

        public void DefaultOnDrawItem() => NativeControl.DefaultOnDrawItem();

        public void SelectTextRange(int start, int length) =>
            NativeControl.SelectTextRange(start, length);

        public void SelectAllText() => NativeControl.SelectAllText();

        public override void OnHandleCreated()
        {
            ItemsToPlatform();
        }

        public void ItemsToPlatform()
        {
            if (Control is null)
                return;
            var s = Control.Text;

            var nativeControl = NativeControl;
            nativeControl.ClearItems();

            var control = Control;
            var items = control.Items;

            var insertion = NativeControl.CreateItemsInsertion();
            for (var i = 0; i < items.Count; i++)
            {
                NativeControl.AddItemToInsertion(
                    insertion,
                    Control.GetItemText(control.Items[i], false));
            }

            NativeControl.CommitItemsInsertion(insertion, 0);
            InvalidateBestSize();
            ApplySelectedItem();

            if (Control.DropDownStyle == ComboBoxStyle.DropDown)
            {
                Text = s;
            }
        }

        internal override Native.Control CreateNativeControl()
        {
            return new Native.ComboBox();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            if (Control is null)
                return;

            if (App.IsWindowsOS)
                UserPaint = true;

            ItemsToPlatform();
            ApplyIsEditable();
 
            Control.Items.ItemRangeAdditionFinished +=
                Items_ItemRangeAdditionFinished;
            Control.Items.ItemInserted += Items_ItemInserted;
            Control.Items.ItemRemoved += Items_ItemRemoved;
            Control.Items.CollectionChanged += Items_CollectionChanged;
            
            Control.IsEditableChanged += Control_IsEditableChanged;
            Control.SelectedItemChanged += Control_SelectedItemChanged;
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            if (Control is null)
                return;

            Control.Items.ItemRangeAdditionFinished -=
                Items_ItemRangeAdditionFinished;
            Control.Items.CollectionChanged -= Items_CollectionChanged;
            Control.Items.ItemInserted -= Items_ItemInserted;
            Control.Items.ItemRemoved -= Items_ItemRemoved;
            Control.IsEditableChanged -= Control_IsEditableChanged;
            Control.SelectedItemChanged -= Control_SelectedItemChanged;
        }

        private void Items_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (Control is null)
                return;
            if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                var item = e.NewItems?[0];
                var index = e.NewStartingIndex;
                var text = Control.GetItemText(item, false);
                NativeControl.SetItem(index, text);
                InvalidateBestSize();
            }
        }

        private void Control_IsEditableChanged(object? sender, EventArgs e)
        {
            if (Control is null)
                return;
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
            if (Control is null)
                return;
            NativeControl.IsEditable = Control.IsEditable;
        }

        private void ApplySelectedItem()
        {
            if (Control is null)
                return;
            NativeControl.SelectedIndex = Control.SelectedIndex ?? -1;
        }

        private void Items_ItemInserted(object? sender, int index, object item)
        {
            if (Control is null)
                return;
            if (!Control.Items.RangeOpInProgress)
            {
                NativeControl.InsertItem(index, Control.GetItemText(item, false));
                InvalidateBestSize();
            }
        }

        private void Items_ItemRemoved(object? sender, int index, object item)
        {
            NativeControl.RemoveItemAt(index);
            InvalidateBestSize();
        }

        private void Items_ItemRangeAdditionFinished(
            object? sender,
            int index,
            IEnumerable<object> items)
        {
            if (Control is null)
                return;
            var insertion = NativeControl.CreateItemsInsertion();
            foreach (var item in items)
            {
                NativeControl.AddItemToInsertion(
                    insertion,
                    Control.GetItemText(item, false));
            }

            NativeControl.CommitItemsInsertion(insertion, index);
            InvalidateBestSize();
        }
    }
}