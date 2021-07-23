// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

#pragma once

#include "Application.h"
#include "Window.h"
#include "ApiUtils.h"

using namespace Alternet::UI;

ALTERNET_UI_API Application* Application_Create_()
{
    return new Application();
}

ALTERNET_UI_API void Application_Run_(Application* obj, Window* window)
{
    obj->Run(window);
}

