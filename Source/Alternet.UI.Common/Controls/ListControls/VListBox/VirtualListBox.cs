using System;
using System.Collections;
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
    /// The <see cref="VirtualListControl.SelectedItems"/> and <see cref="VirtualListControl.SelectedIndices"/>
    /// properties provide access to the selected items and their indices.
    /// </remarks>
    public partial class VirtualListBox : VirtualListControl, IListControl, IScrollEventRouter, IListBoxActions
    {
        /// <summary>
        /// Gets or sets default color of the horizontal line that is drawn between items.
        /// </summary>
        public static LightDarkColor DefaultHorzGridLinesColor = new(light: (229, 229, 229), dark: (99, 99, 99));

        /// <summary>
        /// Gets or sets default color of the vertical line that is drawn between columns.
        /// </summary>
        public static LightDarkColor DefaultVertGridLinesColor = new(light: (229, 229, 229), dark: (99, 99, 99));

        /// <summary>
        /// Gets or sets the default provider used to generate tooltips for items.
        /// </summary>
        /// <remarks>Changing this property affects how item tooltips are displayed.
        /// The default value is set to use the factory provider.</remarks>
        public static ItemToolTipProviderType DefaultItemToolTipProvider { get; set; }
            = ItemToolTipProviderType.Factory;

        private static SetItemsKind defaultSetItemsKind = SetItemsKind.ChangeField;

        private readonly List<ListControlItem> itemsLastPainted = new();

        private bool isPartialRowVisible = true;
        private int scrollOffsetX;
        private ListBoxItemPaintEventArgs? itemPaintArgs;
        private Coord horizontalExtent;
        private DrawMode drawMode = DrawMode.Normal;
        private int firstVisibleItem;
        private string? emptyText;
        private ObjectUniqueId? itemToolTipId;
        private bool useScrollActivity;
        private LightDarkColor? horzGridLinesColor;
        private LightDarkColor? vertGridLinesColor;
        private ListViewGridLinesDisplayMode gridLinesDisplayMode = ListViewGridLinesDisplayMode.None;
        private IListSource<ListControlItem> items = new ListSource<ListControlItem>();
        private bool immutableItems;
        private bool isHoverSelectionEnabled;

        static VirtualListBox()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualListBox"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public VirtualListBox(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualListBox"/> class.
        /// </summary>
        public VirtualListBox()
        {
            MinimumSize = (120, 96);

            UserPaint = true;
            HasBorder = true;

            UseControlColors(true);

            if (UseInternalScrollBars)
            {
                Interior?.Required();
            }

            if (App.IsMaui)
            {
                if (SystemSettings.AppearanceIsDark)
                    this.SetColorThemeToDark();
            }

            UseScrollActivity = DefaultUseScrollActivity;
        }

        /// <summary>
        /// Occurs when the horizontal scroll offset is changed, for example, when the user scrolls horizontally
        /// or when the offset is changed programmatically.
        /// </summary>
        public event EventHandler? ScrollOffsetXChanged;

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
        /// Specifies the way how item tooltip is provided for items that don't fit
        /// in the control's view or when mouse is over the item for some time.
        /// </summary>
        public enum ItemToolTipProviderType
        {
            /// <summary>
            /// Indicates that no item tooltip is displayed.
            /// </summary>
            None = 0,

            /// <summary>
            /// Indicates that the item tooltip is displayed as an overlay.
            /// </summary>
            Overlay = 1,

            /// <summary>
            /// Indicates that the item tooltip is displayed via tooltip factory in the separate window.
            /// </summary>
            Factory = 2,
        }

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
        /// Gets or sets the default border color of the full item tooltip.
        /// </summary>
        public static LightDarkColor? DefaultFullItemToolTipBorderColor { get; set; }

        /// <summary>
        /// Gets or sets the default foreground color of the full item tooltip text.
        /// </summary>
        public static LightDarkColor DefaultFullItemToolTipForeColor { get; set; }
            = new(light: Color.Black, dark: (224, 224, 224));

        /// <summary>
        /// Gets or sets the default background color of the full item tooltip.
        /// </summary>
        public static LightDarkColor DefaultFullItemToolTipBackColor { get; set; }
            = new(light: (240, 240, 240), dark: (51, 51, 51));

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
        /// Gets or sets a value indicating whether scroll activity is used by default in <see cref="VirtualListBox"/>.
        /// </summary>
        /// <remarks>When set to <see langword="true"/>, scroll activity will be enabled by default unless
        /// explicitly overridden. Default is <see langword="true"/>.</remarks>
        public static bool DefaultUseScrollActivity { get; set; } = true;

        /// <summary>
        /// Gets the horizontal scroll offset.
        /// </summary>
        /// <value>
        /// A <see cref="Coord"/> representing the current horizontal scroll position.
        /// </value>
        [Browsable(false)]
        public Coord ScrollOffsetX
        {
            get
            {
                return scrollOffsetX;
            }

            set
            {
                DoActionScrollToHorzPos((int)value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether selection of items
        /// is automatically changed when mouse is hovering over them.
        /// </summary>
        public virtual bool IsHoverSelectionEnabled
        {
            get
            {
                return isHoverSelectionEnabled;
            }

            set
            {
                isHoverSelectionEnabled = value;
            }
        }

        /// <summary>
        /// Gets whether the <see cref="Items"/> property is immutable and cannot be changed.
        /// Use <see cref="SetImmutableItems"/> to mark the items as immutable.
        /// </summary>
        public bool ImmutableItems => immutableItems;

        /// <inheritdoc/>
        public override IListSource<ListControlItem> Items
        {
            get => items;

            set
            {
                if (immutableItems || items == value)
                    return;

                value ??= new ListSource<ListControlItem>();

                if (items is not null)
                {
                    DetachItems(items);
                }

                DoInsideUpdate(() =>
                {
                    items = value;
                    AttachItems(items);
                });
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether scroll activity should be tracked or utilized by the control.
        /// Default value is loaded from <see cref="DefaultUseScrollActivity"/>.
        /// </summary>
        /// <remarks>
        /// This feature is particularly useful on tablets and mobile devices.
        /// When this property is true, users can scroll the items vertically by dragging the interior.
        /// </remarks>
        [Browsable(false)]
        public virtual bool UseScrollActivity
        {
            get => useScrollActivity;

            set
            {
                if (useScrollActivity == value)
                    return;
                useScrollActivity = value;
                if (useScrollActivity)
                    EnsureScrollActivityAttached();
                else
                    EnsureScrollActivityDetached();

                void EnsureScrollActivityAttached()
                {
                    var scrollActivity = new InteriorScrollActivity();
                    scrollActivity.ScrollMethod = InteriorScrollActivity.ScrollMethodKind.RepeatWhilePressed;

                    AddNotification(scrollActivity);

                    scrollActivity.Scroll += (s, e) =>
                    {
                        if (!e.IsVertical)
                            e.Handled = true;
                    };

                    scrollActivity.DeltaScroll += (s, e) =>
                    {
                    };

                    scrollActivity.HitTest = (sender, hitTest) =>
                    {
                        var r = GetPaintRectangle();
                        if (r.Width <= 0 || r.Height <= 0)
                            return -1;
                        if (r.Contains(hitTest))
                            return 0;
                        return -1;
                    };
                }

                void EnsureScrollActivityDetached()
                {
                    RemoveNotificationsOfType<InteriorScrollActivity>();
                }
            }
        }

        /// <summary>
        /// Gets the items that were last painted in the control.
        /// </summary>
        [Browsable(false)]
        public IReadOnlyList<ListControlItem> ItemsLastPainted => itemsLastPainted;

        /// <summary>
        /// Gets or sets the provider used to display tooltips for items.
        /// </summary>
        [Browsable(false)]
        public virtual ItemToolTipProviderType? ItemToolTipProvider { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether partially visible rows are painted.
        /// </summary>
        /// <remarks>This property is affected by the application's platform. If the application
        /// is running on Maui, the property will always return <see langword="false"/> and
        /// cannot be set.</remarks>
        [Browsable(false)]
        public virtual bool IsPartialRowVisible
        {
            get
            {
                return isPartialRowVisible && !App.IsMaui;
            }

            set
            {
                if (isPartialRowVisible == value || App.IsMaui)
                    return;
                isPartialRowVisible = value;
                Invalidate();
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
        public override bool UserPaint
        {
            get => base.UserPaint;
            set => base.UserPaint = true;
        }

        /// <summary>
        /// Gets or sets the text displayed when the collection of items is empty.
        /// </summary>
        /// <remarks>Setting this property triggers a visual update if the collection
        /// of items is empty.</remarks>
        public virtual string? EmptyText
        {
            get => emptyText;

            set
            {
                if (emptyText == value)
                    return;
                emptyText = value;
                if (Count == 0)
                    Invalidate();
            }
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
        /// Gets or sets a value indicating whether an item tooltip is shown
        /// for items that don't fit in the control's view. Default is <c>true</c>.
        /// </summary>
        public virtual bool NeedsFullItemToolTip { get; set; } = true;

        /// <summary>
        /// Gets or sets the foreground color of the full item tooltip text.
        /// Default is <c>null</c>, which means <see cref="DefaultFullItemToolTipForeColor"/>
        /// will be used.
        /// </summary>
        [Browsable(false)]
        public virtual LightDarkColor? FullItemToolTipForeColor { get; set; }

        /// <summary>
        /// Gets or sets the background color of the full item tooltip.
        /// Default is <c>null</c>, which means <see cref="DefaultFullItemToolTipBackColor"/>
        /// will be used.
        /// </summary>
        [Browsable(false)]
        public virtual LightDarkColor? FullItemToolTipBackColor { get; set; }

        /// <summary>
        /// Gets or sets the border color of the full item tooltip.
        /// Default is <c>null</c>, which means <see cref="DefaultFullItemToolTipBackColor"/>
        /// will be used.
        /// </summary>
        [Browsable(false)]
        public virtual LightDarkColor? FullItemToolTipBorderColor { get; set; }

        /// <summary>
        /// Gets or sets color of the horizontal line that is drawn between items.
        /// If not specified, the line color is determined by <see cref="DefaultHorzGridLinesColor"/> property.
        /// </summary>
        [Browsable(false)]
        public virtual LightDarkColor? HorzGridLinesColor
        {
            get
            {
                return horzGridLinesColor;
            }

            set
            {
                if (horzGridLinesColor == value)
                    return;
                horzGridLinesColor = value;
                if (HorzGridLines)
                    Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets color of the vertical line that is drawn between columns.
        /// If not specified, the line color is determined by <see cref="DefaultVertGridLinesColor"/> property.
        /// </summary>
        [Browsable(false)]
        public virtual LightDarkColor? VertGridLinesColor
        {
            get
            {
                return vertGridLinesColor;
            }

            set
            {
                if (vertGridLinesColor == value)
                    return;
                vertGridLinesColor = value;
                if (VertGridLines)
                    Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the horizontal grid lines are drawn between items.
        /// </summary>
        public bool HorzGridLines
        {
            get
            {
                return GridLinesDisplayMode == ListViewGridLinesDisplayMode.Horizontal
                    || GridLinesDisplayMode == ListViewGridLinesDisplayMode.VerticalAndHorizontal;
            }

            set
            {
                if (HorzGridLines == value)
                    return;
                GridLinesDisplayMode = value
                    ? (GridLinesDisplayMode == ListViewGridLinesDisplayMode.Vertical
                        ? ListViewGridLinesDisplayMode.VerticalAndHorizontal
                        : ListViewGridLinesDisplayMode.Horizontal)
                    : (GridLinesDisplayMode == ListViewGridLinesDisplayMode.VerticalAndHorizontal
                        ? ListViewGridLinesDisplayMode.Vertical
                        : ListViewGridLinesDisplayMode.None);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the horizontal grid lines are drawn between items.
        /// </summary>
        public bool VertGridLines
        {
            get
            {
                return GridLinesDisplayMode == ListViewGridLinesDisplayMode.Vertical
                    || GridLinesDisplayMode == ListViewGridLinesDisplayMode.VerticalAndHorizontal;
            }

            set
            {
                if (VertGridLines == value)
                    return;
                GridLinesDisplayMode = value
                    ? (GridLinesDisplayMode == ListViewGridLinesDisplayMode.Horizontal
                        ? ListViewGridLinesDisplayMode.VerticalAndHorizontal
                        : ListViewGridLinesDisplayMode.Vertical)
                    : (GridLinesDisplayMode == ListViewGridLinesDisplayMode.VerticalAndHorizontal
                        ? ListViewGridLinesDisplayMode.Horizontal
                        : ListViewGridLinesDisplayMode.None);
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

        /// <inheritdoc/>
        public override Color? BackgroundColor
        {
            get => base.BackgroundColor;
            set
            {
                base.BackgroundColor = value;
            }
        }

        /// <inheritdoc/>
        public override IScrollEventRouter ScrollEventRouter => this;

        /// <summary>
        /// Gets or sets the grid lines display mode.
        /// </summary>
        [Browsable(false)]
        public virtual ListViewGridLinesDisplayMode GridLinesDisplayMode
        {
            get
            {
                return gridLinesDisplayMode;
            }

            set
            {
                if (gridLinesDisplayMode == value)
                    return;
                gridLinesDisplayMode = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets row item by its index. If row index is below zero,
        /// a new item is created and added to the end of the list and returned.
        /// If row index is above or equal to the number of items, item list is extended with new
        /// items until the specified index is reached, and then the item at that index is returned.
        /// </summary>
        /// <param name="rowIndex">The index of the row.</param>
        /// <param name="fnCreateItem">A function to create a new item if it does not exist. Optional.
        /// If not specified, <see cref="ListControlItem"/> will be created.</param>
        /// <returns>The <see cref="ListControlItem"/> at the specified index.
        /// A result is guaranteed to be not null.</returns>
        public virtual ListControlItem SafeRow(int rowIndex, Func<ListControlItem>? fnCreateItem = null)
        {
            ListControlItem CreateItem()
            {
                if (fnCreateItem != null)
                    return fnCreateItem();
                return new ListControlItem();
            }

            ListControlItem item;

            if (rowIndex < 0)
            {
                item = CreateItem();
                Items.Add(item);
            }
            else
            {
                if (rowIndex >= Items.Count)
                {
                    Items.SetCount(rowIndex + 1, CreateItem);
                }

                item = Items[rowIndex];
            }

            return item;
        }

        /// <summary>
        /// Retrieves the cell associated with the specified row and column,
        /// ensuring that a valid cell is always returned.
        /// </summary>
        /// <param name="rowIndex">The index of the row.</param>
        /// <param name="column">The column for which to retrieve the corresponding cell. Cannot be null.</param>
        /// <param name="fnCreateItem">A function to create a new item if it does not exist. Optional.
        /// If not specified, <see cref="ListControlItem"/> will be created.</param>
        /// <returns>A <see cref="ListControlItem"/> representing the cell for the specified row and column.
        /// If the cell does not exist, it is created and added to the cells collection of the item.</returns>
        public virtual ListControlItem SafeCell(int rowIndex, ListControlColumn column, Func<ListControlItem>? fnCreateItem = null)
        {
            var item = SafeRow(rowIndex, fnCreateItem);
            return item.SafeCell(column);
        }

        /// <summary>
        /// Gets the cell for the specified row and column.
        /// If the cell does not exist, it is created and added to the cells collection of the item.
        /// </summary>
        /// <param name="rowIndex">The index of the row.</param>
        /// <param name="columnId">The unique identifier of the column.</param>
        /// <param name="fnCreateItem">A function to create a new item if it does not exist. Optional.
        /// If not specified, <see cref="ListControlItem"/> will be created.</param>
        /// <returns>A <see cref="ListControlItem"/> representing the cell for the specified row and column.
        /// If the cell does not exist, it is created and added to the cells collection of the item.</returns>
        public virtual ListControlItem SafeCell(int rowIndex, ObjectUniqueId columnId, Func<ListControlItem>? fnCreateItem = null)
        {
            var item = SafeRow(rowIndex, fnCreateItem);
            return item.SafeCell(columnId);
        }

        /// <summary>
        /// Gets cell item at the specified row index and column ID.
        /// </summary>
        /// <param name="rowIndex">The index of the row.</param>
        /// <param name="columnId">The unique ID of the column.</param>
        /// <returns>The cell item at the specified location, or null if no cell exists for the location.</returns>
        public virtual ListControlItem? GetCell(int rowIndex, ObjectUniqueId columnId)
        {
            var item = GetItem(rowIndex);
            return item?.GetCell(columnId);
        }

        /// <summary>
        /// Gets cell item at the specified row index and column.
        /// </summary>
        /// <param name="rowIndex">The index of the row.</param>
        /// <param name="column">The column for which to retrieve the cell.</param>
        /// <returns>The cell item at the specified location, or null if no cell exists for the location.</returns>
        public virtual ListControlItem? GetCell(int rowIndex, ListControlColumn column)
        {
            var item = GetItem(rowIndex);
            return item?.GetCell(column);
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
        /// Gets whether item with the specified index is visible on screen.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <returns><c>true</c> if the item is visible on the screen, <c>false</c> otherwise.</returns>
        public virtual bool IsItemVisibleOnScreen(int index)
        {
            if (DisposingOrDisposed)
                return default;
            var visibleBegin = GetVisibleBegin();
            var visibleEnd = GetVisibleEnd();

            var result = index >= visibleBegin && index < visibleEnd;

            var item = GetItem(index);

            if (item is not null)
                result = result && item.IsVisible;

            return result;
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
        /// <param name="validate">Whether to perform validation on the specified item index.
        /// Optional. Default is True.</param>
        /// <returns>True if we scrolled the control, False if nothing was done.</returns>
        public virtual bool ScrollToRow(int row, bool validate = true)
        {
            if (DisposingOrDisposed || Count == 0)
                return default;
            row = Math.Min(Count - 1, row);
            row = Math.Max(0, row);

            if (validate)
            {
                // determine the real first unit to scroll to: we shouldn't scroll beyond the end
                var unitFirstLast = FindFirstVisibleFromLast(Count - 1, true);
                if (row > unitFirstLast)
                    row = unitFirstLast;
            }

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
        /// Adds a separator item to the control.
        /// </summary>
        /// <remarks>
        /// The separator item is used to visually divide groups of items within the control.
        /// </remarks>
        /// <returns>A <see cref="ListControlItem"/> representing the added separator.</returns>
        public virtual ListControlItem AddSeparator()
        {
            ListControlSeparatorItem item = new();
            Add(item);
            return item;
        }

        /// <summary>
        /// Finds first visible item from the specified last visible item.
        /// </summary>
        /// <param name="unitLast">Index of the last visible item.</param>
        /// <param name="full">Whether to allow partial or full visibility of the last item.</param>
        /// <returns>The index of the first visible item.</returns>
        public virtual int FindFirstVisibleFromLast(int unitLast, bool full = false)
        {
            if (unitLast == 0)
                return 0;

            MeasureItemEventArgs e = new(MeasureCanvas, 0);

            var sWindow = GetPaintRectangle().Height;

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
            if (Count == 0)
                return;

            DoInsideUpdate(() =>
            {
                Items.Clear();
                Invalidate();
            });
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
            RectD itemRect = (0, 0, GetPaintRectangle().Width, 0);

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

        /// <inheritdoc/>
        public override int? HitTest(PointD position)
        {
            if (DisposingOrDisposed || Count == 0)
                return null;

            int lineMax = GetVisibleEnd();
            int lineFirst = GetVisibleBegin();

            for (int line = lineFirst; line < lineMax; line++)
            {
                var rect = GetItemRect(line);
                if (rect is null)
                    continue;
                if (rect.Value.Contains(position))
                    return line;
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

            RectD rect = (0, orientPos, GetPaintRectangle().Width, orientSize);
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

            if (!IsItemVisibleOnScreen(row))
                return;

            MeasureItemEventArgs e = new(MeasureCanvas, row);
            MeasureItemSize(e);

            RectD rect = (0, 0, GetPaintRectangle().Width, e.ItemHeight);

            for (int n = GetVisibleBegin(); n < row; ++n)
            {
                e.Index = n;
                MeasureItemSize(e);
                rect.Top += e.ItemHeight;
            }

            Invalidate(rect);
        }

        /// <summary>
        /// Refreshes the last row in the control.
        /// </summary>
        /// <remarks>
        /// This method checks if there are any items in the control. If there are, it refreshes
        /// the last row by invalidating its area and triggering a repaint.
        /// </remarks>
        public virtual void RefreshLastRow()
        {
            if (Count > 0)
                RefreshRow(Count - 1);
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
        /// Sets items to the new value using the fastest method.
        /// </summary>
        /// <param name="value">Collection with the new items.</param>
        public virtual void SetItemsFastest(IListSource<ListControlItem> value)
        {
            SetItemsFast(value, SetItemsKind.ChangeField);
        }

        /// <summary>
        /// Sets items to the new value using the specified method.
        /// </summary>
        /// <param name="value">Collection with the new items.</param>
        /// <param name="kind">The method which is used when items are set.</param>
        public virtual bool SetItemsFast(IListSource<ListControlItem> value, SetItemsKind kind)
        {
            if (value == Items)
                return true;

            if (DisposingOrDisposed)
                return default;

            firstVisibleItem = 0;

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
                    RecreateItems(value);
                });

                return true;
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
        /// Selects the specified item in the list and scrolls it into view if necessary.
        /// </summary>
        /// <param name="item">The item to select and bring into view.
        /// If <c>null</c>, no action is performed.</param>
        /// <remarks>
        /// This method delegates index resolution to <see cref="FindItemIndex"/>
        /// and scrolls using an index-based overload.
        /// Intended for user interaction scenarios where focus or visibility
        /// of a selected item is required.
        /// </remarks>
        public virtual void SelectItemAndScroll(ListControlItem? item)
        {
            var index = FindItemIndex(item);
            SelectItemAndScroll(index);
        }

        /// <summary>
        /// Selects an item at the specified index and scrolls to make it visible.
        /// </summary>
        /// <remarks>This method updates the selected item and ensures it is
        /// visible by scrolling to its position.</remarks>
        /// <param name="index">The zero-based index of the item to select.
        /// Must be within the valid range of items.</param>
        public virtual void SelectItemAndScroll(int? index)
        {
            if (DisposingOrDisposed)
                return;

            if (index is null)
            {
                SelectFirstItemAndScroll();
                return;
            }

            DoInsideUpdate(() =>
            {
                SelectedIndex = index;
                ScrollToRow(index.Value);
            });
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
            var item = GetItem(itemIndex);
            if (item is null)
                return false;

            var result = item.ToggleCheckState(this);

            if (result)
            {
                RaiseCheckedChanged(EventArgs.Empty);
                RefreshRow(itemIndex);
            }

            return result;
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
            var sWindow = GetPaintRectangle().Height;
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

            if (IsItemVisibleOnScreen(index))
            {
                if (index >= (GetVisibleEnd() - 1))
                    DoActionScrollLineDown();
                return true;
            }

            var newRow = FindFirstVisibleFromLast(index, full: true);
            var result = ScrollToRow(newRow);
            return result;
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
        /// Marks <see cref="Items"/> as immutable. After this call any attempt to change <see cref="Items"/> property
        /// will throw <see cref="InvalidOperationException"/>. This method is intended to be used
        /// in scenarios where items are set once and then not changed.
        /// </summary>
        public void SetImmutableItems()
        {
            immutableItems = true;
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
            base.Add(item);
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
        /// Measures the size of an item at the specified index.
        /// </summary>
        /// <remarks>This method calculates the dimensions of an item based on its index
        /// and returns the measured size. The returned size can be used for layout
        /// or rendering purposes.</remarks>
        /// <param name="index">The zero-based index of the item to measure.</param>
        /// <returns>A <see cref="SizeD"/> structure representing the width and height
        /// of the item.</returns>
        public SizeD MeasureItemSize(int index)
        {
            MeasureItemEventArgs e = new(MeasureCanvas, index);
            MeasureItemSize(e);
            return new SizeD(e.ItemWidth, e.ItemHeight);
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

            var item = GetItem(e.Index);

            if (item is not null)
            {
                if (!item.IsVisible)
                {
                    e.ItemHeight = 0;
                }
            }

            if (HorzGridLines && e.ItemHeight > 0)
            {
                e.ItemHeight += 1;
            }

            itemSize.Size = new SizeD(e.ItemWidth, e.ItemHeight);
            e.MeasureResult = itemSize;

            ListControlItem.MeasureItemSizeResult Internal(int itemIndex)
            {
                if (ItemPainter is null)
                    return ListControlItem.DefaultMeasureItemSize(this, e.Graphics, itemIndex, e.MeasureParams);

                var result = ItemPainter.GetSize(this, itemIndex);
                if (result == SizeD.MinusOne)
                    return ListControlItem.DefaultMeasureItemSize(this, e.Graphics, itemIndex, e.MeasureParams);
                return new(result);
            }
        }

        /// <summary>
        /// Gets effective vertical grid lines color. If <see cref="VertGridLinesColor"/> is not empty, returns it,
        /// otherwise returns <see cref="DefaultVertGridLinesColor"/>.
        /// </summary>
        /// <param name="isDark">Indicates whether the color should be adjusted for dark mode.</param>
        /// <returns>The effective vertical grid lines color.</returns>
        public virtual Color GetEffectiveVertGridLinesColor(bool isDark)
        {
            var result = VertGridLinesColor ?? DefaultVertGridLinesColor;
            return result.LightOrDark(isDark);
        }

        /// <summary>
        /// Gets effective horizontal grid lines color. If <see cref="HorzGridLinesColor"/> is not empty, returns it,
        /// otherwise returns <see cref="DefaultHorzGridLinesColor"/>.
        /// </summary>
        /// <param name="isDark">Indicates whether the color should be adjusted for dark mode.</param>
        /// <returns>The effective horizontal grid lines color.</returns>
        public virtual Color GetEffectiveHorzGridLinesColor(bool isDark)
        {
            var result = HorzGridLinesColor ?? DefaultHorzGridLinesColor;
            return result.LightOrDark(isDark);
        }

        /// <inheritdoc/>
        public virtual void CalcScrollBarInfo(
            out ScrollBarInfo horzScrollbar,
            out ScrollBarInfo vertScrollbar)
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

            Coord sWindow = GetPaintRectangle().Height;

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

            horzScrollbar = new((int)scrollOffsetX, (int)maxWidth, (int)ClientSize.Width);
            vertScrollbar = new(firstVisibleItem, Count, pageHeightInUnits);
            horzScrollbar.Visibility = horzVisibility;
            vertScrollbar.Visibility = vertVisibility;
        }

        /// <inheritdoc/>
        public virtual void DoActionScrollToVertPos(int value)
        {
            ScrollToRow((int)value);
        }

        /// <inheritdoc/>
        public virtual void DoActionScrollToHorzPos(int value)
        {
            var newOffset = Math.Max(value, 0);
            if (newOffset != scrollOffsetX)
            {
                scrollOffsetX = newOffset;
                UpdateScrollBars(true);
                ScrollOffsetXChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Increments horizontal scroll offset.
        /// </summary>
        /// <param name="delta">Increment value in device-independent units.</param>
        public virtual void IncHorizontalOffset(int delta)
        {
            DoActionScrollToHorzPos(scrollOffsetX + delta);
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
            var result = MeasureCanvas.GetTextExtent("W", GetItemFont(-1, true)).Width;
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
            IncHorizontalOffset((int)(chars * GetCharWidth()));
        }

        /// <summary>
        /// Sets horizontal scroll offset to the value specified in characters.
        /// </summary>
        /// <param name="offsetInChars">New horizontal scroll offset value in chars.</param>
        public virtual void SetHorizontalOffsetChars(int offsetInChars)
        {
            DoActionScrollToHorzPos((int)(offsetInChars * GetCharWidth()));
        }

        /// <summary>
        /// Finds the index of the item with <see cref="ListControlItem.Value"/> property which is
        /// equal to the specified value.
        /// </summary>
        /// <param name="value">Value to search for.</param>
        /// <returns></returns>
        public virtual int? FindItemIndexWithValue(object? value)
        {
            if (value is null)
                return null;

            for (int i = 0; i < Count; i++)
            {
                var item = GetItem(i);

                if (item is null)
                    continue;

                if (value.Equals(item.Value))
                    return i;
            }

            return null;
        }

        /// <inheritdoc/>
        public override void ResetCachedImages()
        {
            base.ResetCachedImages();
            ResetCachedImagesInItems();
        }

        /// <summary>
        /// Resets the cached images for all items in the collection.
        /// This method is called from <see cref="ResetCachedImages"/> to clear
        /// the cached images for all items.
        /// </summary>
        /// <remarks>This method iterates through all items in the collection and invokes their
        /// <see cref="ListControlItem.ResetCachedImages"/> method to clear any
        /// cached image data. Use this method to ensure that all items
        /// refresh their cached images,  for example, after an update
        /// to the underlying image source.</remarks>
        public virtual void ResetCachedImagesInItems()
        {
            for (int i = 0; i < Count; i++)
            {
                var item = GetItem(i);
                item?.ResetCachedImages();
            }
        }

        /// <summary>
        /// Searches for the specified item within the current list and returns its index if found.
        /// </summary>
        /// <param name="value">The item to locate within the items collection.</param>
        /// <returns>
        /// The zero-based index of the item if it exists in the collection; otherwise, <c>null</c>.
        /// Returns <c>null</c> if <paramref name="value"/> is <c>null</c>.
        /// </returns>
        /// <remarks>
        /// This method performs a reference equality check to locate the item.
        /// </remarks>
        public virtual int? FindItemIndex(ListControlItem? value)
        {
            if (value is null)
                return null;

            for (int i = 0; i < Count; i++)
            {
                if (GetItem(i) == value)
                    return i;
            }

            return null;
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

            for (int i = 0; i < Count; i++)
            {
                var item = GetItem(i);

                if (item is null)
                    continue;

                if (value.Equals(item.Value))
                    return item;
            }

            return null;
        }

        /// <summary>
        /// Adds a collection of items to the list control.
        /// </summary>
        /// <remarks>If an item in the collection is already a <see cref="ListControlItem"/>,
        /// it is added directly. Otherwise, a new <see cref="ListControlItem"/> is created
        /// for the item, with its <c>Value</c>
        /// property set to the item, and then added to the list.</remarks>
        /// <param name="items">The collection of items to add.
        /// Each item can either be a <see cref="ListControlItem"/> or an object that
        /// will be wrapped in a new <see cref="ListControlItem"/>.</param>
        public virtual void AddRange(IEnumerable items)
        {
            DoInsideUpdate(() =>
            {
                foreach (var item in items)
                {
                    if (item is ListControlItem listItem)
                        Add(listItem);
                    else
                    {
                        ListControlItem newItem = new();
                        newItem.Value = item;
                        Add(newItem);
                    }
                }
            });
        }

        /// <summary>
        /// Simulates the key press event by creating and dispatching a <see cref="KeyEventArgs"/>.
        /// </summary>
        /// <param name="key">The key that was pressed.</param>
        /// <param name="modifiers">Optional modifier keys associated with the key press.</param>
        public virtual void RunKeyDown(Key key, ModifierKeys modifiers = UI.ModifierKeys.None)
        {
            KeyEventArgs e = new();
            e.Key = key;
            e.ModifierKeys = modifiers;
            OnKeyDown(e);
        }

        /// <summary>
        /// Responds to changes in the item count by updating the scrollbars
        /// and invalidating the control.
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

        /// <summary>
        /// Resolves the foreground, background, and border colors to be used for an overlay tooltip, based on current
        /// theme and customization settings.
        /// </summary>
        /// <remarks>This method selects appropriate colors for overlay tooltips by considering
        /// user-defined color properties and the current background theme. Override this method to customize tooltip
        /// color resolution logic in derived classes.</remarks>
        /// <param name="foreColor">When this method returns, contains
        /// the resolved foreground color for the tooltip text.</param>
        /// <param name="backColor">When this method returns, contains
        /// the resolved background color for the tooltip.</param>
        /// <param name="borderColor">When this method returns, contains
        /// the resolved border color for the tooltip.</param>
        protected virtual void ResolveOverlayToolTipColors(out Color foreColor, out Color backColor, out Color borderColor)
        {
            backColor = (FullItemToolTipBackColor ?? DefaultFullItemToolTipBackColor)
                .LightOrDark(IsDarkBackground);
            foreColor = (FullItemToolTipForeColor ?? DefaultFullItemToolTipForeColor)
                .LightOrDark(IsDarkBackground);
            borderColor = (FullItemToolTipBorderColor ?? DefaultFullItemToolTipBorderColor
                ?? DefaultColors.BorderColor)
                .LightOrDark(IsDarkBackground);
        }

        /// <summary>
        /// Retrieves the tooltip text associated with the item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item for which to retrieve the tooltip text.</param>
        /// <returns>An object containing the tooltip for the specified item, or the item's display text if no tooltip is
        /// set.</returns>
        protected virtual object? GetItemToolTip(int index)
        {
            var item = GetItem(index);

            if (item is null)
                return null;

            var toolTip = item.ToolTip;

            if (toolTip is RichToolTipParams prm)
            {
                return prm;
            }

            if (toolTip is IGetAsToolTip asToolTip)
            {
                return asToolTip.GetAsToolTip();
            }

            var asString = toolTip?.ToString() ?? GetItemText(index, forDisplay: true);

            if (string.IsNullOrWhiteSpace(asString))
                return null;

            return asString;
        }

        /// <summary>
        /// Displays an overlay tooltip for the specified item if the item is eligible for tooltip display.
        /// </summary>
        /// <remarks>The method does not display a tooltip if the item index is null, the item text is
        /// empty, the item rectangle cannot be determined, or the item is fully visible without horizontal scrolling.
        /// Override this method to customize tooltip display behavior for items.</remarks>
        /// <param name="horzAlignment">The horizontal alignment for the tooltip relative to the container.</param>
        /// <param name="provider">The type of tooltip provider to use for displaying the tooltip.
        /// If null, the default provider is used.</param>
        /// <param name="itemIndex">The zero-based index of the item for which to show the overlay tooltip,
        /// or null to indicate no item.</param>
        /// <returns>true if the overlay tooltip was shown for the specified item; otherwise, false.</returns>
        protected virtual bool ShowOverlayToolTipForItem(
            int? itemIndex,
            HorizontalAlignment horzAlignment = HorizontalAlignment.Left,
            ItemToolTipProviderType? provider = null)
        {
            if (itemIndex is null)
                return false;

            provider ??= ItemToolTipProvider ?? DefaultItemToolTipProvider;

            if (provider == ItemToolTipProviderType.None) return false;

            var tooltip = GetItemToolTip(itemIndex.Value);

            if (tooltip is null)
                return false;

            var rect = GetItemRect(itemIndex);
            if (rect is null)
                return false;

            var itemSize = MeasureItemSize(itemIndex.Value);

            var container = GetPaintRectangle();

            var item = GetItem(itemIndex.Value);
            var noToolTipNeeded = container.Width > itemSize.Width && scrollOffsetX == 0;

            var showToolTip = item?.IsToolTipVisible ?? !noToolTipNeeded;

            if (!showToolTip)
                return false;

            var vertAlignment = NineRects.SuggestVertAlignmentForToolTip(container, rect.Value);

            RemoveOverlay(ref itemToolTipId, false);

            ResolveOverlayToolTipColors(out var fColor, out var bColor, out var borderColor);

            OverlayToolTipParams data = new()
            {
                Options = OverlayToolTipFlags.DismissAfterInterval,
                HorizontalAlignment = horzAlignment,
                VerticalAlignment = vertAlignment,
                BackgroundColor = bColor,
                ForegroundColor = fColor,
            };

            if (tooltip is string s)
                data.Text = s;
            else
                if (tooltip is RichToolTipParams content)
                    data.ToolTipParams = content;
                else
                    data.Text = tooltip.ToString() ?? string.Empty;

            data.SetBorder(borderColor);

            if (provider == ItemToolTipProviderType.Factory && !App.IsMaui)
            {
                var toolTip = ToolTipFactory.GetToolTip(this);

                if (toolTip != null)
                {
                    toolTip.SetParams(data.ToolTipParams).PostShowToolTip();
                    return true;
                }
            }

            data.MaxWidth = container.Width - 20;
            itemToolTipId = ShowOverlayToolTip(data);
            return true;
        }

        /// <summary>
        /// Displays an overlay tooltip for the item located at the current mouse position.
        /// </summary>
        /// <remarks>Override this method to customize how overlay tooltips are displayed for items at the
        /// mouse position. The method determines the item under the mouse and attempts to show its tooltip.</remarks>
        /// <returns>true if a tooltip was shown for the item under the mouse pointer; otherwise, false.</returns>
        protected virtual bool ShowOverlayToolTipForItemAtMousePos()
        {
            var mousePos = Mouse.GetPosition(this);
            var itemIndex = HitTest(mousePos);

            var mouseLeft = mousePos.X < GetPaintRectangle().Width / 2;

            HorizontalAlignment align = mouseLeft ? HorizontalAlignment.Right : HorizontalAlignment.Left;

            return ShowOverlayToolTipForItem(itemIndex, align);
        }

        /// <inheritdoc/>
        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);
            if (DisposingOrDisposed || !NeedsFullItemToolTip || HasColumns)
                return;
            ShowOverlayToolTipForItemAtMousePos();
        }

        /// <inheritdoc/>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            if (IsHoverSelectionEnabled)
            {
                SelectedIndex = null;
            }
        }

        /// <inheritdoc/>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            var selectedItemChanged = false;

            if (IsHoverSelectionEnabled)
            {
                var mousePos = Mouse.GetPosition(this);
                var itemIndex = HitTest(mousePos);

                selectedItemChanged = SelectedIndex != itemIndex;
                SelectedIndex = itemIndex;
            }

            var overlayRemoved = RemoveOverlay(ref itemToolTipId, invalidate: false);

            if (!selectedItemChanged && overlayRemoved)
                Refresh();
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

        /// <summary>
        /// Called when the <see cref="MeasureItem" /> event is raised.</summary>
        /// <param name="e">A <see cref="MeasureItemEventArgs" /> that
        /// contains the event data.</param>
        protected virtual void OnMeasureItem(MeasureItemEventArgs e)
        {
        }

        /// <inheritdoc/>
        protected override void OnItemsCollectionChanged(
            object? sender,
            ListChangedEventArgs e)
        {
            CountChanged();
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
        /// <param name="flags">Flags indicating the type
        /// of click (e.g., Shift, Ctrl, Keyboard).</param>
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
                                GetItem(item)?.ToggleSelected(this);
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

        /// <summary>
        /// Gets the index of the first visible item at or after the specified index.
        /// </summary>
        /// <param name="index">The starting index to search from.</param>
        /// <returns>The index of the first visible item at or after the specified index.
        /// If no visible item is found, returns the specified index.</returns>
        protected virtual int GetVisibleItemIndexAtOrAfter(int index)
        {
            for (int i = index; i < Count; i++)
            {
                var item = GetItem(i);

                if (item?.IsVisible == true || item is null)
                    return i;
            }

            return index;
        }

        /// <summary>
        /// Gets the index of the first visible item at or before the specified index.
        /// </summary>
        /// <param name="index">The starting index to search from.</param>
        /// <returns>The index of the first visible item at or before the specified index.
        /// If no visible item is found, returns the specified index.</returns>
        protected virtual int GetVisibleItemIndexAtOrBefore(int index)
        {
            for (int i = index; i >= 0; i--)
            {
                var item = GetItem(i);

                if (item?.IsVisible == true || item is null)
                    return i;
            }

            return index;
        }

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (DisposingOrDisposed)
                return;

            base.OnKeyDown(e);

            if (e.IsHandledOrSuppressed || Count == 0)
                return;

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
                        SelectAllVisible();
                        e.Suppressed();
                    }

                    return;

                case Key.Home:
                    if (e.Control)
                    {
                        DoActionScrollToHorzPos(0);
                        e.Suppressed();
                        return;
                    }
                    else
                    {
                        current = GetVisibleItemIndexAtOrAfter(0);
                    }

                    break;

                case Key.End:
                    current = GetVisibleItemIndexAtOrBefore(Count - 1);
                    break;

                case Key.Down:
                    if (selected is null)
                    {
                        SelectedIndex = GetVisibleItemIndexAtOrAfter(0);
                        e.Suppressed();
                        return;
                    }
                    else
                        if (selected >= Count - 1)
                        {
                            e.Suppressed();
                            return;
                        }

                    current = GetVisibleItemIndexAtOrAfter(selected.Value + 1);
                    break;

                case Key.Up:
                    if (selected is null)
                    {
                        SelectedIndex = GetVisibleItemIndexAtOrBefore(Count - 1);
                        e.Suppressed();
                        return;
                    }
                    else
                        if (selected <= 0)
                        {
                            e.Suppressed();
                            return;
                        }

                    current = GetVisibleItemIndexAtOrBefore(selected.Value - 1);
                    break;

                case Key.PageDown:
                    current = GetVisibleItemIndexAtOrAfter(GetIndexOnNextPage() ?? 0);
                    break;

                case Key.PageUp:
                    current = GetVisibleItemIndexAtOrBefore(GetIndexOnPreviousPage() ?? 0);
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
                    // pressing space should work like a mouse click rather than
                    // like a keyboard arrow press, so trick DoHandleItemClick() in
                    // thinking we were clicked.
                    flags &= ~ItemClickFlags.Keyboard;
                    current = GetVisibleItemIndexAtOrAfter(selected ?? 0);
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
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);

            DoInsideUpdate(() =>
            {
                if (SelectedIndex is null)
                {
                    if (Count > 0)
                        SelectItemsAndScroll(0);
                }

                Invalidate();
            });
        }

        /// <inheritdoc/>
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            Invalidate();
        }

        /// <inheritdoc/>
        protected override void OnDpiChanged(DpiChangedEventArgs e)
        {
            ResetCachedImages();
            base.OnDpiChanged(e);
        }

        /// <inheritdoc/>
        protected override void OnSizeChanged(EventArgs e)
        {
            DoInsideUpdate(() =>
            {
                DoActionScrollToHorzPos(0);
                base.OnSizeChanged(e);
            });
        }

        /// <inheritdoc/>
        protected override void OnSystemColorsChanged(EventArgs e)
        {
            if (AutoUpdateColors)
            {
                if (SystemSettings.AppearanceIsDark)
                    SetColorThemeToDark();
                else
                    SetColorThemeToLight();
            }

            ResetCachedImages();

            base.OnSystemColorsChanged(e);
        }
    }
}