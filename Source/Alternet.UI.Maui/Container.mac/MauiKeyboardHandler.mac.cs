using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.Controls.PlatformConfiguration;

#if IOS || MACCATALYST

using UIKit;

namespace Alternet.UI
{
    /// <summary>
    /// Implements <see cref="IKeyboardHandler"/> for MAUI platform under MacOs.
    /// </summary>
    public class MauiKeyboardHandler : MappedKeyboardHandler<UIKeyboardHidUsage>
    {
        /// <summary>
        /// Gets or sets default <see cref="IKeyboardHandler"/> implementation.
        /// </summary>
        public static MauiKeyboardHandler Default = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="MauiKeyboardHandler"/> class.
        /// </summary>
        public MauiKeyboardHandler()
            : base(UIKeyboardHidUsage.KeyboardRightGui, Key.MaxMaui)
        {
        }

        /// <inheritdoc/>
        public override void RegisterKeyMappings()
        {
            AddMapping(UIKeyboardHidUsage.KeyboardA, Key.A);
            AddMapping(UIKeyboardHidUsage.KeyboardB, Key.B);
            AddMapping(UIKeyboardHidUsage.KeyboardC, Key.C);
            AddMapping(UIKeyboardHidUsage.KeyboardD, Key.D);
            AddMapping(UIKeyboardHidUsage.KeyboardE, Key.E);
            AddMapping(UIKeyboardHidUsage.KeyboardF, Key.F);
            AddMapping(UIKeyboardHidUsage.KeyboardG, Key.G);
            AddMapping(UIKeyboardHidUsage.KeyboardH, Key.H);
            AddMapping(UIKeyboardHidUsage.KeyboardI, Key.I);
            AddMapping(UIKeyboardHidUsage.KeyboardJ, Key.J);
            AddMapping(UIKeyboardHidUsage.KeyboardK, Key.K);
            AddMapping(UIKeyboardHidUsage.KeyboardL, Key.L);
            AddMapping(UIKeyboardHidUsage.KeyboardM, Key.M);
            AddMapping(UIKeyboardHidUsage.KeyboardN, Key.N);
            AddMapping(UIKeyboardHidUsage.KeyboardO, Key.O);
            AddMapping(UIKeyboardHidUsage.KeyboardP, Key.P);
            AddMapping(UIKeyboardHidUsage.KeyboardQ, Key.Q);
            AddMapping(UIKeyboardHidUsage.KeyboardR, Key.R);
            AddMapping(UIKeyboardHidUsage.KeyboardS, Key.S);
            AddMapping(UIKeyboardHidUsage.KeyboardT, Key.T);
            AddMapping(UIKeyboardHidUsage.KeyboardU, Key.U);
            AddMapping(UIKeyboardHidUsage.KeyboardV, Key.V);
            AddMapping(UIKeyboardHidUsage.KeyboardW, Key.W);
            AddMapping(UIKeyboardHidUsage.KeyboardX, Key.X);
            AddMapping(UIKeyboardHidUsage.KeyboardY, Key.Y);
            AddMapping(UIKeyboardHidUsage.KeyboardZ, Key.Z);
            AddMapping(UIKeyboardHidUsage.Keyboard1, Key.D1);
            AddMapping(UIKeyboardHidUsage.Keyboard2, Key.D2);
            AddMapping(UIKeyboardHidUsage.Keyboard3, Key.D3);
            AddMapping(UIKeyboardHidUsage.Keyboard4, Key.D4);
            AddMapping(UIKeyboardHidUsage.Keyboard5, Key.D5);
            AddMapping(UIKeyboardHidUsage.Keyboard6, Key.D6);
            AddMapping(UIKeyboardHidUsage.Keyboard7, Key.D7);
            AddMapping(UIKeyboardHidUsage.Keyboard8, Key.D8);
            AddMapping(UIKeyboardHidUsage.Keyboard9, Key.D9);
            AddMapping(UIKeyboardHidUsage.Keyboard0, Key.D0);
            AddMapping(UIKeyboardHidUsage.KeyboardEscape, Key.Escape);
            AddMapping(UIKeyboardHidUsage.KeyboardTab, Key.Tab);
            AddMapping(UIKeyboardHidUsage.KeyboardSpacebar, Key.Space);
            AddMapping(UIKeyboardHidUsage.KeyboardEqualSign, Key.Equals);
            AddMapping(UIKeyboardHidUsage.KeyboardBackslash, Key.Backslash);
            AddMapping(UIKeyboardHidUsage.KeyboardSemicolon, Key.Semicolon);
            AddMapping(UIKeyboardHidUsage.KeyboardQuote, Key.Quote);
            AddMapping(UIKeyboardHidUsage.KeyboardGraveAccentAndTilde, null);
            AddMapping(UIKeyboardHidUsage.KeyboardComma, Key.Comma);
            AddMapping(UIKeyboardHidUsage.KeyboardPeriod, Key.Period);
            AddMapping(UIKeyboardHidUsage.KeyboardSlash, Key.Slash);
            AddMapping(UIKeyboardHidUsage.KeyboardCapsLock, Key.CapsLock);
            AddMapping(UIKeyboardHidUsage.KeyboardF1, Key.F1);
            AddMapping(UIKeyboardHidUsage.KeyboardF2, Key.F2);
            AddMapping(UIKeyboardHidUsage.KeyboardF3, Key.F3);
            AddMapping(UIKeyboardHidUsage.KeyboardF4, Key.F4);
            AddMapping(UIKeyboardHidUsage.KeyboardF5, Key.F5);
            AddMapping(UIKeyboardHidUsage.KeyboardF6, Key.F6);
            AddMapping(UIKeyboardHidUsage.KeyboardF7, Key.F7);
            AddMapping(UIKeyboardHidUsage.KeyboardF8, Key.F8);
            AddMapping(UIKeyboardHidUsage.KeyboardF9, Key.F9);
            AddMapping(UIKeyboardHidUsage.KeyboardF10, Key.F10);
            AddMapping(UIKeyboardHidUsage.KeyboardF11, Key.F11);
            AddMapping(UIKeyboardHidUsage.KeyboardF12, Key.F12);
            AddMapping(UIKeyboardHidUsage.KeyboardPrintScreen, Key.PrintScreen);
            AddMapping(UIKeyboardHidUsage.KeyboardScrollLock, Key.ScrollLock);
            AddMapping(UIKeyboardHidUsage.KeyboardPause, Key.Pause);
            AddMapping(UIKeyboardHidUsage.KeyboardInsert, Key.Insert);
            AddMapping(UIKeyboardHidUsage.KeyboardHome, Key.Home);
            AddMapping(UIKeyboardHidUsage.KeyboardPageUp, Key.PageUp);
            AddMapping(UIKeyboardHidUsage.KeyboardEnd, Key.End);
            AddMapping(UIKeyboardHidUsage.KeyboardPageDown, Key.PageDown);
            AddMapping(UIKeyboardHidUsage.KeyboardRightArrow, Key.RightArrow);
            AddMapping(UIKeyboardHidUsage.KeyboardLeftArrow, Key.LeftArrow);
            AddMapping(UIKeyboardHidUsage.KeyboardDownArrow, Key.DownArrow);
            AddMapping(UIKeyboardHidUsage.KeyboardUpArrow, Key.UpArrow);
            AddMapping(UIKeyboardHidUsage.KeypadNumLock, Key.NumLock);
            AddMapping(UIKeyboardHidUsage.KeypadSlash, Key.NumPadSlash);
            AddMapping(UIKeyboardHidUsage.KeypadPlus, Key.NumPadPlus);
            AddMapping(UIKeyboardHidUsage.Keypad1, Key.NumPad1);
            AddMapping(UIKeyboardHidUsage.Keypad2, Key.NumPad2);
            AddMapping(UIKeyboardHidUsage.Keypad3, Key.NumPad3);
            AddMapping(UIKeyboardHidUsage.Keypad4, Key.NumPad4);
            AddMapping(UIKeyboardHidUsage.Keypad5, Key.NumPad5);
            AddMapping(UIKeyboardHidUsage.Keypad6, Key.NumPad6);
            AddMapping(UIKeyboardHidUsage.Keypad7, Key.NumPad7);
            AddMapping(UIKeyboardHidUsage.Keypad8, Key.NumPad8);
            AddMapping(UIKeyboardHidUsage.Keypad9, Key.NumPad9);
            AddMapping(UIKeyboardHidUsage.Keypad0, Key.NumPad0);
            AddMapping(UIKeyboardHidUsage.KeyboardF13, Key.F13);
            AddMapping(UIKeyboardHidUsage.KeyboardF14, Key.F14);
            AddMapping(UIKeyboardHidUsage.KeyboardF15, Key.F15);
            AddMapping(UIKeyboardHidUsage.KeyboardF16, Key.F16);
            AddMapping(UIKeyboardHidUsage.KeyboardF17, Key.F17);
            AddMapping(UIKeyboardHidUsage.KeyboardF18, Key.F18);
            AddMapping(UIKeyboardHidUsage.KeyboardF19, Key.F19);
            AddMapping(UIKeyboardHidUsage.KeyboardF20, Key.F20);
            AddMapping(UIKeyboardHidUsage.KeyboardF21, Key.F21);
            AddMapping(UIKeyboardHidUsage.KeyboardF22, Key.F22);
            AddMapping(UIKeyboardHidUsage.KeyboardF23, Key.F23);
            AddMapping(UIKeyboardHidUsage.KeyboardF24, Key.F24);
            AddMapping(UIKeyboardHidUsage.KeyboardMute, Key.VolumeMute);
            AddMapping(UIKeyboardHidUsage.KeyboardVolumeUp, Key.VolumeUp);
            AddMapping(UIKeyboardHidUsage.KeyboardVolumeDown, Key.VolumeDown);
            AddMapping(UIKeyboardHidUsage.KeyboardHangul, Key.Hangul);
            AddMapping(UIKeyboardHidUsage.KeyboardKanaSwitch, Key.Kana);
            AddMapping(UIKeyboardHidUsage.KeyboardHanja, Key.Hanja);

            AddMapping(UIKeyboardHidUsage.KeyboardAlphanumericSwitch, null);
            AddMapping(UIKeyboardHidUsage.KeyboardKatakana, null);
            AddMapping(UIKeyboardHidUsage.KeyboardHiragana, null);
            AddMapping(UIKeyboardHidUsage.KeyboardZenkakuHankakuKanji, null);
            AddMapping(UIKeyboardHidUsage.KeyboardLang6, null);
            AddMapping(UIKeyboardHidUsage.KeyboardLang7, null);
            AddMapping(UIKeyboardHidUsage.KeyboardLang8, null);
            AddMapping(UIKeyboardHidUsage.KeyboardLang9, null);
            AddMapping(UIKeyboardHidUsage.KeyboardAlternateErase, null);
            AddMapping(UIKeyboardHidUsage.KeyboardSysReqOrAttention, null);
            AddMapping(UIKeyboardHidUsage.KeyboardCancel, Key.NavigationCancel);
            AddMapping(UIKeyboardHidUsage.KeyboardClear, Key.Clear);
            AddMapping(UIKeyboardHidUsage.KeyboardPrior, Key.Prior);
            AddMapping(UIKeyboardHidUsage.KeyboardReturn, Key.Return);
            AddMapping(UIKeyboardHidUsage.KeyboardSeparator, null);
            AddMapping(UIKeyboardHidUsage.KeyboardOut, null);
            AddMapping(UIKeyboardHidUsage.KeyboardOper, null);
            AddMapping(UIKeyboardHidUsage.KeyboardClearOrAgain, null);
            AddMapping(UIKeyboardHidUsage.KeyboardCrSelOrProps, null);
            AddMapping(UIKeyboardHidUsage.KeyboardExSel, null);
            AddMapping(UIKeyboardHidUsage.KeyboardLeftControl, null);
            AddMapping(UIKeyboardHidUsage.KeyboardLeftShift, null);
            AddMapping(UIKeyboardHidUsage.KeyboardLeftAlt, null);
            AddMapping(UIKeyboardHidUsage.KeyboardLeftGui, null);
            AddMapping(UIKeyboardHidUsage.KeyboardRightControl, null);
            AddMapping(UIKeyboardHidUsage.KeyboardRightShift, null);
            AddMapping(UIKeyboardHidUsage.KeyboardRightAlt, null);
            AddMapping(UIKeyboardHidUsage.KeyboardRightGui, null);
            AddMapping(UIKeyboardHidUsage.KeyboardReturnOrEnter, null);
            AddMapping(UIKeyboardHidUsage.KeyboardDeleteOrBackspace, null);
            AddMapping(UIKeyboardHidUsage.KeyboardHyphen, null);
            AddMapping(UIKeyboardHidUsage.KeyboardOpenBracket, null);
            AddMapping(UIKeyboardHidUsage.KeyboardCloseBracket, null);
            AddMapping(UIKeyboardHidUsage.KeyboardNonUSPound, null);
            AddMapping(UIKeyboardHidUsage.KeyboardDeleteForward, null);
            AddMapping(UIKeyboardHidUsage.KeypadAsterisk, null);
            AddMapping(UIKeyboardHidUsage.KeypadHyphen, null);
            AddMapping(UIKeyboardHidUsage.KeypadEnter, null);
            AddMapping(UIKeyboardHidUsage.KeypadPeriod, null);
            AddMapping(UIKeyboardHidUsage.KeyboardNonUSBackslash, null);
            AddMapping(UIKeyboardHidUsage.KeyboardApplication, null);
            AddMapping(UIKeyboardHidUsage.KeyboardPower, null);
            AddMapping(UIKeyboardHidUsage.KeypadEqualSign, null);
            AddMapping(UIKeyboardHidUsage.KeyboardExecute, null);
            AddMapping(UIKeyboardHidUsage.KeyboardHelp, null);
            AddMapping(UIKeyboardHidUsage.KeyboardMenu, null);
            AddMapping(UIKeyboardHidUsage.KeyboardSelect, null);
            AddMapping(UIKeyboardHidUsage.KeyboardStop, null);
            AddMapping(UIKeyboardHidUsage.KeyboardAgain, null);
            AddMapping(UIKeyboardHidUsage.KeyboardUndo, null);
            AddMapping(UIKeyboardHidUsage.KeyboardCut, null);
            AddMapping(UIKeyboardHidUsage.KeyboardCopy, null);
            AddMapping(UIKeyboardHidUsage.KeyboardPaste, null);
            AddMapping(UIKeyboardHidUsage.KeyboardFind, null);
            AddMapping(UIKeyboardHidUsage.KeyboardLockingCapsLock, null);
            AddMapping(UIKeyboardHidUsage.KeyboardLockingNumLock, null);
            AddMapping(UIKeyboardHidUsage.KeyboardLockingScrollLock, null);
            AddMapping(UIKeyboardHidUsage.KeypadComma, null);
            AddMapping(UIKeyboardHidUsage.KeypadEqualSignAS400, null);
            AddMapping(UIKeyboardHidUsage.KeyboardInternational1, null);
            AddMapping(UIKeyboardHidUsage.KeyboardInternational2, null);
            AddMapping(UIKeyboardHidUsage.KeyboardInternational3, null);
            AddMapping(UIKeyboardHidUsage.KeyboardInternational4, null);
            AddMapping(UIKeyboardHidUsage.KeyboardInternational5, null);
            AddMapping(UIKeyboardHidUsage.KeyboardInternational6, null);
            AddMapping(UIKeyboardHidUsage.KeyboardInternational7, null);
            AddMapping(UIKeyboardHidUsage.KeyboardInternational8, null);
            AddMapping(UIKeyboardHidUsage.KeyboardInternational9, null);
            AddMapping(UIKeyboardHidUsage.KeyboardLang1, null);
            AddMapping(UIKeyboardHidUsage.KeyboardLang2, null);
            AddMapping(UIKeyboardHidUsage.KeyboardLang3, null);
            AddMapping(UIKeyboardHidUsage.KeyboardLang4, null);
            AddMapping(UIKeyboardHidUsage.KeyboardLang5, null);
        }

        /// <inheritdoc/>
        public override KeyStates GetKeyStatesFromSystem(Key key)
        {
            return KeyStates.None;
        }
    }
}
#endif