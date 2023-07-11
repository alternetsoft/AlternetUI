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

        bool hasBorder = true;
        ImageList* _smallImageList = nullptr;
        ImageList* _largeImageList = nullptr;
        ListViewSelectionMode _selectionMode = ListViewSelectionMode::Single;
        ListViewView _view = ListViewView::List;
        bool _allowLabelEdit = false;
        ListViewGridLinesDisplayMode  _gridLinesDisplayMode =
            ListViewGridLinesDisplayMode::None;
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
        std::vector<int> GetSelectedIndices();
        void SetSelectedIndices(const std::vector<int>& value);
        void DeselectAll(wxListView* listView);
        wxListView* GetListView();
        void InsertItem(wxListView* listView, wxListItem& item);
        long GetStyle();
        void RaiseSelectionChanged();
        int GetWxColumnWidth(double width, ListViewColumnWidthMode widthMode);
    };
}
