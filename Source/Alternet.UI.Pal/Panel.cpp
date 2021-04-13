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
        return new wxPanel(parent, -1, wxDefaultPosition, wxDefaultSize, /*wxSUNKEN_BORDER*/wxNO_BORDER);
    }
}
