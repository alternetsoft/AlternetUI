#include "Panel.h"

namespace Alternet::UI
{
    Panel::Panel()
    {
    }

    Panel::~Panel()
    {
    }

    wxWindow* Panel::CreateWxWindowCore(wxWindow* parent)
    {
        auto pn = new wxPanel(parent, wxID_ANY);
        pn->SetBackgroundColour(wxColor("red"));
        return pn;
    }
}
