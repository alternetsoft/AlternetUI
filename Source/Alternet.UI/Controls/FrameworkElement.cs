using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace Alternet.UI
{
    public class FrameworkElement : UIElement
    {
        /// <summary>
        /// Gets or sets the identifying name of the control.
        /// The name provides a reference so that code-behind, such as event handler code,
        /// can refer to a markup control after it is constructed during processing by a UIXML processor.
        /// </summary>
        /// <value>The name of the control. The default is <c>null</c>.</value>
        public string? Name { get; set; } // todo: maybe use Site.Name?
        public static DependencyProperty? BindingGroupProperty { get; internal set; }
        public bool IsInitialized { get; internal set; }
        public FrameworkElement Parent { get; internal set; }
        public bool IsParentAnFE { get; internal set; }

        internal override DependencyObject GetUIParentCore()
        {
            return Parent;
        }

        internal bool IsLogicalChildrenIterationInProgress
        {
            get { return ReadInternalFlag(InternalFlags.IsLogicalChildrenIterationInProgress); }
            set { WriteInternalFlag(InternalFlags.IsLogicalChildrenIterationInProgress, value); }
        }


        /// <summary>
        /// Language can be specified in xaml at any point using the xml language attribute xml:lang.
        /// This will make the culture pertain to the scope of the element where it is applied.  The
        /// XmlLanguage names follow the RFC 3066 standard. For example, U.S. English is "en-US".
        /// </summary>
        static public readonly DependencyProperty LanguageProperty =
                    DependencyProperty.RegisterAttached(
                                "Language",
                                typeof(object),//typeof(XmlLanguage),
                                typeof(FrameworkElement),
                                new FrameworkPropertyMetadata(
                                        /*XmlLanguage.GetLanguage("en-US")*/"en-US",
                                        FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public event RoutedEventHandler LostFocus;

        private InternalFlags _flags = 0; // Stores Flags (see Flags enum)

        /// <summary>
        ///     Indicates the current mode of lookup for both inheritance and resources.
        /// </summary>
        /// <remarks>
        ///     Used in property inheritance and reverse
        ///     inheritance and resource lookup to stop at
        ///     logical tree boundaries
        ///
        ///     It is also used by designers such as Sparkle to
        ///     skip past the app resources directly to the theme.
        ///     They are expected to merge in the client's app
        ///     resources via the MergedDictionaries feature on
        ///     root element of the tree.
        ///
        ///     NOTE: Property can be set only when the
        ///     instance is not yet hooked to the tree. This
        ///     is to encourage setting it at construction time.
        ///     If we didn't restrict it to (roughly) construction
        ///     time, we would have to take the complexity of
        ///     firing property invalidations when the flag was
        ///     changed.
        /// </remarks>
        protected internal InheritanceBehavior InheritanceBehavior
        {
            get
            {
                Debug.Assert((uint)InternalFlags.InheritanceBehavior0 == 0x08);
                Debug.Assert((uint)InternalFlags.InheritanceBehavior1 == 0x10);
                Debug.Assert((uint)InternalFlags.InheritanceBehavior2 == 0x20);

                const uint inheritanceBehaviorMask =
                    (uint)InternalFlags.InheritanceBehavior0 +
                    (uint)InternalFlags.InheritanceBehavior1 +
                    (uint)InternalFlags.InheritanceBehavior2;

                uint inheritanceBehavior = ((uint)_flags & inheritanceBehaviorMask) >> 3;
                return (InheritanceBehavior)inheritanceBehavior;
            }

            set
            {
                Debug.Assert((uint)InternalFlags.InheritanceBehavior0 == 0x08);
                Debug.Assert((uint)InternalFlags.InheritanceBehavior1 == 0x10);
                Debug.Assert((uint)InternalFlags.InheritanceBehavior2 == 0x20);

                const uint inheritanceBehaviorMask =
                    (uint)InternalFlags.InheritanceBehavior0 +
                    (uint)InternalFlags.InheritanceBehavior1 +
                    (uint)InternalFlags.InheritanceBehavior2;

                if (!this.IsInitialized)
                {
                    if ((uint)value < 0 ||
                        (uint)value > (uint)InheritanceBehavior.SkipAllNext)
                    {
                        throw new InvalidEnumArgumentException("value", (int)value, typeof(InheritanceBehavior));
                    }

                    uint inheritanceBehavior = (uint)value << 3;

                    _flags = (InternalFlags)((inheritanceBehavior & inheritanceBehaviorMask) | (((uint)_flags) & ~inheritanceBehaviorMask));

                    if (Parent != null)
                    {
                        // This means that we are in the process of xaml parsing:
                        // an instance of FE has been created and added to a parent,
                        // but no children yet added to it (otherwise it would be initialized already
                        // and we would not be allowed to change InheritanceBehavior).
                        // So we need to re-calculate properties accounting for the new
                        // inheritance behavior.
                        // This must have no performance effect as the subtree of this
                        // element is empty (no children yet added).
                        TreeWalkHelper.InvalidateOnTreeChange(/*fe:*/this, /*fce:null,*/ Parent, true);
                    }
                }
                else
                {
                    throw new InvalidOperationException(SR.Get(SRID.Illegal_InheritanceBehaviorSettor));
                }
            }
        }

        internal bool ReadInternalFlag(InternalFlags reqFlag)
        {
            return (_flags & reqFlag) != 0;
        }

        // Indicates if there are any implicit styles in the ancestry
        internal bool ShouldLookupImplicitStyles
        {
            get { return ReadInternalFlag(InternalFlags.ShouldLookupImplicitStyles); }
            set { WriteInternalFlag(InternalFlags.ShouldLookupImplicitStyles, value); }
        }

        /// <summary>
        ///     Allows FrameworkElement to augment the
        ///     <see cref="EventRoute"/>
        /// </summary>
        /// <remarks>
        ///     NOTE: If this instance does not have a
        ///     visualParent but has a model parent
        ///     then route is built through the model
        ///     parent
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
        internal override bool BuildRouteCore(EventRoute route, RoutedEventArgs args)
        {
            return BuildRouteCoreHelper(route, args, true);
        }

        // Allows adjustments to the branch source popped off the stack
        internal virtual void AdjustBranchSource(RoutedEventArgs args)
        {
        }

        internal static void AddIntermediateElementsToRoute(
            DependencyObject mergePoint,
            EventRoute route,
            RoutedEventArgs args,
            DependencyObject modelTreeNode)
        {
            while (modelTreeNode != null && modelTreeNode != mergePoint)
            {
                UIElement uiElement = modelTreeNode as UIElement;

                if (uiElement != null)
                {
                    uiElement.AddToEventRoute(route, args);

                    //FrameworkElement fe = uiElement as FrameworkElement;
                    //if (fe != null)
                    //{
                    //    AddStyleHandlersToEventRoute(fe, null, route, args);
                    //}
                }

                // Get model parent
                modelTreeNode = LogicalTreeHelper.GetParent(modelTreeNode);
            }
        }

        internal bool BuildRouteCoreHelper(EventRoute route, RoutedEventArgs args, bool shouldAddIntermediateElementsToRoute)
        {
            bool continuePastCoreTree = false;


            //DependencyObject visualParent = VisualTreeHelper.GetParent(this);
            DependencyObject modelParent = GetUIParentCore();

            // FrameworkElement extends the basic event routing strategy by
            // introducing the concept of a logical tree.  When an event
            // passes through an element in a logical tree, the source of
            // the event needs to change to the leaf-most node in the same
            // logical tree that is in the route.

            // Check the route to see if we are returning into a logical tree
            // that we left before.  If so, restore the source of the event to
            // be the source that it was when we left the logical tree.
            DependencyObject branchNode = route.PeekBranchNode() as DependencyObject;
            if (branchNode != null && IsLogicalDescendent(branchNode))
            {
                // We keep the most recent source in the event args.  Note that
                // this is only for our consumption.  Once the event is raised,
                // it will use the source information in the route.
                args.Source = route.PeekBranchSource();

                AdjustBranchSource(args);

                route.AddSource(args.Source);

                // By popping the branch node we will also be setting the source
                // in the event route.
                route.PopBranchNode();

                // Add intermediate ContentElements to the route
                if (shouldAddIntermediateElementsToRoute)
                {
                    FrameworkElement.AddIntermediateElementsToRoute(this, route, args, LogicalTreeHelper.GetParent(branchNode));
                }
            }


            // Check if the next element in the route is in a different
            // logical tree.

            if (!IgnoreModelParentBuildRoute(args))
            {
                // If there is no visual parent, route via the model tree.
                /* yezo
                if (visualParent == null)
                {
                    continuePastCoreTree = modelParent != null;
                }
                else */if (modelParent != null)
                {
                    //Visual visualParentAsVisual = visualParent as Visual;
                    //if (visualParentAsVisual != null)
                    //{
                    //    if (visualParentAsVisual.CheckFlagsAnd(VisualFlags.IsLayoutIslandRoot))
                    //    {
                    //        continuePastCoreTree = true;
                    //    }
                    //}
                    //else
                    //{
                    //    if (((Visual3D)visualParent).CheckFlagsAnd(VisualFlags.IsLayoutIslandRoot))
                    //    {
                    //        continuePastCoreTree = true;
                    //    }
                    //}

                    // If there is a model parent, record the branch node.
                    route.PushBranchNode(this, args.Source);

                    // The source is going to be the visual parent, which
                    // could live in a different logical tree.
                    //args.Source = visualParent;
                    args.Source = Parent;
                }
            }

            return continuePastCoreTree;
        }

        internal virtual bool IgnoreModelParentBuildRoute(RoutedEventArgs args)
        {
            return false;
        }

        /// <summary>
        ///     DataContextChanged event
        /// </summary>
        /// <remarks>
        ///     When an element's DataContext changes, all data-bound properties
        ///     (on this element or any other element) whose Bindings use this
        ///     DataContext will change to reflect the new value.  There is no
        ///     guarantee made about the order of these changes relative to the
        ///     raising of the DataContextChanged event.  The changes can happen
        ///     before the event, after the event, or in any mixture.
        /// </remarks>
        public event DependencyPropertyChangedEventHandler DataContextChanged
        {
            add { EventHandlersStoreAdd(DataContextChangedKey, value); }
            remove { EventHandlersStoreRemove(DataContextChangedKey, value); }
        }

        /// <summary>
        ///     Returns enumerator to logical children
        /// </summary>
        protected internal virtual IEnumerator LogicalChildren
        {
            get { return null; }
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
        internal override object AdjustEventSource(RoutedEventArgs args)
        {
            object source = null;

            // As part of routing events through logical trees, we have
            // to be careful about events that come to us from "foreign"
            // trees.  For example, the event could come from an element
            // in our "implementation" visual tree, or from an element
            // in a different logical tree all together.
            //
            // Note that we consider ourselves to be part of a logical tree
            // if we have either a logical parent, or any logical children.
            //
            // BUGBUG: this misses "trees" that have only one logical node.  No parents, no children.

            if (Parent != null || HasLogicalChildren)
            {
                DependencyObject logicalSource = args.Source as DependencyObject;
                if (logicalSource == null || !IsLogicalDescendent(logicalSource))
                {
                    args.Source = this;
                    source = this;
                }
            }

            return source;
        }

        // Returns if the given child instance is a logical descendent
        private bool IsLogicalDescendent(DependencyObject child)
        {
            while (child != null)
            {
                if (child == this)
                {
                    return true;
                }

                child = LogicalTreeHelper.GetParent(child);
            }

            return false;
        }

        internal void EventHandlersStoreAdd(EventPrivateKey key, Delegate handler)
        {
            EnsureEventHandlersStore();
            EventHandlersStore.Add(key, handler);
        }

        internal void EventHandlersStoreRemove(EventPrivateKey key, Delegate handler)
        {
            EventHandlersStore store = EventHandlersStore;
            if (store != null)
            {
                store.Remove(key, handler);
            }
        }

        /// <summary>
        ///     DataContext DependencyProperty
        /// </summary>
        public static readonly DependencyProperty DataContextProperty =
                    DependencyProperty.Register(
                                "DataContext",
                                typeof(object),
                                typeof(FrameworkElement),
                                new FrameworkPropertyMetadata(null,
                                        FrameworkPropertyMetadataOptions.Inherits,
                                        new PropertyChangedCallback(OnDataContextChanged)));

        /// <summary>
        ///     DataContext Property
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Localizability(LocalizationCategory.NeverLocalize)]
        public object DataContext
        {
            get { return GetValue(DataContextProperty); }
            set { SetValue(DataContextProperty, value); }
        }

        internal virtual bool HasLogicalChildren => false;

        // Helper method to retrieve and fire Clr Event handlers for DependencyPropertyChanged event
        private void RaiseDependencyPropertyChanged(EventPrivateKey key, DependencyPropertyChangedEventArgs args)
        {
            EventHandlersStore store = EventHandlersStore;
            if (store != null)
            {
                Delegate handler = store.Get(key);
                if (handler != null)
                {
                    ((DependencyPropertyChangedEventHandler)handler)(this, args);
                }
            }
        }

        /// <summary>
        ///     DataContextChanged private key
        /// </summary>
        internal static readonly EventPrivateKey DataContextChangedKey = new EventPrivateKey();

        private static void OnDataContextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == BindingExpressionBase.DisconnectedItem)
                return;

            ((FrameworkElement)d).RaiseDependencyPropertyChanged(DataContextChangedKey, e);
        }

        internal static void AddHandler(DependencyObject d, RoutedEvent routedEvent, Delegate handler)
        {
            throw new NotImplementedException();
        }

        internal static void RemoveHandler(DependencyObject d, RoutedEvent routedEvent, Delegate handler)
        {
            throw new NotImplementedException();
        }

        internal void WriteInternalFlag(object isInitialized, bool v)
        {
            throw new NotImplementedException();
        }

        internal static bool GetFrameworkParent(FrameworkElement current, out FrameworkElement feParent)
        {
            FrameworkObject fo = new FrameworkObject(current);

            fo = fo.FrameworkParent;

            feParent = fo.FE;

            return fo.IsValid;
        }

        internal static DependencyObject GetFrameworkParent(object current)
        {
            FrameworkObject fo = new FrameworkObject(current as DependencyObject);

            fo = fo.FrameworkParent;

            return fo.DO;
        }

        // Indicates that an ancestor change tree walk is progressing
        // through the given node
        internal bool AncestorChangeInProgress
        {
            get { return ReadInternalFlag(InternalFlags.AncestorChangeInProgress); }
            set { WriteInternalFlag(InternalFlags.AncestorChangeInProgress, value); }
        }

        internal bool InVisibilityCollapsedTree
        {
            get { return ReadInternalFlag(InternalFlags.InVisibilityCollapsedTree); }
            set { WriteInternalFlag(InternalFlags.InVisibilityCollapsedTree, value); }
        }

        // OnAncestorChangedInternal variant when we know what type (FE/FCE) the
        //  tree node is.
        internal void OnAncestorChangedInternal(TreeChangeInfo parentTreeState)
        {
            // Cache the IsSelfInheritanceParent flag
            bool wasSelfInheritanceParent = IsSelfInheritanceParent;

            if (parentTreeState.Root != this)
            {
                // Clear the HasStyleChanged flag
                //HasStyleChanged = false;
                //HasStyleInvalidated = false;
                //HasTemplateChanged = false;
            }

            // If this is a tree add operation update the ShouldLookupImplicitStyles
            // flag with respect to your parent.
            if (parentTreeState.IsAddOperation)
            {
                FrameworkObject fo =
                    new FrameworkObject(this);

                fo.SetShouldLookupImplicitStyles();
            }

            // Invalidate ResourceReference properties
            //if (HasResourceReference)
            //{
            //    // This operation may cause a style change and hence should be done before the call to
            //    // InvalidateTreeDependents as it relies on the HasStyleChanged flag
            //    TreeWalkHelper.OnResourcesChanged(this, ResourcesChangeInfo.TreeChangeInfo, false);
            //}

            // If parent is a FrameworkElement
            // This is also an operation that could change the style
            FrugalObjectList<DependencyProperty> currentInheritableProperties =
            InvalidateTreeDependentProperties(parentTreeState, IsSelfInheritanceParent, wasSelfInheritanceParent);

            // we have inherited properties that changes as a result of the above;
            // invalidation; push that list of inherited properties on the stack
            // for the children to use
            parentTreeState.InheritablePropertiesStack.Push(currentInheritableProperties);



            // Call OnAncestorChanged
            OnAncestorChanged();

            // Notify mentees if they exist
            if (PotentiallyHasMentees)
            {
                // Raise the ResourcesChanged Event so that ResourceReferenceExpressions
                // on non-[FE/FCE] listening for this can then update their values
                RaiseClrEvent(FrameworkElement.ResourcesChangedKey, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     ResourcesChanged private key
        /// </summary>
        internal static readonly EventPrivateKey ResourcesChangedKey = new EventPrivateKey();

        // Helper method to retrieve and fire Clr Event handlers
        internal void RaiseClrEvent(EventPrivateKey key, EventArgs args)
        {
            EventHandlersStore store = EventHandlersStore;
            if (store != null)
            {
                Delegate handler = store.Get(key);
                if (handler != null)
                {
                    ((EventHandler)handler)(this, args);
                }
            }
        }

        // Indicates if the current element has or had mentees at some point.
        internal bool PotentiallyHasMentees
        {
            get { return ReadInternalFlag(InternalFlags.PotentiallyHasMentees); }
            set
            {
                Debug.Assert(value == true,
                    "This flag is set to true when a mentee attaches a listeners to either the " +
                    "InheritedPropertyChanged event or the ResourcesChanged event. It never goes " +
                    "back to being false because this would involve counting the remaining listeners " +
                    "for either of the aforementioned events. This seems like an overkill for the perf " +
                    "optimization we are trying to achieve here.");

                WriteInternalFlag(InternalFlags.PotentiallyHasMentees, value);
            }
        }

        /// <summary>
        ///     Invoked when ancestor is changed.  This is invoked after
        ///     the ancestor has changed, and the purpose is to allow elements to
        ///     perform actions based on the changed ancestor.
        /// </summary>
        internal virtual void OnAncestorChanged()
        {
        }


        // Invalidate all the properties that may have changed as a result of
        //  changing this element's parent in the logical (and sometimes visual tree.)
        internal FrugalObjectList<DependencyProperty> InvalidateTreeDependentProperties(TreeChangeInfo parentTreeState, bool isSelfInheritanceParent, bool wasSelfInheritanceParent)
        {
            AncestorChangeInProgress = true;

            InVisibilityCollapsedTree = false;  // False == we don't know whether we're in a visibility collapsed tree.

            if (parentTreeState.TopmostCollapsedParentNode == null)
            {
                //// There is no ancestor node with Visibility=Collapsed.
                ////  See if "fe" is the root of a collapsed subtree.
                //if (Visibility == Visibility.Collapsed)
                //{
                //    // This is indeed the root of a collapsed subtree.
                //    //  remember this information as we proceed on the tree walk.
                //    parentTreeState.TopmostCollapsedParentNode = this;

                //    // Yes, this FE node is in a visibility collapsed subtree.
                //    InVisibilityCollapsedTree = true;
                //}
            }
            else
            {
                // There is an ancestor node somewhere above us with
                //  Visibility=Collapsed.  We're in a visibility collapsed subtree.
                InVisibilityCollapsedTree = true;
            }


            try
            {
                // Style property is a special case of a non-inherited property that needs
                // invalidation for parent changes. Invalidate StyleProperty if it hasn't been
                // locally set because local value takes precedence over implicit references
                //if (IsInitialized && !HasLocalStyle && (this != parentTreeState.Root))
                //{
                //    UpdateStyleProperty();
                //}

                //Style selfStyle = null;
                //Style selfThemeStyle = null;
                //DependencyObject templatedParent = null;

                //int childIndex = -1;
                //ChildRecord childRecord = new ChildRecord();
                //bool isChildRecordValid = false;

                //selfStyle = Style;
                //selfThemeStyle = ThemeStyle;
                //templatedParent = TemplatedParent;
                //childIndex = TemplateChildIndex;

                // StyleProperty could have changed during invalidation of ResourceReferenceExpressions if it
                // were locally set or during the invalidation of unresolved implicitly referenced style
                //bool hasStyleChanged = HasStyleChanged;

                // Fetch selfStyle, hasStyleChanged and childIndex for the current node
                //FrameworkElement.GetTemplatedParentChildRecord(templatedParent, childIndex, out childRecord, out isChildRecordValid);

                FrameworkElement parentFE;
                //FrameworkContentElement parentFCE;
                bool hasParent = FrameworkElement.GetFrameworkParent(this, out parentFE/*, out parentFCE*/);

                DependencyObject parent = null;
                InheritanceBehavior parentInheritanceBehavior = InheritanceBehavior.Default;
                if (hasParent)
                {
                    if (parentFE != null)
                    {
                        parent = parentFE;
                        parentInheritanceBehavior = parentFE.InheritanceBehavior;
                    }
                    //else
                    //{
                    //    parent = parentFCE;
                    //    parentInheritanceBehavior = parentFCE.InheritanceBehavior;
                    //}
                }

                if (!TreeWalkHelper.SkipNext(InheritanceBehavior) && !TreeWalkHelper.SkipNow(parentInheritanceBehavior))
                {
                    // Synchronize InheritanceParent
                    this.SynchronizeInheritanceParent(parent);
                }
                else if (!IsSelfInheritanceParent)
                {
                    // Set IsSelfInheritanceParet on the root node at a tree boundary
                    // so that all inheritable properties are cached on it.
                    SetIsSelfInheritanceParent();
                }

                // Loop through all cached inheritable properties for the parent to see if they should be invalidated.
                return TreeWalkHelper.InvalidateTreeDependentProperties(parentTreeState, /* fe = */ this, /* fce = *//* null*//*, selfStyle, selfThemeStyle,
                    ref childRecord, isChildRecordValid, hasStyleChanged,*/ isSelfInheritanceParent, wasSelfInheritanceParent);
            }
            finally
            {
                AncestorChangeInProgress = false;
                InVisibilityCollapsedTree = false;  // 'false' just means 'we don't know' - see comment at definition of the flag.
            }
        }
    }
}