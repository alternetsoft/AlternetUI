using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.Controls.PlatformConfiguration;

#if IOS || MACCATALYST

using Foundation;
using UIKit;

namespace Alternet.UI
{
    /// <summary>
    /// Implements <see cref="IKeyboardHandler"/> for MAUI platform under MacOs.
    /// </summary>
    public class MauiKeyboardHandler : PlatformKeyboardHandler<UIKeyboardHidUsage>
    {
        /// <summary>
        /// Gets or sets default <see cref="IKeyboardHandler"/> implementation.
        /// </summary>
        public static MauiKeyboardHandler Default = new();

        private IKeyboardVisibilityService? visibilityService;

        static MauiKeyboardHandler()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MauiKeyboardHandler"/> class.
        /// </summary>
        public MauiKeyboardHandler()
            : base(UIKeyboardHidUsage.KeyboardRightGui, Key.Max)
        {
        }

        /// <inheritdoc/>
        public override IKeyboardVisibilityService? VisibilityService
        {
            get
            {
                return visibilityService ??= new Alternet.Maui.KeyboardVisibilityService();
            }
        }

        /// <summary>
        /// Converts event arguments from <see cref="UIPress"/> to
        /// <see cref="Alternet.UI.KeyEventArgs"/>.
        /// </summary>
        /// <param name="control">Target control.</param>
        /// <param name="keyStates">The state of the key referenced by the event.</param>
        /// <param name="press">Information about the pressed key.</param>
        /// <returns></returns>
        public virtual Alternet.UI.KeyEventArgs? ToKeyEventArgs(
            AbstractControl? control,
            UIPress press,
            KeyStates keyStates)
        {
            if (press.Key is null || control is null)
                return null;

            var key = Convert(press.Key.KeyCode);
            var modifiers = Convert(press.Key.ModifierFlags);

            Alternet.UI.KeyEventArgs result = new(
                control,
                key,
                keyStates,
                modifiers,
                0);

            return result;
        }

        /// <summary>
        /// Converts event arguments from <see cref="UIPress"/> to
        /// <see cref="Alternet.UI.KeyPressEventArgs"/>.
        /// </summary>
        /// <param name="control">Target control.</param>
        /// <param name="press">Information about the pressed key.</param>
        /// <returns></returns>
        public virtual Alternet.UI.KeyPressEventArgs[]? ToKeyPressEventArgs(
            AbstractControl? control,
            UIPress press)
        {
            if (press.Key is null || control is null)
                return null;

            var inputChars = press.Key.Characters;

            if (inputChars is null)
                return null;

            var length = inputChars.Length;

            if (length == 0)
                return null;

            Alternet.UI.KeyPressEventArgs[] result = new Alternet.UI.KeyPressEventArgs[length];

            for (int i = 0; i < length; i++)
            {
                var inputChar = inputChars[i];

                result[i] = new(control, inputChar);
            }

            return result;
        }

        /// <summary>
        /// Converts <see cref="UIKeyModifierFlags"/> to <see cref="ModifierKeys"/>.
        /// </summary>
        /// <param name="flags">Value to convert.</param>
        /// <returns></returns>
        public virtual ModifierKeys Convert(UIKeyModifierFlags flags)
        {
            ModifierKeys result = ModifierKeys.None;

            if (flags.HasFlag(UIKeyModifierFlags.Shift) || flags.HasFlag(UIKeyModifierFlags.AlphaShift))
                result |= ModifierKeys.Shift;

            if (flags.HasFlag(UIKeyModifierFlags.Control))
                result |= ModifierKeys.Windows;

            if (flags.HasFlag(UIKeyModifierFlags.Alternate))
                result |= ModifierKeys.Alt;

            if (flags.HasFlag(UIKeyModifierFlags.Command))
                result |= ModifierKeys.Control;

            return result;
        }

        /// <inheritdoc/>
        public override void RegisterKeyMappings()
        {
            Add(UIKeyboardHidUsage.KeyboardA, Key.A);
            Add(UIKeyboardHidUsage.KeyboardB, Key.B);
            Add(UIKeyboardHidUsage.KeyboardC, Key.C);
            Add(UIKeyboardHidUsage.KeyboardD, Key.D);
            Add(UIKeyboardHidUsage.KeyboardE, Key.E);
            Add(UIKeyboardHidUsage.KeyboardF, Key.F);
            Add(UIKeyboardHidUsage.KeyboardG, Key.G);
            Add(UIKeyboardHidUsage.KeyboardH, Key.H);
            Add(UIKeyboardHidUsage.KeyboardI, Key.I);
            Add(UIKeyboardHidUsage.KeyboardJ, Key.J);
            Add(UIKeyboardHidUsage.KeyboardK, Key.K);
            Add(UIKeyboardHidUsage.KeyboardL, Key.L);
            Add(UIKeyboardHidUsage.KeyboardM, Key.M);
            Add(UIKeyboardHidUsage.KeyboardN, Key.N);
            Add(UIKeyboardHidUsage.KeyboardO, Key.O);
            Add(UIKeyboardHidUsage.KeyboardP, Key.P);
            Add(UIKeyboardHidUsage.KeyboardQ, Key.Q);
            Add(UIKeyboardHidUsage.KeyboardR, Key.R);
            Add(UIKeyboardHidUsage.KeyboardS, Key.S);
            Add(UIKeyboardHidUsage.KeyboardT, Key.T);
            Add(UIKeyboardHidUsage.KeyboardU, Key.U);
            Add(UIKeyboardHidUsage.KeyboardV, Key.V);
            Add(UIKeyboardHidUsage.KeyboardW, Key.W);
            Add(UIKeyboardHidUsage.KeyboardX, Key.X);
            Add(UIKeyboardHidUsage.KeyboardY, Key.Y);
            Add(UIKeyboardHidUsage.KeyboardZ, Key.Z);
            Add(UIKeyboardHidUsage.Keyboard1, Key.D1);
            Add(UIKeyboardHidUsage.Keyboard2, Key.D2);
            Add(UIKeyboardHidUsage.Keyboard3, Key.D3);
            Add(UIKeyboardHidUsage.Keyboard4, Key.D4);
            Add(UIKeyboardHidUsage.Keyboard5, Key.D5);
            Add(UIKeyboardHidUsage.Keyboard6, Key.D6);
            Add(UIKeyboardHidUsage.Keyboard7, Key.D7);
            Add(UIKeyboardHidUsage.Keyboard8, Key.D8);
            Add(UIKeyboardHidUsage.Keyboard9, Key.D9);
            Add(UIKeyboardHidUsage.Keyboard0, Key.D0);
            Add(UIKeyboardHidUsage.KeyboardEscape, Key.Escape);
            Add(UIKeyboardHidUsage.KeyboardTab, Key.Tab);
            Add(UIKeyboardHidUsage.KeyboardSpacebar, Key.Space);
            Add(UIKeyboardHidUsage.KeyboardEqualSign, Key.Equals);
            Add(UIKeyboardHidUsage.KeyboardBackslash, Key.Backslash);
            Add(UIKeyboardHidUsage.KeyboardSemicolon, Key.Semicolon);
            Add(UIKeyboardHidUsage.KeyboardQuote, Key.Quote);
            Add(UIKeyboardHidUsage.KeyboardGraveAccentAndTilde, Key.Tilde);
            Add(UIKeyboardHidUsage.KeyboardComma, Key.Comma);
            Add(UIKeyboardHidUsage.KeyboardPeriod, Key.Period);
            Add(UIKeyboardHidUsage.KeyboardSlash, Key.Slash);
            Add(UIKeyboardHidUsage.KeyboardCapsLock, Key.CapsLock);
            Add(UIKeyboardHidUsage.KeyboardF1, Key.F1);
            Add(UIKeyboardHidUsage.KeyboardF2, Key.F2);
            Add(UIKeyboardHidUsage.KeyboardF3, Key.F3);
            Add(UIKeyboardHidUsage.KeyboardF4, Key.F4);
            Add(UIKeyboardHidUsage.KeyboardF5, Key.F5);
            Add(UIKeyboardHidUsage.KeyboardF6, Key.F6);
            Add(UIKeyboardHidUsage.KeyboardF7, Key.F7);
            Add(UIKeyboardHidUsage.KeyboardF8, Key.F8);
            Add(UIKeyboardHidUsage.KeyboardF9, Key.F9);
            Add(UIKeyboardHidUsage.KeyboardF10, Key.F10);
            Add(UIKeyboardHidUsage.KeyboardF11, Key.F11);
            Add(UIKeyboardHidUsage.KeyboardF12, Key.F12);
            Add(UIKeyboardHidUsage.KeyboardPrintScreen, Key.PrintScreen);
            Add(UIKeyboardHidUsage.KeyboardScrollLock, Key.ScrollLock);
            Add(UIKeyboardHidUsage.KeyboardPause, Key.Pause);
            Add(UIKeyboardHidUsage.KeyboardInsert, Key.Insert);
            Add(UIKeyboardHidUsage.KeyboardHome, Key.Home);
            Add(UIKeyboardHidUsage.KeyboardPageUp, Key.PageUp);
            Add(UIKeyboardHidUsage.KeyboardEnd, Key.End);
            Add(UIKeyboardHidUsage.KeyboardPageDown, Key.PageDown);
            Add(UIKeyboardHidUsage.KeyboardRightArrow, Key.RightArrow);
            Add(UIKeyboardHidUsage.KeyboardLeftArrow, Key.LeftArrow);
            Add(UIKeyboardHidUsage.KeyboardDownArrow, Key.DownArrow);
            Add(UIKeyboardHidUsage.KeyboardUpArrow, Key.UpArrow);
            Add(UIKeyboardHidUsage.KeypadNumLock, Key.NumLock);
            Add(UIKeyboardHidUsage.KeypadSlash, Key.NumPadSlash);
            Add(UIKeyboardHidUsage.KeypadPlus, Key.NumPadPlus);
            Add(UIKeyboardHidUsage.Keypad1, Key.NumPad1);
            Add(UIKeyboardHidUsage.Keypad2, Key.NumPad2);
            Add(UIKeyboardHidUsage.Keypad3, Key.NumPad3);
            Add(UIKeyboardHidUsage.Keypad4, Key.NumPad4);
            Add(UIKeyboardHidUsage.Keypad5, Key.NumPad5);
            Add(UIKeyboardHidUsage.Keypad6, Key.NumPad6);
            Add(UIKeyboardHidUsage.Keypad7, Key.NumPad7);
            Add(UIKeyboardHidUsage.Keypad8, Key.NumPad8);
            Add(UIKeyboardHidUsage.Keypad9, Key.NumPad9);
            Add(UIKeyboardHidUsage.Keypad0, Key.NumPad0);
            Add(UIKeyboardHidUsage.KeyboardF13, Key.F13);
            Add(UIKeyboardHidUsage.KeyboardF14, Key.F14);
            Add(UIKeyboardHidUsage.KeyboardF15, Key.F15);
            Add(UIKeyboardHidUsage.KeyboardF16, Key.F16);
            Add(UIKeyboardHidUsage.KeyboardF17, Key.F17);
            Add(UIKeyboardHidUsage.KeyboardF18, Key.F18);
            Add(UIKeyboardHidUsage.KeyboardF19, Key.F19);
            Add(UIKeyboardHidUsage.KeyboardF20, Key.F20);
            Add(UIKeyboardHidUsage.KeyboardF21, Key.F21);
            Add(UIKeyboardHidUsage.KeyboardF22, Key.F22);
            Add(UIKeyboardHidUsage.KeyboardF23, Key.F23);
            Add(UIKeyboardHidUsage.KeyboardF24, Key.F24);
            Add(UIKeyboardHidUsage.KeyboardMute, Key.VolumeMute);
            Add(UIKeyboardHidUsage.KeyboardVolumeUp, Key.VolumeUp);
            Add(UIKeyboardHidUsage.KeyboardVolumeDown, Key.VolumeDown);
            Add(UIKeyboardHidUsage.KeypadEqualSign, Key.Equals);
            Add(UIKeyboardHidUsage.KeyboardCancel, Key.NavigationCancel);
            Add(UIKeyboardHidUsage.KeyboardClear, Key.Clear);
            Add(UIKeyboardHidUsage.KeyboardPrior, Key.Prior);
            Add(UIKeyboardHidUsage.KeyboardReturn, Key.Return);
            Add(UIKeyboardHidUsage.KeypadAsterisk, Key.Asterisk);
            Add(UIKeyboardHidUsage.KeyboardSelect, Key.Select);
            Add(UIKeyboardHidUsage.KeypadPeriod, Key.Period);
            Add(UIKeyboardHidUsage.KeyboardStop, Key.MediaStop);
            Add(UIKeyboardHidUsage.KeyboardExecute, Key.Execute);
            Add(UIKeyboardHidUsage.KeyboardOpenBracket, Key.OpenBracket);
            Add(UIKeyboardHidUsage.KeyboardCloseBracket, Key.CloseBracket);
            Add(UIKeyboardHidUsage.KeyboardDeleteForward, Key.Delete);
            Add(UIKeyboardHidUsage.KeyboardDeleteOrBackspace, Key.Backspace);
            Add(UIKeyboardHidUsage.KeypadHyphen, Key.NumPadMinus);
            Add(UIKeyboardHidUsage.KeyboardHyphen, Key.Minus);
            Add(UIKeyboardHidUsage.KeyboardMenu, Key.Menu);

            Add(UIKeyboardHidUsage.KeyboardLeftControl, Key.Control);
            Add(UIKeyboardHidUsage.KeyboardLeftShift, Key.Shift);
            Add(UIKeyboardHidUsage.KeyboardLeftAlt, Key.Alt);
            Add(UIKeyboardHidUsage.KeyboardLeftGui, Key.Windows);

            /* ==================================== */

            AddOneWay(UIKeyboardHidUsage.KeypadEnter, Key.Enter);
            AddOneWay(UIKeyboardHidUsage.KeyboardReturnOrEnter, Key.Enter);
            AddOneWay(UIKeyboardHidUsage.KeypadComma, Key.Comma);

            AddOneWay(UIKeyboardHidUsage.KeyboardRightControl, Key.Control);
            AddOneWay(UIKeyboardHidUsage.KeyboardRightShift, Key.Shift);
            AddOneWay(UIKeyboardHidUsage.KeyboardRightAlt, Key.Alt);
            AddOneWay(UIKeyboardHidUsage.KeyboardRightGui, Key.Windows);

            /* ==================================== */

            // An alias for the LANG1 key on Korean language keyboards.
            Add(UIKeyboardHidUsage.KeyboardHangul, Key.Hangul);

            // An alias for the LANG2 key on Korean language keyboards.
            Add(UIKeyboardHidUsage.KeyboardHanja, Key.Hanja);

            /* ==================================== */

            // An alias for the LANG1 key on Japanese language keyboards from Apple.
            Add(UIKeyboardHidUsage.KeyboardKanaSwitch, Key.Kana);

            // An alias for the LANG2 key on Japanese language keyboards from Apple.
            Add(UIKeyboardHidUsage.KeyboardAlphanumericSwitch, Key.AlphanumericSwitch);

            // An alias for the LANG3 key on Japanese language keyboards.
            Add(UIKeyboardHidUsage.KeyboardKatakana, Key.Katakana);

            // An alias for the LANG4 key on Japanese language keyboards.
            Add(UIKeyboardHidUsage.KeyboardHiragana, Key.Hiragana);

            // An alias for the LANG5 key on Japanese language keyboards.
            Add(UIKeyboardHidUsage.KeyboardZenkakuHankakuKanji, Key.Kanji);

            /* ==================================== */

            Add(UIKeyboardHidUsage.KeyboardAlternateErase, Key.AlternateErase);
            Add(UIKeyboardHidUsage.KeyboardSysReqOrAttention, Key.SysReqOrAttention);
            Add(UIKeyboardHidUsage.KeyboardClearOrAgain, Key.ClearOrAgain);
            Add(UIKeyboardHidUsage.KeyboardCrSelOrProps, Key.CrSelOrProps);
            Add(UIKeyboardHidUsage.KeyboardExSel, Key.ExSel);
            Add(UIKeyboardHidUsage.KeyboardLockingCapsLock, Key.LockingCapsLock);
            Add(UIKeyboardHidUsage.KeyboardLockingNumLock, Key.LockingNumLock);
            Add(UIKeyboardHidUsage.KeyboardLockingScrollLock, Key.LockingScrollLock);
            Add(UIKeyboardHidUsage.KeyboardUndo, Key.Undo);
            Add(UIKeyboardHidUsage.KeyboardCut, Key.Cut);
            Add(UIKeyboardHidUsage.KeyboardCopy, Key.Copy);
            Add(UIKeyboardHidUsage.KeyboardPaste, Key.Paste);
            Add(UIKeyboardHidUsage.KeyboardFind, Key.Find);
            Add(UIKeyboardHidUsage.KeypadEqualSignAS400, Key.NumPadEqualSignAS400);
            Add(UIKeyboardHidUsage.KeyboardHelp, Key.Help);
            Add(UIKeyboardHidUsage.KeyboardPower, Key.Power);
            Add(UIKeyboardHidUsage.KeyboardNonUSBackslash, Key.NonUSBackslash);

            Add(UIKeyboardHidUsage.KeyboardInternational1, Key.International1);
            Add(UIKeyboardHidUsage.KeyboardInternational2, Key.International2);
            Add(UIKeyboardHidUsage.KeyboardInternational3, Key.International3);
            Add(UIKeyboardHidUsage.KeyboardInternational4, Key.International4);
            Add(UIKeyboardHidUsage.KeyboardInternational5, Key.International5);
            Add(UIKeyboardHidUsage.KeyboardInternational6, Key.International6);
            Add(UIKeyboardHidUsage.KeyboardInternational7, Key.International7);
            Add(UIKeyboardHidUsage.KeyboardInternational8, Key.International8);
            Add(UIKeyboardHidUsage.KeyboardInternational9, Key.International9);

            Add(UIKeyboardHidUsage.KeyboardLang1, Key.Lang1);
            Add(UIKeyboardHidUsage.KeyboardLang2, Key.Lang2);
            Add(UIKeyboardHidUsage.KeyboardLang3, Key.Lang3);
            Add(UIKeyboardHidUsage.KeyboardLang4, Key.Lang4);
            Add(UIKeyboardHidUsage.KeyboardLang5, Key.Lang5);
            Add(UIKeyboardHidUsage.KeyboardLang6, Key.Lang6);
            Add(UIKeyboardHidUsage.KeyboardLang7, Key.Lang7);
            Add(UIKeyboardHidUsage.KeyboardLang8, Key.Lang8);
            Add(UIKeyboardHidUsage.KeyboardLang9, Key.Lang9);

            /* ==================================== */

            Add(UIKeyboardHidUsage.KeyboardSeparator, Key.Separator);
            Add(UIKeyboardHidUsage.KeyboardOut, Key.Out);
            Add(UIKeyboardHidUsage.KeyboardOper, Key.Oper);
            Add(UIKeyboardHidUsage.KeyboardNonUSPound, Key.NonUSPound);
            Add(UIKeyboardHidUsage.KeyboardApplication, Key.Application);
            Add(UIKeyboardHidUsage.KeyboardAgain, Key.Again);
        }

        /// <inheritdoc/>
        public override KeyStates GetKeyStatesFromSystem(Key key)
        {
            return PlessKeyboard.GetKeyStatesFromMemory(key);
        }

        /// <inheritdoc/>
        public override bool HideKeyboard(AbstractControl? control)
        {
            var platformView = ControlView.GetPlatformView(control);
            if (platformView is null)
                return false;

            var result = platformView.ResignFirstResponder();

            return result;
        }

        /// <inheritdoc/>
        public override bool IsSoftKeyboardShowing(AbstractControl? control)
        {
            var platformView = ControlView.GetPlatformView(control);
            if (platformView is null)
                return false;

            var result = platformView.IsFirstResponder;
            return result;
        }

        /// <inheritdoc/>
        public override bool ShowKeyboard(AbstractControl? control)
        {
            var platformView = ControlView.GetPlatformView(control);
            if (platformView is null)
                return false;

            var result = platformView.BecomeFirstResponder();
            return result;
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            SafeDispose(ref visibilityService);
            base.DisposeManaged();
        }
    }
}
#endif