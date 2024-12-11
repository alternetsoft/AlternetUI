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
        void OnKeyDown(wxKeyEvent& e, bool& handled);
        void OnKeyUp(wxKeyEvent& e, bool& handled);
        void OnChar(wxKeyEvent& e, bool& handled);

        static int KeyToWxKey(Key value);

        static wxAcceleratorEntryFlags ModifierKeysToAcceleratorFlags(ModifierKeys modifierKeys);

    private:
        static char16_t _inputChar;
        static uint8_t _inputEventCode;
        static bool _inputHandled;
        static Key _inputKey;
        static bool _isRepeat;

        static void SetFields(wxKeyEvent& e, uint8_t eventCode);
        static int IsAsciiKey(int value);
        static Key WxAsciiKeyToKey(int value);
        static Key WxKeyToKey(int value);

        static std::vector<int> KeyToWxKeys(Key value);
        static bool KeyHasMultipleWxKeys(Key value);
    };
}