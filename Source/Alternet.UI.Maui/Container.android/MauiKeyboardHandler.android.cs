using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Platform;

#if ANDROID

using Android.App;
using Android.Content;
using Android.Views;
using Android.Views.InputMethods;
using AndroidX.Core.View;

namespace Alternet.UI
{
    /// <summary>
    /// Implements <see cref="UI.IKeyboardHandler"/> for MAUI platform under Android.
    /// </summary>
    public class MauiKeyboardHandler : UI.PlatformKeyboardHandler<Keycode>
    {
        public const Keycode MaxKeyValue = Keycode.Macro4;

        /// <summary>
        /// Gets or sets default <see cref="UI.IKeyboardHandler"/> implementation.
        /// </summary>
        public static MauiKeyboardHandler Default = new();

        private IKeyboardVisibilityService? visibilityService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MauiKeyboardHandler"/> class.
        /// </summary>
        public MauiKeyboardHandler()
            : base(MaxKeyValue, UI.Key.Max)
        {
        }

        /// <summary>
        /// Gets input method manager.
        /// </summary>
        public static InputMethodManager? InputMethodManager =>
            Android.App.Application.Context
            .GetSystemService(Context.InputMethodService) as InputMethodManager;

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

        public virtual Alternet.UI.KeyEventArgs ToKeyEventArgs(
                    UI.AbstractControl control,
                    UI.KeyStates keyStates,
                    Keycode keyCode,
                    KeyEvent e)
        {
            var key = Convert(keyCode);

            UI.ModifierKeys modifiers;
            int repeatCount;

            modifiers = Convert(e.MetaState);

            repeatCount = e.RepeatCount;

            if (repeatCount < 0)
                repeatCount = 0;

            Alternet.UI.KeyEventArgs result = new(
                control,
                key,
                keyStates,
                modifiers,
                (uint)repeatCount);

            return result;
        }

        public virtual UI.ModifierKeys Convert(MetaKeyStates states)
        {
            var shiftPressed = (states & MetaKeyStates.ShiftMask) != 0;
            var altPressed = (states & MetaKeyStates.AltMask) != 0;
            var controlPressed = (states & MetaKeyStates.CtrlMask) != 0;
            UI.ModifierKeys result = UI.ModifierKeys.None;
            if (shiftPressed)
                result |= UI.ModifierKeys.Shift;
            if (altPressed)
                result |= UI.ModifierKeys.Alt;
            if (controlPressed)
                result |= UI.ModifierKeys.Control;
            return result;
        }

        /// <inheritdoc/>
        public override void RegisterKeyMappings()
        {
            Add(Keycode.Back, null);
            Add(Keycode.SoftLeft, null);
            Add(Keycode.SoftRight, null);
            Add(Keycode.MetaLeft, null);
            Add(Keycode.MetaRight, null);

            Add(Keycode.Num0, Key.D0);
            Add(Keycode.Num1, Key.D1);
            Add(Keycode.Num2, Key.D2);
            Add(Keycode.Num3, Key.D3);
            Add(Keycode.Num4, Key.D4);
            Add(Keycode.Num5, Key.D5);
            Add(Keycode.Num6, Key.D6);
            Add(Keycode.Num7, Key.D7);
            Add(Keycode.Num8, Key.D8);
            Add(Keycode.Num9, Key.D9);

            // '*' key.
            Add(Keycode.Star, Key.Asterisk);

            // '#' key.
            Add(Keycode.Pound, Key.NumberSign);

            // Volume Up key. Adjusts the speaker volume up.
            Add(Keycode.VolumeUp, Key.VolumeUp);

            // Volume Down key. Adjusts the speaker volume down.
            Add(Keycode.VolumeDown, Key.VolumeDown);

            // Power key.
            Add(Keycode.Power, Key.Power);

            // Clear key.
            Add(Keycode.Clear, Key.Clear);

            Add(Keycode.A, Key.A);
            Add(Keycode.B, Key.B);
            Add(Keycode.C, Key.C);
            Add(Keycode.D, Key.D);
            Add(Keycode.E, Key.E);
            Add(Keycode.F, Key.F);
            Add(Keycode.G, Key.G);
            Add(Keycode.H, Key.H);
            Add(Keycode.I, Key.I);
            Add(Keycode.J, Key.J);
            Add(Keycode.K, Key.K);
            Add(Keycode.L, Key.L);
            Add(Keycode.M, Key.M);
            Add(Keycode.N, Key.N);
            Add(Keycode.O, Key.O);
            Add(Keycode.P, Key.P);
            Add(Keycode.Q, Key.Q);
            Add(Keycode.R, Key.R);
            Add(Keycode.S, Key.S);
            Add(Keycode.T, Key.T);
            Add(Keycode.U, Key.U);
            Add(Keycode.V, Key.V);
            Add(Keycode.W, Key.W);
            Add(Keycode.X, Key.X);
            Add(Keycode.Y, Key.Y);
            Add(Keycode.Z, Key.Z);

            // ',' key.
            Add(Keycode.Comma, Key.Comma);

            // '.' key.
            Add(Keycode.Period, Key.Period);

            AddOneWay(Keycode.AltLeft, Key.Alt);
            AddOneWay(Keycode.AltRight, Key.Alt);
            AddOneWay(Keycode.ShiftLeft, Key.Shift);
            AddOneWay(Keycode.ShiftRight, Key.Shift);

            // Tab key.
            Add(Keycode.Tab, Key.Tab);

            // Space key.
            Add(Keycode.Space, Key.Space);

            // Envelope special function key. Used to launch a mail application.
            Add(Keycode.Envelope, Key.LaunchMail);

            // Enter key.
            Add(Keycode.Enter, Key.Enter);

            // Backspace key. Deletes characters before the insertion point,
            // unlike Android.Views.Keycode.ForwardDel.
            Add(Keycode.Del, Key.Backspace);

            // '`' (backtick) key.
            Add(Keycode.Grave, Key.Backtick);

            // '-'.
            Add(Keycode.Minus, Key.Minus);

            // '=' key.
            Add(Keycode.Equals, Key.Equals);

            // '[' key.
            Add(Keycode.LeftBracket, Key.OpenBracket);

            // ']' key.
            Add(Keycode.RightBracket, Key.CloseBracket);

            // '\' key.
            Add(Keycode.Backslash, Key.Backslash);

            // ';' key.
            Add(Keycode.Semicolon, Key.Semicolon);

            // ''' (apostrophe) key.
            Add(Keycode.Apostrophe, Key.Quote);

            // '/' key.
            Add(Keycode.Slash, Key.Slash);

            // '@' key.
            Add(Keycode.At, Key.CommercialAt);

            // '+' key.
            Add(Keycode.Plus, Key.PlusSign);

            // Menu key.
            Add(Keycode.Menu, Key.Menu);

            // Search key.
            Add(Keycode.Search, Key.BrowserSearch);

            // Play/Pause media key.
            Add(Keycode.MediaPlayPause, Key.MediaPlayPause);

            // Stop media key.
            Add(Keycode.MediaStop, Key.MediaStop);

            // Play Next media key.
            Add(Keycode.MediaNext, Key.MediaNextTrack);

            // Play Previous media key.
            Add(Keycode.MediaPrevious, Key.MediaPreviousTrack);

            // Page Up key.
            Add(Keycode.PageUp, Key.PageUp);

            // Page Down key.
            Add(Keycode.PageDown, Key.PageDown);

            // A Button key. On a game controller, the A button should be
            // either the button labeled A or the first button on the bottom row of controller
            // buttons.
            Add(Keycode.ButtonA, Key.GamepadA);

            // B Button key. On a game controller, the B button should be
            // either the button labeled B or the second button on the bottom row of controller
            // buttons.
            Add(Keycode.ButtonB, Key.GamepadB);

            // X Button key. On a game controller, the X button should be
            // either the button labeled X or the first button on the upper row of controller
            // buttons.
            Add(Keycode.ButtonX, Key.GamepadX);

            // Y Button key. On a game controller, the Y button should be
            // either the button labeled Y or the second button on the upper row of controller
            // buttons.
            Add(Keycode.ButtonY, Key.GamepadY);

            // L1 Button key. On a game controller, the L1 button should
            // be either the button labeled L1 (or L) or the top left trigger button.
            Add(Keycode.ButtonL1, null);

            // R1 Button key. On a game controller, the R1 button should
            // be either the button labeled R1 (or R) or the top right trigger button.
            Add(Keycode.ButtonR1, null);

            // L2 Button key. On a game controller, the L2 button should
            // be either the button labeled L2 or the bottom left trigger button.
            Add(Keycode.ButtonL2, null);

            // R2 Button key. On a game controller, the R2 button should
            // be either the button labeled R2 or the bottom right trigger button.
            Add(Keycode.ButtonR2, null);

            // Left Thumb Button key. On a game controller, the left thumb
            // button indicates that the left (or only) joystick is pressed.
            Add(Keycode.ButtonThumbl, null);

            // Right Thumb Button key. On a game controller, the right thumb
            // button indicates that the right joystick is pressed.
            Add(Keycode.ButtonThumbr, null);

            // Start Button key. On a game controller, the button labeled Start.
            Add(Keycode.ButtonStart, null);

            // Select Button key. On a game controller, the button labeled Select.
            Add(Keycode.ButtonSelect, null);

            // Mode Button key. On a game controller, the button labeled
            // Mode.
            Add(Keycode.ButtonMode, null);

            // Escape key.
            Add(Keycode.Escape, Key.Escape);

            // Forward Delete key. Deletes characters ahead of the insertion
            // point, unlike Android.Views.Keycode.Del.
            Add(Keycode.ForwardDel, Key.Delete);

            // Left Control modifier key.
            AddOneWay(Keycode.CtrlLeft, Key.Control);

            // Right Control modifier key.
            AddOneWay(Keycode.CtrlRight, Key.Control);

            // Caps Lock key.
            Add(Keycode.CapsLock, Key.CapsLock);

            // Scroll Lock key.
            Add(Keycode.ScrollLock, Key.ScrollLock);

            // System Request / Print Screen key.
            Add(Keycode.Sysrq, Key.PrintScreen);

            // Break / Pause key.
            Add(Keycode.Break, Key.Pause);

            // Home Movement key. Used for scrolling or moving the cursor
            // around to the start of a line or to the top of a list.
            Add(Keycode.MoveHome, Key.Home);

            // End Movement key. Used for scrolling or moving the cursor
            // around to the end of a line or to the bottom of a list.
            Add(Keycode.MoveEnd, Key.End);

            // Insert key. Toggles insert / overwrite edit mode.
            Add(Keycode.Insert, Key.Insert);

            // Forward key. Navigates forward in the history stack.
            Add(Keycode.Forward, Key.BrowserForward);

            Add(Keycode.F1, Key.F1);
            Add(Keycode.F2, Key.F2);
            Add(Keycode.F3, Key.F3);
            Add(Keycode.F4, Key.F4);
            Add(Keycode.F5, Key.F5);
            Add(Keycode.F6, Key.F6);
            Add(Keycode.F7, Key.F7);
            Add(Keycode.F8, Key.F8);
            Add(Keycode.F9, Key.F9);
            Add(Keycode.F10, Key.F10);
            Add(Keycode.F11, Key.F11);
            Add(Keycode.F12, Key.F12);

            // Num Lock key. This is the Num Lock key; it is different from
            // Android.Views.Keycode.Num. This key alters the behavior of other keys on the
            // numeric keypad.
            Add(Keycode.NumLock, Key.NumLock);

            Add(Keycode.Numpad0, Key.NumPad0);
            Add(Keycode.Numpad1, Key.NumPad1);
            Add(Keycode.Numpad2, Key.NumPad2);
            Add(Keycode.Numpad3, Key.NumPad3);
            Add(Keycode.Numpad4, Key.NumPad4);
            Add(Keycode.Numpad5, Key.NumPad5);
            Add(Keycode.Numpad6, Key.NumPad6);
            Add(Keycode.Numpad7, Key.NumPad7);
            Add(Keycode.Numpad8, Key.NumPad8);
            Add(Keycode.Numpad9, Key.NumPad9);

            // Numeric keypad '/' key (for division).
            Add(Keycode.NumpadDivide, Key.NumPadSlash);

            // Numeric keypad '*' key (for multiplication).
            Add(Keycode.NumpadMultiply, Key.NumPadStar);

            // Numeric keypad '-' key (for subtraction).
            Add(Keycode.NumpadSubtract, Key.NumPadMinus);

            // Numeric keypad '+' key (for addition).
            Add(Keycode.NumpadAdd, Key.NumPadPlus);

            // Numeric keypad '.' key (for decimals or digit grouping).
            Add(Keycode.NumpadDot, Key.NumPadDot);

            // Numeric keypad ',' key (for decimals or digit grouping).
            AddOneWay(Keycode.NumpadComma, Key.Comma);

            // Numeric keypad Enter key.
            AddOneWay(Keycode.NumpadEnter, Key.Enter);

            // Numeric keypad '=' key.
            AddOneWay(Keycode.NumpadEquals, Key.Equals);

            Add(Keycode.VolumeMute, Key.VolumeMute);

            // Numeric keypad '(' key.
            Add(Keycode.NumpadLeftParen, Key.NumPadLeftParen);

            // Numeric keypad ')' key.
            Add(Keycode.NumpadRightParen, Key.NumPadRightParen);

            // C Button key. On a game controller, the C button should be
            // either the button labeled C or the third button on the bottom row of controller
            // buttons.
            Add(Keycode.ButtonC, Key.GamepadC);

            // Z Button key. On a game controller, the Z button should be
            // either the button labeled Z or the third button on the upper row of controller
            // buttons.
            Add(Keycode.ButtonZ, Key.GamepadZ);

            // Function modifier key.
            Add(Keycode.Function, Key.Function);

            // Info key. Common on TV remotes to show additional information
            // related to what is currently being viewed.
            Add(Keycode.Info, Key.TVInfo);

            // Channel up key. On TV remotes, increments the television channel.
            Add(Keycode.ChannelUp, Key.TVChannelUp);

            // Channel down key. On TV remotes, decrements the television
            // channel.
            Add(Keycode.ChannelDown, Key.TVChannelDown);

            // Rewind media key.
            Add(Keycode.MediaRewind, Key.MediaRewind);

            // Fast Forward media key.
            Add(Keycode.MediaFastForward, Key.MediaFastForward);

            // Symbol modifier key. Used to enter alternate symbols.
            Add(Keycode.Sym, Key.Sym);

            // Explorer special function key. Used to launch a browser application.
            Add(Keycode.Explorer, Key.LaunchBrowser);

            // Mute key. Mutes the microphone, unlike Android.Views.Keycode.VolumeMute.
            Add(Keycode.Mute, Key.Mute);

            // Picture Symbols modifier key. Used to switch symbol sets (Emoji, Kao-moji).
            Add(Keycode.Pictsymbols, Key.Pictsymbols);

            // Switch Charset modifier key. Used to switch character sets
            // (Kanji, Katakana).
            Add(Keycode.SwitchCharset, Key.SwitchCharset);

            // Zoom in key.
            Add(Keycode.ZoomIn, Key.ZoomIn);

            // Zoom out key.
            Add(Keycode.ZoomOut, Key.ZoomOut);

            Add(Keycode.Num, Key.Num);

            // Headset Hook key. Used to hang up calls and stop media.
            Add(Keycode.Headsethook, Key.Headsethook);

            // Camera Focus key. Used to focus the camera.
            Add(Keycode.Focus, Key.Focus);

            // Notification key.
            Add(Keycode.Notification, Key.Notification);

            Add(Keycode.Call, Key.Call);
            Add(Keycode.Endcall, Key.Endcall);

            // Directional Pad Up key. May also be synthesized from trackball motions.
            Add(Keycode.DpadUp, Key.UpArrow);

            // Directional Pad Down key. May also be synthesized from trackball motions.
            Add(Keycode.DpadDown, Key.DownArrow);

            // Directional Pad Left key. May also be synthesized from trackball motions.
            Add(Keycode.DpadLeft, Key.LeftArrow);

            // Directional Pad Right key. May also be synthesized from trackball motions.
            Add(Keycode.DpadRight, Key.RightArrow);

            // Directional Pad Center key. May also be synthesized from trackball motions.
            Add(Keycode.DpadCenter, Key.DpadCenter);

            // Camera key. Used to launch a camera application or take pictures.
            Add(Keycode.Camera, Key.Camera);

            // TV key. On TV remotes, switches to viewing live TV.
            Add(Keycode.Tv, Key.TvViewLive);

            // Play media key.
            Add(Keycode.MediaPlay, Key.MediaPlay);

            // Pause media key.
            Add(Keycode.MediaPause, Key.MediaPause);

            // Close media key. May be used to close a CD tray, for example.
            Add(Keycode.MediaClose, Key.MediaClose);

            // Eject media key. May be used to eject a CD tray, for example.
            Add(Keycode.MediaEject, Key.MediaEject);

            // Record media key.
            Add(Keycode.MediaRecord, Key.MediaRecord);

            // Window key. On TV remotes, toggles picture-in-picture mode
            // or other windowing functions.
            Add(Keycode.Window, Key.TVWindow);

            // Guide key. On TV remotes, shows a programming guide.
            Add(Keycode.Guide, Key.TVGuide);

            // DVR key. On some TV remotes, switches to a DVR mode for recorded
            // shows.
            Add(Keycode.Dvr, Key.TvDvr);

            // Bookmark key. On some TV remotes, bookmarks content or web
            // pages.
            Add(Keycode.Bookmark, Key.TVBookmark);

            // Toggle captions key. Switches the mode for closed-captioning
            // text, for example during television shows.
            Add(Keycode.Captions, Key.TVCaptions);

            // Settings key. Starts the system settings activity.
            Add(Keycode.Settings, Key.Settings);

            // TV power key. On TV remotes, toggles the power on a television
            // screen.
            Add(Keycode.TvPower, Key.TVPower);

            // TV input key. On TV remotes, switches the input on a television
            // screen.
            Add(Keycode.TvInput, Key.TVInput);

            // Set-top-box power key. On TV remotes, toggles the power on
            // an external Set-top-box.
            Add(Keycode.StbPower, Key.TVStbPower);

            // Set-top-box input key. On TV remotes, switches the input mode
            // on an external Set-top-box.
            Add(Keycode.StbInput, Key.TVStbInput);

            // A/V Receiver power key. On TV remotes, toggles the power on
            // an external A/V Receiver.
            Add(Keycode.AvrPower, Key.TVAvrPower);

            // A/V Receiver input key. On TV remotes, switches the input
            // mode on an external A/V Receiver.
            Add(Keycode.AvrInput, Key.TVAvrInput);

            // Red "programmable" key. On TV remotes, acts as a contextual/programmable
            // key.
            Add(Keycode.ProgRed, Key.TVProgRed);

            // Green "programmable" key. On TV remotes, actsas a contextual/programmable
            // key.
            Add(Keycode.ProgGreen, Key.TVProgGreen);

            // Yellow "programmable" key. On TV remotes, acts as a contextual/programmable
            // key.
            Add(Keycode.ProgYellow, Key.TVProgYellow);

            // Blue "programmable" key. On TV remotes, acts as a contextual/programmable
            // key.
            Add(Keycode.ProgBlue, Key.TVProgBlue);

            // App switch key. Should bring up the application switcher dialog.
            Add(Keycode.AppSwitch, Key.AppSwitch);

            // Generic Game Pad Buttons
            Add(Keycode.Button1, Key.GamepadButton1);
            Add(Keycode.Button2, Key.GamepadButton2);
            Add(Keycode.Button3, Key.GamepadButton3);
            Add(Keycode.Button4, Key.GamepadButton4);
            Add(Keycode.Button5, Key.GamepadButton5);
            Add(Keycode.Button6, Key.GamepadButton6);
            Add(Keycode.Button7, Key.GamepadButton7);
            Add(Keycode.Button8, Key.GamepadButton8);
            Add(Keycode.Button9, Key.GamepadButton9);
            Add(Keycode.Button10, Key.GamepadButton10);
            Add(Keycode.Button11, Key.GamepadButton11);
            Add(Keycode.Button12, Key.GamepadButton12);
            Add(Keycode.Button13, Key.GamepadButton13);
            Add(Keycode.Button14, Key.GamepadButton14);
            Add(Keycode.Button15, Key.GamepadButton15);
            Add(Keycode.Button16, Key.GamepadButton16);

            // Language Switch key. Toggles the current input language such
            // as switching between English and Japanese on a QWERTY keyboard.
            Add(Keycode.LanguageSwitch, Key.LanguageSwitch);

            // Manner Mode key. Toggles silent or vibrate mode on and off
            // to make the device behave more politely in certain settings such as on a crowded
            // train.
            Add(Keycode.MannerMode, Key.MannerMode);

            // 3D Mode key. Toggles the display between 2D and 3D mode.
            Add(Keycode.ThreeDMode, Key.ThreeDMode);

            // Contacts special function key. Used to launch an address book
            // application.
            Add(Keycode.Contacts, Key.Contacts);

            // Calendar special function key. Used to launch a calendar application.
            Add(Keycode.Calendar, Key.Calendar);

            // Music special function key. Used to launch a music player
            // application.
            Add(Keycode.Music, Key.Music);

            // Calculator special function key. Used to launch a calculator
            // application.
            Add(Keycode.Calculator, Key.Calculator);

            // Japanese full-width / half-width key.
            Add(Keycode.ZenkakuHankaku, Key.JapaneseZenkakuHankaku);

            // Japanese alphanumeric key.
            Add(Keycode.Eisu, Key.JapaneseEisu);

            // Japanese non-conversion key.
            Add(Keycode.Muhenkan, Key.JapaneseMuhenkan);

            // Japanese conversion key.
            Add(Keycode.Henkan, Key.JapaneseHenkan);

            // Japanese Yen key.
            Add(Keycode.Yen, Key.JapaneseYen);

            // Japanese Ro key.
            Add(Keycode.Ro, Key.JapaneseRo);

            // Assist key. Launches the global assist activity.
            Add(Keycode.Assist, Key.Assist);

            // Brightness Down key. Adjusts the screen brightness down.
            Add(Keycode.BrightnessDown, Key.BrightnessDown);

            // Brightness Up key. Adjusts the screen brightness up.
            Add(Keycode.BrightnessUp, Key.BrightnessUp);

            // Japanese katakana / hiragana key.
            Add(Keycode.KatakanaHiragana, Key.Katakana);

            // Japanese kana key.
            Add(Keycode.Kana, Key.Kana);

            Add(Keycode.Sleep, Key.Sleep);
            Add(Keycode.Cut, Key.Cut);
            Add(Keycode.Copy, Key.Copy);
            Add(Keycode.Paste, Key.Paste);
            Add(Keycode.Help, Key.Help);

            Add(Keycode.MediaAudioTrack, Key.MediaAudioTrack);
            Add(Keycode.Wakeup, Key.Wakeup);
            Add(Keycode.Pairing, Key.Pairing);
            Add(Keycode.MediaTopMenu, Key.MediaTopMenu);
            Add(Keycode.K11, Key.K11);
            Add(Keycode.K12, Key.K12);
            Add(Keycode.LastChannel, Key.LastChannel);
            Add(Keycode.TvDataService, Key.TvDataService);
            Add(Keycode.VoiceAssist, Key.VoiceAssist);
            Add(Keycode.TvRadioService, Key.TvRadioService);
            Add(Keycode.TvTeletext, Key.TvTeletext);
            Add(Keycode.TvNumberEntry, Key.TvNumberEntry);
            Add(Keycode.TvTerrestrialAnalog, Key.TvTerrestrialAnalog);
            Add(Keycode.TvTerrestrialDigital, Key.TvTerrestrialDigital);
            Add(Keycode.TvSatellite, Key.TvSatellite);
            Add(Keycode.TvSatelliteBs, Key.TvSatelliteBs);
            Add(Keycode.TvSatelliteCs, Key.TvSatelliteCs);
            Add(Keycode.TvSatelliteService, Key.TvSatelliteService);
            Add(Keycode.TvNetwork, Key.TvNetwork);
            Add(Keycode.TvAntennaCable, Key.TvAntennaCable);
            Add(Keycode.TvInputHdmi1, Key.TvInputHdmi1);
            Add(Keycode.TvInputHdmi2, Key.TvInputHdmi2);
            Add(Keycode.TvInputHdmi3, Key.TvInputHdmi3);
            Add(Keycode.TvInputHdmi4, Key.TvInputHdmi4);
            Add(Keycode.TvInputComposite1, Key.TvInputComposite1);
            Add(Keycode.TvInputComposite2, Key.TvInputComposite2);
            Add(Keycode.TvInputComponent1, Key.TvInputComponent1);
            Add(Keycode.TvInputComponent2, Key.TvInputComponent2);
            Add(Keycode.TvInputVga1, Key.TvInputVga1);
            Add(Keycode.TvAudioDescription, Key.TvAudioDescription);
            Add(Keycode.TvAudioDescriptionMixUp, Key.TvAudioDescriptionMixUp);
            Add(Keycode.TvAudioDescriptionMixDown, Key.TvAudioDescriptionMixDown);
            Add(Keycode.TvZoomMode, Key.TvZoomMode);
            Add(Keycode.TvContentsMenu, Key.TvContentsMenu);
            Add(Keycode.TvMediaContextMenu, Key.TvMediaContextMenu);
            Add(Keycode.TvTimerProgramming, Key.TvTimerProgramming);
            Add(Keycode.NavigatePrevious, Key.NavigatePrevious);
            Add(Keycode.NavigateNext, Key.NavigateNext);
            Add(Keycode.NavigateIn, Key.NavigateIn);
            Add(Keycode.NavigateOut, Key.NavigateOut);
            Add(Keycode.StemPrimary, Key.StemPrimary);
            Add(Keycode.Stem1, Key.Stem1);
            Add(Keycode.Stem2, Key.Stem2);
            Add(Keycode.Stem3, Key.Stem3);
            Add(Keycode.DpadUpLeft, Key.DpadUpLeft);
            Add(Keycode.DpadDownLeft, Key.DpadDownLeft);
            Add(Keycode.DpadUpRight, Key.DpadUpRight);
            Add(Keycode.DpadDownRight, Key.DpadDownRight);
            Add(Keycode.MediaSkipForward, Key.MediaSkipForward);
            Add(Keycode.MediaSkipBackward, Key.MediaSkipBackward);
            Add(Keycode.MediaStepForward, Key.MediaStepForward);
            Add(Keycode.MediaStepBackward, Key.MediaStepBackward);
            Add(Keycode.SoftSleep, Key.SoftSleep);
            Add(Keycode.SystemNavigationUp, Key.SystemNavigationUp);
            Add(Keycode.SystemNavigationDown, Key.SystemNavigationDown);
            Add(Keycode.SystemNavigationLeft, Key.SystemNavigationLeft);
            Add(Keycode.SystemNavigationRight, Key.SystemNavigationRight);
            Add(Keycode.AllApps, Key.AllApps);
            Add(Keycode.Refresh, Key.BrowserRefresh);
            Add(Keycode.ThumbsUp, Key.ThumbsUp);
            Add(Keycode.ThumbsDown, Key.ThumbsDown);
            Add(Keycode.ProfileSwitch, Key.ProfileSwitch);
            Add(Keycode.VideoApp1, Key.VideoApp1);
            Add(Keycode.VideoApp2, Key.VideoApp2);
            Add(Keycode.VideoApp3, Key.VideoApp3);
            Add(Keycode.VideoApp4, Key.VideoApp4);
            Add(Keycode.VideoApp5, Key.VideoApp5);
            Add(Keycode.VideoApp6, Key.VideoApp6);
            Add(Keycode.VideoApp7, Key.VideoApp7);
            Add(Keycode.VideoApp8, Key.VideoApp8);
            Add(Keycode.FeaturedApp1, Key.FeaturedApp1);
            Add(Keycode.FeaturedApp2, Key.FeaturedApp2);
            Add(Keycode.FeaturedApp3, Key.FeaturedApp3);
            Add(Keycode.FeaturedApp4, Key.FeaturedApp4);
            Add(Keycode.DemoApp1, Key.DemoApp1);
            Add(Keycode.DemoApp2, Key.DemoApp2);
            Add(Keycode.DemoApp3, Key.DemoApp3);
            Add(Keycode.DemoApp4, Key.DemoApp4);
            Add(Keycode.KeyboardBacklightDown, Key.KeyboardBacklightDown);
            Add(Keycode.KeyboardBacklightUp, Key.KeyboardBacklightUp);
            Add(Keycode.KeyboardBacklightToggle, Key.KeyboardBacklightToggle);
            Add(Keycode.StylusButtonPrimary, Key.StylusButtonPrimary);
            Add(Keycode.StylusButtonSecondary, Key.StylusButtonSecondary);
            Add(Keycode.StylusButtonTertiary, Key.StylusButtonTertiary);
            Add(Keycode.StylusButtonTail, Key.StylusButtonTail);
            Add(Keycode.RecentApps, Key.RecentApps);
            Add(Keycode.Macro1, Key.Macro1);
            Add(Keycode.Macro2, Key.Macro2);
            Add(Keycode.Macro3, Key.Macro3);
            Add(Keycode.Macro4, Key.Macro4);
        }

        /// <inheritdoc/>
        public override UI.KeyStates GetKeyStatesFromSystem(Key key)
        {
            return UI.PlessKeyboard.GetKeyStatesFromMemory(key);
        }

        /// <inheritdoc/>
        public override bool HideKeyboard(UI.AbstractControl? control)
        {
            var platformView = ControlView.GetPlatformView(control);
            if (platformView is null)
                return false;

            var focusedView = platformView.Context?.GetActivity()?.Window?.CurrentFocus;
            View tokenView = focusedView ?? platformView;

            using var inputMethodManager
                = (InputMethodManager?)tokenView.Context?.GetSystemService(Context.InputMethodService);
            var windowToken = tokenView.WindowToken;

            if (windowToken is not null && inputMethodManager is not null)
            {
                return inputMethodManager.HideSoftInputFromWindow(windowToken, HideSoftInputFlags.None);
            }

            return false;
        }

        /// <inheritdoc/>
        public override bool IsSoftKeyboardShowing(UI.AbstractControl? control)
        {
            var platformView = ControlView.GetPlatformView(control);
            if (platformView is null)
                return false;

            var insets = ViewCompat.GetRootWindowInsets(platformView);
            if (insets is null)
                return false;

            var result = insets.IsVisible(WindowInsetsCompat.Type.Ime());
            return result;
        }

        /// <inheritdoc/>
        public override bool ShowKeyboard(UI.AbstractControl? control)
        {
            var platformView = ControlView.GetPlatformView(control);
            if (platformView is null)
                return false;

            using var inputMethodManager
                = (InputMethodManager?)platformView.Context
                ?.GetSystemService(Context.InputMethodService);

            // The zero value for the second parameter comes from
            // https://developer.android.com/reference/android/view/inputmethod/InputMethodManager#showSoftInput(android.view.View,%20int)
            // Apparently there's no named value for zero in this case
            return inputMethodManager?.ShowSoftInput(platformView, 0) is true;
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