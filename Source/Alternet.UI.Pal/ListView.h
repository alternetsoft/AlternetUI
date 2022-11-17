#pragma once
#include "Control.h"
#include "ImageList.h"
#include "ApiTypes.h"

namespace Alternet::UI
{
    class ListView : Control
    {
#include "Api/ListView.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;

        void OnItemSelected(wxCommandEvent& event);
        void OnItemDeselected(wxCommandEvent& event);
        void OnColumnHeaderClicked(wxListEvent& event);
        void OnBeginLabelEdit(wxListEvent& event);
        void OnEndLabelEdit(wxListEvent& event);

    protected:
        void OnWxWindowCreated() override;

    private:
        void ApplyLargeImageList(wxListView* value);
        void ApplySmallImageList(wxListView* value);


        class Column
        {
        public:
            wxListItem column;
            int width = 80;
        };

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

        ImageList* _smallImageList = nullptr;
        ImageList* _largeImageList = nullptr;

        std::vector<int> _selectedIndices;
        ListViewSelectionMode _selectionMode = ListViewSelectionMode::Single;

        std::vector<Row> _rows;
        std::vector<Column> _columns;
        ListViewView _view = ListViewView::List;

        bool _allowLabelEdit = false;
        ListViewGridLinesDisplayMode  _gridLinesDisplayMode = ListViewGridLinesDisplayMode::None;
        ListViewSortMode _sortMode = ListViewSortMode::None;

        bool _columnHeaderVisible = true;

        class HitTestResult
        {
        public:
            ListViewHitTestLocations locations = ListViewHitTestLocations::None;
            int itemIndex = 0;
            int columnIndex = 0;
        };

        static ListViewHitTestLocations GetHitTestLocationsFromWxFlags(int flags);

        void OnLabelEditEvent(wxListEvent& event, ListViewEvent e);

        void RecreateListView();

        void ApplySelectedIndices();
        void ReceiveSelectedIndices();

        std::vector<int> GetSelectedIndices();
        void SetSelectedIndices(const std::vector<int>& value);

        void DeselectAll(wxListView* listView);

        void ApplyItems();
        void ApplyColumns();

        wxListView* GetListView();

        void InsertColumn(wxListView* listView, const Column& column);
        void InsertItem(wxListView* listView, wxListItem& item);

        Row& GetRow(int index);

        long GetStyle();

        void RaiseSelectionChanged();

        int GetWxColumnWidth(double width, ListViewColumnWidthMode widthMode);
    };
}
