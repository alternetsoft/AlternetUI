#pragma once
#include "Common.h"
#include "Control.h"

namespace Alternet::UI
{
    class TabControl : public Control
    {
#include "Api/TabControl.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;

    protected:
        virtual void OnWxWindowCreated() override;

    private:

        void OnSelectedPageChanged(wxBookCtrlEvent& event);
        void OnSelectedPageChanging(wxBookCtrlEvent& event);

        wxNotebook* GetNotebook();

        TabAlignment _tabAlignment = TabAlignment::Top;

        long GetStyle();

        struct Page
        {
            Page(Control* control_, int index_, const wxString& title_);
            ~Page();

            Control* control;
            int index;
            wxString title;

            BYREF_ONLY(Page);
        };

        std::vector<Page*> _pages;

        void InsertPage(Page* page);
    };
}
