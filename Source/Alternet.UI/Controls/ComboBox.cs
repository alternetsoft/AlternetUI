using System;
using System.Collections.Generic;
using System.Globalization;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a combo box control.
    /// </summary>
    public class ComboBox : ListControl
    {
        private string text = "";

        private int? selectedIndex;

        private bool isEditable = true;

        public event EventHandler? SelectedItemChanged;

        public event EventHandler? TextChanged;

        public event EventHandler? IsEditableChanged;

        /// <summary>
        /// Gets or sets the text displayed in the <see cref="ComboBox"/>.
        /// </summary>
        /// <remarks>
        /// Setting the <see cref="Text"/> property to an empty string ("") sets the <see cref="SelectedIndex"/> to <c>null</c>.
        /// Setting the <see cref="Text"/> property to a value that is in the <see cref="ListControl.Items"/> collection sets the
        /// <see cref="SelectedIndex"/> to the index of that item.
        /// Setting the <see cref="Text"/> property to a value that is not in the collection leaves the <see cref="SelectedIndex"/> unchanged.
        /// Reading the <see cref="Text"/> property returns the text of <see cref="SelectedItem"/>, if it is not <c>null</c>.
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
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                CheckDisposed();

                if (text == value)
                    return;

                text = value;

                if (text == string.Empty)
                    SelectedIndex = null;

                var foundIndex = FindStringExact(text);
                if (foundIndex != null)
                    SelectedIndex = foundIndex.Value;

                InvokeTextChanged(EventArgs.Empty);
            }
        }

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

                var selectedItem = SelectedItem;
                Text = selectedItem == null ? string.Empty : GetItemText(selectedItem);

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
                    SelectedIndex = null;
                    return;
                }

                var index = Items.IndexOf(value);
                if (index != null)
                    SelectedIndex = index;
            }
        }

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

        /// <summary>
        /// Finds the first item in the combo box that matches the specified string.
        /// </summary>
        /// <param name="s">The string to search for.</param>
        /// <returns>The zero-based index of the first item found; returns <c>null</c> if no match is found.</returns>
        public int? FindStringExact(string s)
        {
            // todo: add other similar methods: FindString and overloads.
            return FindStringInternal(s, Items, startIndex: null, exact: true, ignoreCase: true);
        }

        public void InvokeSelectedItemChanged(EventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnSelectedItemChanged(e);
            SelectedItemChanged?.Invoke(this, e);
        }

        public void InvokeTextChanged(EventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnTextChanged(e);
            TextChanged?.Invoke(this, e);
        }

        protected virtual void OnSelectedItemChanged(EventArgs e)
        {
        }

        protected virtual void OnTextChanged(EventArgs e)
        {
        }

        private int? FindStringInternal(string str, IList<object> items, int? startIndex, bool exact, bool ignoreCase)
        {
            if (str is null)
            {
                return null;
            }

            if (items is null || items.Count == 0)
            {
                return null;
            }

            var startIndexInt = startIndex ?? -1;

            if (startIndexInt < -1 || startIndexInt >= items.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndexInt));
            }

            // Start from the start index and wrap around until we find the string
            // in question. Use a separate counter to ensure that we arent cycling through the list infinitely.
            int numberOfTimesThroughLoop = 0;

            // this API is really Find NEXT String...
            for (int index = (startIndexInt + 1) % items.Count; numberOfTimesThroughLoop < items.Count; index = (index + 1) % items.Count)
            {
                numberOfTimesThroughLoop++;

                bool found;
                if (exact)
                {
                    found = string.Compare(str, GetItemText(items[index]), ignoreCase, CultureInfo.CurrentCulture) == 0;
                }
                else
                {
                    found = string.Compare(str, 0, GetItemText(items[index]), 0, str.Length, ignoreCase, CultureInfo.CurrentCulture) == 0;
                }

                if (found)
                {
                    return index;
                }
            }

            return null;
        }
    }
}