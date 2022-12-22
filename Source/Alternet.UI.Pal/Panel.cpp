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
        long style = wxNO_BORDER;

        if (GetIsScrollable())
            style |= wxHSCROLL | wxVSCROLL;

        auto p = new wxPanel(parent, -1, wxDefaultPosition, wxDefaultSize, style);
        return p;
    }
}