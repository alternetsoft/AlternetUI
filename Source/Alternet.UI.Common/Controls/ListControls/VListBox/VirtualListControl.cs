using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Advanced list control with ability to customize item painting.
    /// </summary>
    public abstract partial class VirtualListControl
        : ListControl<ListControlItem>, ICustomListBox<ListControlItem>,
        IListControlItemContainer, IListControlItemDefaults,
        ICheckListBox<ListControlItem>
    {
        /// <summary>
        /// Gets or sets default minimal item height.
        /// </summary>
        public static Coord DefaultMinItemHeight = 24;

        /// <summary>
        /// Gets or sets default item margin.
        /// </summary>
        public static Thickness DefaultItemMargin = 2;

        /// <summary>
        /// Gets or sets default selected item text color.
        /// </summary>
        public static Color DefaultSelectedItemTextColor
            = Color.LightDark(SystemColors.HighlightText);

        /// <summary>
        /// Gets or sets default selected item background color.
        /// </summary>
        public static Color DefaultSelectedItemBackColor
            = Color.LightDark(SystemColors.Highlight);

        /// <summary>
        /// Gets or sets default disabled item text color.
        /// </summary>
        public static Color DefaultDisabledItemTextColor
            = Color.LightDark(SystemColors.GrayText);

        /// <summary>
        /// Gets or sets default item text color.
        /// </summary>
        public static Color DefaultItemTextColor
            = Color.LightDark(SystemColors.WindowText);

        /// <summary>
        /// Gets or sets default border color for the current item.
        /// This is used when <see cref="DefaultCurrentItemBorder"/>
        /// is created.
        /// </summary>
        public static Color DefaultCurrentItemBorderColor
            = Color.LightDark(light: Color.Black, dark: Color.Gray);

        private static BorderSettings? defaultCurrentItemBorder;

        private Color? selectedItemTextColor;
        private Color? itemTextColor;
        private Color? selectedItemBackColor;
        private Color? disabledItemTextColor;

        private Thickness itemMargin = DefaultItemMargin;
        private IListBoxItemPainter? painter;
        private Coord minItemHeight = DefaultMinItemHeight;
        private BorderSettings? currentItemBorder;
        private BorderSettings? selectionBorder;

        private bool selectedIsBold;
        private bool textVisible = true;
        private bool currentItemBorderVisible = true;
        private bool selectionVisible = true;
        private bool checkBoxesVisible;
        private bool checkBoxThreeState;
        private bool selectionUnderImage = true;

        private HVAlignment itemAlignment = ListControlItem.DefaultItemAlignment;

        static VirtualListControl()
        {
        }

        /// <summary>
        /// Occurs when the checked state of an item changes.
        /// </summary>
        [Category("Behavior")]
        public event EventHandler? CheckedChanged;

        /// <summary>
        /// Gets or sets default border of the listbox's current item.
        /// </summary>
        public static BorderSettings DefaultCurrentItemBorder
        {
            get
            {
                if (defaultCurrentItemBorder is null)
                {
                    defaultCurrentItemBorder = new();
                    defaultCurrentItemBorder.Color = DefaultCurrentItemBorderColor;
                }

                return defaultCurrentItemBorder;
            }

            set
            {
                defaultCurrentItemBorder = value;
            }
        }

        AbstractControl? IListControlItemContainer.Control => this;

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
        /// Gets or sets the zero-based index of the currently checked item
        /// in a <see cref="ListBox"/>.
        /// </summary>
        /// <value>A zero-based index of the currently checked item. A value
        /// of <c>null</c> is returned if no item is checked.</value>
        [Browsable(false)]
        public virtual int? CheckedIndex
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return CheckedIndices.FirstOrDefault();
            }

            set
            {
                if (DisposingOrDisposed)
                    return;

                if (value != null && (value < 0 || value >= Items.Count))
                    value = null;

                ClearChecked();
                if (value != null)
                    SetChecked(value.Value, true);
            }
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

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.ListBox;

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

        /// <inheritdoc/>
        public virtual bool SelectionUnderImage
        {
            get
            {
                return selectionUnderImage;
            }

            set
            {
                if (selectionUnderImage == value)
                    return;
                selectionUnderImage = value;
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
        [Browsable(false)]
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
        [Browsable(false)]
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
        /// Gets or sets selected item back color.
        /// </summary>
        [Browsable(false)]
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
        [Browsable(false)]
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
        [Browsable(false)]
        public virtual HVAlignment ItemAlignment
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
        /// Gets a <see cref="IVListBoxHandler"/> associated with this class.
        /// </summary>
        [Browsable(false)]
        internal new IVListBoxHandler Handler
        {
            get
            {
                return (IVListBoxHandler)base.Handler;
            }
        }

        [Browsable(false)]
        internal new string Text
        {
            get => base.Text;
            set => base.Text = value;
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
        /// Returns the zero-based index of the item at the specified coordinates.
        /// </summary>
        /// <param name="position">A <see cref="PointD"/> object containing
        /// the coordinates used to obtain the item
        /// index.</param>
        /// <returns>The zero-based index of the item found at the specified
        /// coordinates; returns <see langword="null"/>
        /// if no match is found.</returns>
        public virtual int? HitTest(PointD position)
        {
            if (DisposingOrDisposed)
                return null;
            return Handler.HitTest(position);
        }

        /// <summary>
        /// Gets only valid indexes from the list of indexes in
        /// the control.
        /// </summary>
        public virtual IReadOnlyList<int> GetValidIndexes(params int[] indexes)
        {
            var validIndexes = new List<int>();

            foreach (int index in indexes)
            {
                if (IsValidIndex(index))
                    validIndexes.Add(index);
            }

            return validIndexes;
        }

        /// <summary>
        /// Checks whether index is valid in the control.
        /// </summary>
        public virtual bool IsValidIndex(int index)
        {
            return index >= 0 && index < Items.Count;
        }

        /// <summary>
        /// Unchecks all items in the control and optionally calls
        /// <see cref="RaiseCheckedChanged"/>.
        /// </summary>
        public virtual bool ClearChecked(bool raiseEvents)
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
            var item = e.Item;
            if (item is null)
                ListControlItem.DefaultDrawBackground(this, e);
            else
                item.DrawBackground(this, e);
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
        /// Use <see cref="VirtualListControl.SetItemCheckedCore(int, bool)"/>
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
        /// Gets whether control has items.
        /// </summary>
        public virtual bool HasItems()
        {
            return Items.Count > 0;
        }

        /// <summary>
        /// Removes checked items from the control.
        /// </summary>
        public virtual void RemoveCheckedItems()
        {
            RemoveItems(CheckedIndicesDescending);
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
        /// Use <see cref="VirtualListControl.SetItemCheckStateCore"/>
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
        /// <see cref="DefaultDrawItemForeground"/>.
        /// </summary>
        /// <param name="e">Draw parameters.</param>
        public virtual void DrawItemForeground(ListBoxItemPaintEventArgs e)
        {
            e.ClipRectangle = e.ClipRectangle.WithMargin(ItemMargin);

            if (painter is null)
            {
                DefaultDrawItemForeground(e);
                return;
            }

            painter.Paint(this, e);
        }

        /// <summary>
        /// Raises the <see cref="CheckedChanged"/> event and calls
        /// <see cref="OnCheckedChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        public void RaiseCheckedChanged(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnCheckedChanged(e);
            CheckedChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Gets whether item with the specified index is current.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <returns></returns>
        public bool IsCurrent(int index)
        {
            return index == SelectedIndex;
        }

        /// <summary>
        /// Gets whether item with the specified index is selected.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <returns></returns>
        public virtual bool IsSelected(int index)
        {
            if (IsSelectionModeSingle)
            {
                return index == SelectedIndex;
            }
            else
            {
                var item = SafeItem(index);
                return item?.IsSelected(this) ?? false;
            }
        }

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
        public HVAlignment GetItemAlignment(int itemIndex)
        {
            return ListControlItem.GetAlignment(SafeItem(itemIndex), this);
        }

        /// <summary>
        /// Gets item minimal height.
        /// </summary>
        /// <param name="itemIndex">Index of the item.</param>
        /// <returns></returns>
        public Coord GetItemMinHeight(int itemIndex)
        {
            return ListControlItem.GetMinHeight(SafeItem(itemIndex), this);
        }

        /// <summary>
        /// Gets item image.
        /// </summary>
        /// <param name="itemIndex">Index of the item.</param>
        /// <param name="svgColor">Color of the svg image when item is selected.</param>
        /// <returns></returns>
        public EnumArrayStateImages GetItemImages(int itemIndex, Color? svgColor)
        {
            return ListControlItem.GetItemImages(SafeItem(itemIndex), this, svgColor);
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
        public bool GetItemShowCheckBox(ListControlItem item)
        {
            return item.GetShowCheckBox(this);
        }

        /// <summary>
        /// Gets selected item text color. Default is <see cref="SelectedItemTextColor"/>
        /// (if it is not <c>null</c>) or <see cref="DefaultSelectedItemTextColor"/>.
        /// </summary>
        /// <returns></returns>
        public Color? GetSelectedItemTextColor(int itemIndex)
        {
            return ListControlItem.GetSelectedTextColor(SafeItem(itemIndex), this);
        }

        /// <summary>
        /// Gets item text color. Default is <see cref="ItemTextColor"/> (if it is not <c>null</c>)
        /// or <see cref="DefaultItemTextColor"/>.
        /// </summary>
        /// <returns></returns>
        public Color? GetItemTextColor(int itemIndex)
        {
            return ListControlItem.GetItemTextColor(SafeItem(itemIndex), this);
        }

        /// <summary>
        /// Gets selected item back color. Default is <see cref="SelectedItemBackColor"/>
        /// (if it is not <c>null</c>) or <see cref="DefaultSelectedItemBackColor"/>.
        /// </summary>
        /// <returns></returns>
        public Color? GetSelectedItemBackColor(int itemIndex)
        {
            return ListControlItem.GetSelectedItemBackColor(SafeItem(itemIndex), this);
        }

        /// <summary>
        /// Sets colors used in the control to the dark theme.
        /// </summary>
        public virtual void SetColorThemeToDark()
        {
            BackgroundColor = (44, 44, 44);
            ForegroundColor = (204, 204, 204);

            SelectedItemTextColor = (255, 255, 255);
            SelectedItemBackColor = (214, 117, 64);
            CurrentItemBorder ??= new();
            CurrentItemBorder.SetColor(Color.White);
            ItemTextColor = (204, 204, 204);
        }

        /// <summary>
        /// Sets colors used in the control to the default theme.
        /// </summary>
        public virtual void SetColorThemeToDefault()
        {
            BackgroundColor = null;
            ForegroundColor = null;

            SelectedItemTextColor = null;
            SelectedItemBackColor = null;
            CurrentItemBorder?.SetColor(VirtualListBox.DefaultCurrentItemBorderColor);
            ItemTextColor = null;
        }

        /// <summary>
        /// Gets disabled item text color.
        /// </summary>
        /// <returns></returns>
        public Color? GetDisabledItemTextColor(int itemIndex)
        {
            return ListControlItem.GetDisabledTextColor(SafeItem(itemIndex), this);
        }

        /// <summary>
        /// Default method which draws items. Called from <see cref="DrawItemForeground"/>.
        /// </summary>
        public virtual void DefaultDrawItemForeground(ListBoxItemPaintEventArgs e)
        {
            var item = SafeItem(e.ItemIndex);
            if (item is null)
                ListControlItem.DefaultDrawForeground(this, e);
            else
                item.DrawForeground(this, e);
        }

        /// <inheritdoc/>
        public void ClearChecked()
        {
            ClearChecked(true);
        }

        /// <inheritdoc/>
        public void SetChecked(int index, bool value)
        {
            SetItemChecked(index, value);
        }

        /// <summary>
        /// Allows to set items from the <see cref="IEnumerable{T}"/> with huge number of items which
        /// is "yield" constructed. This method can be called from the
        /// another thread which is different
        /// from UI thread.
        /// </summary>
        /// <typeparam name="TSource">The type of the item in the source enumerable.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}"/> instance which
        /// is "yield" constructed in the another thread.</param>
        /// <param name="convertItem">The function which is called to convert
        /// <paramref name="source"/> items
        /// to the items which can be used in this control. If this function returns Null,
        /// source item is ignored. This function is called from the thread that
        /// provides <paramref name="source"/> so do not access UI elements from it.</param>
        /// <param name="continueFunc">The function which is called to check whether to continue
        /// the conversion. You can return False to stop the conversion. This function
        /// is called from the
        /// main thread so it can access UI elements.</param>
        /// <param name="bufferSize">Size of the items buffer. Optional. Default is 10.</param>
        /// <param name="sleepAfterBufferMsec">The value in milliseconds to wait after buffer is
        /// converted and all buffered items were added to the control.
        /// Optional. Default is 150.</param>
        public virtual void AddItemsThreadSafe<TSource>(
           IEnumerable<TSource> source,
           Func<TSource, ListControlItem?> convertItem,
           Func<bool> continueFunc,
           int bufferSize = 10,
           int sleepAfterBufferMsec = 150)
        {
            bool AddToDest(IEnumerable<ListControlItem> items)
            {
                var result = true;
                Invoke(() =>
                {
                    if (!continueFunc())
                        result = false;
                    Items.AddRange(items);
                    App.DoEvents();
                });
                Thread.Sleep(sleepAfterBufferMsec);
                return result;
            }

            EnumerableUtils.ConvertItems<TSource, ListControlItem>(
                        convertItem,
                        AddToDest,
                        source,
                        bufferSize);
        }

        /// <summary>
        /// Sets items from the specified collection to the control's items as fast as possible.
        /// </summary>
        /// <typeparam name="TItemFrom">Type of the item in the otgher collection
        /// that will be assigned to the control's items.</typeparam>
        /// <param name="from">The collection to be assigned to item of the control.</param>
        /// <param name="fnAssign">Assign item action. Must assign item data.</param>
        /// <param name="fnCreateItem">Create item action.</param>
        public virtual void SetItemsFast<TItemFrom>(
            IEnumerable<TItemFrom> from,
            Action<ListControlItem, TItemFrom> fnAssign,
            Func<ListControlItem> fnCreateItem)
        {
            var count = from.Count();

            BeginUpdate();
            try
            {
                SetCount(count, fnCreateItem);
                var i = 0;
                foreach (var itemFrom in from)
                {
                    var itemTo = Items[i];
                    fnAssign(itemTo, itemFrom);
                    i++;
                }
            }
            finally
            {
                EndUpdate();
            }

            SelectedIndex = -1;
        }

        int IListControlItemContainer.GetItemCount()
        {
            return Items.Count;
        }

        internal ListControlItem.ItemCheckBoxInfo? GetCheckBoxInfo(int itemIndex, RectD rect)
        {
            var item = SafeItem(itemIndex);
            if (item is null)
                return null;
            return item.GetCheckBoxInfo(this, rect);
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

        /// <inheritdoc/>
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            RunSelectedItemDoubleClickAction();
        }

        /// <summary>
        /// Implements controller for the add range operation which is performed
        /// in the background thread. It allows to continue work with the control while
        /// new items are added to it.
        /// </summary>
        /// <typeparam name="TSource">Type of the source item.</typeparam>
        public class AddRangeController<TSource> : DisposableObject
        {
            /// <summary>
            /// Gets or sets control on which add range operation is performed.
            /// </summary>
            public VirtualListControl ListBox;

            /// <summary>
            /// Gets or sets the function which provides the <see cref="IEnumerable{T}"/>
            /// instance which
            /// is "yield" constructed in the another thread.
            /// </summary>
            public Func<IEnumerable<TSource>> SourceFunc;

            /// <summary>
            /// Gets or sets the function which is called to convert
            /// source items
            /// to the items which can be used in this control. If this function returns Null,
            /// source item is ignored. This function is called from the thread that
            /// provides source items so do not access UI elements from it.
            /// </summary>
            public Func<TSource, ListControlItem?> ConvertItemFunc;

            /// <summary>
            /// Gets or sets the function which is called to check whether to continue
            /// the conversion. You can return False to stop the conversion.
            /// This function is called from the
            /// main thread so it can access UI elements.
            /// </summary>
            public Func<bool> ContinueFunc;

            /// <summary>
            /// Gets or sets whether debug information is logged.
            /// </summary>
            public bool IsDebugInfoLogged = false;

            /// <summary>
            /// Gets or sets object name for the debug purposes.
            /// </summary>
            public string? Name;

            /// <summary>
            /// Gets or sets size of the items buffer. Default is 10.
            /// </summary>
            public int BufferSize = 10;

            /// <summary>
            /// Gets or sets the value in milliseconds to wait after buffer is
            /// converted and all buffered items were added to the control.
            /// Default is 150.
            /// </summary>
            public int SleepAfterBufferMsec = 150;

            private Thread? thread;

            /// <summary>
            /// Initializes a new instance of the <see cref="AddRangeController{TSource}"/>
            /// class with the specified parameters.
            /// </summary>
            public AddRangeController(
                VirtualListControl listBox,
                Func<IEnumerable<TSource>> source,
                Func<TSource, ListControlItem?> convertItem,
                Func<bool> continueFunc)
            {
                ListBox = listBox;
                SourceFunc = source;
                ConvertItemFunc = convertItem;
                ContinueFunc = continueFunc;
            }

            /// <summary>
            /// Gets name of the object safely so it will always be not empty.
            /// </summary>
            public virtual string SafeName => Name ?? "ListBox.AddRangeController";

            /// <summary>
            /// Stops controller add range operation.
            /// </summary>
            public virtual void Stop()
            {
                AsyncUtils.EndThread(ref thread);
            }

            /// <summary>
            /// Starts controller add range operation.
            /// </summary>
            public virtual void Start()
            {
                Stop();

                thread = new Thread(ThreadAction)
                {
                    Name = $"{SafeName}Thread",
                    IsBackground = true,
                };

                thread.Start();
            }

            /// <inheritdoc/>
            protected override void DisposeManaged()
            {
                Stop();
            }

            private void ThreadAction()
            {
                void Fn()
                {
                    ListBox.AddItemsThreadSafe(
                        SourceFunc(),
                        ConvertItemFunc,
                        () =>
                        {
                            if (ListBox.IsDisposed)
                                return false;
                            return ContinueFunc();
                        },
                        BufferSize,
                        SleepAfterBufferMsec);
                }

                CallThreadAction(Fn);
            }

            private void CallThreadAction(Action action)
            {
                try
                {
                    action();
                }
                catch (ThreadInterruptedException)
                {
                    App.DebugIdleLogIf($"{SafeName} awoken.", IsDebugInfoLogged);
                }
                catch (ThreadAbortException)
                {
                    App.DebugIdleLogIf($"{SafeName} aborted.", IsDebugInfoLogged);
                }
                finally
                {
                    App.DebugIdleLogIf($"{SafeName} executing finally block.", IsDebugInfoLogged);
                }
            }
        }
    }
}