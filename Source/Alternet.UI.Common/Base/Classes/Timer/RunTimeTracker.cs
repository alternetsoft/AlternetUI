using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Tracks the runtime duration of operations using a <see cref="Stopwatch"/>.
    /// </summary>
    public class RunTimeTracker
    {
        private Stopwatch? watch;
        private long totalElapsed;
        private int count;

        /// <summary>
        /// Initializes static members of the <see cref="RunTimeTracker"/> class.
        /// </summary>
        static RunTimeTracker()
        {
            Items = new List<RunTimeTracker>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RunTimeTracker"/> class with the specified name.
        /// </summary>
        /// <param name="name">The name of the tracker.</param>
        public RunTimeTracker(string name)
        {
            Name = name;
            Items.Add(this);
        }

        /// <summary>
        /// Gets the list of all <see cref="RunTimeTracker"/> instances.
        /// </summary>
        public static List<RunTimeTracker> Items { get; }

        /// <summary>
        /// Gets the name of the tracker.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the total elapsed ticks recorded by this tracker.
        /// </summary>
        public long TotalElapsed => totalElapsed;

        /// <summary>
        /// Gets the number of times the tracker has been stopped.
        /// </summary>
        public int Count => count;

        /// <summary>
        /// Gets the average elapsed ticks per tracked operation.
        /// </summary>
        public int AverageElapsed => count == 0 ? 0 : (int)((double)totalElapsed / count);

        /// <summary>
        /// Logs the runtime statistics of all <see cref="RunTimeTracker"/>
        /// instances to the debug output.
        /// </summary>
        public static void Log(ILogWriter? logWriter = null)
        {
            logWriter ??= LogWriter.Current;

            logWriter.WriteLine("RunTimeTracker Log:");
            logWriter.Indent();

            foreach (var item in Items)
            {
                if (item.Count == 0)
                    continue;
                logWriter.WriteLine($"{item.Name}: TotalElapsed={item.TotalElapsed} Count={item.Count} AverageElapsed={item.AverageElapsed}");
            }

            logWriter.Unindent();
            logWriter.WriteLine("End of RunTimeTracker Log");
        }

        /// <summary>
        /// Resets the state of the object, clearing any accumulated data and stopping
        /// any ongoing operations.
        /// </summary>
        /// <remarks>This method resets all internal counters and timers to their initial state.
        /// After calling this method, the object is in the same state as
        /// if it were newly created.</remarks>
        public void Reset()
        {
            totalElapsed = 0;
            count = 0;
            watch = null;
        }

        /// <summary>
        /// Starts the tracker. Returns <c>true</c> if started successfully; otherwise, <c>false</c>.
        /// </summary>
        /// <returns><c>true</c> if started; <c>false</c> if already started.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the tracker
        /// is already started and debug is attached.</exception>
        public bool Start()
        {
            if (watch != null)
            {
                if (DebugUtils.IsDebugDefinedAndAttached)
                    throw new InvalidOperationException("Tracker is already started.");
                return false;
            }

            watch = System.Diagnostics.Stopwatch.StartNew();
            return true;
        }

        /// <summary>
        /// Stops the tracker. Returns <c>true</c> if stopped successfully; otherwise, <c>false</c>.
        /// </summary>
        /// <returns><c>true</c> if stopped; <c>false</c> if already stopped.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the tracker
        /// is already stopped and debug is attached.</exception>
        public bool Stop()
        {
            if (watch == null)
            {
                if (DebugUtils.IsDebugDefinedAndAttached)
                    throw new InvalidOperationException("Tracker is already stopped.");
                return false;
            }

            watch.Stop();
            var elapsed = watch.ElapsedTicks;
            watch = null;
            totalElapsed += elapsed;
            count++;
            return true;
        }
    }
}
