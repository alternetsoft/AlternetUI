#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"

namespace Alternet::UI
{
    class Keyboard : public Object
    {
#include "Api/Keyboard.inc"
    public:
        void OnKeyDown(wxKeyEvent& e);
        void OnKeyUp(wxKeyEvent& e);

    private:
        int IsAsciiKey(int value);
        Key WxAsciiKeyToKey(int value);
        Key WxKeyToKey(int value);

        int KeyToWxKey(Key value);
        std::vector<int> KeyToWxKeys(Key value);
        bool KeyHasMultipleWxKeys(Key value);
    };
}
