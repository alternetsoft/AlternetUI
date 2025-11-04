#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "Font.h"
#include "Image.h"
#include "Control.h"

#include <wx/richtext/richtextctrl.h>
#include <wx/richtext/richtexthtml.h>

#include "Api/InputStream.h"
#include "Api/OutputStream.h"
#include "ManagedInputStream.h"
#include "ManagedOutputStream.h"

namespace Alternet::UI
{
    class RichTextBox : public Control
    {
#include "Api/RichTextBox.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;
        wxWindow* CreateWxWindowUnparented() override;
        wxRichTextCtrl* GetTextCtrl();
        void OnTextEnter(wxCommandEvent& event);
        void OnTextUrl(wxTextUrlEvent& event);
    protected:
        bool IsCursorSuppressed() override { return true; }

    private:
        string _eventUrl = wxStr(wxEmptyString);
        bool hasBorder = true;
    };
}
