using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;

namespace Alternet.UI
{
    /// <summary>
    /// Implements a timer that raises an event at user-defined intervals.
    /// This timer is optimized for use in AlterNET UI applications and must be used
    /// in a GUI environment instead of any other timers.
    /// </summary>
    public class Timer : FrameworkElement
    {
        private ITimerHandler? handler;
        private bool autoReset = true;
        private int interval = 100;

        static Timer()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Timer"/> class.
        /// </summary>
        public Timer()
        {
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
        {
            Interval = interval;
            Tick += tickHandler;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Timer"/> class
        /// with a specified interval (in milliseconds) and action to execute on each tick.
        /// </summary>
        /// <param name="interval">The time interval, in milliseconds, between each tick of the timer.
        /// Must be a positive integer.</param>
        /// <param name="tickAction">The action to execute on each tick of the timer.
        /// Cannot be <see langword="null"/>.</param>
        public Timer(int interval, Action tickAction)
        {
            Interval = interval;
            TickAction = tickAction;
        }

        /// <summary>
        /// Occurs when the specified timer interval has elapsed.
        /// </summary>
        [Category("Behavior")]
        public event EventHandler? Tick;

        /// <summary>
        /// Occurs when the specified timer interval has elapsed.
        /// </summary>
        [Category("Behavior")]
        public event ElapsedEventHandler? Elapsed;

        /// <summary>
        /// Gets or sets action which is called when timer interval has elapsed.
        /// </summary>
        public Action? TickAction { get; set; }

        /// <summary>
        /// Gets native timer.
        /// </summary>
        public virtual ITimerHandler Handler
        {
            get
            {
                if(handler is null)
                {
                    handler = App.Handler.CreateTimerHandler(this);
                    handler.Tick = NativeTimerTick;
                }

                return handler;
            }
        }

        /// <summary>
        /// Gets or sets the time duration, before the <see cref="Tick"/> event is raised
        /// relative to the last occurrence of the <see cref="Tick"/> event.
        /// </summary>
        /// <value>
        /// A <see cref="TimeSpan"/> specifying the time duration before the <see cref="Tick"/>
        /// event is raised
        /// relative to the last occurrence of the <see cref="Tick"/> event.
        /// </value>
        public virtual TimeSpan IntervalAsTimeSpan
        {
            get
            {
                return TimeSpan.FromMilliseconds(Interval);
            }

            set
            {
                Interval = (int)value.TotalMilliseconds;
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
        public virtual int Interval
        {
            get
            {
                return interval;
            }

            set
            {
                if (interval == value || DisposingOrDisposed)
                    return;
                interval = value;
                Handler.Interval = value;
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
        public virtual bool AutoReset
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
        public virtual bool Enabled
        {
            get
            {
                CheckDisposed();
                return Handler.Enabled;
            }

            set
            {
                if (Enabled == value)
                    return;
                CheckDisposed();

                if(!DisposingOrDisposed)
                    Handler.Enabled = value;
            }
        }

        /// <summary>
        /// Starts the timer which will raise tick events repeatedly.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StartRepeated()
        {
            AutoReset = true;
            Enabled = true;
        }

        /// <summary>
        /// Starts the timer which will raise tick event only once.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StartOnce()
        {
            AutoReset = false;
            Enabled = true;
        }

        /// <summary>
        /// Restarts the timer by stopping it and then starting it using <see cref="StartOnce"/>.
        /// </summary>
        public void RestartOnce()
        {
            Stop();
            StartOnce();
        }

        /// <summary>
        /// Restarts the timer by stopping it and then starting it using <see cref="StartRepeated"/>.
        /// </summary>
        public void RestartRepeated()
        {
            Stop();
            StartRepeated();
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        /// <remarks>You can also start the timer by setting the <see cref="Enabled"/>
        /// property to <see langword="true"/>.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
            TickAction?.Invoke();
            Elapsed?.Invoke(this, new());
            Tick?.Invoke(this, e);
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            if (handler is null)
                return;

            handler.Tick = null;
            SafeDispose(ref handler);
        }

        private void NativeTimerTick()
        {
            if (!autoReset)
                Stop();
            OnTick(EventArgs.Empty);
        }
    }
}