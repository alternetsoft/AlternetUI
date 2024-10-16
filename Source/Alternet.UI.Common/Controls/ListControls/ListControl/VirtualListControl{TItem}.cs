using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Advanced list control with ability to customize item painting.
    /// </summary>
    /// <typeparam name="TItem">Type of the item.</typeparam>
    public abstract class VirtualListControl<TItem>
        : CustomListBox<TItem>, IListControlItemContainer, IListControlItemDefaults
        where TItem : class, new()
    {
        /// <summary>
        /// Gets or sets default minimal item height.
        /// </summary>
        public static Coord DefaultMinItemHeight = 24;

        /// <summary>
        /// Gets or sets default current item border.
        /// </summary>
        public static BorderSettings DefaultCurrentItemBorder;

        /// <summary>
        /// Gets or sets default item margin.
        /// </summary>
        public static Thickness DefaultItemMargin = 2;

        /// <summary>
        /// Gets or sets default selected item text color.
        /// </summary>
        public static Color DefaultSelectedItemTextColor = SystemColors.HighlightText; // !!!!!!!!!!!!!

        /// <summary>
        /// Gets or sets default selected item background color.
        /// </summary>
        public static Color DefaultSelectedItemBackColor = SystemColors.Highlight; // !!!!!!!!!!!!!

        /// <summary>
        /// Gets or sets default disabled item text color.
        /// </summary>
        public static Color DefaultDisabledItemTextColor = SystemColors.GrayText; // !!!!!!!!!!!!!

        /// <summary>
        /// Gets or sets default item text color.
        /// </summary>
        public static Color DefaultItemTextColor = SystemColors.WindowText; // !!!!!!!!!!!!!

        private Thickness itemMargin = DefaultItemMargin;
        private Color? selectedItemTextColor;
        private Color? itemTextColor;
        private Color? selectedItemBackColor;
        private bool selectedIsBold;
        private Color? disabledItemTextColor;
        private IListBoxItemPainter? painter;
        private Coord minItemHeight = DefaultMinItemHeight;
        private bool textVisible = true;
        private bool currentItemBorderVisible = true;
        private bool selectionVisible = true;
        private BorderSettings? currentItemBorder;
        private BorderSettings? selectionBorder;
        private bool checkBoxesVisible;
        private bool checkBoxThreeState;

        private GenericAlignment itemAlignment = ListControlItem.DefaultItemAlignment;

        static VirtualListControl()
        {
            DefaultCurrentItemBorder = new();
            DefaultCurrentItemBorder.Color = Color.Black; // !!!!!!!!!!!!!
        }

        /// <summary>
        /// Occurs when the checked state of an item changes.
        /// </summary>
        [Category("Behavior")]
        public event EventHandler? CheckedChanged;

        /// <summary>
        /// Gets or sets a value indicating whether the checkbox should be
        /// toggled when an item is clicked on the checkbox area.
        /// </summary>
        public virtual bool CheckOnClick { get; set; } = true;

        /// <summary>
        /// Gets or sets default size of the svg images.
        /// </summary>
        /// <remarks>
        /// Each item has <see cref="ListControlItem.SvgImageSize"/> property where
        /// this setting can be overriden. If <see cref="SvgImageSize"/> is not specified,
        /// default toolbar image size is used. Currently only rectangular svg images
        /// are supported.
        /// </remarks>
        [Browsable(false)]
        public virtual SizeI? SvgImageSize { get; set; }

        /// <summary>
        /// Gets or sets current item border. If it is <c>null</c> (default value),
        /// <see cref="DefaultCurrentItemBorder"/> is used.
        /// </summary>
        [Browsable(false)]
        public virtual BorderSettings? CurrentItemBorder
        {
            get
            {
                return currentItemBorder;
            }

            set
            {
                if (currentItemBorder == value)
                    return;
                currentItemBorder = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets a collection that contains the zero-based indexes of
        /// all currently checked items in the control.
        /// </summary>
        /// <remarks>
        /// Indexes are returned in the descending order (maximal index
        /// is the first).
        /// </remarks>
        /// <value>
        /// An <see cref="IReadOnlyList{T}"/> containing the indexes of the
        /// currently checked items in the control.
        /// If no items are currently selected, an empty
        /// <see cref="IReadOnlyList{T}"/> is returned.
        /// </value>
        [Browsable(false)]
        public virtual IReadOnlyList<int> CheckedIndicesDescending
        {
            get
            {
#pragma warning disable
                int[] sortedCopy =
                    CheckedIndices.OrderByDescending(i => i).ToArray();
#pragma warning restore
                return sortedCopy;
            }
        }

        IListControlItemDefaults IListControlItemContainer.Defaults
        {
            get => this;
        }

        /// <summary>
        /// Gets a collection that contains the zero-based indexes of all
        /// currently checked items in the control.
        /// </summary>
        /// <value>
        /// An <see cref="IReadOnlyList{T}"/> containing the indexes of the
        /// currently checked items in the control.
        /// If no items are currently checked, an empty
        /// <see cref="IReadOnlyList{T}"/> is returned.
        /// </value>
        [Browsable(false)]
        public virtual IReadOnlyList<int> CheckedIndices
        {
            get
            {
                var checkedCount = CheckedCount;
                if (checkedCount == 0)
                    return Array.Empty<int>();
                int[] result = new int[checkedCount];
                var index = 0;

                for (int i = 0; i < Count; i++)
                {
                    var item = SafeItem(i);
                    if (item is null)
                        continue;
                    if (item.CheckState == CheckState.Checked)
                    {
                        result[index] = i;
                        index++;
                    }
                }

                return result;
            }

            set
            {
                var changed = ClearChecked(false);

                foreach (var index in value)
                {
                    if (SetItemCheckedCore(index, true))
                    {
                        changed = true;
                    }
                }

                if (changed)
                {
                    Invalidate();
                    RaiseCheckedChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether checkbox will
        /// allow three check states rather than two.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the checkbox is able to display
        /// three check states; otherwise, <see langword="false" />. The default value
        /// is <see langword="false"/>.
        /// </returns>
        [DefaultValue(false)]
        public virtual bool CheckBoxThreeState
        {
            get => checkBoxThreeState;

            set
            {
                if (checkBoxThreeState == value)
                    return;
                checkBoxThreeState = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets number of checked items.
        /// </summary>
        [DefaultValue(false)]
        [Browsable(false)]
        public virtual int CheckedCount
        {
            get
            {
                var result = 0;
                for (int i = 0; i < Count; i++)
                {
                    var item = SafeItem(i);
                    if (item is null)
                        continue;
                    if (item.CheckState == CheckState.Checked)
                        result++;
                }

                return result;
            }
        }

        /// <summary>
        /// Gets or sets whether user can set the checkboxes to
        /// the third state by clicking.
        /// </summary>
        /// <remarks>
        /// By default a user can't set a 3-state checkboxes to the third state. It can only
        /// be done from code. Using this flags allows the user to set the checkboxes to
        /// the third state by clicking.
        /// </remarks>
        [DefaultValue(false)]
        public virtual bool CheckBoxAllowAllStatesForUser { get; set; }

        /// <summary>
        /// Gets or sets whether to show checkboxes in the items.
        /// </summary>
        public virtual bool CheckBoxVisible
        {
            get
            {
                return checkBoxesVisible;
            }

            set
            {
                if (checkBoxesVisible == value)
                    return;
                checkBoxesVisible = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets selection border.
        /// </summary>
        [Browsable(false)]
        public virtual BorderSettings? SelectionBorder
        {
            get
            {
                return selectionBorder;
            }

            set
            {
                if (selectionBorder == value)
                    return;
                selectionBorder = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets whether selection background is visible.
        /// </summary>
        public virtual bool SelectionVisible
        {
            get
            {
                return selectionVisible;
            }

            set
            {
                if (selectionVisible == value)
                    return;
                selectionVisible = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets whether current item border is visible.
        /// </summary>
        public virtual bool CurrentItemBorderVisible
        {
            get
            {
                return currentItemBorderVisible;
            }

            set
            {
                if (currentItemBorderVisible == value)
                    return;
                currentItemBorderVisible = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets whether item text is displayed.
        /// </summary>
        public virtual bool TextVisible
        {
            get
            {
                return textVisible;
            }

            set
            {
                if (textVisible == value)
                    return;
                textVisible = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets minimal height of the items. Default is <see cref="DefaultMinItemHeight"/>.
        /// </summary>
        public virtual Coord MinItemHeight
        {
            get
            {
                return minItemHeight;
            }

            set
            {
                if (minItemHeight == value)
                    return;
                minItemHeight = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets disabled item text color.
        /// </summary>
        public virtual Color? DisabledItemTextColor
        {
            get
            {
                return disabledItemTextColor;
            }

            set
            {
                if (disabledItemTextColor == value)
                    return;
                disabledItemTextColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets item painter associated with the control.
        /// </summary>
        [Browsable(false)]
        public virtual IListBoxItemPainter? ItemPainter
        {
            get
            {
                return painter;
            }

            set
            {
                if (painter == value)
                    return;
                painter = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets whether <see cref="ListControl{T}.SelectedItem"/> has bold font.
        /// </summary>
        public virtual bool SelectedItemIsBold
        {
            get
            {
                return selectedIsBold;
            }

            set
            {
                if (selectedIsBold == value)
                    return;
                selectedIsBold = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets selected item text color.
        /// </summary>
        public virtual Color? SelectedItemTextColor
        {
            get
            {
                return selectedItemTextColor;
            }

            set
            {
                if (selectedItemTextColor == value)
                    return;
                selectedItemTextColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets selected item text color.
        /// </summary>
        public virtual Color? SelectedItemBackColor
        {
            get
            {
                return selectedItemBackColor;
            }

            set
            {
                if (selectedItemBackColor == value)
                    return;
                selectedItemBackColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets item text color.
        /// </summary>
        public virtual Color? ItemTextColor
        {
            get
            {
                return itemTextColor;
            }

            set
            {
                if (itemTextColor == value)
                    return;
                itemTextColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets default alignment of the items.
        /// </summary>
        /// <remarks>
        /// In order to set individual item alignment, item must be <see cref="ListControlItem"/>
        /// descendant, it has <see cref="ListControlItem.Alignment"/> property.
        /// </remarks>
        public virtual GenericAlignment ItemAlignment
        {
            get
            {
                return itemAlignment;
            }

            set
            {
                if (itemAlignment == value)
                    return;
                itemAlignment = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets item margin.
        /// </summary>
        public virtual Thickness ItemMargin
        {
            get
            {
                return itemMargin;
            }

            set
            {
                if (itemMargin == value)
                    return;
                itemMargin = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets item font. It must not be <c>null</c>.
        /// </summary>
        /// <returns></returns>
        public virtual Font GetItemFont(int itemIndex = -1)
        {
            return ListControlItem.GetFont(SafeItem(itemIndex), this);
        }

        /// <summary>
        /// Measures item size. If <see cref="ItemPainter"/> is assigned, uses
        /// <see cref="IListBoxItemPainter.GetSize"/>, otherwise calls
        /// <see cref="DefaultMeasureItemSize"/>.
        /// </summary>
        /// <param name="itemIndex">Index of the item.</param>
        public virtual SizeD MeasureItemSize(int itemIndex)
        {
            if (painter is null)
                return DefaultMeasureItemSize(itemIndex);
            var result = painter.GetSize(this, itemIndex);
            if (result == SizeD.MinusOne)
                return DefaultMeasureItemSize(itemIndex);
            return result;
        }

        /// <summary>
        /// Unchecks all items in the control.
        /// </summary>
        public virtual bool ClearChecked(bool raiseEvents = true)
        {
            if (Items.Count == 0)
                return false;
            bool changed = false;
            for (int i = 0; i < Count; i++)
            {
                var item = SafeItem(i);
                if (item is null)
                    continue;
                if (item.CheckState != CheckState.Unchecked)
                {
                    item.CheckState = CheckState.Unchecked;
                    changed = true;
                }
            }

            if (changed && raiseEvents)
            {
                Invalidate();
                RaiseCheckedChanged(EventArgs.Empty);
            }

            return changed;
        }

        /// <summary>
        /// Default method which measures item size. Called from <see cref="MeasureItemSize"/>.
        /// </summary>
        /// <param name="itemIndex">Index of the item.</param>
        public virtual SizeD DefaultMeasureItemSize(int itemIndex)
        {
            var s = GetItemText(itemIndex);
            if (string.IsNullOrEmpty(s))
                s = "Wy";

            var (normal, disabled, selected) = GetItemImages(itemIndex, null);
            var maxHeightI =
                MathUtils.Max(normal?.Size.Height, disabled?.Size.Height, selected?.Size.Height);
            var maxHeightD = PixelToDip(maxHeightI);

            var font = GetItemFont(itemIndex).AsBold;
            var size = MeasureCanvas.GetTextExtent(s, font);
            size.Height = Math.Max(size.Height, maxHeightD);
            size.Width += ItemMargin.Horizontal;
            size.Height += ItemMargin.Vertical;
            size.Height = Math.Max(size.Height, GetItemMinHeight(itemIndex));
            return size;
        }

        /// <summary>
        /// Checks or clears the check state for the specified item.
        /// </summary>
        /// <param name="index">The zero-based index of the item in the control
        /// to set or clear the check state.</param>
        /// <param name="value"><c>true</c> to check the specified item;
        /// otherwise, false.</param>
        /// <remarks>
        /// This method doesn't repaint control and raises no events.
        /// </remarks>
        public virtual bool SetItemCheckedCore(int index, bool value)
        {
            if (value)
                return SetItemCheckStateCore(index, CheckState.Checked);
            else
                return SetItemCheckStateCore(index, CheckState.Unchecked);
        }

        /// <summary>
        /// Changes the check state for the specified item.
        /// </summary>
        /// <param name="index">The zero-based index of the item in the control
        /// to change the check state.</param>
        /// <param name="value">New value.</param>
        /// <remarks>
        /// This method doesn't repaint control and raises no events.
        /// </remarks>
        public virtual bool SetItemCheckStateCore(int index, CheckState value)
        {
            var item = SafeItem(index);
            if (item is null)
                return false;
            if (item.CheckState == value)
                return false;
            item.CheckState = value;
            return true;
        }

        /// <summary>
        /// Draws background for the item with the specified index.
        /// If <see cref="ItemPainter"/> is assigned, uses
        /// <see cref="IListBoxItemPainter.PaintBackground"/>, otherwise calls
        /// <see cref="DefaultDrawItemBackground"/>.
        /// </summary>
        /// <param name="e">Draw parameters.</param>
        public virtual void DrawItemBackground(ListBoxItemPaintEventArgs e)
        {
            var result = painter?.PaintBackground(this, e) ?? false;
            if (!result)
                DefaultDrawItemBackground(e);
        }

        /// <summary>
        /// Draws default background for the item with the specified index.
        /// Used inside <see cref="DrawItemBackground"/>.
        /// </summary>
        /// <param name="e">Draw parameters.</param>
        public virtual void DefaultDrawItemBackground(ListBoxItemPaintEventArgs e)
        {
            var rect = e.ClipRectangle;
            var dc = e.Graphics;

            var isSelected = e.IsSelected;
            var isCurrent = e.IsCurrent;

            var item = SafeItem(e.ItemIndex);

            var hideSelection = item?.HideSelection ?? false;
            var hideFocusRect = item?.HideFocusRect ?? false;

            if (Enabled)
            {
                dc.FillBorderRectangle(
                    rect,
                    item?.BackgroundColor?.AsBrush,
                    item?.Border,
                    true,
                    this);

                if (isSelected && selectionVisible)
                {
                    if (!hideSelection)
                    {
                        dc.FillBorderRectangle(
                            rect,
                            GetSelectedItemBackColor(e.ItemIndex).AsBrush,
                            selectionBorder,
                            true,
                            this);
                    }
                }

                if (isCurrent && Focused && currentItemBorderVisible && !hideFocusRect)
                {
                    var border = CurrentItemBorder ?? DefaultCurrentItemBorder;
                    DrawingUtils.DrawBorder(this, e.Graphics, rect, border);
                }
            }
            else
            {
                var border = item?.Border?.ToGrayScale();
                DrawingUtils.DrawBorder(this, e.Graphics, rect, border);
            }
        }

        /// <summary>
        /// Checks or clears the check state for the specified item.
        /// </summary>
        /// <param name="index">The zero-based index of the item in the control
        /// to set or clear the check state.</param>
        /// <param name="value"><c>true</c> to check the specified item;
        /// otherwise, false.</param>
        /// <remarks>
        /// This method repaints control and raises events.
        /// Use <see cref="VirtualListControl{T}.SetItemCheckedCore(int, bool)"/>
        /// method to change checked state without raising events and repainting the
        /// control.
        /// </remarks>
        public virtual bool SetItemChecked(int index, bool value)
        {
            var result = SetItemCheckedCore(index, value);
            if (result)
            {
                Invalidate();
                RaiseCheckedChanged(EventArgs.Empty);
            }

            return result;
        }

        /// <summary>
        /// Removes checked items from the control.
        /// </summary>
        public virtual void RemoveCheckedItems()
        {
            RemoveItems(CheckedIndicesDescending);
        }

        /// <summary>
        /// Selects all items in the control.
        /// </summary>
        public virtual void SelectAll()
        {
            SetAllSelected(true);
        }

        /// <summary>
        /// Unselects all items in the control.
        /// </summary>
        public virtual void UnselectAll()
        {
            SetAllSelected(false);
        }

        /// <summary>
        /// Changes selected state for all items in the control.
        /// </summary>
        /// <param name="selected">New selected state.</param>
        public virtual void SetAllSelected(bool selected)
        {
            if (SelectionMode == ListBoxSelectionMode.Single)
                return;

            DoInsideUpdate(() =>
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    SetSelected(i, selected);
                }
            });
        }

        /// <summary>
        /// Checks items with specified indexes.
        /// </summary>
        public virtual void CheckItems(params int[] indexes)
        {
            CheckedIndices = GetValidIndexes(indexes);
        }

        /// <summary>
        /// Changes the check state for the specified item.
        /// </summary>
        /// <param name="index">The zero-based index of the item in the control
        /// to change the check state.</param>
        /// <param name="value">New value.</param>
        /// <remarks>
        /// This method repaints control and raises events.
        /// Use <see cref="VirtualListControl{T}.SetItemCheckStateCore"/>
        /// method to change checked state without raising events and repainting the
        /// control.
        /// </remarks>
        public virtual bool SetItemCheckState(int index, CheckState value)
        {
            var result = SetItemCheckStateCore(index, value);
            if (result)
            {
                Invalidate();
                RaiseCheckedChanged(EventArgs.Empty);
            }

            return result;
        }

        /// <summary>
        /// Draws item with the specified index.  If <see cref="ItemPainter"/> is assigned, uses
        /// <see cref="IListBoxItemPainter.Paint"/>, otherwise calls
        /// <see cref="DefaultDrawItem"/>.
        /// </summary>
        /// <param name="e">Draw parameters.</param>
        public virtual void DrawItem(ListBoxItemPaintEventArgs e)
        {
            e.ClipRectangle = e.ClipRectangle.WithMargin(ItemMargin);

            if (painter is null)
            {
                DefaultDrawItem(e);
                return;
            }

            painter.Paint(this, e);
        }

        /// <summary>
        /// Gets suggested rectangles of the item's image and text.
        /// </summary>
        /// <param name="rect">Item rectangle.</param>
        /// <param name="imageSize">Image size. Optional. If not specified, calculated
        /// using height of the item.</param>
        /// <returns></returns>
        public virtual (RectD ImageRect, RectD TextRect) GetItemImageRect(
            RectD rect, SizeD? imageSize = null)
        {
            Thickness textMargin = Thickness.Empty;

            var offset = ComboBox.DefaultImageVerticalOffset;

            var size = rect.Height - textMargin.Vertical - (offset * 2);

            if (imageSize is null || imageSize.Value.Height > size)
                imageSize = (size, size);

            PointD imageLocation = (
                rect.X + textMargin.Left,
                rect.Y + textMargin.Top + offset);

            var imageRect = new RectD(imageLocation, imageSize.Value);
            var centeredImageRect = imageRect.CenterIn(rect, false, true);

            var itemRect = rect;
            itemRect.X += centeredImageRect.Width + ComboBox.DefaultImageTextDistance;
            itemRect.Width -= centeredImageRect.Width + ComboBox.DefaultImageTextDistance;

            return (centeredImageRect, itemRect);
        }

        /// <summary>
        /// Raises the <see cref="CheckedChanged"/> event and calls
        /// <see cref="OnCheckedChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        public void RaiseCheckedChanged(EventArgs e)
        {
            OnCheckedChanged(e);
            CheckedChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Gets whether item with the specified index is current.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <returns></returns>
        public abstract bool IsCurrent(int index);

        /// <summary>
        /// Gets whether item with the specified index is selected.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <returns></returns>
        public abstract bool IsSelected(int index);

        /// <summary>
        /// Gets item as object.
        /// </summary>
        /// <param name="index">Index of the item.</param>
        /// <returns></returns>
        public object? GetItemAsObject(int index)
        {
            return GetItem(index);
        }

        /// <summary>
        /// Gets item alignment.
        /// </summary>
        /// <param name="itemIndex">Index of the item.</param>
        /// <returns></returns>
        public virtual GenericAlignment GetItemAlignment(int itemIndex)
        {
            return ListControlItem.GetAlignment(SafeItem(itemIndex), this);
        }

        /// <summary>
        /// Gets item minimal height.
        /// </summary>
        /// <param name="itemIndex">Index of the item.</param>
        /// <returns></returns>
        public virtual Coord GetItemMinHeight(int itemIndex)
        {
            var item = SafeItem(itemIndex);
            if (item is null)
                return MinItemHeight;
            return Math.Max(item.MinHeight, MinItemHeight);
        }

        /// <summary>
        /// Gets item image.
        /// </summary>
        /// <param name="itemIndex">Index of the item.</param>
        /// <param name="svgColor">Color of the svg image when item is selected.</param>
        /// <returns></returns>
        public virtual (Image? Normal, Image? Disabled, Image? Selected)
            GetItemImages(int itemIndex, Color? svgColor)
        {
            return ListControlItem.GetItemImages(SafeItem(itemIndex), this, svgColor);
        }

        /// <summary>
        /// Gets whether three state checkbox is allowed in the item.
        /// </summary>
        /// <param name="item">Item.</param>
        /// <returns></returns>
        public virtual bool GetItemAllowThreeState(ListControlItem item)
        {
            bool allowThreeState = CheckBoxThreeState;
            if (allowThreeState && item.CheckBoxThreeState is not null)
                allowThreeState = item.CheckBoxThreeState.Value;
            return allowThreeState;
        }

        /// <summary>
        /// Gets whether user can set the checkbox of the item to
        /// the third state by clicking.
        /// </summary>
        public virtual bool GetItemCheckBoxAllowAllStatesForUser(ListControlItem item)
        {
            bool result = CheckBoxAllowAllStatesForUser;
            if (result && item.CheckBoxAllowAllStatesForUser is not null)
                result = item.CheckBoxAllowAllStatesForUser.Value;
            return result;
        }

        /// <summary>
        /// Gets whether checkbox is shown in the item.
        /// </summary>
        /// <param name="item">Item.</param>
        /// <returns></returns>
        public virtual bool GetItemShowCheckBox(ListControlItem item)
        {
            return item.GetShowCheckBox(this);
        }

        /// <summary>
        /// Gets <see cref="CheckState"/> of the item using <see cref="GetItemAllowThreeState"/>
        /// and <see cref="ListControlItem.CheckState"/>.
        /// </summary>
        /// <param name="item">Item.</param>
        /// <returns></returns>
        public virtual CheckState GetItemCheckState(ListControlItem item)
        {
            var allowThreeState = GetItemAllowThreeState(item);
            var checkState = item.CheckState;
            if (!allowThreeState && checkState == CheckState.Indeterminate)
                checkState = CheckState.Unchecked;
            return checkState;
        }

        /// <summary>
        /// Gets selected item text color. Default is <see cref="SelectedItemTextColor"/>
        /// (if it is not <c>null</c>) or <see cref="DefaultSelectedItemTextColor"/>.
        /// </summary>
        /// <returns></returns>
        public virtual Color GetSelectedItemTextColor(int itemIndex)
        {
            return ListControlItem.GetSelectedTextColor(SafeItem(itemIndex), this);
        }

        /// <summary>
        /// Gets item text color. Default is <see cref="ItemTextColor"/> (if it is not <c>null</c>)
        /// or <see cref="DefaultItemTextColor"/>.
        /// </summary>
        /// <returns></returns>
        public virtual Color GetItemTextColor(int itemIndex)
        {
            return ListControlItem.GetItemTextColor(SafeItem(itemIndex), this);
        }

        /// <summary>
        /// Gets selected item back color. Default is <see cref="SelectedItemBackColor"/>
        /// (if it is not <c>null</c>) or <see cref="DefaultSelectedItemBackColor"/>.
        /// </summary>
        /// <returns></returns>
        public virtual Color GetSelectedItemBackColor(int itemIndex)
        {
            if (Enabled && SelectionVisible)
                return SelectedItemBackColor ?? DefaultSelectedItemBackColor;
            else
                return RealBackgroundColor;
        }

        /// <summary>
        /// Gets disabled item text color.
        /// </summary>
        /// <returns></returns>
        public virtual Color GetDisabledItemTextColor(int itemIndex)
        {
            return ListControlItem.GetDisabledTextColor(SafeItem(itemIndex), this);
        }

        /// <summary>
        /// Default method which draws items. Called from <see cref="DrawItem"/>.
        /// </summary>
        public virtual void DefaultDrawItem(ListBoxItemPaintEventArgs e)
        {
            var item = SafeItem(e.ItemIndex);

            if (item is not null)
            {
                var info = GetCheckBoxInfo(item, e.ClipRectangle);
                if (info is not null)
                {
                    e.Graphics.DrawCheckBox(this, info.CheckRect, info.CheckState, info.PartState);
                    e.ClipRectangle = info.TextRect;
                }
            }

            var isSelected = e.IsSelected;
            var hideSelection = item?.HideSelection ?? false;
            if (hideSelection)
                isSelected = false;

            var (normalImage, disabledImage, selectedImage) = e.ItemImages;
            var image = Enabled ? (isSelected ? selectedImage : normalImage) : disabledImage;

            var s = textVisible ? e.ItemText.Trim() : string.Empty;

            if (image is not null && s != string.Empty)
                s = $" {s}";

            e.Graphics.DrawLabel(
                s,
                e.ItemFont,
                e.GetTextColor(isSelected),
                Color.Empty,
                image,
                e.ClipRectangle,
                e.ItemAlignment);
        }

        internal ListControlItem.ItemCheckBoxInfo? GetCheckBoxInfo(int itemIndex, RectD rect)
        {
            var item = SafeItem(itemIndex);
            if (item is null)
                return null;
            return GetCheckBoxInfo(item, rect);
        }

        /// <summary>
        /// Called when when the checkbox state of the item has changed.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        /// <remarks>See <see cref="CheckedChanged"/> for details.</remarks>
        protected virtual void OnCheckedChanged(EventArgs e)
        {
        }

        private ListControlItem.ItemCheckBoxInfo? GetCheckBoxInfo(ListControlItem item, RectD rect)
        {
            var showCheckBox = GetItemShowCheckBox(item);
            if (!showCheckBox)
                return null;
            ListControlItem.ItemCheckBoxInfo result = new();
            result.PartState = Enabled
                ? VisualControlState.Normal : VisualControlState.Disabled;
            result.CheckState = GetItemCheckState(item);
            result.CheckSize = DrawingUtils.GetCheckBoxSize(this, result.CheckState, result.PartState);
            var (checkRect, textRect) = GetItemImageRect(rect, result.CheckSize);
            result.CheckRect = checkRect;
            result.TextRect = textRect;
            return result;
        }
    }
}