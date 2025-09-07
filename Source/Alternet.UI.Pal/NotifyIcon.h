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
        static void DestroyAllNotifyIcons();

    private:

        class TaskBarIcon : public wxTaskBarIcon
        {
        public:
            TaskBarIcon(NotifyIcon* owner);
        protected:
            virtual wxMenu* GetPopupMenu() override;
            virtual wxMenu* CreatePopupMenu() override;
        private:
            NotifyIcon* _owner;
        };

        wxTaskBarIcon* _taskBarIcon = nullptr;
    
        void CreateTaskBarIcon();
        void DeleteTaskBarIcon();
        void RecreateTaskBarIconIfNeeded();

        void OnLeftMouseButtonUp(wxTaskBarIconEvent& event);
        void OnLeftMouseButtonDoubleClick(wxTaskBarIconEvent& event);

        void ApplyTextAndIcon();

        optional<string> _text;
        Image* _icon = nullptr;
        wxMenu* _menu = nullptr;
        bool _visible = false;

        inline static std::vector<wxTaskBarIcon*> _taskBarIcons;
    };
}
