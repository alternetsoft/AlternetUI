#include "Popup.h"

namespace Alternet::UI
{
    Popup::Popup()
    {
    }

    Popup::~Popup()
    {
    }

    wxWindow* Popup::CreateWxWindowCore(wxWindow* parent)
    {
        return nullptr;
    }

    void Popup::UpdateWxWindowParent()
    {
    }

    wxPopupTransientWindow* Popup::GetWxPopup()
    {
        return nullptr;
    }
}
