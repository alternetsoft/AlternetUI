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

using Windows.Devices.Input;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.ViewManagement.Core;

namespace Alternet.UI
{
    /// <summary>
    /// Implements <see cref="IKeyboardHandler"/> for MAUI under Windows.
    /// </summary>
    public partial class MauiKeyboardHandler
        : PlatformKeyboardHandler<Windows.System.VirtualKey>, IKeyboardHandler
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

        private static KeyboardCapabilities? keyboardCapabilities;
        private IKeyboardVisibilityService? visibilityService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MauiKeyboardHandler"/> class.
        /// </summary>
        public MauiKeyboardHandler()
            : base(VirtualKeyMaxValue, Key.Max)
        {
        }

        /// <summary>
        /// Gets keyboard capabilities.
        /// </summary>
        public static KeyboardCapabilities KeyboardCapabilities
        {
            get
            {
                return keyboardCapabilities ??= new Windows.Devices.Input.KeyboardCapabilities();
            }
        }

        /// <inheritdoc/>
        public override bool? KeyboardPresent
        {
            get
            {
                var result = KeyboardCapabilities.KeyboardPresent != 0;
                return result;
            }
        }

        /// <inheritdoc/>
        public override IKeyboardVisibilityService? VisibilityService
        {
            get
            {
                try
                {
                    return visibilityService ??= new Alternet.Maui.KeyboardVisibilityService();
                }
                catch
                {
                    visibilityService = new PlessKeyboardVisibilityService();
                    return visibilityService;
                }
            }
        }

        /// <summary>
        /// Converts event arguments from <see cref="CharacterReceivedRoutedEventArgs"/> to
        /// <see cref="Alternet.UI.KeyPressEventArgs"/>.
        /// </summary>
        /// <param name="control">Target control.</param>
        /// <param name="e">Event arguments.</param>
        /// <returns></returns>
        public virtual Alternet.UI.KeyPressEventArgs Convert(
            AbstractControl control,
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
        public virtual Alternet.UI.KeyEventArgs Convert(AbstractControl control, KeyRoutedEventArgs e)
        {
            var alternetKey = Convert(e.Key);

            KeyStates keyStates = e.KeyStatus.WasKeyDown ? KeyStates.Down : KeyStates.None;

            Alternet.UI.KeyEventArgs result = new(
                control,
                alternetKey,
                keyStates,
                Keyboard.Modifiers,
                e.KeyStatus.RepeatCount);

            return result;
        }

        /// <inheritdoc/>
        public override void RegisterKeyMappings()
        {
#pragma warning disable
            Add(Windows.System.VirtualKey.None, Alternet.UI.Key.None);

            Add(Windows.System.VirtualKey.Back, Alternet.UI.Key.Back);
            Add(Windows.System.VirtualKey.Tab, Alternet.UI.Key.Tab);
            Add(Windows.System.VirtualKey.Clear, Alternet.UI.Key.Clear);
            Add(Windows.System.VirtualKey.Enter, Alternet.UI.Key.Enter);
            Add(Windows.System.VirtualKey.Shift, Alternet.UI.Key.Shift);
            Add(Windows.System.VirtualKey.Control, Alternet.UI.Key.Control);
            Add(Windows.System.VirtualKey.Menu, Alternet.UI.Key.Alt);
            Add(Windows.System.VirtualKey.Pause, Alternet.UI.Key.Pause);
            Add(Windows.System.VirtualKey.CapitalLock, Alternet.UI.Key.CapsLock);

            Add(Windows.System.VirtualKey.Space, Alternet.UI.Key.Space);
            Add(Windows.System.VirtualKey.PageUp, Alternet.UI.Key.PageUp);
            Add(Windows.System.VirtualKey.PageDown, Alternet.UI.Key.PageDown);
            Add(Windows.System.VirtualKey.End, Alternet.UI.Key.End);
            Add(Windows.System.VirtualKey.Home, Alternet.UI.Key.Home);
            Add(Windows.System.VirtualKey.Left, Alternet.UI.Key.Left);
            Add(Windows.System.VirtualKey.Up, Alternet.UI.Key.Up);
            Add(Windows.System.VirtualKey.Right, Alternet.UI.Key.Right);
            Add(Windows.System.VirtualKey.Down, Alternet.UI.Key.Down);

            Add(Windows.System.VirtualKey.Insert, Alternet.UI.Key.Insert);
            Add(Windows.System.VirtualKey.Delete, Alternet.UI.Key.Delete);

            Add(Windows.System.VirtualKey.Number0, Alternet.UI.Key.D0);
            Add(Windows.System.VirtualKey.Number1, Alternet.UI.Key.D1);
            Add(Windows.System.VirtualKey.Number2, Alternet.UI.Key.D2);
            Add(Windows.System.VirtualKey.Number3, Alternet.UI.Key.D3);
            Add(Windows.System.VirtualKey.Number4, Alternet.UI.Key.D4);
            Add(Windows.System.VirtualKey.Number5, Alternet.UI.Key.D5);
            Add(Windows.System.VirtualKey.Number6, Alternet.UI.Key.D6);
            Add(Windows.System.VirtualKey.Number7, Alternet.UI.Key.D7);
            Add(Windows.System.VirtualKey.Number8, Alternet.UI.Key.D8);
            Add(Windows.System.VirtualKey.Number9, Alternet.UI.Key.D9);
            Add(Windows.System.VirtualKey.A, Alternet.UI.Key.A);
            Add(Windows.System.VirtualKey.B, Alternet.UI.Key.B);
            Add(Windows.System.VirtualKey.C, Alternet.UI.Key.C);
            Add(Windows.System.VirtualKey.D, Alternet.UI.Key.D);
            Add(Windows.System.VirtualKey.E, Alternet.UI.Key.E);
            Add(Windows.System.VirtualKey.F, Alternet.UI.Key.F);
            Add(Windows.System.VirtualKey.G, Alternet.UI.Key.G);
            Add(Windows.System.VirtualKey.H, Alternet.UI.Key.H);
            Add(Windows.System.VirtualKey.I, Alternet.UI.Key.I);
            Add(Windows.System.VirtualKey.J, Alternet.UI.Key.J);
            Add(Windows.System.VirtualKey.K, Alternet.UI.Key.K);
            Add(Windows.System.VirtualKey.L, Alternet.UI.Key.L);
            Add(Windows.System.VirtualKey.M, Alternet.UI.Key.M);
            Add(Windows.System.VirtualKey.N, Alternet.UI.Key.N);
            Add(Windows.System.VirtualKey.O, Alternet.UI.Key.O);
            Add(Windows.System.VirtualKey.P, Alternet.UI.Key.P);
            Add(Windows.System.VirtualKey.Q, Alternet.UI.Key.Q);
            Add(Windows.System.VirtualKey.R, Alternet.UI.Key.R);
            Add(Windows.System.VirtualKey.S, Alternet.UI.Key.S);
            Add(Windows.System.VirtualKey.T, Alternet.UI.Key.T);
            Add(Windows.System.VirtualKey.U, Alternet.UI.Key.U);
            Add(Windows.System.VirtualKey.V, Alternet.UI.Key.V);
            Add(Windows.System.VirtualKey.W, Alternet.UI.Key.W);
            Add(Windows.System.VirtualKey.X, Alternet.UI.Key.X);
            Add(Windows.System.VirtualKey.Y, Alternet.UI.Key.Y);
            Add(Windows.System.VirtualKey.Z, Alternet.UI.Key.Z);

            Add(Windows.System.VirtualKey.LeftWindows, Alternet.UI.Key.Windows);
            Add(Windows.System.VirtualKey.RightWindows, Alternet.UI.Key.Windows);

            Add(Windows.System.VirtualKey.NumberPad0, Alternet.UI.Key.NumPad0);
            Add(Windows.System.VirtualKey.NumberPad1, Alternet.UI.Key.NumPad1);
            Add(Windows.System.VirtualKey.NumberPad2, Alternet.UI.Key.NumPad2);
            Add(Windows.System.VirtualKey.NumberPad3, Alternet.UI.Key.NumPad3);
            Add(Windows.System.VirtualKey.NumberPad4, Alternet.UI.Key.NumPad4);
            Add(Windows.System.VirtualKey.NumberPad5, Alternet.UI.Key.NumPad5);
            Add(Windows.System.VirtualKey.NumberPad6, Alternet.UI.Key.NumPad6);
            Add(Windows.System.VirtualKey.NumberPad7, Alternet.UI.Key.NumPad7);
            Add(Windows.System.VirtualKey.NumberPad8, Alternet.UI.Key.NumPad8);
            Add(Windows.System.VirtualKey.NumberPad9, Alternet.UI.Key.NumPad9);

            Add(Windows.System.VirtualKey.Multiply, Alternet.UI.Key.Asterisk);
            Add(Windows.System.VirtualKey.Add, Alternet.UI.Key.PlusSign);
            Add(Windows.System.VirtualKey.Subtract, Alternet.UI.Key.Minus);
            Add(Windows.System.VirtualKey.Decimal, Alternet.UI.Key.Period);
            Add(Windows.System.VirtualKey.Divide, Alternet.UI.Key.Slash);

            Add(Windows.System.VirtualKey.F1, Alternet.UI.Key.F1);
            Add(Windows.System.VirtualKey.F2, Alternet.UI.Key.F2);
            Add(Windows.System.VirtualKey.F3, Alternet.UI.Key.F3);
            Add(Windows.System.VirtualKey.F4, Alternet.UI.Key.F4);
            Add(Windows.System.VirtualKey.F5, Alternet.UI.Key.F5);
            Add(Windows.System.VirtualKey.F6, Alternet.UI.Key.F6);
            Add(Windows.System.VirtualKey.F7, Alternet.UI.Key.F7);
            Add(Windows.System.VirtualKey.F8, Alternet.UI.Key.F8);
            Add(Windows.System.VirtualKey.F9, Alternet.UI.Key.F9);
            Add(Windows.System.VirtualKey.F10, Alternet.UI.Key.F10);
            Add(Windows.System.VirtualKey.F11, Alternet.UI.Key.F11);
            Add(Windows.System.VirtualKey.F12, Alternet.UI.Key.F12);
            Add(Windows.System.VirtualKey.F13, Alternet.UI.Key.F13);
            Add(Windows.System.VirtualKey.F14, Alternet.UI.Key.F14);
            Add(Windows.System.VirtualKey.F15, Alternet.UI.Key.F15);
            Add(Windows.System.VirtualKey.F16, Alternet.UI.Key.F16);
            Add(Windows.System.VirtualKey.F17, Alternet.UI.Key.F17);
            Add(Windows.System.VirtualKey.F18, Alternet.UI.Key.F18);
            Add(Windows.System.VirtualKey.F19, Alternet.UI.Key.F19);
            Add(Windows.System.VirtualKey.F20, Alternet.UI.Key.F20);
            Add(Windows.System.VirtualKey.F21, Alternet.UI.Key.F21);
            Add(Windows.System.VirtualKey.F22, Alternet.UI.Key.F22);
            Add(Windows.System.VirtualKey.F23, Alternet.UI.Key.F23);
            Add(Windows.System.VirtualKey.F24, Alternet.UI.Key.F24);

            Add(Windows.System.VirtualKey.Print, Alternet.UI.Key.PrintScreen);
            Add(Windows.System.VirtualKey.Help, Alternet.UI.Key.F1);

            Add(Windows.System.VirtualKey.NumberKeyLock, Alternet.UI.Key.NumLock);
            Add(Windows.System.VirtualKey.Scroll, Alternet.UI.Key.ScrollLock);

            Add(Windows.System.VirtualKey.GoBack, Alternet.UI.Key.BrowserBack);
            Add(Windows.System.VirtualKey.GoForward, Alternet.UI.Key.BrowserForward);
            Add(Windows.System.VirtualKey.Refresh, Alternet.UI.Key.BrowserRefresh);
            Add(Windows.System.VirtualKey.Stop, Alternet.UI.Key.BrowserStop);
            Add(Windows.System.VirtualKey.Search, Alternet.UI.Key.BrowserSearch);
            Add(Windows.System.VirtualKey.Favorites, Alternet.UI.Key.BrowserFavorites);
            Add(Windows.System.VirtualKey.GoHome, Alternet.UI.Key.BrowserHome);

            Add(Windows.System.VirtualKey.GamepadA, Alternet.UI.Key.GamepadA);
            Add(Windows.System.VirtualKey.GamepadB, Alternet.UI.Key.GamepadB);
            Add(Windows.System.VirtualKey.GamepadX, Alternet.UI.Key.GamepadX);
            Add(Windows.System.VirtualKey.GamepadY, Alternet.UI.Key.GamepadY);
            
            Add(
                Windows.System.VirtualKey.GamepadRightShoulder, Alternet.UI.Key.GamepadRightShoulder);
            
            Add(Windows.System.VirtualKey.GamepadLeftShoulder, Alternet.UI.Key.GamepadLeftShoulder);
            Add(Windows.System.VirtualKey.GamepadLeftTrigger, Alternet.UI.Key.GamepadLeftTrigger);
            Add(Windows.System.VirtualKey.GamepadRightTrigger, Alternet.UI.Key.GamepadRightTrigger);
            Add(Windows.System.VirtualKey.GamepadDPadUp, Alternet.UI.Key.GamepadDPadUp);
            Add(Windows.System.VirtualKey.GamepadDPadDown, Alternet.UI.Key.GamepadDPadDown);
            Add(Windows.System.VirtualKey.GamepadDPadLeft, Alternet.UI.Key.GamepadDPadLeft);
            Add(Windows.System.VirtualKey.GamepadDPadRight, Alternet.UI.Key.GamepadDPadRight);
            Add(Windows.System.VirtualKey.GamepadMenu, Alternet.UI.Key.GamepadMenu);
            Add(Windows.System.VirtualKey.GamepadView, Alternet.UI.Key.GamepadView);
            Add(Windows.System.VirtualKey.GamepadLeftThumbstickButton, Key.GamepadLeftThumbstickButton);
            Add(Windows.System.VirtualKey.GamepadRightThumbstickButton, Key.GamepadRightThumbstickButton);
            Add(Windows.System.VirtualKey.GamepadLeftThumbstickUp, Alternet.UI.Key.GamepadLeftThumbstickUp);
            Add(Windows.System.VirtualKey.GamepadLeftThumbstickDown, Key.GamepadLeftThumbstickDown);
            Add(Windows.System.VirtualKey.GamepadLeftThumbstickRight, Key.GamepadLeftThumbstickRight);
            Add(Windows.System.VirtualKey.GamepadLeftThumbstickLeft, Key.GamepadLeftThumbstickLeft);
            Add(Windows.System.VirtualKey.GamepadRightThumbstickUp, Key.GamepadRightThumbstickUp);
            Add(Windows.System.VirtualKey.GamepadRightThumbstickDown, Key.GamepadRightThumbstickDown);
            Add(Windows.System.VirtualKey.GamepadRightThumbstickRight, Key.GamepadRightThumbstickRight);
            Add(Windows.System.VirtualKey.GamepadRightThumbstickLeft, Key.GamepadRightThumbstickLeft);

            Add(Windows.System.VirtualKey.Escape, Alternet.UI.Key.Escape);

            // Add(Windows.System.VirtualKey.LeftButton     , Alternet.UI.Key.LeftButton     );
            // Add(Windows.System.VirtualKey.RightButton    , Alternet.UI.Key.RightButton    );
            // Add(Windows.System.VirtualKey.Cancel         , Alternet.UI.Key.Cancel         );
            // Add(Windows.System.VirtualKey.MiddleButton   , Alternet.UI.Key.MiddleButton   );
            // Add(Windows.System.VirtualKey.XButton1       , Alternet.UI.Key.XButton1       );
            // Add(Windows.System.VirtualKey.XButton2       , Alternet.UI.Key.XButton2       );

            Add(Windows.System.VirtualKey.Kana           , Alternet.UI.Key.Kana           );
            Add(Windows.System.VirtualKey.Hangul         , Alternet.UI.Key.Hangul         );
            Add(Windows.System.VirtualKey.Junja          , Alternet.UI.Key.Junja          );
            Add(Windows.System.VirtualKey.Final          , Alternet.UI.Key.Final          );
            Add(Windows.System.VirtualKey.Hanja          , Alternet.UI.Key.Hanja          );
            Add(Windows.System.VirtualKey.Kanji          , Alternet.UI.Key.Kanji          );

            Add(Windows.System.VirtualKey.Convert        , Alternet.UI.Key.Convert        );
            Add(Windows.System.VirtualKey.NonConvert     , Alternet.UI.Key.NonConvert     );
            Add(Windows.System.VirtualKey.Accept         , Alternet.UI.Key.Accept         );
            Add(Windows.System.VirtualKey.ModeChange     , Alternet.UI.Key.ModeChange     );

            Add(Windows.System.VirtualKey.Select         , Alternet.UI.Key.Select         );
            Add(Windows.System.VirtualKey.Execute        , Alternet.UI.Key.Execute        );
            Add(Windows.System.VirtualKey.Snapshot       , Alternet.UI.Key.Snapshot);

            Add(Windows.System.VirtualKey.Application    , Alternet.UI.Key.Menu    );
            Add(Windows.System.VirtualKey.Sleep          , Alternet.UI.Key.Sleep          );

            Add(Windows.System.VirtualKey.Separator      , Alternet.UI.Key.Comma      );

            Add(Windows.System.VirtualKey.NavigationView , Alternet.UI.Key.NavigationView);
            Add(Windows.System.VirtualKey.NavigationMenu , Alternet.UI.Key.NavigationMenu );
            Add(Windows.System.VirtualKey.NavigationUp   , Alternet.UI.Key.NavigationUp   );
            Add(Windows.System.VirtualKey.NavigationDown , Alternet.UI.Key.NavigationDown );
            Add(Windows.System.VirtualKey.NavigationLeft , Alternet.UI.Key.NavigationLeft );
            Add(Windows.System.VirtualKey.NavigationRight, Alternet.UI.Key.NavigationRight);
            Add(Windows.System.VirtualKey.NavigationAccept, Alternet.UI.Key.NavigationAccept);
            Add(Windows.System.VirtualKey.NavigationCancel, Alternet.UI.Key.NavigationCancel);
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
            /*
            There is no need to get left and right modifiers state
            as there is key code for modifiers without separation on left/right.
            */

            return Fn(Convert(key));

            KeyStates Fn(Windows.System.VirtualKey key)
            {
                var keyState = Microsoft.UI.Input.InputKeyboardSource.GetKeyStateForCurrentThread(key);
                return Convert(keyState);
            }
        }

        /// <inheritdoc/>
        public override bool HideKeyboard(AbstractControl? control)
        {
            return HideKeyboard();
        }

        /// <inheritdoc/>
        public override bool IsSoftKeyboardShowing(AbstractControl? control)
        {
            return IsSoftKeyboardShowing();
        }

        /// <inheritdoc/>
        public override bool ShowKeyboard(AbstractControl? control)
        {
            return ShowKeyboard();
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            SafeDispose(ref visibilityService);
            base.DisposeManaged();
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