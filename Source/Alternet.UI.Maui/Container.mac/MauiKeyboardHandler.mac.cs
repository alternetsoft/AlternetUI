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
            AddMapping(UIKeyboardHidUsage.KeyboardGraveAccentAndTilde, Key.Tilde);
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
            AddMapping(UIKeyboardHidUsage.KeypadEqualSign, Key.Equals);
            AddMapping(UIKeyboardHidUsage.KeyboardCancel, Key.NavigationCancel);
            AddMapping(UIKeyboardHidUsage.KeyboardClear, Key.Clear);
            AddMapping(UIKeyboardHidUsage.KeyboardPrior, Key.Prior);
            AddMapping(UIKeyboardHidUsage.KeyboardReturn, Key.Return);
            AddMapping(UIKeyboardHidUsage.KeypadAsterisk, Key.Asterisk);
            AddMapping(UIKeyboardHidUsage.KeyboardSelect, Key.Select);
            AddMapping(UIKeyboardHidUsage.KeypadPeriod, Key.Period);
            AddMapping(UIKeyboardHidUsage.KeyboardStop, Key.MediaStop);
            AddMapping(UIKeyboardHidUsage.KeyboardExecute, Key.Execute);
            AddMapping(UIKeyboardHidUsage.KeyboardOpenBracket, Key.OpenBracket);
            AddMapping(UIKeyboardHidUsage.KeyboardCloseBracket, Key.CloseBracket);
            AddMapping(UIKeyboardHidUsage.KeyboardDeleteForward, Key.Delete);
            AddMapping(UIKeyboardHidUsage.KeyboardDeleteOrBackspace, Key.Backspace);
            AddMapping(UIKeyboardHidUsage.KeypadHyphen, Key.NumPadMinus);
            AddMapping(UIKeyboardHidUsage.KeyboardHyphen, Key.Minus);
            AddMapping(UIKeyboardHidUsage.KeyboardMenu, Key.Menu);

            AddMapping(UIKeyboardHidUsage.KeyboardLeftControl, Key.Control);
            AddMapping(UIKeyboardHidUsage.KeyboardLeftShift, Key.Shift);
            AddMapping(UIKeyboardHidUsage.KeyboardLeftAlt, Key.Alt);
            AddMapping(UIKeyboardHidUsage.KeyboardLeftGui, Key.Windows);

            /* ==================================== */

            AddOneWayMapping(UIKeyboardHidUsage.KeypadEnter, Key.Enter);
            AddOneWayMapping(UIKeyboardHidUsage.KeyboardReturnOrEnter, Key.Enter);
            AddOneWayMapping(UIKeyboardHidUsage.KeypadComma, Key.Comma);

            AddOneWayMapping(UIKeyboardHidUsage.KeyboardRightControl, Key.Control);
            AddOneWayMapping(UIKeyboardHidUsage.KeyboardRightShift, Key.Shift);
            AddOneWayMapping(UIKeyboardHidUsage.KeyboardRightAlt, Key.Alt);
            AddOneWayMapping(UIKeyboardHidUsage.KeyboardRightGui, Key.Windows);

            /* ==================================== */

            // An alias for the LANG1 key on Korean language keyboards.
            AddMapping(UIKeyboardHidUsage.KeyboardHangul, Key.Hangul);

            // An alias for the LANG2 key on Korean language keyboards.
            AddMapping(UIKeyboardHidUsage.KeyboardHanja, Key.Hanja);

            /* ==================================== */

            // An alias for the LANG1 key on Japanese language keyboards from Apple.
            AddMapping(UIKeyboardHidUsage.KeyboardKanaSwitch, Key.Kana);

            // An alias for the LANG2 key on Japanese language keyboards from Apple.
            AddMapping(UIKeyboardHidUsage.KeyboardAlphanumericSwitch, Key.AlphanumericSwitch);

            // An alias for the LANG3 key on Japanese language keyboards.
            AddMapping(UIKeyboardHidUsage.KeyboardKatakana, Key.Katakana);

            // An alias for the LANG4 key on Japanese language keyboards.
            AddMapping(UIKeyboardHidUsage.KeyboardHiragana, Key.Hiragana);

            // An alias for the LANG5 key on Japanese language keyboards.
            AddMapping(UIKeyboardHidUsage.KeyboardZenkakuHankakuKanji, Key.Kanji);

            /* ==================================== */

            AddMapping(UIKeyboardHidUsage.KeyboardAlternateErase, Key.AlternateErase);
            AddMapping(UIKeyboardHidUsage.KeyboardSysReqOrAttention, Key.SysReqOrAttention);
            AddMapping(UIKeyboardHidUsage.KeyboardClearOrAgain, Key.ClearOrAgain);
            AddMapping(UIKeyboardHidUsage.KeyboardCrSelOrProps, Key.CrSelOrProps);
            AddMapping(UIKeyboardHidUsage.KeyboardExSel, Key.ExSel);
            AddMapping(UIKeyboardHidUsage.KeyboardLockingCapsLock, Key.LockingCapsLock);
            AddMapping(UIKeyboardHidUsage.KeyboardLockingNumLock, Key.LockingNumLock);
            AddMapping(UIKeyboardHidUsage.KeyboardLockingScrollLock, Key.LockingScrollLock);
            AddMapping(UIKeyboardHidUsage.KeyboardUndo, Key.Undo);
            AddMapping(UIKeyboardHidUsage.KeyboardCut, Key.Cut);
            AddMapping(UIKeyboardHidUsage.KeyboardCopy, Key.Copy);
            AddMapping(UIKeyboardHidUsage.KeyboardPaste, Key.Paste);
            AddMapping(UIKeyboardHidUsage.KeyboardFind, Key.Find);
            AddMapping(UIKeyboardHidUsage.KeypadEqualSignAS400, Key.NumPadEqualSignAS400);
            AddMapping(UIKeyboardHidUsage.KeyboardHelp, Key.Help);
            AddMapping(UIKeyboardHidUsage.KeyboardPower, Key.Power);
            AddMapping(UIKeyboardHidUsage.KeyboardNonUSBackslash, Key.NonUSBackslash);

            AddMapping(UIKeyboardHidUsage.KeyboardInternational1, Key.International1);
            AddMapping(UIKeyboardHidUsage.KeyboardInternational2, Key.International2);
            AddMapping(UIKeyboardHidUsage.KeyboardInternational3, Key.International3);
            AddMapping(UIKeyboardHidUsage.KeyboardInternational4, Key.International4);
            AddMapping(UIKeyboardHidUsage.KeyboardInternational5, Key.International5);
            AddMapping(UIKeyboardHidUsage.KeyboardInternational6, Key.International6);
            AddMapping(UIKeyboardHidUsage.KeyboardInternational7, Key.International7);
            AddMapping(UIKeyboardHidUsage.KeyboardInternational8, Key.International8);
            AddMapping(UIKeyboardHidUsage.KeyboardInternational9, Key.International9);

            AddMapping(UIKeyboardHidUsage.KeyboardLang1, Key.Lang1);
            AddMapping(UIKeyboardHidUsage.KeyboardLang2, Key.Lang2);
            AddMapping(UIKeyboardHidUsage.KeyboardLang3, Key.Lang3);
            AddMapping(UIKeyboardHidUsage.KeyboardLang4, Key.Lang4);
            AddMapping(UIKeyboardHidUsage.KeyboardLang5, Key.Lang5);
            AddMapping(UIKeyboardHidUsage.KeyboardLang6, Key.Lang6);
            AddMapping(UIKeyboardHidUsage.KeyboardLang7, Key.Lang7);
            AddMapping(UIKeyboardHidUsage.KeyboardLang8, Key.Lang8);
            AddMapping(UIKeyboardHidUsage.KeyboardLang9, Key.Lang9);

            /* ==================================== */

            AddMapping(UIKeyboardHidUsage.KeyboardSeparator, Key.Separator);
            AddMapping(UIKeyboardHidUsage.KeyboardOut, Key.Out);
            AddMapping(UIKeyboardHidUsage.KeyboardOper, Key.Oper);
            AddMapping(UIKeyboardHidUsage.KeyboardNonUSPound, Key.NonUSPound);
            AddMapping(UIKeyboardHidUsage.KeyboardApplication, Key.Application);
            AddMapping(UIKeyboardHidUsage.KeyboardAgain, Key.Again);
        }

        /// <inheritdoc/>
        public override KeyStates GetKeyStatesFromSystem(Key key)
        {
            return KeyStates.None;
        }
    }
}
#endif