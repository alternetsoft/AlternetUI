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
        auto p = new wxPanel(parent, -1, wxDefaultPosition, wxDefaultSize, wxNO_BORDER);
        return p;
    }
}