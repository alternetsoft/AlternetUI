#pragma once
#include "Control.h"
#include "ImageList.h"
#include "ApiTypes.h"

#include <wx/dynarray.h>

namespace Alternet::UI
{
    class wxListView2 : public wxListCtrl, public wxWidgetExtender
    {
    public:
        wxListView2() {}
        wxListView2(wxWindow* parent,
            wxWindowID winid = wxID_ANY,
            const wxPoint& pos = wxDefaultPosition,
            const wxSize& size = wxDefaultSize,
            long style = wxLC_REPORT,
            const wxValidator& validator = wxDefaultValidator,
            const wxString& name = wxASCII_STR(wxListCtrlNameStr))
        {
            Create(parent, winid, pos, size, style, validator, name);
        }

        // focus/selection stuff
        // ---------------------

        // [de]select an item
        void Select(long n, bool on = true)
        {
            SetItemState(n, on ? wxLIST_STATE_SELECTED : 0, wxLIST_STATE_SELECTED);
        }

        // focus and show the given item
        void Focus(long index)
        {
            SetItemState(index, wxLIST_STATE_FOCUSED, wxLIST_STATE_FOCUSED);
            EnsureVisible(index);
        }

        // get the currently focused item or -1 if none
        long GetFocusedItem() const
        {
            return GetNextItem(-1, wxLIST_NEXT_ALL, wxLIST_STATE_FOCUSED);
        }

        // get first and subsequent selected items, return -1 when no more
        long GetNextSelected(long item) const
        {
            return GetNextItem(item, wxLIST_NEXT_ALL, wxLIST_STATE_SELECTED);
        }
        long GetFirstSelected() const
        {
            return GetNextSelected(-1);
        }

        // return true if the item is selected
        bool IsSelected(long index) const
        {
            return GetItemState(index, wxLIST_STATE_SELECTED) != 0;
        }

        // columns
        // -------

        void SetColumnImage(int col, int image)
        {
            wxListItem item;
            item.SetMask(wxLIST_MASK_IMAGE);
            item.SetImage(image);
            SetColumn(col, item);
        }

        void ClearColumnImage(int col) { SetColumnImage(col, -1); }
    };

    class ListView : Control
    {
#include "Api/ListView.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;
        wxWindow* CreateWxWindowUnparented() override;

        void OnItemSelected(wxListEvent& event);
        void OnItemDeselected(wxListEvent& event);
        void OnColumnHeaderClicked(wxListEvent& event);
        void OnBeginLabelEdit(wxListEvent& event);
        void OnEndLabelEdit(wxListEvent& event);

    protected:
        void OnWxWindowCreated() override;

    private:
        void ApplyLargeImageList(wxListView2* value);
        void ApplySmallImageList(wxListView2* value);

        bool hasBorder = false;
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
            int64_t itemIndex = 0;
            int64_t columnIndex = 0;
        };

        static ListViewHitTestLocations GetHitTestLocationsFromWxFlags(int flags);
        void OnLabelEditEvent(wxListEvent& event, ListViewEvent e);
        virtual void RecreateWxWindowIfNeeded() override;
        std::vector<int64_t> GetSelectedIndices();
        void SetSelectedIndices(const std::vector<int64_t>& value);
        void DeselectAll(wxListView2* listView);
        wxListView2* GetListView();
        void InsertItem(wxListView2* listView, wxListItem& item);
        long GetStyle();
        void RaiseSelectionChanged();
        int GetWxColumnWidth(double width, ListViewColumnWidthMode widthMode);
    };
}
