using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if ANDROID

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;

using SkiaSharp;
using SkiaSharp.Views.Android;

namespace Alternet.UI
{
    /// <summary>
    /// Default implementation of the <see cref="Application.IActivityLifecycleCallbacks"/>
    /// interface.
    /// </summary>
    public partial class ApplicationCallbacks
        : Java.Lang.Object, Android.App.Application.IActivityLifecycleCallbacks
    {
        private static ApplicationCallbacks? callbacks;

        private readonly WeakReference<Android.App.Activity?> currentActivity = new(null);

        public event EventHandler? ActivityDestroyed;

        public event EventHandler? ActivityPaused;

        public event EventHandler? ActivityResumed;

        public event EventHandler? ActivityStarted;

        public event EventHandler? ActivityStopped;

        public event EventHandler<UI.BaseEventArgs<Bundle?>>? ActivityCreated;

        public event EventHandler<UI.BaseEventArgs<Bundle>>? ActivitySaveInstanceState;

        public static ApplicationCallbacks Default
        {
            get
            {
                if (callbacks is null)
                {
                    callbacks = new();
                }

                return callbacks;
            }
        }

        public Context Context
        {
            get
            {
                return Activity ?? Application.Context;
            }
        }

        public Android.App.Activity? Activity
        {
            get => currentActivity.TryGetTarget(out var a) ? a : null;
            set => currentActivity.SetTarget(value);
        }

        public static void Init(Application application)
        {
            if (callbacks != null)
                return;
            application.RegisterActivityLifecycleCallbacks(Default);
        }

        public virtual void OnActivityCreated(PlatformWindow activity, Bundle? savedInstanceState)
        {
            Activity = activity;
            ActivityCreated?.Invoke(activity, new(savedInstanceState));
        }

        public virtual void OnActivityDestroyed(PlatformWindow activity)
        {
            ActivityDestroyed?.Invoke(activity, EventArgs.Empty);
        }

        public virtual void OnActivityPaused(PlatformWindow activity)
        {
            Activity = activity;
            ActivityPaused?.Invoke(activity, EventArgs.Empty);
        }

        public virtual void OnActivityResumed(PlatformWindow activity)
        {
            Activity = activity;
            ActivityResumed?.Invoke(activity, EventArgs.Empty);
        }

        public virtual void OnActivitySaveInstanceState(PlatformWindow activity, Bundle outState)
        {
            Activity = activity;
            ActivitySaveInstanceState?.Invoke(activity, new(outState));
        }

        public virtual void OnActivityStarted(PlatformWindow activity)
        {
            Activity = activity;
            ActivityStarted?.Invoke(activity, EventArgs.Empty);
        }

        public virtual void OnActivityStopped(PlatformWindow activity)
        {
            Activity = activity;
            ActivityStopped?.Invoke(activity, EventArgs.Empty);
        }
    }
}
#endif