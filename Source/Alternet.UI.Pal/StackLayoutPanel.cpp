#include "StackLayoutPanel.h"

namespace Alternet::UI
{
    StackLayoutPanel::StackLayoutPanel()
    {
    }

    StackLayoutPanel::~StackLayoutPanel()
    {
    }

    StackLayoutOrientation StackLayoutPanel::GetOrientation()
    {
        return StackLayoutOrientation();
    }

    void StackLayoutPanel::SetOrientation(StackLayoutOrientation value)
    {
    }

    wxWindowBase* StackLayoutPanel::GetControl()
    {
        return _button;
    }

    wxWindow* StackLayoutPanel::CreateWxWindow(wxWindow* parent)
    {
        _button = new wxButton(parent, wxID_ANY, wxStr(u"STACK"), wxDefaultPosition, wxSize(100, 20));
        parent->GetSizer()->Add(_button, wxALIGN_TOP);
        return _button;
    }
}
