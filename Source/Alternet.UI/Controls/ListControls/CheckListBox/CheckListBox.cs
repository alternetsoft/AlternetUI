﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control to display a list of items with checkboxes.
    /// Please consider using <see cref="VCheckListBox"/> instead of this simple control.
    /// </summary>
    /// <remarks>
    /// The <see cref="CheckListBox"/> control enables you to display a list of
    /// items to the user that the user can check by clicking.
    /// A <see cref="CheckListBox"/> control can provide single or
    /// multiple selections using the <see cref="ListBox.SelectionMode"/> property.
    /// The <see cref="Control.BeginUpdate"/> and <see cref="Control.EndUpdate"/>
    /// methods enable
    /// you to add a large number of items to the CheckListBox without the control
    /// being repainted each time an item is added to the list.
    /// The <see cref="ListControl.Items"/>, <see cref="ListBox.SelectedItems"/>,
    /// <see cref="ListBox.SelectedIndices"/>, and <see cref="CheckedIndices"/>
    /// properties provide access to the
    /// collections that are used by the <see cref="CheckListBox"/>.
    /// </remarks>
    [ControlCategory("Common")]
    public partial class CheckListBox : ListBox
    {
        private readonly HashSet<int> checkedIndices = new();

        /// <summary>
        /// Occurs when the checkbox state of the item has changed.
        /// </summary>
        public event EventHandler? CheckedChanged;

        /// <inheritdoc/>
        public override bool HasBorder
        {
            get
            {
                CheckDisposed();
                if (Handler is not NativeCheckListBoxHandler handler)
                    return true;
                return handler.NativeControl.HasBorder;
            }

            set
            {
                CheckDisposed();
                if (Handler is not NativeCheckListBoxHandler handler)
                    return;
                handler.NativeControl.HasBorder = value;
            }
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.CheckListBox;

        /// <summary>
        /// Gets a collection that contains the zero-based indexes of all
        /// currently checked items in the <see cref="CheckListBox"/>.
        /// </summary>
        /// <value>
        /// An <see cref="IReadOnlyList{T}"/> containing the indexes of the
        /// currently checked items in the control.
        /// If no items are currently checked, an empty
        /// <see cref="IReadOnlyList{T}"/> is returned.
        /// </value>
        [Browsable(false)]
        public IReadOnlyList<int> CheckedIndices
        {
            get
            {
                CheckDisposed();
                return checkedIndices.ToArray();
            }

            set
            {
                CheckDisposed();

                ClearCheckedCore();

                bool changed = false;
                foreach (var index in value)
                {
                    if (SetCheckedCore(index, true))
                        changed = true;
                }

                if (changed)
                    RaiseCheckedChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets a collection that contains the zero-based indexes of
        /// all currently checked items in the <see cref="CheckListBox"/>.
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
        public IReadOnlyList<int> CheckedIndicesDescending
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

        /// <summary>
        /// Gets or sets the zero-based index of the currently checked item
        /// in a <see cref="ListBox"/>.
        /// </summary>
        /// <value>A zero-based index of the currently checked item. A value
        /// of <c>null</c> is returned if no item is checked.</value>
        /// <exception cref="ArgumentOutOfRangeException">The assigned value
        /// is less than 0 or greater than or equal to the item count.</exception>
        [Browsable(false)]
        public int? CheckedIndex
        {
            get
            {
                CheckDisposed();
                return checkedIndices.FirstOrDefault();
            }

            set
            {
                CheckDisposed();

                if (value != null && (value < 0 || value >= Items.Count))
                    throw new ArgumentOutOfRangeException(nameof(value));

                ClearChecked();
                if (value != null)
                    SetChecked(value.Value, true);
            }
        }

        /// <summary>
        /// Gets a <see cref="CheckListBoxHandler"/> associated with this class.
        /// </summary>
        [Browsable(false)]
        internal new CheckListBoxHandler Handler
        {
            get
            {
                return (CheckListBoxHandler)base.Handler;
            }
        }

        /// <summary>
        /// Removes checked items from the <see cref="CheckListBox"/>.
        /// </summary>
        public void RemoveCheckedItems()
        {
            RemoveItems(CheckedIndicesDescending);
        }

        /// <summary>
        /// Checks items with specified indexes in the <see cref="CheckListBox"/>.
        /// </summary>
        public void CheckItems(params int[] indexes)
        {
            CheckedIndices = GetValidIndexes(indexes);
        }

        /// <inheritdoc/>
        public override void RemoveItems(IReadOnlyList<int> items)
        {
            if (items == null || items.Count == 0)
                return;

            BeginUpdate();
            try
            {
                ClearSelected();
                foreach (int index in items)
                {
                    if (index < Items.Count)
                    {
                        SetChecked(index, false);
                        Items.RemoveAt(index);
                    }
                }
            }
            finally
            {
                EndUpdate();
            }
        }

        /// <summary>
        /// Unchecks all items in the ListBox.
        /// </summary>
        public void ClearChecked()
        {
            ClearCheckedCore();
            RaiseCheckedChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Checks or clears the check state for the specified item in
        /// a <see cref="CheckListBox"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the item in a
        /// <see cref="CheckListBox"/> to set or clear the check state.</param>
        /// <param name="value"><c>true</c> to check the specified item;
        /// otherwise, false.</param>
        /// <exception cref="ArgumentOutOfRangeException">The specified
        /// index was outside the range of valid values.</exception>
        public void SetChecked(int index, bool value)
        {
            if (index < 0 || index >= Items.Count)
                throw new ArgumentOutOfRangeException(nameof(value));

            CheckDisposed();

            var changed = SetCheckedCore(index, value);

            if (changed)
                RaiseCheckedChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="CheckedChanged"/> event and calls
        /// <see cref="OnCheckedChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        public void RaiseCheckedChanged(EventArgs e)
        {
            OnCheckedChanged(e);
            CheckedChanged?.Invoke(this, e);
        }

        /// <inheritdoc/>
        internal override ControlHandler CreateHandler()
        {
            return GetEffectiveControlHandlerHactory().
                CreateCheckListBoxHandler(this);
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
        protected override void OnHandlerAttached(EventArgs e)
        {
            base.OnHandlerAttached(e);
        }

        private void ClearCheckedCore()
        {
            checkedIndices.Clear();
        }

        private bool SetCheckedCore(int index, bool value)
        {
            bool changed;
            if (value)
                changed = checkedIndices.Add(index);
            else
                changed = checkedIndices.Remove(index);

            return changed;
        }
    }
}