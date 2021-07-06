using System;
using System.Collections.Generic;
using System.Linq;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a combo box control.
    /// </summary>
    public class ComboBox : ListControl
    {
        private string text = "";

        /// <summary>
        /// Gets or sets the text displayed in the <see cref="ComboBox"/>.
        /// </summary>
        /// <remarks>
        /// Setting the Text property to null or an empty string ("") sets the SelectedIndex to -1.
        /// Setting the Text property to a value that is in the Items collection sets the SelectedIndex to the index of that item.
        /// Setting the Text property to a value that is not in the collection leaves the SelectedIndex unchanged.
        /// Reading the Text property returns the text of SelectedItem, if it is not null.
        /// </remarks>
        public string Text
        {
            get
            {
                CheckDisposed();
                return text;
            }

            set
            {
                CheckDisposed();
                if (text == value)
                    return;

                text = value;
                InvokeTextChanged(EventArgs.Empty);
            }
        }

        int? selectedIndex;

        /// <summary>
        /// Gets or sets the index specifying the currently selected item.
        /// </summary>
        /// <value>A zero-based index of the currently selected item. A value of <c>null</c> is returned if no item is selected.</value>
        public int? SelectedIndex
        {
            get
            {
                CheckDisposed();
                return selectedIndex;
            }

            set
            {
                CheckDisposed();

                if (selectedIndex == value)
                    return;

                selectedIndex = value;

                InvokeSelectedItemChanged(EventArgs.Empty);
            }
        }

        public object? SelectedItem
        {
            get
            {
                CheckDisposed();
                
                if (SelectedIndex == null)
                    return null;

                return Items[SelectedIndex.Value];
            }

            set
            {
                CheckDisposed();

                if (value == null)
                {
                    SelectedIndex = -1;
                    return;
                }

                var index = Items.IndexOf(value);
                if (index != -1)
                    SelectedIndex = index;
            }
        }

        public void InvokeSelectedItemChanged(EventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnSelectedItemChanged(e);
            SelectedItemChanged?.Invoke(this, e);
        }

        protected virtual void OnSelectedItemChanged(EventArgs e)
        {
        }

        public event EventHandler? SelectedItemChanged;

        public void InvokeTextChanged(EventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnTextChanged(e);
            TextChanged?.Invoke(this, e);
        }

        protected virtual void OnTextChanged(EventArgs e)
        {
        }

        public event EventHandler? TextChanged;

        public event EventHandler? IsEditableChanged;

        bool isEditable = true;

        public bool IsEditable
        {
            get
            {
                CheckDisposed();
                return isEditable;
            }

            set
            {
                CheckDisposed();

                if (isEditable == value)
                    return;

                isEditable = value;

                IsEditableChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}