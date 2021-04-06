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
        return new wxPanel(parent);
    }
}
