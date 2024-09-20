using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.Controls.PlatformConfiguration;

#if WINDOWS
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;

using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.ViewManagement.Core;

namespace Alternet.UI
{
    /// <summary>
    /// Implements <see cref="IKeyboardHandler"/> for MAUI under Windows.
    /// </summary>
    public class MauiKeyboardHandler : MappedKeyboardHandler<Windows.System.VirtualKey>, IKeyboardHandler
    {
        /// <summary>
        /// Max supported value of <see cref="Windows.System.VirtualKey"/>.
        /// </summary>
        public const Windows.System.VirtualKey VirtualKeyMaxValue
                    = Windows.System.VirtualKey.GamepadRightThumbstickLeft;

        /// <summary>
        /// Gets or sets default <see cref="IKeyboardHandler"/> implementation.
        /// </summary>
        public static MauiKeyboardHandler Default = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="MauiKeyboardHandler"/> class.
        /// </summary>
        public MauiKeyboardHandler()
            : base(VirtualKeyMaxValue, Key.MaxMaui)
        {
        }

        /// <summary>
        /// Converts event arguments from <see cref="CharacterReceivedRoutedEventArgs"/> to
        /// <see cref="Alternet.UI.KeyPressEventArgs"/>.
        /// </summary>
        /// <param name="control">Target control.</param>
        /// <param name="e">Event arguments.</param>
        /// <returns></returns>
        public virtual Alternet.UI.KeyPressEventArgs Convert(
            Control control,
            CharacterReceivedRoutedEventArgs e)
        {
            Alternet.UI.KeyPressEventArgs result = new(control, e.Character);
            return result;
        }

        /// <summary>
        /// Converts event arguments from <see cref="KeyRoutedEventArgs"/> to
        /// <see cref="Alternet.UI.KeyEventArgs"/>.
        /// </summary>
        /// <param name="control">Target control.</param>
        /// <param name="e">Event arguments.</param>
        /// <returns></returns>
        public virtual Alternet.UI.KeyEventArgs Convert(Control control, KeyRoutedEventArgs e)
        {
            var alternetKey = Convert(e.Key);

            Alternet.UI.KeyEventArgs result = new(control, alternetKey, e.KeyStatus.RepeatCount);

            return result;
        }

        /// <inheritdoc/>
        public override void RegisterKeyMappings()
        {
#pragma warning disable
            AddMapping(Windows.System.VirtualKey.None, Alternet.UI.Key.None);

            AddMapping(Windows.System.VirtualKey.Back, Alternet.UI.Key.Back);
            AddMapping(Windows.System.VirtualKey.Tab, Alternet.UI.Key.Tab);
            AddMapping(Windows.System.VirtualKey.Clear, Alternet.UI.Key.Clear);
            AddMapping(Windows.System.VirtualKey.Enter, Alternet.UI.Key.Enter);
            AddMapping(Windows.System.VirtualKey.Shift, Alternet.UI.Key.Shift);
            AddMapping(Windows.System.VirtualKey.Control, Alternet.UI.Key.Control);
            AddMapping(Windows.System.VirtualKey.Menu, Alternet.UI.Key.Alt);
            AddMapping(Windows.System.VirtualKey.Pause, Alternet.UI.Key.Pause);
            AddMapping(Windows.System.VirtualKey.CapitalLock, Alternet.UI.Key.CapsLock);

            AddMapping(Windows.System.VirtualKey.Space, Alternet.UI.Key.Space);
            AddMapping(Windows.System.VirtualKey.PageUp, Alternet.UI.Key.PageUp);
            AddMapping(Windows.System.VirtualKey.PageDown, Alternet.UI.Key.PageDown);
            AddMapping(Windows.System.VirtualKey.End, Alternet.UI.Key.End);
            AddMapping(Windows.System.VirtualKey.Home, Alternet.UI.Key.Home);
            AddMapping(Windows.System.VirtualKey.Left, Alternet.UI.Key.Left);
            AddMapping(Windows.System.VirtualKey.Up, Alternet.UI.Key.Up);
            AddMapping(Windows.System.VirtualKey.Right, Alternet.UI.Key.Right);
            AddMapping(Windows.System.VirtualKey.Down, Alternet.UI.Key.Down);

            AddMapping(Windows.System.VirtualKey.Insert, Alternet.UI.Key.Insert);
            AddMapping(Windows.System.VirtualKey.Delete, Alternet.UI.Key.Delete);

            AddMapping(Windows.System.VirtualKey.Number0, Alternet.UI.Key.D0);
            AddMapping(Windows.System.VirtualKey.Number1, Alternet.UI.Key.D1);
            AddMapping(Windows.System.VirtualKey.Number2, Alternet.UI.Key.D2);
            AddMapping(Windows.System.VirtualKey.Number3, Alternet.UI.Key.D3);
            AddMapping(Windows.System.VirtualKey.Number4, Alternet.UI.Key.D4);
            AddMapping(Windows.System.VirtualKey.Number5, Alternet.UI.Key.D5);
            AddMapping(Windows.System.VirtualKey.Number6, Alternet.UI.Key.D6);
            AddMapping(Windows.System.VirtualKey.Number7, Alternet.UI.Key.D7);
            AddMapping(Windows.System.VirtualKey.Number8, Alternet.UI.Key.D8);
            AddMapping(Windows.System.VirtualKey.Number9, Alternet.UI.Key.D9);
            AddMapping(Windows.System.VirtualKey.A, Alternet.UI.Key.A);
            AddMapping(Windows.System.VirtualKey.B, Alternet.UI.Key.B);
            AddMapping(Windows.System.VirtualKey.C, Alternet.UI.Key.C);
            AddMapping(Windows.System.VirtualKey.D, Alternet.UI.Key.D);
            AddMapping(Windows.System.VirtualKey.E, Alternet.UI.Key.E);
            AddMapping(Windows.System.VirtualKey.F, Alternet.UI.Key.F);
            AddMapping(Windows.System.VirtualKey.G, Alternet.UI.Key.G);
            AddMapping(Windows.System.VirtualKey.H, Alternet.UI.Key.H);
            AddMapping(Windows.System.VirtualKey.I, Alternet.UI.Key.I);
            AddMapping(Windows.System.VirtualKey.J, Alternet.UI.Key.J);
            AddMapping(Windows.System.VirtualKey.K, Alternet.UI.Key.K);
            AddMapping(Windows.System.VirtualKey.L, Alternet.UI.Key.L);
            AddMapping(Windows.System.VirtualKey.M, Alternet.UI.Key.M);
            AddMapping(Windows.System.VirtualKey.N, Alternet.UI.Key.N);
            AddMapping(Windows.System.VirtualKey.O, Alternet.UI.Key.O);
            AddMapping(Windows.System.VirtualKey.P, Alternet.UI.Key.P);
            AddMapping(Windows.System.VirtualKey.Q, Alternet.UI.Key.Q);
            AddMapping(Windows.System.VirtualKey.R, Alternet.UI.Key.R);
            AddMapping(Windows.System.VirtualKey.S, Alternet.UI.Key.S);
            AddMapping(Windows.System.VirtualKey.T, Alternet.UI.Key.T);
            AddMapping(Windows.System.VirtualKey.U, Alternet.UI.Key.U);
            AddMapping(Windows.System.VirtualKey.V, Alternet.UI.Key.V);
            AddMapping(Windows.System.VirtualKey.W, Alternet.UI.Key.W);
            AddMapping(Windows.System.VirtualKey.X, Alternet.UI.Key.X);
            AddMapping(Windows.System.VirtualKey.Y, Alternet.UI.Key.Y);
            AddMapping(Windows.System.VirtualKey.Z, Alternet.UI.Key.Z);

            AddMapping(Windows.System.VirtualKey.LeftWindows, Alternet.UI.Key.Windows);
            AddMapping(Windows.System.VirtualKey.RightWindows, Alternet.UI.Key.Windows);

            AddMapping(Windows.System.VirtualKey.NumberPad0, Alternet.UI.Key.NumPad0);
            AddMapping(Windows.System.VirtualKey.NumberPad1, Alternet.UI.Key.NumPad1);
            AddMapping(Windows.System.VirtualKey.NumberPad2, Alternet.UI.Key.NumPad2);
            AddMapping(Windows.System.VirtualKey.NumberPad3, Alternet.UI.Key.NumPad3);
            AddMapping(Windows.System.VirtualKey.NumberPad4, Alternet.UI.Key.NumPad4);
            AddMapping(Windows.System.VirtualKey.NumberPad5, Alternet.UI.Key.NumPad5);
            AddMapping(Windows.System.VirtualKey.NumberPad6, Alternet.UI.Key.NumPad6);
            AddMapping(Windows.System.VirtualKey.NumberPad7, Alternet.UI.Key.NumPad7);
            AddMapping(Windows.System.VirtualKey.NumberPad8, Alternet.UI.Key.NumPad8);
            AddMapping(Windows.System.VirtualKey.NumberPad9, Alternet.UI.Key.NumPad9);

            AddMapping(Windows.System.VirtualKey.Multiply, Alternet.UI.Key.Asterisk);
            AddMapping(Windows.System.VirtualKey.Add, Alternet.UI.Key.PlusSign);
            AddMapping(Windows.System.VirtualKey.Subtract, Alternet.UI.Key.Minus);
            AddMapping(Windows.System.VirtualKey.Decimal, Alternet.UI.Key.Period);
            AddMapping(Windows.System.VirtualKey.Divide, Alternet.UI.Key.Slash);

            AddMapping(Windows.System.VirtualKey.F1, Alternet.UI.Key.F1);
            AddMapping(Windows.System.VirtualKey.F2, Alternet.UI.Key.F2);
            AddMapping(Windows.System.VirtualKey.F3, Alternet.UI.Key.F3);
            AddMapping(Windows.System.VirtualKey.F4, Alternet.UI.Key.F4);
            AddMapping(Windows.System.VirtualKey.F5, Alternet.UI.Key.F5);
            AddMapping(Windows.System.VirtualKey.F6, Alternet.UI.Key.F6);
            AddMapping(Windows.System.VirtualKey.F7, Alternet.UI.Key.F7);
            AddMapping(Windows.System.VirtualKey.F8, Alternet.UI.Key.F8);
            AddMapping(Windows.System.VirtualKey.F9, Alternet.UI.Key.F9);
            AddMapping(Windows.System.VirtualKey.F10, Alternet.UI.Key.F10);
            AddMapping(Windows.System.VirtualKey.F11, Alternet.UI.Key.F11);
            AddMapping(Windows.System.VirtualKey.F12, Alternet.UI.Key.F12);
            AddMapping(Windows.System.VirtualKey.F13, Alternet.UI.Key.F13);
            AddMapping(Windows.System.VirtualKey.F14, Alternet.UI.Key.F14);
            AddMapping(Windows.System.VirtualKey.F15, Alternet.UI.Key.F15);
            AddMapping(Windows.System.VirtualKey.F16, Alternet.UI.Key.F16);
            AddMapping(Windows.System.VirtualKey.F17, Alternet.UI.Key.F17);
            AddMapping(Windows.System.VirtualKey.F18, Alternet.UI.Key.F18);
            AddMapping(Windows.System.VirtualKey.F19, Alternet.UI.Key.F19);
            AddMapping(Windows.System.VirtualKey.F20, Alternet.UI.Key.F20);
            AddMapping(Windows.System.VirtualKey.F21, Alternet.UI.Key.F21);
            AddMapping(Windows.System.VirtualKey.F22, Alternet.UI.Key.F22);
            AddMapping(Windows.System.VirtualKey.F23, Alternet.UI.Key.F23);
            AddMapping(Windows.System.VirtualKey.F24, Alternet.UI.Key.F24);

            AddMapping(Windows.System.VirtualKey.Print, Alternet.UI.Key.PrintScreen);
            AddMapping(Windows.System.VirtualKey.Help, Alternet.UI.Key.F1);

            AddMapping(Windows.System.VirtualKey.NumberKeyLock, Alternet.UI.Key.NumLock);
            AddMapping(Windows.System.VirtualKey.Scroll, Alternet.UI.Key.ScrollLock);

            AddMapping(Windows.System.VirtualKey.GoBack, Alternet.UI.Key.BrowserBack);
            AddMapping(Windows.System.VirtualKey.GoForward, Alternet.UI.Key.BrowserForward);
            AddMapping(Windows.System.VirtualKey.Refresh, Alternet.UI.Key.BrowserRefresh);
            AddMapping(Windows.System.VirtualKey.Stop, Alternet.UI.Key.BrowserStop);
            AddMapping(Windows.System.VirtualKey.Search, Alternet.UI.Key.BrowserSearch);
            AddMapping(Windows.System.VirtualKey.Favorites, Alternet.UI.Key.BrowserFavorites);
            AddMapping(Windows.System.VirtualKey.GoHome, Alternet.UI.Key.BrowserHome);

            AddMapping(Windows.System.VirtualKey.GamepadA, Alternet.UI.Key.GamepadA);
            AddMapping(Windows.System.VirtualKey.GamepadB, Alternet.UI.Key.GamepadB);
            AddMapping(Windows.System.VirtualKey.GamepadX, Alternet.UI.Key.GamepadX);
            AddMapping(Windows.System.VirtualKey.GamepadY, Alternet.UI.Key.GamepadY);
            
            AddMapping(
                Windows.System.VirtualKey.GamepadRightShoulder, Alternet.UI.Key.GamepadRightShoulder);
            
            AddMapping(Windows.System.VirtualKey.GamepadLeftShoulder, Alternet.UI.Key.GamepadLeftShoulder);
            AddMapping(Windows.System.VirtualKey.GamepadLeftTrigger, Alternet.UI.Key.GamepadLeftTrigger);
            AddMapping(Windows.System.VirtualKey.GamepadRightTrigger, Alternet.UI.Key.GamepadRightTrigger);
            AddMapping(Windows.System.VirtualKey.GamepadDPadUp, Alternet.UI.Key.GamepadDPadUp);
            AddMapping(Windows.System.VirtualKey.GamepadDPadDown, Alternet.UI.Key.GamepadDPadDown);
            AddMapping(Windows.System.VirtualKey.GamepadDPadLeft, Alternet.UI.Key.GamepadDPadLeft);
            AddMapping(Windows.System.VirtualKey.GamepadDPadRight, Alternet.UI.Key.GamepadDPadRight);
            AddMapping(Windows.System.VirtualKey.GamepadMenu, Alternet.UI.Key.GamepadMenu);
            AddMapping(Windows.System.VirtualKey.GamepadView, Alternet.UI.Key.GamepadView);
            AddMapping(Windows.System.VirtualKey.GamepadLeftThumbstickButton, Alternet.UI.Key.GamepadLeftThumbstickButton);
            AddMapping(Windows.System.VirtualKey.GamepadRightThumbstickButton, Alternet.UI.Key.GamepadRightThumbstickButton);
            AddMapping(Windows.System.VirtualKey.GamepadLeftThumbstickUp, Alternet.UI.Key.GamepadLeftThumbstickUp);
            AddMapping(Windows.System.VirtualKey.GamepadLeftThumbstickDown, Alternet.UI.Key.GamepadLeftThumbstickDown);
            AddMapping(Windows.System.VirtualKey.GamepadLeftThumbstickRight, Alternet.UI.Key.GamepadLeftThumbstickRight);
            AddMapping(Windows.System.VirtualKey.GamepadLeftThumbstickLeft, Alternet.UI.Key.GamepadLeftThumbstickLeft);
            AddMapping(Windows.System.VirtualKey.GamepadRightThumbstickUp, Alternet.UI.Key.GamepadRightThumbstickUp);
            AddMapping(Windows.System.VirtualKey.GamepadRightThumbstickDown, Alternet.UI.Key.GamepadRightThumbstickDown);
            AddMapping(Windows.System.VirtualKey.GamepadRightThumbstickRight, Alternet.UI.Key.GamepadRightThumbstickRight);
            AddMapping(Windows.System.VirtualKey.GamepadRightThumbstickLeft, Alternet.UI.Key.GamepadRightThumbstickLeft);

            AddMapping(Windows.System.VirtualKey.Escape, Alternet.UI.Key.Escape);

            // AddMapping(Windows.System.VirtualKey.LeftButton     , Alternet.UI.Key.LeftButton     );
            // AddMapping(Windows.System.VirtualKey.RightButton    , Alternet.UI.Key.RightButton    );
            // AddMapping(Windows.System.VirtualKey.Cancel         , Alternet.UI.Key.Cancel         );
            // AddMapping(Windows.System.VirtualKey.MiddleButton   , Alternet.UI.Key.MiddleButton   );
            // AddMapping(Windows.System.VirtualKey.XButton1       , Alternet.UI.Key.XButton1       );
            // AddMapping(Windows.System.VirtualKey.XButton2       , Alternet.UI.Key.XButton2       );

            AddMapping(Windows.System.VirtualKey.Kana           , Alternet.UI.Key.Kana           );
            AddMapping(Windows.System.VirtualKey.Hangul         , Alternet.UI.Key.Hangul         );
            AddMapping(Windows.System.VirtualKey.Junja          , Alternet.UI.Key.Junja          );
            AddMapping(Windows.System.VirtualKey.Final          , Alternet.UI.Key.Final          );
            AddMapping(Windows.System.VirtualKey.Hanja          , Alternet.UI.Key.Hanja          );
            AddMapping(Windows.System.VirtualKey.Kanji          , Alternet.UI.Key.Kanji          );

            AddMapping(Windows.System.VirtualKey.Convert        , Alternet.UI.Key.Convert        );
            AddMapping(Windows.System.VirtualKey.NonConvert     , Alternet.UI.Key.NonConvert     );
            AddMapping(Windows.System.VirtualKey.Accept         , Alternet.UI.Key.Accept         );
            AddMapping(Windows.System.VirtualKey.ModeChange     , Alternet.UI.Key.ModeChange     );

            AddMapping(Windows.System.VirtualKey.Select         , Alternet.UI.Key.Select         );
            AddMapping(Windows.System.VirtualKey.Execute        , Alternet.UI.Key.Execute        );
            AddMapping(Windows.System.VirtualKey.Snapshot       , Alternet.UI.Key.Snapshot);

            AddMapping(Windows.System.VirtualKey.Application    , Alternet.UI.Key.Menu    );
            AddMapping(Windows.System.VirtualKey.Sleep          , Alternet.UI.Key.Sleep          );

            AddMapping(Windows.System.VirtualKey.Separator      , Alternet.UI.Key.Comma      );

            AddMapping(Windows.System.VirtualKey.NavigationView , Alternet.UI.Key.NavigationView);
            AddMapping(Windows.System.VirtualKey.NavigationMenu , Alternet.UI.Key.NavigationMenu );
            AddMapping(Windows.System.VirtualKey.NavigationUp   , Alternet.UI.Key.NavigationUp   );
            AddMapping(Windows.System.VirtualKey.NavigationDown , Alternet.UI.Key.NavigationDown );
            AddMapping(Windows.System.VirtualKey.NavigationLeft , Alternet.UI.Key.NavigationLeft );
            AddMapping(Windows.System.VirtualKey.NavigationRight, Alternet.UI.Key.NavigationRight);
            AddMapping(Windows.System.VirtualKey.NavigationAccept, Alternet.UI.Key.NavigationAccept);
            AddMapping(Windows.System.VirtualKey.NavigationCancel, Alternet.UI.Key.NavigationCancel);

            // AddMapping(Windows.System.VirtualKey.LeftShift      , Alternet.UI.Key.LeftShift);
            // AddMapping(Windows.System.VirtualKey.RightShift     , Alternet.UI.Key.RightShift);
            // AddMapping(Windows.System.VirtualKey.LeftControl    , Alternet.UI.Key.LeftControl);
            // AddMapping(Windows.System.VirtualKey.RightControl   , Alternet.UI.Key.RightControl);
            // AddMapping(Windows.System.VirtualKey.LeftMenu       , Alternet.UI.Key.LeftMenu);
            // AddMapping(Windows.System.VirtualKey.RightMenu      , Alternet.UI.Key.RightMenu      );

#pragma warning restore
        }

        /// <summary>
        /// Converts <see cref="CoreVirtualKeyStates"/> to <see cref="KeyStates"/>.
        /// </summary>
        /// <param name="keyStates">Key states.</param>
        /// <returns></returns>
        public virtual KeyStates Convert(CoreVirtualKeyStates keyStates)
        {
            KeyStates result = default;

            if (keyStates.HasFlag(CoreVirtualKeyStates.Down))
                result |= KeyStates.Down;

            if (keyStates.HasFlag(CoreVirtualKeyStates.Locked))
                result |= KeyStates.Toggled;

            return result;
        }

        /// <inheritdoc/>
        public override KeyStates GetKeyStatesFromSystem(Key key)
        {
            return Fn(Convert(key));

            KeyStates Fn(Windows.System.VirtualKey key)
            {
                var keyState = Microsoft.UI.Input.InputKeyboardSource.GetKeyStateForCurrentThread(key);
                return Convert(keyState);
            }
        }

        /// <inheritdoc/>
        public override bool HideKeyboard(Control? control)
        {
            return HideKeyboard();
        }

        /// <inheritdoc/>
        public override bool IsSoftKeyboardShowing(Control? control)
        {
            return IsSoftKeyboardShowing();
        }

        /// <inheritdoc/>
        public override bool ShowKeyboard(Control? control)
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
#endif