#pragma once

#include "ApiUtils.h"
#include "Object.h"

using namespace Alternet::UI;

ALTERNET_UI_API void Object_Release(Object* obj)
{
    obj->Release();
}