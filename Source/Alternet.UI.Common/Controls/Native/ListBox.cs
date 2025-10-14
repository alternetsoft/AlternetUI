using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that displays a list of items and allows
    /// the user to select one or more items.
    /// This control encapsulates the native list box implemented by the operating system.
    /// It has limited functionality.
    /// For the full featured list control, use the <see cref="VirtualListBox"/>
    /// or <see cref="StdListBox"/> controls.
    /// </summary>
    /// <remarks>The <see cref="ListBox"/> control is commonly used to present a collection of items in a
    /// scrollable list.  It supports single or multiple selection modes,
    /// depending on the configuration.
    /// This control is a part of the user interface framework and inherits
    /// from <see cref="Control"/>.</remarks>
    [DefaultEvent("SelectedIndexChanged")]
    [DefaultProperty("Items")]
    [DefaultBindingProperty("SelectedValue")]
    [ControlCategory("Common")]
    public partial class ListBox : Control
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListBox"/> class
        /// with the specified parent control.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ListBox(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBox"/> class.
        /// </summary>
        public ListBox()
        {
            Handler.Required();
        }

        /// <summary>
        /// Gets or sets a value indicating whether the list box has a visible border.
        /// </summary>
        [Browsable(true)]
        public override bool HasBorder
        {
            get => base.HasBorder;
            set
            {
                if (base.HasBorder == value)
                    return;
                base.HasBorder = value;
                PlatformControl.HasBorder = value;
            }
        }

        internal IListBoxHandler PlatformControl
        {
            get
            {
                CheckDisposed();
                return (IListBoxHandler)Handler;
            }
        }

        /// <summary>
        /// Gets the index of the current selection.
        /// </summary>
        /// <returns>The zero-based index of the selected item, or -1 if no selection.</returns>
        public virtual int GetSelection()
        {
            return PlatformControl.GetSelection();
        }

        /// <summary>
        /// Determines whether the item at the specified index is selected.
        /// </summary>
        /// <param name="n">Zero-based index of the item to test.</param>
        /// <returns><c>true</c> if the item is selected; otherwise, <c>false</c>.</returns>
        public virtual bool IsSelected(int n) => PlatformControl.IsSelected(n);

        /// <summary>
        /// Determines whether the items in the list box are displayed in sorted order.
        /// </summary>
        /// <returns><c>true</c> if items are sorted; otherwise, <c>false</c>.</returns>
        public virtual bool IsSorted() => PlatformControl.IsSorted();

        /// <summary>
        /// Selects or deselects the first item whose content matches the specified string.
        /// </summary>
        /// <param name="s">The string to match.</param>
        /// <param name="select"><c>true</c> to select the matching item;
        /// <c>false</c> to deselect.</param>
        /// <returns><c>true</c> if an item was found and its
        /// selection state changed; otherwise, <c>false</c>.</returns>
        public virtual bool SetStringSelection(string s, bool select)
        {
            return PlatformControl.SetStringSelection(s, select);
        }

        /// <summary>
        /// Finds the index of the first item whose string representation matches the specified text.
        /// </summary>
        /// <param name="s">The string to search for.</param>
        /// <param name="bCase">If <c>true</c>, perform a case-sensitive search;
        /// otherwise case-insensitive.</param>
        /// <returns>The zero-based index of the matching item, or -1 if not found.</returns>
        public virtual int FindString(string s, bool bCase = false) => PlatformControl.FindString(s, bCase);

        /// <summary>
        /// Gets the number of items that are visible per page in the list box viewport.
        /// </summary>
        /// <returns>The number of items per page.</returns>
        public virtual int GetCountPerPage()
        {
            return PlatformControl.GetCountPerPage();
        }

        /// <summary>
        /// Gets the index of the topmost visible item in the list box.
        /// </summary>
        /// <returns>The zero-based index of the top item.</returns>
        public virtual int GetTopItem()
        {
            return PlatformControl.GetTopItem();
        }

        /// <summary>
        /// Performs a hit test at the specified point and returns the index of the item under that point.
        /// </summary>
        /// <param name="point">Point in control coordinates to test.</param>
        /// <returns>The zero-based index of the item under the point, or -1 if none.</returns>
        public virtual int HitTest(PointD point)
        {
            return PlatformControl.HitTest(point);
        }

        /// <summary>
        /// Gets the string representation of the item at the specified index.
        /// </summary>
        /// <param name="n">Zero-based index of the item.</param>
        /// <returns>The item's string.</returns>
        public virtual string GetItem(int n)
        {
            return PlatformControl.GetString(n);
        }

        /// <summary>
        /// Gets the number of items in the list box.
        /// </summary>
        /// <returns>The count of items.</returns>
        public virtual int GetCount()
        {
            return PlatformControl.GetCount();
        }

        /// <summary>
        /// Deselects the item at the specified index.
        /// </summary>
        /// <param name="n">Zero-based index of the item to deselect.</param>
        public virtual void Deselect(int n)
        {
            PlatformControl.Deselect(n);
        }

        /// <summary>
        /// Ensures that the item at the given index is visible in the viewport.
        /// </summary>
        /// <param name="n">Zero-based index of the item to make visible.</param>
        public virtual void EnsureVisible(int n)
        {
            PlatformControl.EnsureVisible(n);
        }

        /// <summary>
        /// Sets the specified item to be the first (top) visible item.
        /// </summary>
        /// <param name="n">Zero-based index of the item to place first.</param>
        public virtual void SetFirstItem(int n)
        {
            PlatformControl.SetFirstItem(n);
        }

        /// <summary>
        /// Sets the first visible item by searching for the specified string and making that item first.
        /// </summary>
        /// <param name="s">The string of the item to set as first.</param>
        public virtual void SetFirstItem(string s)
        {
            PlatformControl.SetFirstItemStr(s);
        }

        /// <summary>
        /// Sets the selection to the item at the specified index.
        /// </summary>
        /// <param name="n">Zero-based index of the item to select.</param>
        public virtual void SetSelection(int n)
        {
            PlatformControl.SetSelection(n);
        }

        /// <summary>
        /// Replaces the item at the specified index.
        /// </summary>
        /// <param name="n">Zero-based index of the item.</param>
        /// <param name="s">The new string for the item.</param>
        public virtual bool SetItem(int n, object s)
        {
            if (n < 0 || n >= GetCount())
                return false;
            PlatformControl.SetString(n, ItemToString(s));
            return true;
        }

        /// <summary>
        /// Removes all items from the list box.
        /// </summary>
        public virtual void Clear()
        {
            PlatformControl.Clear();
        }

        /// <summary>
        /// Deletes the item at the specified index.
        /// </summary>
        /// <param name="n">Zero-based index of the item to delete.</param>
        public virtual void Delete(int n)
        {
            PlatformControl.Delete(n);
        }

        /// <summary>
        /// Appends an item with the specified string to the end of the list box.
        /// </summary>
        /// <param name="s">The string to append.</param>
        /// <returns>The zero-based index of the newly appended item.</returns>
        public virtual int Add(object s)
        {
            return PlatformControl.Append(ItemToString(s));
        }

        /// <summary>
        /// Inserts an item at the specified position.
        /// </summary>
        /// <param name="item">The string for the item to insert.</param>
        /// <param name="pos">Zero-based position to insert the item at.</param>
        /// <returns>The zero-based index of the inserted item.</returns>
        public virtual int Insert(object item, int pos)
        {
            return PlatformControl.Insert(ItemToString(item), pos);
        }

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return ControlFactory.Handler.CreateListBoxHandler(this);
        }

        /// <summary>
        /// Converts the specified item to its string representation.
        /// </summary>
        /// <param name="item">The object to convert. Can be <see langword="null"/>.</param>
        /// <returns>A string representation of the specified item. Returns an empty string
        /// if <paramref name="item"/> is <see langword="null"/>.
        /// If the item is a string, it is returned as-is. If the item implements
        /// <see cref="System.IFormattable"/>, its formatted string representation
        /// is returned using the current culture.
        /// Otherwise, the result of <see cref="object.ToString"/> is returned.</returns>
        protected virtual string ItemToString(object item)
        {
            if (item == null)
                return string.Empty;
            if (item is string s)
                return s;
            if (item is IFormattable formattable)
                return formattable.ToString(null, System.Globalization.CultureInfo.CurrentCulture);
            return item.ToString() ?? string.Empty;
        }
    }
}
