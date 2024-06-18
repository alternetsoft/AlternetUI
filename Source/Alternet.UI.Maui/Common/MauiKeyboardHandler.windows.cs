using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.UI.Xaml;

using Windows.UI.ViewManagement;
using Windows.UI.ViewManagement.Core;

namespace Alternet.UI
{
    public class MauiKeyboardHandler : DisposableObject, IKeyboardHandler
    {
        public static readonly EnumMapping<Windows.System.VirtualKey, Alternet.UI.Key>
            VirtualKeyToAlternet = new();

        static MauiKeyboardHandler()
        {
            RegisterKeyMappings();
        }

        public static Alternet.UI.Key Convert(Windows.System.VirtualKey key)
        {
            return VirtualKeyToAlternet.Convert(key);
        }

        public static void RegisterKeyMapping(Windows.System.VirtualKey windowsKey, Alternet.UI.Key key)
        {
            VirtualKeyToAlternet.Add(windowsKey, key);
        }

        public static void RegisterKeyMappings()
        {
#pragma warning disable
            RegisterKeyMapping(Windows.System.VirtualKey.None, Alternet.UI.Key.None);

            RegisterKeyMapping(Windows.System.VirtualKey.Back, Alternet.UI.Key.Back);
            RegisterKeyMapping(Windows.System.VirtualKey.Tab, Alternet.UI.Key.Tab);
            RegisterKeyMapping(Windows.System.VirtualKey.Clear, Alternet.UI.Key.Clear);
            RegisterKeyMapping(Windows.System.VirtualKey.Enter, Alternet.UI.Key.Enter);
            RegisterKeyMapping(Windows.System.VirtualKey.Shift, Alternet.UI.Key.Shift);
            RegisterKeyMapping(Windows.System.VirtualKey.Control, Alternet.UI.Key.Control);
            RegisterKeyMapping(Windows.System.VirtualKey.Menu, Alternet.UI.Key.Menu);
            RegisterKeyMapping(Windows.System.VirtualKey.Pause, Alternet.UI.Key.Pause);
            RegisterKeyMapping(Windows.System.VirtualKey.CapitalLock, Alternet.UI.Key.CapsLock);

            RegisterKeyMapping(Windows.System.VirtualKey.Space, Alternet.UI.Key.Space);
            RegisterKeyMapping(Windows.System.VirtualKey.PageUp, Alternet.UI.Key.PageUp);
            RegisterKeyMapping(Windows.System.VirtualKey.PageDown, Alternet.UI.Key.PageDown);
            RegisterKeyMapping(Windows.System.VirtualKey.End, Alternet.UI.Key.End);
            RegisterKeyMapping(Windows.System.VirtualKey.Home, Alternet.UI.Key.Home);
            RegisterKeyMapping(Windows.System.VirtualKey.Left, Alternet.UI.Key.Left);
            RegisterKeyMapping(Windows.System.VirtualKey.Up, Alternet.UI.Key.Up);
            RegisterKeyMapping(Windows.System.VirtualKey.Right, Alternet.UI.Key.Right);
            RegisterKeyMapping(Windows.System.VirtualKey.Down, Alternet.UI.Key.Down);

            RegisterKeyMapping(Windows.System.VirtualKey.Insert, Alternet.UI.Key.Insert);
            RegisterKeyMapping(Windows.System.VirtualKey.Delete, Alternet.UI.Key.Delete);

            RegisterKeyMapping(Windows.System.VirtualKey.Number0, Alternet.UI.Key.D0);
            RegisterKeyMapping(Windows.System.VirtualKey.Number1, Alternet.UI.Key.D1);
            RegisterKeyMapping(Windows.System.VirtualKey.Number2, Alternet.UI.Key.D2);
            RegisterKeyMapping(Windows.System.VirtualKey.Number3, Alternet.UI.Key.D3);
            RegisterKeyMapping(Windows.System.VirtualKey.Number4, Alternet.UI.Key.D4);
            RegisterKeyMapping(Windows.System.VirtualKey.Number5, Alternet.UI.Key.D5);
            RegisterKeyMapping(Windows.System.VirtualKey.Number6, Alternet.UI.Key.D6);
            RegisterKeyMapping(Windows.System.VirtualKey.Number7, Alternet.UI.Key.D7);
            RegisterKeyMapping(Windows.System.VirtualKey.Number8, Alternet.UI.Key.D8);
            RegisterKeyMapping(Windows.System.VirtualKey.Number9, Alternet.UI.Key.D9);
            RegisterKeyMapping(Windows.System.VirtualKey.A, Alternet.UI.Key.A);
            RegisterKeyMapping(Windows.System.VirtualKey.B, Alternet.UI.Key.B);
            RegisterKeyMapping(Windows.System.VirtualKey.C, Alternet.UI.Key.C);
            RegisterKeyMapping(Windows.System.VirtualKey.D, Alternet.UI.Key.D);
            RegisterKeyMapping(Windows.System.VirtualKey.E, Alternet.UI.Key.E);
            RegisterKeyMapping(Windows.System.VirtualKey.F, Alternet.UI.Key.F);
            RegisterKeyMapping(Windows.System.VirtualKey.G, Alternet.UI.Key.G);
            RegisterKeyMapping(Windows.System.VirtualKey.H, Alternet.UI.Key.H);
            RegisterKeyMapping(Windows.System.VirtualKey.I, Alternet.UI.Key.I);
            RegisterKeyMapping(Windows.System.VirtualKey.J, Alternet.UI.Key.J);
            RegisterKeyMapping(Windows.System.VirtualKey.K, Alternet.UI.Key.K);
            RegisterKeyMapping(Windows.System.VirtualKey.L, Alternet.UI.Key.L);
            RegisterKeyMapping(Windows.System.VirtualKey.M, Alternet.UI.Key.M);
            RegisterKeyMapping(Windows.System.VirtualKey.N, Alternet.UI.Key.N);
            RegisterKeyMapping(Windows.System.VirtualKey.O, Alternet.UI.Key.O);
            RegisterKeyMapping(Windows.System.VirtualKey.P, Alternet.UI.Key.P);
            RegisterKeyMapping(Windows.System.VirtualKey.Q, Alternet.UI.Key.Q);
            RegisterKeyMapping(Windows.System.VirtualKey.R, Alternet.UI.Key.R);
            RegisterKeyMapping(Windows.System.VirtualKey.S, Alternet.UI.Key.S);
            RegisterKeyMapping(Windows.System.VirtualKey.T, Alternet.UI.Key.T);
            RegisterKeyMapping(Windows.System.VirtualKey.U, Alternet.UI.Key.U);
            RegisterKeyMapping(Windows.System.VirtualKey.V, Alternet.UI.Key.V);
            RegisterKeyMapping(Windows.System.VirtualKey.W, Alternet.UI.Key.W);
            RegisterKeyMapping(Windows.System.VirtualKey.X, Alternet.UI.Key.X);
            RegisterKeyMapping(Windows.System.VirtualKey.Y, Alternet.UI.Key.Y);
            RegisterKeyMapping(Windows.System.VirtualKey.Z, Alternet.UI.Key.Z);

            RegisterKeyMapping(Windows.System.VirtualKey.LeftWindows, Alternet.UI.Key.Windows);
            RegisterKeyMapping(Windows.System.VirtualKey.RightWindows, Alternet.UI.Key.Windows);

            RegisterKeyMapping(Windows.System.VirtualKey.NumberPad0, Alternet.UI.Key.NumPad0);
            RegisterKeyMapping(Windows.System.VirtualKey.NumberPad1, Alternet.UI.Key.NumPad1);
            RegisterKeyMapping(Windows.System.VirtualKey.NumberPad2, Alternet.UI.Key.NumPad2);
            RegisterKeyMapping(Windows.System.VirtualKey.NumberPad3, Alternet.UI.Key.NumPad3);
            RegisterKeyMapping(Windows.System.VirtualKey.NumberPad4, Alternet.UI.Key.NumPad4);
            RegisterKeyMapping(Windows.System.VirtualKey.NumberPad5, Alternet.UI.Key.NumPad5);
            RegisterKeyMapping(Windows.System.VirtualKey.NumberPad6, Alternet.UI.Key.NumPad6);
            RegisterKeyMapping(Windows.System.VirtualKey.NumberPad7, Alternet.UI.Key.NumPad7);
            RegisterKeyMapping(Windows.System.VirtualKey.NumberPad8, Alternet.UI.Key.NumPad8);
            RegisterKeyMapping(Windows.System.VirtualKey.NumberPad9, Alternet.UI.Key.NumPad9);

            RegisterKeyMapping(Windows.System.VirtualKey.Multiply, Alternet.UI.Key.Asterisk);
            RegisterKeyMapping(Windows.System.VirtualKey.Add, Alternet.UI.Key.PlusSign);
            RegisterKeyMapping(Windows.System.VirtualKey.Subtract, Alternet.UI.Key.Minus);
            RegisterKeyMapping(Windows.System.VirtualKey.Decimal, Alternet.UI.Key.Period);
            RegisterKeyMapping(Windows.System.VirtualKey.Divide, Alternet.UI.Key.Slash);

            RegisterKeyMapping(Windows.System.VirtualKey.F1, Alternet.UI.Key.F1);
            RegisterKeyMapping(Windows.System.VirtualKey.F2, Alternet.UI.Key.F2);
            RegisterKeyMapping(Windows.System.VirtualKey.F3, Alternet.UI.Key.F3);
            RegisterKeyMapping(Windows.System.VirtualKey.F4, Alternet.UI.Key.F4);
            RegisterKeyMapping(Windows.System.VirtualKey.F5, Alternet.UI.Key.F5);
            RegisterKeyMapping(Windows.System.VirtualKey.F6, Alternet.UI.Key.F6);
            RegisterKeyMapping(Windows.System.VirtualKey.F7, Alternet.UI.Key.F7);
            RegisterKeyMapping(Windows.System.VirtualKey.F8, Alternet.UI.Key.F8);
            RegisterKeyMapping(Windows.System.VirtualKey.F9, Alternet.UI.Key.F9);
            RegisterKeyMapping(Windows.System.VirtualKey.F10, Alternet.UI.Key.F10);
            RegisterKeyMapping(Windows.System.VirtualKey.F11, Alternet.UI.Key.F11);
            RegisterKeyMapping(Windows.System.VirtualKey.F12, Alternet.UI.Key.F12);
            RegisterKeyMapping(Windows.System.VirtualKey.F13, Alternet.UI.Key.F13);
            RegisterKeyMapping(Windows.System.VirtualKey.F14, Alternet.UI.Key.F14);
            RegisterKeyMapping(Windows.System.VirtualKey.F15, Alternet.UI.Key.F15);
            RegisterKeyMapping(Windows.System.VirtualKey.F16, Alternet.UI.Key.F16);
            RegisterKeyMapping(Windows.System.VirtualKey.F17, Alternet.UI.Key.F17);
            RegisterKeyMapping(Windows.System.VirtualKey.F18, Alternet.UI.Key.F18);
            RegisterKeyMapping(Windows.System.VirtualKey.F19, Alternet.UI.Key.F19);
            RegisterKeyMapping(Windows.System.VirtualKey.F20, Alternet.UI.Key.F20);
            RegisterKeyMapping(Windows.System.VirtualKey.F21, Alternet.UI.Key.F21);
            RegisterKeyMapping(Windows.System.VirtualKey.F22, Alternet.UI.Key.F22);
            RegisterKeyMapping(Windows.System.VirtualKey.F23, Alternet.UI.Key.F23);
            RegisterKeyMapping(Windows.System.VirtualKey.F24, Alternet.UI.Key.F24);

            // MapKey(Windows.System.VirtualKey.LeftButton     , Alternet.UI.Key.LeftButton     );
            // MapKey(Windows.System.VirtualKey.RightButton    , Alternet.UI.Key.RightButton    );
            // MapKey(Windows.System.VirtualKey.Cancel         , Alternet.UI.Key.Cancel         );
            // MapKey(Windows.System.VirtualKey.MiddleButton   , Alternet.UI.Key.MiddleButton   );
            // MapKey(Windows.System.VirtualKey.XButton1       , Alternet.UI.Key.XButton1       );
            // MapKey(Windows.System.VirtualKey.XButton2       , Alternet.UI.Key.XButton2       );

            // MapKey(Windows.System.VirtualKey.Kana           , Alternet.UI.Key.Kana           );
            // MapKey(Windows.System.VirtualKey.Hangul         , Alternet.UI.Key.Hangul         );
            // MapKey(Windows.System.VirtualKey.Junja          , Alternet.UI.Key.Junja          );
            // MapKey(Windows.System.VirtualKey.Final          , Alternet.UI.Key.Final          );
            // MapKey(Windows.System.VirtualKey.Hanja          , Alternet.UI.Key.Hanja          );
            // MapKey(Windows.System.VirtualKey.Kanji          , Alternet.UI.Key.Kanji          );
            // MapKey(Windows.System.VirtualKey.Escape         , Alternet.UI.Key.Escape         );
            // MapKey(Windows.System.VirtualKey.Convert        , Alternet.UI.Key.Convert        );
            // MapKey(Windows.System.VirtualKey.NonConvert     , Alternet.UI.Key.NonConvert     );
            // MapKey(Windows.System.VirtualKey.Accept         , Alternet.UI.Key.Accept         );
            // MapKey(Windows.System.VirtualKey.ModeChange     , Alternet.UI.Key.ModeChange     );

            // MapKey(Windows.System.VirtualKey.Select         , Alternet.UI.Key.Select         );
            // MapKey(Windows.System.VirtualKey.Print          , Alternet.UI.Key.Print          );
            // MapKey(Windows.System.VirtualKey.Execute        , Alternet.UI.Key.Execute        );
            // MapKey(Windows.System.VirtualKey.Snapshot       , Alternet.UI.Key.Snapshot);

            // MapKey(Windows.System.VirtualKey.Help           , Alternet.UI.Key.Help           );

            // MapKey(Windows.System.VirtualKey.Application    , Alternet.UI.Key.Application    );
            // MapKey(Windows.System.VirtualKey.Sleep          , Alternet.UI.Key.Sleep          );
            // MapKey(Windows.System.VirtualKey.Separator      , Alternet.UI.Key.Separator      );

            // MapKey(Windows.System.VirtualKey.NavigationView , Alternet.UI.Key.NavigationView);
            // MapKey(Windows.System.VirtualKey.NavigationMenu , Alternet.UI.Key.NavigationMenu );
            // MapKey(Windows.System.VirtualKey.NavigationUp   , Alternet.UI.Key.NavigationUp   );
            // MapKey(Windows.System.VirtualKey.NavigationDown , Alternet.UI.Key.NavigationDown );
            // MapKey(Windows.System.VirtualKey.NavigationLeft , Alternet.UI.Key.NavigationLeft );
            // MapKey(Windows.System.VirtualKey.NavigationRight, Alternet.UI.Key.NavigationRight);
            // MapKey(Windows.System.VirtualKey.NavigationAccept, Alternet.UI.Key.NavigationAccept);
            // MapKey(Windows.System.VirtualKey.NavigationCancel, Alternet.UI.Key.NavigationCancel);
            // MapKey(Windows.System.VirtualKey.NumberKeyLock  , Alternet.UI.Key.NumberKeyLock);
            // MapKey(Windows.System.VirtualKey.Scroll         , Alternet.UI.Key.Scroll         );
            // MapKey(Windows.System.VirtualKey.LeftShift      , Alternet.UI.Key.LeftShift      );
            // MapKey(Windows.System.VirtualKey.RightShift     , Alternet.UI.Key.RightShift     );
            // MapKey(Windows.System.VirtualKey.LeftControl    , Alternet.UI.Key.LeftControl    );
            // MapKey(Windows.System.VirtualKey.RightControl   , Alternet.UI.Key.RightControl   );
            // MapKey(Windows.System.VirtualKey.LeftMenu       , Alternet.UI.Key.LeftMenu       );
            // MapKey(Windows.System.VirtualKey.RightMenu      , Alternet.UI.Key.RightMenu      );
            // MapKey(Windows.System.VirtualKey.GoBack         , Alternet.UI.Key.GoBack         );
            // MapKey(Windows.System.VirtualKey.GoForward      , Alternet.UI.Key.GoForward      );
            // MapKey(Windows.System.VirtualKey.Refresh        , Alternet.UI.Key.Refresh        );
            // MapKey(Windows.System.VirtualKey.Stop           , Alternet.UI.Key.Stop           );
            // MapKey(Windows.System.VirtualKey.Search         , Alternet.UI.Key.Search         );
            // MapKey(Windows.System.VirtualKey.Favorites      , Alternet.UI.Key.Favorites      );
            // MapKey(Windows.System.VirtualKey.GoHome         , Alternet.UI.Key.GoHome         );
            // MapKey(Windows.System.VirtualKey.GamepadA       , Alternet.UI.Key.GamepadA       );
            // MapKey(Windows.System.VirtualKey.GamepadB       , Alternet.UI.Key.GamepadB       );
            // MapKey(Windows.System.VirtualKey.GamepadX       , Alternet.UI.Key.GamepadX       );
            // MapKey(Windows.System.VirtualKey.GamepadY       , Alternet.UI.Key.GamepadY       );
            // MapKey(Windows.System.VirtualKey.GamepadRightShoulder   , Alternet.UI.Key.GamepadRightShoulder);
            // MapKey(Windows.System.VirtualKey.GamepadLeftShoulder    , Alternet.UI.Key.GamepadLeftShoulder    );
            // MapKey(Windows.System.VirtualKey.GamepadLeftTrigger     , Alternet.UI.Key.GamepadLeftTrigger     );
            // MapKey(Windows.System.VirtualKey.GamepadRightTrigger    , Alternet.UI.Key.GamepadRightTrigger    );
            // MapKey(Windows.System.VirtualKey.GamepadDPadUp          , Alternet.UI.Key.GamepadDPadUp          );
            // MapKey(Windows.System.VirtualKey.GamepadDPadDown        , Alternet.UI.Key.GamepadDPadDown        );
            // MapKey(Windows.System.VirtualKey.GamepadDPadLeft        , Alternet.UI.Key.GamepadDPadLeft        );
            // MapKey(Windows.System.VirtualKey.GamepadDPadRight       , Alternet.UI.Key.GamepadDPadRight       );
            // MapKey(Windows.System.VirtualKey.GamepadMenu            , Alternet.UI.Key.GamepadMenu            );
            // MapKey(Windows.System.VirtualKey.GamepadView            , Alternet.UI.Key.GamepadView            );
            // MapKey(Windows.System.VirtualKey.GamepadLeftThumbstickButton    , Alternet.UI.Key.GamepadLeftThumbstickButton);
            // MapKey(Windows.System.VirtualKey.GamepadRightThumbstickButton   , Alternet.UI.Key.GamepadRightThumbstickButton   );
            // MapKey(Windows.System.VirtualKey.GamepadLeftThumbstickUp        , Alternet.UI.Key.GamepadLeftThumbstickUp        );
            // MapKey(Windows.System.VirtualKey.GamepadLeftThumbstickDown      , Alternet.UI.Key.GamepadLeftThumbstickDown      );
            // MapKey(Windows.System.VirtualKey.GamepadLeftThumbstickRight     , Alternet.UI.Key.GamepadLeftThumbstickRight     );
            // MapKey(Windows.System.VirtualKey.GamepadLeftThumbstickLeft      , Alternet.UI.Key.GamepadLeftThumbstickLeft      );
            // MapKey(Windows.System.VirtualKey.GamepadRightThumbstickUp       , Alternet.UI.Key.GamepadRightThumbstickUp       );
            // MapKey(Windows.System.VirtualKey.GamepadRightThumbstickDown     , Alternet.UI.Key.GamepadRightThumbstickDown     );
            // MapKey(Windows.System.VirtualKey.GamepadRightThumbstickRight    , Alternet.UI.Key.GamepadRightThumbstickRight    );
            // MapKey(Windows.System.VirtualKey.GamepadRightThumbstickLeft     , Alternet.UI.Key.GamepadRightThumbstickLeft     );

#pragma warning restore
        }

        public KeyStates GetKeyStatesFromSystem(Key key)
        {
            return KeyStates.None;
        }

        public bool HideKeyboard(Control? control)
        {
            return HideKeyboard();
        }

        public bool IsSoftKeyboardShowing(Control? control)
        {
            return IsSoftKeyboardShowing();
        }

        public bool ShowKeyboard(Control? control)
        {
            return ShowKeyboard();
        }

        private static bool HideKeyboard()
        {
            if (TryGetInputPane(out CoreInputView? inputPane))
            {
                return inputPane.TryHide();
            }

            return false;
        }

        private static bool ShowKeyboard()
        {
            if (TryGetInputPane(out CoreInputView? inputPane))
            {
                return inputPane.TryShow();
            }

            return false;
        }

        private static bool IsSoftKeyboardShowing()
        {
            if (TryGetInputPane(out InputPane? inputPane))
            {
                return inputPane.Visible;
            }

            return false;
        }

        private static bool TryGetInputPane([NotNullWhen(true)] out CoreInputView? inputPane)
        {
            inputPane = CoreInputView.GetForCurrentView();
            return true;
        }

        private static bool TryGetInputPane([NotNullWhen(true)] out InputPane? inputPane)
        {
            nint mainWindowHandle = Process.GetCurrentProcess().MainWindowHandle;
            if (mainWindowHandle == IntPtr.Zero)
            {
                inputPane = null;
                return false;
            }

            inputPane = InputPaneInterop.GetForWindow(mainWindowHandle);
            return true;
        }
    }
}
