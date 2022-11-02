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
        wxTaskBarIcon* _taskBarIcon = nullptr;
    
        void CreateTaskBarIcon();
        void DeleteTaskBarIcon();
        void RecreateTaskBarIcon();

        optional<string> _text;
        Image* _icon = nullptr;
        bool _visible = false;

        inline static std::vector<wxTaskBarIcon*> _taskBarIcons;
    };
}
