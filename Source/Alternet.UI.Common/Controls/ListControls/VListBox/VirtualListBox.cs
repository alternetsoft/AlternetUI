using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
        /// Gets or sets whether horizontal scrollbar is visible in the control.
        /// </summary>
        public virtual bool HScrollBarVisible
        {
            get => Handler.HScrollBarVisible;

            set
            {
                if (HScrollBarVisible == value || !App.IsWindowsOS)
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
            for (int line = Handler.GetVisibleBegin(); line < lineMax; line++)
            {
                var hRow = MeasureItemSize(line).Height;

                rectRow.Height = hRow;

                if (rectRow.IntersectsWith(rectUpdate))
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
                    DrawItem(itemPaintArgs);
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