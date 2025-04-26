﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

using Alternet.Base.Collections;
using Alternet.Drawing;
using Alternet.UI.Extensions;

namespace Alternet.UI
{
    /// <summary>
    /// Advanced list box control with ability to customize item painting.
    /// This control enables you to display a list of items
    /// to the user that the user can select by clicking.
    /// Works fine with
    /// large number of the items. You can add <see cref="ListControlItem"/> items to this control.
    /// </summary>
    /// <remarks>
    /// This control can provide single or multiple selections
    /// using the <see cref="VirtualListControl.SelectionMode"/> property.
    /// The <see cref="AbstractControl.BeginUpdate"/>
    /// and <see cref="AbstractControl.EndUpdate"/> methods enable
    /// you to add a large number of items without the control being repainted each
    /// time an item is added to the list.
    /// The <see cref="ListControl{T}.Items"/>, <see cref="VirtualListControl.SelectedItems"/>
    /// and <see cref="VirtualListControl.SelectedIndices"/>
    /// properties provide access to
    /// the three collections that are used by the control.
    /// </remarks>
    public partial class VirtualListBox : VirtualListControl, IListControl
    {
        /// <summary>
        /// Defines the default increment for horizontal scrollbar position.
        /// Value is specified in characters. Default is 4.
        /// </summary>
        public static int DefaultHorizontalScrollBarLargeIncrement = 4;

        private static SetItemsKind defaultSetItemsKind = SetItemsKind.ChangeField;

        private Coord scrollOffset;
        private ListBoxItemPaintEventArgs? itemPaintArgs;
        private Coord horizontalExtent;
        private DrawMode drawMode = DrawMode.Normal;
        private int firstVisibleItem;
        private ScrollBarSettings? horizontalScrollBarSettings;
        private ScrollBarSettings? verticalScrollBarSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualListBox"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public VirtualListBox(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualListBox"/> class.
        /// </summary>
        public VirtualListBox()
        {
            BorderStyle = ControlBorderStyle.Simple;
            UserPaint = true;
            SuggestedSize = 200;
            IsScrollable = true;
            BackColor = DefaultColors.ControlBackColor;
            ForeColor = DefaultColors.ControlForeColor;
        }

        /// <summary>
        /// Occurs when a visual aspect of an owner-drawn control is changed.
        /// </summary>
        [Category("Behavior")]
        public event DrawItemEventHandler? DrawItem;

        /// <summary>
        /// Occurs when the sizes of the list items are determined.
        /// </summary>
        [Category("Behavior")]
        public event MeasureItemEventHandler? MeasureItem;

        /// <summary>
        /// Enumerates known flags for the item click method.
        /// </summary>
        [Flags]
        public enum ItemClickFlags
        {
            /// <summary>
            /// Item is shift-clicked
            /// </summary>
            Shift = 1,

            /// <summary>
            /// Item is ctrl-clicked.
            /// </summary>
            Ctrl = 2,

            /// <summary>
            /// Item selected from keyboard.
            /// </summary>
            Keyboard = 4,
        }

        /// <summary>
        /// Enumerates supported kinds for <see cref="SetItemsFast"/> method.
        /// </summary>
        public enum SetItemsKind
        {
            /// <summary>
            /// Items are cleared and after that 'AddRange' method is called.
            /// </summary>
            ClearAddRange,

            /// <summary>
            /// Internal field is changed to the new value. This is the fastest method.
            /// </summary>
            ChangeField,

            /// <summary>
            /// Uses <see cref="DefaultSetItemsKind"/> to get the desired method.
            /// </summary>
            Default,
        }

        /// <summary>
        /// Gets or sets the way how items are set when <see cref="SetItemsKind.Default"/>
        /// is specified in <see cref="SetItemsFast"/>. Default is
        /// <see cref="SetItemsKind.ChangeField"/>.
        /// </summary>
        public static SetItemsKind DefaultSetItemsKind
        {
            get
            {
                return defaultSetItemsKind;
            }

            set
            {
                if (value == SetItemsKind.Default)
                    return;
                defaultSetItemsKind = value;
            }
        }

        /// <summary>
        /// Gets the horizontal scroll offset.
        /// </summary>
        /// <value>
        /// A <see cref="Coord"/> representing the current horizontal scroll position.
        /// </value>
        public Coord ScrollOffsetX => scrollOffset;

        /// <summary>
        /// Indicates whether vertical scrollbar settings are defined.
        /// </summary>
        [Browsable(false)]
        public bool HasVerticalScrollBarSettings => verticalScrollBarSettings is not null;

        /// <summary>
        /// Indicates whether horizontal scrollbar settings are defined.
        /// </summary>
        [Browsable(false)]
        public bool HasHorizontalScrollBarSettings => horizontalScrollBarSettings is not null;

        /// <summary>
        /// Gets the horizontal scrollbar settings. Initializes them if required.
        /// </summary>
        [Browsable(false)]
        public virtual ScrollBarSettings HorizontalScrollBarSettings
        {
            get
            {
                if (horizontalScrollBarSettings is null)
                {
                    horizontalScrollBarSettings = new();
                    horizontalScrollBarSettings.PropertyChangedAction = (e) =>
                    {
                        UpdateScrollBars(true);
                    };
                }

                return horizontalScrollBarSettings;
            }
        }

        /// <summary>
        /// Gets the vertical scrollbar settings. Initializes them if required.
        /// </summary>
        [Browsable(false)]
        public virtual ScrollBarSettings VerticalScrollBarSettings
        {
            get
            {
                if (verticalScrollBarSettings is null)
                {
                    verticalScrollBarSettings = new();
                    verticalScrollBarSettings.PropertyChangedAction = (e) =>
                    {
                        UpdateScrollBars(true);
                    };
                }

                return verticalScrollBarSettings;
            }
        }

        /// <summary>
        /// Gets or sets the width by which the horizontal scroll bar can scroll.
        /// </summary>
        /// <returns>
        /// The width, in device-independent units, that the horizontal scroll bar can
        /// scroll the control. The default is zero.
        /// </returns>
        [Category("Behavior")]
        [DefaultValue(0)]
        [Localizable(true)]
        [Browsable(false)]
        public virtual Coord HorizontalExtent
        {
            get
            {
                return horizontalExtent;
            }

            set
            {
                if (value == horizontalExtent)
                    return;
                horizontalExtent = value;
            }
        }

        /// <inheritdoc/>
        public override ControlBorderStyle BorderStyle
        {
            get => base.BorderStyle;
            set
            {
                if (BorderStyle == value)
                    return;
                base.BorderStyle = value;
                UpdateScrollBars(true);
            }
        }

        /// <inheritdoc/>
        [Browsable(true)]
        public override bool HasBorder
        {
            get
            {
                return BorderStyle != ControlBorderStyle.None;
            }

            set
            {
                if (HasBorder == value)
                    return;
                base.HasBorder = value;
                if (value)
                    BorderStyle = ControlBorderStyle.Theme;
                else
                    BorderStyle = ControlBorderStyle.None;
            }
        }

        /// <inheritdoc/>
        public override bool UserPaint
        {
            get => base.UserPaint;
            set => base.UserPaint = true;
        }

        /// <summary>
        /// Gets number of visible items.
        /// </summary>
        [Browsable(false)]
        public virtual int VisibleCount
        {
            get
            {
                var firstVisibleItem = GetVisibleBegin();
                var lastVisibleItem = GetVisibleEnd();
                if (firstVisibleItem < 0 || lastVisibleItem < 0)
                    return 0;
                return lastVisibleItem - firstVisibleItem + 1;
            }
        }

        /// <inheritdoc/>
        public override int Count
        {
            get
            {
                return Items.Count;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                if (Count == value)
                    return;
                DoInsideUpdate(() =>
                {
                    SafeItems().SetCount(value, () => new ListControlItem());
                });
            }
        }

        /// <summary>
        /// Gets or sets the drawing mode for the control.
        /// </summary>
        /// <returns>
        /// One of the <see cref="DrawMode" /> values representing the mode for drawing
        /// the items of the control. The default is <see langword="DrawMode.Normal" />.
        /// </returns>
        [Category("Behavior")]
        [DefaultValue(DrawMode.Normal)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public virtual DrawMode DrawMode
        {
            get
            {
                return drawMode;
            }

            set
            {
                if (drawMode == value)
                    return;
                drawMode = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the index of the first visible item in the control.</summary>
        /// <returns>The zero-based index of the first visible item in the control.</returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int TopIndex
        {
            get
            {
                return GetVisibleBegin();
            }

            set
            {
                if (TopIndex == value)
                    return;
                ScrollToRow(value);
            }
        }

        /// <summary>
        /// Alias for <see cref="ContextMenuStrip"/> property.
        /// </summary>
        [Browsable(false)]
        public ContextMenuStrip ContextMenu
        {
            get => ContextMenuStrip;
            set => ContextMenuStrip = value;
        }

        /// <inheritdoc cref="StringSearch.FindStringEx(string?, int?, bool, bool)"/>
        /// <remarks>
        /// If text is found in the control, item which contains this text will be selected
        /// and scrolled into the view.
        /// </remarks>
        public virtual int? FindAndSelect(
         string? str,
         int? startIndex,
         bool exact,
         bool ignoreCase)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                SelectedIndex = null;
                return null;
            }

            var result = FindStringEx(str, startIndex, exact, ignoreCase);

            DoInsideUpdate(() =>
            {
                SelectedIndex = result;
                if (result is not null)
                    EnsureVisible(result.Value);
            });

            return result;
        }

        /// <summary>
        /// Gets whether item with the specified index is visible.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <returns></returns>
        public virtual bool IsItemVisible(int index)
        {
            if (DisposingOrDisposed)
                return default;
            var visibleBegin = GetVisibleBegin();
            var visibleEnd = GetVisibleEnd();

            return index >= visibleBegin && index < visibleEnd;
        }

        /// <summary>
        /// Scroll by the specified number of rows which may be positive (to scroll down)
        /// or negative (to scroll up).
        /// </summary>
        /// <param name="rows">Number of items to scroll.</param>
        /// <returns><c>true</c> if the control was scrolled, <c>false</c> otherwise
        /// (for example, if we're trying to scroll down but we are already
        /// showing the last row).</returns>
        public virtual bool ScrollRows(int rows)
        {
            if (DisposingOrDisposed)
                return default;
            rows += GetVisibleBegin();
            if (rows < 0)
                rows = 0;
            return ScrollToRow(rows);
        }

        /// <summary>
        /// Scrolls to the specified row.
        /// </summary>
        /// <param name="row">It will become the first visible row in the control.</param>
        /// <returns>True if we scrolled the control, False if nothing was done.</returns>
        public virtual bool ScrollToRow(int row)
        {
            if (DisposingOrDisposed || Count == 0)
                return default;
            row = Math.Min(Count - 1, row);
            row = Math.Max(0, row);

            // determine the real first unit to scroll to: we shouldn't scroll beyond the end
            var unitFirstLast = FindFirstVisibleFromLast(Count - 1, true);
            if (row > unitFirstLast)
                row = unitFirstLast;

            if (row == firstVisibleItem)
                return false;

            firstVisibleItem = row;

            UpdateScrollBars(true);
            return true;
        }

        /// <summary>
        /// Sets items from the specified collection to the control's items as fast as possible.
        /// </summary>
        public virtual void SetItemsFast<TItemFrom>(
            IEnumerable<TItemFrom> from,
            Action<ListControlItem, TItemFrom> fnAssign)
        {
            SetItemsFast(from, fnAssign, () => new ListControlItem());
        }

        /// <summary>
        /// Scroll by the specified number of pages which may be positive
        /// (to scroll down) or negative (to scroll up).
        /// </summary>
        /// <param name="pages">Number of pages to scroll.</param>
        /// <returns><c>true</c> if the control was scrolled, <c>false</c> otherwise
        /// (for example, if we're trying to scroll down but we are already
        /// showing the last row).</returns>
        public virtual bool ScrollRowPages(int pages)
        {
            if (DisposingOrDisposed)
                return default;

            bool didSomething = false;

            while (pages != 0)
            {
                int unit;
                if (pages > 0)
                {
                    unit = GetVisibleEnd();
                    if (unit != 0)
                        --unit;
                    --pages;
                }
                else
                {
                    unit = FindFirstVisibleFromLast(GetVisibleEnd());
                    ++pages;
                }

                didSomething = ScrollToRow(unit);
            }

            return didSomething;
        }

        /// <summary>
        /// Removes selected items and updates selection so the closest item to the
        /// previous selection will become selected.
        /// </summary>
        /// <returns></returns>
        public virtual bool RemoveSelectedAndUpdateSelection()
        {
            bool removed = false;

            DoInsideSuspendedSelectionEvents(() =>
            {
                var selectedIndex = SelectedIndex;

                removed = RemoveSelectedItems();

                if (removed)
                {
                    if (selectedIndex != null)
                    {
                        if (selectedIndex < Count)
                        {
                            SelectedIndex = selectedIndex;
                            EnsureVisible(selectedIndex.Value);
                        }
                        else
                            SelectLastItemAndScroll();
                    }
                }
            });

            return removed;
        }

        /// <summary>
        /// Finds first visible item from the specified last visible item.
        /// </summary>
        /// <param name="unitLast">Index of the last visible item.</param>
        /// <param name="full">Whether to allow partial or full visibility of the last item.</param>
        /// <returns></returns>
        public virtual int FindFirstVisibleFromLast(int unitLast, bool full = false)
        {
            if (unitLast == 0)
                return 0;

            MeasureItemEventArgs e = new(MeasureCanvas, 0);

            var sWindow = ClientSize.Height;

            // go upwards until we arrive at a unit such that unitLast is not visible
            // any more when it is shown
            int unitFirst = unitLast;
            Coord s = 0;
            while (true)
            {
                e.Index = unitFirst;
                MeasureItemSize(e);

                s += e.ItemHeight;

                if (s > sWindow)
                {
                    // for this unit to be fully visible we need to go one unit
                    // down, but if it is enough for it to be only partly visible then
                    // this unit will do as well
                    if (full)
                    {
                        ++unitFirst;
                    }

                    break;
                }

                if (unitFirst <= 0)
                    break;

                --unitFirst;
            }

            return unitFirst;
        }

        /// <summary>
        /// Shows list editor dialog which allows to edit items.
        /// </summary>
        public virtual void EditItemsWithListEditor()
        {
            DialogFactory.EditItemsWithListEditor(this);
        }

        /// <inheritdoc/>
        public override void RemoveAll()
        {
            if (DisposingOrDisposed)
                return;
            if (Items.Count == 0)
                return;

            DoInsideUpdate(() =>
            {
                RecreateItems();
                Invalidate();
            });
        }

        /// <summary>
        /// Sets items to the new value using the specified method.
        /// </summary>
        /// <param name="value">Collection with the new items.</param>
        /// <param name="kind">The method which is used when items are set.</param>
        public virtual bool SetItemsFast(VirtualListBoxItems value, SetItemsKind kind)
        {
            if (DisposingOrDisposed)
                return default;

            if (kind == SetItemsKind.Default)
                kind = DefaultSetItemsKind;
            if (kind == SetItemsKind.Default)
                kind = SetItemsKind.ChangeField;

            switch (kind)
            {
                case SetItemsKind.ClearAddRange:
                    return UseClearAddRange();
                case SetItemsKind.ChangeField:
                    return UseChangeField();
                default:
                    return false;
            }

            bool UseClearAddRange()
            {
                DoInsideUpdate(() =>
                {
                    RemoveAll();
                    Items.AddRange(value);
                });

                return true;
            }

            bool UseChangeField()
            {
                DoInsideUpdate(() =>
                {
                    ClearSelected();
                    DetachItems(Items);
                    RecreateItems(value);
                    AttachItems(Items);
                });

                return true;
            }
        }

        /// <summary>
        /// Gets the rectangle that represents the bounds of the item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item.</param>
        /// <returns>
        /// A <see cref="RectD"/> that represents the bounds of the item, or <c>null</c> if
        /// the index is invalid, item is not currently visible or the control is disposed.
        /// </returns>
        public virtual RectD? GetItemRect(int? index)
        {
            if (index is null || DisposingOrDisposed)
                return default;
            var n = index.Value;

            var lineMax = GetVisibleEnd();
            if (n >= lineMax)
                return null;
            var line = GetVisibleBegin();
            if (n < line)
                return null;

            MeasureItemEventArgs e = new(MeasureCanvas, 0);
            RectD itemRect = (0, 0, ClientSize.Width, 0);

            while (line <= n)
            {
                e.Index = line;
                MeasureItemSize(e);

                itemRect.Y += itemRect.Height;
                itemRect.Height = e.ItemHeight;

                line++;
            }

            return itemRect;
        }

        /// <inheritdoc/>
        public override int? HitTest(PointD position)
        {
            if (DisposingOrDisposed)
                return null;
            var y = position.Y;
            var unitMax = GetVisibleEnd();

            MeasureItemEventArgs e = new(MeasureCanvas, 0);

            for (int unit = GetVisibleBegin(); unit < unitMax; ++unit)
            {
                e.Index = unit;
                MeasureItemSize(e);
                y -= e.ItemHeight;
                if (y < 0)
                    return unit;
            }

            return null;
        }

        /// <summary>
        /// Triggers a refresh for the area between the specified range of rows given (inclusively).
        /// </summary>
        /// <param name="from">First item index.</param>
        /// <param name="to">Last item index.</param>
        public virtual void RefreshRows(int from, int to)
        {
            if (DisposingOrDisposed || CanSkipInvalidate())
                return;

            if (from > to)
                return;

            if (from < GetVisibleBegin())
                from = GetVisibleBegin();

            if (to > GetVisibleEnd())
                to = GetVisibleEnd();

            Coord orientSize = 0;
            Coord orientPos = 0;

            MeasureItemEventArgs e = new(MeasureCanvas, 0);

            for (int nBefore = GetVisibleBegin();
                  nBefore < from;
                  nBefore++)
            {
                e.Index = nBefore;
                MeasureItemSize(e);

                orientPos += e.ItemHeight;
            }

            for (int nBetween = from; nBetween <= to; nBetween++)
            {
                e.Index = nBetween;
                MeasureItemSize(e);
                orientSize += e.ItemHeight;
            }

            RectD rect = (0, orientPos, ClientSize.Width, orientSize);
            Invalidate(rect);
        }

        /// <summary>
        /// Triggers a refresh for just the given row's area of the control if it is visible.
        /// </summary>
        /// <param name="row">Item index.</param>
        public virtual void RefreshRow(int row)
        {
            if (DisposingOrDisposed || CanSkipInvalidate())
                return;

            if (!IsItemVisible(row))
                return;

            MeasureItemEventArgs e = new(MeasureCanvas, row);
            MeasureItemSize(e);

            RectD rect = (0, 0, ClientSize.Width, e.ItemHeight);

            for (int n = GetVisibleBegin(); n < row; ++n)
            {
                e.Index = n;
                MeasureItemSize(e);
                rect.Top += e.ItemHeight;
            }

            Invalidate(rect);
        }

        /// <summary>
        /// Selects item on the next page.
        /// </summary>
        public virtual void SelectItemOnNextPage()
        {
            var selected = SelectedIndex;

            if (selected is null)
            {
                SelectFirstItem();
                return;
            }

            var numVisible = VisibleCount - 1;

            if (numVisible <= 0)
                return;

            SelectedIndex = Math.Min(selected.Value + numVisible, Count - 1);
        }

        /// <summary>
        /// Selects item on the previous page.
        /// </summary>
        public virtual void SelectItemOnPreviousPage()
        {
            var selected = SelectedIndex;

            if (selected is null)
            {
                SelectFirstItem();
                return;
            }

            var numVisible = VisibleCount - 1;

            if (numVisible <= 0)
                return;

            SelectedIndex = Math.Max(selected.Value - numVisible, 0);
        }

        /// <summary>
        /// Selects next item.
        /// </summary>
        public virtual void SelectNextItem()
        {
            if (SelectedIndex is null)
            {
                SelectFirstItem();
            }
            else
            if (SelectedIndex == Count - 1)
            {
            }
            else
            {
                SelectedIndex++;
            }
        }

        /// <summary>
        /// Selects previous item.
        /// </summary>
        public virtual void SelectPreviousItem()
        {
            if (SelectedIndex is null)
            {
                SelectFirstItem();
            }
            else
            if (SelectedIndex == 0)
            {
            }
            else
            {
                SelectedIndex--;
            }
        }

        /// <summary>
        /// Selects items with the specified indexes
        /// and scrolls the control so the first selected item will become visible in the view.
        /// </summary>
        public virtual bool SelectItemsAndScroll(params int[] indexes)
        {
            var validIndexes = GetValidIndexes(indexes);
            if (validIndexes.Count == 0)
                return false;
            SelectedIndices = validIndexes;
            EnsureVisible(validIndexes[0]);
            return true;
        }

        /// <summary>
        /// Selects the first item in the tree view control and scrolls to it.
        /// </summary>
        /// <remarks>
        /// This method ensures that the first item is selected and visible.
        /// If no items are present, no action is taken.
        /// </remarks>
        public virtual void SelectFirstItemAndScroll()
        {
            SelectFirstItem();
            ScrollToFirstRow();
        }

        /// <summary>
        /// Gets the number of items currently rendered in the control.
        /// </summary>
        /// <returns>The count of visible items rendered in the control.</returns>
        public virtual int GetRenderedItemCount()
        {
            var visibleBegin = GetVisibleBegin();
            var visibleEnd = GetVisibleEnd();
            var visibleItems = visibleEnd - visibleBegin;
            return visibleItems;
        }

        /// <summary>
        /// Selects last item in the control and scrolls the control so last item will be visible.
        /// </summary>
        public virtual void SelectLastItemAndScroll()
        {
            SelectLastItem();
            ScrollToLastRow();
        }

        /// <summary>
        /// Gets index of the first visible item.
        /// </summary>
        /// <returns></returns>
        public virtual int GetVisibleBegin()
        {
            return firstVisibleItem;
        }

        /// <summary>
        /// Changes item <see cref="CheckState"/> to the next value.
        /// </summary>
        /// <param name="itemIndex">Index of the item.</param>
        public virtual bool ToggleItemCheckState(int itemIndex)
        {
            var item = SafeItem(itemIndex);
            if (item is null)
                return false;
            var checkState = item.GetCheckState(this);
            var allowThreeState = item.GetAllowThreeState(this);
            var allowAllStatesForUser = GetItemCheckBoxAllowAllStatesForUser(item);

            allowThreeState = allowThreeState && allowAllStatesForUser;

            if (allowThreeState)
            {
                switch (checkState)
                {
                    case CheckState.Unchecked:
                        item.CheckState = CheckState.Checked;
                        break;
                    case CheckState.Checked:
                        item.CheckState = CheckState.Indeterminate;
                        break;
                    case CheckState.Indeterminate:
                        item.CheckState = CheckState.Unchecked;
                        break;
                }
            }
            else
            {
                if (checkState == CheckState.Checked)
                    item.CheckState = CheckState.Unchecked;
                else
                    item.CheckState = CheckState.Checked;
            }

            RaiseCheckedChanged(EventArgs.Empty);
            RefreshRow(itemIndex);
            return true;
        }

        /// <summary>
        /// Determines the index of the last visible item in the control and calculates
        /// the total height and maximal width of visible items.
        /// </summary>
        /// <param name="visibleHeight">Outputs the total height
        /// of all visible items in device-independent units.</param>
        /// <param name="maxWidth">Outputs the maximal width
        /// of all visible items in device-independent units.</param>
        /// <returns>The zero-based index of the last visible item in the control.</returns>
        public virtual int GetVisibleEnd(out Coord maxWidth, out Coord visibleHeight)
        {
            maxWidth = 0;

            if (DisposingOrDisposed || Count == 0)
            {
                visibleHeight = 0;
                return default;
            }

            MeasureItemEventArgs e = new(MeasureCanvas);
            var sWindow = ClientSize.Height;
            Coord totalHeight = 0;
            var firstItem = GetVisibleBegin();

            int unit;

            for (unit = firstItem; unit < Count; ++unit)
            {
                if (totalHeight > sWindow)
                    break;

                e.Index = unit;
                MeasureItemSize(e);

                totalHeight += e.ItemHeight;
                maxWidth = Math.Max(maxWidth, e.ItemWidth);
            }

            visibleHeight = totalHeight;
            return unit;
        }

        /// <summary>
        /// Determines the index of the last visible item in the control.
        /// </summary>
        /// <returns>The zero-based index of the last visible item in the control.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetVisibleEnd()
        {
            return GetVisibleEnd(out _, out _);
        }

        /// <inheritdoc/>
        public override void DefaultPaint(PaintEventArgs e)
        {
            if (DisposingOrDisposed)
                return;

            TransformMatrix matrix = TransformMatrix.CreateTranslation(-scrollOffset, 0);

            var r = e.ClipRectangle;
            r.Width += scrollOffset;
            e.ClipRectangle = r;

            bool fullPaint = true;

            var dc = e.Graphics;

            RectD rectUpdate;

            if (fullPaint)
                rectUpdate = e.ClipRectangle;
            else
                rectUpdate = GetUpdateClientRect();

            dc.FillRectangle(RealBackgroundColor.AsBrush, rectUpdate);

            RectD rectRow = RectD.Empty;
            rectRow.Width = r.Width;

            int lineMax = GetVisibleEnd();

            MeasureItemEventArgs measureItemArgs = new(dc, 0);
            DrawItemEventArgs drawItemArgs = new(dc);

            dc.PushTransform(matrix);

            for (int line = GetVisibleBegin(); line < lineMax; line++)
            {
                measureItemArgs.Index = line;
                MeasureItemSize(measureItemArgs);

                var hRow = measureItemArgs.ItemHeight;

                rectRow.Height = hRow;

                if (fullPaint || rectRow.IntersectsWith(rectUpdate))
                {
                    var isCurrentItem = IsCurrent(line);
                    var isSelectedItem = IsSelected(line);

                    if (drawMode != DrawMode.Normal)
                    {
                        var item = SafeItem(line);
                        drawItemArgs.Bounds = rectRow;
                        drawItemArgs.Index = line;
                        drawItemArgs.Font = ListControlItem.GetFont(item, this);

                        DrawItemState state = 0;
                        if (isCurrentItem)
                            state |= DrawItemState.Focus;
                        if (isSelectedItem)
                            state |= DrawItemState.Selected;

                        drawItemArgs.State = state;

                        if (isSelectedItem)
                        {
                            drawItemArgs.BackColor
                                = ListControlItem.GetSelectedItemBackColor(item, this)
                                ?? RealBackgroundColor;
                            drawItemArgs.ForeColor
                                = ListControlItem.GetSelectedTextColor(item, this)
                                ?? RealForegroundColor;
                        }
                        else
                        {
                            drawItemArgs.BackColor = RealBackgroundColor;
                            drawItemArgs.ForeColor
                                = ListControlItem.GetItemTextColor(item, this)
                                ?? RealForegroundColor;
                        }

                        RaiseDrawItem(drawItemArgs);
                    }
                    else
                    {
                        if (itemPaintArgs is null)
                            itemPaintArgs = new(this, dc, rectRow, line);
                        else
                        {
                            itemPaintArgs.Graphics = dc;
                            itemPaintArgs.ClipRectangle = rectRow;
                            itemPaintArgs.ItemIndex = line;
                            itemPaintArgs.IsCurrent = IsCurrent(line);
                            itemPaintArgs.IsSelected = IsSelected(line);
                            itemPaintArgs.LabelMetrics = new();
                            itemPaintArgs.Visible = true;
                        }

                        DrawItemBackground(itemPaintArgs);
                        DrawItemForeground(itemPaintArgs);
                    }
                }
                else
                {
                    if (!fullPaint)
                    {
                        if (rectRow.Top > rectUpdate.Bottom)
                            break;
                    }
                }

                rectRow.Top += hRow;
            }

            dc.Pop();
        }

        /// <summary>
        /// Scrolls control to the first row.
        /// </summary>
        /// <returns></returns>
        public virtual bool ScrollToFirstRow()
        {
            return ScrollToRow(0);
        }

        /// <inheritdoc/>
        public override int EndUpdate()
        {
            var result = base.EndUpdate();
            if (result == 0)
                CountChanged();
            return result;
        }

        /// <summary>
        /// Scrolls control to the last row.
        /// </summary>
        /// <returns></returns>
        public virtual bool ScrollToLastRow()
        {
            if (Count == 0)
                return false;
            var visibleRows = VisibleCount;
            if (visibleRows <= 0)
                return false;

            return EnsureVisible(Count - 1);
        }

        /// <inheritdoc/>
        public override bool EnsureVisible(int index)
        {
            if (index < 0 || index >= Count)
                return false;
            if (IsItemVisible(index))
                return true;
            var newRow = FindFirstVisibleFromLast(index, true);
            var result = ScrollToRow(newRow);
            return result;
        }

        /// <summary>
        /// Returns the zero-based index of the item, if specified coordinates are over checkbox;
        /// otherwise returns <c>null</c>.
        /// </summary>
        /// <param name="position">A <see cref="PointD"/> object containing
        /// the coordinates used to obtain the item
        /// index.</param>
        public virtual int? HitTestCheckBox(PointD position)
        {
            var itemIndex = HitTest(position);
            if (itemIndex is null)
                return null;
            var rect = GetItemRect(itemIndex);
            if (rect is null)
                return null;
            var info = GetCheckBoxInfo(itemIndex.Value, rect.Value);
            if (info is null || !info.IsCheckBoxVisible)
                return null;
            var checkRect = info.CheckRect;
            checkRect.Inflate(2);
            var isOverCheck = checkRect.Contains(position);
            return isOverCheck ? itemIndex : null;
        }

        /// <summary>
        /// Raises <see cref="MeasureItem"/> event and <see cref="OnMeasureItem"/> method.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public void RaiseMeasureItem(MeasureItemEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnMeasureItem(e);
            MeasureItem?.Invoke(this, e);
        }

        /// <summary>
        /// Raises <see cref="DrawItem"/> event and <see cref="OnDrawItem"/> method.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public void RaiseDrawItem(DrawItemEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnDrawItem(e);
            DrawItem?.Invoke(this, e);
        }

        void IListControl.Add(ListControlItem item)
        {
            Items.Add(item);
        }

        /// <summary>
        /// Toggles checked state of the item at the specified coordinates.
        /// </summary>
        /// <param name="location">A <see cref="PointD"/> object containing
        /// the coordinates used to obtain the item
        /// index.</param>
        public virtual bool ToggleItemCheckState(PointD location)
        {
            var itemIndex = HitTestCheckBox(location);
            if (itemIndex is null)
                return false;
            return ToggleItemCheckState(itemIndex.Value);
        }

        /// <summary>
        /// Measures item size. If <see cref="VirtualListControl.ItemPainter"/> is assigned,
        /// uses <see cref="IListBoxItemPainter.GetSize"/>, otherwise calls
        /// <see cref="ListControlItem.DefaultMeasureItemSize"/>. Additionally calls
        /// <see cref="MeasureItem"/> event and <see cref="OnMeasureItem"/> method if
        /// drawing mode is not <see cref="DrawMode.Normal"/>.
        /// </summary>
        public virtual void MeasureItemSize(MeasureItemEventArgs e)
        {
            var itemSize = Internal(e.Index);
            e.ItemWidth = itemSize.Width;
            e.ItemHeight = itemSize.Height;

            if (drawMode != DrawMode.Normal)
            {
                RaiseMeasureItem(e);
            }

            SizeD Internal(int itemIndex)
            {
                if (ItemPainter is null)
                    return ListControlItem.DefaultMeasureItemSize(this, e.Graphics, itemIndex);
                var result = ItemPainter.GetSize(this, itemIndex);
                if (result == SizeD.MinusOne)
                    return ListControlItem.DefaultMeasureItemSize(this, e.Graphics, itemIndex);
                return result;
            }
        }

        /// <summary>
        /// Calculates the position information for the scrollbars based on the number of visible items
        /// and their total height and maximal width.
        /// </summary>
        public virtual void CalcScrollBarInfo(out ScrollBarInfo horzScrollbar, out ScrollBarInfo vertScrollbar)
        {
            HiddenOrVisible vertVisibility = HiddenOrVisible.Auto;
            HiddenOrVisible horzVisibility = HiddenOrVisible.Auto;

            if (HasHorizontalScrollBarSettings)
                horzVisibility = HorizontalScrollBarSettings.SuggestedVisibility;
            if (HasVerticalScrollBarSettings)
                vertVisibility = VerticalScrollBarSettings.SuggestedVisibility;

            if (Count == 0)
            {
                horzScrollbar = new(horzVisibility);
                vertScrollbar = new(vertVisibility);
                return;
            }

            Coord sWindow = ClientSize.Height;

            var visibleBegin = GetVisibleBegin();
            var visibleEnd = GetVisibleEnd(out Coord maxWidth, out var visibleHeight);
            var visibleItemsCount = visibleEnd - visibleBegin;

            int pageHeightInUnits = visibleItemsCount;
            if (visibleHeight > sWindow)
            {
                // last unit is only partially visible, we still need the scrollbar and
                // so we have to "fix" pageSize because if it is equal to m_unitMax
                // the scrollbar is not shown at all under MSW
                --pageHeightInUnits;
            }

            horzScrollbar = new((int)scrollOffset, (int)maxWidth, (int)ClientSize.Width);
            vertScrollbar = new(firstVisibleItem, Count, pageHeightInUnits);
            horzScrollbar.Visibility = horzVisibility;
            vertScrollbar.Visibility = vertVisibility;
        }

        /// <summary>
        /// Sets horizontal scroll offset.
        /// </summary>
        /// <param name="value">Value of the horizontal scroll offset in device-independent units.</param>
        public virtual void SetHorizontalOffset(Coord value)
        {
            var newOffset = Math.Max(value, 0);
            if (newOffset != scrollOffset)
            {
                scrollOffset = newOffset;
                UpdateScrollBars(true);
            }
        }

        /// <summary>
        /// Increments horizontal scroll offset.
        /// </summary>
        /// <param name="delta">Increment value in device-independent units.</param>
        public virtual void IncHorizontalOffset(Coord delta)
        {
            SetHorizontalOffset(scrollOffset + delta);
        }

        /// <summary>
        /// Retrieves the width of a single character using the font returned by
        /// <see cref="VirtualListControl.GetItemFont"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="Coord"/> representing the width of the character.
        /// </returns>
        public virtual Coord GetCharWidth()
        {
            var result = MeasureCanvas.GetTextExtent("W", GetItemFont()).Width;
            if (result <= 0)
                result = 16;
            return result;
        }

        /// <summary>
        /// Increments horizontal scroll offset on the value specified in characters.
        /// </summary>
        /// <param name="chars">Increment value in chars.</param>
        public virtual void IncHorizontalOffsetChars(int chars = 1)
        {
            IncHorizontalOffset(chars * GetCharWidth());
        }

        /// <summary>
        /// Sets horizontal scroll offset to the value specified in characters.
        /// </summary>
        /// <param name="offsetInChars">New horizontal scroll offset value in chars.</param>
        public virtual void SetHorizontalOffsetChars(int offsetInChars)
        {
            SetHorizontalOffset(offsetInChars * GetCharWidth());
        }

        /// <summary>
        /// Finds the item with <see cref="ListControlItem.Value"/> property which is
        /// equal to the specified value.
        /// </summary>
        /// <param name="value">Value to search for.</param>
        /// <returns></returns>
        public virtual ListControlItem? FindItemWithValue(object? value)
        {
            if (value is null)
                return null;

            for (int i = 0; i < Items.Count; i++)
            {
                var item = Items[i];

                if (value.Equals(item.Value))
                    return item;
            }

            return null;
        }

        /// <inheritdoc/>
        public override void UpdateScrollBars(bool refresh)
        {
            if (DisposingOrDisposed)
                return;

            CalcScrollBarInfo(out var horzScrollbar, out var vertScrollbar);
            VertScrollBarInfo = vertScrollbar;
            HorzScrollBarInfo = horzScrollbar;
            if (refresh)
                Refresh();
        }

        /// <summary>
        /// Simulates the key press event by creating and dispatching a <see cref="KeyEventArgs"/>.
        /// </summary>
        /// <param name="key">The key that was pressed.</param>
        /// <param name="modifiers">Optional modifier keys associated with the key press.</param>
        protected virtual void RunKeyDown(Key key, ModifierKeys modifiers = UI.ModifierKeys.None)
        {
            KeyEventArgs e = new();
            e.Key = key;
            e.ModifierKeys = modifiers;
            OnKeyDown(e);
        }

        /// <summary>
        /// Responds to changes in the item count by updating the scrollbars and invalidating the control.
        /// </summary>
        protected virtual void CountChanged()
        {
            if (DisposingOrDisposed || InUpdates)
                return;
            UpdateScrollBars(true);
        }

        /// <inheritdoc/>
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            if (CheckOnClick && CheckBoxVisible)
            {
                if (ToggleItemCheckState(e.Location))
                {
                    e.Handled = true;
                    return;
                }
            }

            base.OnMouseDoubleClick(e);
        }

        /// <inheritdoc/>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateScrollBars(false);
        }

        /// <inheritdoc/>
        protected override void OnMouseLeftButtonDown(MouseEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            if (CheckOnClick && CheckBoxVisible)
            {
                if (ToggleItemCheckState(e.Location))
                {
                    e.Handled = true;
                    return;
                }
            }

            var itemIndex = HitTest(e.Location);
            e.Handled = true;

            if (itemIndex is not null)
            {
                var modifiers = Keyboard.Modifiers;

                ItemClickFlags flags = 0;

                if (modifiers.HasShift())
                    flags |= ItemClickFlags.Shift;
                if (modifiers.HasControl())
                    flags |= ItemClickFlags.Ctrl;

                DoHandleItemClick(itemIndex.Value, flags);
            }
        }

        /// <summary>
        /// Called when <see cref="DrawItem"/> event is raised.
        /// </summary>
        /// <param name="e">Paint arguments.</param>
        protected virtual void OnDrawItem(DrawItemEventArgs e)
        {
        }

        /// <inheritdoc/>
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);

            if (SelectedIndex is null)
            {
                if (Count > 0)
                    SelectItemsAndScroll(0);
            }
        }

        /// <summary>
        /// Called when the <see cref="MeasureItem" /> event is raised.</summary>
        /// <param name="e">A <see cref="MeasureItemEventArgs" /> that
        /// contains the event data.</param>
        protected virtual void OnMeasureItem(MeasureItemEventArgs e)
        {
        }

        /// <inheritdoc/>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }

        /// <inheritdoc/>
        protected override void ItemsCollectionChanged(
            object? sender,
            NotifyCollectionChangedEventArgs e)
        {
            CountChanged();
        }

        /// <inheritdoc/>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            UpdateScrollBars(false);
        }

        /// <summary>
        /// Gets the index of the first item on the previous page relative to the specified index.
        /// </summary>
        /// <param name="index">The starting index to calculate the previous page from.
        /// If null, the current selected index is used.</param>
        /// <returns>
        /// The zero-based index of the first item on the previous page, or 0 if the list
        /// is empty or the index is null.
        /// </returns>
        protected virtual int? GetIndexOnPreviousPage(int? index = null)
        {
            if (Count == 0)
                return null;

            var selected = index ?? SelectedIndex;

            if (selected is null)
            {
                return 0;
            }

            var numVisible = VisibleCount - 1;

            if (numVisible <= 0)
                return null;

            return Math.Max(selected.Value - numVisible, 0);
        }

        /// <summary>
        /// Gets the index of the first item on the next page relative to the specified index.
        /// </summary>
        /// <param name="index">The starting index to calculate the next page from.
        /// If null, the current selected index is used.</param>
        /// <returns>
        /// The zero-based index of the first item on the next page, or null if the list is empty.
        /// </returns>
        protected virtual int? GetIndexOnNextPage(int? index = null)
        {
            if (Count == 0)
                return null;

            var selected = index ?? SelectedIndex;

            if (selected is null)
            {
                return 0;
            }

            var numVisible = VisibleCount - 1;

            if (numVisible <= 0)
                return null;

            return Math.Min(selected.Value + numVisible, Count - 1);
        }

        /// <summary>
        /// Handles the logic for item selection when an item is clicked.
        /// </summary>
        /// <param name="item">The index of the item that was clicked.</param>
        /// <param name="flags">Flags indicating the type of click (e.g., Shift, Ctrl, Keyboard).</param>
        /// <remarks>
        /// This method processes item selection based on the current selection mode
        /// (single or multiple). It supports Shift-click for range selection and
        /// Ctrl-click for toggling selection of individual items in multiple-selection mode.
        /// </remarks>
        protected virtual void DoHandleItemClick(int item, ItemClickFlags flags)
        {
            DoInsideSuspendedSelectionEvents(Internal);

            void Internal()
            {
                var current = SelectedIndex;
                var setSelected = true;

                if (IsSelectionModeMultiple)
                {
                    bool select = true;

                    if (flags.HasFlag(ItemClickFlags.Shift))
                    {
                        if (current is not null)
                        {
                            if (AnchorIndex is null)
                                AnchorIndex = current;

                            select = false;

                            ClearSelected();

                            if (AnchorIndex is null)
                                select = true;
                            else
                                SelectRange(AnchorIndex.Value, item);
                        }
                    }
                    else
                    {
                        AnchorIndex = item;

                        if (flags.HasFlag(ItemClickFlags.Ctrl))
                        {
                            select = false;

                            if (!flags.HasFlag(ItemClickFlags.Keyboard))
                            {
                                SafeItem(item)?.ToggleSelected(this);
                                setSelected = false;
                            }
                        }
                    }

                    if (select)
                    {
                        SetSelectedIndex(item, clearSelection: true);
                    }
                }

                var savedAnchor = AnchorIndex;
                SetSelectedIndex(item, clearSelection: false, setSelected: setSelected);
                AnchorIndex = savedAnchor;
            }
        }

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (DisposingOrDisposed)
                return;

            if (Count == 0)
            {
                base.OnKeyDown(e);
                return;
            }

            ItemClickFlags flags = ItemClickFlags.Keyboard;

            var selected = SelectedIndex;

#pragma warning disable
            int current = 0;
#pragma warning restore

            switch (e.Key)
            {
                case Key.A:
                    if (e.Control)
                    {
                        SelectAll();
                        e.Suppressed();
                    }

                    return;

                case Key.Home:
                    if (e.Control)
                    {
                        SetHorizontalOffset(0);
                        e.Suppressed();
                        return;
                    }
                    else
                    {
                        current = 0;
                    }

                    break;

                case Key.End:
                    current = Count - 1;
                    break;

                case Key.Down:
                    if (selected is null)
                    {
                        SelectedIndex = 0;
                        e.Suppressed();
                        return;
                    }
                    else
                    if (selected >= Count - 1)
                    {
                        e.Suppressed();
                        return;
                    }

                    current = selected.Value + 1;
                    break;

                case Key.Up:
                    if (selected is null)
                    {
                        SelectedIndex = Count - 1;
                        e.Suppressed();
                        return;
                    }
                    else
                    if (selected <= 0)
                    {
                        e.Suppressed();
                        return;
                    }

                    current = selected.Value - 1;
                    break;

                case Key.PageDown:
                    current = GetIndexOnNextPage() ?? 0;
                    break;

                case Key.PageUp:
                    current = GetIndexOnPreviousPage() ?? 0;
                    break;

                case Key.Left:
                    if (e.Control)
                        DoActionScrollPageLeft();
                    else
                        DoActionScrollCharLeft();
                    e.Suppressed();
                    return;
                case Key.Right:
                    if (e.Control)
                        DoActionScrollPageRight();
                    else
                        DoActionScrollCharRight();
                    e.Suppressed();
                    return;

                case Key.Space:
                    // hack: pressing space should work like a mouse click rather than
                    // like a keyboard arrow press, so trick DoHandleItemClick() in
                    // thinking we were clicked.
                    flags &= ~ItemClickFlags.Keyboard;
                    current = selected ?? 0;
                    break;

                default:
                    return;
            }

            e.Suppressed();

            if (e.Shift)
                flags |= ItemClickFlags.Shift;
            if (e.Control)
                flags |= ItemClickFlags.Ctrl;

            DoHandleItemClick(current, flags);
        }

        /// <inheritdoc/>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            var delta = e.Delta;
            if (delta > 0)
            {
                delta = 1;
            }
            else
            if (delta < 0)
            {
                delta = -1;
            }
            else
                return;

            if (Keyboard.IsShiftPressed)
            {
                if (delta < 0)
                    DoActionScrollCharRight();
                else
                    DoActionScrollCharLeft();
            }
            else
            {
                if (delta < 0)
                    DoActionScrollLineDown();
                else
                    DoActionScrollLineUp();
            }
        }

        /// <inheritdoc/>
        protected override void OnScroll(ScrollEventArgs e)
        {
            if (DisposingOrDisposed)
                return;

            base.OnScroll(e);

            if (e.IsVertical)
            {
                switch (e.Type)
                {
                    case ScrollEventType.SmallDecrement:
                        DoActionScrollLineUp();
                        break;
                    case ScrollEventType.SmallIncrement:
                        DoActionScrollLineDown();
                        break;
                    case ScrollEventType.LargeDecrement:
                        DoActionScrollPageUp();
                        break;
                    case ScrollEventType.LargeIncrement:
                        DoActionScrollPageDown();
                        break;
                    case ScrollEventType.ThumbTrack:
                        ScrollToRow(e.NewValue);
                        break;
                    case ScrollEventType.First:
                        DoActionScrollToFirstLine();
                        break;
                    case ScrollEventType.Last:
                        DoActionScrollToLastLine();
                        break;
                    case ScrollEventType.ThumbPosition:
                    case ScrollEventType.EndScroll:
                    default:
                        break;
                }
            }
            else
            {
                switch (e.Type)
                {
                    case ScrollEventType.SmallDecrement:
                        DoActionScrollCharLeft();
                        break;
                    case ScrollEventType.SmallIncrement:
                        DoActionScrollCharRight();
                        break;
                    case ScrollEventType.LargeDecrement:
                        DoActionScrollPageLeft();
                        break;
                    case ScrollEventType.LargeIncrement:
                        DoActionScrollPageRight();
                        break;
                    case ScrollEventType.ThumbTrack:
                        SetHorizontalOffset(e.NewValue);
                        break;
                    case ScrollEventType.First:
                        SetHorizontalOffset(0);
                        break;
                    case ScrollEventType.Last:
                        break;
                    case ScrollEventType.ThumbPosition:
                    case ScrollEventType.EndScroll:
                    default:
                        break;
                }
            }
        }
    }
}