using System;
using System.Reflection;

namespace Alternet.UI.Integration.UIXmlHostApp.Remote
{
    internal static class InternalsAccessor
    {
        private static MethodInfo getHandleMethod;

        private static MethodInfo saveScreenshotMethod;

        private static MethodInfo wakeUpIdleMethod;

        public static void AttachEventHandler(object obj, string eventName, EventHandler handler)
        {
            static Delegate ConvertDelegate(Delegate originalDelegate, Type targetDelegateType)
            {
                return Delegate.CreateDelegate(
                    targetDelegateType,
                    originalDelegate.Target,
                    originalDelegate.Method);
            }

            var eventInfo = obj.GetType().GetEvent(eventName, BindingFlags.NonPublic | BindingFlags.Instance);
            var convertedHandler = ConvertDelegate(handler, eventInfo.EventHandlerType);
            var addMethod = eventInfo.GetAddMethod(true);
            addMethod.Invoke(obj, new[] { convertedHandler });
        }

        public static IntPtr GetHandle(Control control)
        {
            if (getHandleMethod == null)
                getHandleMethod = typeof(ControlHandler).GetMethod("GetHandle", BindingFlags.Instance | BindingFlags.NonPublic);

            return (IntPtr)getHandleMethod.Invoke(control.Handler, new object[0]);
        }

        public static void SaveScreenshot(Control control, string fileName)
        {
            if (saveScreenshotMethod == null)
                saveScreenshotMethod = typeof(ControlHandler).GetMethod("SaveScreenshot", BindingFlags.Instance | BindingFlags.NonPublic);

            saveScreenshotMethod.Invoke(control.Handler, new object[] { fileName });
        }

        public static void SetUixmlPreviewerMode(Application application)
        {
            var property = typeof(Application).GetProperty("InUixmlPreviewerMode", BindingFlags.Instance | BindingFlags.NonPublic);
            property.SetValue(application, true);
        }

        public static void WakeUpIdle(Application application)
        {
            if (wakeUpIdleMethod == null)
                wakeUpIdleMethod = typeof(Application).GetMethod("WakeUpIdle", BindingFlags.Instance | BindingFlags.NonPublic);
            wakeUpIdleMethod.Invoke(application, new object[0]);
        }
    }
}