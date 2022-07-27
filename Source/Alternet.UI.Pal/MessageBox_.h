#pragma once

#include "Common.h"
#include "Window.h"

namespace Alternet::UI
{
    class MessageBox_
    {
#include "Api/MessageBox_.inc"
    public:

    private:
        static long GetStyle(MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton);
        static MessageBoxResult GetResult(int wxResult);
    };
}
