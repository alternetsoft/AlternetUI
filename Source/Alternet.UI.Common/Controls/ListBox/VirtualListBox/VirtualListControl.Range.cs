using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public partial class VirtualListControl
    {
        /// <summary>
        /// Implements controller for the add range operation which is performed
        /// in the background thread. It allows to continue work with the control while
        /// new items are added to it.
        /// </summary>
        /// <typeparam name="TSource">Type of the source item.</typeparam>
        public class RangeAdditionController<TSource> : DisposableObject
        {
            /// <summary>
            /// Gets or sets control on which add range operation is performed.
            /// </summary>
            public VirtualListControl ListBox;

            /// <summary>
            /// Gets or sets the function which provides the <see cref="IEnumerable{T}"/>
            /// instance which
            /// is "yield" constructed in the another thread.
            /// </summary>
            public Func<IEnumerable<TSource>> SourceFunc;

            /// <summary>
            /// Gets or sets the function which is called to convert
            /// source items
            /// to the items which can be used in this control. If this function returns Null,
            /// source item is ignored. This function is called from the thread that
            /// provides source items so do not access UI elements from it.
            /// </summary>
            public Func<TSource, ListControlItem?> ConvertItemFunc;

            /// <summary>
            /// Gets or sets the function which is called to check whether to continue
            /// the conversion. You can return False to stop the conversion.
            /// This function is called from the
            /// main thread so it can access UI elements.
            /// </summary>
            public Func<bool> ContinueFunc;

            /// <summary>
            /// Gets or sets whether debug information is logged.
            /// </summary>
            public bool IsDebugInfoLogged = false;

            /// <summary>
            /// Gets or sets object name for the debug purposes.
            /// </summary>
            public string? Name;

            /// <summary>
            /// Gets or sets size of the items buffer. Default is 10.
            /// </summary>
            public int BufferSize = 10;

            /// <summary>
            /// Gets or sets the value in milliseconds to wait after buffer is
            /// converted and all buffered items were added to the control.
            /// Default is 150.
            /// </summary>
            public int SleepAfterBufferMsec = 150;

            private const bool logControllerState = false;

            private static int globalUniqueNumber;

            private readonly int uniqueNumber = ++globalUniqueNumber;

            private CancellationTokenSource? cts;

            /// <summary>
            /// Initializes a new instance of the <see cref="RangeAdditionController{TSource}"/>
            /// class with the specified parameters.
            /// </summary>
            public RangeAdditionController(
                VirtualListControl listBox,
                Func<IEnumerable<TSource>> source,
                Func<TSource, ListControlItem?> convertItem,
                Func<bool> continueFunc)
            {
                ListBox = listBox;
                SourceFunc = source;
                ConvertItemFunc = convertItem;
                ContinueFunc = continueFunc;

                App.DebugLogIf($"{SafeName} Created.", logControllerState);
            }

            /// <summary>
            /// Gets or sets action which is called when thread action is finished.
            /// </summary>
            public Action? ThreadActionFinished { get; set; }

            /// <summary>
            /// Gets name of the object safely so it will always be not empty.
            /// </summary>
            public virtual string SafeName
                => Name ?? ("ListBox.RangeAdditionController" + uniqueNumber);

            /// <summary>
            /// Gets a value indicating whether the cancellation has been requested for the operation.
            /// </summary>
            public virtual bool IsCancellationRequested
            {
                get
                {
                    return cts?.Token.IsCancellationRequested ?? true;
                }
            }

            /// <summary>
            /// Gets a value indicating whether the operation has been stopped.
            /// </summary>
            public virtual bool IsStopped
            {
                get
                {
                    return cts is null;
                }
            }

            /// <summary>
            /// Stops controller add range operation.
            /// </summary>
            public virtual void Stop()
            {
                cts?.Cancel();
                cts = null;
            }

            /// <summary>
            /// Starts controller add range operation.
            /// </summary>
            public virtual void Start(Action? onComplete = null)
            {
                Stop();

                Internal();

                void Internal()
                {
                    cts = new();
                    var ctsCopy = cts;

                    App.AddBackgroundTask(() =>
                    {
                        Task result;

                        if (DisposingOrDisposed || cts is null || cts.Token.IsCancellationRequested)
                            result = Task.CompletedTask;
                        else
                            result = new Task(ThreadAction, ctsCopy.Token);

                        result.ContinueWith((task) =>
                        {
                            Invoke(onComplete);
                        });

                        return result;
                    });
                }
            }

            /// <inheritdoc/>
            protected override void DisposeManaged()
            {
                Stop();
                App.DebugLogIf($"{SafeName} Disposed.", logControllerState);
            }

            private void ThreadAction()
            {
                var ctsCopy = cts ?? new();

                App.DebugLogIf($"{SafeName} Started.", logControllerState);
                ListBox.RemoveAll();
                ListBox.AddItemsThreadSafe(
                    SourceFunc(),
                    ConvertItemFunc,
                    () =>
                    {
                        if (ctsCopy.Token.IsCancellationRequested || ListBox.IsDisposed)
                            return false;
                        return ContinueFunc();
                    },
                    BufferSize,
                    SleepAfterBufferMsec);
                App.DebugLogIf($"{SafeName} Finished.", logControllerState);
                ThreadActionFinished?.Invoke();
            }
        }
    }
}
