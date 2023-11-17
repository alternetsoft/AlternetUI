#include "Keyboard.h"
#include "Application.h"

namespace Alternet::UI
{
	Keyboard::Keyboard()
	{
	}

	Keyboard::~Keyboard()
	{
	}

	bool IsValidLinuxKey(Key key)
	{
		if (key == Key::Control || key == Key::ScrollLock || key == Key::NumLock ||
			key == Key::CapsLock || key == Key::Shift || key == Key::Alt)
			return true;
		return false;
	}

	KeyStates Keyboard::GetKeyState(Key key)
	{
		// We check here for Linux as under Ubuntu 23 (and probably 22)
		// we have an exception in IsKeyDown_private
		//  ../src/unix/utilsx11.cpp(2645): assert ""Assert failure"" failed in
		//  wxGetKeyStateGTK(): Unsupported key 393, the only supported ones
		//  are: Ctrl, Alt, Shift, Caps Lock, Num Lock and Scroll Lock for GTK 3.18+
#ifdef __WXGTK__
		if (!IsValidLinuxKey(key))
			return KeyStates::None;
#endif

		std::vector<int> wxKeys;
		if (KeyHasMultipleWxKeys(key))
			wxKeys = KeyToWxKeys(key);
		else
			wxKeys.push_back(KeyToWxKey(key));

		for (int wxKey : wxKeys)
		{
			if (wxKey == WXK_NONE)
				continue;

			auto kk = (wxKeyCode)wxKey;

			if (wxGetKeyState(kk))
			{
				if (key == Key::CapsLock || key == Key::NumLock)
				{
					/*
					https://groups.google.com/g/wx-dev/c/LZdkOSReRF0
					Caps Lock, Num Lock and Scroll Lock are "special" though. From the docs:
					>For togglable keys (Caps Lock, Num Lock and Scroll Lock), returns true
					>if the key is toggled such that its LED indicator is lit. There is
					>currently no way to test whether togglable keys are up or down.
					*/
					return KeyStates::Toggled;
				}
				return KeyStates::Down;
			}
		}

		return KeyStates::None;
	}

	void Keyboard::OnChar(wxKeyEvent& e, bool& handled)
	{
		auto unicodeKey = e.GetUnicodeKey();
		auto char16 = wcharToChar16(unicodeKey);
		TextInputEventData textInputdata{ char16, e.GetTimestamp() };
		handled = RaiseEvent(KeyboardEvent::TextInput, &textInputdata);
	}

	void Keyboard::OnKeyDown(wxKeyEvent& e, bool& handled)
	{
		KeyEventData keyData{ WxKeyToKey(e.GetKeyCode()), e.GetTimestamp(), e.IsAutoRepeat() };
		handled = RaiseEvent(KeyboardEvent::KeyDown, &keyData);
	}

	void Keyboard::OnKeyUp(wxKeyEvent& e, bool& handled)
	{
		KeyEventData data{ WxKeyToKey(e.GetKeyCode()), e.GetTimestamp(), false };
		handled = RaiseEvent(KeyboardEvent::KeyUp, &data);
	}

	int Keyboard::IsAsciiKey(int value)
	{
		return value >= 33 && value <= 126;
	}

	int Keyboard::KeyToWxKey(Key value)
	{
		switch (value)
		{
		case Key::None: return WXK_NONE;
		case Key::Quote: return '\'';
		case Key::Comma: return ',';
		case Key::Minus: return '-';
		case Key::Period: return '.';
		case Key::Slash: return '/';
		case Key::D0: return '0';
		case Key::D1: return '1';
		case Key::D2: return '2';
		case Key::D3: return '3';
		case Key::D4: return '4';
		case Key::D5: return '5';
		case Key::D6: return '6';
		case Key::D7: return '7';
		case Key::D8: return '8';
		case Key::D9: return '9';
		case Key::Semicolon: return ';';
		case Key::Equals: return '=';
		case Key::A: return 'A';
		case Key::B: return 'B';
		case Key::C: return 'C';
		case Key::D: return 'D';
		case Key::E: return 'E';
		case Key::F: return 'F';
		case Key::G: return 'G';
		case Key::H: return 'H';
		case Key::I: return 'I';
		case Key::J: return 'J';
		case Key::K: return 'K';
		case Key::L: return 'L';
		case Key::M: return 'M';
		case Key::N: return 'N';
		case Key::O: return 'O';
		case Key::P: return 'P';
		case Key::Q: return 'Q';
		case Key::R: return 'R';
		case Key::S: return 'S';
		case Key::T: return 'T';
		case Key::U: return 'U';
		case Key::V: return 'V';
		case Key::W: return 'W';
		case Key::X: return 'X';
		case Key::Y: return 'Y';
		case Key::Z: return 'Z';
		case Key::OpenBracket: return '[';
		case Key::Backslash: return '\\';
		case Key::CloseBracket: return ']';
		case Key::Backtick: return '`';
		case Key::Backspace: return WXK_BACK;
		case Key::Tab: return WXK_TAB;
		case Key::Enter: return WXK_RETURN;
		case Key::Escape: return WXK_ESCAPE;
		case Key::Space: return WXK_SPACE;
		case Key::Delete: return WXK_DELETE;
		case Key::Clear: return WXK_CLEAR;
		case Key::Shift: return WXK_SHIFT;
		case Key::MacCommand: return WXK_CONTROL;
		case Key::MacOption: return WXK_ALT;
#ifdef __WXOSX__
		case Key::MacControl: return WXK_RAW_CONTROL;
#else
		case Key::MacControl: return WXK_CONTROL;
#endif
		case Key::Alt: return WXK_ALT;
		case Key::Control: return WXK_CONTROL;
		case Key::Pause: return WXK_PAUSE;
		case Key::CapsLock: return WXK_CAPITAL;
		case Key::End: return WXK_END;
		case Key::Home: return WXK_HOME;
		case Key::LeftArrow: return WXK_LEFT;
		case Key::UpArrow: return WXK_UP;
		case Key::RightArrow: return WXK_RIGHT;
		case Key::DownArrow: return WXK_DOWN;
		case Key::PrintScreen: return WXK_SNAPSHOT;
		case Key::Insert: return WXK_INSERT;
		case Key::NumPad0: return WXK_NUMPAD0;
		case Key::NumPad1: return WXK_NUMPAD1;
		case Key::NumPad2: return WXK_NUMPAD2;
		case Key::NumPad3: return WXK_NUMPAD3;
		case Key::NumPad4: return WXK_NUMPAD4;
		case Key::NumPad5: return WXK_NUMPAD5;
		case Key::NumPad6: return WXK_NUMPAD6;
		case Key::NumPad7: return WXK_NUMPAD7;
		case Key::NumPad8: return WXK_NUMPAD8;
		case Key::NumPad9: return WXK_NUMPAD9;
		case Key::NumPadStar: return WXK_MULTIPLY;
		case Key::NumPadPlus: return WXK_ADD;
		case Key::NumPadMinus: return WXK_SUBTRACT;
		case Key::NumPadDot: return WXK_DECIMAL;
		case Key::F1: return WXK_F1;
		case Key::F2: return WXK_F2;
		case Key::F3: return WXK_F3;
		case Key::F4: return WXK_F4;
		case Key::F5: return WXK_F5;
		case Key::F6: return WXK_F6;
		case Key::F7: return WXK_F7;
		case Key::F8: return WXK_F8;
		case Key::F9: return WXK_F9;
		case Key::F10: return WXK_F10;
		case Key::F11: return WXK_F11;
		case Key::F12: return WXK_F12;
		case Key::F13: return WXK_F13;
		case Key::F14: return WXK_F14;
		case Key::F15: return WXK_F15;
		case Key::F16: return WXK_F16;
		case Key::F17: return WXK_F17;
		case Key::F18: return WXK_F18;
		case Key::F19: return WXK_F19;
		case Key::F20: return WXK_F20;
		case Key::F21: return WXK_F21;
		case Key::F22: return WXK_F22;
		case Key::F23: return WXK_F23;
		case Key::F24: return WXK_F24;
		case Key::NumLock: return WXK_NUMLOCK;
		case Key::ScrollLock: return WXK_SCROLL;
		case Key::PageUp: return WXK_PAGEUP;
		case Key::PageDown: return WXK_PAGEDOWN;
		case Key::NumPadSlash: return WXK_NUMPAD_DIVIDE;
		case Key::BrowserBack: return WXK_BROWSER_BACK;
		case Key::BrowserForward: return WXK_BROWSER_FORWARD;
		case Key::BrowserRefresh: return WXK_BROWSER_REFRESH;
		case Key::BrowserStop: return WXK_BROWSER_STOP;
		case Key::BrowserSearch: return WXK_BROWSER_SEARCH;
		case Key::BrowserFavorites: return WXK_BROWSER_FAVORITES;
		case Key::BrowserHome: return WXK_BROWSER_HOME;
		case Key::VolumeMute: return WXK_VOLUME_MUTE;
		case Key::VolumeDown: return WXK_VOLUME_DOWN;
		case Key::VolumeUp: return WXK_VOLUME_UP;
		case Key::MediaNextTrack: return WXK_MEDIA_NEXT_TRACK;
		case Key::MediaPreviousTrack: return WXK_MEDIA_PREV_TRACK;
		case Key::MediaStop: return WXK_MEDIA_STOP;
		case Key::MediaPlayPause: return WXK_MEDIA_PLAY_PAUSE;
		case Key::LaunchMail: return WXK_LAUNCH_MAIL;
		case Key::LaunchApplication1: return WXK_LAUNCH_APP1;
		case Key::LaunchApplication2: return WXK_LAUNCH_APP2;

		case Key::ExclamationMark: '!';
		case Key::QuotationMark:  '"';
		case Key::NumberSign: 136, '#';
		case Key::DollarSign: 137, '$';
		case Key::PercentSign: 138, '%';
		case Key::Ampersand: 139, '&';
		case Key::LeftParenthesis:  '(';
		case Key::RightParenthesis:')';
		case Key::Asterisk: '*';
		case Key::PlusSign: '+';
		case Key::Colon: ':';
		case Key::LessThanSign: '<';
		case Key::GreaterThanSign: '>';
		case Key::QuestionMark: '?';
		case Key::CommercialAt: '@';
		case Key::CircumflexAccent: '^';
		case Key::LowLine: '_';
		case Key::LeftCurlyBracket: '{';
		case Key::VerticalLine: '|';
		case Key::RightCurlyBracket: '}';
		case Key::Tilde: '~';
		default:
			return WXK_NONE;
		}
	}

	wxAcceleratorEntryFlags Keyboard::ModifierKeysToAcceleratorFlags(ModifierKeys modifierKeys)
	{
		wxAcceleratorEntryFlags result = wxACCEL_NORMAL;

		if ((modifierKeys & ModifierKeys::Alt) != ModifierKeys::None)
			result = (wxAcceleratorEntryFlags)(result | wxACCEL_ALT);
		if ((modifierKeys & ModifierKeys::Control) != ModifierKeys::None)
			result = (wxAcceleratorEntryFlags)(result | wxACCEL_CTRL);
		if ((modifierKeys & ModifierKeys::Shift) != ModifierKeys::None)
			result = (wxAcceleratorEntryFlags)(result | wxACCEL_SHIFT);
		if ((modifierKeys & ModifierKeys::Windows) != ModifierKeys::None)
			result = (wxAcceleratorEntryFlags)(result | wxACCEL_RAW_CTRL);

		return result;
}

	std::vector<int> Keyboard::KeyToWxKeys(Key value)
	{
		if (value == Key::Menu)
			return { WXK_WINDOWS_MENU, WXK_MENU };
		if (value == Key::Windows)
#ifdef __WXOSX__
			// Commented out as shows error:
			// Debug: Unrecognised keycode 393
			// Debug : Unrecognised keycode 394
			return { /*WXK_WINDOWS_LEFT, WXK_WINDOWS_RIGHT, WXK_RAW_CONTROL*/ };
#else
			return { WXK_WINDOWS_LEFT, WXK_WINDOWS_RIGHT };
#endif

		return { };
	}

	bool Keyboard::KeyHasMultipleWxKeys(Key value)
	{
		return value == Key::Menu || value == Key::Windows;
	}

	Key Keyboard::WxAsciiKeyToKey(int value)
	{
		if (!IsAsciiKey(value))
			return Key::None;

		switch (value)
		{
		case '!': return Key::ExclamationMark;     // 33 - 0x21 - 
		case '"': return Key::QuotationMark;     // 34 - 0x22 - 
		case '#': return Key::NumberSign;     // 35 - 0x23 - 
		case '$': return Key::DollarSign;     // 36 - 0x24 - 
		case '%': return Key::PercentSign;     // 37 - 0x25 -   
		case '&': return Key::Ampersand;     // 38 - 0x26 - 
		case '\'': return Key::Quote;   // 39   
		case '(': return Key::LeftParenthesis;     // 40 - 0x28 - 
		case ')': return Key::RightParenthesis;     // 41 - 0x29 - 
		case '*': return Key::Asterisk;     // 42 - 0x2A - 
		case '+': return Key::PlusSign;     // 43 - 0x2B - 
		case ',': return Key::Comma;    // 44
		case '-': return Key::Minus;    // 45
		case '.': return Key::Period;   // 46
		case '/': return Key::Slash;    // 47
		case '0': return Key::D0;       // 48
		case '1': return Key::D1;       // 49
		case '2': return Key::D2;       // 50
		case '3': return Key::D3;       // 51
		case '4': return Key::D4;       // 52
		case '5': return Key::D5;       // 53
		case '6': return Key::D6;       // 54
		case '7': return Key::D7;       // 55
		case '8': return Key::D8;       // 56
		case '9': return Key::D9;       // 57
		case ':': return Key::Colon;            // 58 - 0x3A - Colon
		case ';': return Key::Semicolon;        // 59 
		case '<': return Key::LessThanSign;     // 60 - 0x3C - Less-Than Sign
		case '=': return Key::Equals;           // 61
		case '>': return Key::GreaterThanSign;  // 62 - 0x3E - Greater-Than
		case '?': return Key::QuestionMark;     // 63 - 0x3F - Question Mark
		case '@': return Key::CommercialAt;     // 64 - 0x40 - Commercial At
		case 'A': return Key::A;        // 65
		case 'B': return Key::B;        // 66
		case 'C': return Key::C;        // 67
		case 'D': return Key::D;        // 68
		case 'E': return Key::E;        // 69
		case 'F': return Key::F;        // 70
		case 'G': return Key::G;        // 71
		case 'H': return Key::H;        // 72
		case 'I': return Key::I;        // 73
		case 'J': return Key::J;        // 74
		case 'K': return Key::K;        // 75
		case 'L': return Key::L;        // 76
		case 'M': return Key::M;        // 77
		case 'N': return Key::N;        // 78
		case 'O': return Key::O;        // 79
		case 'P': return Key::P;        // 80
		case 'Q': return Key::Q;        // 81
		case 'R': return Key::R;        // 82
		case 'S': return Key::S;        // 83
		case 'T': return Key::T;        // 84
		case 'U': return Key::U;        // 85
		case 'V': return Key::V;        // 86
		case 'W': return Key::W;        // 87
		case 'X': return Key::X;        // 88
		case 'Y': return Key::Y;        // 89
		case 'Z': return Key::Z;        // 90
		case '[': return Key::OpenBracket; // 91
		case '\\': return Key::Backslash;  // 92
		case ']': return Key::CloseBracket;// 93
		case '^': return Key::CircumflexAccent;     // 94 - 0x5E Circumflex Accent
		case '_': return Key::LowLine;              // 95 - 0x5F Low Line 
		case '`': return Key::Backtick;    // 96
		case 'a': return Key::A;           // 97
		case 'b': return Key::B;           // 98
		case 'c': return Key::C;           // 99
		case 'd': return Key::D;           // 100
		case 'e': return Key::E;           // 101
		case 'f': return Key::F;           //
		case 'g': return Key::G;           //
		case 'h': return Key::H;           //
		case 'i': return Key::I;           //
		case 'j': return Key::J;           //
		case 'k': return Key::K;           //
		case 'l': return Key::L;           //
		case 'm': return Key::M;           //
		case 'n': return Key::N;           //
		case 'o': return Key::O;           //
		case 'p': return Key::P;           //
		case 'q': return Key::Q;           //
		case 'r': return Key::R;           // 114 
		case 's': return Key::S;           // 115 
		case 't': return Key::T;           // 116 
		case 'u': return Key::U;           // 117 
		case 'v': return Key::V;           // 118
		case 'w': return Key::W;           // 119
		case 'x': return Key::X;           // 120
		case 'y': return Key::Y;           // 121
		case 'z': return Key::Z;           // 122
		case '{': return Key::LeftCurlyBracket;     // 123 - 0x7B - Left Curly Bracket
		case '|': return Key::VerticalLine;         // 124 - 0x7C - Vertical Line
		case '}': return Key::RightCurlyBracket;    // 125 - 0x7D - Right Curly Bracket
		case '~': return Key::Tilde;                // 126 - 0x7E - Tilde
		default:
			// auto s = std::to_string(value);
			// Application::Log("Invalid key ("+s+") in Keyboard::WxAsciiKeyToKey");
			return Key::None;
		}
	}

	Key Keyboard::WxKeyToKey(int value)
	{
		auto asciiKey = WxAsciiKeyToKey(value);
		if (asciiKey != Key::None)
			return asciiKey;

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
			return Key::None;  // !!
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
			return Key::None;  // !!
		case WXK_CLEAR:
			return Key::Clear;
		case WXK_SHIFT:
			return Key::Shift;
		case WXK_ALT:
			return Key::Alt;
		case WXK_CONTROL:
			return Key::Control;
		case WXK_WINDOWS_LEFT:
			return Key::Windows;
		case WXK_WINDOWS_RIGHT:
			return Key::Windows;
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
			return Key::None; // !!
		case WXK_PRINT:
			return Key::None; // !!
		case WXK_EXECUTE:
			return Key::None; // !!
		case WXK_SNAPSHOT:
			return Key::PrintScreen;
		case WXK_INSERT:
			return Key::Insert;
		case WXK_HELP:
			return Key::None; // !!
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
			return Key::None; // !!
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
			return Key::F16;
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
			return Key::None; // !!
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
			return Key::None;  // !!
		case WXK_NUMPAD_SUBTRACT:
			return Key::NumPadMinus;
		case WXK_NUMPAD_DECIMAL:
			return Key::NumPadDot;
		case WXK_NUMPAD_DIVIDE:
			return Key::NumPadSlash;
		case WXK_WINDOWS_MENU:
			return Key::Menu;
#ifdef __WXOSX__
		case WXK_RAW_CONTROL:
			return Key::Windows;
#endif
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
			return Key::None; // !!
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
			return Key::MediaStop;
		case WXK_MEDIA_PLAY_PAUSE:
			return Key::MediaPlayPause;
		case WXK_LAUNCH_MAIL:
			return Key::LaunchMail;
		case WXK_LAUNCH_APP1:
			return Key::LaunchApplication1;
		case WXK_LAUNCH_APP2:
			return Key::LaunchApplication2;
		default:
			// auto s = std::to_string(value);
			// Application::Log("Invalid key (" + s + ") in Keyboard::WxKeyToKey");
			return Key::None;
		}
	}
}
