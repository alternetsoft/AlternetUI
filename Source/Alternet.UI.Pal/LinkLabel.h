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
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;

    private:
        wxHyperlinkCtrlBase* GetStaticText();
        void OnHyperlinkClick(wxHyperlinkEvent& event);
    };
}
