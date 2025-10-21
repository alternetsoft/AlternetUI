using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
    public partial class ListBox : Control, ICollectionChangeRouter, IListBoxActions
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

        /// <summary>
        /// Gets whether checkboxes are shown in the items.
        /// </summary>
        [Browsable(false)]
        public virtual bool CheckBoxVisible
        {
            get
            {
                return false;
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
            if(SelectionMode == SelectionMode.MultiExtended || SelectionMode == SelectionMode.MultiSimple)
            {
                var indexes = SelectedIndices;
                if(indexes.Count > 0)
                    return indexes[0];
                return -1;
            }

            return PlatformControl.GetSelection();
        }

        /// <summary>
        /// Determines whether the item at the specified index is selected.
        /// </summary>
        /// <param name="n">Zero-based index of the item to test.</param>
        /// <returns><c>true</c> if the item is selected; otherwise, <c>false</c>.</returns>
        public virtual bool GetSelected(int n) => PlatformControl.IsSelected(n);

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
        public virtual int FindStringExact(string s, bool bCase = false) => PlatformControl.FindString(s, bCase);

        /// <summary>
        /// Gets the number of items that are visible per page in the list box viewport.
        /// </summary>
        /// <returns>The number of items per page.</returns>
        public virtual int GetCountPerPage()
        {
            return PlatformControl.GetCountPerPage();
        }

        /// <summary>
        /// Deselects all currently selected items.
        /// </summary>
        public void ClearSelected()
        {
            var count = GetCount();
            if (count == 0)
                return;

            PlatformControl.UpdateSelections();
            var selectedCount = PlatformControl.GetSelectionsCount();
            if (selectedCount == 0)
                return;

            if (selectedCount == 1)
                Internal();
            else
                DoInsideUpdate(Internal);

            void Internal()
            {
                for (int i = 0; i < selectedCount; i++)
                {
                    var index = PlatformControl.GetSelectionsItem(i);
                    Deselect(index);
                }
            }
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
        /// Sets the selection to the item at the specified index.
        /// </summary>
        /// <param name="n">Zero-based index of the item to select.</param>
        public virtual void SetSelection(int n)
        {
            PlatformControl.SetSelection(n);
        }

        /// <summary>
        /// Updates the selection state of the specified item.
        /// </summary>
        /// <remarks>When <paramref name="select"/> is <see langword="true"/>,
        /// the item is selected using
        /// the platform-specific  selection mechanism. When <paramref name="select"/>
        /// is <see langword="false"/>, the item is deselected.</remarks>
        /// <param name="n">The zero-based index of the item to update.</param>
        /// <param name="select">A value indicating whether to select or deselect the item.
        /// <see langword="true"/> to select the item; <see
        /// langword="false"/> to deselect it.</param>
        public virtual void SetSelected(int n, bool select)
        {
            if (select)
                PlatformControl.SetSelection(n, select);
            else
                Deselect(n);
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
        public virtual string GetItemText(object item)
        {
            if (item == null)
                return string.Empty;
            if (item is string s)
                return s;
            if (item is IFormattable formattable)
                return formattable.ToString(null, System.Globalization.CultureInfo.CurrentCulture);
            return item.ToString() ?? string.Empty;
        }

        /// <summary>
        /// Selects all items in the list box, if the selection mode allows multiple selections.
        /// </summary>
        /// <remarks>This method performs the selection operation only if the <see cref="SelectionMode"/>
        /// property is set to a mode that supports multiple selections.</remarks>
        /// <returns><see langword="true"/> if all items were successfully selected;
        /// <see langword="false"/> if the selection
        /// mode does not allow multiple selections.</returns>
        public virtual void SelectAll()
        {
            if (SelectionMode == SelectionMode.One || SelectionMode == SelectionMode.None)
                return;

            DoInsideUpdate(() =>
            {
                var count = GetCount();
                for (int i = 0; i < count; i++)
                {
                    SetSelected(i, true);
                }
            });
        }

        /// <summary>
        /// Deselects all currently selected items.
        /// </summary>
        /// <remarks>This method clears the selection state of all items. After calling this method, no
        /// items will be selected.</remarks>
        public virtual void UnselectAll()
        {
            DoInsideUpdate(() =>
            {
                foreach (var index in SelectedIndices)
                {
                    Deselect(index);
                }
            });
        }

        /// <summary>
        /// Removes the items at the currently selected indices from the collection.
        /// </summary>
        /// <remarks>The indices are processed in descending order to ensure that removing an item does
        /// not affect the positions of subsequent indices. This operation is performed
        /// within an update block to
        /// maintain consistency.</remarks>
        public virtual void RemoveSelectedItems()
        {
            var descendingSelectedIndices = SelectedIndices.OrderByDescending(i => i);

            DoInsideUpdate(() =>
            {
                foreach (var index in descendingSelectedIndices)
                {
                    Items.RemoveAt(index);
                }
            });
        }

        /// <summary>
        /// Selects the first item in the list box, if any items are available.
        /// </summary>
        /// <remarks>If the list box is empty, this method performs no action. Otherwise, it clears the
        /// current selection and selects the first item in the list box.</remarks>
        public virtual void SelectFirstItem()
        {
            if (GetCount() == 0)
                return;

            DoInsideUpdate(() =>
            {
                ClearSelected();
                SetSelection(0);
            });
        }

        /// <summary>
        /// Selects the last item in the list box, if any items are available.
        /// </summary>
        /// <remarks>If the list box is empty, this method performs no action. Otherwise, it clears the
        /// current selection and selects the last item in the list box.</remarks>
        public virtual void SelectLastItem()
        {
            if (GetCount() == 0)
                return;

            DoInsideUpdate(() =>
            {
                ClearSelected();
                SetSelection(GetCount() - 1);
            });
        }

        /// <summary>
        /// Removes all items from the list box.
        /// </summary>
        /// <remarks>If the list box is already empty, this method performs no operation. This method
        /// ensures that the removal operation is performed within an update context.</remarks>
        public virtual void RemoveAll()
        {
            if (GetCount() == 0)
                return;
            DoInsideUpdate(() =>
            {
                Items.Clear();
            });
        }

        /// <summary>
        /// Returns a string representation for this control.
        /// </summary>
        public override string ToString()
        {
            string s = base.ToString();
            if (itemsCollection is not null)
            {
                s += $", Items.Count: {Items.Count}";
                if (Items.Count > 0)
                {
                    string? z = GetItemText(Items[0]);
                    if (z is not null)
                    {
                        ReadOnlySpan<char> txt = (z.Length > 40) ? z.AsSpan(0, 40) : z;
                        s += $", Items[0]: {txt.ToString()}";
                    }
                }
            }

            return s;
        }

        /// <inheritdoc/>
        void ICollectionChangeRouter.OnCollectionAdd(object? sender, IList newItems, int newIndex)
        {
            if (DisposingOrDisposed)
                return;
            if (newItems.Count <= 1)
                Internal();
            else
                DoInsideUpdate(Internal);

            void Internal()
            {
                foreach (var newItem in newItems)
                {
                    Insert(newItem, newIndex);
                    newIndex++;
                }
            }
        }

        /// <inheritdoc/>
        void ICollectionChangeRouter.OnCollectionReset(object? sender)
        {
            if(DisposingOrDisposed)
                return;
            DoInsideUpdate(() =>
            {
                if (GetCount() != 0)
                    PlatformControl.Clear();
                foreach (var item in Items)
                {
                    Add(item);
                }
            });
        }

        /// <inheritdoc/>
        void ICollectionChangeRouter.OnCollectionReplace(
            object? sender,
            IList oldItems,
            IList newItems,
            int index)
        {
            if (DisposingOrDisposed)
                return;
            var router = (ICollectionChangeRouter)this;
            DoInsideUpdate(() =>
            {
                router.OnCollectionRemove(sender, oldItems, index);
                router.OnCollectionAdd(sender, newItems, index);
            });
        }

        /// <inheritdoc/>
        void ICollectionChangeRouter.OnCollectionMove(
            object? sender,
            IList movedItems,
            int oldIndex,
            int newIndex)
        {
            if (oldIndex == newIndex)
                return;
            if (DisposingOrDisposed)
                return;
            var router = (ICollectionChangeRouter)this;
            DoInsideUpdate(() =>
            {
                router.OnCollectionRemove(sender, movedItems, oldIndex);
                router.OnCollectionAdd(sender, movedItems, newIndex);
            });
        }

        /// <inheritdoc/>
        void ICollectionChangeRouter.OnCollectionRemove(object? sender, IList oldItems, int oldIndex)
        {
            if (DisposingOrDisposed)
                return;

            if (oldItems.Count <= 1)
                Internal();
            else
                DoInsideUpdate(Internal);

            void Internal()
            {
                for (int i = 0; i < oldItems.Count; i++)
                    Delete(oldIndex);
            }
        }

        /// <summary>
        /// Gets the string representation of the item at the specified index.
        /// </summary>
        /// <param name="n">Zero-based index of the item.</param>
        /// <returns>The item's string.</returns>
        internal virtual string GetItem(int n)
        {
            return PlatformControl.GetString(n);
        }

        /// <summary>
        /// Replaces the item at the specified index.
        /// </summary>
        /// <param name="n">Zero-based index of the item.</param>
        /// <param name="s">The new string for the item.</param>
        internal virtual bool SetItem(int n, object s)
        {
            if (n < 0 || n >= GetCount())
                return false;
            PlatformControl.SetString(n, GetItemText(s));
            return true;
        }

        /// <summary>
        /// Deletes the item at the specified index.
        /// </summary>
        /// <param name="n">Zero-based index of the item to delete.</param>
        internal virtual void Delete(int n)
        {
            PlatformControl.Delete(n);
        }

        /// <summary>
        /// Appends an item with the specified string to the end of the list box.
        /// </summary>
        /// <param name="s">The string to append.</param>
        /// <returns>The zero-based index of the newly appended item.</returns>
        internal virtual int Add(object s)
        {
            return PlatformControl.Append(GetItemText(s));
        }

        /// <summary>
        /// Inserts an item at the specified position.
        /// </summary>
        /// <param name="item">The string for the item to insert.</param>
        /// <param name="pos">Zero-based position to insert the item at.</param>
        /// <returns>The zero-based index of the inserted item.</returns>
        internal virtual int Insert(object item, int pos)
        {
            return PlatformControl.Insert(GetItemText(item), pos);
        }

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return ControlFactory.Handler.CreateListBoxHandler(this);
        }

        /// <inheritdoc/>
        protected override void OnInsertedToParent(AbstractControl parentControl)
        {
            base.OnInsertedToParent(parentControl);

            // This is needed to fix rendering issues with wxWidgets on Windows
            // when dark mode is used.
            if (App.PlatformKind == UIPlatformKind.WxWidgets && App.IsWindowsOS)
            {
                RecreateHandler();
            }
        }
    }
}
