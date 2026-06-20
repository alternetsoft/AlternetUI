using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains properties and methods which allow to work with text box control.
    /// </summary>
    public interface ITextBoxHandler : IDisposableObject
    {
        /// <inheritdoc cref="TextBox.HideSelection"/>
        bool HideSelection { get; set; }

        /// <inheritdoc cref="TextBox.WantTab"/>
        bool ProcessTab { get; set; }

        /// <inheritdoc cref="TextBox.ProcessEnter"/>
        bool ProcessEnter { get; set; }

        /// <inheritdoc cref="TextBox.IsPassword"/>
        bool IsPassword { get; set; }

        /// <inheritdoc cref="TextBox.AutoUrl"/>
        bool AutoUrl { get; set; }

        /// <inheritdoc cref="TextBox.HideVertScrollbar"/>
        bool HideVertScrollbar { get; set; }

        /// <inheritdoc cref="TextBox.HasSelection"/>
        bool HasSelection { get; }

        /// <inheritdoc cref="TextBox.IsModified"/>
        bool IsModified { get; set; }

        /// <inheritdoc cref="TextBox.CanCopy"/>
        bool CanCopy { get; }

        /// <inheritdoc cref="TextBox.CanCut"/>
        bool CanCut { get; }

        /// <inheritdoc cref="TextBox.CanPaste"/>
        bool CanPaste { get; }

        /// <inheritdoc cref="TextBox.CanRedo"/>
        bool CanRedo { get; }

        /// <inheritdoc cref="TextBox.CanUndo"/>
        bool CanUndo { get; }

        /// <inheritdoc cref="TextBox.IsEmpty"/>
        bool IsEmpty { get; }

        /// <inheritdoc cref="TextBox.EmptyTextHint"/>
        string EmptyTextHint { get; set; }

        /// <inheritdoc cref="TextBox.Multiline"/>
        bool Multiline { get; set; }

        /// <inheritdoc cref="TextBox.ReadOnly"/>
        bool ReadOnly { get; set; }

        /// <inheritdoc cref="TextBox.TextWrap"/>
        TextBoxTextWrap TextWrap { get; set; }

        /// <inheritdoc cref="TextBox.TextAlign"/>
        TextHorizontalAlignment TextAlign { get; set; }

        /// <inheritdoc cref="TextBox.GetLineLength"/>
        int GetLineLength(long lineNo);

        /// <inheritdoc cref="TextBox.GetLineText"/>
        string GetLineText(long lineNo);

        /// <inheritdoc cref="TextBox.GetNumberOfLines"/>
        int GetNumberOfLines();

        /// <inheritdoc cref="TextBox.PositionToXY"/>
        PointI PositionToXY(long pos);

        /// <inheritdoc cref="TextBox.PositionToCoord"/>
        PointD PositionToCoord(long pos);

        /// <inheritdoc cref="TextBox.ShowPosition"/>
        void ShowPosition(long pos);

        /// <inheritdoc cref="TextBox.XYToPosition"/>
        long XYToPosition(long x, long y);

        /// <inheritdoc cref="TextBox.Clear"/>
        void Clear();

        /// <inheritdoc cref="TextBox.Copy"/>
        void Copy();

        /// <inheritdoc cref="TextBox.Cut"/>
        void Cut();

        /// <inheritdoc cref="TextBox.AppendText"/>
        void AppendText(string text);

        /// <inheritdoc cref="TextBox.GetInsertionPoint"/>
        long GetInsertionPoint();

        /// <inheritdoc cref="TextBox.Paste"/>
        void Paste();

        /// <inheritdoc cref="TextBox.Redo"/>
        void Redo();

        /// <inheritdoc cref="TextBox.Remove"/>
        void Remove(long from, long to);

        /// <inheritdoc cref="TextBox.Replace"/>
        void Replace(long from, long to, string value);

        /// <inheritdoc cref="TextBox.SetInsertionPoint"/>
        void SetInsertionPoint(long pos);

        /// <inheritdoc cref="TextBox.SetInsertionPointEnd"/>
        void SetInsertionPointEnd();

        /// <summary>
        /// Sets maximal possible text length.
        /// </summary>
        /// <param name="len">Text length.</param>
        void SetMaxLength(ulong len);

        /// <inheritdoc cref="TextBox.SetSelection"/>
        void SetSelection(long from, long to);

        /// <inheritdoc cref="TextBox.SelectAll"/>
        void SelectAll();

        /// <inheritdoc cref="TextBox.SelectNone"/>
        void SelectNone();

        /// <inheritdoc cref="TextBox.Undo"/>
        void Undo();

        /// <inheritdoc cref="TextBox.WriteText"/>
        void WriteText(string text);

        /// <inheritdoc cref="TextBox.GetRange"/>
        string GetRange(long from, long to);

        /// <inheritdoc cref="TextBox.GetStringSelection"/>
        string GetStringSelection();

        /// <inheritdoc cref="TextBox.EmptyUndoBuffer"/>
        void EmptyUndoBuffer();

        /// <inheritdoc cref="TextBox.IsValidPosition"/>
        bool IsValidPosition(long pos);

        /// <inheritdoc cref="TextBox.GetLastPosition"/>
        long GetLastPosition();

        /// <inheritdoc cref="TextBox.GetSelectionStart"/>
        long GetSelectionStart();

        /// <inheritdoc cref="TextBox.GetSelectionEnd"/>
        long GetSelectionEnd();
    }
}
