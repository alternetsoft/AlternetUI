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

    class wxCheckListBox2 : public wxCheckListBox, public wxWidgetExtender
    {
    public:
        wxCheckListBox2() {}

        wxCheckListBox2(
            wxWindow* parent,
            wxWindowID id,
            const wxPoint& pos = wxDefaultPosition,
            const wxSize& size = wxDefaultSize,
            int n = 0,
            const wxString choices[] = nullptr,
            long style = 0)
            : wxCheckListBox(parent, id, pos, size, n, choices, style)
        {
        }

    };

    class ListBox : public Control
    {
#include "Api/ListBox.inc"
    public:
        ListBox(ListBoxHandlerCreateFlags createFlags);

        wxWindow* CreateWxWindowCore(wxWindow* parent) override;
        wxWindow* CreateWxWindowUnparented() override;

    protected:
        void OnSelectedChanged(wxCommandEvent& event);
        void OnCheckedChanged(wxCommandEvent& event);

    private:
		ListBoxHandlerCreateFlags _createFlags = ListBoxHandlerCreateFlags::None;
        bool hasBorder = true;
		ListBoxHandlerFlags _flags = ListBoxHandlerFlags::SingleSelection | ListBoxHandlerFlags::IntegralHeight;
        wxArrayInt _selections = wxArrayInt();

        wxListBoxBase* GetListBoxBase();
        wxListBox* GetListBox();
        wxItemContainer* GetItemContainer();

        bool HasCheckBoxes()
        {
            return (_createFlags & ListBoxHandlerCreateFlags::CheckBoxes) == ListBoxHandlerCreateFlags::CheckBoxes;
		}
    private:
    
    };
}
