#include "Popup.h"

namespace Alternet::UI
{
    Popup::Popup()
    {
        SetVisible(false);
        CreateWxWindow();
    }

    Popup::~Popup()
    {
    }

    wxWindow* Popup::CreateWxWindowCore(wxWindow* parent)
    {
        return new wxPopupTransientWindow(parent);
    }

    void Popup::UpdateWxWindowParent()
    {
    }

    wxPopupTransientWindow* Popup::GetWxPopup()
    {
        return dynamic_cast<wxPopupTransientWindow*>(GetWxWindow());
    }
}
