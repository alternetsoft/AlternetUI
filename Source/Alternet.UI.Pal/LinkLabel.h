#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "wx/hyperlink.h"
#include "Control.h"

namespace Alternet::UI
{
    class LinkLabel : public Control
    {
#include "Api/LinkLabel.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;

    private:

        DelayedValue<LinkLabel, string> _text;
        DelayedValue<LinkLabel, string> _url;

        wxHyperlinkCtrl* GetStaticText();

        string RetrieveText();
        string RetrieveUrl();

        void ApplyText(const string& value);
        void ApplyUrl(const string& value);
    };
}
