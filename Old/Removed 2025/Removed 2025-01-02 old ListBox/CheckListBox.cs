using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control to display a list of items with checkboxes.
    /// Please consider using <see cref="VirtualCheckListBox"/> instead of this simple control.
    /// </summary>
    /// <remarks>
    /// The <see cref="CheckListBox"/> control enables you to display a list of
    /// items to the user that the user can check by clicking.
    /// A <see cref="CheckListBox"/> control can provide single or
    /// multiple selections using the <see cref="CustomListBox{T}.SelectionMode"/> property.
    /// The <see cref="AbstractControl.BeginUpdate"/> and <see cref="AbstractControl.EndUpdate"/>
    /// methods enable
    /// you to add a large number of items to the CheckListBox without the control
    /// being repainted each time an item is added to the list.
    /// The <see cref="ListControl{T}.Items"/>, <see cref="CustomListBox{T}.SelectedItems"/>,
    /// <see cref="CustomListBox{T}.SelectedIndices"/>, and <see cref="CheckedIndices"/>
    /// properties provide access to the
    /// collections that are used by the <see cref="CheckListBox"/>.
    /// </remarks>
    [ControlCategory("Common")]
    public partial class CheckListBox : ListBox, ICheckListBox<object>
    {
        private readonly HashSet<int> checkedIndices = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckListBox"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public CheckListBox(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckListBox"/> class.
        /// </summary>
        public CheckListBox()
        {
        }

        /// <summary>
        /// Occurs when the checkbox state of the item has changed.
        /// </summary>
        public event EventHandler? CheckedChanged;

        /// <inheritdoc/>
        public override bool HasBorder
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return Handler.HasBorder;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.HasBorder = value;
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
        public virtual IReadOnlyList<int> CheckedIndices
        {
            get
            {
                if (DisposingOrDisposed)
                    return [];
                return checkedIndices.ToArray();
            }

            set
            {
                if (DisposingOrDisposed)
                    return;

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
                return checkedIndices.FirstOrDefault();
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

        [Browsable(false)]
        internal new string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        /// <summary>
        /// Gets a <see cref="ICheckListBoxHandler"/> associated with this class.
        /// </summary>
        [Browsable(false)]
        internal new ICheckListBoxHandler Handler
        {
            get
            {
                return (ICheckListBoxHandler)base.Handler;
            }
        }

        /// <summary>
        /// Removes checked items from the <see cref="CheckListBox"/>.
        /// </summary>
        public virtual void RemoveCheckedItems()
        {
            RemoveItems(CheckedIndicesDescending);
        }

        /// <summary>
        /// Checks items with specified indexes in the <see cref="CheckListBox"/>.
        /// </summary>
        public virtual void CheckItems(params int[] indexes)
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
        public virtual void ClearChecked()
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
        public virtual void SetChecked(int index, bool value)
        {
            if (index < 0 || index >= Items.Count)
                return;

            if(DisposingOrDisposed)
                return;

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
            if (DisposingOrDisposed)
                return;
            OnCheckedChanged(e);
            CheckedChanged?.Invoke(this, e);
        }

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return ControlFactory.Handler.CreateCheckListBoxHandler(this);
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
            if (DisposingOrDisposed)
                return;
            checkedIndices.Clear();
        }

        private bool SetCheckedCore(int index, bool value)
        {
            if (DisposingOrDisposed)
                return false;
            bool changed;
            if (value)
                changed = checkedIndices.Add(index);
            else
                changed = checkedIndices.Remove(index);

            return changed;
        }
    }
}