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
        /// Identifies the <see cref="Text"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(
                "Text",
                typeof(string),
                typeof(LinkLabel),
                new FrameworkPropertyMetadata(
                        string.Empty,
                        FrameworkPropertyMetadataOptions.AffectsLayout | FrameworkPropertyMetadataOptions.AffectsPaint,
                        new PropertyChangedCallback(OnTextPropertyChanged),
                        new CoerceValueCallback(CoerceText)
                        ));

        /// <summary>
        /// Event for "Text has changed"
        /// </summary>
        public static readonly RoutedEvent TextChangedEvent =
            EventManager.RegisterRoutedEvent(
                "TextChanged",
                RoutingStrategy.Bubble,
                typeof(TextChangedEventHandler),
                typeof(LinkLabel));

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkLabel"/> class.
        /// </summary>
        public LinkLabel()
            : base()
        {
        }

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
        /// Occurs when a link is clicked within the control.
        /// </summary>
        public event CancelEventHandler? LinkClicked;

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

        /// <inheritdoc/>
        public override ControlId ControlKind => ControlId.LinkLabel;

        /// <summary>
        /// Gets or sets the URL associated with the hyperlink.
        /// </summary>
        public string Url
        {
            get { return Handler.Url; }
            set { Handler.Url = value; }
        }

        internal new NativeLinkLabelHandler Handler =>
            (NativeLinkLabelHandler)base.Handler;

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

        internal void RaiseLinkClicked(CancelEventArgs e)
        {
            OnLinkClicked(e);
            if (!e.Cancel)
                LinkClicked?.Invoke(this, e);
        }

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return new NativeLinkLabelHandler();
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
        /// Called when a link is clicked within the control.
        /// </summary>
        /// <param name="e">
        /// An <see cref="CancelEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnLinkClicked(CancelEventArgs e)
        {
        }

        private static object CoerceText(DependencyObject d, object value)
            => value ?? string.Empty;

        /// <summary>
        /// Callback for changes to the Text property
        /// </summary>
        private static void OnTextPropertyChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            LinkLabel label = (LinkLabel)d;
            label.OnTextPropertyChanged((string)e.OldValue, (string)e.NewValue);
        }

#pragma warning disable IDE0060 // Remove unused parameter
        private void OnTextPropertyChanged(string oldText, string newText)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            OnTextChanged(new TextChangedEventArgs(TextChangedEvent));
        }
    }
}