#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "Control.h"

#include "wx/vlbox.h"

namespace Alternet::UI
{
    class wxVListBox2 : public wxVListBox, public wxWidgetExtender
    {
    public:
        wxVListBox2() {}
        wxVListBox2(wxWindow* parent,
            wxWindowID id = wxID_ANY,
            const wxPoint& pos = wxDefaultPosition,
            const wxSize& size = wxDefaultSize,
            long style = 0,
            const wxString& name = wxASCII_STR(wxVListBoxNameStr))
            : wxVListBox(parent, id, pos, size, style, name)
        {
        }

        // the derived class must implement this function to actually draw the item
        // with the given index on the provided DC
        virtual void OnDrawItem(wxDC& dc, const wxRect& rect, size_t n) const override;

        // the derived class must implement this method to return the height of the
        // specified item
        virtual wxCoord OnMeasureItem(size_t n) const override;

        virtual void OnDrawBackground(wxDC& dc, const wxRect& rect, size_t n) const override;
    };

    class VListBox : public Control
    {
#include "Api/VListBox.inc"
    public:
        VListBox(int64_t styles);

        wxWindow* CreateWxWindowCore(wxWindow* parent) override;
        wxWindow* CreateWxWindowUnparented() override;

        void OnSelectionChanged(wxCommandEvent& event);

        Size GetPreferredSize(const Size& availableSize) override;

        void OnDrawItem(wxDC& dc, const wxRect& rect, size_t n);
        wxCoord OnMeasureItem(size_t n);

    protected:
        void OnWxWindowCreated() override;
        long GetSelectionStyle();

        bool hasBorder = true;
        ListBoxSelectionMode _selectionMode = ListBoxSelectionMode::Single;
    private:
        wxVListBox2* GetListBox();
        unsigned long selectedCookie = 0;
        wxDC* eventDc = nullptr;
        RectI eventRect;
        int eventItemIndex = -1;
        int eventItemHeight = 16;

        void UpdateDc(wxDC& dc);
    };
}
