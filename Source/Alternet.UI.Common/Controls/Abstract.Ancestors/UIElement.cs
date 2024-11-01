using System;
using System.ComponentModel;
using System.Diagnostics;
using Alternet.UI.Localization;
using Alternet.UI.Port;

namespace Alternet.UI
{
    /// <summary>
    /// <see cref="UIElement"/> is a base class for core level implementations building
    /// on elements and basic presentation characteristics.
    /// </summary>
    public class UIElement : DependencyObject
    {
        internal const int MAXELEMENTSINROUTE = 4096;

        /// <summary>
        /// Occurs when the layout of the various visual elements changes.
        /// </summary>
        public event EventHandler? LayoutUpdated;

        /// <summary>
        /// Raises <see cref="LayoutUpdated"/> event.
        /// </summary>
        [Browsable(false)]
        public void RaiseLayoutUpdated()
        {
            LayoutUpdated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Adds a handler for the given attached event
        /// </summary>
        [FriendAccessAllowed] // Built into Core, also used by Framework.
        internal static void AddHandler(
            DependencyObject d,
            RoutedEvent routedEvent,
            Delegate handler)
        {
            if (d == null)
            {
                throw new ArgumentNullException(nameof(d));
            }

            if (routedEvent is null)
            {
                throw new ArgumentNullException(nameof(routedEvent));
            }

            if (d is UIElement uiElement)
            {
                uiElement.AddHandler(routedEvent, handler);
            }
            else
                throw new ArgumentException(SR.Get(SRID.Invalid_IInputElement, d.GetType()));
        }

        /// <summary>
        ///     Removes a handler for the given attached event
        /// </summary>
        [FriendAccessAllowed] // Built into Core, also used by Framework.
        internal static void RemoveHandler(
            DependencyObject d,
            RoutedEvent routedEvent,
            Delegate handler)
        {
            if (d == null)
            {
                throw new ArgumentNullException(nameof(d));
            }

            if (routedEvent is null)
            {
                throw new ArgumentNullException(nameof(routedEvent));
            }

            if (d is UIElement uiElement)
                uiElement.RemoveHandler(routedEvent, handler);
            else
                throw new ArgumentException(SR.Get(SRID.Invalid_IInputElement, d.GetType()));
        }

        /// <summary>
        ///     Implementation of RaiseEvent.
        ///     Called by both the trusted and non-trusted flavors of RaiseEvent.
        /// </summary>
        internal static void RaiseEventImpl(DependencyObject sender, RoutedEventArgs args)
        {
            EventRoute route = EventRouteFactory.FetchObject(args.RoutedEvent);

            if (TraceRoutedEvent.IsEnabled)
            {
                TraceRoutedEvent.Trace(
                    TraceEventType.Start,
                    TraceRoutedEvent.RaiseEvent,
                    args.RoutedEvent,
                    sender,
                    args,
                    args.Handled);
            }

            try
            {
                // Set Source
                args.Source = sender;

                UIElement.BuildRouteHelper(sender, route, args);

                route.InvokeHandlers(sender, args);

                // Reset Source to OriginalSource
                args.Source = args.OriginalSource;
            }
            finally
            {
                if (TraceRoutedEvent.IsEnabled)
                {
                    TraceRoutedEvent.Trace(
                        TraceEventType.Stop,
                        TraceRoutedEvent.RaiseEvent,
                        args.RoutedEvent,
                        sender,
                        args,
                        args.Handled);
                }
            }

            EventRouteFactory.RecycleObject(route);
        }

        internal static void BuildRouteHelper(
            DependencyObject? e,
            EventRoute route,
            RoutedEventArgs args)
        {
            if (route == null)
            {
                throw new ArgumentNullException(nameof(route));
            }

            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            if (args.Source == null)
            {
                throw new ArgumentException(SR.Get(SRID.SourceNotSet));
            }

            if (args.RoutedEvent != route.RoutedEvent)
            {
                throw new ArgumentException(SR.Get(SRID.Mismatched_RoutedEvent));
            }

            // Route via visual tree
            if (args.RoutedEvent.RoutingStrategy == RoutingStrategy.Direct)
            {
                // Add this element to route
                if (e is UIElement uiElement)
                {
                    uiElement.AddToEventRoute(route, args);
                }
            }
            else
            {
                int cElements = 0;

                while (e != null)
                {
                    var uiElement = e as UIElement;

                    // Protect against infinite loops by limiting the number of elements
                    // that we will process.
                    if (cElements++ > MAXELEMENTSINROUTE)
                    {
                        throw new InvalidOperationException(SR.Get(SRID.TreeLoop));
                    }

                    // Allow the element to adjust source
                    object? newSource = null;
                    if (uiElement != null)
                    {
                        newSource = uiElement.AdjustEventSource(args);
                    }

                    // Add changed source information to the route
                    if (newSource != null)
                    {
                        route.AddSource(newSource);
                    }

                    // Invoke BuildRouteCore
                    if (uiElement != null)
                    {
                        /* yezo
                        // Add a Synchronized input pre-opportunity handler just before the class
                        // and instance handlers
                        uiElement.AddSynchronizedInputPreOpportunityHandler(route, args);
                        */

                        bool continuePastVisualTree = uiElement.BuildRouteCore(route, args);

                        // Add this element to route
                        uiElement.AddToEventRoute(route, args);

                        /* yezo
                        // Add a Synchronized input post-opportunity handler just after class
                        // and instance handlers
                        uiElement.AddSynchronizedInputPostOpportunityHandler(route, args);*/

                        // Get element's visual parent
                        e = uiElement.GetUIParent(continuePastVisualTree);
                    }

                    // If the BuildRouteCore implementation changed the
                    // args.Source to the route parent, respect it in
                    // the actual route.
                    if (e == args.Source)
                    {
                        route.AddSource(e);
                    }
                }
            }
        }

        /// <summary>
        ///     Raise the events specified by
        ///     <see cref="RoutedEventArgs.RoutedEvent"/>
        /// </summary>
        /// <param name="e">
        ///     <see cref="RoutedEventArgs"/> for the event to
        ///     be raised
        /// </param>
        internal void RaiseEvent(RoutedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            e.ClearUserInitiated();

            UIElement.RaiseEventImpl(this, e);
        }

        /// <summary>
        ///     Add the event handlers for this element to the route.
        /// </summary>
        internal void AddToEventRoute(EventRoute route, RoutedEventArgs e)
        {
            if (route == null)
            {
                throw new ArgumentNullException(nameof(route));
            }

            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            // Get class listeners for this UIElement
            RoutedEventHandlerInfoList classListeners =
                GlobalEventManager.GetDTypedClassListeners(this.DependencyObjectType, e.RoutedEvent);

            // Add all class listeners for this UIElement
            while (classListeners != null)
            {
                for (int i = 0; i < classListeners.Handlers.Length; i++)
                {
                    route.Add(
                        this,
                        classListeners.Handlers[i].Handler,
                        classListeners.Handlers[i].InvokeHandledEventsToo);
                }

                classListeners = classListeners.Next;
            }

            var store = EventHandlersStore;
            if (store != null)
            {
                // Get instance listeners for this UIElement
                FrugalObjectList<RoutedEventHandlerInfo>? instanceListeners = store[e.RoutedEvent];

                // Add all instance listeners for this UIElement
                if (instanceListeners != null)
                {
                    for (int i = 0; i < instanceListeners.Count; i++)
                    {
                        route.Add(
                            this,
                            instanceListeners[i].Handler,
                            instanceListeners[i].InvokeHandledEventsToo);
                    }
                }
            }

            // Allow Framework to add event handlers in styles
            AddToEventRouteCore(route, e);
        }

        /// <summary>
        ///     Allows UIElement to augment the
        ///     <see cref="EventRoute"/>
        /// </summary>
        /// <remarks>
        ///     Sub-classes of UIElement can override
        ///     this method to custom augment the route
        /// </remarks>
        /// <param name="route">
        ///     The <see cref="EventRoute"/> to be
        ///     augmented
        /// </param>
        /// <param name="args">
        ///     <see cref="RoutedEventArgs"/> for the
        ///     RoutedEvent to be raised post building
        ///     the route
        /// </param>
        /// <returns>
        ///     Whether or not the route should continue past the visual tree.
        ///     If this is true, and there are no more visual parents, the route
        ///     building code will call the GetUIParentCore method to find the
        ///     next non-visual parent.
        /// </returns>
        internal virtual bool BuildRouteCore(EventRoute route, RoutedEventArgs args)
        {
            return false;
        }

        /// <summary>
        ///     This virtual method is to be overridden in Framework
        ///     to be able to add handlers for styles
        /// </summary>
        internal virtual void AddToEventRouteCore(EventRoute route, RoutedEventArgs args)
        {
        }

        /// <summary>
        ///     Allows adjustment to the event source
        /// </summary>
        /// <remarks>
        ///     Subclasses must override this method
        ///     to be able to adjust the source during
        ///     route invocation <para/>
        ///
        ///     NOTE: Expected to return null when no
        ///     change is made to source
        /// </remarks>
        /// <param name="args">
        ///     Routed Event Args
        /// </param>
        /// <returns>
        ///     Returns new source
        /// </returns>
        internal virtual object? AdjustEventSource(RoutedEventArgs args)
        {
            return null;
        }

        internal virtual DependencyObject? GetUIParentCore()
        {
            return null;
        }

#pragma warning disable
        internal DependencyObject? GetUIParent(bool v)
#pragma warning enable
        {
            return GetUIParentCore();
        }

        /// <summary>
        ///     Re-raises an event with as a different RoutedEvent.
        /// </summary>
        /// <remarks>
        ///     Only used internally.  Added to support cracking generic MouseButtonDown/Up events
        ///     into MouseLeft/RightButtonDown/Up events.
        /// </remarks>
        private static void ReRaiseEventAs(
            DependencyObject sender,
            RoutedEventArgs args,
            RoutedEvent newEvent)
        {
            // Preseve and change the RoutedEvent
            RoutedEvent preservedRoutedEvent = args.RoutedEvent;
            args.OverrideRoutedEvent(newEvent);

            // Preserve Source
            object preservedSource = args.Source;

            EventRoute route = EventRouteFactory.FetchObject(args.RoutedEvent);

            if (TraceRoutedEvent.IsEnabled)
            {
                TraceRoutedEvent.Trace(
                    TraceEventType.Start,
                    TraceRoutedEvent.ReRaiseEventAs,
                    args.RoutedEvent,
                    sender,
                    args,
                    args.Handled);
            }

            try
            {
                // Build the route and invoke the handlers
                UIElement.BuildRouteHelper(sender, route, args);

                route.ReInvokeHandlers(sender, args);

                // Restore Source
                args.OverrideSource(preservedSource);

                // Restore RoutedEvent
                args.OverrideRoutedEvent(preservedRoutedEvent);
            }
            finally
            {
                if (TraceRoutedEvent.IsEnabled)
                {
                    TraceRoutedEvent.Trace(
                        TraceEventType.Stop,
                        TraceRoutedEvent.ReRaiseEventAs,
                        args.RoutedEvent,
                        sender,
                        args,
                        args.Handled);
                }
            }

            // Recycle the route object
            EventRouteFactory.RecycleObject(route);
        }

        // Helper method to retrieve and fire Clr Event handlers for DependencyPropertyChanged event
        private void RaiseDependencyPropertyChanged(
            EventPrivateKey key,
            DependencyPropertyChangedEventArgs args)
        {
            var store = EventHandlersStore;
            if (store != null)
            {
                Delegate handler = store.Get(key);
                if (handler != null)
                {
                    ((DependencyPropertyChangedEventHandler)handler)(this, args);
                }
            }
        }

        private void EventHandlersStoreAdd(EventPrivateKey key, Delegate handler)
        {
            EnsureEventHandlersStore();
            EventHandlersStore!.Add(key, handler);
        }

        private void EventHandlersStoreRemove(EventPrivateKey key, Delegate handler)
        {
            var store = EventHandlersStore;
            if (store != null)
            {
                store.Remove(key, handler);
                if (store.Count == 0)
                {
                    // last event handler was removed -- throw away underlying EventHandlersStore
                    EventHandlersStoreField.ClearValue(this);
                    WriteFlag(CoreFlags.ExistsEventHandlersStore, false);
                }
            }
        }

        /// <summary>
        ///     See overloaded method for details
        /// </summary>
        /// <remarks>
        ///     handledEventsToo defaults to false <para/>
        ///     See overloaded method for details
        /// </remarks>
        /// <param name="routedEvent"/>
        /// <param name="handler"/>
        internal void AddHandler(RoutedEvent routedEvent, Delegate handler)
        {
            // HandledEventToo defaults to false
            // Call forwarded
            AddHandler(routedEvent, handler, false);
        }

        /// <summary>
        ///     Adds a routed event handler for the particular
        ///     <see cref="RoutedEvent"/>
        /// </summary>
        /// <remarks>
        ///     The handler added thus is also known as
        ///     an instance handler <para/>
        ///     <para/>
        ///
        ///     NOTE: It is not an error to add a handler twice
        ///     (handler will simply be called twice) <para/>
        ///     <para/>
        ///
        ///     Input parameters <see cref="RoutedEvent"/>
        ///     and handler cannot be null <para/>
        ///     handledEventsToo input parameter when false means
        ///     that listener does not care about already handled events.
        ///     Hence the handler will not be invoked on the target if
        ///     the RoutedEvent has already been
        ///     <see cref="RoutedEventArgs.Handled"/> <para/>
        ///     handledEventsToo input parameter when true means
        ///     that the listener wants to hear about all events even if
        ///     they have already been handled. Hence the handler will
        ///     be invoked irrespective of the event being
        ///     <see cref="RoutedEventArgs.Handled"/>
        /// </remarks>
        /// <param name="routedEvent">
        ///     <see cref="RoutedEvent"/> for which the handler
        ///     is attached
        /// </param>
        /// <param name="handler">
        ///     The handler that will be invoked on this object
        ///     when the RoutedEvent is raised
        /// </param>
        /// <param name="handledEventsToo">
        ///     Flag indicating whether or not the listener wants to
        ///     hear about events that have already been handled
        /// </param>
        internal void AddHandler(
            RoutedEvent routedEvent,
            Delegate handler,
            bool handledEventsToo)
        {
            if (routedEvent == null)
            {
                throw new ArgumentNullException(nameof(routedEvent));
            }

            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            if (!routedEvent.IsLegalHandler(handler))
            {
                throw new ArgumentException(SR.Get(SRID.HandlerTypeIllegal));
            }

            EnsureEventHandlersStore();
            EventHandlersStore!.AddRoutedEventHandler(routedEvent, handler, handledEventsToo);

            OnAddHandler(routedEvent, handler);
        }

        /// <summary>
        ///     Notifies subclass of a new routed event handler.  Note that this is
        ///     called once for each handler added, but OnRemoveHandler is only called
        ///     on the last removal.
        /// </summary>
        internal virtual void OnAddHandler(
            RoutedEvent routedEvent,
            Delegate handler)
        {
        }

        /// <summary>
        ///     Removes all instances of the specified routed
        ///     event handler for this object instance
        /// </summary>
        /// <remarks>
        ///     The handler removed thus is also known as
        ///     an instance handler <para/>
        ///     <para/>
        ///
        ///     NOTE: This method does nothing if there were
        ///     no handlers registered with the matching
        ///     criteria <para/>
        ///     <para/>
        ///
        ///     Input parameters <see cref="RoutedEvent"/>
        ///     and handler cannot be null <para/>
        ///     This method ignores the handledEventsToo criterion
        /// </remarks>
        /// <param name="routedEvent">
        ///     <see cref="RoutedEvent"/> for which the handler
        ///     is attached
        /// </param>
        /// <param name="handler">
        ///     The handler for this object instance to be removed
        /// </param>
        internal void RemoveHandler(RoutedEvent routedEvent, Delegate handler)
        {
            if (routedEvent == null)
            {
                throw new ArgumentNullException(nameof(routedEvent));
            }

            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            if (!routedEvent.IsLegalHandler(handler))
            {
                throw new ArgumentException(SR.Get(SRID.HandlerTypeIllegal));
            }

            var store = EventHandlersStore;
            if (store != null)
            {
                store.RemoveRoutedEventHandler(routedEvent, handler);

                OnRemoveHandler(routedEvent, handler);

                if (store.Count == 0)
                {
                    // last event handler was removed -- throw away underlying EventHandlersStore
                    EventHandlersStoreField.ClearValue(this);
                    WriteFlag(CoreFlags.ExistsEventHandlersStore, false);
                }
            }
        }

        /// <summary>
        ///     Notifies subclass of an event for which a handler has been removed.
        /// </summary>
        internal virtual void OnRemoveHandler(
            RoutedEvent routedEvent,
            Delegate handler)
        {
        }

        /// <summary>
        ///     Ensures that EventHandlersStore will return
        ///     non-null when it is called.
        /// </summary>
        [FriendAccessAllowed] // Built into Core, also used by Framework.
        internal void EnsureEventHandlersStore()
        {
            if (EventHandlersStore == null)
            {
                EventHandlersStoreField.SetValue(this, new EventHandlersStore());
                WriteFlag(CoreFlags.ExistsEventHandlersStore, true);
            }
        }

        internal void WriteFlag(CoreFlags field, bool value)
        {
            if (value)
            {
                coreFlags |= field;
            }
            else
            {
                coreFlags &= ~field;
            }
        }

        private CoreFlags coreFlags;

        internal bool ReadFlag(CoreFlags field)
        {
            return (coreFlags & field) != 0;
        }

        /// <summary>
        ///     Event Handlers Store
        /// </summary>
        /// <remarks>
        ///     The idea of exposing this property is to allow
        ///     elements in the Framework to generically use
        ///     EventHandlersStore for Clr events as well.
        /// </remarks>
        internal EventHandlersStore? EventHandlersStore
        {
            [FriendAccessAllowed] // Built into Core, also used by Framework.
            get
            {
                if (!ReadFlag(CoreFlags.ExistsEventHandlersStore))
                {
                    return null;
                }

                return EventHandlersStoreField.GetValue(this);
            }
        }

        internal static readonly UncommonField<EventHandlersStore> EventHandlersStoreField = new ();
    }
}