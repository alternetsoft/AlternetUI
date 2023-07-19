#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "wx/splitter.h"
#include "Control.h"

namespace Alternet::UI
{
    class SplitterPanel : public Control
    {
#include "Api/SplitterPanel.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;
        
        SplitterPanel(long styles);

    private:
        int64_t _styles = wxSP_3D;
        bool _redrawOnSashPosition = true;

        wxSplitterWindow* GetSplitterWindow();

    };
}
