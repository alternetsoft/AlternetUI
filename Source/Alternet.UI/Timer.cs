using System;
using System.ComponentModel;
using System.Timers;

namespace Alternet.UI
{
    /// <summary>
    /// Implements a timer that raises an event at user-defined intervals.
    /// This timer is optimized for use in AlterNET UI applications and must be used
    /// in a GUI environment.
    /// </summary>
    public class Timer : DisposableObject, IComponent, IDisposable
    {
        private Native.Timer nativeTimer;
        private bool autoReset = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="Timer"/> class.
        /// </summary>
        public Timer()
        {
            nativeTimer = new Native.Timer();
            nativeTimer.Tick += NativeTimer_Tick;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Timer"/> class with the specified
        /// tick interval.
        /// </summary>
        /// <param name="interval">
        /// The time duration before the <see cref="Tick"/> event is raised
        /// relative to the last occurrence of the <see cref="Tick"/> event.
        /// </param>
        public Timer(TimeSpan interval)
            : this()
        {
            IntervalAsTimeSpan = interval;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Timer"/> class with the specified
        /// tick interval (in milliseconds).
        /// </summary>
        /// <param name="interval">
        /// The time duration before the <see cref="Tick"/> event is raised.
        /// </param>
        public Timer(int interval)
            : this()
        {
            Interval = interval;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Timer"/> class with the specified
        /// tick interval and <see cref="Tick"/> handler.
        /// </summary>
        /// <param name="interval">
        /// The time duration before the <see cref="Tick"/> event is raised
        /// relative to the last occurrence of the <see cref="Tick"/> event.
        /// </param>
        /// <param name="tickHandler">
        /// An <see cref="EventHandler"/> which is called when the specified timer interval
        /// has elapsed and the timer is enabled.
        /// </param>
        public Timer(TimeSpan interval, EventHandler tickHandler)
            : this()
        {
            IntervalAsTimeSpan = interval;
            Tick += tickHandler;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Timer"/> class with the specified
        /// tick interval (in milliseconds) and <see cref="Tick"/> handler.
        /// </summary>
        /// <param name="interval">
        /// The time duration before the <see cref="Tick"/> event is raised.
        /// </param>
        /// <param name="tickHandler">
        /// An <see cref="EventHandler"/> which is called when the specified timer interval
        /// has elapsed and the timer is enabled.
        /// </param>
        public Timer(int interval, EventHandler tickHandler)
            : this()
        {
            Interval = interval;
            Tick += tickHandler;
        }

        /// <summary>
        /// Occurs when the specified timer interval has elapsed.
        /// </summary>
        public event EventHandler? Tick;

        /// <summary>
        /// Occurs when the specified timer interval has elapsed.
        /// </summary>
        [Category("Behavior")]
        public event ElapsedEventHandler? Elapsed;

        /// <summary>
        /// Gets or sets the <see cref="ISite"/> associated with the object; or null,
        /// if the object does not have a site.
        /// </summary>
        public ISite? Site { get; set; }

        /// <summary>
        /// Gets or sets the time duration, before the <see cref="Tick"/> event is raised
        /// relative to the last occurrence of the <see cref="Tick"/> event.
        /// </summary>
        /// <value>
        /// A <see cref="TimeSpan"/> specifying the time duration before the <see cref="Tick"/>
        /// event is raised
        /// relative to the last occurrence of the <see cref="Tick"/> event.
        /// </value>
        public TimeSpan IntervalAsTimeSpan
        {
            get
            {
                CheckDisposed();
                return TimeSpan.FromMilliseconds(nativeTimer.Interval);
            }

            set
            {
                CheckDisposed();
                nativeTimer.Interval = (int)value.TotalMilliseconds;
            }
        }

        /// <summary>
        /// Gets or sets the time duration (in milliseconds), before the <see cref="Tick"/> event
        /// is raised.
        /// </summary>
        /// <value>
        /// An <see cref="int"/> specifying the time duration before the <see cref="Tick"/>
        /// event is raised.
        /// </value>
        public int Interval
        {
            get
            {
                CheckDisposed();
                return nativeTimer.Interval;
            }

            set
            {
                CheckDisposed();
                nativeTimer.Interval = value;
            }
        }

        /// <summary>
        /// Gets or sets a <see cref="bool"/> indicating whether the <see cref="Timer" /> should
        /// raise events only once (<see langword="false"/>) or repeatedly
        /// (<see langword="true" />).</summary>
        /// <returns>
        /// <see langword="true" /> if the <see cref="Timer" /> should raise
        /// the events each time the interval elapses; <see langword="false"/> if it should
        /// raise the events only once, after the first time the interval elapses.
        /// The default is <see langword="true"/>.</returns>
        [Category("Behavior")]
        [DefaultValue(true)]
        public bool AutoReset
        {
            get
            {
                return autoReset;
            }

            set
            {
                autoReset = value;
            }
        }

        /// <summary>
        /// Gets or sets an arbitrary <see cref="object"/> representing some type of user state.
        /// </summary>
        /// <value>An arbitrary <see cref="object"/> representing some type of user state.</value>
        public object? Tag { get; set; }

        /// <summary>
        /// Gets or sets whether the timer is running.
        /// </summary>
        /// <value><see langword="true"/> if the timer is currently enabled; otherwise,
        /// <see langword="false"/>. The default is <see langword="false"/>.</value>
        /// <remarks>
        /// Calling the <see cref="Start"/> method is the same as setting <see cref="Enabled"/>
        /// to <see langword="true"/>.
        /// Likewise, calling the <see cref="Stop"/> method is the same as setting
        /// <see cref="Enabled"/> to <see langword="false"/>.
        /// </remarks>
        public bool Enabled
        {
            get
            {
                CheckDisposed();
                return nativeTimer.Enabled;
            }

            set
            {
                CheckDisposed();
                nativeTimer.Enabled = value;
            }
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        /// <remarks>You can also start the timer by setting the <see cref="Enabled"/>
        /// property to <see langword="true"/>.</remarks>
        public void Start()
        {
            Enabled = true;
        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
        /// <remarks>
        /// <para>
        /// You can also stop the timer by setting the <see cref="Enabled"/> property to
        /// <see langword="false"/>.
        /// A <see cref="Timer"/> object may be enabled and disabled multiple times within
        /// the same application session.
        /// </para>
        /// <para>
        /// Calling <see cref="Start"/> after you have disabled a <see cref="Timer"/> by
        /// calling <see cref="Stop"/> will cause
        /// the <see cref="Timer"/> to restart the interrupted interval.
        /// If your <see cref="Timer"/> is set for a 5000-millisecond interval, and you
        /// call <see cref="Stop"/> at around 3000 milliseconds,
        /// calling <see cref="Start"/> will cause the <see cref="Timer"/> to wait 5000
        /// milliseconds before raising the <see cref="Tick"/> event.
        /// </para>
        /// </remarks>
        public void Stop()
        {
            Enabled = false;
        }

        /// <summary>
        /// Raises the <see cref="Tick"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.
        /// This is always <see cref="EventArgs.Empty"/>.</param>
        /// <remarks>
        /// <para>
        /// This method is called for each timer tick. It calls any methods that are added
        /// through <see cref="Tick"/>.
        /// If you are inheriting from <see cref="Timer"/>, you can override this method.
        /// </para>
        /// <para>
        /// When overriding <see cref="OnTick(EventArgs)"/> in a derived class,
        /// make sure that you call the base class's <see cref="OnTick(EventArgs)"/> method.
        /// </para>
        /// </remarks>
        protected virtual void OnTick(EventArgs e)
        {
            Elapsed?.Invoke(this, new());
            Tick?.Invoke(this, e);
        }

        /// <inheritdoc/>
        protected override void DisposeResources()
        {
            nativeTimer.Tick -= NativeTimer_Tick;
            nativeTimer.Dispose();
            nativeTimer = null!;
        }

        private void NativeTimer_Tick(object? sender, EventArgs e)
        {
            if (!autoReset)
                Stop();
            OnTick(EventArgs.Empty);
        }
    }
}