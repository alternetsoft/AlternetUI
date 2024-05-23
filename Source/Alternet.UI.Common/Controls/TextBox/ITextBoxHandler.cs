using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public interface ITextBoxHandler : IControlHandler
    {
        string ReportedUrl { get; }

        bool HideSelection { get; set; }

        bool ProcessTab { get; set; }

        bool ProcessEnter { get; set; }

        bool IsPassword { get; set; }

        bool AutoUrl { get; set; }

        bool HideVertScrollbar { get; set; }

        bool HasSelection { get; }

        bool IsModified { get; set; }

        bool CanCopy { get; }

        bool CanCut { get; }

        bool CanPaste { get; }

        bool CanRedo { get; }

        bool CanUndo { get; }

        bool IsEmpty { get; }

        string EmptyTextHint { get; set; }

        TextBoxTextWrap TextWrap { get; set; }

        GenericAlignment TextAlign { get; set; }

        bool IsRichEdit { get; set; }

        int GetLineLength(long lineNo);

        string GetLineText(long lineNo);

        int GetNumberOfLines();

        PointI PositionToXY(long pos);

        PointD PositionToCoords(long pos);

        void ShowPosition(long pos);

        long XYToPosition(long x, long y);

        void Clear();

        void Copy();

        void Cut();

        void AppendText(string text);

        long GetInsertionPoint();

        void Paste();

        void Redo();

        void Remove(long from, long to);

        void Replace(long from, long to, string value);

        void SetInsertionPoint(long pos);

        ITextBoxTextAttr CreateTextAttr();

        void SetInsertionPointEnd();

        void SetMaxLength(ulong len);

        void SetSelection(long from, long to);

        void SelectAll();

        void SelectNone();

        void Undo();

        void WriteText(string text);

        string GetRange(long from, long to);

        string GetStringSelection();

        void EmptyUndoBuffer();

        bool IsValidPosition(long pos);

        ITextBoxTextAttr GetDefaultStyle();

        ITextBoxTextAttr GetStyle(long pos);

        bool SetDefaultStyle(ITextBoxTextAttr style);

        long GetLastPosition();

        long GetSelectionStart();

        long GetSelectionEnd();

        bool SetStyle(long start, long end, ITextBoxTextAttr style);

        void SetValidator(IValueValidator? value);
    }
}
