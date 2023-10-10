using System;
using System.ComponentModel;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a url label control.
    /// </summary>
    [DefaultProperty("Text")]
    [DefaultBindingProperty("Text")]
    [ControlCategory("Other")]
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
                        PropMetadataOption.AffectsLayout | PropMetadataOption.AffectsPaint,
                        new PropertyChangedCallback(OnTextPropertyChanged),
                        new CoerceValueCallback(CoerceText)));

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
        /// Initializes a new instance of the <see cref="LinkLabel"/> class with the specified text.
        /// </summary>
        public LinkLabel(string text)
            : base()
        {
            Text = text;
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
        /// Gets or sets whether to use generic or native control for <see cref="LinkLabel"/>.
        /// </summary>
        public static bool UseGenericControl
        {
            get => Native.LinkLabel.UseGenericControl;
            set => Native.LinkLabel.UseGenericControl = value;
        }

        /// <summary>
        /// Gets or sets whether to use our <see cref="AppUtils.ShellExecute"/>
        /// for opening the <see cref="Url"/> or allow native control to open it.
        /// </summary>
        /// <remarks>
        /// On Linux (Ubuntu 23) native control has errors with opening urls, so
        /// this property is <c>true</c> by default and we do not use it.
        /// </remarks>
        public static bool UseShellExecute { get; set; } = true;

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

        /// <summary>
        /// Gets or sets the color used to draw the label of the hyperlink when the mouse is
        /// over the control.
        /// </summary>
        public Color HoverColor
        {
            get => Handler.HoverColor;
            set => Handler.HoverColor = value;
        }

        /// <summary>
        /// Gets or sets the color used to draw the label when the link has never been
        /// clicked before (i.e. the link has not been visited) and the mouse is
        /// not over the control.
        /// </summary>
        public Color NormalColor
        {
            get => Handler.NormalColor;
            set => Handler.NormalColor = value;
        }

        /// <summary>
        /// Gets or sets the color used to draw the label when the mouse is not over the
        /// control and the link has already been clicked
        /// before (i.e. the link has been visited).
        /// </summary>
        public Color VisitedColor
        {
            get => Handler.VisitedColor;
            set => Handler.VisitedColor = value;
        }

        /// <summary>
        /// Gets or sets whether the hyperlink has already been clicked by
        /// the user at least one time.
        /// </summary>
        public bool Visited
        {
            get => Handler.Visited;
            set => Handler.Visited = value;
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
            if (!e.Cancel && UseShellExecute)
            {
                e.Cancel = true;
                AppUtils.ShellExecute(Url);
            }
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