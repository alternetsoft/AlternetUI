#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Control.h"
#include "Menu.h"

namespace Alternet::UI
{
    class MainMenu : public Control
    {
#include "Api/MainMenu.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;

        wxMenuBar* GetWxMenuBar();
    protected:

    private:
    };
}