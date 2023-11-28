#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "Control.h"
#include "Api/NativeEventDataTypes.h"

#include <wx/splitter.h>

namespace Alternet::UI
{
    class SplitterPanel : public Control
    {
#include "Api/SplitterPanel.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;
        wxWindow* CreateWxWindowUnparented() override;

        SplitterPanel(long styles);

    private:
        int64_t _styles = wxSP_3D;
        bool _redrawOnSashPosition = true;
        bool _canDoubleClick = true;
        bool _canMoveSplitter = true;
        SplitterPanelEventData _eventData = SplitterPanelEventData();

        void ToEventData(wxSplitterEvent& event);
        void FromEventData(wxSplitterEvent& event);

        void RaiseEventEx(SplitterPanelEvent eventId, wxSplitterEvent& event,
            bool canVeto);

        wxSplitterWindow* GetSplitterWindow();
        void OnSplitterSashPosChanged(wxSplitterEvent& event);
        void OnSplitterSashPosChanging(wxSplitterEvent& event);
        void OnSplitterSashPosResize(wxSplitterEvent& event);
        void OnSplitterDoubleClicked(wxSplitterEvent& event);
        void OnSplitterUnsplit(wxSplitterEvent& event);
    };
}
