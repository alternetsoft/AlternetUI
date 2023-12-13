#pragma once
#include "Common.h"
#include "Control.h"
#include "ApiTypes.h"
#include "Object.h"

#include <wx/scrolbar.h>

namespace Alternet::UI
{
    class ScrollBar : public Control
    {
#include "Api/ScrollBar.inc"
    public:                          
        enum ScrollEventType
        {
            SmallDecrement,
            SmallIncrement,
            LargeDecrement,
            LargeIncrement,
            ThumbPosition,
            ThumbTrack,
            First,
            Last,
            EndScroll,
        };

        void OnEventScrollTop(wxScrollEvent& event);
        void OnEventScrollBottom(wxScrollEvent& event);
        void OnEventScrollLineUp(wxScrollEvent& event);
        void OnEventScrollLineDown(wxScrollEvent& event);
        void OnEventScrollPageUp(wxScrollEvent& event);
        void OnEventScrollPageDown(wxScrollEvent& event);
        void OnEventScrollThumbTrack(wxScrollEvent& event);
        void OnEventScrollThumbRelease(wxScrollEvent& event);
        void OnEventScrollChanged(wxScrollEvent& event);
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;
        wxWindow* CreateWxWindowUnparented() override;

    private:

        void OnScrollInternal(ScrollEventType evType, wxScrollEvent& event);
        wxScrollBarBase* GetScrollBar();

        bool _isVertical = false;
        int _eventType = 0;
        int _eventPos = 0;
    };
}
