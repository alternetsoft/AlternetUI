using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements <see cref="IKeyboardHandler"/> interface provider for WxWidgets platform.
    /// </summary>
    public class WxKeyboardHandler : DisposableObject, IKeyboardHandler
    {
        private static AbstractTwoWayEnumMapping<WxWidgetsKeyCode, Key>? keyAndWxMapping;

        /// <inheritdoc/>
        public virtual bool IsValidKey(Key key)
        {
            return (int)key >= (int)Key.None && (int)key <= (int)Key.MaxWxWidgets;
        }

        /// <summary>
        /// Gets or sets <see cref="WxWidgetsKeyCode"/> to/from <see cref="Key"/> enum mapping.
        /// </summary>
        public static AbstractTwoWayEnumMapping<WxWidgetsKeyCode, Key> KeyAndWxMapping
        {
            get
            {
                if (keyAndWxMapping is null)
                {
                    keyAndWxMapping =
                        new TwoWayEnumMapping<WxWidgetsKeyCode, Key>(WxWidgetsKeyCode.Max, Key.Max);
                    RegisterKeyMappings();
                }

                return keyAndWxMapping;
            }

            set
            {
                keyAndWxMapping = value;
            }
        }

        /// <inheritdoc/>
        public bool? KeyboardPresent => null;

        /// <inheritdoc/>
        public IKeyboardVisibilityService? VisibilityService { get; }

        /// <summary>
        /// Registers default <see cref="WxWidgetsKeyCode"/> to/from <see cref="Key"/> enum mappings.
        /// </summary>
        public static void RegisterKeyMappings()
        {
            Fn(WxWidgetsKeyCode.Back, Key.Backspace);
            Fn(WxWidgetsKeyCode.Tab, Key.Tab);
            Fn(WxWidgetsKeyCode.Return, Key.Enter);
            Fn(WxWidgetsKeyCode.Pause, Key.Pause);
            Fn(WxWidgetsKeyCode.Capital, Key.CapsLock);
            Fn(WxWidgetsKeyCode.Escape, Key.Escape);
            Fn(WxWidgetsKeyCode.Space, Key.Space);
            Fn(WxWidgetsKeyCode.PageUp, Key.PageUp);
            Fn(WxWidgetsKeyCode.PageDown, Key.PageDown);
            Fn(WxWidgetsKeyCode.End, Key.End);
            Fn(WxWidgetsKeyCode.Home, Key.Home);
            Fn(WxWidgetsKeyCode.Left, Key.LeftArrow);
            Fn(WxWidgetsKeyCode.Up, Key.UpArrow);
            Fn(WxWidgetsKeyCode.Right, Key.RightArrow);
            Fn(WxWidgetsKeyCode.Down, Key.DownArrow);
            Fn(WxWidgetsKeyCode.Print, Key.PrintScreen);
            Fn(WxWidgetsKeyCode.Insert, Key.Insert);
            Fn(WxWidgetsKeyCode.Delete, Key.Delete);
            Fn(WxWidgetsKeyCode.D0, Key.D0);
            Fn(WxWidgetsKeyCode.D1, Key.D1);
            Fn(WxWidgetsKeyCode.D2, Key.D2);
            Fn(WxWidgetsKeyCode.D3, Key.D3);
            Fn(WxWidgetsKeyCode.D4, Key.D4);
            Fn(WxWidgetsKeyCode.D5, Key.D5);
            Fn(WxWidgetsKeyCode.D6, Key.D6);
            Fn(WxWidgetsKeyCode.D7, Key.D7);
            Fn(WxWidgetsKeyCode.D8, Key.D8);
            Fn(WxWidgetsKeyCode.D9, Key.D9);
            Fn(WxWidgetsKeyCode.A, Key.A);
            Fn(WxWidgetsKeyCode.B, Key.B);
            Fn(WxWidgetsKeyCode.C, Key.C);
            Fn(WxWidgetsKeyCode.D, Key.D);
            Fn(WxWidgetsKeyCode.E, Key.E);
            Fn(WxWidgetsKeyCode.F, Key.F);
            Fn(WxWidgetsKeyCode.G, Key.G);
            Fn(WxWidgetsKeyCode.H, Key.H);
            Fn(WxWidgetsKeyCode.I, Key.I);
            Fn(WxWidgetsKeyCode.J, Key.J);
            Fn(WxWidgetsKeyCode.K, Key.K);
            Fn(WxWidgetsKeyCode.L, Key.L);
            Fn(WxWidgetsKeyCode.M, Key.M);
            Fn(WxWidgetsKeyCode.N, Key.N);
            Fn(WxWidgetsKeyCode.O, Key.O);
            Fn(WxWidgetsKeyCode.P, Key.P);
            Fn(WxWidgetsKeyCode.Q, Key.Q);
            Fn(WxWidgetsKeyCode.R, Key.R);
            Fn(WxWidgetsKeyCode.S, Key.S);
            Fn(WxWidgetsKeyCode.T, Key.T);
            Fn(WxWidgetsKeyCode.U, Key.U);
            Fn(WxWidgetsKeyCode.V, Key.V);
            Fn(WxWidgetsKeyCode.W, Key.W);
            Fn(WxWidgetsKeyCode.X, Key.X);
            Fn(WxWidgetsKeyCode.Y, Key.Y);
            Fn(WxWidgetsKeyCode.Z, Key.Z);

            Fn(WxWidgetsKeyCode.LowerA, Key.A);
            Fn(WxWidgetsKeyCode.LowerB, Key.B);
            Fn(WxWidgetsKeyCode.LowerC, Key.C);
            Fn(WxWidgetsKeyCode.LowerD, Key.D);
            Fn(WxWidgetsKeyCode.LowerE, Key.E);
            Fn(WxWidgetsKeyCode.LowerF, Key.F);
            Fn(WxWidgetsKeyCode.LowerG, Key.G);
            Fn(WxWidgetsKeyCode.LowerH, Key.H);
            Fn(WxWidgetsKeyCode.LowerI, Key.I);
            Fn(WxWidgetsKeyCode.LowerJ, Key.J);
            Fn(WxWidgetsKeyCode.LowerK, Key.K);
            Fn(WxWidgetsKeyCode.LowerL, Key.L);
            Fn(WxWidgetsKeyCode.LowerM, Key.M);
            Fn(WxWidgetsKeyCode.LowerN, Key.N);
            Fn(WxWidgetsKeyCode.LowerO, Key.O);
            Fn(WxWidgetsKeyCode.LowerP, Key.P);
            Fn(WxWidgetsKeyCode.LowerQ, Key.Q);
            Fn(WxWidgetsKeyCode.LowerR, Key.R);
            Fn(WxWidgetsKeyCode.LowerS, Key.S);
            Fn(WxWidgetsKeyCode.LowerT, Key.T);
            Fn(WxWidgetsKeyCode.LowerU, Key.U);
            Fn(WxWidgetsKeyCode.LowerV, Key.V);
            Fn(WxWidgetsKeyCode.LowerW, Key.W);
            Fn(WxWidgetsKeyCode.LowerX, Key.X);
            Fn(WxWidgetsKeyCode.LowerY, Key.Y);
            Fn(WxWidgetsKeyCode.LowerZ, Key.Z);

            Fn(WxWidgetsKeyCode.Numpad0, Key.NumPad0);
            Fn(WxWidgetsKeyCode.Numpad1, Key.NumPad1);
            Fn(WxWidgetsKeyCode.Numpad2, Key.NumPad2);
            Fn(WxWidgetsKeyCode.Numpad3, Key.NumPad3);
            Fn(WxWidgetsKeyCode.Numpad4, Key.NumPad4);
            Fn(WxWidgetsKeyCode.Numpad5, Key.NumPad5);
            Fn(WxWidgetsKeyCode.Numpad6, Key.NumPad6);
            Fn(WxWidgetsKeyCode.Numpad7, Key.NumPad7);
            Fn(WxWidgetsKeyCode.Numpad8, Key.NumPad8);
            Fn(WxWidgetsKeyCode.Numpad9, Key.NumPad9);
            Fn(WxWidgetsKeyCode.NumpadMultiply, Key.NumPadStar);
            Fn(WxWidgetsKeyCode.NumpadAdd, Key.NumPadPlus);
            Fn(WxWidgetsKeyCode.NumpadSubtract, Key.NumPadMinus);
            Fn(WxWidgetsKeyCode.NumpadDecimal, Key.NumPadDot);
            Fn(WxWidgetsKeyCode.NumpadDivide, Key.NumPadSlash);

            Fn(WxWidgetsKeyCode.F1, Key.F1);
            Fn(WxWidgetsKeyCode.F2, Key.F2);
            Fn(WxWidgetsKeyCode.F3, Key.F3);
            Fn(WxWidgetsKeyCode.F4, Key.F4);
            Fn(WxWidgetsKeyCode.F5, Key.F5);
            Fn(WxWidgetsKeyCode.F6, Key.F6);
            Fn(WxWidgetsKeyCode.F7, Key.F7);
            Fn(WxWidgetsKeyCode.F8, Key.F8);
            Fn(WxWidgetsKeyCode.F9, Key.F9);
            Fn(WxWidgetsKeyCode.F10, Key.F10);
            Fn(WxWidgetsKeyCode.F11, Key.F11);
            Fn(WxWidgetsKeyCode.F12, Key.F12);
            Fn(WxWidgetsKeyCode.F13, Key.F13);
            Fn(WxWidgetsKeyCode.F14, Key.F14);
            Fn(WxWidgetsKeyCode.F15, Key.F15);
            Fn(WxWidgetsKeyCode.F16, Key.F16);
            Fn(WxWidgetsKeyCode.F17, Key.F17);
            Fn(WxWidgetsKeyCode.F18, Key.F18);
            Fn(WxWidgetsKeyCode.F19, Key.F19);
            Fn(WxWidgetsKeyCode.F20, Key.F20);
            Fn(WxWidgetsKeyCode.F21, Key.F21);
            Fn(WxWidgetsKeyCode.F22, Key.F22);
            Fn(WxWidgetsKeyCode.F23, Key.F23);
            Fn(WxWidgetsKeyCode.F24, Key.F24);
            Fn(WxWidgetsKeyCode.NumLock, Key.NumLock);
            Fn(WxWidgetsKeyCode.Scroll, Key.ScrollLock);
            Fn(WxWidgetsKeyCode.BrowserBack, Key.BrowserBack);
            Fn(WxWidgetsKeyCode.BrowserForward, Key.BrowserForward);
            Fn(WxWidgetsKeyCode.BrowserRefresh, Key.BrowserRefresh);
            Fn(WxWidgetsKeyCode.BrowserStop, Key.BrowserStop);
            Fn(WxWidgetsKeyCode.BrowserSearch, Key.BrowserSearch);
            Fn(WxWidgetsKeyCode.BrowserFavorites, Key.BrowserFavorites);
            Fn(WxWidgetsKeyCode.BrowserHome, Key.BrowserHome);
            Fn(WxWidgetsKeyCode.VolumeMute, Key.VolumeMute);
            Fn(WxWidgetsKeyCode.VolumeDown, Key.VolumeDown);
            Fn(WxWidgetsKeyCode.VolumeUp, Key.VolumeUp);
            Fn(WxWidgetsKeyCode.MediaNextTrack, Key.MediaNextTrack);
            Fn(WxWidgetsKeyCode.MediaPrevTrack, Key.MediaPreviousTrack);
            Fn(WxWidgetsKeyCode.MediaStop, Key.MediaStop);
            Fn(WxWidgetsKeyCode.MediaPlayPause, Key.MediaPlayPause);
            Fn(WxWidgetsKeyCode.LaunchMail, Key.LaunchMail);
            Fn(WxWidgetsKeyCode.Select, Key.SelectMedia);
            Fn(WxWidgetsKeyCode.LaunchApp1, Key.LaunchApplication1);
            Fn(WxWidgetsKeyCode.LaunchApp2, Key.LaunchApplication2);
            Fn(WxWidgetsKeyCode.Semicolon, Key.Semicolon);
            Fn(WxWidgetsKeyCode.EqualsSign, Key.Equals);
            Fn(WxWidgetsKeyCode.Comma, Key.Comma);
            Fn(WxWidgetsKeyCode.MinusSign, Key.Minus);
            Fn(WxWidgetsKeyCode.Dot, Key.Period);
            Fn(WxWidgetsKeyCode.Slash, Key.Slash);
            Fn(WxWidgetsKeyCode.OpeningSquareBracket, Key.OpenBracket);
            Fn(WxWidgetsKeyCode.ClosingSquareBracket, Key.CloseBracket);
            Fn(WxWidgetsKeyCode.SingleQuote, Key.Quote);
            Fn(WxWidgetsKeyCode.Backslash, Key.Backslash);
            Fn(WxWidgetsKeyCode.Clear, Key.Clear);
            Fn(WxWidgetsKeyCode.GraveAccent, Key.Backtick);

            Fn(WxWidgetsKeyCode.Shift, Key.Shift);
            Fn(WxWidgetsKeyCode.Control, Key.Control);
            Fn(WxWidgetsKeyCode.Alt, Key.Alt);

            Fn(WxWidgetsKeyCode.Control, Key.MacCommand);
            Fn(WxWidgetsKeyCode.Alt, Key.MacOption);

            if(App.IsMacOS)
                Fn(WxWidgetsKeyCode.RawControl, Key.MacControl);
            else
                Fn(WxWidgetsKeyCode.Control, Key.MacControl);

            Fn(WxWidgetsKeyCode.WindowsRight, Key.Windows);
            Fn(WxWidgetsKeyCode.WindowsLeft, Key.Windows);
            Fn(WxWidgetsKeyCode.WindowsMenu, Key.Menu);

            Fn(WxWidgetsKeyCode.ExclamationMark, Key.ExclamationMark);
            Fn(WxWidgetsKeyCode.DoubleQuotes, Key.QuotationMark);
            Fn(WxWidgetsKeyCode.NumberSign, Key.NumberSign);
            Fn(WxWidgetsKeyCode.DollarSign, Key.DollarSign);
            Fn(WxWidgetsKeyCode.PercentSign, Key.PercentSign);
            Fn(WxWidgetsKeyCode.Ampersand, Key.Ampersand);
            Fn(WxWidgetsKeyCode.OpeningRoundBracket, Key.LeftParenthesis);
            Fn(WxWidgetsKeyCode.ClosingRoundBracket, Key.RightParenthesis);
            Fn(WxWidgetsKeyCode.Asterisk, Key.Asterisk);
            Fn(WxWidgetsKeyCode.PlusSign, Key.PlusSign);
            Fn(WxWidgetsKeyCode.Colon, Key.Colon);
            Fn(WxWidgetsKeyCode.LessThanSign, Key.LessThanSign);
            Fn(WxWidgetsKeyCode.GreaterThanSign, Key.GreaterThanSign);
            Fn(WxWidgetsKeyCode.QuestionMark, Key.QuestionMark);
            Fn(WxWidgetsKeyCode.AtSign, Key.CommercialAt);
            Fn(WxWidgetsKeyCode.CircumflexAccent, Key.CircumflexAccent);
            Fn(WxWidgetsKeyCode.Underscore, Key.LowLine);
            Fn(WxWidgetsKeyCode.OpeningCurlyBracket, Key.LeftCurlyBracket);
            Fn(WxWidgetsKeyCode.VerticalBar, Key.VerticalLine);
            Fn(WxWidgetsKeyCode.ClosingCurlyBracket, Key.RightCurlyBracket);
            Fn(WxWidgetsKeyCode.Tilde, Key.Tilde);

            void Fn(WxWidgetsKeyCode keys, Key key)
            {
                KeyAndWxMapping.Add(keys, key);
            }
        }

        /// <inheritdoc/>
        public virtual KeyStates GetKeyStatesFromSystem(Key key)
        {
            return Native.Keyboard.GetKeyState(key);
        }

        /// <inheritdoc/>
        public virtual bool HideKeyboard(AbstractControl? control)
        {
            return false;
        }

        /// <inheritdoc/>
        public virtual bool IsSoftKeyboardShowing(AbstractControl? control)
        {
            return false;
        }

        /// <inheritdoc/>
        public virtual bool ShowKeyboard(AbstractControl? control)
        {
            return false;
        }
    }
}
