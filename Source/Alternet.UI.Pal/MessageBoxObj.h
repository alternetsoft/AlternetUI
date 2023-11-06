#pragma once

#include "Common.h"
#include "Window.h"

namespace Alternet::UI
{
    class MessageBoxObj
    {
#include "Api/MessageBoxObj.inc"
    public:

    private:
        static long GetStyle(MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton);
        static MessageBoxResult GetResult(int wxResult);
    };
}
