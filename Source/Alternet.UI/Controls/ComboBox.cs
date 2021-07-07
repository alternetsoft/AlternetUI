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

        /// <summary>
        /// Occurs when the <see cref="SelectedItem"/> property value changes.
        /// </summary>
        /// <remarks>
        /// This event is raised if the <see cref="SelectedItem"/> property is changed by either a programmatic modification or user interaction.
        /// </remarks>
        public event EventHandler? SelectedItemChanged;

        /// <summary>
        /// Occurs when the <see cref="Text"/> property value changes.
        /// </summary>
        /// <remarks>
        /// This event is raised if the <see cref="Text"/> property is changed by either a programmatic modification or user interaction.
        /// </remarks>
        public event EventHandler? TextChanged;

        /// <summary>
        /// Occurs when the <see cref="IsEditable"/> property value changes.
        /// </summary>
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

                RaiseTextChanged(EventArgs.Empty);
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

                RaiseSelectedItemChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets currently selected item in the combo box.
        /// </summary>
        /// <value>The object that is the currently selected item or <c>null</c> if there is no currently selected item.</value>
        /// <remarks>
        /// When you set the <see cref="SelectedItem"/> property to an object, the <see cref="ComboBox"/> attempts to make that object the currently selected one in the list.
        /// If the object is found in the list, it is displayed in the edit portion of the ComboBox and the SelectedIndex property is set to the corresponding index.
        /// If the object does not exist in the list, the SelectedIndex property is left at its current value.
        /// </remarks>
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
                if (index != -1)
                    SelectedIndex = index;
            }
        }

        /// <summary>
        /// Gets or a value that enables or disables editing of the text in text box area of the <see cref="ComboBox"/>.
        /// </summary>
        /// <value><c>true</c> if the <see cref="ComboBox"/> can be edited; otherwise <c>false</c>. The default is <c>false</c>.</value>
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

        /// <summary>
        /// Raises the <see cref="SelectedItemChanged"/> event and calls <see cref="OnSelectedItemChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        public void RaiseSelectedItemChanged(EventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnSelectedItemChanged(e);
            SelectedItemChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="TextChanged"/> event and calls <see cref="OnTextChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        public void RaiseTextChanged(EventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnTextChanged(e);
            TextChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Called when the value of the <see cref="SelectedItem"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnSelectedItemChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the value of the <see cref="Text"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
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