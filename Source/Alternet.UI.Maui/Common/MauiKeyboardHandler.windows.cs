using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;

using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.ViewManagement.Core;

namespace Alternet.UI
{
    using VirtualKeyToAlternetMapping
        = AbstractTwoWayEnumMapping<Windows.System.VirtualKey, Alternet.UI.Key>;

    public class MauiKeyboardHandler : DisposableObject, IKeyboardHandler
    {
        private static VirtualKeyToAlternetMapping? virtualKeyToAlternet;

        static MauiKeyboardHandler()
        {
        }

        public static VirtualKeyToAlternetMapping VirtualKeyToAlternet
        {
            get
            {
                if (virtualKeyToAlternet is null)
                {
                    virtualKeyToAlternet =
                        new TwoWayEnumMapping<Windows.System.VirtualKey, Alternet.UI.Key>();
                    RegisterKeyMappings();
                }

                return virtualKeyToAlternet;
            }

            set
            {
                virtualKeyToAlternet = value;
            }
        }

        public static Alternet.UI.KeyPressEventArgs Convert(
            Control control,
            CharacterReceivedRoutedEventArgs e)
        {
            Alternet.UI.KeyPressEventArgs result = new(control, e.Character);
            return result;
        }

        public static Alternet.UI.KeyEventArgs Convert(Control control, KeyRoutedEventArgs e)
        {
            var alternetKey = Alternet.UI.MauiKeyboardHandler.Convert(e.Key);

            Alternet.UI.KeyEventArgs result = new(control, alternetKey, e.KeyStatus.RepeatCount);

            return result;
        }

        public static Alternet.UI.Key Convert(Windows.System.VirtualKey key)
        {
            return VirtualKeyToAlternet.Convert(key);
        }

        public static Windows.System.VirtualKey Convert(Alternet.UI.Key key)
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
            RegisterKeyMapping(Windows.System.VirtualKey.Menu, Alternet.UI.Key.Alt);
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

            RegisterKeyMapping(Windows.System.VirtualKey.Print, Alternet.UI.Key.PrintScreen);
            RegisterKeyMapping(Windows.System.VirtualKey.Help, Alternet.UI.Key.F1);

            RegisterKeyMapping(Windows.System.VirtualKey.NumberKeyLock, Alternet.UI.Key.NumLock);
            RegisterKeyMapping(Windows.System.VirtualKey.Scroll, Alternet.UI.Key.ScrollLock);

            RegisterKeyMapping(Windows.System.VirtualKey.GoBack, Alternet.UI.Key.BrowserBack);
            RegisterKeyMapping(Windows.System.VirtualKey.GoForward, Alternet.UI.Key.BrowserForward);
            RegisterKeyMapping(Windows.System.VirtualKey.Refresh, Alternet.UI.Key.BrowserRefresh);
            RegisterKeyMapping(Windows.System.VirtualKey.Stop, Alternet.UI.Key.BrowserStop);
            RegisterKeyMapping(Windows.System.VirtualKey.Search, Alternet.UI.Key.BrowserSearch);
            RegisterKeyMapping(Windows.System.VirtualKey.Favorites, Alternet.UI.Key.BrowserFavorites);
            RegisterKeyMapping(Windows.System.VirtualKey.GoHome, Alternet.UI.Key.BrowserHome);

            RegisterKeyMapping(Windows.System.VirtualKey.GamepadA, Alternet.UI.Key.GamepadA);
            RegisterKeyMapping(Windows.System.VirtualKey.GamepadB, Alternet.UI.Key.GamepadB);
            RegisterKeyMapping(Windows.System.VirtualKey.GamepadX, Alternet.UI.Key.GamepadX);
            RegisterKeyMapping(Windows.System.VirtualKey.GamepadY, Alternet.UI.Key.GamepadY);
            RegisterKeyMapping(Windows.System.VirtualKey.GamepadRightShoulder, Alternet.UI.Key.GamepadRightShoulder);
            RegisterKeyMapping(Windows.System.VirtualKey.GamepadLeftShoulder, Alternet.UI.Key.GamepadLeftShoulder);
            RegisterKeyMapping(Windows.System.VirtualKey.GamepadLeftTrigger, Alternet.UI.Key.GamepadLeftTrigger);
            RegisterKeyMapping(Windows.System.VirtualKey.GamepadRightTrigger, Alternet.UI.Key.GamepadRightTrigger);
            RegisterKeyMapping(Windows.System.VirtualKey.GamepadDPadUp, Alternet.UI.Key.GamepadDPadUp);
            RegisterKeyMapping(Windows.System.VirtualKey.GamepadDPadDown, Alternet.UI.Key.GamepadDPadDown);
            RegisterKeyMapping(Windows.System.VirtualKey.GamepadDPadLeft, Alternet.UI.Key.GamepadDPadLeft);
            RegisterKeyMapping(Windows.System.VirtualKey.GamepadDPadRight, Alternet.UI.Key.GamepadDPadRight);
            RegisterKeyMapping(Windows.System.VirtualKey.GamepadMenu, Alternet.UI.Key.GamepadMenu);
            RegisterKeyMapping(Windows.System.VirtualKey.GamepadView, Alternet.UI.Key.GamepadView);
            RegisterKeyMapping(Windows.System.VirtualKey.GamepadLeftThumbstickButton, Alternet.UI.Key.GamepadLeftThumbstickButton);
            RegisterKeyMapping(Windows.System.VirtualKey.GamepadRightThumbstickButton, Alternet.UI.Key.GamepadRightThumbstickButton);
            RegisterKeyMapping(Windows.System.VirtualKey.GamepadLeftThumbstickUp, Alternet.UI.Key.GamepadLeftThumbstickUp);
            RegisterKeyMapping(Windows.System.VirtualKey.GamepadLeftThumbstickDown, Alternet.UI.Key.GamepadLeftThumbstickDown);
            RegisterKeyMapping(Windows.System.VirtualKey.GamepadLeftThumbstickRight, Alternet.UI.Key.GamepadLeftThumbstickRight);
            RegisterKeyMapping(Windows.System.VirtualKey.GamepadLeftThumbstickLeft, Alternet.UI.Key.GamepadLeftThumbstickLeft);
            RegisterKeyMapping(Windows.System.VirtualKey.GamepadRightThumbstickUp, Alternet.UI.Key.GamepadRightThumbstickUp);
            RegisterKeyMapping(Windows.System.VirtualKey.GamepadRightThumbstickDown, Alternet.UI.Key.GamepadRightThumbstickDown);
            RegisterKeyMapping(Windows.System.VirtualKey.GamepadRightThumbstickRight, Alternet.UI.Key.GamepadRightThumbstickRight);
            RegisterKeyMapping(Windows.System.VirtualKey.GamepadRightThumbstickLeft, Alternet.UI.Key.GamepadRightThumbstickLeft);

            RegisterKeyMapping(Windows.System.VirtualKey.Escape, Alternet.UI.Key.Escape);

            // RegisterKeyMapping(Windows.System.VirtualKey.LeftButton     , Alternet.UI.Key.LeftButton     );
            // RegisterKeyMapping(Windows.System.VirtualKey.RightButton    , Alternet.UI.Key.RightButton    );
            // RegisterKeyMapping(Windows.System.VirtualKey.Cancel         , Alternet.UI.Key.Cancel         );
            // RegisterKeyMapping(Windows.System.VirtualKey.MiddleButton   , Alternet.UI.Key.MiddleButton   );
            // RegisterKeyMapping(Windows.System.VirtualKey.XButton1       , Alternet.UI.Key.XButton1       );
            // RegisterKeyMapping(Windows.System.VirtualKey.XButton2       , Alternet.UI.Key.XButton2       );

            RegisterKeyMapping(Windows.System.VirtualKey.Kana           , Alternet.UI.Key.Kana           );
            RegisterKeyMapping(Windows.System.VirtualKey.Hangul         , Alternet.UI.Key.Hangul         );
            RegisterKeyMapping(Windows.System.VirtualKey.Junja          , Alternet.UI.Key.Junja          );
            RegisterKeyMapping(Windows.System.VirtualKey.Final          , Alternet.UI.Key.Final          );
            RegisterKeyMapping(Windows.System.VirtualKey.Hanja          , Alternet.UI.Key.Hanja          );
            RegisterKeyMapping(Windows.System.VirtualKey.Kanji          , Alternet.UI.Key.Kanji          );

            RegisterKeyMapping(Windows.System.VirtualKey.Convert        , Alternet.UI.Key.Convert        );
            RegisterKeyMapping(Windows.System.VirtualKey.NonConvert     , Alternet.UI.Key.NonConvert     );
            RegisterKeyMapping(Windows.System.VirtualKey.Accept         , Alternet.UI.Key.Accept         );
            RegisterKeyMapping(Windows.System.VirtualKey.ModeChange     , Alternet.UI.Key.ModeChange     );

            RegisterKeyMapping(Windows.System.VirtualKey.Select         , Alternet.UI.Key.Select         );
            RegisterKeyMapping(Windows.System.VirtualKey.Execute        , Alternet.UI.Key.Execute        );
            RegisterKeyMapping(Windows.System.VirtualKey.Snapshot       , Alternet.UI.Key.Snapshot);

            RegisterKeyMapping(Windows.System.VirtualKey.Application    , Alternet.UI.Key.Menu    );
            RegisterKeyMapping(Windows.System.VirtualKey.Sleep          , Alternet.UI.Key.Sleep          );

            RegisterKeyMapping(Windows.System.VirtualKey.Separator      , Alternet.UI.Key.Comma      );

            RegisterKeyMapping(Windows.System.VirtualKey.NavigationView , Alternet.UI.Key.NavigationView);
            RegisterKeyMapping(Windows.System.VirtualKey.NavigationMenu , Alternet.UI.Key.NavigationMenu );
            RegisterKeyMapping(Windows.System.VirtualKey.NavigationUp   , Alternet.UI.Key.NavigationUp   );
            RegisterKeyMapping(Windows.System.VirtualKey.NavigationDown , Alternet.UI.Key.NavigationDown );
            RegisterKeyMapping(Windows.System.VirtualKey.NavigationLeft , Alternet.UI.Key.NavigationLeft );
            RegisterKeyMapping(Windows.System.VirtualKey.NavigationRight, Alternet.UI.Key.NavigationRight);
            RegisterKeyMapping(Windows.System.VirtualKey.NavigationAccept, Alternet.UI.Key.NavigationAccept);
            RegisterKeyMapping(Windows.System.VirtualKey.NavigationCancel, Alternet.UI.Key.NavigationCancel);

            // RegisterKeyMapping(Windows.System.VirtualKey.LeftShift      , Alternet.UI.Key.LeftShift);
            // RegisterKeyMapping(Windows.System.VirtualKey.RightShift     , Alternet.UI.Key.RightShift);
            // RegisterKeyMapping(Windows.System.VirtualKey.LeftControl    , Alternet.UI.Key.LeftControl);
            // RegisterKeyMapping(Windows.System.VirtualKey.RightControl   , Alternet.UI.Key.RightControl);
            // RegisterKeyMapping(Windows.System.VirtualKey.LeftMenu       , Alternet.UI.Key.LeftMenu);
            // RegisterKeyMapping(Windows.System.VirtualKey.RightMenu      , Alternet.UI.Key.RightMenu      );

#pragma warning restore
        }

        public static KeyStates Convert(CoreVirtualKeyStates keyStates)
        {
            KeyStates result = default;

            if (keyStates.HasFlag(CoreVirtualKeyStates.Down))
                result |= KeyStates.Down;

            if (keyStates.HasFlag(CoreVirtualKeyStates.Locked))
                result |= KeyStates.Toggled;

            return result;
        }

        public virtual KeyStates GetKeyStatesFromSystem(Key key)
        {
            return Fn(Convert(key));

            KeyStates Fn(Windows.System.VirtualKey key)
            {
                var keyState = Microsoft.UI.Input.InputKeyboardSource.GetKeyStateForCurrentThread(key);
                return Convert(keyState);
            }
        }

        public virtual bool HideKeyboard(Control? control)
        {
            return HideKeyboard();
        }

        public virtual bool IsSoftKeyboardShowing(Control? control)
        {
            return IsSoftKeyboardShowing();
        }

        public virtual bool ShowKeyboard(Control? control)
        {
            return ShowKeyboard();
        }

        public virtual bool IsValidKey(Key key)
        {
            return (int)key >= (int)Key.None && (int)key <= (int)Key.MaxMaui;
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
