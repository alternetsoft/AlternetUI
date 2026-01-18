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
    public partial class TextBox : CustomTextBox, ISimpleRichTextBox
    {
        private bool multiline = false;
        private bool hasBorder = true;
        private bool readOnly = false;
        private TextBoxTextWrap textWrap;
        private TextHorizontalAlignment textAlign;

        static TextBox()
        {
            PropertyGrid.AddInitializer(() =>
            {
                var prm = PropertyGrid.GetNewItemParams(typeof(TextBox), nameof(TextBox.TextAlign));
                if (prm is not null)
                {
                    var choices = PropertyGrid.CreateChoices();
                    choices.Add(PropNameStrings.Default.Left, GenericAlignment.Left);
                    choices.Add(PropNameStrings.Default.Right, GenericAlignment.Right);
                    choices.Add(PropNameStrings.Default.Center, GenericAlignment.CenterHorizontal);

                    prm.EnumIsFlags = false;
                    prm.Choices = choices;
                }
            });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBox"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public TextBox(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBox"/> class.
        /// </summary>
        public TextBox()
        {
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
        /// You need to call <see cref="IdleAction"/> periodically in order to enable
        /// <see cref="CurrentPositionChanged"/> event firing.
        /// </remarks>
        public event EventHandler? CurrentPositionChanged;

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
        /// Gets or sets default value of the <see cref="AutoUrlModifiers"/> property.
        /// </summary>
        /// <remarks>
        /// If this is not assigned (default),
        /// <see cref="PlatformDefaults.TextBoxUrlClickModifiers"/> is used as property default.
        /// </remarks>
        public static ModifierKeys? DefaultAutoUrlModifiers { get; set; }

        /// <summary>
        /// Gets or sets default value of the <see cref="AutoUrlOpen"/> property.
        /// </summary>
        public static bool DefaultAutoUrlOpen { get; set; } = false;

        /// <inheritdoc/>
        public override bool WantTab
        {
            get => base.WantTab && !ReadOnly;

            set
            {
                if (WantTab == value || DisposingOrDisposed)
                    return;
                base.WantTab = value;
                Handler.ProcessTab = value;
            }
        }

        /// <summary>
        /// Gets or sets the starting point of text selected in the control.
        /// </summary>
        /// <returns>
        /// The starting position of text selected in the control.
        /// </returns>
        [Category("Appearance")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int SelectionStart
        {
            get
            {
                return (int)GetSelectionStart();
            }

            set
            {
                if (value < 0)
                    value = 0;
                if (SelectionStart == value)
                    return;
                SetSelection(value, GetSelectionEnd());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the currently selected text in the control.
        /// </summary>
        /// <returns>
        /// A string that represents the currently selected text in the control.
        /// </returns>
        /// <remarks>
        /// If there is no selection, the replacement text is inserted at the caret.
        /// </remarks>
        [Category("Appearance")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string SelectedText
        {
            get
            {
                return GetStringSelection();
            }

            set
            {
                value ??= string.Empty;

                if (SelectedText == value)
                    return;

                if (HasSelection)
                {
                    Replace(GetSelectionStart(), GetSelectionEnd(), value);
                }
                else
                {
                    WriteText(value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the number of characters selected in the control.
        /// </summary>
        /// <returns>
        /// The number of characters selected in the control.
        /// </returns>
        [Category("Appearance")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int SelectionLength
        {
            get
            {
                if (HasSelection)
                {
                    var selectionStart = GetSelectionStart();
                    var selectionEnd = GetSelectionEnd();
                    var selectionLength = (int)(selectionEnd - selectionStart);
                    if (selectionLength < 0)
                        selectionLength = 0;
                    return selectionLength;
                }
                else
                    return 0;
            }

            set
            {
                if (value < 0)
                    value = 0;
                if (SelectionLength == value)
                    return;
                if(value == 0)
                {
                    SelectNone();
                }
                else
                {
                    if (HasSelection)
                    {
                        var selectionStart = GetSelectionStart();
                        var selectionEnd = selectionStart + value;
                        SetSelection(selectionStart, selectionEnd);
                    }
                    else
                    {
                        var selectionStart = GetInsertionPoint();
                        if (selectionStart < 0)
                            selectionStart = 0;
                        var selectionEnd = selectionStart + value;
                        SetSelection(selectionStart, selectionEnd);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether urls in the input text
        /// are opened in the default browser.
        /// </summary>
        /// <remarks>
        /// Use <see cref="AutoUrl"/> in order to highlight and underline urls.
        /// </remarks>
        [Browsable(false)]
        public virtual bool AutoUrlOpen { get; set; } = DefaultAutoUrlOpen;

        /// <summary>
        /// Gets or sets <see cref="ModifierKeys"/> used when clicked url is automatically opened
        /// in the browser when <see cref="AutoUrlOpen"/> is <c>true</c>.
        /// </summary>
        [Browsable(false)]
        public virtual ModifierKeys? AutoUrlModifiers { get; set; }

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
        [Browsable(false)]
        public virtual bool Multiline
        {
            get
            {
                return multiline;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                if (multiline == value)
                    return;
                multiline = value;
                Handler.Multiline = value;
                MultilineChanged?.Invoke(this, EventArgs.Empty);
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
                if (DisposingOrDisposed)
                    return;
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
        /// Default value is <see cref="TextHorizontalAlignment.Left"/>.
        /// </remarks>
        [DefaultValue(TextHorizontalAlignment.Left)]
        [Browsable(false)]
        public virtual TextHorizontalAlignment TextAlign
        {
            get
            {
                return textAlign;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                if (textAlign == value)
                    return;
                textAlign = value;

                Handler.TextAlign = value;
            }
        }

        /// <summary>
        /// Gets the length of text in the control.
        /// </summary>
        /// <returns>
        /// The number of characters contained in the text of the control.
        /// </returns>
        [Browsable(false)]
        public virtual int TextLength
        {
            get
            {
                return Text.Length;
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
        /// set the value of the <see cref="AbstractControl.Text"/> property in code.
        /// </remarks>
        public virtual bool ReadOnly
        {
            get
            {
                return readOnly;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                if (readOnly == value)
                    return;

                readOnly = value;
                Handler.ReadOnly = value;
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
                if (DisposingOrDisposed)
                    return;
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
        [Browsable(true)]
        public override bool HasBorder
        {
            get
            {
                return hasBorder;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                if (hasBorder == value)
                    return;
                hasBorder = value;
                base.Handler.HasBorder = value;
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
                if (DisposingOrDisposed)
                    return default;
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
                if (DisposingOrDisposed)
                    return default;
                return Handler.IsModified;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
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
                if (DisposingOrDisposed)
                    return default;
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
                if (DisposingOrDisposed)
                    return default;
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
                if (DisposingOrDisposed)
                    return default;
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
                if (DisposingOrDisposed)
                    return default;
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
                if (DisposingOrDisposed)
                    return default;
                return Handler.HideSelection;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.HideSelection = value;
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
                if (DisposingOrDisposed)
                    return default;
                return Handler.ProcessEnter;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
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
                if (DisposingOrDisposed)
                    return default;
                return Handler.IsPassword;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.IsPassword = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether urls in the input text
        /// are highlighted and underlined.
        /// Currently does nothing as WxWidgets implementation is buggy.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if urls in the input text
        /// are highlighted and underlined;
        /// <see langword="false" />, if urls in the input text are shown as normal
        /// text. The default is <see langword="false" />.
        /// </returns>
        /// <remarks>
        /// We do not suggest using this property as there is no known way to specify color
        /// of the url text. On dark themes auto urls don�t look good on Linux. If you need
        /// to highlight urls, use RichTextBox control.
        /// </remarks>
        /// <remarks>
        /// <see cref="TextUrl"/> event is fired when url is clicked.
        /// </remarks>
        [Browsable(false)]
        public virtual bool AutoUrl
        {
            get
            {
                return false;
            }

            set
            {
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
        [Browsable(false)]
        public virtual bool HideVertScrollbar
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return Handler.HideVertScrollbar;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
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
        public virtual PointI? CurrentPosition
        {
            get
            {
                var insertPoint = GetInsertionPoint();
                var currentPos = PositionToXY(insertPoint);
                if (currentPos == PointI.MinusOne)
                    return null;
                return currentPos;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                value ??= PointI.Empty;
                var insertPoint = XYToPosition((long)value.Value.X, (long)value.Value.Y);
                SetInsertionPoint(insertPoint);
                CurrentPositionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets position of the caret which was reported in the event.
        /// </summary>
        [Browsable(false)]
        public virtual PointI? ReportedPosition { get; set; }

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
                if (DisposingOrDisposed)
                    return default;
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
                if (DisposingOrDisposed)
                    return default;
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
                if (DisposingOrDisposed)
                    return default;
                return Handler.EmptyTextHint;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                value ??= string.Empty;
                Handler.EmptyTextHint = value;
            }
        }

        /// <summary>
        /// Gets control handler.
        /// </summary>
        [Browsable(false)]
        public new ITextBoxHandler Handler => (ITextBoxHandler)base.Handler;

        /// <summary>
        /// Shows 'Go To Line' dialog.
        /// </summary>
        public virtual void ShowDialogGoToLine()
        {
            TextBoxUtils.ShowDialogGoToLine(this);
        }

        /// <summary>
        /// Gets the length of the specified line, not including any trailing
        /// newline character(s).
        /// </summary>
        /// <param name="lineNo">Line number (starting from zero).</param>
        /// <returns>The length of the line, or -1 if lineNo was invalid.</returns>
        public virtual int GetLineLength(long lineNo)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.GetLineLength(lineNo);
        }

        /// <inheritdoc/>
        public override string GetLineText(long lineNo)
        {
            if (DisposingOrDisposed)
                return string.Empty;
            return Handler.GetLineText(lineNo);
        }

        /// <summary>
        ///     Raises the <see cref="TextMaxLength"/> event.
        /// </summary>
        public virtual void OnTextMaxLength()
        {
            if (DisposingOrDisposed)
                return;
            TextMaxLength?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Raises the <see cref="TextUrl"/> event.
        /// </summary>
        /// <param name="e">
        ///     An <see cref="UrlEventArgs"/> that contains the event data.
        /// </param>
        public virtual void OnTextUrl(UrlEventArgs e)
        {
            if (DisposingOrDisposed)
                return;

            // Under MacOs url parameter of the event data is always empty,
            // so event is not fired. Also on MacOs url is opened automatically.
            if (App.IsMacOS)
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
        public virtual void OnEnterPressed()
        {
            if (DisposingOrDisposed)
                return;
            EnterPressed?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public override int GetNumberOfLines()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.GetNumberOfLines();
        }

        /// <summary>
        /// Converts given position to a zero-based column, line number pair.
        /// </summary>
        /// <param name="pos">Position in the text.</param>
        /// <returns>Point.X receives zero based column number. Point.Y receives
        /// zero based line number. If failure returns (-1,-1).</returns>
        public virtual PointI PositionToXY(long pos)
        {
            if (DisposingOrDisposed)
                return default;
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
        /// Only available for the MSW, GTK ports.
        /// Additionally, GTK only implements this method for multiline
        /// controls and (-1,-1) is always returned for the single line ones.
        /// </remarks>
        public virtual PointD PositionToCoord(long pos)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.PositionToCoord(pos);
        }

        /// <summary>
        /// Makes the line containing the given position visible.
        /// </summary>
        /// <param name="pos">The position that should be visible.</param>
        public virtual void ShowPosition(long pos)
        {
            if (DisposingOrDisposed)
                return;
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
            if (DisposingOrDisposed)
                return default;
            return Handler.XYToPosition(x, y);
        }

        /// <summary>
        /// Clears all text in the control.
        /// </summary>
        public virtual void Clear()
        {
            if (DisposingOrDisposed)
                return;
            Handler.Clear();
        }

        /// <summary>
        /// Copies the current selection in the control to the Clipboard.
        /// </summary>
        public virtual void Copy()
        {
            if (DisposingOrDisposed)
                return;
            Handler.Copy();
        }

        /// <summary>
        /// Moves the current selection in the control to the Clipboard.
        /// </summary>
        public virtual void Cut()
        {
            if (DisposingOrDisposed)
                return;
            Handler.Cut();
        }

        /// <summary>
        /// Appends the text to the end of the text control.
        /// </summary>
        /// <param name="text">Text to write to the control.</param>
        /// <remarks>
        /// After the text is appended, the insertion point will be at the end
        /// of the text control. If this behavior is not desired,
        /// the programmer should use <see cref="GetInsertionPoint"/>
        /// and <see cref="SetInsertionPoint"/>.
        /// </remarks>
        public virtual void AppendText(string text)
        {
            if (DisposingOrDisposed)
                return;
            Handler.AppendText(text);
        }

        /// <summary>
        /// Appends new line to the end of the text control.
        /// </summary>
        /// <remarks>
        /// After the text is appended, the insertion point will be at the end
        /// of the text control. If this behavior is not desired,
        /// the programmer should use <see cref="GetInsertionPoint"/>
        /// and <see cref="SetInsertionPoint"/>.
        /// </remarks>
        public virtual void AppendNewLine()
        {
            AppendText("\n");
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
            if (DisposingOrDisposed)
                return default;
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
        /// contents of the clipboard.
        /// </summary>
        public virtual void Paste()
        {
            if (DisposingOrDisposed)
                return;
            Handler.Paste();
        }

        /// <summary>
        /// Redo the last edit operation in the control.
        /// </summary>
        public virtual void Redo()
        {
            if (DisposingOrDisposed)
                return;
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
            if (DisposingOrDisposed)
                return;
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
            if (DisposingOrDisposed)
                return;
            Handler.Replace(from, to, value);
        }

        /// <summary>
        /// Sets the insertion point at the given position.
        /// </summary>
        /// <param name="pos">Position to set, in the range from 0
        /// to <see cref="GetLastPosition"/> inclusive.</param>
        public virtual void SetInsertionPoint(long pos)
        {
            if (DisposingOrDisposed)
                return;
            Handler.SetInsertionPoint(pos);
        }

        /// <summary>
        /// Sets the insertion point at the end of the text control.
        /// </summary>
        public virtual void SetInsertionPointEnd()
        {
            if (DisposingOrDisposed)
                return;
            Handler.SetInsertionPointEnd();
        }

        /// <summary>
        /// Call this method periodically
        /// in order to update information related to the current selection and caret position.
        /// </summary>
        public virtual void IdleAction()
        {
            if (DisposingOrDisposed)
                return;
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
            if (DisposingOrDisposed)
                return;
            Handler.SetSelection(from, to);
        }

        /// <summary>
        /// Selects all text in the control.
        /// </summary>
        public virtual void SelectAll()
        {
            if (DisposingOrDisposed)
                return;
            Handler.SelectAll();
        }

        /// <summary>
        /// Clears selection. All text becomes unselected.
        /// </summary>
        public virtual void SelectNone()
        {
            if (DisposingOrDisposed)
                return;
            Handler.SelectNone();
        }

        /// <summary>
        /// Undoes the last edit operation in the control.
        /// </summary>
        public virtual void Undo()
        {
            if (DisposingOrDisposed)
                return;
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
            if (DisposingOrDisposed)
                return;
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
            if (DisposingOrDisposed)
                return string.Empty;
            return Handler.GetRange(from, to);
        }

        /// <summary>
        /// Gets the text currently selected in the control.
        /// </summary>
        /// <returns></returns>
        public virtual string GetStringSelection()
        {
            if (DisposingOrDisposed)
                return string.Empty;
            return Handler.GetStringSelection();
        }

        /// <summary>
        /// Clears internal undo buffer. No undo operations are available
        /// after this operation.
        /// </summary>
        public virtual void EmptyUndoBuffer()
        {
            if (DisposingOrDisposed)
                return;
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
            if (DisposingOrDisposed)
                return default;
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
            if (DisposingOrDisposed)
                return default;
            return Handler.GetLastPosition();
        }

        /// <summary>
        /// Gets the current selection start position.
        /// </summary>
        /// <returns></returns>
        public virtual long GetSelectionStart()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.GetSelectionStart();
        }

        /// <summary>
        /// Gets the current selection end position.
        /// </summary>
        /// <returns></returns>
        public virtual long GetSelectionEnd()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.GetSelectionEnd();
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

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return ControlFactory.Handler.CreateTextBoxHandler(this);
        }

        /// <inheritdoc/>
        protected override void OnTextChanged(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            base.OnTextChanged(e);
            if (Options.HasFlag(TextBoxOptions.DefaultValidation))
                RunDefaultValidation();
        }

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (App.IsMacOS)
            {
                if (Multiline)
                    return;

                if (e.Key == Key.Home)
                {
                    MoveToBeginOfText();
                    e.Suppressed();
                }
                else
                if (e.Key == Key.End)
                {
                    MoveToEndOfText();
                    e.Suppressed();
                }
            }
        }
    }
}