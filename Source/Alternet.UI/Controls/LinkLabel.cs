using System;
using System.ComponentModel;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a url label control.
    /// </summary>
    public class LinkLabel : Control
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkLabel"/> class.
        /// </summary>
        public LinkLabel()
            : base()
        {
        }

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

        internal new NativeLinkLabelHandler Handler =>
            (NativeLinkLabelHandler)base.Handler;

        public string Url
        {
            get { return Handler.Url; }
            set { Handler.Url = value; }
        }

        protected override ControlHandler CreateHandler()
        {
            return new NativeLinkLabelHandler();
        }

        /// <summary>
        /// Event for "Text has changed"
        /// </summary>
        public static readonly RoutedEvent TextChangedEvent = EventManager.RegisterRoutedEvent(
            "TextChanged",
            RoutingStrategy.Bubble,
            typeof(TextChangedEventHandler),
            typeof(LinkLabel));

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
                typeof(LinkLabel), // Property owner
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
            LinkLabel label = (LinkLabel)d;
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
        /// Raises the <see cref="TextChanged"/> event and calls 
        /// <see cref="OnTextChanged(TextChangedEventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        public void RaiseTextChanged(TextChangedEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnTextChanged(e);
        }

        private static object CoerceText(DependencyObject d, object value)
            => value ?? string.Empty;
    }
}