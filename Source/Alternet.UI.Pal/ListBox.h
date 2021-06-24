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

        void ApplyItems();

        wxListBox* GetListBox();

        std::vector<string> _items;

    };
}
