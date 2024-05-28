#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "Control.h"

#include <wx/hyperlink.h>

namespace Alternet::UI
{
    class LinkLabel : public Control
    {
#include "Api/LinkLabel.inc"
    public:
        string GetText() override;
        void SetText(const string& value) override;

        wxWindow* CreateWxWindowCore(wxWindow* parent) override;
        wxWindow* CreateWxWindowUnparented() override;

    private:
        wxHyperlinkCtrlBase* GetStaticText();
        void OnHyperlinkClick(wxHyperlinkEvent& event);
    };
}
