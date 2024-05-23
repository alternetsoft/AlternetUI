#nullable disable

// Manager for the CanExecuteChanged event in the "weak event listener"
// pattern. See WeakEventTable.cs for an overview.
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Alternet.UI.Internal;

namespace Alternet.UI
{
    /// <summary>
    /// Manager for the ICommand.CanExecuteChanged event.
    /// </summary>
    public class CanExecuteChangedEventManager : WeakEventManager
    {
        private static readonly object StaticSource = new NamedObject("StaticSource");
#pragma warning disable
        private readonly ConditionalWeakTable<object, object> cwt = new();
#pragma warning restore

        private CanExecuteChangedEventManager()
        {
        }

        // get the event manager for the current thread
        private static CanExecuteChangedEventManager CurrentManager
        {
            get
            {
                Type managerType = typeof(CanExecuteChangedEventManager);
                CanExecuteChangedEventManager manager = (CanExecuteChangedEventManager)GetCurrentManager(managerType);

                // at first use, create and register a new manager
                if (manager == null)
                {
                    manager = new CanExecuteChangedEventManager();
                    SetCurrentManager(managerType, manager);
                }

                return manager;
            }
        }

        /// <summary>
        /// Add a handler for the given source's event.
        /// </summary>
        public static void AddHandler(ICommand source, EventHandler<EventArgs> handler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            CurrentManager.PrivateAddHandler(source, handler);
        }

        /// <summary>
        /// Remove a handler for the given source's event.
        /// </summary>
        public static void RemoveHandler(ICommand source, EventHandler<EventArgs> handler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            CurrentManager.PrivateRemoveHandler(source, handler);
        }

        /// <summary>
        /// Listen to the given source for the event.
        /// </summary>
        protected override void StartListening(object source)
        {
            // never called
        }

        /// <summary>
        /// Stop listening to the given source for the event.
        /// </summary>
        protected override void StopListening(object source)
        {
            // never called
        }

        /// <inheritdoc/>
        protected override bool Purge(object source, object data, bool purgeAll)
        {
            // Incremental cleanups (!purgeAll) are guaranteed to be on the right thread,
            // but the final cleanup (purgeAll) can happen on a different thread.
            bool isOnOriginalThread = !purgeAll || CheckAccess();

#pragma warning disable
            ICommand command = source as ICommand;
#pragma warning restore
            List<HandlerSink> list = data as List<HandlerSink>;
            List<HandlerSink> toRemove = null;

#pragma warning disable
            bool foundDirt = false;
#pragma warning restore
            bool removeList = purgeAll || source == null;

            // find dead entries to be removed from the list
            if (!removeList)
            {
                foreach (HandlerSink sink in list)
                {
                    if (sink.IsInactive)
                    {
                        toRemove ??= new();

                        toRemove.Add(sink);
                    }
                }

                removeList = toRemove != null && toRemove.Count == list.Count;
            }

            if (removeList)
            {
                toRemove = list;
            }

            foundDirt = toRemove != null;

            // if the whole list is going away, remove the data (unless parent table
            // is already doing that for us - purgeAll=true)
            if (removeList && !purgeAll && source != null)
            {
                Remove(source);
            }

            // remove and detach the dead entries
            if (foundDirt)
            {
                foreach (HandlerSink sink in toRemove)
                {
                    EventHandler<EventArgs> handler = sink.Handler;
                    sink.Detach(isOnOriginalThread);

                    // if list is going away, no need to remove from it
                    if (!removeList)
                    {
                        list.Remove(sink);
                    }

                    if (handler != null)
                    {
                        RemoveHandlerFromCWT(handler, cwt);
                    }
                }
            }

            return foundDirt;
        }

        private void PrivateAddHandler(ICommand source, EventHandler<EventArgs> handler)
        {
            // get the list of sinks for this source, creating if necessary
            List<HandlerSink> list = (List<HandlerSink>)this[source];
            if (list == null)
            {
                list = new();
                this[source] = list;
            }

            // add a new sink to the list
            HandlerSink sink = new(this, source, handler);
            list.Add(sink);

            // keep the handler alive
            AddHandlerToCWT(handler, cwt);
        }

        private void PrivateRemoveHandler(ICommand source, EventHandler<EventArgs> handler)
        {
            // get the list of sinks for this source
            List<HandlerSink> list = (List<HandlerSink>)this[source];
            if (list != null)
            {
                HandlerSink sinkToRemove = null;
                bool foundDirt = false;

                // look for the given sink on the list
                foreach (HandlerSink sink in list)
                {
                    if (sink.Matches(source, handler))
                    {
                        sinkToRemove = sink;
                        break;
                    }
                    else if (sink.IsInactive)
                    {
                        foundDirt = true;
                    }
                }

                // remove the sink (outside the loop, to avoid re-entrancy issues)
                if (sinkToRemove != null)
                {
                    list.Remove(sinkToRemove);
                    sinkToRemove.Detach(isOnOriginalThread: true);
                    RemoveHandlerFromCWT(handler, cwt);
                }

                // if we noticed any stale sinks, schedule a purge
                if (foundDirt)
                {
                    ScheduleCleanup();
                }
            }
        }

        // add the handler to the CWT - this keeps the handler alive throughout
        // the lifetime of the target, without prolonging the lifetime of
        // the target
        private void AddHandlerToCWT(Delegate handler, ConditionalWeakTable<object, object> cwt)
        {
            object target = handler.Target ?? StaticSource;
            if (!cwt.TryGetValue(target, out object value))
            {
                // 99% case - the target only listens once
                cwt.Add(target, handler);
            }
            else
            {
                // 1% case - the target listens multiple times
                // we store the delegates in a list
                if (value is not List<Delegate> list)
                {
                    // lazily allocate the list, and add the old handler
                    Delegate oldHandler = value as Delegate;
#pragma warning disable
                    list = new List<Delegate>();
#pragma warning restore
                    list.Add(oldHandler);

                    // install the list as the CWT value
                    cwt.Remove(target);
                    cwt.Add(target, list);
                }

                // add the new handler to the list
                list.Add(handler);
            }
        }

#pragma warning disable
        private void RemoveHandlerFromCWT(Delegate handler, ConditionalWeakTable<object, object> cwt)
#pragma warning restore
        {
            object target = handler.Target;
            target ??= StaticSource;

            if (this.cwt.TryGetValue(target, out object value))
            {
                if (value is not List<Delegate> list)
                {
                    // 99% case - the target is removing its single handler
                    this.cwt.Remove(target);
                }
                else
                {
                    // 1% case - the target had multiple handlers, and is removing one
                    list.Remove(handler);
                    if (list.Count == 0)
                    {
                        this.cwt.Remove(target);
                    }
                }
            }
        }

        // Some sources delegate their CanExecuteChanged event to another event
        // on a different object.  For example, RoutedCommands delegate to
        // CommandManager.RequerySuggested, as do some custom commands (dev11 281808).
        // Similarly, some 3rd-party commands delegate to a custom class (dev11 449384).
        // The standard weak-event pattern won't work in these cases.  It registers
        // the source at AddHandler-time, and uses the 'sender' argument at event-delivery
        // time to look up the relevant information;  if these are different, we won't find
        // the information and therefore won't deliver the event to the intended listeners.
        //
        // To cope with this, we use the HandlerSink class.  Each call to AddHandler
        // creates a new HandlerSink, in which we record the original source and
        // handler, and register a local listener.  When the event is raised to a
        // sink's local listener, the sink merely passes the event along to the original
        // handler.  With judicious use of WeakReference and ConditionalWeakTable,
        // we can do this without extending the lifetime of the original source or
        // listener, and without leaking any of the internal data structures.

        // Here's a diagram illustrating a Button listening to CanExecuteChanged from
        // a Command that delegates to some other event Proxy.Foo.
        //
        // Button  --*-->  OriginalHandler <---o--- Sink --------o------------> Command
        //   ^                   |                  ^  ^
        //   ---------------------                  |  |
        //                               List -------  ----> LocalHandler <---- Proxy.Foo
        //            Table:  (Command) ---^
        //
        // Legend:  Weak reference:    --o-->
        //          CWT reference:     --*-->
        //          Strong reference:  ----->
        //
        // For each source (i.e. Command), the Manager stores a list of Sinks pertaining
        // to that Command.  Each Sink remembers (weakly) its original source and handler,
        // and registers for the Command.CanExecuteChanged event in the normal way.
        // The event may be raised from a different place (Proxy.Foo), but the Sink
        // knows to pass the event along to the original handler.   The Manager uses a
        // ConditionalWeakTable to keep the original handlers alive, and the Sink
        // keeps its local handler alive with a local strong reference.

        // The internal data structures (Sink, List entry, LocalHandler) can be purged
        // when either the Button or the Command is GC'd.  Both conditions are
        // testable by querying the Sink's weak references.
        private class HandlerSink
        {
            private readonly CanExecuteChangedEventManager manager;
            private readonly EventHandler onCanExecuteChangedHandler;   // see remarks in the constructor
            private WeakReference source;
            private WeakReference originalHandler;

            public HandlerSink(CanExecuteChangedEventManager manager, ICommand source, EventHandler<EventArgs> originalHandler)
            {
                this.manager = manager;
                this.source = new WeakReference(source);
                this.originalHandler = new WeakReference(originalHandler);

                // In WPF 4.0, elements with commands (Button, Hyperlink, etc.) listened
                // for CanExecuteChanged and also stored a strong reference to the handler
                // (in an uncommon field).   Some third-party commands relied on this
                // undocumented implementation detail by storing a weak reference to
                // the handler.  (One such example is Win8 Server Manager's DelegateCommand -
                // Microsoft.Management.UI.DelegateCommand<T> - see Win8 Bugs 588129.)
                //
                // Commands that do this won't work with normal listeners:   the listener
                // simply calls command.CanExecuteChanged += new EventHandler(MyMethod);
                // the command stores a weak-ref to the handler, no one has a strong-ref
                // so the handler is soon GC'd, after which the event doesn't get
                // delivered to the listener.
                //
                // In WPF 4.5, Button et al. use this weak event manager to listen to
                // CanExecuteChanged, indirectly.  Only the manager actually listens
                // directly to the command's event.  For compat, the manager stores a
                // strong reference to its handler.   The only reason for this is to
                // support those commands that relied on the 4.0 implementation.
                onCanExecuteChangedHandler = new EventHandler(OnCanExecuteChanged);

                // BTW, the reason commands used weak-references was to avoid leaking
                // the Button.   This is fixed in 4.5, precisely
                // by using the weak-event pattern.   Commands can now implement
                // the CanExecuteChanged event the default way - no need for any
                // fancy weak-reference tricks (which people usually get wrong in
                // general, as in the case of DelegateCommand<T>).

                // register the local listener
                source.CanExecuteChanged += onCanExecuteChangedHandler;
            }

            public bool IsInactive
            {
                get
                {
                    return source == null || !source.IsAlive
                        || originalHandler == null || !originalHandler.IsAlive;
                }
            }

            public EventHandler<EventArgs> Handler
            {
                get { return (originalHandler != null) ? (EventHandler<EventArgs>)originalHandler.Target : null; }
            }

            public bool Matches(ICommand source, EventHandler<EventArgs> handler)
            {
                return (this.source != null && (ICommand)this.source.Target == source) &&
                        (originalHandler != null && (EventHandler<EventArgs>)originalHandler.Target == handler);
            }

            public void Detach(bool isOnOriginalThread)
            {
                if (source != null)
                {
                    ICommand target = (ICommand)source.Target;
                    if (target != null && isOnOriginalThread)
                    {
                        // some sources delegate the event to another weak-event
                        // manager, using thread-static information (CurrentManager)
                        // along the way;  e.g. all built-in RoutedCommands.
                        // If we're on the wrong thread, bypass this step as it
                        // would create a new WeakEventTable and Dispatcher on
                        // the wrong thread, causing problems at shutdown
                        target.CanExecuteChanged -= onCanExecuteChangedHandler;
                    }

                    this.source = null;
                    originalHandler = null;
                }
            }

            private void OnCanExecuteChanged(object sender, EventArgs e)
            {
                // this protects against re-entrancy:  a purge happening
                // while a CanExecuteChanged event is being delivered
                if (source == null)
                    return;

                // if the sender is our own CommandManager, the original
                // source delegated is CanExecuteChanged event to
                // CommandManager.RequerySuggested.  We use the original
                // source as the sender when passing the event along, so that
                // listeners can distinguish which command is changing.
                // We could do that for 3rd-party commands that delegate as
                // well, but we don't for compat with 4.0.
                // if (sender is CommandManager)
                // {
                //    sender = _source.Target;
                // }

                // pass the event along to the original listener
                EventHandler<EventArgs> handler = (EventHandler<EventArgs>)originalHandler.Target;
                if (handler != null)
                {
                    handler(sender, e);
                }
                else
                {
                    // listener has been GC'd - schedule a purge
                    manager.ScheduleCleanup();
                }
            }
        }
    }
}