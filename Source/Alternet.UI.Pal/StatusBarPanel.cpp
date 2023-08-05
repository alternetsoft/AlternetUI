#include "StatusBarPanel.h"
#include "StatusBar.h"

namespace Alternet::UI
{
    StatusBarPanel::StatusBarPanel()
    {
    }
    
    StatusBarPanel::~StatusBarPanel()
    {
    }
    
    string StatusBarPanel::GetText()
    {
        return _text;
    }
    
    void StatusBarPanel::SetText(const string& value)
    {
        if (_text == value)
            return;

        _text = value;

        if (_parentStatusBar != nullptr && _index.has_value())
            _parentStatusBar->OnItemChanged(_index.value());
    }

    wxWindow* StatusBarPanel::CreateWxWindowCore(wxWindow* parent)
    {
        return new wxDummyPanel("statusbarpanel");
    }

    void StatusBarPanel::SetParentStatusBar(StatusBar* value, optional<int> index)
    {
        _parentStatusBar = value;
        _index = index;
    }

    StatusBar* StatusBarPanel::GetParentStatusBar()
    {
        return _parentStatusBar;
    }

    optional<int> StatusBarPanel::GetIndex()
    {
        return _index;
    }

    void StatusBarPanel::ShowCore()
    {
    }

    Rect StatusBarPanel::RetrieveBounds()
    {
        return Rect();
    }

    void StatusBarPanel::ApplyBounds(const Rect& value)
    {
    }

    Size StatusBarPanel::SizeToClientSize(const Size& size)
    {
        return size;
    }

    void StatusBarPanel::UpdateWxWindowParent()
    {
    }
}
