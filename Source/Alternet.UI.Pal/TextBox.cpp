#include "TextBox.h"

namespace Alternet::UI
{
    TextBox::TextBox():
        _text(*this, u"", &Control::IsWxWindowCreated, &TextBox::RetrieveText, &TextBox::ApplyText)
    {
        GetDelayedValues().Add(&_text);
    }

    TextBox::~TextBox()
    {
        if (IsWxWindowCreated())
        {
            auto window = GetWxWindow();
            if (window != nullptr)
            {
                window->Unbind(wxEVT_TEXT, &TextBox::OnTextChanged, this);
            }
        }
    }

    string TextBox::GetText()
    {
        return _text.Get();
    }

    void TextBox::SetText(const string& value)
    {
        _text.Set(value);
    }

    wxWindow* TextBox::CreateWxWindowCore(wxWindow* parent)
    {
        long style = GetCreateStyle() | GetBorderStyle();

        auto textCtrl = new TextCtrlEx(
            parent, wxID_ANY, wxEmptyString, wxDefaultPosition, 
            wxDefaultSize, style);

#ifdef __WXOSX__
        // todo: port all platforms to the latest wx version, and then the ifdef can be removed.
        // EnableVisibleFocus is implemented only on macOS at the moment through.
        if (_editControlOnly)
            textCtrl->EnableVisibleFocus(false);
#endif
            
        textCtrl->Bind(wxEVT_TEXT, &TextBox::OnTextChanged, this);
        return textCtrl;
    }

    long TextBox::GetCreateStyle()
    {
        long style = _editControlOnly ? wxNO_BORDER : 0;

        if (_readOnly)
            style |= wxTE_READONLY;

        if (_multiline)
            style |= wxTE_MULTILINE;

        if(_isRichEdit)
            style |= wxTE_RICH2;

        return style;
    }

    void TextBox::OnTextChanged(wxCommandEvent& event)
    {
        RaiseEvent(TextBoxEvent::TextChanged);
    }

    bool TextBox::GetEditControlOnly()
    {
        return _editControlOnly;
    }

    void TextBox::SetEditControlOnly(bool value)
    {
        _editControlOnly = value;
        RecreateWindow();
    }

    bool TextBox::GetReadOnly()
    {
        return _readOnly;
    }

    void TextBox::SetReadOnly(bool value)
    {
        if (_readOnly == value)
            return;

        _readOnly = value;
        GetTextCtrl()->SetEditable(!_readOnly);
    }

    bool TextBox::GetMultiline()
    {
        return _multiline;
    }

    void TextBox::SetMultiline(bool value)
    {
        if (_multiline == value)
            return;

        _multiline = value;
        RecreateWxWindowIfNeeded();
    }

    wxTextCtrl* TextBox::GetTextCtrl()
    {
        return dynamic_cast<wxTextCtrl*>(GetWxWindow());
    }

    string TextBox::RetrieveText()
    {
        return wxStr(GetTextCtrl()->GetValue());
    }

    void TextBox::ApplyText(const string& value)
    {
        GetTextCtrl()->SetValue(wxStr(value));
    }

    bool TextBox::GetIsRichEdit() 
    {
        return _isRichEdit;
    }
    
    void TextBox::SetIsRichEdit(bool value) 
    {
        if (_isRichEdit == value)
            return;
        _isRichEdit = value;
        RecreateWxWindowIfNeeded();
    }

    bool TextBox::GetIsModified() 
    {
        return GetTextCtrl()->IsModified();
    }

    void TextBox::SetIsModified(bool value) 
    {
        GetTextCtrl()->SetModified(value);
    }

    bool TextBox::GetCanCopy() 
    {
        return GetTextCtrl()->CanCopy();
    }

    bool TextBox::GetCanCut() 
    {
        return GetTextCtrl()->CanCut();
    }

    bool TextBox::GetCanPaste() 
    {
        return GetTextCtrl()->CanPaste();
    }

    bool TextBox::GetCanRedo() 
    {
        return GetTextCtrl()->CanRedo();
    }

    bool TextBox::GetCanUndo() 
    {
        return GetTextCtrl()->CanUndo();
    }

    bool TextBox::GetIsEmpty() 
    {
        return GetTextCtrl()->IsEmpty();
    }

    int TextBox::GetLineLength(int64_t lineNo) 
    {
        return GetTextCtrl()->GetLineLength(lineNo);
    }

    string TextBox::GetLineText(int64_t lineNo) 
    {
        return wxStr(GetTextCtrl()->GetLineText(lineNo));
    }

    int TextBox::GetNumberOfLines() 
    {
        return GetTextCtrl()->GetNumberOfLines();
    }

    Point TextBox::PositionToXY(int64_t pos) 
    {
        long x; 
        long y;
        auto result = GetTextCtrl()->PositionToXY(pos, &x, &y);
        if(result)
            return Point(x,y);
        return Point(-1, -1);
    }

    Point TextBox::PositionToCoords(int64_t pos)     
    {
        auto p = GetTextCtrl()->PositionToCoords(pos);
        return Point(p.x,p.y);
    }

    void TextBox::ShowPosition(int64_t pos) 
    {
        GetTextCtrl()->ShowPosition(pos);
    }

    int64_t TextBox::XYToPosition(int64_t x, int64_t y) 
    {
        return GetTextCtrl()->XYToPosition(x, y);
    }

    void TextBox::Clear() 
    {
        GetTextCtrl()->Clear();
    }

    void TextBox::Copy() 
    {
        GetTextCtrl()->Copy();
    }

    void TextBox::Cut() 
    {
        GetTextCtrl()->Cut();
    }

    void TextBox::AppendText(const string& text) 
    {
        GetTextCtrl()->AppendText(wxStr(text));
    }

    int64_t TextBox::GetInsertionPoint() 
    {
        return GetTextCtrl()->GetInsertionPoint();
    }

    void TextBox::Paste() 
    {
        GetTextCtrl()->Paste();
    }

    void TextBox::Redo() 
    {
        GetTextCtrl()->Redo();
    }

    void TextBox::Remove(int64_t from, int64_t to) 
    {
        GetTextCtrl()->Remove(from, to);
    }

    void TextBox::Replace(int64_t from, int64_t to, const string& value) 
    {
        GetTextCtrl()->Replace(from, to, wxStr(value));
    }

    void TextBox::SetInsertionPoint(int64_t pos) 
    {
        GetTextCtrl()->SetInsertionPoint(pos);
    }

    void TextBox::SetInsertionPointEnd() 
    {
        GetTextCtrl()->SetInsertionPointEnd();
    }

    void TextBox::SetMaxLength(uint64_t len) 
    {
        GetTextCtrl()->SetMaxLength(len);
    }

    void TextBox::SetSelection(int64_t from, int64_t to) 
    {
        GetTextCtrl()->SetSelection(from, to);
    }

    void TextBox::SelectAll() 
    {
        GetTextCtrl()->SelectAll();
    }

    void TextBox::SelectNone() 
    {
        GetTextCtrl()->SelectNone();
    }

    void TextBox::TextBox::Undo() 
    {
        GetTextCtrl()->Undo();
    }

    void TextBox::WriteText(const string& text) 
    {
        GetTextCtrl()->WriteText(wxStr(text));
    }

    string TextBox::GetRange(int64_t from, int64_t to) 
    {
        return wxStr(GetTextCtrl()->GetRange(from, to));
    }

    string TextBox::GetStringSelection() 
    {
        return wxStr(GetTextCtrl()->GetStringSelection());
    }

    void TextBox::EmptyUndoBuffer() 
    {
        GetTextCtrl()->EmptyUndoBuffer();
    }

    bool TextBox::IsValidPosition(int64_t pos) 
    {
        return pos >= 0 && pos <= GetTextCtrl()->GetLastPosition();
    }

    int64_t TextBox::GetLastPosition() 
    {
        return GetTextCtrl()->GetLastPosition();
    }
    
    int64_t TextBox::GetSelectionStart() 
    {
        long selectionFrom;
        long selectionTo;
        GetTextCtrl()->GetSelection(&selectionFrom, &selectionTo);
        return selectionFrom;
    }

    int64_t TextBox::GetSelectionEnd() 
    {
        long selectionFrom;
        long selectionTo;
        GetTextCtrl()->GetSelection(&selectionFrom, &selectionTo);
        return selectionTo;
    }

    string TextBox::GetEmptyTextHint()
    {
        return wxStr(GetTextCtrl()->GetHint());
    }

    void TextBox::SetEmptyTextHint(const string& value)
    {
        GetTextCtrl()->SetHint(wxStr(value));
    }

    void* TextBox::GetStyle(int64_t position) 
    {
        auto result = GetTextCtrl()->GetStyle(position, _textAttr);
        if (!result)
            return nullptr;
        return &_textAttr;
    }

    bool TextBox::SetDefaultStyle(void* style)
    {
        return GetTextCtrl()->SetDefaultStyle((wxTextAttr&)style);
    }

    bool TextBox::SetStyle(int64_t start, int64_t end, void* style)
    {
        return GetTextCtrl()->SetStyle(start, end, (wxTextAttr&)style);
    }

    void* TextBox::GetDefaultStyle()
    {
        _textAttr = GetTextCtrl()->GetDefaultStyle();
        return &_textAttr;
    }

    bool TextBox::GetHasSelection()
    {
        long from, to;
        GetTextCtrl()->GetSelection(&from, &to);
        return from != to;
    }

}
