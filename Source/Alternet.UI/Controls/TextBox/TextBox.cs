using System;
using System.ComponentModel;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that can be used to display or edit unformatted text.
    /// </summary>
    /// <remarks>
    /// With the <see cref="TextBox"/> control, the user can enter text in
    /// an application.
    /// </remarks>
    public class TextBox : Control
    {
        /// <summary>
        /// Identifies the <see cref="Text"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(
                "Text",
                typeof(string),
                typeof(TextBox),
                new FrameworkPropertyMetadata(
                        string.Empty,
                        FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsPaint,
                        new PropertyChangedCallback(OnTextPropertyChanged),
                        new CoerceValueCallback(CoerceText),
                        true, // IsAnimationProhibited
                        UpdateSourceTrigger.PropertyChanged));

        /// <summary>
        /// Event for "Text has changed"
        /// </summary>
        public static readonly RoutedEvent TextChangedEvent =
            EventManager.RegisterRoutedEvent(
            "TextChanged",
            RoutingStrategy.Bubble,
            typeof(TextChangedEventHandler),
            typeof(TextBox));

        private bool hasBorder = true;
        private bool multiline = false;
        private bool readOnly = false;
        private bool isRichEdit = false;

        /// <summary>
        /// Occurs when <see cref="Multiline"/> property value changes.
        /// </summary>
        public event EventHandler? MultilineChanged;

        /// <summary>
        /// Occurs when <see cref="ReadOnly"/> property value changes.
        /// </summary>
        public event EventHandler? ReadOnlyChanged;

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
        /// Occurs when <see cref="HasBorder"/> property value changes.
        /// </summary>
        public event EventHandler? HasBorderChanged;

        /// <summary>
        /// Gets or sets the text contents of the text box.
        /// </summary>
        /// <value>A string containing the text contents of the text box. The
        /// default is an empty string ("").</value>
        /// <remarks>
        /// Getting this property returns a string copy of the contents of the
        /// text box. Setting this property replaces the contents of the text box
        /// with the specified string.
        /// </remarks>
        [DefaultValue("")]
        [Localizability(LocalizationCategory.Text)]
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public bool IsRichEdit
        {
            get
            {
                return isRichEdit;
            }

            set
            {
                if (isRichEdit == value)
                    return;
                isRichEdit = value;
                Handler.IsRichEdit = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether text in the text box is read-only.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the text box is read-only; otherwise,
        /// <see langword="false"/>. The default is <see
        /// langword="false"/>.
        /// </value>
        /// <remarks>
        /// When this property is set to <see langword="true"/>, the contents
        /// of the control cannot be changed by the user at runtime.
        /// With this property set to <see langword="true"/>, you can still
        /// set the value of the <see cref="Text"/> property in code.
        /// </remarks>
        public bool ReadOnly
        {
            get
            {
                CheckDisposed();
                return readOnly;
            }

            set
            {
                CheckDisposed();
                if (readOnly == value)
                    return;

                readOnly = value;
                ReadOnlyChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this is a multiline text
        /// box control.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the control is a multiline text box
        /// control; otherwise, <see langword="false"/>. The default
        /// is <see langword="false"/>.
        /// </value>
        public bool Multiline
        {
            get
            {
                CheckDisposed();
                return multiline;
            }

            set
            {
                CheckDisposed();
                if (multiline == value)
                    return;

                multiline = value;
                MultilineChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control has a border.
        /// </summary>
        public bool HasBorder
        {
            get
            {
                CheckDisposed();
                return hasBorder;
            }

            set
            {
                CheckDisposed();
                if (hasBorder == value)
                    return;

                hasBorder = value;
                HasBorderChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        internal new NativeTextBoxHandler Handler =>
            (NativeTextBoxHandler)base.Handler;

        /// <summary>
        /// Raises the <see cref="TextChanged"/> event and calls
        /// <see cref="OnTextChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        public void RaiseTextChanged(TextChangedEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnTextChanged(e);
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
        /// Called when the value of the <see cref="Text"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        protected virtual void OnTextChanged(EventArgs e)
        {
        }

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            //if (!hasBorder)
                return new NativeTextBoxHandler();

            //return GetEffectiveControlHandlerHactory().CreateTextBoxHandler(this);
        }        

        private static object CoerceText(DependencyObject d, object value) =>
            value ?? string.Empty;

        /// <summary>
        /// Callback for changes to the Text property
        /// </summary>
        private static void OnTextPropertyChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            TextBox textBox = (TextBox)d;
            textBox.OnTextPropertyChanged((string)e.OldValue, (string)e.NewValue);
        }

#pragma warning disable IDE0060 // Remove unused parameter
        private void OnTextPropertyChanged(string oldText, string newText)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            OnTextChanged(new TextChangedEventArgs(TextChangedEvent));
        }
    }
}