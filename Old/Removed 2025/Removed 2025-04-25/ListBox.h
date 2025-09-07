#pragma once
#include "Common.h"
#include "Control.h"

namespace Alternet::UI
{
    class ListBox : public Control
    {
#include "Api/ListBox.inc"
    public:
        ListBox(int64_t styles);

        wxWindow* CreateWxWindowCore(wxWindow* parent) override;
        wxWindow* CreateWxWindowUnparented() override;

        void OnSelectionChanged(wxCommandEvent& event);

        Size GetPreferredSize(const Size& availableSize) override;
    protected:
        void OnWxWindowCreated() override;
        void OnBeforeDestroyWxWindow() override;
        long GetSelectionStyle();

        bool hasBorder = true;
    private:
        std::vector<string> _items;
        std::vector<int> _selectedIndices;

        ListBoxSelectionMode _selectionMode = ListBoxSelectionMode::Single;

        void ApplyItems();
        void ReceiveItems();

        void ApplySelectedIndices();
        void ReceiveSelectedIndices();

        wxListBox* GetListBox();

        std::vector<int> GetSelectedIndices();
        void SetSelectedIndices(const std::vector<int>& value);
    };
}
