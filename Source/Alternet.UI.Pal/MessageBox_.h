#pragma once

#include "Common.h"

namespace Alternet::UI
{
    class MessageBox_
    {
    public:
        static void Show(string text, string caption);

    private:
        MessageBox_() {}
        virtual ~MessageBox_() {}
        BYREF_ONLY(MessageBox_);
    };
}
