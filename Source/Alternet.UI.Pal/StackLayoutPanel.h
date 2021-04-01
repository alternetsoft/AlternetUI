#pragma once
#include "Common.h"
#include "Control.h"
#include "ApiTypes.h"

namespace Alternet::UI
{
    class StackLayoutPanel : Control
    {
#include "Api/StackLayoutPanel.inc"
    public:
    
        wxWindowBase* GetControl() override;
        wxWindow* CreateWxWindow(wxWindow* parent) override;

    private:

        wxButton* _button = nullptr;

    };
}
