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

    private:

        class Row
        {
        public:
            Row(int index) : _index(index) {}

            void GetCell(int index, wxListItem& cell)
            {
                EnsureCellsReserved(index + 1);
                cell = _cells[index];
            }

            void SetCell(int index, const wxListItem& cell)
            {
                EnsureCellsReserved(index + 1);
                _cells[index] = cell;
            }

            int GetCellCount()
            {
                return _cells.size();
            }

        private:

            void EnsureCellsReserved(int newCount)
            {
                auto oldCount = _cells.size();
                if (newCount <= oldCount)
                    return;

                _cells.resize(newCount);
                for (int i = oldCount; i < newCount; i++)
                {
                    _cells[i].SetId(_index);
                    _cells[i].SetColumn(i);
                }
            }

            std::vector<wxListItem> _cells;
            int _index;
        };

        std::vector<Row> _rows;
        std::vector<string> _columns;
        ListViewView _view = ListViewView::List;

        void ApplyItems();
        void ApplyColumns();

        wxListCtrl* GetListCtrl();

        void InsertColumn(wxListCtrl* listCtrl, const string& title, int index);
        void InsertItem(wxListCtrl* listCtrl, const wxListItem& item);

        Row& GetRow(int index);

        long GetStyle();
    };
}
