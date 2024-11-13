using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

using Alternet.Drawing;
using Alternet.UI.Extensions;

namespace Alternet.UI
{
    /// <summary>
    /// Advanced list box control with ability to customize item painting. Works fine with
    /// large number of the items. You can add <see cref="ListControlItem"/> items to this control.
    /// </summary>
    public class VirtualListBox : VirtualListControl<ListControlItem>, IListControl
    {
        private readonly TransformMatrix matrix = new();

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

        /// <summary>
        /// Gets or sets whether horizontal scrollbar is visible in the control.
        /// </summary>
        public virtual bool HorizontalScrollbar
        {
            get => Handler.HScrollBarVisible;

            set
            {
                if (HorizontalScrollbar == value || !App.IsWindowsOS)
                    return;
                Handler.HScrollBarVisible = value;
                Refresh();
            }
        }

        /// <inheritdoc/>
        public override bool UserPaint
        {
            get => base.UserPaint;
            set => base.UserPaint = true;
        }

        /// <inheritdoc/>
        public override int Count
        {
            get
            {
                var result = Handler.ItemsCount;
                return result;
            }

            set
            {
                Handler.ItemsCount = value;
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
        }

        /// <inheritdoc/>
        public override int? SelectedIndex
        {
            get
            {
                if (SelectionMode == ListBoxSelectionMode.Single)
                {
                    var result = Handler.GetSelection();
                    if (result < 0)
                        return null;
                    return result;
                }
                else
                {
                    var result = Handler.GetFirstSelected();
                    if (result < 0)
                        return null;
                    return result;
                }
            }

            set
            {
                if (SelectedIndex == value)
                    return;
                if (SelectionMode == ListBoxSelectionMode.Single)
                {
                    Handler.SetSelection(value ?? -1);
                }
                else
                {
                    Handler.ClearSelected();
                    if (value is not null && value >= 0)
                        Handler.SetSelected(value.Value, true);
                }
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

        /// <summary>
        /// Gets or sets whether vertical scrollbar is visible in the control.
        /// </summary>
        internal virtual bool VScrollBarVisible
        {
            get => Handler.VScrollBarVisible;
            set
            {
                if (VScrollBarVisible == value)
                    return;
                Handler.VScrollBarVisible = value;
                Refresh();
            }
        }

        /// <summary>
        /// Gets whether item with the specified index is selected.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <returns></returns>
        public override bool IsSelected(int index)
        {
            return Handler.IsSelected(index);
        }

        /// <summary>
        /// Gets whether item with the specified index is visible.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <returns></returns>
        public virtual bool IsItemVisible(int index)
        {
            return Handler.IsVisible(index);
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
            return Handler.ScrollRows(rows);
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
            return Handler.ScrollRowPages(pages);
        }

        /// <inheritdoc/>
        public override void RemoveAll()
        {
            if (Items.Count == 0)
                return;

            DoInsideUpdate(() =>
            {
                Handler.DetachItems(Items);
                RecreateItems();
                Handler.AttachItems(Items);
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
                    Handler.DetachItems(Items);
                    RecreateItems(value);
                    Handler.AttachItems(Items);
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
            Handler.RefreshRow(row);
        }

        /// <summary>
        /// Triggers a refresh for the area between the specified range of rows given (inclusively).
        /// </summary>
        /// <param name="from">First item index.</param>
        /// <param name="to">Last item index.</param>
        public virtual void RefreshRows(int from, int to)
        {
            Handler.RefreshRows(from, to);
        }

        /// <summary>
        /// Gets whether item with the specified index is current.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <returns></returns>
        public override bool IsCurrent(int index)
        {
            return Handler.IsCurrent(index);
        }

        /// <summary>
        /// Gets index of the first visible item.
        /// </summary>
        /// <returns></returns>
        public virtual int GetVisibleBegin()
        {
            return Handler.GetVisibleBegin();
        }

        /// <summary>
        /// Changes item <see cref="CheckState"/> to the next value.
        /// </summary>
        /// <param name="itemIndex">Index of the item.</param>
        public virtual void ToggleItemCheckState(int itemIndex)
        {
            var item = SafeItem(itemIndex);
            if (item is null)
                return;
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
        }

        /// <summary>
        /// Gets index of the last visible item.
        /// </summary>
        /// <returns></returns>
        public virtual int GetVisibleEnd()
        {
            return Handler.GetVisibleEnd();
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
            OnMeasureItem(e);
            MeasureItem?.Invoke(this, e);
        }

        /// <summary>
        /// Raises <see cref="DrawItem"/> event and <see cref="OnDrawItem"/> method.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public void RaiseDrawItem(DrawItemEventArgs e)
        {
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
            if (index is null)
                return null;
            var result = Handler.GetItemRect(index.Value);
            return result;
        }

        void IListControl.Add(ListControlItem item)
        {
            Items.Add(item);
        }

        internal void ToggleItemCheckState(PointD location)
        {
            var itemIndex = HitTestCheckBox(location);
            if (itemIndex is null)
                return;
            ToggleItemCheckState(itemIndex.Value);
        }

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return ControlFactory.Handler.CreateVListBoxHandler(this);
        }

        /// <inheritdoc/>
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            if(CheckOnClick)
                ToggleItemCheckState(e.Location);
            base.OnMouseDoubleClick(e);
        }

        /// <inheritdoc/>
        protected override void OnMouseLeftButtonDown(MouseEventArgs e)
        {
            if (CheckOnClick)
                ToggleItemCheckState(e.Location);
            base.OnMouseLeftButtonDown(e);
        }

        /// <summary>
        /// Called when <see cref="DrawItem"/> event is raised.
        /// </summary>
        /// <param name="e">Paint arguments.</param>
        protected virtual void OnDrawItem(DrawItemEventArgs e)
        {
        }

        /// <inheritdoc/>
        protected override void OnPaint(PaintEventArgs e)
        {
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
                var itemSize = MeasureItemSize(line);

                if (drawMode != DrawMode.Normal)
                {
                    measureItemArgs.Index = line;
                    measureItemArgs.ItemWidth = itemSize.Width;
                    measureItemArgs.ItemHeight = itemSize.Height;
                    RaiseMeasureItem(measureItemArgs);
                }

                var hRow = itemSize.Height;

                rectRow.Height = hRow;

                if (rectRow.IntersectsWith(rectUpdate))
                {
                    var isCurrentItem = IsCurrent(line);
                    var isSelectedItem = IsSelected(line);

                    if(drawMode != DrawMode.Normal)
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
                                = ListControlItem.GetSelectedTextColor(item, this) ?? RealForegroundColor;
                        }
                        else
                        {
                            drawItemArgs.BackColor = RealBackgroundColor;
                            drawItemArgs.ForeColor
                                = ListControlItem.GetItemTextColor(item, this) ?? RealForegroundColor;
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

        /// <inheritdoc/>
        protected override void OnScroll(ScrollEventArgs e)
        {
            if (!App.IsWindowsOS)
                return;
            Coord CharWidth() => MeasureCanvas.GetTextExtent("W", GetItemFont()).Width;

            void IncOffset(Coord delta)
            {
                var newOffset = Math.Max(scrollOffset + delta, 0);
                if (newOffset != scrollOffset)
                {
                    scrollOffset = newOffset;
                    Refresh();
                }
            }

            base.OnScroll(e);

            switch (e.Type)
            {
                case ScrollEventType.SmallDecrement:
                    if (scrollOffset == 0)
                        return;
                    IncOffset(-CharWidth());
                    break;
                case ScrollEventType.SmallIncrement:
                    IncOffset(CharWidth());
                    break;
                case ScrollEventType.LargeDecrement:
                    break;
                case ScrollEventType.LargeIncrement:
                    break;
                case ScrollEventType.ThumbPosition:
                    break;
                case ScrollEventType.ThumbTrack:
                    break;
                case ScrollEventType.First:
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
        /// <param name="e">A <see cref="MeasureItemEventArgs" /> that contains the event data.</param>
        protected virtual void OnMeasureItem(MeasureItemEventArgs e)
        {
        }

        /// <inheritdoc/>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }

        /// <inheritdoc/>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
        }
    }
}