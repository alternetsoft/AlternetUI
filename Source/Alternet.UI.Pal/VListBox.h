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

        void ProcessScrollEvent(wxScrollWinEvent& event);

        virtual bool ProcessEvent(wxEvent& event) override
        {
            wxEventType evType = event.GetEventType();

            if (evType == wxEVT_SCROLLWIN_TOP ||
                evType == wxEVT_SCROLLWIN_BOTTOM ||
                evType == wxEVT_SCROLLWIN_LINEUP ||
                evType == wxEVT_SCROLLWIN_LINEDOWN ||
                evType == wxEVT_SCROLLWIN_PAGEUP ||
                evType == wxEVT_SCROLLWIN_PAGEDOWN ||
                evType == wxEVT_SCROLLWIN_THUMBTRACK ||
                evType == wxEVT_SCROLLWIN_THUMBRELEASE)
            {
                ProcessScrollEvent((wxScrollWinEvent&)event);
            }

            return wxVListBox::ProcessEvent(event);
        }

        // the derived class must implement this function to actually draw the item
        // with the given index on the provided DC
        virtual void OnDrawItem(wxDC& dc, const wxRect& rect, size_t n) const override;

        void OnDrawItem(DrawingContext* dc, const wxRect& rect, size_t n) const;

        // the derived class must implement this method to return the height of the
        // specified item
        virtual wxCoord OnMeasureItem(size_t n) const override;

        virtual void OnDrawBackground(wxDC& dc, const wxRect& rect, size_t n) const override;

        bool SetCurrent(int current)
        {
            return DoSetCurrent(current);
        }
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

        void OnDrawItem(DrawingContext* dc, const wxRect& rect, size_t n);
        wxCoord OnMeasureItem(size_t n);
        void ProcessScrollEvent(wxScrollWinEvent& event);

    protected:
        virtual void OnPaint(wxPaintEvent& event) override;
        void OnWxWindowCreated() override;
        long GetSelectionStyle();

        bool hasBorder = true;
        ListBoxSelectionMode _selectionMode = ListBoxSelectionMode::Single;
    private:
        wxVListBox2* GetListBox();
        unsigned long selectedCookie = 0;
        DrawingContext* eventDc = nullptr;
        RectI eventRect;
        int eventItemIndex = -1;
        int eventItemHeight = 16;
        int _itemsCount = 0;
        bool _hScrollBarVisible = false;
        bool _vScrollBarVisible = true;
    };
}
