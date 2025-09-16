#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Image.h"
#include "Menu.h"

namespace Alternet::UI
{
    class NotifyIcon : public Object
    {
#include "Api/NotifyIcon.inc"
    public:
    private:

        class TaskBarIcon : public wxTaskBarIcon
        {
        public:
            TaskBarIcon(NotifyIcon* owner);

            wxMenu* _menu = nullptr;
        protected:
            virtual wxMenu* GetPopupMenu() override;
            virtual wxMenu* CreatePopupMenu() override;

        private:
            NotifyIcon* _owner;
        };

        TaskBarIcon* _taskBarIcon = nullptr;
    
        void CreateTaskBarIcon();
        void DeleteTaskBarIcon();
        void RecreateTaskBarIconIfNeeded();

        void OnLeftMouseButtonDown(wxTaskBarIconEvent& event);
        void OnRightMouseButtonDown(wxTaskBarIconEvent& event);
        void OnRightMouseButtonUp(wxTaskBarIconEvent& event);
        void OnRightMouseButtonDoubleClick(wxTaskBarIconEvent& event);
        void OnClick(wxTaskBarIconEvent& event);
        void OnLeftMouseButtonUp(wxTaskBarIconEvent& event);
        void OnLeftMouseButtonDoubleClick(wxTaskBarIconEvent& event);

        void ApplyTextAndIcon();

        optional<string> _text;
        Image* _icon = nullptr;
        bool _visible = false;
    };
}
