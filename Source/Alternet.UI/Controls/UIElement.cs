using Alternet.Drawing;
using Alternet.UI.Input;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;

namespace Alternet.UI
{
    public abstract class UIElement : DependencyObject
    {
        internal const int MAX_ELEMENTS_IN_ROUTE = 4096;
        internal static readonly UncommonField<EventHandlersStore> EventHandlersStoreField = new UncommonField<EventHandlersStore>();
        internal SizeChangedInfo sizeChangedInfo;

        internal ContextLayoutManager.LayoutQueue.Request MeasureRequest;
        internal ContextLayoutManager.LayoutQueue.Request ArrangeRequest;
        private const uint TreeLevelLimit = 0x7FF;
        private Size _previousAvailableSize;
        private Rect _finalRect;
        private Size _size;

        private uint treeLevel;

        private Size _desiredSize;

        private bool visible = true;

        private CoreFlags _flags;

        public UIElement()
        {
            Initialize();
        }

        /// <summary>
        /// Rounds a size to integer values for layout purposes, compensating for high DPI screen coordinates.
        /// </summary>
        /// <param name="size">Input size.</param>
        /// <param name="dpiScaleX">DPI along x-dimension.</param>
        /// <param name="dpiScaleY">DPI along y-dimension.</param>
        /// <returns>Value of size that will be rounded under screen DPI.</returns>
        /// <remarks>This is a layout helper method. It takes DPI into account and also does not return
        /// the rounded value if it is unacceptable for layout, e.g. Infinity or NaN. It's a helper associated with
        /// UseLayoutRounding  property and should not be used as a general rounding utility.</remarks>
        internal static Size RoundLayoutSize(Size size, double dpiScaleX, double dpiScaleY)
        {
            return new Size(RoundLayoutValue(size.Width, dpiScaleX), RoundLayoutValue(size.Height, dpiScaleY));
        }

        private static readonly UncommonField<EventHandler> LayoutUpdatedHandlersField = new UncommonField<EventHandler>();
        private static readonly UncommonField<object> LayoutUpdatedListItemsField = new UncommonField<object>();

        /// <summary>
        /// Occurs when the value of the <see cref="Visible"/> property changes.
        /// </summary>
        public event EventHandler? VisibleChanged;

        private LayoutEventList.ListItem getLayoutUpdatedHandler(EventHandler d)
        {
            object cachedLayoutUpdatedItems = LayoutUpdatedListItemsField.GetValue(this);

            if (cachedLayoutUpdatedItems == null)
            {
                return null;
            }
            else
            {
                EventHandler cachedLayoutUpdatedHandler = LayoutUpdatedHandlersField.GetValue(this);
                if (cachedLayoutUpdatedHandler != null)
                {
                    if (cachedLayoutUpdatedHandler == d) return (LayoutEventList.ListItem)cachedLayoutUpdatedItems;
                }
                else //already have a list
                {
                    Hashtable list = (Hashtable)cachedLayoutUpdatedItems;
                    LayoutEventList.ListItem item = (LayoutEventList.ListItem)(list[d]);
                    return item;
                }
                return null;
            }
        }

        private void addLayoutUpdatedHandler(EventHandler handler, LayoutEventList.ListItem item)
        {
            object cachedLayoutUpdatedItems = LayoutUpdatedListItemsField.GetValue(this);

            if (cachedLayoutUpdatedItems == null)
            {
                LayoutUpdatedListItemsField.SetValue(this, item);
                LayoutUpdatedHandlersField.SetValue(this, handler);
            }
            else
            {
                EventHandler cachedLayoutUpdatedHandler = LayoutUpdatedHandlersField.GetValue(this);
                if (cachedLayoutUpdatedHandler != null)
                {
                    //second unique handler is coming in.
                    //allocate a datastructure
                    Hashtable list = new Hashtable(2);

                    //add previously cached handler
                    list.Add(cachedLayoutUpdatedHandler, cachedLayoutUpdatedItems);

                    //add new handler
                    list.Add(handler, item);

                    LayoutUpdatedHandlersField.ClearValue(this);
                    LayoutUpdatedListItemsField.SetValue(this, list);
                }
                else //already have a list
                {
                    Hashtable list = (Hashtable)cachedLayoutUpdatedItems;
                    list.Add(handler, item);
                }
            }
        }

        private void removeLayoutUpdatedHandler(EventHandler d)
        {
            object cachedLayoutUpdatedItems = LayoutUpdatedListItemsField.GetValue(this);
            EventHandler cachedLayoutUpdatedHandler = LayoutUpdatedHandlersField.GetValue(this);

            if (cachedLayoutUpdatedHandler != null) //single handler
            {
                if (cachedLayoutUpdatedHandler == d)
                {
                    LayoutUpdatedListItemsField.ClearValue(this);
                    LayoutUpdatedHandlersField.ClearValue(this);
                }
            }
            else //there is an ArrayList allocated
            {
                Hashtable list = (Hashtable)cachedLayoutUpdatedItems;
                list.Remove(d);
            }
        }

        /// <summary>
        /// This event fires every time Layout updates the layout of the trees associated with current Dispatcher.
        /// Layout update can happen as a result of some propety change, window resize or explicit user request.
        /// </summary>
        public event EventHandler LayoutUpdated
        {
            add
            {
                LayoutEventList.ListItem item = getLayoutUpdatedHandler(value);

                if (item == null)
                {
                    //set a weak ref in LM
                    item = ContextLayoutManager.From(Dispatcher).LayoutEvents.Add(value);
                    addLayoutUpdatedHandler(value, item);
                }
            }
            remove
            {
                LayoutEventList.ListItem item = getLayoutUpdatedHandler(value);

                if (item != null)
                {
                    removeLayoutUpdatedHandler(value);
                    //remove a weak ref from LM
                    ContextLayoutManager.From(Dispatcher).LayoutEvents.Remove(item);
                }
            }
        }

        /// <summary>
        /// This is a public read-only property that returns size of the UIElement.
        /// This size is typcally used to find out where ink of the element will go.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Size RenderSize
        {
            get
            {
                if (!Visible)
                    return new Size();
                else
                    return _size;
            }
            set
            {
                _size = value;
                //InvalidateHitTestBounds();
            }
        }

        /// <summary>
        /// Returns the size the element computed during the Measure pass.
        /// This is only valid if IsMeasureValid is true.
        /// </summary>
        public Size DesiredSize
        {
            get
            {
                if (!Visible)
                    return new Size(0, 0);
                else
                    return _desiredSize;
            }
        }

        internal bool AreTransformsClean
        {
            get { return ReadFlag(CoreFlags.AreTransformsClean); }
            set { WriteFlag(CoreFlags.AreTransformsClean, value); }
        }

        /// <summary>
        /// Determines if the DesiredSize is valid.
        /// </summary>
        /// <remarks>
        /// A developer can force arrangement to be invalidated by calling InvalidateMeasure.
        /// IsArrangeValid and IsMeasureValid are related,
        /// in that arrangement cannot be valid without measurement first being valid.
        /// </remarks>
        public bool IsMeasureValid
        {
            get { return !MeasureDirty; }
        }

        /// <summary>
        /// Determines if the RenderSize and position of child elements is valid.
        /// </summary>
        /// <remarks>
        /// A developer can force arrangement to be invalidated by calling InvalidateArrange.
        /// IsArrangeValid and IsMeasureValid are related, in that arrangement cannot be valid without measurement first
        /// being valid.
        /// </remarks>
        public bool IsArrangeValid
        {
            get { return !ArrangeDirty; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control and all its child controls are displayed.
        /// </summary>
        /// <value><c>true</c> if the control and all its child controls are displayed; otherwise, <c>false</c>. The default is <c>true</c>.</value>
        public bool Visible
        {
            get => visible;

            set
            {
                if (visible == value)
                    return;

                visible = value;
                if (visible)
                    InvalidateArrange();
                OnVisibleChanged(EventArgs.Empty);
                VisibleChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        internal Size PreviousConstraint
        {
            get
            {
                return _previousAvailableSize;
            }
        }

        /// <summary>
        /// This is used by LayoutManager as a perf optimization for layout updates.
        /// During layout updates, LM needs to find which areas of the visual tree
        /// are higher in the tree - they have to be processed first to avoid multiple
        /// updates of lower descendants.The tree level counter is maintained by
        /// UIElement.PropagateResume/SuspendLayout methods and uses 8 bits in VisualFlags to
        /// keep the count.
        /// </summary>
        internal uint TreeLevel
        {
            get
            {
                //return ((uint)_flags & 0xFFE00000) >> 21;
                return treeLevel;
            }
            set
            {
                if (value > TreeLevelLimit)
                {
                    throw new InvalidOperationException(SR.Get(SRID.LayoutManager_DeepRecursion, TreeLevelLimit));
                }

                //_flags = (VisualFlags)(((uint)_flags & 0x001FFFFF) | (value << 21));
                treeLevel = value;
            }
        }

        internal abstract bool IsLayoutSuspended { get; }

        internal bool MeasureInProgress
        {
            get { return ReadFlag(CoreFlags.MeasureInProgress); }
            set { WriteFlag(CoreFlags.MeasureInProgress, value); }
        }

        internal bool ArrangeInProgress
        {
            get { return ReadFlag(CoreFlags.ArrangeInProgress); }
            set { WriteFlag(CoreFlags.ArrangeInProgress, value); }
        }

        internal Rect PreviousArrangeRect
        {
            //  called from PresentationFramework!System.Windows.Controls.Primitives.LayoutInformation.GetLayoutSlot()
            [FriendAccessAllowed]
            get
            {
                return _finalRect;
            }
        }

        internal bool NeverMeasured
        {
            get { return ReadFlag(CoreFlags.NeverMeasured); }
            set { WriteFlag(CoreFlags.NeverMeasured, value); }
        }

        internal bool MeasureDuringArrange
        {
            get { return ReadFlag(CoreFlags.MeasureDuringArrange); }
            set { WriteFlag(CoreFlags.MeasureDuringArrange, value); }
        }

        internal bool NeverArranged
        {
            get { return ReadFlag(CoreFlags.NeverArranged); }
            set { WriteFlag(CoreFlags.NeverArranged, value); }
        }

        internal bool UseLayoutRounding { get; set; }

        internal bool MeasureDirty
        {
            get { return ReadFlag(CoreFlags.MeasureDirty); }
            set { WriteFlag(CoreFlags.MeasureDirty, value); }
        }

        internal bool ArrangeDirty
        {
            get { return ReadFlag(CoreFlags.ArrangeDirty); }
            set { WriteFlag(CoreFlags.ArrangeDirty, value); }
        }

        /// <summary>
        /// Identical to VisualParent, except that skips verify access for perf.
        /// </summary>
        internal DependencyObject InternalVisualParent
        {
            get
            {
                return GetUIParentCore();
            }
        }

        /// <summary>
        ///     Event Handlers Store
        /// </summary>
        /// <remarks>
        ///     The idea of exposing this property is to allow
        ///     elements in the Framework to generically use
        ///     EventHandlersStore for Clr events as well.
        /// </remarks>
        internal EventHandlersStore EventHandlersStore
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

        private bool RenderingInvalidated
        {
            get { return ReadFlag(CoreFlags.RenderingInvalidated); }
            set { WriteFlag(CoreFlags.RenderingInvalidated, value); }
        }

        /// <summary>
        /// Parents or system call this method to arrange the internals of children on a second pass of layout update.
        /// </summary>
        /// <remarks>
        /// This method internally calls ArrangeCore override, giving the derived class opportunity
        /// to arrange its children and/or content using final computed size.
        /// In their ArrangeCore overrides, derived class is supposed to create its visual structure and
        /// prepare itself for rendering. Arrange is called by parents
        /// from their implementation of ArrangeCore or by system when needed.
        /// This method sets Bounds=finalSize before calling ArrangeCore.
        /// </remarks>
        /// <param name="finalRect">This is the final size and location that parent or system wants this UIElement to assume.</param>
        public void Arrange(Rect finalRect)
        {
            bool etwTracingEnabled = false;
            long perfElementID = 0;

            ContextLayoutManager ContextLayoutManager = ContextLayoutManager.From(Dispatcher);
            if (ContextLayoutManager.AutomationEvents.Count != 0)
                UIElementHelper.InvalidateAutomationAncestors(this);

            if (EventTrace.IsEnabled(EventTrace.Keyword.KeywordLayout, EventTrace.Level.Verbose))
            {
                perfElementID = PerfService.GetPerfElementID(this);

                etwTracingEnabled = true;
                EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientArrangeElementBegin, EventTrace.Keyword.KeywordLayout, EventTrace.Level.Verbose, perfElementID, finalRect.Top, finalRect.Left, finalRect.Width, finalRect.Height);
            }

            try
            {
                //             VerifyAccess();

                // Disable reentrancy during the arrange pass.  This is because much work is done
                // during arrange - such as formatting PTS stuff, creating
                // fonts, etc.  Generally speaking, we cannot survive reentrancy in these code
                // paths.
                using (Dispatcher.DisableProcessing())
                {
                    //enforce that Arrange can not come with Infinity size or NaN
                    if (double.IsPositiveInfinity(finalRect.Width)
                        || double.IsPositiveInfinity(finalRect.Height)
                        || DoubleUtil.IsNaN(finalRect.Width)
                        || DoubleUtil.IsNaN(finalRect.Height)
                      )
                    {
                        DependencyObject parent = GetUIParent() as UIElement;
                        throw new InvalidOperationException(
                            SR.Get(
                                SRID.UIElement_Layout_InfinityArrange,
                                    (parent == null ? "" : parent.GetType().FullName),
                                    this.GetType().FullName));
                    }

                    //if Collapsed, we should not Arrange, keep dirty bit but remove request
                    if (!Visible
                        || IsLayoutSuspended)
                    {
                        //reset arrange request.
                        if (ArrangeRequest != null)
                            ContextLayoutManager.From(Dispatcher).ArrangeQueue.Remove(this);

                        //  remember though that parent tried to arrange at this rect
                        //  in case when later this element is called to arrange incrementally
                        //  it has up-to-date information stored in _finalRect
                        _finalRect = finalRect;

                        return;
                    }

                    //in case parent did not call Measure on a child, we call it now.
                    //parent can skip calling Measure on a child if it does not care about child's size
                    //passing finalSize practically means "set size" because that's what Measure(sz)/Arrange(same_sz) means
                    //Note that in case of IsLayoutSuspended (temporarily out of the tree) the MeasureDirty can be true
                    //while it does not indicate that we should re-measure - we just came of Measure that did nothing
                    //because of suspension
                    if (MeasureDirty
                       || NeverMeasured)
                    {
                        try
                        {
                            MeasureDuringArrange = true;
                            //If never measured - that means "set size", arrange-only scenario
                            //Otherwise - the parent previosuly measured the element at constriant
                            //and the fact that we are arranging the measure-dirty element now means
                            //we are not in the UpdateLayout loop but rather in manual sequence of Measure/Arrange
                            //(like in HwndSource when new RootVisual is attached) so there are no loops and there could be
                            //measure-dirty elements left after previosu single Measure pass) - so need to use cached constraint
                            if (NeverMeasured)
                                Measure(finalRect.Size);
                            else
                            {
                                Measure(PreviousConstraint);
                            }
                        }
                        finally
                        {
                            MeasureDuringArrange = false;
                        }
                    }

                    //bypass - if clean and rect is the same, no need to re-arrange
                    if (!IsArrangeValid
                        || NeverArranged
                        || !DoubleUtil.AreClose(finalRect, _finalRect))
                    {
                        bool firstArrange = NeverArranged;
                        NeverArranged = false;
                        ArrangeInProgress = true;

                        ContextLayoutManager layoutManager = ContextLayoutManager.From(Dispatcher);

                        Size oldSize = RenderSize;
                        bool sizeChanged = false;
                        bool gotException = true;

                        // If using layout rounding, round final size before calling ArrangeCore.
                        if (UseLayoutRounding)
                        {
                            DpiScale dpi = GetDpi();
                            finalRect = RoundLayoutRect(finalRect, dpi.DpiScaleX, dpi.DpiScaleY);
                        }

                        try
                        {
                            layoutManager.EnterArrange();

                            //This has to update RenderSize
                            ArrangeCore(finalRect);

                            //to make sure Clip is tranferred to Visual
                            //ensureClip(finalRect.Size); //yezo

                            //  see if we need to call OnRenderSizeChanged on this element
                            sizeChanged = markForSizeChangedIfNeeded(oldSize, RenderSize);

                            gotException = false;
                        }
                        finally
                        {
                            ArrangeInProgress = false;
                            layoutManager.ExitArrange();

                            if (gotException)
                            {
                                // we don't want to reset last exception element on layoutManager if it's been already set.
                                if (layoutManager.GetLastExceptionElement() == null)
                                {
                                    layoutManager.SetLastExceptionElement(this);
                                }
                            }
                        }

                        _finalRect = finalRect;

                        ArrangeDirty = false;

                        //reset request.
                        if (ArrangeRequest != null)
                            ContextLayoutManager.From(Dispatcher).ArrangeQueue.Remove(this);

                        // yezo
                        //if ((sizeChanged || RenderingInvalidated || firstArrange) && IsRenderable())
                        //{
                        //    DrawingContext dc = RenderOpen();
                        //    try
                        //    {
                        //        bool etwGeneralEnabled = EventTrace.IsEnabled(EventTrace.Keyword.KeywordGraphics | EventTrace.Keyword.KeywordPerf, EventTrace.Level.Verbose);
                        //        if (etwGeneralEnabled == true)
                        //        {
                        //            EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientOnRenderBegin, EventTrace.Keyword.KeywordGraphics | EventTrace.Keyword.KeywordPerf, EventTrace.Level.Verbose, perfElementID);
                        //        }

                        //        try
                        //        {
                        //            OnRender(dc);
                        //        }
                        //        finally
                        //        {
                        //            if (etwGeneralEnabled == true)
                        //            {
                        //                EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientOnRenderEnd, EventTrace.Keyword.KeywordGraphics | EventTrace.Keyword.KeywordPerf, EventTrace.Level.Verbose, perfElementID);
                        //            }
                        //        }
                        //    }
                        //    finally
                        //    {
                        //        dc.Close();
                        //        RenderingInvalidated = false;
                        //    }

                        //    updatePixelSnappingGuidelines();
                        //}

                        if (firstArrange)
                        {
                            EndPropertyInitialization();
                        }
                    }
                }
            }
            finally
            {
                if (etwTracingEnabled == true)
                {
                    EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientArrangeElementEnd, EventTrace.Keyword.KeywordLayout, EventTrace.Level.Verbose, perfElementID, finalRect.Top, finalRect.Left, finalRect.Width, finalRect.Height);
                }
            }
        }

        /// <summary>
        /// Invalidates the measurement state for the element.
        /// This has the effect of also invalidating the arrange state for the element.
        /// The element will be queued for an update layout that will occur asynchronously.
        /// </summary>
        public void InvalidateMeasure()
        {
            if (!MeasureDirty
                && !MeasureInProgress)
            {
                Debug.Assert(MeasureRequest == null, "can't be clean and still have MeasureRequest");

                //                 VerifyAccess();

                if (!NeverMeasured) //only measured once elements are allowed in *update* queue
                {
                    ContextLayoutManager ContextLayoutManager = ContextLayoutManager.From(Dispatcher);
                    if (EventTrace.IsEnabled(EventTrace.Keyword.KeywordLayout, EventTrace.Level.Verbose))
                    {
                        // Knowing when the layout queue goes from clean to dirty is interesting.
                        if (ContextLayoutManager.MeasureQueue.IsEmpty)
                        {
                            EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientLayoutInvalidated, EventTrace.Keyword.KeywordLayout, EventTrace.Level.Verbose, PerfService.GetPerfElementID(this));
                        }
                    }

                    ContextLayoutManager.MeasureQueue.Add(this);
                }
                MeasureDirty = true;
            }
        }

        /// <summary>
        /// Invalidates the arrange state for the element.
        /// The element will be queued for an update layout that will occur asynchronously.
        /// MeasureCore will not be called unless InvalidateMeasure is also called - or that something
        /// else caused the measure state to be invalidated.
        /// </summary>
        public void InvalidateArrange()
        {
            if (!ArrangeDirty &&
               !ArrangeInProgress)
            {
                Debug.Assert(ArrangeRequest == null, "can't be clean and still have MeasureRequest");

                //                 VerifyAccess();

                //if (!NeverArranged) // yezo
                {
                    ContextLayoutManager ContextLayoutManager = ContextLayoutManager.From(Dispatcher);
                    ContextLayoutManager.ArrangeQueue.Add(this);
                }

                ArrangeDirty = true;
            }
        }

        /// <summary>
        /// Invalidates the rendering of the element.
        /// Causes <see cref="System.Windows.UIElement.OnRender"/> to be called at a later time.
        /// </summary>
        public void InvalidateVisual()
        {
            InvalidateArrange();
            RenderingInvalidated = true;
        }

        /// <summary>
        /// Updates DesiredSize of the UIElement. Must be called by parents from theor MeasureCore, to form recursive update.
        /// This is first pass of layout update.
        /// </summary>
        /// <remarks>
        /// Measure is called by parents on their children. Internally, Measure calls MeasureCore override on the same object,
        /// giving it opportunity to compute its DesiredSize.<para/>
        /// This method will return immediately if child is not Dirty, previously measured
        /// and availableSize is the same as cached. <para/>
        /// This method also resets the IsMeasureinvalid bit on the child.<para/>
        /// In case when "unbounded measure to content" is needed, parent can use availableSize
        /// as double.PositiveInfinity. Any returned size is OK in this case.
        /// </remarks>
        /// <param name="availableSize">Available size that parent can give to the child. May be infinity (when parent wants to
        /// measure to content). This is soft constraint. Child can return bigger size to indicate that it wants bigger space and hope
        /// that parent can throw in scrolling...</param>
        public void Measure(Size availableSize)
        {
            bool etwTracingEnabled = false;
            long perfElementID = 0;
            ContextLayoutManager ContextLayoutManager = ContextLayoutManager.From(Dispatcher);
            if (ContextLayoutManager.AutomationEvents.Count != 0)
                UIElementHelper.InvalidateAutomationAncestors(this);

            if (EventTrace.IsEnabled(EventTrace.Keyword.KeywordLayout, EventTrace.Level.Verbose))
            {
                perfElementID = PerfService.GetPerfElementID(this);

                etwTracingEnabled = true;
                EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientMeasureElementBegin, EventTrace.Keyword.KeywordLayout, EventTrace.Level.Verbose, perfElementID, availableSize.Width, availableSize.Height);
            }
            try
            {
                //             VerifyAccess();

                // Disable reentrancy during the measure pass.  This is because much work is done
                // during measure - such as inflating templates, formatting PTS stuff, creating
                // fonts, etc.  Generally speaking, we cannot survive reentrancy in these code
                // paths.
                using (Dispatcher.DisableProcessing())
                {
                    //enforce that Measure can not receive NaN size .
                    if (DoubleUtil.IsNaN(availableSize.Width) || DoubleUtil.IsNaN(availableSize.Height))
                        throw new InvalidOperationException(SR.Get(SRID.UIElement_Layout_NaNMeasure));

                    bool neverMeasured = NeverMeasured;

                    if (neverMeasured)
                    {
                        switchVisibilityIfNeeded(Visible);
                        //to make sure effects are set correctly - otherwise it's not used
                        //simply because it is never pulled by anybody
                        pushVisualEffects();
                    }

                    bool isCloseToPreviousMeasure = DoubleUtil.AreClose(availableSize, _previousAvailableSize);

                    //if Collapsed, we should not Measure, keep dirty bit but remove request
                    if (!Visible || IsLayoutSuspended)
                    {
                        //reset measure request.
                        if (MeasureRequest != null)
                            ContextLayoutManager.From(Dispatcher).MeasureQueue.Remove(this);

                        //  remember though that parent tried to measure at this size
                        //  in case when later this element is called to measure incrementally
                        //  it has up-to-date information stored in _previousAvailableSize
                        if (!isCloseToPreviousMeasure)
                        {
                            //this will ensure that element will be actually re-measured at the new available size
                            //later when it becomes visible.
                            InvalidateMeasureInternal();

                            _previousAvailableSize = availableSize;
                        }

                        return;
                    }

                    //your basic bypass. No reason to calc the same thing.
                    if (IsMeasureValid                       //element is clean
                        && !neverMeasured                       //previously measured
                        && isCloseToPreviousMeasure) //and contraint matches
                    {
                        return;
                    }

                    NeverMeasured = false;
                    Size prevSize = _desiredSize;

                    //we always want to be arranged, ensure arrange request
                    //doing it before OnMeasure prevents unneeded requests from children in the queue
                    InvalidateArrange();
                    //_measureInProgress prevents OnChildDesiredSizeChange to cause the elements be put
                    //into the queue.

                    MeasureInProgress = true;

                    Size desiredSize = new Size(0, 0);

                    ContextLayoutManager layoutManager = ContextLayoutManager.From(Dispatcher);

                    bool gotException = true;

                    try
                    {
                        layoutManager.EnterMeasure();
                        desiredSize = MeasureCore(availableSize);

                        gotException = false;
                    }
                    finally
                    {
                        // reset measure in progress
                        MeasureInProgress = false;

                        _previousAvailableSize = availableSize;

                        layoutManager.ExitMeasure();

                        if (gotException)
                        {
                            // we don't want to reset last exception element on layoutManager if it's been already set.
                            if (layoutManager.GetLastExceptionElement() == null)
                            {
                                layoutManager.SetLastExceptionElement(this);
                            }
                        }
                    }

                    //enforce that MeasureCore can not return PositiveInfinity size even if given Infinte availabel size.
                    //Note: NegativeInfinity can not be returned by definition of Size structure.
                    if (double.IsPositiveInfinity(desiredSize.Width) || double.IsPositiveInfinity(desiredSize.Height))
                        throw new InvalidOperationException(SR.Get(SRID.UIElement_Layout_PositiveInfinityReturned, this.GetType().FullName));

                    //enforce that MeasureCore can not return NaN size .
                    if (DoubleUtil.IsNaN(desiredSize.Width) || DoubleUtil.IsNaN(desiredSize.Height))
                        throw new InvalidOperationException(SR.Get(SRID.UIElement_Layout_NaNReturned, this.GetType().FullName));

                    //reset measure dirtiness

                    MeasureDirty = false;
                    //reset measure request.
                    if (MeasureRequest != null)
                        ContextLayoutManager.From(Dispatcher).MeasureQueue.Remove(this);

                    //cache desired size
                    _desiredSize = desiredSize;

                    //notify parent if our desired size changed (watefall effect)
                    if (!MeasureDuringArrange
                       && !DoubleUtil.AreClose(prevSize, desiredSize))
                    {
                        var p = GetUIParentCore() as UIElement;
                        if (p != null && !p.MeasureInProgress) //this is what differs this code from signalDesiredSizeChange()
                            p.OnChildDesiredSizeChanged(this);
                    }
                }
            }
            finally
            {
                if (etwTracingEnabled == true)
                {
                    EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientMeasureElementEnd, EventTrace.Keyword.KeywordLayout, EventTrace.Level.Verbose, perfElementID, _desiredSize.Width, _desiredSize.Height);
                }
            }
        }

        /// <summary>
        ///     Raise the events specified by
        ///     <see cref="RoutedEventArgs.RoutedEvent"/>
        /// </summary>
        /// <remarks>
        ///     This method is a shorthand for
        ///     <see cref="UIElement.BuildRoute"/> and
        ///     <see cref="EventRoute.InvokeHandlers"/>
        /// </remarks>
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
            FrugalObjectList<RoutedEventHandlerInfo> instanceListeners = null;
            EventHandlersStore store = EventHandlersStore;
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
            EventHandlersStore.AddRoutedEventHandler(routedEvent, handler, handledEventsToo);

            OnAddHandler(routedEvent, handler);
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

            EventHandlersStore store = EventHandlersStore;
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
        /// Calculates the value to be used for layout rounding at high DPI.
        /// </summary>
        /// <param name="value">Input value to be rounded.</param>
        /// <param name="dpiScale">Ratio of screen's DPI to layout DPI</param>
        /// <returns>Adjusted value that will produce layout rounding on screen at high dpi.</returns>
        /// <remarks>This is a layout helper method. It takes DPI into account and also does not return
        /// the rounded value if it is unacceptable for layout, e.g. Infinity or NaN. It's a helper associated with
        /// UseLayoutRounding  property and should not be used as a general rounding utility.</remarks>
        internal static double RoundLayoutValue(double value, double dpiScale)
        {
            double newValue;

            // If DPI == 1, don't use DPI-aware rounding.
            if (!DoubleUtil.AreClose(dpiScale, 1.0))
            {
                newValue = Math.Round(value * dpiScale) / dpiScale;
                // If rounding produces a value unacceptable to layout (NaN, Infinity or MaxValue), use the original value.
                if (DoubleUtil.IsNaN(newValue) ||
                    Double.IsInfinity(newValue) ||
                    DoubleUtil.AreClose(newValue, Double.MaxValue))
                {
                    newValue = value;
                }
            }
            else
            {
                newValue = Math.Round(value);
            }

            return newValue;
        }

        /// <summary>
        /// If layout rounding is in use, rounds the size and offset of a rect.
        /// </summary>
        /// <param name="rect">Rect to be rounded.</param>
        /// <param name="dpiScaleX">DPI along x-dimension.</param>
        /// <param name="dpiScaleY">DPI along y-dimension.</param>
        /// <returns>Rounded rect.</returns>
        /// <remarks>This is a layout helper method. It takes DPI into account and also does not return
        /// the rounded value if it is unacceptable for layout, e.g. Infinity or NaN. It's a helper associated with
        /// UseLayoutRounding  property and should not be used as a general rounding utility.</remarks>
        internal static Rect RoundLayoutRect(Rect rect, double dpiScaleX, double dpiScaleY)
        {
            return new Rect(RoundLayoutValue(rect.X, dpiScaleX),
                            RoundLayoutValue(rect.Y, dpiScaleY),
                            RoundLayoutValue(rect.Width, dpiScaleX),
                            RoundLayoutValue(rect.Height, dpiScaleY)
                            );
        }

        internal static void BuildRouteHelper(DependencyObject e, EventRoute route, RoutedEventArgs args)
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
                UIElement uiElement = e as UIElement;

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
                    UIElement uiElement = e as UIElement;

                    // Protect against infinite loops by limiting the number of elements
                    // that we will process.
                    if (cElements++ > MAX_ELEMENTS_IN_ROUTE)
                    {
                        throw new InvalidOperationException(SR.Get(SRID.TreeLoop));
                    }

                    // Allow the element to adjust source
                    object newSource = null;
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

        /// <summary>
        /// Walks visual tree up to find UIElement parent within Element Layout Island, so stops the walk if the island's root is found
        /// </summary>
        internal UIElement GetUIParentWithinLayoutIsland()
        {
            return InternalVisualParent as UIElement;

            // yezo
            //UIElement uiParent = null;

            //for (Visual v = VisualTreeHelper.GetParent(this) as Visual; v != null; v = VisualTreeHelper.GetParent(v) as Visual)
            //{
            //    if (v.CheckFlagsAnd(VisualFlags.IsLayoutIslandRoot))
            //    {
            //        break;
            //    }

            //    if (v.CheckFlagsAnd(VisualFlags.IsUIElement))
            //    {
            //        uiParent = (UIElement)v;
            //        break;
            //    }
            //}
            //return uiParent;
        }

        /// <summary>
        /// Helper, gives the UIParent under control of which
        /// the OnMeasure or OnArrange are currently called.
        /// This may be implemented as a tree walk up until
        /// LayoutElement is found.
        /// </summary>
        internal DependencyObject GetUIParent()
        {
            return UIElementHelper.GetUIParent(this, false);
        }

        /// <summary>
        /// Returns the DPI information at which this Visual is rendered.
        /// </summary>
        internal DpiScale GetDpi()
        {
            return new DpiScale(1, 1); // yezo

            //DpiScale dpi;
            //lock (UIElement.DpiLock)
            //{
            //    if (UIElement.DpiScaleXValues.Count == 0)
            //    {
            //        // This is for scenarios where an HWND hasn't been created yet.
            //        return UIElement.EnsureDpiScale();
            //    }

            //    // initialized to system DPI as a fallback value
            //    dpi = new DpiScale(UIElement.DpiScaleXValues[0], UIElement.DpiScaleYValues[0]);

            //    int index = 0;
            //    index = CheckFlagsAnd(VisualFlags.DpiScaleFlag1) ? index | 1 : index;
            //    index = CheckFlagsAnd(VisualFlags.DpiScaleFlag2) ? index | 2 : index;

            //    if (index < 3 && UIElement.DpiScaleXValues[index] != 0 && UIElement.DpiScaleYValues[index] != 0)
            //    {
            //        dpi = new DpiScale(UIElement.DpiScaleXValues[index], UIElement.DpiScaleYValues[index]);
            //    }

            //    else if (index >= 3)
            //    {
            //        int actualIndex = DpiIndex.GetValue(this);
            //        dpi = new DpiScale(UIElement.DpiScaleXValues[actualIndex], UIElement.DpiScaleYValues[actualIndex]);
            //    }
            //}
            //return dpi;
        }

        internal void InvalidateMeasureInternal()
        {
            MeasureDirty = true;
        }

        internal void InvalidateArrangeInternal()
        {
            ArrangeDirty = true;
        }

        internal DependencyObject GetUIParentNo3DTraversal()
        {
            DependencyObject parent = null;

            // Try to find a UIElement parent in the visual ancestry.
            DependencyObject myParent = InternalVisualParent;
            parent = InputElement.GetContainingUIElement(myParent, true);

            return parent;
        }

        /// <summary>
        ///     This virtual method is to be overridden in Framework
        ///     to be able to add handlers for styles
        /// </summary>
        internal virtual void AddToEventRouteCore(EventRoute route, RoutedEventArgs args)
        {
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
        internal virtual object AdjustEventSource(RoutedEventArgs args)
        {
            return null;
        }

        internal abstract DependencyObject GetUIParentCore();

        internal DependencyObject GetUIParent(bool v)
        {
            return GetUIParentCore();
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

        internal bool ReadFlag(CoreFlags field)
        {
            return (_flags & field) != 0;
        }

        /// <summary>
        /// This is invoked after layout update before rendering if the element's RenderSize
        /// has changed as a result of layout update.
        /// </summary>
        /// <param name="info">Packaged parameters (<seealso cref="SizeChangedInfo"/>, includes
        /// old and new sizes and which dimension actually changes. </param>
        protected internal virtual void OnRenderSizeChanged(SizeChangedInfo info)
        { }

        protected void SetVisibleValue(bool value) => visible = value;

        /// <summary>
        /// Called when the value of the <see cref="Visible"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnVisibleChanged(EventArgs e)
        {
        }

        /// <summary>
        /// ArrangeCore allows for the customization of the final sizing and positioning of children.
        /// </summary>
        /// <remarks>
        /// Element authors should override this method, call Arrange on each visible child element,
        /// to size and position each child element by passing a rectangle reserved for the child within parent space.
        /// Note: It is required that a parent element calls Arrange on each child or they won't be rendered.
        /// Typical override follows a pattern roughly like this (pseudo-code):
        /// <example>
        ///     <code lang="C#">
        /// <![CDATA[
        ///
        /// protected override Size ArrangeCore(Rect finalRect)
        /// {
        ///     //Call base, it will set offset and _size to the finalRect:
        ///     base.ArrangeCore(finalRect);
        ///
        ///     foreach (UIElement child in ...)
        ///     {
        ///         child.Arrange(new Rect(childX, childY, childWidth, childHeight);
        ///     }
        /// }
        /// ]]>
        ///     </code>
        /// </example>
        /// </remarks>
        /// <param name="finalRect">The final area within the parent that element should use to arrange itself and its children.</param>
        protected virtual void ArrangeCore(Rect finalRect)
        {
            // Set the element size.
            RenderSize = finalRect.Size;

            throw new NotImplementedException(); // todo: set layout rect

            ////Set transform to reflect the offset of finalRect - parents that have multiple children
            ////pass offset in the finalRect to communicate the location of this child withing the parent.
            //Transform renderTransform = RenderTransform;
            //if (renderTransform == Transform.Identity)
            //    renderTransform = null;

            //Vector oldOffset = VisualOffset;
            //if (!DoubleUtil.AreClose(oldOffset.X, finalRect.X) ||
            //    !DoubleUtil.AreClose(oldOffset.Y, finalRect.Y))
            //{
            //    VisualOffset = new Vector(finalRect.X, finalRect.Y);
            //}

            //if (renderTransform != null)
            //{
            //    //render transform + layout offset, create a collection
            //    TransformGroup t = new TransformGroup();

            //    Point origin = RenderTransformOrigin;
            //    bool hasOrigin = (origin.X != 0d || origin.Y != 0d);
            //    if (hasOrigin)
            //        t.Children.Add(new TranslateTransform(-(finalRect.Width * origin.X), -(finalRect.Height * origin.Y)));

            //    t.Children.Add(renderTransform);

            //    if (hasOrigin)
            //        t.Children.Add(new TranslateTransform(finalRect.Width * origin.X,
            //                                              finalRect.Height * origin.Y));

            //    VisualTransform = t;
            //}
            //else
            //{
            //    VisualTransform = null;
            //}
        }

        /// <summary>
        /// Notification that is called by Measure of a child when
        /// it ends up with different desired size for the child.
        /// </summary>
        /// <remarks>
        /// Default implementation simply calls invalidateMeasure(), assuming that layout of a
        /// parent should be updated after child changed its size.<para/>
        /// Finer point: this method can only be called in the scenario when the system calls Measure on a child,
        /// not when parent calls it since if parent calls it, it means parent has dirty layout and is recalculating already.
        /// </remarks>
        protected virtual void OnChildDesiredSizeChanged(UIElement child)
        {
            if (IsMeasureValid)
            {
                InvalidateMeasure();
            }
        }

        /// <summary>
        /// Measurement override. Implement your size-to-content logic here.
        /// </summary>
        /// <remarks>
        /// MeasureCore is designed to be the main customizability point for size control of layout.
        /// Element authors should override this method, call Measure on each child element,
        /// and compute their desired size based upon the measurement of the children.
        /// The return value should be the desired size.<para/>
        /// Note: It is required that a parent element calls Measure on each child or they won't be sized/arranged.
        /// Typical override follows a pattern roughly like this (pseudo-code):
        /// <example>
        ///     <code lang="C#">
        /// <![CDATA[
        ///
        /// protected override Size MeasureCore(Size availableSize)
        /// {
        ///     foreach (UIElement child in ...)
        ///     {
        ///         child.Measure(availableSize);
        ///         availableSize.Deflate(child.DesiredSize);
        ///         _cache.StoreInfoAboutChild(child);
        ///     }
        ///
        ///     Size desired = CalculateBasedOnCache(_cache);
        ///     return desired;
        /// }
        /// ]]>
        ///     </code>
        /// </example>
        /// The key aspects of this snippet are:
        ///     <list type="bullet">
        /// <item>You must call Measure on each child element</item>
        /// <item>It is common to cache measurement information between the MeasureCore and ArrangeCore method calls</item>
        /// <item>Calling base.MeasureCore is not required.</item>
        /// <item>Calls to Measure on children are passing either the same availableSize as the parent, or a subset of the area depending
        /// on the type of layout the parent will perform (for example, it would be valid to remove the area
        /// for some border or padding).</item>
        ///     </list>
        /// </remarks>
        /// <param name="availableSize">Available size that parent can give to the child. May be infinity (when parent wants to
        /// measure to content). This is soft constraint. Child can return bigger size to indicate that it wants bigger space and hope
        /// that parent can throw in scrolling...</param>
        /// <returns>Desired Size of the control, given available size passed as parameter.</returns>
        protected virtual Size MeasureCore(Size availableSize)
        {
            //can not return availableSize here - this is too "greedy" and can cause the Infinity to be
            //returned. So the next "reasonable" choice is (0,0).
            return new Size(0, 0);
        }

        private void Initialize()
        {
            BeginPropertyInitialization();
            NeverMeasured = true;
            NeverArranged = true;

            //SnapsToDevicePixelsCache = (bool)SnapsToDevicePixelsProperty.GetDefaultValue(DependencyObjectType);
            //ClipToBoundsCache = (bool)ClipToBoundsProperty.GetDefaultValue(DependencyObjectType);
            //VisibilityCache = (Visibility)VisibilityProperty.GetDefaultValue(DependencyObjectType);

            //SetFlags(true, VisualFlags.IsUIElement);

            // Note: IsVisibleCache is false by default.

            if (EventTrace.IsEnabled(EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Verbose))
            {
                PerfService.GetPerfElementID(this);
            }
        }

        private bool markForSizeChangedIfNeeded(Size oldSize, Size newSize)
        {
            //already marked for SizeChanged, simply update the newSize
            bool widthChanged = !DoubleUtil.AreClose(oldSize.Width, newSize.Width);
            bool heightChanged = !DoubleUtil.AreClose(oldSize.Height, newSize.Height);

            SizeChangedInfo info = sizeChangedInfo;

            if (info != null)
            {
                info.Update(widthChanged, heightChanged);
                return true;
            }
            else if (widthChanged || heightChanged)
            {
                info = new SizeChangedInfo(this, oldSize, widthChanged, heightChanged);
                sizeChangedInfo = info;
                ContextLayoutManager.From(Dispatcher).AddToSizeChangedChain(info);

                //
                // This notifies Visual layer that hittest boundary potentially changed
                //

                // yezo
                //PropagateFlags(
                //    this,
                //    VisualFlags.IsSubtreeDirtyForPrecompute,
                //    VisualProxyFlags.IsSubtreeDirtyForRender);

                return true;
            }

            //this result is used to determine if we need to call OnRender after Arrange
            //OnRender is called for 2 reasons - someone called InvalidateVisual - then OnRender is called
            //on next Arrange, or the size changed.
            return false;
        }

        private void switchVisibilityIfNeeded(bool visible)
        {
            if (visible)
                ensureVisible();
            else
                ensureInvisible();
        }

        private void ensureInvisible()
        {
            if (!ReadFlag(CoreFlags.IsOpacitySuppressed))
            {
                //base.VisualOpacity = 0; // yezo
                WriteFlag(CoreFlags.IsOpacitySuppressed, true);
            }

            WriteFlag(CoreFlags.IsCollapsed, true);

            //invalidate parent
            signalDesiredSizeChange();
        }

        private void signalDesiredSizeChange()
        {
            var p = GetUIParentCore() as UIElement;

            if (p != null)
                p.OnChildDesiredSizeChanged(this);
        }

        private void ensureVisible()
        {
            if (ReadFlag(CoreFlags.IsOpacitySuppressed))
            {
                ////restore Opacity
                //base.VisualOpacity = Opacity;

                if (ReadFlag(CoreFlags.IsCollapsed))
                {
                    WriteFlag(CoreFlags.IsCollapsed, false);

                    //invalidate parent if needed
                    signalDesiredSizeChange();

                    //we are suppressing rendering (see IsRenderable) of collapsed children (to avoid
                    //confusion when they see RenderSize=(0,0) reported for them)
                    //so now we should invalidate to re-render if some rendering props
                    //changed while UIElement was Collapsed (Arrange will cause re-rendering)
                    InvalidateVisual();
                }

                WriteFlag(CoreFlags.IsOpacitySuppressed, false);
            }
        }

        /// <summary>
        /// pushVisualEffects - helper to propagate cacheMode, Opacity, OpacityMask, BitmapEffect, BitmapScalingMode and EdgeMode
        /// </summary>
        private void pushVisualEffects()
        {
            //pushCacheMode();
            //pushOpacity();
            //pushOpacityMask();
            //pushBitmapEffect();
            //pushEdgeMode();
            //pushBitmapScalingMode();
            //pushClearTypeHint();
            //pushTextHintingMode();
        }
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