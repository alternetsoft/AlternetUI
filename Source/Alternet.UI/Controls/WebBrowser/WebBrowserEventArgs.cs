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
    ///     Provides data for the WebBrowser events.
    /// </summary>
    public class WebBrowserEventArgs : CancelEventArgs
    {
        private bool FIsError = false;
        private readonly int FIntVal = 0;

        
        public IntPtr ClientData { get; set; }
        
        /// <summary>
        ///     Gets the type of browser event.
        /// </summary>
        public string EventType { get; }
        
        /// <summary>
        ///     Gets the integer value for the received event.
        /// </summary>
        public int IntVal { get => FIntVal; }
        
        /// <summary>
        ///     Gets the type of error that can caused navigation to fail.
        /// </summary>
        public WebBrowserNavigationError? NavigationError 
        { 
            get 
            {
                if(EventType == WebBrowserEvent.Error.ToString())
                    return (WebBrowserNavigationError)Enum.ToObject(
                        typeof(WebBrowserNavigationError), FIntVal);
                return null;
            } 
        }
        
        /// <summary>
        ///     Gets the string value for the received event. 
        /// </summary>
        public string? Text { get; set; }
        
        /// <summary>
        ///     Gets the URL being visited.
        /// </summary>
        public string? Url { get; }
        
        /// <summary>
        ///     Gets the name of the target frame which the url of this event has been 
        ///     or will be loaded into.         
        ///     This may return an empty string if the frame is not available.
        /// </summary>
        public string? TargetFrameName { get; }
        
        /// <summary>
        ///     Gets the type of navigation action. Only valid for NewWindow events.
        /// </summary>
        public WebBrowserNavigationAction NavigationAction { get; }
        
        /// <summary>
        ///     Gets the name of the script handler. Only valid for ScriptMessageReceived events.
        /// </summary>
        public string? MessageHandler { get; }
        
        /// <summary>
        ///     Returns true the script execution failed. Only valid for ScriptResult events.
        /// </summary>
        public virtual bool IsError 
        { 
            get
            {
                if (EventType == WebBrowserEvent.ScriptResult.ToString())
                    return FIsError;
                return false;
            }
            set
            {
                FIsError = value;
            }
        }
        
        /// <summary>
        ///     Initializes a new instance of the WebBrowserEventArgs class. 
        /// </summary>
        //TODO: !KU help
        public WebBrowserEventArgs(string? eventType=null) : base()
        {
            if(eventType==null)
            {
                EventType = WebBrowserEvent.Unknown.ToString();
                return;
            }
            EventType = eventType;
        }
        
        internal IntPtr CancelAsIntPtr() 
        {
            return Cancel ? (IntPtr)1 : IntPtr.Zero;
        }
        
        internal WebBrowserEventArgs(Native.NativeEventArgs<Native.WebBrowserEventData> e, 
            WebBrowserEvent eventType= WebBrowserEvent.Unknown) : base()
        {
            Url = e.Data.Url;
            TargetFrameName = e.Data.Target;
            NavigationAction = (WebBrowserNavigationAction)Enum.ToObject(
                typeof(WebBrowserNavigationAction), e.Data.ActionFlags);
            MessageHandler = e.Data.MessageHandler;
            FIsError = e.Data.IsError;
            EventType = eventType.ToString();
            Text = e.Data.Text;
            FIntVal = e.Data.IntVal;
            ClientData = e.Data.ClientData;
        }
        
    }
    
}

