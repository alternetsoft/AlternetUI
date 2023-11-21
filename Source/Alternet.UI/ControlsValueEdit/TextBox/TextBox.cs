using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that can be used to display or edit unformatted text.
    /// </summary>
    /// <remarks>
    /// With the <see cref="TextBox"/> control, the user can enter text in
    /// an application.
    /// </remarks>
    [DefaultEvent("TextChanged")]
    [DefaultBindingProperty("Text")]
    [ControlCategory("Common")]
    public class TextBox : CustomTextBox, ISimpleRichTextBox
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
                        PropMetadataOption.BindsTwoWayByDefault | PropMetadataOption.AffectsPaint,
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

        private bool isRichEdit = false;
        private bool multiline = false;
        private bool hasBorder = true;
        private bool readOnly = false;
        private TextBoxTextWrap textWrap;
        private GenericAlignment textAlign;
        private IValueValidator? validator;

        static TextBox()
        {
            var choices = PropertyGrid.CreateChoices();
            choices.Add(GenericAlignment.Left);
            choices.Add(GenericAlignment.Right);
            choices.Add(GenericAlignment.CenterHorizontal);
            var prm = PropertyGrid.GetNewItemParams(typeof(TextBox), nameof(TextBox.TextAlign));
            prm.EnumIsFlags = false;
            prm.Choices = choices;

            var useErrorColors = Application.IsWindowsOS;
            DefaultErrorUseForegroundColor = useErrorColors;
            DefaultErrorUseBackgroundColor = useErrorColors;
        }

        /// <summary>
        /// Occurs when <see cref="Multiline"/> property value changes.
        /// </summary>
        public event EventHandler? MultilineChanged;

        /// <summary>
        /// Occurs when <see cref="ReadOnly"/> property value changes.
        /// </summary>
        public event EventHandler? ReadOnlyChanged;

        /// <summary>
        /// Occurs when <see cref="CurrentPosition"/> property value changes.
        /// </summary>
        /// <remarks>
        /// You need to call <see cref="IdleAction"/> in the
        /// <see cref="Application.Idle"/> event handler in order to enable
        /// <see cref="CurrentPositionChanged"/> event firing.
        /// </remarks>
        public event EventHandler? CurrentPositionChanged;

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
        /// Occurs when Enter key is pressed in the control.
        /// </summary>
        /// <remarks>
        /// Event is raised only if <see cref="ProcessEnter"/> is true.
        /// </remarks>
        public event EventHandler? EnterPressed;

        /// <summary>
        /// Occurs when url clicked in the text.
        /// </summary>
        /// <remarks>
        /// This event is not fired on MacOs. On this os, url is
        /// automatically opened in the default browser.
        /// </remarks>
        public event UrlEventHandler? TextUrl;

        /// <summary>
        /// Occurs when maximal text length is reached.
        /// </summary>
        /// <remarks>
        /// Use <see cref="MaxLength"/> to set maximal text length.
        /// </remarks>
        public event EventHandler? TextMaxLength;

        /// <summary>
        /// Occurs when <see cref="HasBorder"/> property value changes.
        /// </summary>
        public event EventHandler? HasBorderChanged;

        /// <summary>
        /// Gets or sets default value of the <see cref="AutoUrlOpen"/> property.
        /// </summary>
        public static bool DefaultAutoUrlOpen { get; set; } = false;

        /// <summary>
        /// Gets or sets default value of the <see cref="AutoUrlModifiers"/> property.
        /// </summary>
        /// <remarks>
        /// If this is not assigned (default),
        /// <see cref="PlatformDefaults.TextBoxUrlClickModifiers"/> is used as property default.
        /// </remarks>
        public static ModifierKeys? DefaultAutoUrlModifiers { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether urls in the input text
        /// are opened in the default browser.
        /// </summary>
        /// <remarks>
        /// Use <see cref="AutoUrl"/> in order to highlight and underline urls.
        /// </remarks>
        public virtual bool AutoUrlOpen { get; set; } = DefaultAutoUrlOpen;

        /// <summary>
        /// Gets or sets <see cref="ModifierKeys"/> used when clicked url is autoimatically opened
        /// in the browser when <see cref="AutoUrlOpen"/> is <c>true</c>.
        /// </summary>
        public virtual ModifierKeys? AutoUrlModifiers { get; set; }

        /// <summary>
        /// Gets or sets <see cref="IValueValidator"/> for the <see cref="TextBox"/> control.
        /// </summary>
        /// <remarks>
        /// <see cref="IValueValidator"/> allows to set limitations on possible values of
        /// the <see cref="Text"/> property. See <see cref="IValueValidatorText"/> and
        /// <see cref="ValueValidatorFactory.CreateValueValidatorText"/>.
        /// </remarks>
        [Browsable(false)]
        public virtual IValueValidator? Validator
        {
            get
            {
                return validator;
            }

            set
            {
                if (validator == value)
                    return;
                validator = value;
                if (validator == null)
                    Handler.NativeControl.Validator = IntPtr.Zero;
                else
                    Handler.NativeControl.Validator = validator.Handle;
            }
        }

        /// <inheritdoc/>
        [DefaultValue("")]
        [Localizability(LocalizationCategory.Text)]
        public override string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }

            set
            {
                SetValue(TextProperty, value);
            }
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.TextBox;

        /// <summary>
        /// Gets or sets a value indicating whether this is a multiline text
        /// box control.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the control is a multiline text box
        /// control; otherwise, <see langword="false"/>. The default
        /// is <see langword="false"/>.
        /// </value>
        public virtual bool Multiline
        {
            get
            {
                return multiline;
            }

            set
            {
                if (multiline == value)
                    return;
                multiline = value;
                MultilineChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether text control is in rich edit mode
        /// </summary>
        /// <remarks>
        /// In the rich edit mode it is possible to apply text formatting (for example
        /// change text font or color). Also it is possible to edit large texts.
        /// </remarks>
        public virtual bool IsRichEdit
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
        /// Gets or sets text wrap style fot the multiline <see cref="TextBox"/> controls.
        /// </summary>
        /// <remarks>
        /// Default value is TextBoxTextWrap.Best. Some platforms do not support all wrap styles.
        /// </remarks>
        [DefaultValue(TextBoxTextWrap.Best)]
        public virtual TextBoxTextWrap TextWrap
        {
            get
            {
                return textWrap;
            }

            set
            {
                if (textWrap == value)
                    return;
                textWrap = value;
                Handler.TextWrap = value;
            }
        }

        /// <summary>
        /// Gets or sets text alignment for the <see cref="TextBox"/> control.
        /// </summary>
        /// <remarks>
        /// Only <see cref="GenericAlignment.Left"/>, <see cref="GenericAlignment.Right"/>
        /// and <see cref="GenericAlignment.CenterHorizontal"/> are supported.
        /// </remarks>
        /// <remarks>
        /// Default value is <see cref="GenericAlignment.Left"/>.
        /// </remarks>
        [DefaultValue(GenericAlignment.Left)]
        public virtual GenericAlignment TextAlign
        {
            get
            {
                return textAlign;
            }

            set
            {
                if (textAlign == value)
                    return;
                textAlign = value;
                Handler.TextAlign = value;
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
        public virtual bool ReadOnly
        {
            get
            {
                return readOnly;
            }

            set
            {
                if (readOnly == value)
                    return;

                readOnly = value;
                ReadOnlyChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <inheritdoc/>
        public override int MaxLength
        {
            get
            {
                return base.MaxLength;
            }

            set
            {
                if (MaxLength == value || value < 0)
                    return;
                base.MaxLength = value;
                if (Options.HasFlag(TextBoxOptions.SetNativeMaxLength))
                    Handler.SetMaxLength((ulong)value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control has a border.
        /// </summary>
        public virtual bool HasBorder
        {
            get
            {
                return hasBorder;
            }

            set
            {
                if (hasBorder == value)
                    return;
                hasBorder = value;
                HasBorderChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets whether control has selected text.
        /// </summary>
        [Browsable(false)]
        public virtual bool HasSelection
        {
            get
            {
                return Handler.HasSelection;
            }
        }

        /// <summary>
        /// Gets or sets whether text is modified in the control.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsModified
        {
            get
            {
                return Handler.IsModified;
            }

            set
            {
                Handler.IsModified = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current selection can be copied,
        /// which allows the<see cref="Copy"/> method to succeed.
        /// </summary>
        /// <returns>
        /// <see langword = "true" /> if the current selection can be copied;
        /// otherwise, <see langword = "false" />.
        /// </returns>
        [Browsable(false)]
        public virtual bool CanCopy
        {
            get
            {
                return Handler.CanCopy;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current selection can be cut,
        /// which allows the<see cref="Cut"/> method to succeed.
        /// </summary>
        /// <returns>
        /// <see langword = "true" /> if the current selection can be cut;
        /// otherwise, <see langword = "false" />.
        /// </returns>
        [Browsable(false)]
        public virtual bool CanCut
        {
            get
            {
                return Handler.CanCut;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current selection can be replaced with
        /// the contents of the Clipboard, which allows
        /// the<see cref="Paste"/> method to succeed.
        /// </summary>
        /// <returns>
        /// <see langword = "true" /> if the data can be pasted;
        /// otherwise, <see langword = "false" />.
        /// </returns>
        [Browsable(false)]
        public virtual bool CanPaste
        {
            get
            {
                return Handler.CanPaste;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the user can redo the previous operation
        /// in the control.
        /// </summary>
        /// <returns>
        /// <see langword = "true" /> if the user can redo the previous
        /// operation performed
        /// in the control; otherwise, <see langword = "false" />.
        /// </returns>
        [Browsable(false)]
        public virtual bool CanRedo
        {
            get
            {
                return Handler.CanRedo;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the selected text
        /// in the text box control remains highlighted when the
        /// control loses focus.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if the selected text does not appear
        /// highlighted when the text box control loses focus;
        /// <see langword="false" />, if the selected text remains
        /// highlighted when the text box control loses focus.
        /// The default is <see langword="true" />.
        /// </returns>
        public virtual bool HideSelection
        {
            get
            {
                return Handler.HideSelection;
            }

            set
            {
                Handler.HideSelection = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the TAB key is received and
        /// processed by the control.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if the TAB key is received by the control;
        /// <see langword="false" />, if the TAB key is not received by the control.
        /// The default is <see langword="false" />.
        /// </returns>
        /// <remarks>
        /// Normally, TAB key is used for passing to the next control in a dialog.
        /// For the control created with this style, you can still use
        /// Ctrl-Enter to pass to the next control from the keyboard.
        /// </remarks>
        public virtual bool ProcessTab
        {
            get
            {
                return Handler.ProcessTab;
            }

            set
            {
                Handler.ProcessTab = value;
            }
        }

        /// <summary>
        /// Gets the line number (zero-based) with the cursor.
        /// </summary>
        [Browsable(false)]
        public virtual long InsertionPointLineNumber
        {
            get
            {
                var currentPosition = GetInsertionPoint();
                var caretLineNumber = PositionToXY(currentPosition).Y;
                return caretLineNumber;
            }
        }

        /// <summary>
        /// Gets the last line number (zero-based).
        /// </summary>
        [Browsable(false)]
        public virtual long LastLineNumber
        {
            get
            {
                var lastPosition = GetLastPosition();
                var lastLineNumber = PositionToXY(lastPosition).Y;
                return lastLineNumber;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="EnterPressed"/>
        /// event is fired.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if <see cref="EnterPressed"/> event is generated;
        /// <see langword="false" />, if the ENTER key is either processed
        /// internally by the control or used for navigation between dialog controls.
        /// The default is <see langword="false" />.
        /// </returns>
        public virtual bool ProcessEnter
        {
            get
            {
                return Handler.ProcessEnter;
            }

            set
            {
                Handler.ProcessEnter = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the input text is password and
        /// all characters are shown as *.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if all characters of the input text are shown
        /// as password chars;
        /// <see langword="false" />, if text is shown normally.
        /// The default is <see langword="false" />.
        /// </returns>
        public virtual bool IsPassword
        {
            get
            {
                return Handler.IsPassword;
            }

            set
            {
                Handler.IsPassword = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether urls in the input text
        /// are highlighted and underlined.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if urls in the input text
        /// are highlighted and underlined;
        /// <see langword="false" />, if urls in the input text are shown as normal
        /// text. The default is <see langword="false" />.
        /// </returns>
        /// <remarks>
        /// We do not suggest using this property as there is no known way to specify color
        /// of the url text. On dark themes auto urls don’t look good on Linux. If you need
        /// to highlight urls, use RichTextBox control.
        /// </remarks>
        /// <remarks>
        /// <see cref="TextUrl"/> event is fired when url is clicked.
        /// </remarks>
        public virtual bool AutoUrl
        {
            get
            {
                return Handler.AutoUrl;
            }

            set
            {
                Handler.AutoUrl = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether vertical scrollbar is
        /// hidden in the control.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if vertical scrollbar is hidden;
        /// <see langword="false" />, if vertical scrollbar is shown.
        /// The default is <see langword="false" />.
        /// </returns>
        /// <remarks>
        /// This property is for multiline controls only. Hidden vertical scrollbar
        /// limits the amount of text which can be entered into the control to
        /// what can be displayed in it under Windows but not under Linux.
        /// Currently not implemented for the other platforms.
        /// </remarks>
        public virtual bool HideVertScrollbar
        {
            get
            {
                return Handler.HideVertScrollbar;
            }

            set
            {
                Handler.HideVertScrollbar = value;
            }
        }

        /// <summary>
        /// Gets or sets current position of the caret.
        /// </summary>
        /// <remarks>
        /// Uses <see cref="GetInsertionPoint"/> and <see cref="PositionToXY"/> to get the position.
        /// Uses <see cref="SetInsertionPoint"/> and <see cref="XYToPosition"/> to set the position.
        /// </remarks>
        /// <remarks>
        /// Caret position starts from (0,0).
        /// </remarks>
        [Browsable(false)]
        public virtual Int32Point? CurrentPosition
        {
            get
            {
                var insertPoint = GetInsertionPoint();
                var currentPos = PositionToXY(insertPoint);
                if (currentPos == Int32Point.MinusOne)
                    return null;
                return currentPos;
            }

            set
            {
                value ??= Int32Point.Empty;
                var insertPoint = XYToPosition((long)value.Value.X, (long)value.Value.Y);
                SetInsertionPoint(insertPoint);
                CurrentPositionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets position of the caret which was reported in the event.
        /// </summary>
        [Browsable(false)]
        public virtual Int32Point? ReportedPosition { get; set; }

        /// <summary>
        /// Gets a value indicating whether the user can undo the previous operation
        /// in the control.
        /// </summary>
        /// <returns>
        /// <see langword = "true" /> if the user can undo the previous
        /// operation performed
        /// in the control; otherwise, <see langword = "false" />.
        /// </returns >
        [Browsable(false)]
        public virtual bool CanUndo
        {
            get
            {
                return Handler.CanUndo;
            }
        }

        /// <summary>
        /// Gets whether control contains text.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsEmpty
        {
            get
            {
                return Handler.IsEmpty;
            }
        }

        /// <summary>
        /// Gets or sets a hint shown in an empty unfocused text control.
        /// </summary>
        public virtual string? EmptyTextHint
        {
            get
            {
                return Handler.EmptyTextHint;
            }

            set
            {
                value ??= string.Empty;
                Handler.EmptyTextHint = value;
            }
        }

        internal new NativeTextBoxHandler Handler =>
            (NativeTextBoxHandler)base.Handler;

        /// <summary>
        /// Creates new custom text style.
        /// </summary>
        /// <returns></returns>
        public static ITextBoxTextAttr CreateTextAttr()
        {
            return new TextBoxTextAttr();
        }

        /// <inheritdoc cref="ValueValidatorFactory.CreateValidator(ValueValidatorKind)"/>
        public static IValueValidatorText CreateValidator(ValueValidatorKind kind)
        {
            return ValueValidatorFactory.CreateValidator(kind);
        }

        /// <inheritdoc cref="ValueValidatorFactory.CreateValidator(TypeCode)"/>
        public static IValueValidatorText CreateValidator(TypeCode typeCode)
        {
            return ValueValidatorFactory.CreateValidator(typeCode);
        }

        /// <summary>
        /// Creates <see cref="IValueValidatorText"/> instance for the specified type.
        /// </summary>
        /// <param name="type">Type.</param>
        public static IValueValidatorText CreateValidator(Type type)
        {
            var typeCode = AssemblyUtils.GetRealTypeCode(type);
            return CreateValidator(typeCode);
        }

        /// <summary>
        /// Changes the style of selection (if any). If no text is selected, style of the
        /// insertion point is changed.
        /// </summary>
        /// <param name="style">The new text style.</param>
        /// <returns>
        /// <c>true</c> on success, <c>false</c> if an error occurred (this may also
        /// mean that the styles are not supported under this platform).
        /// </returns>
        /// <remarks>
        /// If any attribute within style is not set, the corresponding
        /// attribute from <see cref="GetDefaultStyle"/> is used.
        /// </remarks>
        /// <remarks>
        /// Turn on <see cref="TextBox.IsRichEdit"/> in order to use this method.
        /// </remarks>
        public virtual bool SetSelectionStyle(ITextBoxTextAttr style)
        {
            if (HasSelection)
            {
                var selectionStart = GetSelectionStart();
                var selectionEnd = GetSelectionEnd();
                var result = SetStyle(selectionStart, selectionEnd, style);
                return result;
            }
            else
            {
                var position = GetInsertionPoint();
                var result = SetStyle(position, position, style);
                return result;
            }
        }

        /// <summary>
        /// Toggles <see cref="FontStyle"/> of the selection. If no text is selected, style of the
        /// insertion point is changed.
        /// </summary>
        /// <param name="toggle">Font style to toggle</param>
        public virtual void SelectionToggleFontStyle(FontStyle toggle)
        {
            var position = GetInsertionPoint();
            var fs = GetStyle(position);
            var style = fs.GetFontStyle();
            style = Font.ChangeFontStyle(style, toggle, !style.HasFlag(toggle));

            var newStyle = TextBox.CreateTextAttr();
            newStyle.Copy(fs);
            newStyle.SetFontStyle(style);
            SetSelectionStyle(newStyle);
        }

        /// <summary>
        /// Clears text formatting when <see cref="TextBox"/> is in rich edit mode.
        /// </summary>
        public void ClearTextFormatting()
        {
            var attr = CreateTextAttr();
            attr.SetFlags(TextBoxTextAttrFlags.All);
            SetSelectionStyle(attr);
        }

        /// <summary>
        /// Shows 'Go To Line' dialog.
        /// </summary>
        public virtual void ShowDialogGoToLine()
        {
            TextBoxUtils.ShowDialogGoToLine(this);
        }

        /// <summary>
        /// Sets foreground and background colors of the selection.  If no text is selected,
        /// style of the insertion point is changed.
        /// </summary>
        /// <param name="textColor">Foreground color.</param>
        /// <param name="backColor">Background color.</param>
        /// <remarks>
        /// If any of the color parameters is null, it will not be changed.
        /// </remarks>
        public void SelectionSetColor(Color? textColor, Color? backColor = null)
        {
            var position = GetInsertionPoint();
            var fs = GetStyle(position);
            var newStyle = TextBox.CreateTextAttr();
            newStyle.Copy(fs);
            if (backColor is not null)
                newStyle.SetBackgroundColor(backColor.Value);
            if (textColor is not null)
                newStyle.SetTextColor(textColor.Value);
            SetSelectionStyle(newStyle);
        }

        /// <summary>
        /// Toggles <see cref="FontStyle.Bold"/> style of the selection. If no text is selected,
        /// style of the insertion point is changed.
        /// </summary>
        public virtual void SelectionToggleBold()
        {
            SelectionToggleFontStyle(FontStyle.Bold);
        }

        /// <summary>
        /// Toggles <see cref="FontStyle.Italic"/> style of the selection. If no text is selected,
        /// style of the insertion point is changed.
        /// </summary>
        public virtual void SelectionToggleItalic()
        {
            SelectionToggleFontStyle(FontStyle.Italic);
        }

        /// <summary>
        /// Toggles <see cref="FontStyle.Underlined"/> style of the selection. If no text is selected,
        /// style of the insertion point is changed.
        /// </summary>
        public virtual void SelectionToggleUnderline()
        {
            SelectionToggleFontStyle(FontStyle.Underlined);
        }

        /// <summary>
        /// Toggles <see cref="FontStyle.Strikethrough"/> style of the selection. If no text is
        /// selected, style of the insertion point is changed.
        /// </summary>
        public virtual void SelectionToggleStrikethrough()
        {
            SelectionToggleFontStyle(FontStyle.Strikethrough);
        }

        /// <summary>
        /// Sets <see cref="CustomTextBox.DataType"/> property to <typeparamref name="T"/>
        /// and <see cref="Validator"/> to the appropriate validator provider.
        /// </summary>
        /// <typeparam name="T">New <see cref="CustomTextBox.DataType"/> property value.</typeparam>
        public virtual void UseValidator<T>()
        {
            DataType = typeof(T);
            Validator = CreateValidator(typeof(T));
        }

        /// <summary>
        /// Gets the length of the specified line, not including any trailing
        /// newline character(s).
        /// </summary>
        /// <param name="lineNo">Line number (starting from zero).</param>
        /// <returns>The length of the line, or -1 if lineNo was invalid.</returns>
        public virtual int GetLineLength(long lineNo)
        {
            return Handler.GetLineLength(lineNo);
        }

        /// <inheritdoc/>
        public override string GetLineText(long lineNo)
        {
            return Handler.GetLineText(lineNo);
        }

        /// <summary>
        ///     Raises the <see cref="TextMaxLength"/> event.
        /// </summary>
        /// <param name="e">
        ///     An <see cref="EventArgs"/> that contains the event data.
        /// </param>
        public virtual void OnTextMaxLength(EventArgs e)
        {
            TextMaxLength?.Invoke(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="TextUrl"/> event.
        /// </summary>
        /// <param name="e">
        ///     An <see cref="UrlEventArgs"/> that contains the event data.
        /// </param>
        public virtual void OnTextUrl(UrlEventArgs e)
        {
            // Under MacOs url parameter of the event data is always empty,
            // so event is not fired. Also on MacOs url is opened automatically.
            if (Application.IsMacOs)
                return;
            TextUrl?.Invoke(this, e);
            if (e.Cancel)
                return;
            if (AutoUrlOpen && e.IsValidUrl)
            {
                var modifiers = AutoUrlModifiers ?? DefaultAutoUrlModifiers
                    ?? AllPlatformDefaults.PlatformCurrent.TextBoxUrlClickModifiers;

                if (e.Modifiers == modifiers)
                    AppUtils.OpenUrl(e.Url!);
            }
        }

        /// <summary>
        ///     Raises the <see cref="EnterPressed"/> event.
        /// </summary>
        /// <param name="e">
        ///     An <see cref="EventArgs"/> that contains the event data.
        /// </param>
        public virtual void OnEnterPressed(EventArgs e)
        {
            EnterPressed?.Invoke(this, e);
        }

        /// <inheritdoc/>
        public override int GetNumberOfLines()
        {
            return Handler.GetNumberOfLines();
        }

        /// <summary>
        /// Converts given position to a zero-based column, line number pair.
        /// </summary>
        /// <param name="pos">Position in the text.</param>
        /// <returns>Point.X receives zero based column number. Point.Y receives
        /// zero based line number. If failure returns (-1,-1).</returns>
        public virtual Int32Point PositionToXY(long pos)
        {
            return Handler.PositionToXY(pos);
        }

        /// <summary>
        /// Converts given text position to client coordinates in pixels.
        /// </summary>
        /// <param name="pos">
        /// Text position in 0 to <see cref="GetLastPosition"/>
        /// range (inclusive).
        /// </param>
        /// <returns>
        /// On success returns a wxPoint which contains client
        /// coordinates for the given position in pixels, otherwise
        /// returns (-1,-1).
        /// </returns>
        /// <remarks>
        /// This function allows finding where is the character at the
        /// given position displayed in the text control.
        /// </remarks>
        /// <remarks>
        /// Availability: only available for the MSW, GTK ports.
        /// Additionally, GTK only implements this method for multiline
        /// controls and (-1,-1) is always returned for the single line ones.
        /// </remarks>
        public virtual Point PositionToCoords(long pos)
        {
            return Handler.PositionToCoords(pos);
        }

        /// <summary>
        /// Makes the line containing the given position visible.
        /// </summary>
        /// <param name="pos">The position that should be visible.</param>
        public virtual void ShowPosition(long pos)
        {
            Handler.ShowPosition(pos);
        }

        /// <summary>
        /// Converts the given zero based column and line number to a position.
        /// </summary>
        /// <param name="x">The column number.</param>
        /// <param name="y">The line number.</param>
        /// <returns>The position value, or -1 if x or y was invalid.</returns>
        public virtual long XYToPosition(long x, long y)
        {
            return Handler.XYToPosition(x, y);
        }

        /// <summary>
        /// Clears all text in the control.
        /// </summary>
        public virtual void Clear()
        {
            Handler.Clear();
        }

        /// <summary>
        /// Copies the current selection in the control to the Clipboard.
        /// </summary>
        public virtual void Copy()
        {
            Handler.Copy();
        }

        /// <summary>
        /// Moves the current selection in the control to the Clipboard.
        /// </summary>
        public virtual void Cut()
        {
            Handler.Cut();
        }

        /// <summary>
        /// Appends the text to the end of the text control.
        /// </summary>
        /// <param name="text">Text to write to the control.</param>
        /// <remarks>
        /// After the text is appended, the insertion point will be at the end
        /// of the text control. If this behaviour is not desired,
        /// the programmer should use <see cref="GetInsertionPoint"/>
        /// and <see cref="SetInsertionPoint"/>.
        /// </remarks>
        public virtual void AppendText(string text)
        {
            Handler.AppendText(text);
        }

        /// <summary>
        /// Appends new line to the end of the text control.
        /// </summary>
        /// <remarks>
        /// After the text is appended, the insertion point will be at the end
        /// of the text control. If this behaviour is not desired,
        /// the programmer should use <see cref="GetInsertionPoint"/>
        /// and <see cref="SetInsertionPoint"/>.
        /// </remarks>
        public virtual void AppendNewLine()
        {
            AppendText("\n");
        }

        /// <summary>
        /// Appends text and styles to the end of the text control.
        /// </summary>
        /// <param name="list">List containing strings or
        /// <see cref="ITextBoxTextAttr"/> instances.</param>
        /// <remarks>
        /// After the text is appended, the insertion point will be at the end
        /// of the text control. If this behaviour is not desired,
        /// the programmer should use <see cref="GetInsertionPoint"/>
        /// and <see cref="SetInsertionPoint"/>.
        /// </remarks>
        /// <remarks>
        /// Turn on <see cref="TextBox.IsRichEdit"/> in order to use this method.
        /// </remarks>
        public virtual void AppendTextAndStyles(IEnumerable<object> list)
        {
            foreach (object item in list)
            {
                var ta = item as ITextBoxTextAttr;
                if (ta is not null)
                {
                    SetDefaultStyle(ta);
                    continue;
                }

                if (item != null)
                    AppendText(item.ToString()!);
            }
        }

        /// <summary>
        /// Returns the insertion point, or cursor, position.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// This is defined as the zero based index of the character position to the
        /// right of the insertion point. For example, if the insertion point is at
        /// the end of the single-line text control, it is equal to
        /// <see cref="GetLastPosition"/>.
        /// </remarks>
        /// <remarks>
        /// Notice that insertion position is, in general, different from the
        /// index of the character the cursor position at in the string returned
        /// by GetValue(). While this is always the case for the single line
        /// controls, multi-line controls can use two characters "\\r\\n" as
        /// line separator (this is notably the case under MSW) meaning that
        /// indices in the control and its string value are offset by 1 for
        /// every line.
        /// </remarks>
        public virtual long GetInsertionPoint()
        {
            return Handler.GetInsertionPoint();
        }

        /// <summary>
        /// Returns current character or an empty string.
        /// </summary>
        /// <returns></returns>
        public virtual string GetCurrentChar()
        {
            long pos = GetInsertionPoint();
            if (pos == GetLastPosition())
                return string.Empty;
            return GetRange(pos, pos + 1);
        }

        /// <summary>
        /// Replaces the current selection in the control with the
        /// contents of the Clipboard.
        /// </summary>
        public virtual void Paste()
        {
            Handler.Paste();
        }

        /// <summary>
        /// Redos the last edit operation in the control.
        /// </summary>
        public virtual void Redo()
        {
            Handler.Redo();
        }

        /// <summary>
        /// Removes the text starting at the first given position
        /// up to(but not including) the character at the last position.
        /// </summary>
        /// <param name="from">The first position.</param>
        /// <param name="to">The last position.</param>
        /// <remarks>
        /// This function puts the current insertion point position at
        /// to as a side effect.
        /// </remarks>
        public virtual void Remove(long from, long to)
        {
            Handler.Remove(from, to);
        }

        /// <summary>
        /// Replaces the text starting at the first position up
        /// to(but not including) the character at the last position
        /// with the given text.
        /// </summary>
        /// <param name="from">The first position.</param>
        /// <param name="to">The last position.</param>
        /// <param name="value">The value to replace the existing text with.</param>
        /// <remarks>
        /// This function puts the current insertion point position
        /// at to as a side effect.
        /// </remarks>
        public virtual void Replace(long from, long to, string value)
        {
            Handler.Replace(from, to, value);
        }

        /// <summary>
        /// Sets the insertion point at the given position.
        /// </summary>
        /// <param name="pos">Position to set, in the range from 0
        /// to <see cref="GetLastPosition"/> inclusive.</param>
        public virtual void SetInsertionPoint(long pos)
        {
            Handler.SetInsertionPoint(pos);
        }

        /// <summary>
        /// Sets the insertion point at the end of the text control.
        /// </summary>
        public virtual void SetInsertionPointEnd()
        {
            Handler.SetInsertionPointEnd();
        }

        /// <summary>
        /// Call this method in <see cref="Application.Idle"/> event handler
        /// in order to update information related to the current selection and caret position.
        /// </summary>
        public virtual void IdleAction()
        {
            if (CurrentPositionChanged is not null)
            {
                var currentPos = CurrentPosition;
                if (ReportedPosition != currentPos)
                {
                    ReportedPosition = currentPos;
                    CurrentPositionChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Selects the text starting at the first position up to
        /// (but not including) the character at the last position.
        /// </summary>
        /// <param name="from">The first position.</param>
        /// <param name="to">The last position.</param>
        /// <remarks>
        /// If both parameters are equal to -1 all text in the control is selected.
        /// </remarks>
        /// <remarks>
        /// Notice that the insertion point will be moved to from by this function.
        /// </remarks>
        public virtual void SetSelection(long from, long to)
        {
            Handler.SetSelection(from, to);
        }

        /// <summary>
        /// Selects all text in the control.
        /// </summary>
        public virtual void SelectAll()
        {
            if (IsRichEdit)
            {
                DoInsideUpdate(() =>
                {
                    SetSelection(-1, -1);
                });
            }
            else
                Handler.SelectAll();
        }

        /// <summary>
        /// Clears selection. All text becomes unselected.
        /// </summary>
        public virtual void SelectNone()
        {
            Handler.SelectNone();
        }

        /// <summary>
        /// Undoes the last edit operation in the control.
        /// </summary>
        public virtual void Undo()
        {
            Handler.Undo();
        }

        /// <summary>
        /// Writes the text into the text control at the current insertion position.
        /// </summary>
        /// <param name="text">Text to write to the text control.</param>
        /// <remarks>
        /// Newlines in the text string are the only special characters
        /// allowed, and they will cause appropriate line breaks.
        /// </remarks>
        /// <remarks>
        /// After the write operation, the insertion point will be at the
        /// end of the inserted text, so subsequent write operations will be appended.
        /// </remarks>
        /// <remarks>
        /// To append text after the user may have interacted with the control,
        /// call <see cref="SetInsertionPointEnd"/> before writing.
        /// </remarks>
        public virtual void WriteText(string text)
        {
            Handler.WriteText(text);
        }

        /// <summary>
        /// Returns the string containing the text starting in the
        /// positions from and up to to in the control.
        /// </summary>
        /// <param name="from">The first position.</param>
        /// <param name="to">The last position.</param>
        /// <returns>
        /// <see cref="string"/> containing the text from the first
        /// to the last position.
        /// </returns>
        /// <remarks>
        /// The positions must have been returned by another control method.
        /// Please note that the positions in a multiline text control do not
        /// correspond to the indices in the string returned by Value because of the
        /// different new line representations(CR or CR LF). This method should be
        /// used to obtain the correct results instead of extracting parts of
        /// the entire value. It may also be more efficient, especially if the
        /// control contains a lot of data.
        /// </remarks>
        public virtual string GetRange(long from, long to)
        {
            return Handler.GetRange(from, to);
        }

        /// <summary>
        /// Gets the text currently selected in the control.
        /// </summary>
        /// <returns></returns>
        public virtual string GetStringSelection()
        {
            return Handler.GetStringSelection();
        }

        /// <summary>
        /// Clears internal undo buffer. No undo operations are available
        /// after this operation.
        /// </summary>
        public virtual void EmptyUndoBuffer()
        {
            Handler.EmptyUndoBuffer();
        }

        /// <summary>
        /// Return true if the given position is valid, i.e. positive
        /// and less than the last position.
        /// </summary>
        /// <param name="pos">Position to check.</param>
        /// <returns></returns>
        public virtual bool IsValidPosition(long pos)
        {
            return Handler.IsValidPosition(pos);
        }

        /// <summary>
        /// Returns the zero based index of the last position in
        /// the text control, which is equal to the number of characters
        /// in the control.
        /// </summary>
        /// <returns></returns>
        public virtual long GetLastPosition()
        {
            return Handler.GetLastPosition();
        }

        /// <summary>
        /// Gets the current selection start position.
        /// </summary>
        /// <returns></returns>
        public virtual long GetSelectionStart()
        {
            return Handler.GetSelectionStart();
        }

        /// <summary>
        /// Gets the current selection end position.
        /// </summary>
        /// <returns></returns>
        public virtual long GetSelectionEnd()
        {
            return Handler.GetSelectionEnd();
        }

        /// <summary>
        /// Raises the <see cref="TextChanged"/> event and calls
        /// <see cref="OnTextChanged"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        public void RaiseTextChanged(TextChangedEventArgs e)
        {
#pragma warning disable
            if (e == null)
                throw new ArgumentNullException(nameof(e));
#pragma warning restore

            OnTextChanged(e);
        }

        /// <summary>
        /// Moves caret to the beginning of the text.
        /// </summary>
        public virtual void MoveToBeginOfText()
        {
            SetInsertionPoint(0);
        }

        /// <summary>
        /// Moves caret to the end of the text.
        /// </summary>
        public virtual void MoveToEndOfText()
        {
            SetInsertionPointEnd();
        }

        /// <summary>
        /// Returns the style currently used for the new text.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Turn on <see cref="TextBox.IsRichEdit"/> in order to use this method.
        /// </remarks>
        public virtual ITextBoxTextAttr GetDefaultStyle()
        {
            return new TextBoxTextAttr(Handler.GetDefaultStyle());
        }

        /// <summary>
        /// Returns the style at this position in the text control.
        /// </summary>
        /// <param name="pos">The position for which text style is returned.</param>
        /// <returns></returns>
        /// <remarks>
        /// Turn on <see cref="TextBox.IsRichEdit"/> in order to use this method.
        /// </remarks>
        public virtual ITextBoxTextAttr GetStyle(long pos)
        {
            return new TextBoxTextAttr(Handler.GetStyle(pos));
        }

        /// <summary>
        /// Changes the default style to use for the new text which is
        /// going to be added to the control using <see cref="WriteText"/>
        /// or <see cref="AppendText"/>.
        /// </summary>
        /// <param name="style">The style for the new text.</param>
        /// <returns>
        /// true on success, false if an error occurred(this may
        /// also mean that the styles are not supported under this platform).
        /// </returns>
        /// <remarks>
        /// If either of the font, foreground, or background colour is not set in
        /// style, the values of the previous default style are used for them. If the
        /// previous default style didn't set them neither, the global font or colors
        /// of the text control itself are used as fall back.
        /// </remarks>
        /// <remarks>
        /// Turn on <see cref="TextBox.IsRichEdit"/> in order to use this method.
        /// </remarks>
        public virtual bool SetDefaultStyle(ITextBoxTextAttr style)
        {
            if (style is not TextBoxTextAttr s)
                return false;
            return Handler.SetDefaultStyle(s.Handle);
        }

        /// <summary>
        /// Changes the style of the given range.
        /// </summary>
        /// <param name="start">The start of the range to change.</param>
        /// <param name="end">The end of the range to change.</param>
        /// <param name="style">The new style for the range.</param>
        /// <returns>
        /// <c>true</c> on success, <c>false</c> if an error occurred (this may also
        /// mean that the styles are not supported under this platform).
        /// </returns>
        /// <remarks>
        /// If any attribute within style is not set, the corresponding
        /// attribute from <see cref="GetDefaultStyle"/> is used.
        /// </remarks>
        /// <remarks>
        /// Turn on <see cref="TextBox.IsRichEdit"/> in order to use this method.
        /// </remarks>
        public virtual bool SetStyle(long start, long end, ITextBoxTextAttr style)
        {
            if (style is not TextBoxTextAttr s)
                return false;
            return Handler.SetStyle(start, end, s.Handle);
        }

        /// <summary>
        /// Executes a browser command with the specified name and parameters.
        /// </summary>
        /// <param name = "cmdName" >
        /// Name of the command to execute.
        /// </param>
        /// <param name = "args" >
        /// Parameters of the command.
        /// </param>
        /// <returns>
        /// An <see cref="object"/> representing the result of the command execution.
        /// </returns>
        public virtual object? DoCommand(string cmdName, params object?[] args)
        {
            if (cmdName == "GetReportedUrl")
            {
                return Handler.ReportedUrl;
            }

            return null;
        }

        /// <summary>
        /// Sets text alignment in the current position to <paramref name="alignment"/>.
        /// </summary>
        /// <param name="alignment">New alignment value.</param>
        public virtual void SelectionSetAlignment(TextBoxTextAttrAlignment alignment)
        {
            var fs = CreateTextAttr();
            fs.SetAlignment(alignment);
            SetSelectionStyle(fs);
        }

        /// <summary>
        /// Sets text alignment in the current position to <see cref="TextBoxTextAttrAlignment.Center"/>
        /// </summary>
        public virtual void SelectionAlignCenter()
        {
            SelectionSetAlignment(TextBoxTextAttrAlignment.Center);
        }

        /// <summary>
        /// Sets text alignment in the current position to <see cref="TextBoxTextAttrAlignment.Left"/>
        /// </summary>
        public virtual void SelectionAlignLeft()
        {
            SelectionSetAlignment(TextBoxTextAttrAlignment.Left);
        }

        /// <summary>
        /// Sets <see cref="CustomTextBox.ValidatorErrorText"/> property
        /// to <paramref name="knownError"/>.
        /// </summary>
        /// <param name="knownError">Known error identifier.</param>
        public virtual void SetErrorText(ValueValidatorKnownError knownError)
        {
            ValidatorErrorText = GetKnownErrorText(knownError);
        }

        /// <summary>
        /// Sets text alignment in the current position to <see cref="TextBoxTextAttrAlignment.Right"/>
        /// </summary>
        public virtual void SelectionAlignRight()
        {
            SelectionSetAlignment(TextBoxTextAttrAlignment.Right);
        }

        /// <summary>
        /// Sets text alignment in the current position to
        /// <see cref="TextBoxTextAttrAlignment.Justified"/>
        /// </summary>
        public virtual void SelectionAlignJustified()
        {
            SelectionSetAlignment(TextBoxTextAttrAlignment.Justified);
        }

        /// <summary>
        /// Called when content in this Control changes.
        /// Raises the TextChanged event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnTextChanged(TextChangedEventArgs e)
        {
            RaiseEvent(e);
            if (Options.HasFlag(TextBoxOptions.DefaultValidation))
                RunDefaultValidation();
        }

        /*
        /// <summary>
        /// Called when the value of the <see cref="Text"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        protected virtual void OnTextChanged(EventArgs e)
        {
        }*/

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return new NativeTextBoxHandler();
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
            textBox.OnTextChanged(new TextChangedEventArgs(TextChangedEvent));
        }
    }
}