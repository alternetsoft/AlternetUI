using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;

using Alternet.Base.Collections;
using Alternet.Drawing;
using Alternet.UI.Extensions;

namespace Alternet.UI
{
    /// <summary>
    /// Advanced list box control with ability to customize item painting. Works fine with
    /// large number of the items. You can add <see cref="ListControlItem"/> items to this control.
    /// </summary>
    public class VirtualListBox : VirtualListControl, IListControl
    {
        private TransformMatrix matrix = new();

        private Coord scrollOffset;
        private ListBoxItemPaintEventArgs? itemPaintArgs;
        private Coord horizontalExtent;
        private DrawMode drawMode = DrawMode.Normal;

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
            UserPaint = true;
            SuggestedSize = 200;
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
        /// Enumerates flast for the item click method.
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
        [Browsable(true)]
        public override bool HasBorder
        {
            get => base.HasBorder;
            set => base.HasBorder = value;
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
            return index >= GetVisibleBegin() && index < (GetVisibleEnd() - 1);
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
        /// <param name="rows">It will become the first visible row in the control.</param>
        /// <returns>True if we scrolled the control, False if nothing was done.</returns>
        public virtual bool ScrollToRow(int rows)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.ScrollToRow(rows);
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
                    Invalidate();
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
                    Handler.ItemsCount = Items.Count;
                    Invalidate();
                });

                return true;
            }
        }

        /// <summary>
        /// Triggers a refresh for just the given row's area of the control if it is visible.
        /// </summary>
        /// <param name="row">Item index.</param>
        public virtual void RefreshRow(int row)
        {
            if (DisposingOrDisposed)
                return;
            Handler.RefreshRow(row);
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
        /// Triggers a refresh for the area between the specified range of rows given (inclusively).
        /// </summary>
        /// <param name="from">First item index.</param>
        /// <param name="to">Last item index.</param>
        public virtual void RefreshRows(int from, int to)
        {
            if (DisposingOrDisposed)
                return;
            Handler.RefreshRows(from, to);
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
            if (DisposingOrDisposed)
                return default;
            return Handler.GetVisibleBegin();
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
        /// Gets index of the last visible item.
        /// </summary>
        /// <returns></returns>
        public virtual int GetVisibleEnd()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.GetVisibleEnd();
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
            if (info is null)
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

        /// <summary>
        /// Returns the rectangle occupied by this item in physical coordinates (dips).
        /// If the item is not currently visible, returns an empty rectangle.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual RectD? GetItemRect(int? index)
        {
            if (DisposingOrDisposed)
                return default;
            if (index is null)
                return null;
            var result = Handler.GetItemRect(index.Value);
            return result;
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

        internal void CountChanged()
        {
            if (DisposingOrDisposed || InUpdates)
                return;
            var newCount = Items.Count;
            Handler.ItemsCount = newCount;
            Invalidate();
        }

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return ControlFactory.Handler.CreateVListBoxHandler(this);
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

            int current = 0;

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
                        DoInsideUpdate(() => scrollOffset = 0);
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
                        IncHorizontalOffsetChars(-4);
                    else
                        IncHorizontalOffsetChars(-1);
                    e.Suppressed();
                    return;
                case Key.Right:
                    if (e.Control)
                        IncHorizontalOffsetChars(4);
                    else
                        IncHorizontalOffsetChars(1);
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

            if(SelectedIndex is null)
            {
                if (Count > 0)
                    SelectItemsAndScroll(0);
            }
        }

        /// <inheritdoc/>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            var clientSize = ClientSize;

            var dc = e.Graphics;

            var rectUpdate = GetUpdateClientRect();

            dc.FillRectangle(RealBackgroundColor.AsBrush, rectUpdate);

            RectD rectRow = RectD.Empty;
            rectRow.Width = clientSize.Width;

            int lineMax = Handler.GetVisibleEnd();

            MeasureItemEventArgs measureItemArgs = new(dc, 0);
            DrawItemEventArgs drawItemArgs = new(dc);

            for (int line = Handler.GetVisibleBegin(); line < lineMax; line++)
            {
                measureItemArgs.Index = line;
                MeasureItemSize(measureItemArgs);

                var hRow = measureItemArgs.ItemHeight;

                rectRow.Height = hRow;

                if (rectRow.IntersectsWith(rectUpdate))
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

                        matrix.Reset();
                        dc.Transform = matrix;
                        DrawItemBackground(itemPaintArgs);

                        matrix.Translate(-scrollOffset, 0);
                        dc.Transform = matrix;
                        DrawItemForeground(itemPaintArgs);
                    }
                }
                else
                {
                    if (rectRow.Top > rectUpdate.Bottom)
                        break;
                }

                rectRow.Top += hRow;
            }
        }

        /// <summary>
        /// Increments horizontal scroll offset.
        /// </summary>
        /// <param name="delta">Increment value in device-independent units.</param>
        protected void IncHorizontalOffset(Coord delta)
        {
            var newOffset = Math.Max(scrollOffset + delta, 0);
            if (newOffset != scrollOffset)
            {
                scrollOffset = newOffset;
                Refresh();
            }
        }

        /// <summary>
        /// Increments horizontal scroll offset.
        /// </summary>
        /// <param name="chars">Increment value in chars.</param>
        protected void IncHorizontalOffsetChars(int chars = 1)
        {
            Coord CharWidth() => MeasureCanvas.GetTextExtent("W", GetItemFont()).Width;
            IncHorizontalOffset(chars * CharWidth());
        }

        /// <inheritdoc/>
        protected override void OnScroll(ScrollEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            if (!App.IsWindowsOS)
                return;

            base.OnScroll(e);

            switch (e.Type)
            {
                case ScrollEventType.SmallDecrement:
                    IncHorizontalOffsetChars(-1);
                    break;
                case ScrollEventType.SmallIncrement:
                    IncHorizontalOffsetChars(1);
                    break;
                case ScrollEventType.LargeDecrement:
                    IncHorizontalOffsetChars(-4);
                    break;
                case ScrollEventType.LargeIncrement:
                    IncHorizontalOffsetChars(4);
                    break;
                case ScrollEventType.ThumbPosition:
                    break;
                case ScrollEventType.ThumbTrack:
                    break;
                case ScrollEventType.First:
                    scrollOffset = 0;
                    Invalidate();
                    break;
                case ScrollEventType.Last:
                    break;
                case ScrollEventType.EndScroll:
                    break;
                default:
                    break;
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
        }

        private int? GetIndexOnNextPage()
        {
            if (Count == 0)
                return null;

            var selected = SelectedIndex;

            if (selected is null)
            {
                return 0;
            }

            var numVisible = VisibleCount - 1;

            if (numVisible <= 0)
                return null;

            return Math.Min(selected.Value + numVisible, Count - 1);
        }

        private void DoHandleItemClick(int item, ItemClickFlags flags)
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

        private int? GetIndexOnPreviousPage()
        {
            if (Count == 0)
                return null;

            var selected = SelectedIndex;

            if (selected is null)
            {
                return 0;
            }

            var numVisible = VisibleCount - 1;

            if (numVisible <= 0)
                return null;

            return Math.Max(selected.Value - numVisible, 0);
        }
    }
}