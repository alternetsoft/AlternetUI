using System;
using System.ComponentModel;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a text label control.
    /// </summary>
    /// <remarks>
    /// <see cref="Label" /> controls are typically used to provide descriptive text for a control.
    /// For example, you can use a <see cref="Label" /> to add descriptive text for a <see cref="TextBox"/> control to inform the
    /// user about the type of data expected in the control. <see cref="Label" /> controls can also be used
    /// to add descriptive text to a <see cref="Window"/> to provide the user with helpful information.
    /// For example, you can add a <see cref="Label" /> to the top of a <see cref="Window"/> that provides instructions
    /// to the user on how to input data in the controls on the form. <see cref="Label" /> controls can be
    /// also used to display run time information on the status of an application. For example,
    /// you can add a <see cref="Label" /> control to a form to display the status of each file as a list of files is processed.
    /// </remarks>
    public class Label : Control
    {
        /// <summary>
        /// Gets or sets the text displayed on this label.
        /// </summary>
        [DefaultValue("")]
        [Localizability(LocalizationCategory.Text)]
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Event for "Text has changed"
        /// </summary>
        public static readonly RoutedEvent TextChangedEvent = EventManager.RegisterRoutedEvent(
            "TextChanged", // Event name
            RoutingStrategy.Bubble, //
            typeof(TextChangedEventHandler), //
            typeof(Label)); //

        /// <summary>
        /// Occurs when the value of the <see cref="Text"/> property changes.
        /// </summary>
        public event TextChangedEventHandler TextChanged
        {
            add
            {
                AddHandler(TextChangedEvent, value);
            }

            remove
            {
                RemoveHandler(TextChangedEvent, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="Text"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(
                "Text", // Property name
                typeof(string), // Property type
                typeof(Label), // Property owner
                new FrameworkPropertyMetadata( // Property metadata
                        string.Empty, // default value
                        FrameworkPropertyMetadataOptions.AffectsLayout | FrameworkPropertyMetadataOptions.AffectsPaint,// Flags
                        new PropertyChangedCallback(OnTextPropertyChanged),    // property changed callback
                        new CoerceValueCallback(CoerceText)
                        ));

        /// <summary>
        /// Callback for changes to the Text property
        /// </summary>
        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Label label = (Label)d;
            label.OnTextPropertyChanged((string)e.OldValue, (string)e.NewValue);
        }

        private void OnTextPropertyChanged(string oldText, string newText)
        {
            OnTextChanged(new TextChangedEventArgs(TextChangedEvent));
        }

        /// <summary>
        /// Called when content in this Control changes.
        /// Raises the TextChanged event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnTextChanged(TextChangedEventArgs e)
        {
            RaiseEvent(e);
        }

        /// <summary>
        /// Raises the <see cref="TextChanged"/> event and calls <see cref="OnTextChanged(TextChangedEventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        public void RaiseTextChanged(TextChangedEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnTextChanged(e);
        }

        private static object CoerceText(DependencyObject d, object value) => value == null ? string.Empty : value;
    }
}