#pragma once
#include "Common.h"
#include "Control.h"
#include "ListBox.h"

namespace Alternet::UI
{
    class CheckListBox : public ListBox
    {
#include "Api/CheckListBox.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;
        wxWindow* CreateWxWindowUnparented() override;

        void OnCheckedChanged(wxCommandEvent& event);
        void OnSelectionChanged(wxCommandEvent& event);

    protected:
        void OnWxWindowCreated() override;
        void OnBeforeDestroyWxWindow() override;

    private:

        std::vector<int> _checkedIndices;

        void UncheckAll();
        void ApplyCheckedIndices();
        void ReceiveCheckedIndices();

        wxCheckListBox* GetCheckListBox();

        std::vector<int> GetCheckedIndices();
        void SetCheckedIndices(const std::vector<int>& value);
    };
}
