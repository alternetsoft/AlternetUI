#pragma once
#include "Control.h"
#include "ApiTypes.h"

namespace Alternet::UI
{
    class ListView : Control
    {
#include "Api/ListView.inc"
    public:

        wxWindow* CreateWxWindowCore(wxWindow* parent) override;

    protected:
        void OnWxWindowCreated() override;
        void OnWxWindowDestroying() override;

    private:
        std::vector<string> _items;
        ListViewView _view = ListViewView::List;

        void ApplyItems();
        void ReceiveItems();

        wxListCtrl* GetListCtrl();

        long GetStyle();
    };
}
