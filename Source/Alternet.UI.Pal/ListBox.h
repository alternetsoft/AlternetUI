#pragma once
#include "Common.h"
#include "Control.h"

namespace Alternet::UI
{
    class ListBox : public Control
    {
#include "Api/ListBox.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;

    protected:
        void OnWxWindowCreated() override;
        void OnWxWindowDestroying() override;

    private:

        std::vector<string> _items;
        std::vector<int> _selectedIndices;

        ListBoxSelectionMode _selectionMode = ListBoxSelectionMode::Single;

        long GetSelectionStyle();

        void ApplyItems();
        void ReceiveItems();

        void ApplySelectedIndices();
        void ReceiveSelectedIndices();

        wxListBox* GetListBox();

        std::vector<int> GetSelectedIndices();
        void SetSelectedIndices(const std::vector<int>& value);
    };
}
