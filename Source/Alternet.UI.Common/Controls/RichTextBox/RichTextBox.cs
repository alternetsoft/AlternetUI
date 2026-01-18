using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Implements rich text editor functionality.
    /// </summary>
    [ControlCategory("Common")]
    public partial class RichTextBox : Control, IReadOnlyStrings, IRichTextBox
    {
        private bool hasBorder = true;
        private StringSearch? search;

        static RichTextBox()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RichTextBox"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public RichTextBox(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RichTextBox"/> class.
        /// </summary>
        public RichTextBox()
        {
            WantTab = true;
        }

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
        /// Gets or sets default url color. This color looks good
        /// on white background. This property was added for the convenience and currently is used
        /// only <see cref="CreateUrlAttr"/>.
        /// </summary>
        public static Color DefaultUrlColorOnWhite { get; set; } = Color.Blue;

        /// <summary>
        /// Gets or sets default url color for the dark color scheme. This color looks good
        /// on dark background. This property was added for the convenience and currently is used
        /// only <see cref="CreateUrlAttr"/>.
        /// </summary>
        public static Color DefaultUrlColorOnBlack { get; set; } = Color.FromRgb(156, 220, 254);

        /// <summary>
        /// Gets or sets a value indicating whether urls in the input text
        /// are opened in the default browser.
        /// </summary>
        public virtual bool AutoUrlOpen { get; set; } = TextBox.DefaultAutoUrlOpen;

        /// <summary>
        /// Gets or sets position of the caret which was reported in the event.
        /// </summary>
        [Browsable(false)]
        public virtual PointI? ReportedPosition { get; set; }

        /// <summary>
        /// Gets or sets string search provider.
        /// </summary>
        [Browsable(false)]
        public virtual StringSearch Search
        {
            get => search ??= new(this);

            set
            {
                if (value is null)
                    return;
                search = value;
            }
        }

        /// <inheritdoc/>
        public override bool WantTab
        {
            get => base.WantTab && !ReadOnly;
            set => base.WantTab = value;
        }

        /// <summary>
        /// Gets or sets whether to call <see cref="HandleAdditionalKeys"/> method
        /// when key is pressed. Default is <c>true</c>.
        /// </summary>
        public virtual bool AllowAdditionalKeys { get; set; } = true;

        int IReadOnlyStrings.Count => GetNumberOfLines();

        /// <summary>
        /// Gets or sets the current filename associated with the control.
        /// </summary>
        [Browsable(false)]
        public virtual string FileName
        {
            get
            {
                if (DisposingOrDisposed)
                    return string.Empty;
                return Handler.GetFileName();
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.SetFileName(value ?? string.Empty);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether text in the control is read-only.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the control is read-only; otherwise,
        /// <see langword="false"/>. The default is <see langword="false"/>.
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
                return !IsEditable();
            }

            set
            {
                SetEditable(!value);
            }
        }

        /// <summary>
        /// Gets or sets <see cref="ModifierKeys"/> used when clicked url is automatically opened
        /// in the browser when <see cref="AutoUrlOpen"/> is <c>true</c>.
        /// </summary>
        [Browsable(false)]
        public virtual ModifierKeys? AutoUrlModifiers { get; set; }

        /// <summary>
        /// Gets or sets the text contents of the control.
        /// </summary>
        /// <value>A string containing the text contents of the control. The
        /// default is an empty string ("").</value>
        /// <remarks>
        /// Getting this property returns a string copy of the contents of the
        /// control. Setting this property replaces the contents of the control
        /// with the specified string.
        /// </remarks>
        [DefaultValue("")]
        public override string Text
        {
            get
            {
                return base.Text;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                value ??= string.Empty;
                if (value == Text)
                    return;
                base.Text = value;
                Handler.SetValue(value);
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
                value ??= PointI.Empty;
                var insertPoint = XYToPosition((long)value.Value.X, (long)value.Value.Y);
                SetInsertionPoint(insertPoint);
                CurrentPositionChanged?.Invoke(this, EventArgs.Empty);
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
                Handler.HasBorder = value;
            }
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.RichTextBox;

        [Browsable(false)]
        internal new IRichTextBox Handler
        {
            get
            {
                CheckDisposed();
                return (IRichTextBox)base.Handler;
            }
        }

        string? IReadOnlyStrings.this[int index]
        {
            get
            {
                try
                {
                    return GetLineText(index);
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Creates new custom rich text style.
        /// </summary>
        /// <returns></returns>
        public virtual ITextBoxRichAttr CreateRichAttr()
        {
            if (DisposingOrDisposed)
                return new PlessTextBoxRichAttr();
            return Handler.CreateRichAttr();
        }

        /// <summary>
        /// <inheritdoc cref="TextBox.GetRange"/>
        /// </summary>
        public virtual string GetRange(long from, long to)
        {
            if (DisposingOrDisposed)
                return string.Empty;
            return Handler.GetRange(from, to);
        }

        /// <summary>
        /// Returns the length of the specified line in characters.
        /// </summary>
        public virtual int GetLineLength(long lineNo)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.GetLineLength(lineNo);
        }

        /// <summary>
        /// Returns the text for the given line.
        /// </summary>
        public virtual string GetLineText(long lineNo)
        {
            if (DisposingOrDisposed)
                return string.Empty;
            return Handler.GetLineText(lineNo);
        }

        /// <summary>
        /// Returns the number of lines in the buffer.
        /// </summary>
        public virtual int GetNumberOfLines()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.GetNumberOfLines();
        }

        /// <summary>
        /// Returns <c>true</c> if the buffer has been modified.
        /// </summary>
        public virtual bool IsModified()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.IsModified();
        }

        /// <summary>
        /// Returns <c>true</c> if the control is editable.
        /// </summary>
        public virtual bool IsEditable()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.IsEditable();
        }

        /// <summary>
        /// Returns <c>true</c> if the control is single-line.
        /// Currently <see cref="RichTextBox"/> does not support single-line editing.
        /// </summary>
        public virtual bool IsSingleLine()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.IsSingleLine();
        }

        /// <summary>
        /// Returns <c>true</c> if the control is multiline.
        /// </summary>
        public virtual bool IsMultiLine()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.IsMultiLine();
        }

        /// <summary>
        /// Returns the text within the current selection range, if any.
        /// </summary>
        public virtual string GetStringSelection()
        {
            if (DisposingOrDisposed)
                return string.Empty;
            return Handler.GetStringSelection();
        }

        /// <summary>
        /// Sets the size of the buffer beyond which layout is delayed during resizing.
        /// This optimizes sizing for large buffers. The default is 20000.
        /// </summary>
        /// <param name="threshold"></param>
        public virtual void SetDelayedLayoutThreshold(long threshold)
        {
            if (DisposingOrDisposed)
                return;
            Handler.SetDelayedLayoutThreshold(threshold);
        }

        /// <summary>
        /// Gets the size of the buffer beyond which layout is delayed during resizing.
        /// This optimizes sizing for large buffers. The default is 20000.
        /// </summary>
        public virtual long GetDelayedLayoutThreshold()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.GetDelayedLayoutThreshold();
        }

        /// <summary>
        /// Gets the flag indicating that full layout is required.
        /// </summary>
        public virtual bool GetFullLayoutRequired()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.GetFullLayoutRequired();
        }

        /// <summary>
        /// Sets the flag indicating that full layout is required.
        /// </summary>
        /// <param name="b"></param>
        public virtual void SetFullLayoutRequired(bool b)
        {
            if (DisposingOrDisposed)
                return;
            Handler.SetFullLayoutRequired(b);
        }

        /// <summary>
        /// Returns the last time full layout was performed.
        /// </summary>
        public virtual long GetFullLayoutTime()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.GetFullLayoutTime();
        }

        /// <summary>
        /// Sets the last time full layout was performed.
        /// </summary>
        /// <param name="t"></param>
        public virtual void SetFullLayoutTime(long t)
        {
            if (DisposingOrDisposed)
                return;
            Handler.SetFullLayoutTime(t);
        }

        /// <summary>
        /// Returns the position that should be shown when full (delayed) layout is performed.
        /// </summary>
        /// <returns></returns>
        public virtual long GetFullLayoutSavedPosition()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.GetFullLayoutSavedPosition();
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
            TextUrl?.Invoke(this, e);
            if (e.Cancel)
                return;
            if (AutoUrlOpen && e.IsValidUrl)
            {
                var modifiers = AutoUrlModifiers ?? TextBox.DefaultAutoUrlModifiers
                    ?? AllPlatformDefaults.PlatformCurrent.TextBoxUrlClickModifiers;
                if (e.Modifiers == modifiers)
                    AppUtils.OpenUrl(e.Url!);
            }
        }

        /// <summary>
        /// Raises the <see cref="EnterPressed"/> event.
        /// </summary>
        public virtual void OnEnterPressed()
        {
            if (DisposingOrDisposed)
                return;
            EnterPressed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Writes url with title and style.
        /// </summary>
        /// <param name="style">Url style.</param>
        /// <param name="url">Url.</param>
        /// <param name="text">Url title (optional).</param>
        public virtual void WriteUrl(ITextBoxRichAttr style, string url, string? text = default)
        {
            BeginStyle(style);
            BeginURL(url);
            WriteText(text ?? url);
            EndURL();
            EndStyle();
        }

        /// <summary>
        /// Sets the current default style, which can be used to change how subsequently
        /// inserted text is displayed.
        /// </summary>
        /// <param name="style">The style for the new text.</param>
        /// <returns>
        /// <c>true</c> on success, <c>false</c> if an error occurred (this may
        /// also mean that the styles are not supported under this platform).
        /// </returns>
        public virtual bool SetDefaultStyle(ITextBoxTextAttr style)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.SetDefaultStyle(style);
        }

        /// <inheritdoc cref="SetDefaultStyle"/>
        public virtual bool SetDefaultRichStyle(ITextBoxRichAttr style)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.SetDefaultRichStyle(style);
        }

        /// <summary>
        /// Appends text and styles to the end of the text control.
        /// </summary>
        /// <param name="list">List containing strings or
        /// <see cref="ITextBoxTextAttr"/> instances.</param>
        /// <remarks>
        /// After the text is appended, the insertion point will be at the end
        /// of the text control. If this behavior is not desired,
        /// the programmer should use <see cref="GetInsertionPoint"/>
        /// and <see cref="SetInsertionPoint"/>.
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
                {
                    var s = item.ToString()!;
                    if (s == "<b>")
                        BeginBold();
                    else
                    if (s == "</b>")
                        EndBold();
                    else
                    if (s == "<i>")
                        BeginItalic();
                    else
                    if (s == "</i>")
                        EndItalic();
                    else
                    if (s == "<u>")
                        BeginUnderline();
                    else
                    if (s == "</u>")
                        EndUnderline();
                    else
                        WriteText(s);
                }
            }
        }

        /// <summary>
        /// Sets the position that should be shown when full (delayed) layout is performed.
        /// </summary>
        /// <param name="p"></param>
        public virtual void SetFullLayoutSavedPosition(long p)
        {
            if (DisposingOrDisposed)
                return;
            Handler.SetFullLayoutSavedPosition(p);
        }

        /// <summary>
        /// Forces any pending layout due to delayed, partial layout when the control was resized.
        /// </summary>
        public virtual void ForceDelayedLayout()
        {
            if (DisposingOrDisposed)
                return;
            Handler.ForceDelayedLayout();
        }

        /// <summary>
        /// Returns <c>true</c> if we are showing the caret position at the start of a line
        /// instead of at the end of the previous one.
        /// </summary>
        /// <returns></returns>
        public virtual bool GetCaretAtLineStart()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.GetCaretAtLineStart();
        }

        /// <summary>
        /// Sets a flag to remember that we are showing the caret position at the start of a line
        /// instead of at the end of the previous one.
        /// </summary>
        /// <param name="atStart"></param>
        public virtual void SetCaretAtLineStart(bool atStart)
        {
            if (DisposingOrDisposed)
                return;
            Handler.SetCaretAtLineStart(atStart);
        }

        /// <summary>
        /// Returns <c>true</c> if we are dragging a selection.
        /// </summary>
        /// <returns></returns>
        public virtual bool GetDragging()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.GetDragging();
        }

        /// <summary>
        /// Shows 'Go To Line' dialog.
        /// </summary>
        public virtual void ShowDialogGoToLine()
        {
            TextBoxUtils.ShowDialogGoToLine(this);
        }

        /// <summary>
        /// Sets a flag to remember if we are dragging a selection.
        /// </summary>
        /// <param name="dragging"></param>
        public virtual void SetDragging(bool dragging)
        {
            if (DisposingOrDisposed)
                return;
            Handler.SetDragging(dragging);
        }

        /// <summary>
        /// Returns an anchor so we know how to extend the selection.
        /// It's a caret position since it's between two characters.
        /// </summary>
        public virtual long GetSelectionAnchor()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.GetSelectionAnchor();
        }

        /// <summary>
        /// Sets an anchor so we know how to extend the selection.
        /// It's a caret position since it's between two characters.
        /// </summary>
        public virtual void SetSelectionAnchor(long anchor)
        {
            if (DisposingOrDisposed)
                return;
            Handler.SetSelectionAnchor(anchor);
        }

        /// <summary>
        /// Clears the buffer content, leaving a single empty paragraph. Cannot be undone.
        /// </summary>
        public virtual void Clear()
        {
            if (DisposingOrDisposed)
                return;
            Handler.Clear();
        }

        /// <summary>
        /// Replaces the content in the specified range with the string specified by a value.
        /// </summary>
        public virtual void Replace(long from, long to, string value)
        {
            if (DisposingOrDisposed)
                return;
            Handler.Replace(from, to, value);
        }

        /// <summary>
        /// Removes the content in the specified range.
        /// </summary>
        public virtual void Remove(long from, long to)
        {
            if (DisposingOrDisposed)
                return;
            Handler.Remove(from, to);
        }

        /// <summary>
        /// Loads content into the control's buffer using the given type.
        /// </summary>
        /// <param name="file">File name.</param>
        /// <param name="type">File type.</param>
        /// <remarks>
        /// If the specified type is <see cref="RichTextFileType.Any"/>, the type is deduced from
        /// the filename extension.
        /// </remarks>
        /// <remarks>
        /// Use <see cref="SetFileHandlerFlags"/> to setup load options.
        /// </remarks>
        public virtual bool LoadFromFile(string file, RichTextFileType type = RichTextFileType.Any)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.LoadFromFile(file, type);
        }

        /// <summary>
        /// Saves the buffer content using the given type.
        /// </summary>
        /// <remarks>
        /// If the specified type is <see cref="RichTextFileType.Any"/>, the type is deduced from
        /// the filename extension.
        /// </remarks>
        /// <param name="file">File name.</param>
        /// <param name="type">File type.</param>
        /// <remarks>
        /// Use <see cref="SetFileHandlerFlags"/> to setup save options.
        /// </remarks>
        public virtual bool SaveToFile(string file, RichTextFileType type = RichTextFileType.Any)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.SaveToFile(file, type);
        }

        /// <summary>
        /// Saves the buffer content to <see cref="Stream"/> using the given data type.
        /// </summary>
        /// <param name="stream">Output stream.</param>
        /// <param name="type">File type.</param>
        /// <remarks>
        /// Use <see cref="SetFileHandlerFlags"/> to setup save options.
        /// </remarks>
        /// <returns>
        /// <c>true</c> on success, <c>false</c> if an error occurred.
        /// </returns>
        public virtual bool SaveToStream(Stream stream, RichTextFileType type)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.SaveToStream(stream, type);
        }

        /// <summary>
        /// Loads the buffer content from <see cref="Stream"/> using the given data type.
        /// </summary>
        /// <param name="stream">Output stream.</param>
        /// <param name="type">File type.</param>
        /// <remarks>
        /// Use <see cref="SetFileHandlerFlags"/> to setup save options.
        /// </remarks>
        /// <returns>
        /// <c>true</c> on success, <c>false</c> if an error occurred.
        /// </returns>
        public virtual bool LoadFromStream(Stream stream, RichTextFileType type)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.LoadFromStream(stream, type);
        }

        /// <summary>
        /// Sets flags that change the behavior of loading or saving.
        /// See the documentation for each handler class to see what flags are
        /// relevant for each handler.
        /// </summary>
        public virtual void SetFileHandlerFlags(RichTextHandlerFlags knownFlags, int customFlags = 0)
        {
            if (DisposingOrDisposed)
                return;
            Handler.SetFileHandlerFlags(knownFlags, customFlags);
        }

        /// <summary>
        /// Returns flags that change the behavior of loading or saving.
        /// See the documentation for each handler class to see what flags are
        /// relevant for each handler.
        /// </summary>
        public virtual int GetFileHandlerFlags()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.GetFileHandlerFlags();
        }

        /// <summary>
        /// Marks the buffer as modified.
        /// </summary>
        public virtual void MarkDirty()
        {
            if (DisposingOrDisposed)
                return;
            Handler.MarkDirty();
        }

        /// <summary>
        /// Sets the buffer's modified status to false, and clears the buffer's command history.
        /// </summary>
        public virtual void DiscardEdits()
        {
            if (DisposingOrDisposed)
                return;
            Handler.DiscardEdits();
        }

        /// <summary>
        /// Sets the maximum number of characters that may be entered in a single line
        /// text control. For compatibility only; currently does nothing.
        /// </summary>
        public virtual void SetMaxLength(ulong len)
        {
            if (DisposingOrDisposed)
                return;
            Handler.SetMaxLength(len);
        }

        /// <summary>
        /// Writes text at the current position.
        /// </summary>
        public virtual void WriteText(string text)
        {
            if (DisposingOrDisposed)
                return;
            Handler.WriteText(text);
        }

        /// <summary>
        /// Writes text at the current position and moves caret
        /// to the beginning of the next line.
        /// </summary>
        public virtual void WriteLineText(string text)
        {
            if (DisposingOrDisposed)
                return;
            WriteText(text);
            Handler.NewLine();
        }

        /// <summary>
        /// Sets the insertion point to the end of the buffer and writes the text.
        /// </summary>
        public virtual void AppendText(string text)
        {
            if (DisposingOrDisposed)
                return;
            Handler.AppendText(text);
        }

        /// <summary>
        /// Translates from column and line number to position.
        /// </summary>
        public virtual long XYToPosition(long x, long y)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.XYToPosition(x, y);
        }

        /// <summary>
        /// Scrolls the buffer so that the caret position becomes visible in the view.
        /// </summary>
        public virtual void ScrollToCaret()
        {
            var caretPosition = GetCaretPosition();
            ShowPosition(caretPosition);
        }

        /// <summary>
        /// Scrolls the buffer so that the given position is in view.
        /// </summary>
        public virtual void ShowPosition(long pos)
        {
            if (DisposingOrDisposed)
                return;
            Handler.ShowPosition(pos);
        }

        /// <summary>
        /// Copies the selected content (if any) to the clipboard.
        /// </summary>
        public virtual void Copy()
        {
            if (DisposingOrDisposed)
                return;
            Handler.Copy();
        }

        /// <summary>
        /// Copies the selected content (if any) to the clipboard and deletes the selection.
        /// This is undoable.
        /// </summary>
        public virtual void Cut()
        {
            if (DisposingOrDisposed)
                return;
            Handler.Cut();
        }

        /// <summary>
        /// Pastes content from the clipboard to the buffer.
        /// </summary>
        public virtual void Paste()
        {
            if (DisposingOrDisposed)
                return;
            Handler.Paste();
        }

        /// <summary>
        /// Deletes the content in the selection, if any. This is undoable.
        /// </summary>
        public virtual void DeleteSelection()
        {
            if (DisposingOrDisposed)
                return;
            Handler.DeleteSelection();
        }

        /// <summary>
        /// Returns <c>true</c> if selected content can be copied to the clipboard.
        /// </summary>
        public virtual bool CanCopy()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.CanCopy();
        }

        /// <summary>
        /// Returns <c>true</c> if selected content can be copied to the clipboard and deleted.
        /// </summary>
        public virtual bool CanCut()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.CanCut();
        }

        /// <summary>
        /// Returns <c>true</c> if the clipboard content can be pasted to the buffer.
        /// </summary>
        public virtual bool CanPaste()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.CanPaste();
        }

        /// <summary>
        /// Returns <c>true</c> if selected content can be deleted.
        /// </summary>
        public virtual bool CanDeleteSelection()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.CanDeleteSelection();
        }

        /// <summary>
        /// Undoes the command at the top of the command history, if there is one.
        /// </summary>
        public virtual void Undo()
        {
            if (DisposingOrDisposed)
                return;
            Handler.Undo();
        }

        /// <summary>
        /// Redoes the current command.
        /// </summary>
        public virtual void Redo()
        {
            if (DisposingOrDisposed)
                return;
            Handler.Redo();
        }

        /// <summary>
        /// Returns <c>true</c> if there is a command in the command history that can be undone.
        /// </summary>
        public virtual bool CanUndo()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.CanUndo();
        }

        /// <summary>
        /// Returns <c>true</c> if there is a command in the command history that can be redone.
        /// </summary>
        public virtual bool CanRedo()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.CanRedo();
        }

        /// <summary>
        /// Sets the insertion point and causes the current editing style to be taken from
        /// the new position (unlike <see cref="SetCaretPosition"/>).
        /// </summary>
        public virtual void SetInsertionPoint(long pos)
        {
            if (DisposingOrDisposed)
                return;
            Handler.SetInsertionPoint(pos);
        }

        /// <summary>
        /// Sets the insertion point to the end of the text control.
        /// </summary>
        public virtual void SetInsertionPointEnd()
        {
            if (DisposingOrDisposed)
                return;
            Handler.SetInsertionPointEnd();
        }

        /// <summary>
        /// Returns the current insertion point.
        /// </summary>
        public virtual long GetInsertionPoint()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.GetInsertionPoint();
        }

        /// <summary>
        /// Sets the selection to the given range.
        /// The end point of range is specified as the last character position of the span
        /// of text, plus one.
        /// So, for example, to set the selection for a character at position 5, use the
        /// range (5,6).
        /// </summary>
        public virtual void SetSelection(long from, long to)
        {
            if (DisposingOrDisposed)
                return;
            Handler.SetSelection(from, to);
        }

        /// <summary>
        /// Makes the control editable, or not.
        /// </summary>
        public virtual void SetEditable(bool editable)
        {
            if (DisposingOrDisposed)
                return;
            Handler.SetEditable(editable);
        }

        /// <summary>
        /// Returns <c>true</c> if there is a selection and the object containing the selection
        /// was the same as the current focus object.
        /// </summary>
        public virtual bool HasSelection()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.HasSelection();
        }

        /// <summary>
        /// Returns <c>true</c> if there was a selection, whether or not the current focus object
        /// is the same as the selection's container object.
        /// </summary>
        public virtual bool HasUnfocusedSelection()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.HasUnfocusedSelection();
        }

        /// <summary>
        /// Inserts a new paragraph at the current insertion point. See <see cref="LineBreak"/>.
        /// </summary>
        public virtual bool NewLine() => NewLine(1);

        /// <summary>
        /// Inserts new paragraphs at the current insertion point. See <see cref="LineBreak"/>.
        /// </summary>
        public virtual bool NewLine(int count)
        {
            if (DisposingOrDisposed)
                return default;
            for (int i = 0; i < count; i++)
            {
                var result = Handler.NewLine();
                if (!result)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Inserts a line break at the current insertion point.
        /// A line break forces wrapping within a paragraph, and can be introduced by
        /// using this function or by typing Shift-Return.
        /// </summary>
        public virtual bool LineBreak()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.LineBreak();
        }

        /// <summary>
        /// Ends the current style.
        /// </summary>
        public virtual bool EndStyle()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.EndStyle();
        }

        /// <summary>
        /// Ends application of all styles in the current style stack.
        /// </summary>
        public virtual bool EndAllStyles()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.EndAllStyles();
        }

        /// <summary>
        /// Begins using bold.
        /// </summary>
        public virtual bool BeginBold()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.BeginBold();
        }

        /// <summary>
        /// Ends using bold.
        /// </summary>
        public virtual bool EndBold()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.EndBold();
        }

        /// <summary>
        /// Begins using italic.
        /// </summary>
        public virtual bool BeginItalic()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.BeginItalic();
        }

        /// <summary>
        /// Ends using italic.
        /// </summary>
        public virtual bool EndItalic()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.EndItalic();
        }

        /// <summary>
        /// Begins using underlining.
        /// </summary>
        public virtual bool BeginUnderline()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.BeginUnderline();
        }

        /// <summary>
        /// End applying underlining.
        /// </summary>
        public virtual bool EndUnderline()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.EndUnderline();
        }

        /// <summary>
        /// Begins using the given point size.
        /// </summary>
        public virtual bool BeginFontSize(int pointSize)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.BeginFontSize(pointSize);
        }

        /// <summary>
        /// Begins using the given point size.
        /// </summary>
        public virtual bool BeginFontSize(Coord pointSize)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.BeginFontSize((int)pointSize);
        }

        /// <summary>
        /// Ends using a point size.
        /// </summary>
        public virtual bool EndFontSize()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.EndFontSize();
        }

        /// <summary>
        /// Ends using a font.
        /// </summary>
        public virtual bool EndFont()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.EndFont();
        }

        /// <summary>
        /// Begins using this color.
        /// </summary>
        public virtual bool BeginTextColor(Color color)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.BeginTextColor(color);
        }

        /// <summary>
        /// Ends applying a text color.
        /// </summary>
        public virtual bool EndTextColor()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.EndTextColor();
        }

        /// <summary>
        /// Begins using alignment.
        /// </summary>
        public virtual bool BeginAlignment(TextBoxTextAttrAlignment alignment)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.BeginAlignment(alignment);
        }

        /// <summary>
        /// Ends alignment.
        /// </summary>
        public virtual bool EndAlignment()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.EndAlignment();
        }

        /// <summary>
        /// Begins applying a left indent and sub-indent in tenths of a millimeter.
        /// </summary>
        /// <param name="leftIndent">
        /// The distance between the margin and the bullet.
        /// </param>
        /// <param name="leftSubIndent">
        /// The distance between the left edge of the bullet and the left edge
        /// of the actual paragraph.
        /// </param>
        /// <returns></returns>
        /// <remarks>
        /// The sub-indent is an offset from the left edge of the paragraph, and is
        /// used for all but the first line in a paragraph. A positive value will
        /// cause the first line to appear to the left of the subsequent lines, and
        /// a negative value will cause the first line to be indented to the right
        /// of the subsequent lines.
        /// Control uses indentation to render a bulleted item. The
        /// content of the paragraph, including the first line, starts at the
        /// leftIndent plus the leftSubIndent.
        /// </remarks>
        public virtual bool BeginLeftIndent(int leftIndent, int leftSubIndent = 0)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.BeginLeftIndent(leftIndent, leftSubIndent);
        }

        /// <summary>
        /// Ends left indent.
        /// </summary>
        public virtual bool EndLeftIndent()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.EndLeftIndent();
        }

        /// <summary>
        /// Begins a right indent, specified in tenths of a millimeter.
        /// </summary>
        public virtual bool BeginRightIndent(int rightIndent)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.BeginRightIndent(rightIndent);
        }

        /// <summary>
        /// Ends right indent.
        /// </summary>
        public virtual bool EndRightIndent()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.EndRightIndent();
        }

        /// <summary>
        /// Begins paragraph spacing; pass the before-paragraph and after-paragraph spacing
        /// in tenths of a millimeter.
        /// </summary>
        public virtual bool BeginParagraphSpacing(int before, int after)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.BeginParagraphSpacing(before, after);
        }

        /// <summary>
        /// Ends paragraph spacing.
        /// </summary>
        public virtual bool EndParagraphSpacing()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.EndParagraphSpacing();
        }

        /// <summary>
        /// Begins applying line spacing.
        /// </summary>
        /// <param name="lineSpacing">A multiple, where 10 means
        /// single-spacing, 15 means 1.5 spacing, and 20 means double spacing.</param>
        /// <remarks>
        /// The <see cref="TextBoxTextAttrLineSpacing"/> constants are defined for convenience.
        /// </remarks>
        /// <returns></returns>
        public virtual bool BeginLineSpacing(int lineSpacing)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.BeginLineSpacing(lineSpacing);
        }

        /// <summary>
        /// Ends line spacing.
        /// </summary>
        public virtual bool EndLineSpacing()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.EndLineSpacing();
        }

        /// <summary>
        /// Begins a numbered bullet.
        /// </summary>
        /// <param name="bulletNumber">A number, usually starting with 1</param>
        /// <param name="leftIndent">A value in tenths of a millimeter.</param>
        /// <param name="leftSubIndent">A value in tenths of a millimeter.</param>
        /// <param name="bulletStyle">Bullet style.</param>
        /// <remarks>
        /// This call will be needed for each item in the list, and the
        /// application should take care of incrementing the numbering.
        /// </remarks>
        /// <remarks>
        /// Control uses indentation to render a bulleted item.
        /// The left indent is the distance between the margin and the bullet.
        /// The content of the paragraph, including the first line, starts
        /// at leftMargin + leftSubIndent.
        /// So the distance between the left edge of the bullet and the
        /// left of the actual paragraph is leftSubIndent.
        /// </remarks>
        /// <returns></returns>
        public virtual bool BeginNumberedBullet(
            int bulletNumber,
            int leftIndent,
            int leftSubIndent,
            TextBoxTextAttrBulletStyle bulletStyle
            = TextBoxTextAttrBulletStyle.Arabic | TextBoxTextAttrBulletStyle.Period)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.BeginNumberedBullet(
                bulletNumber,
                leftIndent,
                leftSubIndent,
                bulletStyle);
        }

        /// <summary>
        /// Ends application of a numbered bullet.
        /// </summary>
        public virtual bool EndNumberedBullet()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.EndNumberedBullet();
        }

        /// <summary>
        /// Begins applying a symbol bullet, using a character from the current font.
        /// See <see cref="BeginNumberedBullet"/> for an explanation of how indentation is used
        /// to render the bulleted paragraph.
        /// </summary>
        public virtual bool BeginSymbolBullet(
            string symbol,
            int leftIndent,
            int leftSubIndent,
            TextBoxTextAttrBulletStyle bulletStyle = TextBoxTextAttrBulletStyle.Symbol)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.BeginSymbolBullet(
                symbol,
                leftIndent,
                leftSubIndent,
                bulletStyle);
        }

        /// <summary>
        /// Ends applying a symbol bullet.
        /// </summary>
        public virtual bool EndSymbolBullet()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.EndSymbolBullet();
        }

        /// <summary>
        /// Begins applying a symbol bullet.
        /// </summary>
        public virtual bool BeginStandardBullet(
            string bulletName,
            int leftIndent,
            int leftSubIndent,
            TextBoxTextAttrBulletStyle bulletStyle = TextBoxTextAttrBulletStyle.Standard)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.BeginStandardBullet(
                bulletName,
                leftIndent,
                leftSubIndent,
                bulletStyle);
        }

        /// <summary>
        /// Begins applying a standard bullet.
        /// </summary>
        public virtual bool EndStandardBullet()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.EndStandardBullet();
        }

        /// <summary>
        /// Begins using the named character style.
        /// </summary>
        public virtual bool BeginCharacterStyle(string characterStyle)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.BeginCharacterStyle(characterStyle);
        }

        /// <summary>
        /// Ends application of a named character style.
        /// </summary>
        public virtual bool EndCharacterStyle()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.EndCharacterStyle();
        }

        /// <summary>
        /// Begins applying the named paragraph style.
        /// </summary>
        public virtual bool BeginParagraphStyle(string paragraphStyle)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.BeginParagraphStyle(paragraphStyle);
        }

        /// <summary>
        /// Ends application of a named paragraph style.
        /// </summary>
        public virtual bool EndParagraphStyle()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.EndParagraphStyle();
        }

        /// <summary>
        /// Begins using a specified list style.
        /// Optionally, you can also pass a level and a number.
        /// </summary>
        public virtual bool BeginListStyle(string listStyle, int level = 1, int number = 1)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.BeginListStyle(listStyle, level, number);
        }

        /// <summary>
        /// Ends using a specified list style.
        /// </summary>
        public virtual bool EndListStyle()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.EndListStyle();
        }

        /// <summary>
        /// Begins applying <see cref="TextBoxTextAttrFlags.Url"/> to the content.
        /// Pass a URL and optionally, a character style to apply, since it is common
        /// to mark a URL with a familiar style such as blue text with underlining.
        /// </summary>
        public virtual bool BeginURL(string url, string? characterStyle = default)
        {
            if (DisposingOrDisposed)
                return default;
            characterStyle ??= string.Empty;
            return Handler.BeginURL(url, characterStyle);
        }

        /// <summary>
        /// Ends applying a URL.
        /// </summary>
        public virtual bool EndURL()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.EndURL();
        }

        /// <summary>
        /// Returns <c>true</c> if all of the selection, or the content
        /// at the caret position, is bold.
        /// </summary>
        public virtual bool IsSelectionBold()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.IsSelectionBold();
        }

        /// <summary>
        /// Returns <c>true</c> if all of the selection, or the content
        /// at the caret position, is italic.
        /// </summary>
        public virtual bool IsSelectionItalics()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.IsSelectionItalics();
        }

        /// <summary>
        /// Returns <c>true</c> if all of the selection, or the content
        /// at the caret position, is underlined.
        /// </summary>
        public virtual bool IsSelectionUnderlined()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.IsSelectionUnderlined();
        }

        /// <summary>
        /// Returns <c>true</c> if all of the selection, or the content
        /// at the current caret position, has the supplied effects flag(s).
        /// </summary>
        public virtual bool DoesSelectionHaveTextEffectFlag(TextBoxTextAttrEffects flag)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.DoesSelectionHaveTextEffectFlag(flag);
        }

        /// <summary>
        /// Returns <c>true</c> if all of the selection, or the content
        /// at the caret position, is aligned according to the specified flag.
        /// </summary>
        public virtual bool IsSelectionAligned(TextBoxTextAttrAlignment alignment)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.IsSelectionAligned(alignment);
        }

        /// <summary>
        /// Apples bold to the selection or default style (undoable).
        /// </summary>
        public virtual bool ApplyBoldToSelection()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.ApplyBoldToSelection();
        }

        /// <summary>
        /// Applies italic to the selection or default style (undoable).
        /// </summary>
        public virtual bool ApplyItalicToSelection()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.ApplyItalicToSelection();
        }

        /// <summary>
        /// Applies underline to the selection or default style (undoable).
        /// </summary>
        public virtual bool ApplyUnderlineToSelection()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.ApplyUnderlineToSelection();
        }

        /// <summary>
        /// Applies one or more <see cref="TextBoxTextAttrEffects"/> flags to the selection (undoable).
        /// If there is no selection, it is applied to the default style.
        /// </summary>
        public virtual bool ApplyTextEffectToSelection(TextBoxTextAttrEffects flags)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.ApplyTextEffectToSelection(flags);
        }

        /// <summary>
        /// Applies the given alignment to the selection or the default style (undoable).
        /// </summary>
        public virtual bool ApplyAlignmentToSelection(TextBoxTextAttrAlignment alignment)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.ApplyAlignmentToSelection(alignment);
        }

        /// <summary>
        /// Sets the default style to the style under the cursor.
        /// </summary>
        public virtual bool SetDefaultStyleToCursorStyle()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.SetDefaultStyleToCursorStyle();
        }

        /// <summary>
        /// Cancels any selection.
        /// </summary>
        public virtual void SelectNone()
        {
            if (DisposingOrDisposed)
                return;
            Handler.SelectNone();
        }

        /// <summary>
        /// Selects the word at the given character position.
        /// </summary>
        public virtual bool SelectWord(long position)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.SelectWord(position);
        }

        /// <summary>
        /// Lays out the buffer, which must be done before certain operations, such as
        /// setting the caret position.
        /// This function should not normally be required by the application.
        /// </summary>
        public virtual bool LayoutContent(bool onlyVisibleRect = false)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.LayoutContent(onlyVisibleRect);
        }

        /// <summary>
        /// Moves right.
        /// </summary>
        public virtual bool MoveRight(int noPositions = 1, RichTextMoveCaretFlags flags = 0)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.MoveRight(noPositions, flags);
        }

        /// <summary>
        /// Moves left.
        /// </summary>
        public virtual bool MoveLeft(int noPositions = 1, RichTextMoveCaretFlags flags = 0)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.MoveLeft(noPositions, flags);
        }

        /// <summary>
        /// Moves to the start of the paragraph.
        /// </summary>
        public virtual bool MoveUp(int noLines = 1, RichTextMoveCaretFlags flags = 0)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.MoveUp(noLines, flags);
        }

        /// <summary>
        /// Moves the caret down.
        /// </summary>
        public virtual bool MoveDown(int noLines = 1, RichTextMoveCaretFlags flags = 0)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.MoveDown(noLines, flags);
        }

        /// <summary>
        /// Moves to the end of the line.
        /// </summary>
        public virtual bool MoveToLineEnd(RichTextMoveCaretFlags flags = 0)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.MoveToLineEnd(flags);
        }

        /// <summary>
        /// Moves to the start of the line.
        /// </summary>
        public virtual bool MoveToLineStart(RichTextMoveCaretFlags flags = 0)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.MoveToLineStart(flags);
        }

        /// <summary>
        /// Moves to the end of the paragraph.
        /// </summary>
        public virtual bool MoveToParagraphEnd(RichTextMoveCaretFlags flags = 0)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.MoveToParagraphEnd(flags);
        }

        /// <summary>
        /// Moves to the start of the paragraph.
        /// </summary>
        public virtual bool MoveToParagraphStart(RichTextMoveCaretFlags flags = 0)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.MoveToParagraphStart(flags);
        }

        /// <summary>
        /// Moves to the start of the buffer.
        /// </summary>
        public virtual bool MoveHome(RichTextMoveCaretFlags flags = 0)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.MoveHome(flags);
        }

        /// <summary>
        /// Moves to the end of the buffer.
        /// </summary>
        public virtual bool MoveEnd(RichTextMoveCaretFlags flags = 0)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.MoveEnd(flags);
        }

        /// <summary>
        /// Moves one or more pages up.
        /// </summary>
        public virtual bool PageUp(int noPages = 1, RichTextMoveCaretFlags flags = 0)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.PageUp(noPages, flags);
        }

        /// <summary>
        /// Moves one or more pages down.
        /// </summary>
        public virtual bool PageDown(int noPages = 1, RichTextMoveCaretFlags flags = 0)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.PageDown(noPages, flags);
        }

        /// <summary>
        /// Moves a number of words to the left.
        /// </summary>
        public virtual bool WordLeft(int noPages = 1, RichTextMoveCaretFlags flags = 0)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.WordLeft(noPages, flags);
        }

        /// <summary>
        /// Move a number of words to the right.
        /// </summary>
        public virtual bool WordRight(int noPages = 1, RichTextMoveCaretFlags flags = 0)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.WordRight(noPages, flags);
        }

        /// <summary>
        /// Starts batching undo history for commands.
        /// </summary>
        public virtual bool BeginBatchUndo(string cmdName)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.BeginBatchUndo(cmdName);
        }

        /// <summary>
        /// Ends batching undo command history.
        /// </summary>
        public virtual bool EndBatchUndo()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.EndBatchUndo();
        }

        /// <summary>
        /// Returns <c>true</c> if undo commands are being batched.
        /// </summary>
        public virtual bool BatchingUndo()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.BatchingUndo();
        }

        /// <summary>
        /// Starts suppressing undo history for commands.
        /// </summary>
        public virtual bool BeginSuppressUndo()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.BeginSuppressUndo();
        }

        /// <summary>
        /// Ends suppressing undo command history.
        /// </summary>
        public virtual bool EndSuppressUndo()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.EndSuppressUndo();
        }

        /// <summary>
        /// Returns the current default style, which can be used to change how subsequently
        /// inserted text is displayed.
        /// </summary>
        public ITextBoxRichAttr GetDefaultStyleEx()
        {
            if (DisposingOrDisposed)
                return PlessTextBoxRichAttr.Empty;
            return Handler.GetDefaultStyleEx();
        }

        /// <summary>
        /// Returns <c>true</c> if undo history suppression is on.
        /// </summary>
        public virtual bool SuppressingUndo()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.SuppressingUndo();
        }

        /// <summary>
        /// Enable or disable the vertical scrollbar.
        /// </summary>
        public virtual void EnableVerticalScrollbar(bool enable)
        {
            if (DisposingOrDisposed)
                return;
            Handler.EnableVerticalScrollbar(enable);
        }

        /// <summary>
        /// Returns <c>true</c> if the vertical scrollbar is enabled.
        /// </summary>
        public virtual bool GetVerticalScrollbarEnabled()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.GetVerticalScrollbarEnabled();
        }

        /// <summary>
        /// Sets the scale factor for displaying fonts, for example for more comfortable editing.
        /// </summary>
        public virtual void SetFontScale(Coord fontScale, bool refresh = false)
        {
            if (DisposingOrDisposed)
                return;
            Handler.SetFontScale(fontScale, refresh);
        }

        /// <summary>
        /// Returns the scale factor for displaying fonts, for example for more comfortable editing.
        /// </summary>
        public virtual Coord GetFontScale()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.GetFontScale();
        }

        /// <summary>
        /// Returns <c>true</c> if this control can use attributes and text. The default is false.
        /// </summary>
        public virtual bool GetVirtualAttributesEnabled()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.GetVirtualAttributesEnabled();
        }

        /// <summary>
        /// Pass <c>true</c> to let the control use attributes. The default is false.
        /// </summary>
        public virtual void EnableVirtualAttributes(bool b)
        {
            if (DisposingOrDisposed)
                return;
            Handler.EnableVirtualAttributes(b);
        }

        /// <summary>
        /// Writes text.
        /// </summary>
        public virtual void DoWriteText(string value, TextBoxSetValueFlags flags = 0)
        {
            if (DisposingOrDisposed)
                return;
            Handler.DoWriteText(value, flags);
        }

        /// <summary>
        /// Helper function for extending the selection, returning <c>true</c> if the selection
        /// was changed. Selections are in caret positions.
        /// </summary>
        public virtual bool ExtendSelection(
            long oldPosition,
            long newPosition,
            RichTextMoveCaretFlags flags)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.ExtendSelection(oldPosition, newPosition, flags);
        }

        /// <summary>
        /// Sets the caret position.
        /// </summary>
        /// <remarks>
        /// The caret position is the character position just before the caret.
        /// A value of -1 means the caret is at the start of the buffer.
        /// Please note that this does not update the current editing style
        /// from the new position or cause the actual caret to be refreshed; to do that,
        /// call <see cref="SetInsertionPoint"/> instead.
        /// </remarks>
        public virtual void SetCaretPosition(long position, bool showAtLineStart = false)
        {
            if (DisposingOrDisposed)
                return;
            Handler.SetCaretPosition(position, showAtLineStart);
        }

        /// <summary>
        /// Returns the current caret position.
        /// </summary>
        public virtual long GetCaretPosition()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.GetCaretPosition();
        }

        /// <summary>
        /// Gets the adjusted caret position.
        /// </summary>
        /// <remarks>
        /// The adjusted caret position is the character position adjusted to take
        /// into account whether we're at the start of a paragraph, in which case
        /// style information should be taken from the next position, not current one.
        /// </remarks>
        public virtual long GetAdjustedCaretPosition(long caretPos)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.GetAdjustedCaretPosition(caretPos);
        }

        /// <summary>
        /// Move the caret one visual step forward: this may mean setting a flag
        /// and keeping the same position if we're going from the end of one line
        /// to the start of the next, which may be the exact same caret position.
        /// </summary>
        public virtual void MoveCaretForward(long oldPosition)
        {
            if (DisposingOrDisposed)
                return;
            Handler.MoveCaretForward(oldPosition);
        }

        /// <summary>
        /// Transforms logical (not scrolled) position to physical window position.
        /// </summary>
        public virtual PointI GetPhysicalPoint(PointI ptLogical)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.GetPhysicalPoint(ptLogical);
        }

        /// <summary>
        /// Transforms physical window position to logical (not scrolled) position.
        /// </summary>
        public virtual PointI GetLogicalPoint(PointI ptPhysical)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.GetLogicalPoint(ptPhysical);
        }

        /// <summary>
        /// Helper function for finding the caret position for the next word.
        /// Direction is 1 (forward) or -1 (backwards).
        /// </summary>
        public virtual long FindNextWordPosition(int direction = 1)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.FindNextWordPosition(direction);
        }

        /// <summary>
        /// Returns <c>true</c> if the given position is visible on the screen.
        /// </summary>
        public virtual bool IsPositionVisible(long pos)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.IsPositionVisible(pos);
        }

        /// <summary>
        /// Returns the first visible position in the current view.
        /// </summary>
        public virtual long GetFirstVisiblePosition()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.GetFirstVisiblePosition();
        }

        /// <summary>
        /// Returns the caret position since the default formatting was changed. As
        /// soon as this position changes, we no longer reflect the default style
        /// in the UI. A value of -2 means that we should only reflect the style of the
        /// content under the caret.
        /// </summary>
        public virtual long GetCaretPositionForDefaultStyle()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.GetCaretPositionForDefaultStyle();
        }

        /// <summary>
        /// Set the caret position for the default style that the user is selecting.
        /// </summary>
        public virtual void SetCaretPositionForDefaultStyle(long pos)
        {
            if (DisposingOrDisposed)
                return;
            Handler.SetCaretPositionForDefaultStyle(pos);
        }

        /// <summary>
        /// Move the caret one visual step forward: this may mean setting a flag
        /// and keeping the same position if we're going from the end of one line
        /// to the start of the next, which may be the exact same caret position.
        /// </summary>
        public virtual void MoveCaretBack(long oldPosition)
        {
            if (DisposingOrDisposed)
                return;
            Handler.MoveCaretBack(oldPosition);
        }

        /// <summary>
        /// Begins using this font.
        /// </summary>
        public virtual bool BeginFont(Font? font)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.BeginFont(font);
        }

        /// <summary>
        /// Returns <c>true</c> if the user has recently set the default style without moving
        /// the caret, and therefore the UI needs to reflect the default style and not
        /// the style at the caret.
        /// </summary>
        public virtual bool IsDefaultStyleShowing()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.IsDefaultStyleShowing();
        }

        /// <summary>
        /// Returns the first visible point in the control.
        /// </summary>
        public virtual PointI GetFirstVisiblePoint()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.GetFirstVisiblePoint();
        }

        /// <summary>
        /// Enable or disable images.
        /// </summary>
        public virtual void EnableImages(bool b)
        {
            if (DisposingOrDisposed)
                return;
            Handler.EnableImages(b);
        }

        /// <summary>
        /// Returns <c>true</c> if images are enabled.
        /// </summary>
        public virtual bool GetImagesEnabled()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.GetImagesEnabled();
        }

        /// <summary>
        /// Enable or disable delayed image loading.
        /// </summary>
        public virtual void EnableDelayedImageLoading(bool b)
        {
            if (DisposingOrDisposed)
                return;
            Handler.EnableDelayedImageLoading(b);
        }

        /// <summary>
        /// Returns <c>true</c> if delayed image loading is enabled.
        /// </summary>
        public virtual bool GetDelayedImageLoading()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.GetDelayedImageLoading();
        }

        /// <summary>
        /// Gets the flag indicating that delayed image processing is required.
        /// </summary>
        public virtual bool GetDelayedImageProcessingRequired()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.GetDelayedImageProcessingRequired();
        }

        /// <summary>
        /// Sets the flag indicating that delayed image processing is required.
        /// </summary>
        public virtual void SetDelayedImageProcessingRequired(bool b)
        {
            if (DisposingOrDisposed)
                return;
            Handler.SetDelayedImageProcessingRequired(b);
        }

        /// <summary>
        /// Returns the last time delayed image processing was performed.
        /// </summary>
        public virtual long GetDelayedImageProcessingTime()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.GetDelayedImageProcessingTime();
        }

        /// <summary>
        /// Sets the last time delayed image processing was performed.
        /// </summary>
        public virtual void SetDelayedImageProcessingTime(long t)
        {
            if (DisposingOrDisposed)
                return;
            Handler.SetDelayedImageProcessingTime(t);
        }

        /// <summary>
        /// Returns the content of the entire control as a string.
        /// </summary>
        public virtual string GetValue()
        {
            if (DisposingOrDisposed)
                return string.Empty;
            return Handler.GetValue();
        }

        /// <summary>
        /// Replaces existing content with the given text.
        /// </summary>
        public virtual void SetValue(string value)
        {
            if (DisposingOrDisposed)
                return;
            Handler.SetValue(value);
        }

        /// <summary>
        /// Sets the line increment height in pixels.
        /// </summary>
        public virtual void SetLineHeight(int height)
        {
            if (DisposingOrDisposed)
                return;
            Handler.SetLineHeight(height);
        }

        /// <summary>
        /// Gets the line increment height in pixels.
        /// </summary>
        public virtual int GetLineHeight()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.GetLineHeight();
        }

        /// <summary>
        /// Does delayed image loading and garbage-collect other images.
        /// </summary>
        public virtual bool ProcessDelayedImageLoading(bool refresh)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.ProcessDelayedImageLoading(refresh);
        }

        /// <summary>
        /// Requests delayed image processing.
        /// </summary>
        public virtual void RequestDelayedImageProcessing()
        {
            if (DisposingOrDisposed)
                return;
            Handler.RequestDelayedImageProcessing();
        }

        /// <summary>
        /// Returns the last position in the buffer.
        /// </summary>
        public virtual long GetLastPosition()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.GetLastPosition();
        }

        /// <summary>
        /// Sets the list attributes for the given range, passing flags to determine how
        /// the attributes are set.
        /// </summary>
        /// <remarks>
        /// <paramref name="flags"/> is a bit list of the following:
        /// <see cref="RichTextSetStyleFlags.WithUndo"/>: specifies that this command will
        /// be undoable.
        /// <see cref="RichTextSetStyleFlags.Renumber"/>: specifies that numbering
        /// should start from <paramref name="startFrom"/>, otherwise existing attributes are used.
        /// <see cref="RichTextSetStyleFlags.SpecifyLevel"/>: specifies that
        /// <paramref name="specifiedLevel"/> should be used
        /// as the level for all paragraphs, otherwise the current indentation will be used.
        /// </remarks>
        public virtual bool SetListStyle(
            long startRange,
            long endRange,
            string defName,
            RichTextSetStyleFlags flags = RichTextSetStyleFlags.WithUndo,
            int startFrom = 1,
            int specifiedLevel = -1)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.SetListStyle(
                startRange,
                endRange,
                defName,
                flags,
                startFrom,
                specifiedLevel);
        }

        /// <summary>
        /// Clears the list style from the given range, clearing list-related attributes
        /// and applying any named paragraph style associated with each paragraph.
        /// </summary>
        /// <param name="startRange"></param>
        /// <param name="endRange"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        /// <remarks>
        /// <paramref name="flags"/> is a bit list of the following:
        /// <see cref="RichTextSetStyleFlags.WithUndo"/>: specifies that
        /// this command will be undoable.
        /// </remarks>
        public virtual bool ClearListStyle(
            long startRange,
            long endRange,
            RichTextSetStyleFlags flags = RichTextSetStyleFlags.WithUndo)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.ClearListStyle(startRange, endRange, flags);
        }

        /// <summary>
        /// Writes a table at the current insertion point, returning the table.
        /// If Null is returned, table was not created.
        /// </summary>
        public virtual object? WriteTable(
            int rows,
            int cols,
            ITextBoxRichAttr? tableAttr = default,
            ITextBoxRichAttr? cellAttr = default)
        {
            if (DisposingOrDisposed)
                return null;
            return Handler.WriteTable(
                rows,
                cols,
                tableAttr,
                cellAttr);
        }

        /// <summary>
        /// Numbers the paragraphs in the given range.
        /// </summary>
        /// <remarks>
        /// <paramref name="flags"/> is a bit list of the following:
        /// <see cref="RichTextSetStyleFlags.WithUndo"/>: specifies that this command will
        /// be undoable.
        /// <see cref="RichTextSetStyleFlags.Renumber"/>: specifies that numbering
        /// should start from <paramref name="startFrom"/>, otherwise existing attributes are used.
        /// <see cref="RichTextSetStyleFlags.SpecifyLevel"/>: specifies that
        /// <paramref name="specifiedLevel"/> should be used
        /// as the level for all paragraphs, otherwise the current indentation will be used.
        /// </remarks>
        public virtual bool NumberList(
            long startRange,
            long endRange,
            string defName,
            RichTextSetStyleFlags flags = RichTextSetStyleFlags.WithUndo,
            int startFrom = 1,
            int specifiedLevel = -1)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.NumberList(
                startRange,
                endRange,
                defName,
                flags,
                startFrom,
                specifiedLevel);
        }

        /// <summary>
        /// Creates default style for the urls.
        /// </summary>
        /// <returns></returns>
        public ITextBoxRichAttr CreateUrlAttr()
        {
            var urlStyle = CreateRichAttr();
            var color = IsDarkBackground ? DefaultUrlColorOnBlack : DefaultUrlColorOnWhite;

            urlStyle.SetTextColor(color).SetFontUnderlined(true);
            return urlStyle;
        }

        /// <summary>
        /// Gets the attributes at the given position.
        /// </summary>
        /// <remarks>
        /// This function gets the combined style - that is, the style you see on the
        /// screen as a result of combining base style, paragraph style and character
        /// style attributes.
        /// To get the character or paragraph style alone, use GetUncombinedStyle.
        /// </remarks>
        public virtual ITextBoxTextAttr GetStyle(long position)
        {
            if (DisposingOrDisposed)
                return PlessTextBoxRichAttr.Empty;
            return Handler.GetStyle(position);
        }

        /// <summary>
        /// Gets the attributes at the given position.
        /// </summary>
        /// <remarks>
        /// This function gets the combined style - that is, the style you see on the
        /// screen as a result of combining base style, paragraph style and character
        /// style attributes.
        /// To get the character or paragraph style alone, use GetUncombinedStyle.
        /// </remarks>
        public virtual ITextBoxRichAttr GetRichStyle(long position)
        {
            if (DisposingOrDisposed)
                return PlessTextBoxRichAttr.Empty;
            return Handler.GetRichStyle(position);
        }

        /// <summary>
        /// Sets the attributes for the given range, passing flags to determine how the
        /// attributes are set.
        /// </summary>
        /// <remarks>
        /// The end point of range is specified as the last character position of the span
        /// of text, plus one. So, for example, to set the style for a character at
        /// position 5, use the range (5,6).
        /// <paramref name="flags"/> may contain a bit list of the following values:
        /// <see cref="RichTextSetStyleFlags.None"/>,
        /// <see cref="RichTextSetStyleFlags.WithUndo"/>,
        /// <see cref="RichTextSetStyleFlags.Optimize"/>,
        /// <see cref="RichTextSetStyleFlags.ParagraphsOnly"/>,
        /// <see cref="RichTextSetStyleFlags.CharactersOnly"/>,
        /// <see cref="RichTextSetStyleFlags.Reset"/>,
        /// <see cref="RichTextSetStyleFlags.Remove"/>.
        /// </remarks>
        public virtual bool SetStyleEx(
            long startRange,
            long endRange,
            ITextBoxRichAttr style,
            RichTextSetStyleFlags flags = RichTextSetStyleFlags.WithUndo)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.SetStyleEx(startRange, endRange, style, flags);
        }

        /// <summary>
        /// Promotes or demotes the paragraphs in the given range.
        /// </summary>
        /// <remarks>
        /// A positive <paramref name="promoteBy"/> produces a smaller indent, and a negative number
        /// produces a larger indent. Pass flags to determine how the attributes are set.
        /// </remarks>
        /// <remarks>
        /// <paramref name="flags"/> is a bit list of the following:
        /// <see cref="RichTextSetStyleFlags.WithUndo"/>: specifies that this command will
        /// be undoable.
        /// <see cref="RichTextSetStyleFlags.Renumber"/>: specifies that numbering
        /// should start from start number, otherwise existing attributes are used.
        /// <see cref="RichTextSetStyleFlags.SpecifyLevel"/>: specifies that
        /// <paramref name="specifiedLevel"/> should be used
        /// as the level for all paragraphs, otherwise the current indentation will be used.
        /// </remarks>
        public virtual bool PromoteList(
            int promoteBy,
            long startRange,
            long endRange,
            string defName,
            RichTextSetStyleFlags flags = RichTextSetStyleFlags.WithUndo,
            int specifiedLevel = -1)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.PromoteList(
                promoteBy,
                startRange,
                endRange,
                defName,
                flags,
                specifiedLevel);
        }

        /// <summary>
        /// Sets the text (normal) cursor.
        /// </summary>
        public virtual void SetTextCursor(Cursor? cursor)
        {
            if (DisposingOrDisposed)
                return;
            Handler.SetTextCursor(cursor);
        }

        /// <summary>
        /// Returns the text (normal) cursor.
        /// </summary>
        public virtual Cursor GetTextCursor()
        {
            if (DisposingOrDisposed)
                return Cursors.Default;
            return Handler.GetTextCursor();
        }

        /// <summary>
        /// Sets the cursor to be used over URLs.
        /// </summary>
        public virtual void SetURLCursor(Cursor? cursor)
        {
            if (DisposingOrDisposed)
                return;
            Handler.SetURLCursor(cursor);
        }

        /// <summary>
        /// Returns the cursor to be used over URLs.
        /// </summary>
        public virtual Cursor GetURLCursor()
        {
            if (DisposingOrDisposed)
                return Cursors.Default;
            return Handler.GetURLCursor();
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
        /// Sets <paramref name="attr"/> as the default style and tells the control that the UI should
        /// reflect this attribute until the user moves the caret.
        /// </summary>
        public virtual void SetAndShowDefaultStyle(ITextBoxRichAttr attr)
        {
            if (DisposingOrDisposed)
                return;
            Handler.SetAndShowDefaultStyle(attr);
        }

        /// <summary>
        /// Sets the basic (overall) style.
        /// </summary>
        /// <remarks>
        /// This is the style of the whole buffer before further styles are applied,
        /// unlike the default style, which only affects the style currently being
        /// applied (for example, setting the default style to bold will cause
        /// subsequently inserted text to be bold).
        /// </remarks>
        public virtual void SetBasicStyle(ITextBoxRichAttr style)
        {
            if (DisposingOrDisposed)
                return;
            Handler.SetBasicStyle(style);
        }

        /// <summary>
        /// Test if this whole range has paragraph attributes of the specified kind.
        /// </summary>
        /// <remarks>
        /// If any of the attributes are different within the range, the test fails.
        /// You can use this to implement, for example, centering button updating.
        /// <paramref name="style"/> must have flags indicating which attributes are of interest.
        /// </remarks>
        public virtual bool HasParagraphAttributes(
            long startRange,
            long endRange,
            ITextBoxRichAttr style)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.HasParagraphAttributes(startRange, endRange, style);
        }

        /// <summary>
        /// Gets the basic (overall) style.
        /// </summary>
        /// <remarks>
        /// This is the style of the whole buffer before further styles are applied,
        /// unlike the default style, which only affects the style currently being
        /// applied (for example, setting the default style to bold will cause
        /// subsequently inserted text to be bold).
        /// </remarks>
        public virtual ITextBoxRichAttr GetBasicStyle()
        {
            if (DisposingOrDisposed)
                return PlessTextBoxRichAttr.Empty;
            return Handler.GetBasicStyle();
        }

        /// <summary>
        /// Deletes the content within the given range.
        /// </summary>
        public virtual bool Delete(long startRange, long endRange)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.Delete(startRange, endRange);
        }

        /// <summary>
        /// Begins applying a style.
        /// </summary>
        public virtual bool BeginStyle(ITextBoxRichAttr style)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.BeginStyle(style);
        }

        /// <summary>
        /// Sets the attributes for the given range.
        /// </summary>
        /// <remarks>
        /// The end point of range is specified as the last character position of the span
        /// of text, plus one. So, for example, to set the style for a character at
        /// position 5, use the range (5,6).
        /// </remarks>
        public virtual bool SetStyle(long start, long end, ITextBoxTextAttr style)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.SetStyle(start, end, style);
        }

        /// <summary>
        /// Sets the attributes for the given range.
        /// </summary>
        /// <remarks>
        /// The end point of range is specified as the last character position of the span
        /// of text, plus one. So, for example, to set the style for a character at
        /// position 5, use the range (5,6).
        /// </remarks>
        public virtual bool SetRichStyle(long start, long end, ITextBoxRichAttr style)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.SetRichStyle(start, end, style);
        }

        /// <summary>
        /// Sets the selection to the given range.
        /// The end point of range is specified as the last character position of the span
        /// of text, plus one.
        /// So, for example, to set the selection for a character at position 5, use the
        /// range (5,6).
        /// </summary>
        public virtual void SetSelectionRange(long startRange, long endRange)
        {
            if (DisposingOrDisposed)
                return;
            Handler.SetSelectionRange(startRange, endRange);
        }

        /// <summary>
        /// Converts a text position to zero-based column and line numbers.
        /// </summary>
        public virtual PointI PositionToXY(long pos)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.PositionToXY(pos);
        }

        /// <summary>
        /// Gets the attributes common to the specified range.
        /// Attributes that differ in value within the range will not be included
        /// in style flags.
        /// </summary>
        public virtual ITextBoxTextAttr GetStyleForRange(long startRange, long endRange)
        {
            if (DisposingOrDisposed)
                return PlessTextBoxRichAttr.Empty;
            return Handler.GetStyleForRange(startRange, endRange);
        }

        /// <summary>
        /// Gets the attributes common to the specified range.
        /// Attributes that differ in value within the range will not be included
        /// in style flags.
        /// </summary>
        public virtual ITextBoxRichAttr GetRichStyleForRange(long startRange, long endRange)
        {
            if (DisposingOrDisposed)
                return PlessTextBoxRichAttr.Empty;
            return Handler.GetRichStyleForRange(startRange, endRange);
        }

        /// <summary>
        /// Creates text attributes object.
        /// </summary>
        /// <returns></returns>
        public virtual ITextBoxTextAttr CreateTextAttr()
        {
            if (DisposingOrDisposed)
                return PlessTextBoxRichAttr.Empty;
            return Handler.CreateTextAttr();
        }

        /// <summary>
        /// Deletes content if there is a selection, e.g. when pressing a key.
        /// Returns the new caret position in @e newPos, or leaves it if there
        /// was no action. This is undoable.
        /// </summary>
        public virtual long DeleteSelectedContent()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.DeleteSelectedContent();
        }

        /// <summary>
        /// Writes svg image in the normal state at the current insertion point.
        /// </summary>
        /// <param name="image">Svg image.</param>
        /// <param name="size">Image size.</param>
        /// <param name="textAttr">Optional text attribute.</param>
        /// <returns></returns>
        public virtual bool WriteImageAsNormal(
            SvgImage? image,
            int size,
            ITextBoxRichAttr? textAttr = null)
        {
            if (DisposingOrDisposed)
                return default;
            if (image is null)
                return false;
            return WriteImage(image.AsNormalImage(size, IsDarkBackground));
        }

        /// <summary>
        /// Writes a bitmap at the current insertion point.
        /// Supply an optional type to use for internal and file storage of the raw data.
        /// </summary>
        public virtual bool WriteImage(
            Image? bitmap,
            BitmapType bitmapType = BitmapType.Png,
            ITextBoxRichAttr? textAttr = null)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.WriteImage(bitmap, bitmapType, textAttr);
        }

        /// <summary>
        /// Loads an image from a file and writes it at the current insertion point.
        /// </summary>
        public virtual bool WriteImage(
            string filename,
            BitmapType bitmapType = BitmapType.Png,
            ITextBoxRichAttr? textAttr = null)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.WriteImage(filename, bitmapType, textAttr);
        }

        /// <summary>
        /// Gets name of the file loaded into the control.
        /// </summary>
        /// <returns></returns>
        public virtual string GetFileName()
        {
            if (DisposingOrDisposed)
                return string.Empty;
            return Handler.GetFileName();
        }

        /// <summary>
        /// Sets name of the file loaded into the control.
        /// </summary>
        /// <returns></returns>
        public virtual void SetFileName(string value)
        {
            if (DisposingOrDisposed)
                return;
            Handler.SetFileName(value ?? string.Empty);
        }

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return ControlFactory.Handler.CreateRichTextBoxHandler(this);
        }

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            base.OnKeyDown(e);
            if (AllowAdditionalKeys)
                HandleAdditionalKeys(e);
        }
    }
}