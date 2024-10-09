using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if IOS || MACCATALYST

using Foundation;
using UIKit;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties related to the MacOs/iOS platform
    /// </summary>
    public static class MacUtils
    {
        /// <summary>
        /// Notification name for "NSApplication.DidChangeScreenParametersNotification".
        /// </summary>
        // https://developer.apple.com/documentation/appkit/nsapplication
        public const string NSApplicationDidChangeScreenParametersNotification
            = "NSApplicationDidChangeScreenParametersNotification";

        /*
        KeyboardAutoManagerScroll.cs in MAUI source - scroll when keyboard shows/hides

        https://developer.apple.com/documentation/uikit/uiresponder

        keyboardDidShowNotification
        A notification that posts immediately after displaying the keyboard.

        keyboardWillHideNotification
        A notification that posts immediately prior to dismissing the keyboard.

        keyboardDidHideNotification
        A notification that posts immediately after dismissing the keyboard.

        https://www.hackingwithswift.com/read/19/7/fixing-the-keyboard-notificationcenter
        */

        static MacUtils()
        {
        }

        /// <summary>
        /// Adds observer for the specified notification.
        /// </summary>
        /// <param name="name">Notification name.</param>
        /// <param name="action">Action to call.</param>
        /// <returns></returns>
        public static NSObject AddObserver(string name, Action<NSNotification> action)
        {
            NSObject notificationToken
                = NSNotificationCenter.DefaultCenter.AddObserver(new NSString(name), action);
            return notificationToken;
        }
    }
}

#endif