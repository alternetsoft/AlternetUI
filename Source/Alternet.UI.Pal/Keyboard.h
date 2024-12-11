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

        wxAcceleratorEntryFlags ModifierKeysToAcceleratorFlags(ModifierKeys modifierKeys);

    private:
        char16_t _inputChar;
        uint8_t _inputEventCode;
        bool _inputHandled;
        Key _inputKey;
        bool _isRepeat;

        void SetFields(wxKeyEvent& e, uint8_t eventCode);
        int IsAsciiKey(int value);
        Key WxAsciiKeyToKey(int value);
        Key WxKeyToKey(int value);

        std::vector<int> KeyToWxKeys(Key value);
        bool KeyHasMultipleWxKeys(Key value);
    };
}