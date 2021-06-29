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

    private:

        std::vector<string> _items;

        DelayedValue<ListBox, ListBoxSelectionMode> _selectionMode;

        void ApplyItems();

        ListBoxSelectionMode RetrieveSelectionMode();
        void ApplySelectionMode(const ListBoxSelectionMode& value);

        wxListBox* GetListBox();
    };
}
