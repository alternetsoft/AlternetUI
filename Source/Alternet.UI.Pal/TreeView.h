#pragma once
#include "Common.h"
#include "Control.h"
#include "ImageList.h"

namespace Alternet::UI
{
    class TreeView : public Control
    {
#include "Api/TreeView.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;

    private:
    
    };
}
