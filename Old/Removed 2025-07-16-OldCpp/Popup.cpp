#include "Popup.h"

namespace Alternet::UI
{
    Popup::Popup()
    {
        /*SetVisible(false);
        CreateWxWindow();*/
        /*_popupWindows.push_back(GetWxWindow());*/
    }

    Popup::~Popup()
    {
        /*_popupWindows.erase(std::find(_popupWindows.begin(), _popupWindows.end(), GetWxWindow()));*/
    }

    class wxPopupTransientWindow2 : public wxPopupTransientWindow, 
        public wxWidgetExtender
    {
    public:
        wxPopupTransientWindow2(){}
        wxPopupTransientWindow2(wxWindow* parent, int style = wxBORDER_NONE)
        {
            Create(parent, style);
        }
    };

    class wxPopupWindow2 : public wxPopupWindow,
        public wxWidgetExtender
    {
    public:
        wxPopupWindow2(){}
        wxPopupWindow2(wxWindow* parent, int style = wxBORDER_NONE)
        {
            Create(parent, style);
        }
    };

    bool Popup::GetPuContainsControls()
    {
        return _puContainsControls;
    }

    void Popup::SetPuContainsControls(bool value)
    {
        if (_puContainsControls == value)
            return;
        _puContainsControls = value;
        RecreateWxWindowIfNeeded();
    }

    wxWindow* Popup::CreateWxWindowUnparented()
    {
        if (_isTransient)
            return new wxPopupTransientWindow2();
        else
            return new wxPopupWindow2();
    }

    wxWindow* Popup::CreateWxWindowCore(wxWindow* parent)
    {
        int style = _borderStyle;

        if (_puContainsControls)
            style |= wxPU_CONTAINS_CONTROLS;

        if(_isTransient)
            return new wxPopupTransientWindow2(parent, style);
        else
            return new wxPopupWindow2(parent, style);
    }

    /*void Popup::UpdateWxWindowParent()
    {
    }*/

    bool Popup::GetIsTransient()
    {
        return _isTransient;
    }
    
    void Popup::SetIsTransient(bool value)
    {
        if (_isTransient == value)
            return;
        _isTransient = value;
        RecreateWxWindowIfNeeded();
    }

    void Popup::DoPopup(void* focus)
    {
        auto p = GetWxTransientPopup();
        if (p == nullptr)
            return;
        p->Popup((wxWindow*)focus);
    }

    void Popup::Dismiss()
    {
        auto p = GetWxTransientPopup();
        if (p == nullptr)
            return;
        p->Dismiss();
    }

    wxPopupTransientWindow* Popup::GetWxTransientPopup()
    {
        return dynamic_cast<wxPopupTransientWindow*>(GetWxWindow());
    }

    wxPopupWindow* Popup::GetWxPopup()
    {
        return dynamic_cast<wxPopupWindow*>(GetWxWindow());
    }

    void Popup::Position(const Int32Point& ptOrigin, const Int32Size& sizePopup)
    {
        GetWxPopup()->Position(ptOrigin, sizePopup);
    }

    /*std::vector<wxWindow*> Popup::GetVisiblePopupWindows()
    {
        std::vector<wxWindow*> result;
        for (auto window : _popupWindows)
        {
            if (window->IsShown())
                result.push_back(window);
        }

        return result;
    }*/

}
