using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace Alternet.UI
{
    public static class MauiUtils
    {
        private static PlatformApplication? platformApplication;

        public static PlatformApplication PlatformApplication
        {
            get
            {
#if ANDROID
                platformApplication ??= (Android.App.Application)Android.App.Application.Context;
#elif IOS || MACCATALYST
                platformApplication ??= UIKit.UIApplication.SharedApplication.Delegate;
#elif WINDOWS
                platformApplication ??= Microsoft.UI.Xaml.Application.Current;
#endif
                return platformApplication;
            }
        }

#if WINDOWS
        public static void MapKey(Windows.System.VirtualKey windowsKey, Alternet.UI.Key key)
        {
        }

        public static void MapKeys()
        {
#pragma warning disable
            MapKey(Windows.System.VirtualKey.None           , Alternet.UI.Key.None);

            MapKey(Windows.System.VirtualKey.Back           , Alternet.UI.Key.Back           );
            MapKey(Windows.System.VirtualKey.Tab            , Alternet.UI.Key.Tab            );
            MapKey(Windows.System.VirtualKey.Clear          , Alternet.UI.Key.Clear          );
            MapKey(Windows.System.VirtualKey.Enter          , Alternet.UI.Key.Enter          );
            MapKey(Windows.System.VirtualKey.Shift          , Alternet.UI.Key.Shift          );
            MapKey(Windows.System.VirtualKey.Control        , Alternet.UI.Key.Control        );
            MapKey(Windows.System.VirtualKey.Menu           , Alternet.UI.Key.Menu           );
            MapKey(Windows.System.VirtualKey.Pause          , Alternet.UI.Key.Pause          );
            MapKey(Windows.System.VirtualKey.CapitalLock    , Alternet.UI.Key.CapsLock);

            MapKey(Windows.System.VirtualKey.Space          , Alternet.UI.Key.Space          );
            MapKey(Windows.System.VirtualKey.PageUp         , Alternet.UI.Key.PageUp         );
            MapKey(Windows.System.VirtualKey.PageDown       , Alternet.UI.Key.PageDown       );
            MapKey(Windows.System.VirtualKey.End            , Alternet.UI.Key.End            );
            MapKey(Windows.System.VirtualKey.Home           , Alternet.UI.Key.Home           );
            MapKey(Windows.System.VirtualKey.Left           , Alternet.UI.Key.Left           );
            MapKey(Windows.System.VirtualKey.Up             , Alternet.UI.Key.Up             );
            MapKey(Windows.System.VirtualKey.Right          , Alternet.UI.Key.Right          );
            MapKey(Windows.System.VirtualKey.Down           , Alternet.UI.Key.Down           );
            
            MapKey(Windows.System.VirtualKey.Insert         , Alternet.UI.Key.Insert         );
            MapKey(Windows.System.VirtualKey.Delete         , Alternet.UI.Key.Delete         );

            MapKey(Windows.System.VirtualKey.Number0        , Alternet.UI.Key.D0        );
            MapKey(Windows.System.VirtualKey.Number1        , Alternet.UI.Key.D1        );
            MapKey(Windows.System.VirtualKey.Number2        , Alternet.UI.Key.D2        );
            MapKey(Windows.System.VirtualKey.Number3        , Alternet.UI.Key.D3        );
            MapKey(Windows.System.VirtualKey.Number4        , Alternet.UI.Key.D4        );
            MapKey(Windows.System.VirtualKey.Number5        , Alternet.UI.Key.D5        );
            MapKey(Windows.System.VirtualKey.Number6        , Alternet.UI.Key.D6        );
            MapKey(Windows.System.VirtualKey.Number7        , Alternet.UI.Key.D7        );
            MapKey(Windows.System.VirtualKey.Number8        , Alternet.UI.Key.D8        );
            MapKey(Windows.System.VirtualKey.Number9        , Alternet.UI.Key.D9        );
            MapKey(Windows.System.VirtualKey.A              , Alternet.UI.Key.A              );
            MapKey(Windows.System.VirtualKey.B              , Alternet.UI.Key.B              );
            MapKey(Windows.System.VirtualKey.C              , Alternet.UI.Key.C              );
            MapKey(Windows.System.VirtualKey.D              , Alternet.UI.Key.D              );
            MapKey(Windows.System.VirtualKey.E              , Alternet.UI.Key.E              );
            MapKey(Windows.System.VirtualKey.F              , Alternet.UI.Key.F              );
            MapKey(Windows.System.VirtualKey.G              , Alternet.UI.Key.G              );
            MapKey(Windows.System.VirtualKey.H              , Alternet.UI.Key.H              );
            MapKey(Windows.System.VirtualKey.I              , Alternet.UI.Key.I              );
            MapKey(Windows.System.VirtualKey.J              , Alternet.UI.Key.J              );
            MapKey(Windows.System.VirtualKey.K              , Alternet.UI.Key.K              );
            MapKey(Windows.System.VirtualKey.L              , Alternet.UI.Key.L              );
            MapKey(Windows.System.VirtualKey.M              , Alternet.UI.Key.M              );
            MapKey(Windows.System.VirtualKey.N              , Alternet.UI.Key.N              );
            MapKey(Windows.System.VirtualKey.O              , Alternet.UI.Key.O              );
            MapKey(Windows.System.VirtualKey.P              , Alternet.UI.Key.P              );
            MapKey(Windows.System.VirtualKey.Q              , Alternet.UI.Key.Q              );
            MapKey(Windows.System.VirtualKey.R              , Alternet.UI.Key.R              );
            MapKey(Windows.System.VirtualKey.S              , Alternet.UI.Key.S              );
            MapKey(Windows.System.VirtualKey.T              , Alternet.UI.Key.T              );
            MapKey(Windows.System.VirtualKey.U              , Alternet.UI.Key.U              );
            MapKey(Windows.System.VirtualKey.V              , Alternet.UI.Key.V              );
            MapKey(Windows.System.VirtualKey.W              , Alternet.UI.Key.W              );
            MapKey(Windows.System.VirtualKey.X              , Alternet.UI.Key.X              );
            MapKey(Windows.System.VirtualKey.Y              , Alternet.UI.Key.Y              );
            MapKey(Windows.System.VirtualKey.Z              , Alternet.UI.Key.Z              );
            
            MapKey(Windows.System.VirtualKey.LeftWindows    , Alternet.UI.Key.Windows    );
            MapKey(Windows.System.VirtualKey.RightWindows   , Alternet.UI.Key.Windows   );

            MapKey(Windows.System.VirtualKey.NumberPad0     , Alternet.UI.Key.NumPad0);
            MapKey(Windows.System.VirtualKey.NumberPad1     , Alternet.UI.Key.NumPad1);
            MapKey(Windows.System.VirtualKey.NumberPad2     , Alternet.UI.Key.NumPad2);
            MapKey(Windows.System.VirtualKey.NumberPad3     , Alternet.UI.Key.NumPad3);
            MapKey(Windows.System.VirtualKey.NumberPad4     , Alternet.UI.Key.NumPad4);
            MapKey(Windows.System.VirtualKey.NumberPad5     , Alternet.UI.Key.NumPad5);
            MapKey(Windows.System.VirtualKey.NumberPad6     , Alternet.UI.Key.NumPad6);
            MapKey(Windows.System.VirtualKey.NumberPad7     , Alternet.UI.Key.NumPad7);
            MapKey(Windows.System.VirtualKey.NumberPad8     , Alternet.UI.Key.NumPad8);
            MapKey(Windows.System.VirtualKey.NumberPad9     , Alternet.UI.Key.NumPad9);
            
            MapKey(Windows.System.VirtualKey.Multiply       , Alternet.UI.Key.Asterisk);
            MapKey(Windows.System.VirtualKey.Add            , Alternet.UI.Key.PlusSign            );
            MapKey(Windows.System.VirtualKey.Subtract       , Alternet.UI.Key.Minus       );
            MapKey(Windows.System.VirtualKey.Decimal        , Alternet.UI.Key.Period        );
            MapKey(Windows.System.VirtualKey.Divide         , Alternet.UI.Key.Slash         );
            
            MapKey(Windows.System.VirtualKey.F1             , Alternet.UI.Key.F1             );
            MapKey(Windows.System.VirtualKey.F2             , Alternet.UI.Key.F2             );
            MapKey(Windows.System.VirtualKey.F3             , Alternet.UI.Key.F3             );
            MapKey(Windows.System.VirtualKey.F4             , Alternet.UI.Key.F4             );
            MapKey(Windows.System.VirtualKey.F5             , Alternet.UI.Key.F5             );
            MapKey(Windows.System.VirtualKey.F6             , Alternet.UI.Key.F6             );
            MapKey(Windows.System.VirtualKey.F7             , Alternet.UI.Key.F7             );
            MapKey(Windows.System.VirtualKey.F8             , Alternet.UI.Key.F8             );
            MapKey(Windows.System.VirtualKey.F9             , Alternet.UI.Key.F9             );
            MapKey(Windows.System.VirtualKey.F10            , Alternet.UI.Key.F10            );
            MapKey(Windows.System.VirtualKey.F11            , Alternet.UI.Key.F11            );
            MapKey(Windows.System.VirtualKey.F12            , Alternet.UI.Key.F12            );
            MapKey(Windows.System.VirtualKey.F13            , Alternet.UI.Key.F13            );
            MapKey(Windows.System.VirtualKey.F14            , Alternet.UI.Key.F14            );
            MapKey(Windows.System.VirtualKey.F15            , Alternet.UI.Key.F15            );
            MapKey(Windows.System.VirtualKey.F16            , Alternet.UI.Key.F16            );
            MapKey(Windows.System.VirtualKey.F17            , Alternet.UI.Key.F17            );
            MapKey(Windows.System.VirtualKey.F18            , Alternet.UI.Key.F18            );
            MapKey(Windows.System.VirtualKey.F19            , Alternet.UI.Key.F19            );
            MapKey(Windows.System.VirtualKey.F20            , Alternet.UI.Key.F20            );
            MapKey(Windows.System.VirtualKey.F21            , Alternet.UI.Key.F21            );
            MapKey(Windows.System.VirtualKey.F22            , Alternet.UI.Key.F22            );
            MapKey(Windows.System.VirtualKey.F23            , Alternet.UI.Key.F23            );
            MapKey(Windows.System.VirtualKey.F24            , Alternet.UI.Key.F24            );

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
#endif

        public static void AddAllViewsToParent(Layout parent)
        {
            var types = AssemblyUtils.GetTypeDescendants(typeof(View));

            foreach(var type in types)
            {
                if (type == typeof(WebView) || type == typeof(VerticalStackLayout) || type == typeof(GroupableItemsView))
                    continue;

                View? instance;

                try
                {
                    if (!AssemblyUtils.HasConstructorNoParams(type))
                        continue;
                    instance = (View?)Activator.CreateInstance(type);
                }
                catch (Exception)
                {
                    instance = null;
                }

                if (instance is null)
                    continue;

                try
                {
                    parent.Children.Add(instance);
                    var nativeType = instance.Handler?.PlatformView?.GetType().FullName;
                    LogUtils.LogToFile($"{instance.GetType().FullName} uses {nativeType}");
                    parent.Children.Remove(instance);
                }
                catch (Exception)
                {
                }
            }
        }

        public static void EnumViewsToLog(Layout parent)
        {
            foreach (var item in parent.Children)
            {
                var nativeType = item.Handler?.PlatformView?.GetType().Name;

                LogUtils.LogToFile($"{item.GetType().Name} uses {nativeType}");
            }
        }
    }
}
