using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal enum WebBrowserEvent
    {
        Unknown,
        Navigating,
        Navigated,
        Loaded,
        Error,
        NewWindow,
        TitleChanged,
        FullScreenChanged,
        ScriptMessageReceived,
        ScriptResult,
        BeforeBrowserCreate,
    }

    /// <summary>
    ///     Provides data for the <see cref="WebBrowser"/> events.
    /// </summary>
    public class WebBrowserEventArgs : CancelEventArgs
    {
        private readonly int intVal = 0;
        private bool isError = false;

        /// <summary>
        ///     Initializes a new instance of the WebBrowserEventArgs class.
        /// </summary>
        /// <param name="eventType">
        /// A <see cref="string"/> representing type of the event.
        /// </param>
        public WebBrowserEventArgs(string? eventType = null)
            : base()
        {
            if (eventType == null)
            {
                EventType = WebBrowserEvent.Unknown.ToString();
                return;
            }

            EventType = eventType;
        }

        internal WebBrowserEventArgs(
            Native.NativeEventArgs<Native.WebBrowserEventData> e,
            WebBrowserEvent eventType = WebBrowserEvent.Unknown)
            : base()
        {
            Url = e.Data.Url;
            TargetFrameName = e.Data.Target;
            NavigationAction = (WebBrowserNavigationAction)Enum.ToObject(
                typeof(WebBrowserNavigationAction), e.Data.ActionFlags);
            MessageHandler = e.Data.MessageHandler;
            isError = e.Data.IsError;
            EventType = eventType.ToString();
            Text = e.Data.Text;
            intVal = e.Data.IntVal;
            ClientData = e.Data.ClientData;
        }

        /// <summary>
        ///     Gets the client data value for the received event.
        /// </summary>
        /// <returns>
        /// A <see cref="System.IntPtr"/> representing the client data value
        /// for the received event.
        /// </returns>
        public IntPtr ClientData { get; set; }

        /// <summary>
        ///     Gets the type of the <see cref="WebBrowser"/> event.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> representing the type of
        /// the <see cref="WebBrowser"/> event.
        /// </returns>
        public string EventType { get; }

        /// <summary>
        ///     Gets the integer value for the received event.
        /// </summary>
        public int IntVal { get => intVal; }

        /// <summary>
        ///     Gets the type of error that can caused navigation to fail.
        /// </summary>
        /// <returns>
        /// A <see cref="WebBrowserNavigationError"/> representing the type of
        /// error that can caused navigation to fail.
        /// </returns>
        public WebBrowserNavigationError? NavigationError
        {
            get
            {
                if (EventType != WebBrowserEvent.Error.ToString())
                    return null;

                return (WebBrowserNavigationError)Enum.ToObject(
                        typeof(WebBrowserNavigationError),
                        intVal);
            }
        }

        /// <summary>
        ///     Gets the text data of the received event.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> representing the text data of the received event.
        /// </returns>
        public string? Text { get; set; }

        /// <summary>
        ///     Gets the URL being visited.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> representing the URL being visited.
        /// </returns>
        public string? Url { get; }

        /// <summary>
        ///     Gets the name of the target frame which the url of this event
        ///     has been or will be loaded into.
        /// </summary>
        /// <remarks>
        ///     This may return an empty string if the frame is not available.
        /// </remarks>
        /// <returns>
        /// A <see cref="string"/> representing the name of the target frame
        /// which the url of this event
        /// has been or will be loaded into.
        /// </returns>
        public string? TargetFrameName { get; }

        /// <summary>
        ///     Gets the type of navigation action.
        /// </summary>
        /// <remarks>
        /// Only valid for NewWindow events.
        /// </remarks>
        /// <returns>
        /// A <see cref="WebBrowserNavigationAction"/> representing the type
        /// of navigation action.
        /// </returns>
        public WebBrowserNavigationAction NavigationAction { get; }

        /// <summary>
        ///     Gets the name of the script handler.
        /// </summary>
        /// <remarks>
        /// Only valid for ScriptMessageReceived events.
        /// </remarks>
        /// <returns>
        /// A <see cref="string"/> representing the name of the script handler.
        /// </returns>
        public string? MessageHandler { get; }

        /// <summary>
        ///     Returns true the script execution failed.
        /// </summary>
        /// <remarks>
        /// Only valid for ScriptResult events.
        /// </remarks>
        /// <returns>
        /// <see langword="true"/> if the script execution failed; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public virtual bool IsError
        {
            get
            {
                if (EventType == WebBrowserEvent.ScriptResult.ToString())
                    return isError;
                return false;
            }

            set
            {
                isError = value;
            }
        }

        internal IntPtr CancelAsIntPtr()
        {
            return Cancel ? (IntPtr)1 : IntPtr.Zero;
        }
    }
}