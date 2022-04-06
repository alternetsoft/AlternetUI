using Alternet.UI.Internal.KnownBoxes;
using System;
using System.Diagnostics;

namespace Alternet.UI
{
    /// <summary>
    /// <see cref="UIElement"/> is a base class for core level implementations building on elements and basic presentation characteristics.
    /// </summary>
    public class UIElement : DependencyObject
    {
        /// <summary>
        ///     Alias to the Keyboard.PreviewKeyDownEvent.
        /// </summary>
        public static readonly RoutedEvent PreviewKeyDownEvent = Keyboard.PreviewKeyDownEvent.AddOwner(typeof(UIElement));

        /// <summary>
        ///     Event reporting a key was pressed
        /// </summary>
        public event KeyEventHandler PreviewKeyDown
        {
            add { AddHandler(Keyboard.PreviewKeyDownEvent, value, false); }
            remove { RemoveHandler(Keyboard.PreviewKeyDownEvent, value); }
        }

        /// <summary>
        ///     Alias to the Keyboard.KeyDownEvent.
        /// </summary>
        public static readonly RoutedEvent KeyDownEvent = Keyboard.KeyDownEvent.AddOwner(typeof(UIElement));

        /// <summary>
        ///     Event reporting a key was pressed
        /// </summary>
        public event KeyEventHandler KeyDown
        {
            add { AddHandler(Keyboard.KeyDownEvent, value, false); }
            remove { RemoveHandler(Keyboard.KeyDownEvent, value); }
        }

        /// <summary>
        ///     Alias to the Keyboard.PreviewKeyUpEvent.
        /// </summary>
        public static readonly RoutedEvent PreviewKeyUpEvent = Keyboard.PreviewKeyUpEvent.AddOwner(typeof(UIElement));

        /// <summary>
        ///     Event reporting a key was released
        /// </summary>
        public event KeyEventHandler PreviewKeyUp
        {
            add { AddHandler(Keyboard.PreviewKeyUpEvent, value, false); }
            remove { RemoveHandler(Keyboard.PreviewKeyUpEvent, value); }
        }

        /// <summary>
        ///     Alias to the Keyboard.KeyUpEvent.
        /// </summary>
        public static readonly RoutedEvent KeyUpEvent = Keyboard.KeyUpEvent.AddOwner(typeof(UIElement));

        /// <summary>
        ///     Event reporting a key was released
        /// </summary>
        public event KeyEventHandler KeyUp
        {
            add { AddHandler(Keyboard.KeyUpEvent, value, false); }
            remove { RemoveHandler(Keyboard.KeyUpEvent, value); }
        }

        /// <summary>
        ///     The DependencyProperty for the Focusable property.
        /// </summary>
        [CommonDependencyProperty]
        public static readonly DependencyProperty FocusableProperty =
                DependencyProperty.Register(
                        "Focusable",
                        typeof(bool),
                        typeof(UIElement),
                        new UIPropertyMetadata(
                                BooleanBoxes.FalseBox, // default value
                                new PropertyChangedCallback(OnFocusableChanged)));

        /// <summary>
        ///     Gettor and Settor for Focusable Property
        /// </summary>
        public bool Focusable
        {
            get { return (bool)GetValue(FocusableProperty); }
            set { SetValue(FocusableProperty, BooleanBoxes.Box(value)); }
        }

        /// <summary>
        ///     FocusableChanged event
        /// </summary>
        public event DependencyPropertyChangedEventHandler FocusableChanged
        {
            add { EventHandlersStoreAdd(FocusableChangedKey, value); }
            remove { EventHandlersStoreRemove(FocusableChangedKey, value); }
        }
        internal static readonly EventPrivateKey FocusableChangedKey = new EventPrivateKey(); // Used by ContentElement

        private static void OnFocusableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIElement uie = (UIElement)d;

            // Raise the public changed event.
            uie.RaiseDependencyPropertyChanged(FocusableChangedKey, e);
        }

        /// <summary>
        ///     The DependencyProperty for the IsFocused property.
        /// </summary>
        internal static readonly DependencyPropertyKey IsFocusedPropertyKey =
                    DependencyProperty.RegisterReadOnly(
                                "IsFocused",
                                typeof(bool),
                                typeof(UIElement),
                                new PropertyMetadata(
                                            BooleanBoxes.FalseBox, // default value
                                            new PropertyChangedCallback(IsFocused_Changed)));

        /// <summary>
        ///     The DependencyProperty for IsFocused.
        ///     Flags:              None
        ///     Read-Only:          true
        /// </summary>
        public static readonly DependencyProperty IsFocusedProperty
            = IsFocusedPropertyKey.DependencyProperty;

        private static void IsFocused_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIElement uiElement = ((UIElement)d);

            if ((bool)e.NewValue)
            {
                uiElement.OnGotFocus(new RoutedEventArgs(GotFocusEvent, uiElement));
            }
            else
            {
                uiElement.OnLostFocus(new RoutedEventArgs(LostFocusEvent, uiElement));
            }
        }

        /// <summary>
        ///     This method is invoked when the IsFocused property changes to true
        /// </summary>
        /// <param name="e">RoutedEventArgs</param>
        protected virtual void OnGotFocus(RoutedEventArgs e)
        {
            RaiseEvent(e);
        }

        /// <summary>
        ///     This method is invoked when the IsFocused property changes to false
        /// </summary>
        /// <param name="e">RoutedEventArgs</param>
        protected virtual void OnLostFocus(RoutedEventArgs e)
        {
            RaiseEvent(e);
        }

        /// <summary>
        ///     GotFocus event
        /// </summary>
        public static readonly RoutedEvent GotFocusEvent = FocusManager.GotFocusEvent.AddOwner(typeof(UIElement));

        /// <summary>
        ///     An event announcing that IsFocused changed to true.
        /// </summary>
        public event RoutedEventHandler GotFocus
        {
            add { AddHandler(GotFocusEvent, value); }
            remove { RemoveHandler(GotFocusEvent, value); }
        }

        /// <summary>
        ///     LostFocus event
        /// </summary>
        public static readonly RoutedEvent LostFocusEvent = FocusManager.LostFocusEvent.AddOwner(typeof(UIElement));

        /// <summary>
        ///     An event announcing that IsFocused changed to false.
        /// </summary>
        public event RoutedEventHandler LostFocus
        {
            add { AddHandler(LostFocusEvent, value); }
            remove { RemoveHandler(LostFocusEvent, value); }
        }

        // Helper method to retrieve and fire Clr Event handlers for DependencyPropertyChanged event
        private void RaiseDependencyPropertyChanged(EventPrivateKey key, DependencyPropertyChangedEventArgs args)
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
        /// Occurs when the layout of the various visual elements changes.
        /// </summary>
        public event EventHandler? LayoutUpdated;

        internal void RaiseLayoutUpdated()
        {
            LayoutUpdated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Adds a handler for the given attached event
        /// </summary>
        [FriendAccessAllowed] // Built into Core, also used by Framework.
        internal static void AddHandler(DependencyObject d, RoutedEvent routedEvent, Delegate handler)
        {
            if (d == null)
            {
                throw new ArgumentNullException("d");
            }

            if (routedEvent is null)
            {
                throw new ArgumentNullException(nameof(routedEvent));
            }

            var uiElement = d as UIElement;
            if (uiElement != null)
            {
                uiElement.AddHandler(routedEvent, handler);
            }
            else
                throw new ArgumentException(SR.Get(SRID.Invalid_IInputElement, d.GetType()));
            //{
            //    ContentElement contentElement = d as ContentElement;
            //    if (contentElement != null)
            //    {
            //        contentElement.AddHandler(routedEvent, handler);
            //    }
            //    else
            //    {
            //        UIElement3D uiElement3D = d as UIElement3D;
            //        if (uiElement3D != null)
            //        {
            //            uiElement3D.AddHandler(routedEvent, handler);
            //        }
            //        else
            //        {
            //            throw new ArgumentException(SR.Get(SRID.Invalid_IInputElement, d.GetType()));
            //        }
            //    }
            //}
        }

        /// <summary>
        ///     Removes a handler for the given attached event
        /// </summary>
        [FriendAccessAllowed] // Built into Core, also used by Framework.
        internal static void RemoveHandler(DependencyObject d, RoutedEvent routedEvent, Delegate handler)
        {
            if (d == null)
            {
                throw new ArgumentNullException("d");
            }

            if (routedEvent is null)
            {
                throw new ArgumentNullException(nameof(routedEvent));
            }

            var uiElement = d as UIElement;
            if (uiElement != null)
                uiElement.RemoveHandler(routedEvent, handler);
            else
                throw new ArgumentException(SR.Get(SRID.Invalid_IInputElement, d.GetType()));
            //else
            //{
            //    ContentElement contentElement = d as ContentElement;
            //    if (contentElement != null)
            //    {
            //        contentElement.RemoveHandler(routedEvent, handler);
            //    }
            //    else
            //    {
            //        UIElement3D uiElement3D = d as UIElement3D;
            //        if (uiElement3D != null)
            //        {
            //            uiElement3D.RemoveHandler(routedEvent, handler);
            //        }
            //        else
            //        {
            //            throw new ArgumentException(SR.Get(SRID.Invalid_IInputElement, d.GetType()));
            //        }
            //    }
            //}
        }

        /// <summary>
        ///     Raise the events specified by
        ///     <see cref="RoutedEventArgs.RoutedEvent"/>
        /// </summary>
        /// <param name="e">
        ///     <see cref="RoutedEventArgs"/> for the event to
        ///     be raised
        /// </param>
        public void RaiseEvent(RoutedEventArgs e)
        {
            // VerifyAccess();

            if (e == null)
            {
                throw new ArgumentNullException("e");
            }
            e.ClearUserInitiated();

            UIElement.RaiseEventImpl(this, e);
        }

        internal const int MAX_ELEMENTS_IN_ROUTE = 4096;

        /// <summary>
        ///     Add the event handlers for this element to the route.
        /// </summary>
        public void AddToEventRoute(EventRoute route, RoutedEventArgs e)
        {
            if (route == null)
            {
                throw new ArgumentNullException("route");
            }
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }

            // Get class listeners for this UIElement
            RoutedEventHandlerInfoList classListeners =
                GlobalEventManager.GetDTypedClassListeners(this.DependencyObjectType, e.RoutedEvent);

            // Add all class listeners for this UIElement
            while (classListeners != null)
            {
                for (int i = 0; i < classListeners.Handlers.Length; i++)
                {
                    route.Add(this, classListeners.Handlers[i].Handler, classListeners.Handlers[i].InvokeHandledEventsToo);
                }

                classListeners = classListeners.Next;
            }

            // Get instance listeners for this UIElement
            FrugalObjectList<RoutedEventHandlerInfo>? instanceListeners = null;
            var store = EventHandlersStore;
            if (store != null)
            {
                instanceListeners = store[e.RoutedEvent];

                // Add all instance listeners for this UIElement
                if (instanceListeners != null)
                {
                    for (int i = 0; i < instanceListeners.Count; i++)
                    {
                        route.Add(this, instanceListeners[i].Handler, instanceListeners[i].InvokeHandledEventsToo);
                    }
                }
            }

            // Allow Framework to add event handlers in styles
            AddToEventRouteCore(route, e);
        }

        /// <summary>
        ///     This virtual method is to be overridden in Framework
        ///     to be able to add handlers for styles
        /// </summary>
        internal virtual void AddToEventRouteCore(EventRoute route, RoutedEventArgs args)
        {
        }

        internal static void BuildRouteHelper(DependencyObject? e, EventRoute route, RoutedEventArgs args)
        {
            if (route == null)
            {
                throw new ArgumentNullException("route");
            }

            if (args == null)
            {
                throw new ArgumentNullException("args");
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
                var uiElement = e as UIElement;

                // Add this element to route
                if (uiElement != null)
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
                    if (cElements++ > MAX_ELEMENTS_IN_ROUTE)
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
                    bool continuePastVisualTree = false;
                    if (uiElement != null)
                    {
                        /* yezo
                        //Add a Synchronized input pre-opportunity handler just before the class and instance handlers
                        uiElement.AddSynchronizedInputPreOpportunityHandler(route, args);
                        */

                        continuePastVisualTree = uiElement.BuildRouteCore(route, args);

                        // Add this element to route
                        uiElement.AddToEventRoute(route, args);

                        /* yezo
                        //Add a Synchronized input post-opportunity handler just after class and instance handlers
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

        internal virtual DependencyObject? GetUIParentCore()
        {
            return null;
        }

        internal DependencyObject? GetUIParent(bool v)
        {
            return GetUIParentCore();
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
        public void AddHandler(RoutedEvent routedEvent, Delegate handler)
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
        public void AddHandler(
            RoutedEvent routedEvent,
            Delegate handler,
            bool handledEventsToo)
        {
            // VerifyAccess();

            if (routedEvent == null)
            {
                throw new ArgumentNullException("routedEvent");
            }

            if (handler == null)
            {
                throw new ArgumentNullException("handler");
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
        public void RemoveHandler(RoutedEvent routedEvent, Delegate handler)
        {
            // VerifyAccess();

            if (routedEvent == null)
            {
                throw new ArgumentNullException("routedEvent");
            }

            if (handler == null)
            {
                throw new ArgumentNullException("handler");
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
                _flags |= field;
            }
            else
            {
                _flags &= (~field);
            }
        }

        private CoreFlags _flags;

        internal bool ReadFlag(CoreFlags field)
        {
            return (_flags & field) != 0;
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

        internal static readonly UncommonField<EventHandlersStore> EventHandlersStoreField = new UncommonField<EventHandlersStore>();
    }

    [Flags]
    internal enum CoreFlags : uint
    {
        None = 0x00000000,
        SnapsToDevicePixelsCache = 0x00000001,
        ClipToBoundsCache = 0x00000002,
        MeasureDirty = 0x00000004,
        ArrangeDirty = 0x00000008,
        MeasureInProgress = 0x00000010,
        ArrangeInProgress = 0x00000020,
        NeverMeasured = 0x00000040,
        NeverArranged = 0x00000080,
        MeasureDuringArrange = 0x00000100,
        IsCollapsed = 0x00000200,
        IsKeyboardFocusWithinCache = 0x00000400,
        IsKeyboardFocusWithinChanged = 0x00000800,
        IsMouseOverCache = 0x00001000,
        IsMouseOverChanged = 0x00002000,
        IsMouseCaptureWithinCache = 0x00004000,
        IsMouseCaptureWithinChanged = 0x00008000,
        IsStylusOverCache = 0x00010000,
        IsStylusOverChanged = 0x00020000,
        IsStylusCaptureWithinCache = 0x00040000,
        IsStylusCaptureWithinChanged = 0x00080000,
        HasAutomationPeer = 0x00100000,
        RenderingInvalidated = 0x00200000,
        IsVisibleCache = 0x00400000,
        AreTransformsClean = 0x00800000,
        IsOpacitySuppressed = 0x01000000,
        ExistsEventHandlersStore = 0x02000000,
        TouchesOverCache = 0x04000000,
        TouchesOverChanged = 0x08000000,
        TouchesCapturedWithinCache = 0x10000000,
        TouchesCapturedWithinChanged = 0x20000000,
        TouchLeaveCache = 0x40000000,
        TouchEnterCache = 0x80000000,
    }
}