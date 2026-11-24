using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Interface for the custom list box control which extends <see cref="IListControl{T}"/>
    /// with multi-select and other features.
    /// </summary>
    /// <typeparam name="TItem">Type of the item.</typeparam>
    public interface ICustomListBox<TItem> : IListControl<TItem>
            where TItem : class, new()
    {
        /// <summary>
        /// Occurs when the <see cref="IListControl{TItem}.SelectedIndex"/> property or the
        /// <see cref="SelectedIndices"/> collection has changed.
        /// </summary>
        /// <remarks>
        /// You can create an event handler for this event to determine when the
        /// selected index in the control has been changed.
        /// This can be useful when you need to display information in other
        /// controls based on the current selection in the control.
        /// <para>
        /// You can use the event handler for this event to load the information in
        /// the other controls. If the <see cref="SelectionMode"/> property
        /// is set to <see cref="ListBoxSelectionMode.Multiple"/>, any change to the
        /// <see cref="SelectedIndices"/> collection,
        /// including removing an item from the selection, will raise this event.
        /// </para>
        /// </remarks>
        event EventHandler? SelectionChanged;

        /// <summary>
        /// Occurs when the <see cref="IListControl{TItem}.SelectedIndex"/>,
        /// <see cref="IListControl{TItem}.SelectedItem"/> or the
        /// <see cref="SelectedIndices"/> have changed.
        /// </summary>
        /// <remarks>
        /// This is a delayed event. If multiple events are occurred during the delay,
        /// they are ignored.
        /// </remarks>
        event EventHandler<EventArgs>? DelayedSelectionChanged;

        /// <summary>
        /// Occurs when the <see cref="IListControl{TItem}.SelectedIndex"/> property has changed.
        /// Same as <see cref="SelectionChanged"/>, see it for the details.
        /// </summary>
        event EventHandler? SelectedIndexChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="SelectionMode"/> property changes.
        /// </summary>
        event EventHandler? SelectionModeChanged;

        /// <summary>
        /// Gets a collection that contains the zero-based indexes of all
        /// currently selected items in the control.
        /// </summary>
        /// <value>
        /// An <see cref="IReadOnlyList{T}"/> containing the indexes of the
        /// currently selected items in the control.
        /// If no items are currently selected, an empty
        /// <see cref="IReadOnlyList{T}"/> is returned.
        /// </value>
        /// <remarks>
        /// For a multiple-selection control, this property
        /// returns a collection containing the indexes to all items that are selected
        /// in the control. For a single-selection
        /// control, this property returns a collection containing a
        /// single element containing the index of the only selected item in the
        /// control.
        /// <para>
        /// There are a number of ways to
        /// reference selected items. Instead of using the <see cref="SelectedIndices"/>
        /// property to obtain the index position of the currently selected item
        /// in a single-selection control, you
        /// can use the <see cref="IListControl{TItem}.SelectedIndex"/> property.
        /// If you want to obtain
        /// the item that is currently selected in the control,
        /// instead of the index position of the item, use the
        /// <see cref="IListControl{TItem}.SelectedItem"/> property. In addition,
        /// you can use the <see cref="SelectedItems"/> property if you want to
        /// obtain all the selected items in a multiple-selection control.
        /// </para>
        /// </remarks>
        /// <seealso cref="SelectedIndicesDescending"/>
        IReadOnlyList<int> SelectedIndices { get; set; }

        /// <summary>
        /// Gets or sets default timeout interval (in msec) for timer that calls
        /// <see cref="DelayedSelectionChanged"/> event. If not specified,
        /// <see cref="TimerUtils.DefaultDelayedTextChangedTimeout"/> is used.
        /// </summary>
        int? DelayedSelectionChangedInterval { get; set; }

        /// <summary>
        /// Same as <see cref="SelectedIndices"/>.
        /// </summary>
        IReadOnlyList<int> SelectedIndexes { get; }

        /// <summary>
        /// Gets a collection that contains the zero-based indexes of all
        /// currently selected
        /// items in the control.
        /// </summary>
        /// <remarks>
        /// Indexes are returned in the descending order (maximal index is
        /// the first).
        /// </remarks>
        /// <seealso cref="SelectedIndices"/>
        /// <value>
        /// An <see cref="IReadOnlyList{T}"/> containing the indexes of the
        /// currently selected items in the control.
        /// If no items are currently selected, an empty
        /// <see cref="IReadOnlyList{T}"/> is returned.
        /// </value>
        IReadOnlyList<int> SelectedIndicesDescending { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the control has a border.
        /// </summary>
        bool HasBorder { get; set; }

        /// <summary>
        /// Gets 'Tag' property of the selected item if it is <see cref="BaseObjectWithAttr"/>.
        /// </summary>
        object? SelectedItemTag { get; }

        /// <summary>
        /// Gets a collection containing the currently selected items in the
        /// control.
        /// </summary>
        /// <value>A <see cref="IReadOnlyList{T}"/> containing the currently
        /// selected items in the control.</value>
        /// <remarks>
        /// For a multiple-selection control, this property
        /// returns a collection containing the indexes to all items that are selected
        /// in the control. For a single-selection
        /// control, this property returns a collection containing a
        /// single element containing the index of the only selected item in the
        /// control.
        /// <para>
        /// There are a number of ways to
        /// reference selected items. Instead of using the <see cref="SelectedIndices"/>
        /// property to obtain the index position of the currently selected item
        /// in a single-selection control, you
        /// can use the <see cref="IListControl{TItem}.SelectedIndex"/> property.
        /// If you want to obtain
        /// the item that is currently selected in the control,
        /// instead of the index position of the item, use the
        /// <see cref="IListControl{TItem}.SelectedItem"/> property.
        /// In addition, you can use the <see cref="SelectedIndices"/> property
        /// to obtain all the selected indexes in a multiple-selection
        /// control.
        /// </para>
        /// </remarks>
        IReadOnlyList<TItem> SelectedItems { get; }

        /// <summary>
        /// Gets or sets whether selection mode is <see cref="ListBoxSelectionMode.Single"/>
        /// </summary>
        bool IsSelectionModeSingle { get; set; }

        /// <summary>
        /// Gets or sets whether selection mode is <see cref="ListBoxSelectionMode.Multiple"/>
        /// </summary>
        bool IsSelectionModeMultiple { get; set; }

        /// <summary>
        /// Gets or sets the method in which items are selected in the
        /// control.
        /// </summary>
        /// <value>One of the <see cref="ListBoxSelectionMode"/> values.
        /// The default is <see cref="ListBoxSelectionMode.Single"/>.</value>
        /// <remarks>
        /// The <see cref="SelectionMode"/> property enables you to determine
        /// how many items in the control
        /// a user can select at one time.
        /// </remarks>
        ListBoxSelectionMode SelectionMode { get; set; }

        /// <summary>
        /// Removes selected items from the control.
        /// </summary>
        bool RemoveSelectedItems();

        /// <summary>
        /// Returns the zero-based index of the item at the specified coordinates.
        /// </summary>
        /// <param name="position">A <see cref="PointD"/> object containing
        /// the coordinates used to obtain the item
        /// index.</param>
        /// <returns>The zero-based index of the item found at the specified
        /// coordinates; returns <see langword="null"/>
        /// if no match is found.</returns>
        int? HitTest(PointD position);

        /// <summary>
        /// Gets only valid indexes from the list of indexes in
        /// the control.
        /// </summary>
        IReadOnlyList<int> GetValidIndexes(params int[] indexes);

        /// <summary>
        /// Copies result of the <see cref="SelectedItemsAsText"/> to clipboard.
        /// </summary>
        bool SelectedItemsToClipboard(string? separator = default);

        /// <summary>
        /// Gets selected items as <see cref="string"/>.
        /// </summary>
        /// <remarks>Each item is separated by <paramref name="separator"/> or
        /// <see cref="Environment.NewLine"/> if it is empty.</remarks>
        /// <param name="separator">Items separator string.</param>
        string? SelectedItemsAsText(string? separator = default);

        /// <summary>
        /// Selects items with specified indexes in the control.
        /// </summary>
        bool SelectItems(params int[] indexes);

        /// <summary>
        /// Checks whether index is valid in the control.
        /// </summary>
        bool IsValidIndex(int index);

        /// <summary>
        /// Selects or clears the selection for the specified item in a
        /// control.
        /// </summary>
        /// <param name="index">The zero-based index of the item in a
        /// control to select or clear the selection for.</param>
        /// <param name="value"><c>true</c> to select the specified item;
        /// otherwise, false.</param>
        /// <remarks>
        /// You can use this property to set the selection of items in a
        /// multiple-selection control.
        /// To select an item in a single-selection control, use
        /// the <see cref="IListControl{TItem}.SelectedIndex"/> property.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">The specified index
        /// was outside the range of valid values.</exception>
        bool SetSelected(int index, bool value);

        /// <summary>
        /// Gets whether control has items.
        /// </summary>
        bool HasItems();

        /// <summary>
        /// Gets whether control has selected items.
        /// </summary>
        bool HasSelectedItems();

        /// <summary>
        /// Gets whether selected item can be removed.
        /// </summary>
        /// <returns></returns>
        bool CanRemoveSelectedItem();

        /// <summary>
        /// Removes selected item from the control.
        /// </summary>
        void RemoveSelectedItem();

        /// <summary>
        /// Runs action specified in the <see cref="ListControlItem.DoubleClickAction"/> property
        /// of the selected item.
        /// </summary>
        void RunSelectedItemDoubleClickAction();

        /// <summary>
        /// Raises the <see cref="SelectionChanged"/> event and calls related method.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that
        /// contains the event data.</param>
        void RaiseSelectionChanged(EventArgs? e = null);
    }
}
