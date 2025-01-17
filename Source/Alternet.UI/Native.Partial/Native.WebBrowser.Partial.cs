using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;

namespace Alternet.UI.Native
{
    internal partial class WebBrowser
    {
        public WebBrowserEventArgs CreateArgs(
            Native.NativeEventArgs<Native.WebBrowserEventData> e,
            WebBrowserEvent eventType = WebBrowserEvent.Unknown)
        {
            var result = new WebBrowserEventArgs();

            result.Url = e.Data.Url;
            result.TargetFrameName = e.Data.Target;
            result.NavigationAction = (WebBrowserNavigationAction)Enum.ToObject(
                typeof(WebBrowserNavigationAction), e.Data.ActionFlags);
            result.MessageHandler = e.Data.MessageHandler;
            result.IsError = e.Data.IsError;
            result.EventType = eventType.ToString();
            result.Text = e.Data.Text;
            result.IntVal = e.Data.IntVal;
            result.ClientData = e.Data.ClientData;

            return result;
        }

        public void OnPlatformEventNavigating(NativeEventArgs<WebBrowserEventData> e)
        {
            WebBrowserEventArgs ea = CreateArgs(e, WebBrowserEvent.Navigating);
            (UIControl as UI.WebBrowser)?.RaiseNavigating(ea);
            e.Result = ea.CancelAsIntPtr();
        }

        public void OnPlatformEventNavigated(NativeEventArgs<WebBrowserEventData> e)
        {
            WebBrowserEventArgs ea = CreateArgs(e, WebBrowserEvent.Navigated);
            (UIControl as UI.WebBrowser)?.RaiseNavigated(ea);
        }

        public void OnPlatformEventLoaded(NativeEventArgs<WebBrowserEventData> e)
        {
            WebBrowserEventArgs ea = CreateArgs(e, WebBrowserEvent.Loaded);
            (UIControl as UI.WebBrowser)?.RaiseLoaded(ea);
        }

        public void OnPlatformEventError(NativeEventArgs<WebBrowserEventData> e)
        {
            WebBrowserEventArgs ea = CreateArgs(e, WebBrowserEvent.Error);
            (UIControl as UI.WebBrowser)?.RaiseError(ea);
        }

        public void OnPlatformEventNewWindow(NativeEventArgs<WebBrowserEventData> e)
        {
            WebBrowserEventArgs ea = CreateArgs(e, WebBrowserEvent.NewWindow);
            (UIControl as UI.WebBrowser)?.RaiseNewWindow(ea);
        }

        public void OnPlatformEventTitleChanged(NativeEventArgs<WebBrowserEventData> e)
        {
            WebBrowserEventArgs ea = CreateArgs(e, WebBrowserEvent.TitleChanged);
            (UIControl as UI.WebBrowser)?.RaiseDocumentTitleChanged(ea);
        }

        public void OnPlatformEventFullScreenChanged(NativeEventArgs<WebBrowserEventData> e)
        {
            WebBrowserEventArgs ea = CreateArgs(e, WebBrowserEvent.FullScreenChanged);
            (UIControl as UI.WebBrowser)?.RaiseFullScreenChanged(ea);
        }

        public void OnPlatformEventScriptMessageReceived(NativeEventArgs<WebBrowserEventData> e)
        {
            WebBrowserEventArgs ea = CreateArgs(e, WebBrowserEvent.ScriptMessageReceived);
            (UIControl as UI.WebBrowser)?.RaiseScriptMessageReceived(ea);
        }

        public void OnPlatformEventScriptResult(NativeEventArgs<WebBrowserEventData> e)
        {
            WebBrowserEventArgs ea = CreateArgs(e, WebBrowserEvent.ScriptResult);
            (UIControl as UI.WebBrowser)?.RaiseScriptResult(ea);
        }

        public void OnPlatformEventBeforeBrowserCreate(NativeEventArgs<WebBrowserEventData> e)
        {
            WebBrowserEventArgs ea = CreateArgs(e, WebBrowserEvent.BeforeBrowserCreate);
            (UIControl as UI.WebBrowser)?.RaiseBeforeBrowserCreate(ea);
        }
    }
}