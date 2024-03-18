#pragma once

#include "ApiUtils.h"
#include "Object.h"

using namespace Alternet::UI;

ALTERNET_UI_API void Object_AddRef(Object* obj)
{
    obj->AddRef();
}

ALTERNET_UI_API void Object_Release(Object* obj)
{
    obj->Release();
}

ALTERNET_UI_API uint64_t Object_GetId(Object* obj)
{
    return obj->GetId();
}

ALTERNET_UI_API void Object_SetId(Object* obj, uint64_t value)
{
    obj->SetId(value);
}
