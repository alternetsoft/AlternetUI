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
        std::vector<string> _columns;
        ListViewView _view = ListViewView::List;

        void ApplyItems();
        void ReceiveItems();

        void ApplyColumns();

        wxListCtrl* GetListCtrl();

        void InsertItem(wxListCtrl* listCtrl, const string& item, int index);
        void InsertColumn(wxListCtrl* listCtrl, const string& title, int index);

        long GetStyle();
    };
}
