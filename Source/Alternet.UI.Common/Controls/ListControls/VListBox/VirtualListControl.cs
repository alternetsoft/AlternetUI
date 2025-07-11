using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
        /// Gets or sets default selected item text color for the unfocused container.
        /// </summary>
        public static LightDarkColor DefaultUnfocusedSelectedItemTextColor
            = Color.LightDark(light: SystemColors.WindowText, dark: SystemColors.HighlightText);

        /// <summary>
        /// Gets or sets default selected item background color for the unfocused container.
        /// </summary>
        public static LightDarkColor DefaultUnfocusedSelectedItemBackColor
            = Color.LightDark(light: (227, 227, 227), dark: (61, 61, 61));

        /// <summary>
        /// Gets or sets default selected item text color.
        /// </summary>
        public static LightDarkColor DefaultSelectedItemTextColor
            = Color.LightDark(SystemColors.HighlightText);

        /// <summary>
        /// Gets or sets default selected item background color.
        /// </summary>
        public static LightDarkColor DefaultSelectedItemBackColor
            = Color.LightDark(SystemColors.Highlight);

        /// <summary>
        /// Gets or sets default disabled item text color.
        /// </summary>
        public static LightDarkColor DefaultDisabledItemTextColor
            = Color.LightDark(SystemColors.GrayText);

        /// <summary>
        /// Gets or sets default item text color.
        /// </summary>
        public static LightDarkColor DefaultItemTextColor
            = Color.LightDark(SystemColors.WindowText);

        /// <summary>
        /// Gets or sets default border color for the current item.
        /// This is used when <see cref="DefaultCurrentItemBorder"/>
        /// is created.
        /// </summary>
        public static LightDarkColor DefaultCurrentItemBorderColor
            = Color.LightDark(light: Color.Gray600, dark: Color.White);

        private static BorderSettings? defaultCurrentItemBorder;

        private Color? selectedItemTextColor;
        private Color? itemTextColor;
        private Color? selectedItemBackColor;
        private Color? disabledItemTextColor;
        private Color? unfocusedSelectedItemBackColor;
        private Color? unfocusedSelectedItemTextColor;

        private Thickness itemMargin = DefaultItemMargin;
        private IListBoxItemPainter? painter;
        private Coord minItemHeight = DefaultMinItemHeight;
        private BorderSettings? currentItemBorder;
        private BorderSettings? selectionBorder;
        private WeakReferenceValue<ImageList> imageList = new();
        private BaseCollection<ListControlColumn>? columns;

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
        /// Gets or sets default border of the current item.
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
        /// Gets or sets the <see cref="ImageList"/> associated with the control.
        /// </summary>
        /// <value>An <see cref="ImageList"/> that contains the images to be used by the control.
        /// If no <see cref="ImageList"/> is set, this property returns null.</value>
        public virtual ImageList? ImageList
        {
            get
            {
                return imageList.Value;
            }

            set
            {
                if (ImageList == value)
                    return;
                imageList.Value = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets default size of the svg images.
        /// </summary>
        /// <remarks>
        /// Each item has <see cref="ListControlItem.SvgImageSize"/> property where
        /// this setting can be overridden. If <see cref="SvgImageSize"/> is not specified,
        /// default toolbar image size is used. Currently only rectangular svg images
        /// are supported.
        /// </remarks>
        [Browsable(false)]
        public virtual SizeI? SvgImageSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the checkbox should be
        /// toggled when an item is clicked on the checkbox area.
        /// </summary>
        public virtual bool CheckOnClick { get; set; } = true;

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
        /// Gets a value indicating whether the control has columns defined.
        /// </summary>
        /// <value>
        /// <c>true</c> if the control has one or more columns; otherwise, <c>false</c>.
        /// </value>
        [Browsable(false)]
        public virtual bool HasColumns
        {
            get
            {
                return columns is not null && columns.Count > 0;
            }
        }

        /// <summary>
        /// Gets or sets the collection of list control columns.
        /// </summary>
        /// <remarks>
        /// This collection defines the structure of the list control by managing multiple columns.
        /// Each column represents an individual segment of the control with specific layout properties.
        /// Currently this is under development and is not working yet.
        /// </remarks>
        public virtual BaseCollection<ListControlColumn> Columns
        {
            get
            {
                if(columns is null)
                {
                    columns = new NotNullCollection<ListControlColumn>();
                    columns.CollectionChanged += OnColumnsChanged;
                }

                return columns;
            }

            set
            {
                if (columns == value)
                    return;
                if(columns is not null)
                    columns.CollectionChanged -= OnColumnsChanged;
                columns = value;
                if (columns is not null)
                    columns.CollectionChanged += OnColumnsChanged;
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
        /// Gets or sets selected item text color for the unfocused control.
        /// </summary>
        [Browsable(false)]
        public virtual Color? UnfocusedSelectedItemTextColor
        {
            get
            {
                return unfocusedSelectedItemTextColor;
            }

            set
            {
                if (unfocusedSelectedItemTextColor == value)
                    return;
                unfocusedSelectedItemTextColor = value;
                if (!Focused)
                    Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets selected item back color for the unfocused control.
        /// </summary>
        [Browsable(false)]
        public virtual Color? UnfocusedSelectedItemBackColor
        {
            get
            {
                return unfocusedSelectedItemBackColor;
            }

            set
            {
                if (unfocusedSelectedItemBackColor == value)
                    return;
                unfocusedSelectedItemBackColor = value;
                if(!Focused)
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
        public virtual Font GetItemFont(int itemIndex, bool isSelected)
        {
            return ListControlItem.GetFont(SafeItem(itemIndex), this, isSelected);
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
        public abstract int? HitTest(PointD position);

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
            BackgroundColor = DefaultColors.ControlBackColor.Dark;
            ForegroundColor = DefaultColors.ControlForeColor.Dark;

            SelectedItemTextColor = null;
            SelectedItemBackColor = null;
            CurrentItemBorder ??= new();
            CurrentItemBorder.SetColor(DefaultCurrentItemBorderColor.Dark);
            ItemTextColor = (204, 204, 204);
        }

        /// <summary>
        /// Sets colors used in the control to the default theme.
        /// </summary>
        public virtual void SetColorThemeToDefault()
        {
            BackColor = DefaultColors.ControlBackColor;
            ForeColor = DefaultColors.ControlForeColor;

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
            bool AddToDestination(IEnumerable<ListControlItem> items)
            {
                var result = true;
                var ttt = items.ToArray();
                Invoke(() =>
                {
                    if (DisposingOrDisposed || !continueFunc())
                        result = false;
                    DoInsideUpdate(() =>
                    {
                        Items.AddRange(ttt);
                    });
                });
                Thread.Sleep(sleepAfterBufferMsec);
                return result;
            }

            EnumerableUtils.ConvertItems<TSource, ListControlItem>(
                        convertItem,
                        AddToDestination,
                        source,
                        bufferSize);
        }

        /// <summary>
        /// Sets items from the specified collection to the control's items as fast as possible.
        /// </summary>
        /// <typeparam name="TItemFrom">Type of the item in the other collection
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

        /// <summary>
        /// Retrieves information about the checkbox associated with a specific item in the control.
        /// </summary>
        /// <param name="itemIndex">The zero-based index of the item in the control.</param>
        /// <param name="rect">The bounding rectangle of the item.</param>
        /// <returns>
        /// An <see cref="ListControlItem.ItemCheckBoxInfo"/> object containing details about the checkbox,
        /// or <c>null</c> if the item does not have a checkbox.
        /// </returns>
        public ListControlItem.ItemCheckBoxInfo? GetCheckBoxInfo(int itemIndex, RectD rect)
        {
            var item = SafeItem(itemIndex);
            if (item is null)
                return null;
            rect.ApplyMargin(item.ForegroundMargin);
            return item.GetCheckBoxInfo(this, rect);
        }

        /// <summary>
        /// Updates the vertical and horizontal scrollbars.
        /// </summary>
        public abstract void UpdateScrollBars(bool refresh);

        /// <summary>
        /// Called when when the checkbox state of the item has changed.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        /// <remarks>See <see cref="CheckedChanged"/> for details.</remarks>
        protected virtual void OnCheckedChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Handles changes to the columns collection.
        /// </summary>
        /// <param name="sender">The source of the change event.</param>
        /// <param name="e">Details about the collection change, including added
        /// or removed items.</param>
        /// <remarks>
        /// This method is triggered when the column collection is modified.
        /// Override it to implement custom behavior for handling column updates.
        /// </remarks>
        protected virtual void OnColumnsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            Invalidate();
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
        public class RangeAdditionController<TSource> : DisposableObject
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

            private const bool logControllerState = false;

            private static int globalUniqueNumber;

            private readonly int uniqueNumber = ++globalUniqueNumber;

            private CancellationTokenSource? cts;

            /// <summary>
            /// Initializes a new instance of the <see cref="RangeAdditionController{TSource}"/>
            /// class with the specified parameters.
            /// </summary>
            public RangeAdditionController(
                VirtualListControl listBox,
                Func<IEnumerable<TSource>> source,
                Func<TSource, ListControlItem?> convertItem,
                Func<bool> continueFunc)
            {
                ListBox = listBox;
                SourceFunc = source;
                ConvertItemFunc = convertItem;
                ContinueFunc = continueFunc;

                App.DebugLogIf($"{SafeName} Created.", logControllerState);
            }

            /// <summary>
            /// Gets name of the object safely so it will always be not empty.
            /// </summary>
            public virtual string SafeName => Name ?? ("ListBox.RangeAdditionController" + uniqueNumber);

            /// <summary>
            /// Gets a value indicating whether the cancellation has been requested for the operation.
            /// </summary>
            public virtual bool IsCancellationRequested
            {
                get
                {
                    return cts?.Token.IsCancellationRequested ?? true;
                }
            }

            /// <summary>
            /// Stops controller add range operation.
            /// </summary>
            public virtual void Stop()
            {
                cts?.Cancel();
                cts = null;
            }

            /// <summary>
            /// Starts controller add range operation.
            /// </summary>
            public virtual void Start(Action? onComplete = null)
            {
                Stop();

                cts = new();
                var ctsCopy = cts;

                App.AddBackgroundTask(() =>
                {
                    Task result;

                    if (DisposingOrDisposed || cts is null || cts.Token.IsCancellationRequested)
                        result = Task.CompletedTask;
                    else
                        result = new Task(ThreadAction, ctsCopy.Token);

                    result.ContinueWith((task) =>
                    {
                        Invoke(onComplete);
                    });

                    return result;
                });
            }

            /// <inheritdoc/>
            protected override void DisposeManaged()
            {
                Stop();
                App.DebugLogIf($"{SafeName} Disposed.", logControllerState);
            }

            private void ThreadAction()
            {
                var ctsCopy = cts ?? new();

                App.DebugLogIf($"{SafeName} Started.", logControllerState);
                ListBox.RemoveAll();
                ListBox.AddItemsThreadSafe(
                    SourceFunc(),
                    ConvertItemFunc,
                    () =>
                    {
                        if (ctsCopy.Token.IsCancellationRequested || ListBox.IsDisposed)
                            return false;
                        return ContinueFunc();
                    },
                    BufferSize,
                    SleepAfterBufferMsec);
                App.DebugLogIf($"{SafeName} Finished.", logControllerState);
            }
        }
    }
}