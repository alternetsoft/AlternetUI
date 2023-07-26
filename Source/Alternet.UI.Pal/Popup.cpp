#include "Popup.h"

namespace Alternet::UI
{
    Popup::Popup()
    {
        SetVisible(false);
        CreateWxWindow();
        _popupWindows.push_back(GetWxWindow());
    }

    Popup::~Popup()
    {
        _popupWindows.erase(std::find(_popupWindows.begin(), _popupWindows.end(), GetWxWindow()));
    }

    class wxPopupTransientWindow2 : public wxPopupTransientWindow, 
        public wxWidgetExtender
    {
    public:
        wxPopupTransientWindow2(wxWindow* parent, int style = wxBORDER_NONE)
        {
            Create(parent, style);
        }
    };

    wxWindow* Popup::CreateWxWindowCore(wxWindow* parent)
    {
        return new wxPopupTransientWindow2(parent);
    }

    void Popup::UpdateWxWindowParent()
    {
    }

    wxPopupTransientWindow* Popup::GetWxPopup()
    {
        return dynamic_cast<wxPopupTransientWindow*>(GetWxWindow());
    }

    /*static*/ std::vector<wxWindow*> Popup::GetVisiblePopupWindows()
    {
        std::vector<wxWindow*> result;
        for (auto window : _popupWindows)
        {
            if (window->IsShown())
                result.push_back(window);
        }

        return result;
    }

}
