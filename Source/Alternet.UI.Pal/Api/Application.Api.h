#pragma once

#include "ApiUtils.h"
#include "Window.h"

#include "Application.h"

using namespace Alternet::UI;

ALTERNET_UI_API Application* Application_Create()
{
    return new Application();
}

ALTERNET_UI_API void Application_Destroy(Application* obj)
{
    delete obj;
}

ALTERNET_UI_API void Application_Run(Application* obj, Alternet::UI::Window* window)
{
    obj->Run(*window);
}