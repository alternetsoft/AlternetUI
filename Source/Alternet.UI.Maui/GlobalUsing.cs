global using Coord = float;

#if ANDROID
global using PlatformApplication = Android.App.Application;
global using PlatformView = Android.Views.View;
global using PlatformWindow = Android.App.Activity;
#elif IOS || MACCATALYST
global using PlatformApplication = UIKit.IUIApplicationDelegate;
global using PlatformView = UIKit.UIView;
global using PlatformWindow = UIKit.UIWindow;
#elif WINDOWS
global using PlatformApplication = Microsoft.UI.Xaml.Application;
global using PlatformView = Microsoft.UI.Xaml.FrameworkElement;
global using PlatformWindow = Microsoft.UI.Xaml.Window;
#endif
