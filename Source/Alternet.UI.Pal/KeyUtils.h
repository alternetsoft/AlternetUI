#pragma once
#include "Common.h"
#include "ApiTypes.h"

namespace Alternet::UI
{
    inline int IsAsciiKey(int value)
    {
        return value >= 33 && value <= 126;
    }

    inline Key WxAsciiKeyToKey(int value)
    {
        if (!IsAsciiKey(value))
            return Key::None;
    
        switch (value)
        {
            case '!': return Key::None;
            case '"': return Key::None;
            case '#': return Key::None;
            case '$': return Key::None;
            case '%': return Key::None;
            case '&': return Key::None;
            case '\'': return Key::Quote;
            case '(': return Key::None;
            case ')': return Key::None;
            case '*': return Key::None;
            case '+': return Key::None;
            case ',': return Key::Comma;
            case '-': return Key::Minus;
            case '.': return Key::Period;
            case '/': return Key::Slash ;
            case '0': return Key::D0;
            case '1': return Key::D1;
            case '2': return Key::D2;
            case '3': return Key::D3;
            case '4': return Key::D4;
            case '5': return Key::D5;
            case '6': return Key::D6;
            case '7': return Key::D7;
            case '8': return Key::D8;
            case '9': return Key::D9;
            case ':': return Key::None;
            case ';': return Key::Semicolon;
            case '<': return Key::None;
            case '=': return Key::Equals;
            case '>': return Key::None;
            case '?': return Key::None;
            case '@': return Key::None;
            case 'A': return Key::A;
            case 'B': return Key::B;
            case 'C': return Key::C;
            case 'D': return Key::D;
            case 'E': return Key::E;
            case 'F': return Key::F;
            case 'G': return Key::G;
            case 'H': return Key::H;
            case 'I': return Key::I;
            case 'J': return Key::J;
            case 'K': return Key::K;
            case 'L': return Key::L;
            case 'M': return Key::M;
            case 'N': return Key::N;
            case 'O': return Key::O;
            case 'P': return Key::P;
            case 'Q': return Key::Q;
            case 'R': return Key::R;
            case 'S': return Key::S;
            case 'T': return Key::T;
            case 'U': return Key::U;
            case 'V': return Key::V;
            case 'W': return Key::W;
            case 'X': return Key::X;
            case 'Y': return Key::Y;
            case 'Z': return Key::Z;
            case '[': return Key::OpenBracket;
            case '\\': return Key::Backslash;
            case ']': return Key::CloseBracket;
            case '^': return Key::None;
            case '_': return Key::None;
            case '`': return Key::Backtick;
            case 'a': return Key::A;
            case 'b': return Key::B;
            case 'c': return Key::C;
            case 'd': return Key::D;
            case 'e': return Key::E;
            case 'f': return Key::F;
            case 'g': return Key::G;
            case 'h': return Key::H;
            case 'i': return Key::I;
            case 'j': return Key::J;
            case 'k': return Key::K;
            case 'l': return Key::L;
            case 'm': return Key::M;
            case 'n': return Key::N;
            case 'o': return Key::O;
            case 'p': return Key::P;
            case 'q': return Key::Q;
            case 'r': return Key::R;
            case 's': return Key::S;
            case 't': return Key::T;
            case 'u': return Key::U;
            case 'v': return Key::V;
            case 'w': return Key::W;
            case 'x': return Key::X;
            case 'y': return Key::Y;
            case 'z': return Key::Z;
            case '{': return Key::None;
            case '|': return Key::None;
            case '}': return Key::None;
            case '~': return Key::None;
            default:
                wxASSERT(false);
                throw 0;
        }
    }

	inline Key WxKeyToKey(int value)
	{
        auto asciiKey = WxAsciiKeyToKey(value);
        if (asciiKey != Key::None)
            return asciiKey;

        wxASSERT(!IsAsciiKey(value)); // todo: test and remove.

        switch (value)
        {
        case WXK_NONE:
            return Key::None;
        case WXK_CONTROL_A:
        case WXK_CONTROL_B:
        case WXK_CONTROL_C:
        case WXK_CONTROL_D:
        case WXK_CONTROL_E:
        case WXK_CONTROL_F:
        case WXK_CONTROL_G:
        // case WXK_CONTROL_H: same as WXK_BACK
        // case WXK_CONTROL_I: same as WXK_TAB
        case WXK_CONTROL_J:
        case WXK_CONTROL_K:
        case WXK_CONTROL_L:
        // case WXK_CONTROL_M: same as WXK_RETURN
        case WXK_CONTROL_N:
        case WXK_CONTROL_O:
        case WXK_CONTROL_P:
        case WXK_CONTROL_Q:
        case WXK_CONTROL_R:
        case WXK_CONTROL_S:
        case WXK_CONTROL_T:
        case WXK_CONTROL_U:
        case WXK_CONTROL_V:
        case WXK_CONTROL_W:
        case WXK_CONTROL_X:
        case WXK_CONTROL_Y:
        case WXK_CONTROL_Z:
            return Key::None;
        case WXK_BACK:
            return Key::Backspace;
        case WXK_TAB:
            return Key::Tab;
        case WXK_RETURN:
            return Key::Enter;
        case WXK_ESCAPE:
            return Key::Escape;
        case WXK_SPACE:
            return Key::Space;
        case WXK_DELETE:
            return Key::Delete;
        case WXK_START:
            return Key::None;
        case WXK_CLEAR:
            return Key::Clear;
        case WXK_SHIFT:
            return Key::Shift;
        case WXK_ALT:
            return Key::Alt;
        case WXK_CONTROL:
            return Key::Control;
        case WXK_MENU:
            return Key::Menu;
        case WXK_PAUSE:
            return Key::Pause;
        case WXK_CAPITAL:
            return Key::CapsLock;
        case WXK_END:
            return Key::End;
        case WXK_HOME:
            return Key::Home;
        case WXK_LEFT:
            return Key::LeftArrow;
        case WXK_UP:
            return Key::UpArrow;
        case WXK_RIGHT:
            return Key::RightArrow;
        case WXK_DOWN:
            return Key::DownArrow;
        case WXK_SELECT:
            return Key::None;
        case WXK_PRINT:
            return Key::None;
        case WXK_EXECUTE:
            return Key::None;
        case WXK_SNAPSHOT:
            return Key::PrintScreen;
        case WXK_INSERT:
            return Key::Insert;
        case WXK_HELP:
            return Key::None;
        case WXK_NUMPAD0:
            return Key::NumPad0;
        case WXK_NUMPAD1:
            return Key::NumPad1;
        case WXK_NUMPAD2:
            return Key::NumPad2;
        case WXK_NUMPAD3:
            return Key::NumPad3;
        case WXK_NUMPAD4:
            return Key::NumPad4;
        case WXK_NUMPAD5:
            return Key::NumPad5;
        case WXK_NUMPAD6:
            return Key::NumPad6;
        case WXK_NUMPAD7:
            return Key::NumPad7;
        case WXK_NUMPAD8:
            return Key::NumPad8;
        case WXK_NUMPAD9:
            return Key::NumPad9;
        case WXK_MULTIPLY:
            return Key::NumPadStar;
        case WXK_ADD:
            return Key::NumPadPlus;
        case WXK_SEPARATOR:
            return Key::None;
        case WXK_SUBTRACT:
            return Key::NumPadMinus;
        case WXK_DECIMAL:
            return Key::NumPadDot;
        case WXK_DIVIDE:
            return Key::Slash;
        case WXK_F1:
            return Key::F1;
        case WXK_F2:
            return Key::F2;
        case WXK_F3:
            return Key::F3;
        case WXK_F4:
            return Key::F4;
        case WXK_F5:
            return Key::F5;
        case WXK_F6:
            return Key::F6;
        case WXK_F7:
            return Key::F7;
        case WXK_F8:
            return Key::F8;
        case WXK_F9:
            return Key::F9;
        case WXK_F10:
            return Key::F10;
        case WXK_F11:
            return Key::F11;
        case WXK_F12:
            return Key::F12;
        case WXK_F13:
            return Key::F13;
        case WXK_F14:
            return Key::F14;
        case WXK_F15:
            return Key::F15;
        case WXK_F16:
            return Key::F15;
        case WXK_F17:
            return Key::F17;
        case WXK_F18:
            return Key::F18;
        case WXK_F19:
            return Key::F19;
        case WXK_F20:
            return Key::F20;
        case WXK_F21:
            return Key::F21;
        case WXK_F22:
            return Key::F22;
        case WXK_F23:
            return Key::F23;
        case WXK_F24:
            return Key::F24;
        case WXK_NUMLOCK:
            return Key::NumLock;
        case WXK_SCROLL:
            return Key::ScrollLock;
        case WXK_PAGEUP:
            return Key::PageUp;
        case WXK_PAGEDOWN:
            return Key::PageDown;
        case WXK_NUMPAD_SPACE:
            return Key::Space;
        case WXK_NUMPAD_TAB:
            return Key::Tab;
        case WXK_NUMPAD_ENTER:
            return Key::Enter;
        case WXK_NUMPAD_F1:
            return Key::F1;
        case WXK_NUMPAD_F2:
            return Key::F2;
        case WXK_NUMPAD_F3:
            return Key::F3;
        case WXK_NUMPAD_F4:
            return Key::F4;
        case WXK_NUMPAD_HOME:
            return Key::Home;
        case WXK_NUMPAD_LEFT:
            return Key::LeftArrow;
        case WXK_NUMPAD_UP:
            return Key::UpArrow;
        case WXK_NUMPAD_RIGHT:
            return Key::RightArrow;
        case WXK_NUMPAD_DOWN:
            return Key::DownArrow;
        case WXK_NUMPAD_PAGEUP:
            return Key::PageUp;
        case WXK_NUMPAD_PAGEDOWN:
            return Key::PageDown;
        case WXK_NUMPAD_END:
            return Key::End;
        case WXK_NUMPAD_BEGIN:
            return Key::None;
        case WXK_NUMPAD_INSERT:
            return Key::Insert;
        case WXK_NUMPAD_DELETE:
            return Key::Delete;
        case WXK_NUMPAD_EQUAL:
            return Key::Equals;
        case WXK_NUMPAD_MULTIPLY:
            return Key::NumPadStar;
        case WXK_NUMPAD_ADD:
            return Key::NumPadPlus;
        case WXK_NUMPAD_SEPARATOR:
            return Key::None;
        case WXK_NUMPAD_SUBTRACT:
            return Key::NumPadMinus;
        case WXK_NUMPAD_DECIMAL:
            return Key::NumPadDot;
        case WXK_NUMPAD_DIVIDE:
            return Key::NumPadSlash;
        case WXK_WINDOWS_LEFT:
            return Key::Windows;
        case WXK_WINDOWS_RIGHT:
            return Key::Windows;
        case WXK_WINDOWS_MENU:
            return Key::Menu;
        // case WXK_RAW_CONTROL: same as WXK_CONTROL
            //return Key::None;
        // case WXK_COMMAND: same as WXK_CONTROL
            //return Key::None;
        case WXK_SPECIAL1:
        case WXK_SPECIAL2:
        case WXK_SPECIAL3:
        case WXK_SPECIAL4:
        case WXK_SPECIAL5:
        case WXK_SPECIAL6:
        case WXK_SPECIAL7:
        case WXK_SPECIAL8:
        case WXK_SPECIAL9:
        case WXK_SPECIAL10:
        case WXK_SPECIAL11:
        case WXK_SPECIAL12:
        case WXK_SPECIAL13:
        case WXK_SPECIAL14:
        case WXK_SPECIAL15:
        case WXK_SPECIAL16:
        case WXK_SPECIAL17:
        case WXK_SPECIAL18:
        case WXK_SPECIAL19:
        case WXK_SPECIAL20:
            return Key::None;
        case WXK_BROWSER_BACK:
            return Key::BrowserBack;
        case WXK_BROWSER_FORWARD:
            return Key::BrowserForward;
        case WXK_BROWSER_REFRESH:
            return Key::BrowserRefresh;
        case WXK_BROWSER_STOP:
            return Key::BrowserStop;
        case WXK_BROWSER_SEARCH:
            return Key::BrowserSearch;
        case WXK_BROWSER_FAVORITES:
            return Key::BrowserFavorites;
        case WXK_BROWSER_HOME:
            return Key::BrowserHome;
        case WXK_VOLUME_MUTE:
            return Key::VolumeMute;
        case WXK_VOLUME_DOWN:
            return Key::VolumeDown;
        case WXK_VOLUME_UP:
            return Key::VolumeUp;
        case WXK_MEDIA_NEXT_TRACK:
            return Key::MediaNextTrack;
        case WXK_MEDIA_PREV_TRACK:
            return Key::MediaPreviousTrack;
        case WXK_MEDIA_STOP:
            return Key::BrowserStop;
        case WXK_MEDIA_PLAY_PAUSE:
            return Key::MediaPlayPause;
        case WXK_LAUNCH_MAIL:
            return Key::LaunchMail;
        case WXK_LAUNCH_APP1:
            return Key::LaunchApplication1;
        case WXK_LAUNCH_APP2:
            return Key::LaunchApplication2;
        default:
            wxASSERT(false);
            throw 0;
        }
	}
}