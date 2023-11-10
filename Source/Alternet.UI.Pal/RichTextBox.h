#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "Font.h"
#include "Image.h"
#include "Control.h"
#include "wx/richtext/richtextctrl.h"

namespace Alternet::UI
{
    class RichTextBox : public Control
    {
#include "Api/RichTextBox.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;
        wxRichTextCtrl* GetTextCtrl();
        void OnTextChanged(wxCommandEvent& event);
        void OnTextEnter(wxCommandEvent& event);
        void OnTextUrl(wxTextUrlEvent& event);
    private:
        string _eventUrl = wxStr(wxEmptyString);
        bool hasBorder = true;
    };
}
