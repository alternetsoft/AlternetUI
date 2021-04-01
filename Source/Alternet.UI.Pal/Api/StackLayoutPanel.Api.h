// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

#pragma once

#include "StackLayoutPanel.h"
#include "ApiUtils.h"

using namespace Alternet::UI;

ALTERNET_UI_API StackLayoutPanel* StackLayoutPanel_Create()
{
    return new StackLayoutPanel();
}

ALTERNET_UI_API void StackLayoutPanel_Destroy(StackLayoutPanel* obj)
{
    delete obj;
}

ALTERNET_UI_API StackLayoutOrientation StackLayoutPanel_GetOrientation(StackLayoutPanel* obj)
{
    return obj->GetOrientation();
}

ALTERNET_UI_API void StackLayoutPanel_SetOrientation(StackLayoutPanel* obj, StackLayoutOrientation value)
{
    obj->SetOrientation(value);
}

