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
        /// Notification name.
        /// </summary>
        public const string NSApplicationDidChangeScreenParametersNotification
            = "NSApplicationDidChangeScreenParametersNotification";

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