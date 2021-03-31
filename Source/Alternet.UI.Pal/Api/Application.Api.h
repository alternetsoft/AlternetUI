// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

#pragma once

#include "Application.h"
#include "Window.h"
#include "ApiUtils.h"

using namespace Alternet::UI;

ALTERNET_UI_API Application* Application_Create()
{
    return new Application();
}

ALTERNET_UI_API void Application_Destroy(Application* obj)
{
    delete obj;
}

ALTERNET_UI_API void Application_Run(Application* obj, Window* window)
{
    obj->Run(*window);
}

