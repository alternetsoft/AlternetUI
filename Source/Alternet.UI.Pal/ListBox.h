#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "Control.h"

namespace Alternet::UI
{
    class wxListBox2 : public wxListBox, public wxWidgetExtender
    {
    public:
        wxListBox2() {}

        wxListBox2(
            wxWindow* parent,
            wxWindowID id,
            const wxPoint& pos = wxDefaultPosition,
            const wxSize& size = wxDefaultSize,
            int n = 0,
            const wxString choices[] = nullptr,
            long style = 0)
			: wxListBox(parent, id, pos, size, n, choices, style)
        {
        }

    };

    class ListBox : public Control
    {
#include "Api/ListBox.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;
        wxWindow* CreateWxWindowUnparented() override;

    protected:
        void OnSelectedChanged(wxCommandEvent& event);

    private:
        bool hasBorder = true;
        wxArrayInt _selections = wxArrayInt();

        wxListBoxBase* GetListBoxBase();
        wxListBox* GetListBox();
        wxItemContainer* GetItemContainer();
    private:
    
    };
}
