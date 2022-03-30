using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a combo box control.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A <see cref="ComboBox"/> displays a text box combined with a <see cref="ListBox"/>, which enables the user
    /// to select items from the list or enter a new value.
    /// The <see cref="IsEditable"/> property specifies whether the text portion can be edited.
    /// </para>
    /// <para>
    /// To add or remove objects in the list at run time, use methods of the <see cref="Collection{Object}" /> class
    /// (through the <see cref="ListControl.Items"/> property of the <see cref="ComboBox" />).
    /// The list then displays the default string value for each object. You can add individual objects with the <see cref="ICollection{Object}.Add"/> method.
    /// You can delete items with the <see cref="ICollection{Object}.Remove"/> method or clear the entire list with the <see cref="ICollection{Object}.Clear"/> method.
    /// </para>
    /// <para>
    /// In addition to display and selection functionality, the <see cref="ComboBox" /> also provides features that enable you to
    /// efficiently add items to the <see cref="ComboBox" /> and to find text within the items of the list. With the <see cref="Control.BeginUpdate"/>
    /// and <see cref="Control.EndUpdate"/> methods, you can add a large number of items to the <see cref="ComboBox" /> without the control
    /// being repainted each time an item is added to the list.
    /// </para>
    /// <para>
    /// You can use the <see cref="Text"/> property to specify the string displayed in the editing field,
    /// the <see cref="SelectedIndex"/> property to get or set the current item,
    /// and the <see cref="SelectedItem"/> property to get or set a reference to the selected object.
    /// </para>
    /// </remarks>
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
        /// You can create an event handler for this event to determine when the selected index in the <see cref="ComboBox"/> has been changed.
        /// This can be useful when you need to display information in other controls based on the current selection in the <see cref="ComboBox"/>.
        /// You can use the event handler for this event to load the information in the other controls.
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
        /// <exception cref="ArgumentNullException">The <c>value</c> is <c>null</c>.</exception>
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
        /// <remarks>This property indicates the zero-based index of the currently selected item in the combo box list.
        /// Setting a new index raises the <see cref="SelectedItemChanged"/> event.</remarks>
        /// <exception cref="ArgumentOutOfRangeException">The assigned value is less than 0 or greater than or equal to the item count.</exception>
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

                if (value != null && (value < 0 || value >= Items.Count))
                    throw new ArgumentOutOfRangeException(nameof(value));

                if (selectedIndex == value)
                    return;

                selectedIndex = value;

                SelectedItem = selectedIndex == null ? null : Items[selectedIndex.Value];

                var selectedItem = SelectedItem;
                Text = selectedItem == null ? string.Empty : GetItemText(selectedItem);

                RaiseSelectedItemChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Identifies the <see cref="SelectedItem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(
                "SelectedItem", // Property name
                typeof(object), // Property type
                typeof(ComboBox), // Property owner
                new FrameworkPropertyMetadata( // Property metadata
                        null, // default value
                        FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | // Flags
                            FrameworkPropertyMetadataOptions.AffectsPaint,
                        new PropertyChangedCallback(OnSelectedItemPropertyChanged),    // property changed callback
                        new CoerceValueCallback(CoerceSelectedItem),
                        true, // IsAnimationProhibited
                        UpdateSourceTrigger.PropertyChanged
                        //UpdateSourceTrigger.LostFocus   // DefaultUpdateSourceTrigger
                        ));

        /// <summary>
        /// Callback for changes to the SelectedItem property
        /// </summary>
        private static void OnSelectedItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ComboBox)d;
            control.OnSelectedItemPropertyChanged(e.OldValue, e.NewValue);
        }

        private void OnSelectedItemPropertyChanged(object oldValue, object newValue)
        {
            if (newValue == null)
            {
                SelectedIndex = null;
                return;
            }

            var index = Items.IndexOf(newValue);
            if (index != -1)
                SelectedIndex = index;
        }

        /// <summary>
        /// Gets or sets currently selected item in the combo box.
        /// </summary>
        /// <value>The object that is the currently selected item or <c>null</c> if there is no currently selected item.</value>
        /// <remarks>
        /// When you set the <see cref="SelectedItem"/> property to an object, the <see cref="ComboBox"/> attempts to
        /// make that object the currently selected one in the list.
        /// If the object is found in the list, it is displayed in the edit portion of the <see cref="ComboBox"/> and
        /// the <see cref="SelectedIndex"/> property is set to the corresponding index.
        /// If the object does not exist in the list, the <see cref="SelectedIndex"/> property is left at its current value.
        /// </remarks>
        public object? SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        private static object CoerceSelectedItem(DependencyObject d, object value) => value;

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