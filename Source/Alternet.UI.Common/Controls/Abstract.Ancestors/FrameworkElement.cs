using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

using Alternet.UI.Port;

namespace Alternet.UI
{
    /// <summary>
    /// Provides a framework-level set of properties, events, and methods for AlterNET UI elements.
    /// This class represents the provided framework-level implementation that is built on the
    /// core-level APIs that are defined by <see cref="UIElement"/>.
    /// </summary>
    public class FrameworkElement : UIElement
    {
        /// <summary>
        /// Language can be specified in xaml at any point using the xml language attribute xml:lang.
        /// This will make the culture pertain to the scope of the element where it is applied.  The
        /// XmlLanguage names follow the RFC 3066 standard. For example, U.S. English is "en-US".
        /// </summary>
        internal static readonly DependencyProperty LanguageProperty =
                    DependencyProperty.RegisterAttached(
                                "Language",
                                typeof(object),
                                typeof(FrameworkElement),
                                new FrameworkPropertyMetadata(
                                        /*XmlLanguage.GetLanguage("en-US")*/"en-US",
                                        PropMetadataOption.Inherits | PropMetadataOption.AffectsLayout));

        /// <summary>
        ///     DataContext DependencyProperty
        /// </summary>
        internal static readonly DependencyProperty DataContextProperty =
                    DependencyProperty.Register(
                                "DataContext",
                                typeof(object),
                                typeof(FrameworkElement),
                                new FrameworkPropertyMetadata(
                                    null,
                                    PropMetadataOption.Inherits,
                                    new PropertyChangedCallback(OnDataContextChanged)));

        /// <summary>
        ///     InheritedPropertyChanged private key
        /// </summary>
        internal static readonly EventPrivateKey InheritedPropertyChangedKey = new();

        /// <summary>
        ///     ResourcesChanged private key
        /// </summary>
        internal static readonly EventPrivateKey ResourcesChangedKey = new();

        /// <summary>
        ///     DataContextChanged private key
        /// </summary>
        internal static readonly EventPrivateKey DataContextChangedKey = new();

        private static readonly UncommonField<DependencyObject> InheritanceContextField = new();

        private InternalFlags internalFlags = 0;
        private FrameworkElement? logicalParent;

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
        internal event DependencyPropertyChangedEventHandler DataContextChanged
        {
            add { EventHandlersStoreAdd(DataContextChangedKey, value); }
            remove { EventHandlersStoreRemove(DataContextChangedKey, value); }
        }

        /// <summary>
        /// Gets or sets data context.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Localizability(LocalizationCategory.NeverLocalize)]
        public virtual object? DataContext
        {
            get { return GetValue(DataContextProperty); }
            set { SetValue(DataContextProperty, value); }
        }

        /// <summary>
        /// Returns a collection of content elements which is used by the UIXML loader to
        /// find content items by index.
        /// </summary>
        [Browsable(false)]
        internal virtual IReadOnlyList<FrameworkElement> ContentElements =>
            LogicalChildrenCollection.ToArray();

        [Browsable(false)]
        internal FrameworkElement? LogicalParent
        {
            get => logicalParent;
            set
            {
                var oldParent = logicalParent;
                logicalParent = value;
                ChangeLogicalParent(oldParent, logicalParent);
            }
        }

        internal bool IsInitialized { get; set; }

        internal bool IsLogicalChildrenIterationInProgress
        {
            get
            {
                return ReadInternalFlag(InternalFlags.IsLogicalChildrenIterationInProgress);
            }

            set
            {
                WriteInternalFlag(InternalFlags.IsLogicalChildrenIterationInProgress, value);
            }
        }

        // Indicates if there are any implicit styles in the ancestry
        internal bool ShouldLookupImplicitStyles
        {
            get { return ReadInternalFlag(InternalFlags.ShouldLookupImplicitStyles); }
            set { WriteInternalFlag(InternalFlags.ShouldLookupImplicitStyles, value); }
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

        // Indicates if the current element has or had mentees at some point.
        internal bool PotentiallyHasMentees
        {
            get
            {
                return ReadInternalFlag(InternalFlags.PotentiallyHasMentees);
            }

            set
            {
                /*
                Debug.Assert(value == true,
                    "This flag is set to true when a mentee attaches a listeners to either the " +
                    "InheritedPropertyChanged event or the ResourcesChanged event. It never goes " +
                    "back to being false because this would involve"+
                    " counting the remaining listeners " +
                    "for either of the aforementioned events. This seems"+
                " like an overkill for the perf " +
                    "optimization we are trying to achieve here.");
                */

                WriteInternalFlag(InternalFlags.PotentiallyHasMentees, value);
            }
        }

        internal bool IsParentAnFE { get; set; }

        internal virtual bool HasLogicalChildren => LogicalChildrenCollection.Any();

        /// <summary>
        ///     Returns enumerator to logical children
        /// </summary>
        internal virtual IEnumerator LogicalChildren =>
            LogicalChildrenCollection.GetEnumerator();

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
        internal InheritanceBehavior InheritanceBehavior
        {
            get
            {
                const uint inheritanceBehaviorMask =
                    (uint)InternalFlags.InheritanceBehavior0 +
                    (uint)InternalFlags.InheritanceBehavior1 +
                    (uint)InternalFlags.InheritanceBehavior2;

                uint inheritanceBehavior = ((uint)internalFlags & inheritanceBehaviorMask) >> 3;
                return (InheritanceBehavior)inheritanceBehavior;
            }

            set
            {
                const uint inheritanceBehaviorMask =
                    (uint)InternalFlags.InheritanceBehavior0 +
                    (uint)InternalFlags.InheritanceBehavior1 +
                    (uint)InternalFlags.InheritanceBehavior2;

                if (!this.IsInitialized)
                {
                    if ((uint)value < 0 ||
                        (uint)value > (uint)InheritanceBehavior.SkipAllNext)
                    {
                        throw new InvalidEnumArgumentException(
                            nameof(value),
                            (int)value,
                            typeof(InheritanceBehavior));
                    }

                    uint inheritanceBehavior = (uint)value << 3;

                    internalFlags = (InternalFlags)((inheritanceBehavior & inheritanceBehaviorMask) |
                        (((uint)internalFlags) & ~inheritanceBehaviorMask));

                    if (LogicalParent != null)
                    {
                        // This means that we are in the process of xaml parsing:
                        // an instance of FE has been created and added to a parent,
                        // but no children yet added to it (otherwise it would be initialized already
                        // and we would not be allowed to change InheritanceBehavior).
                        // So we need to re-calculate properties accounting for the new
                        // inheritance behavior.
                        // This must have no performance effect as the subtree of this
                        // element is empty (no children yet added).
                        TreeWalkHelper.InvalidateOnTreeChange(
                            /*fe:*/this,
                            /*fce:null,*/ LogicalParent,
                            true);
                    }
                }
                else
                {
                    throw new InvalidOperationException(
                        SR.Get(SRID.Illegal_InheritanceBehaviorSettor));
                }
            }
        }

        /// <summary>
        /// Returns a collection of elements which can be treated as "logical children" of
        /// this element.
        /// </summary>
        [Browsable(false)]
        internal virtual IEnumerable<FrameworkElement> LogicalChildrenCollection =>
            Array.Empty<FrameworkElement>();

        /// <summary>
        /// Recursively searches all <see cref="LogicalChildrenCollection"/> for a control with
        /// the specified name, and returns that control if found.
        /// </summary>
        /// <param name="name">The name of the control to be found.</param>
        /// <returns>The found control, or <c>null</c> if no control with the provided name
        /// is found.</returns>
        public FrameworkElement? TryFindElement(string name)
        {
            if (name is null)
                return null;

            if (Name == name)
                return this;

            foreach (var child in LogicalChildrenCollection)
            {
                var result = child.TryFindElement(name);
                if (result != null)
                    return result;
            }

            return null;
        }

        /// <summary>
        /// Recursively searches all <see cref="LogicalChildrenCollection"/> for a control
        /// with the specified name,
        /// and throws an exception if the requested control is not found.
        /// </summary>
        /// <param name="name">The name of the control to be found.</param>
        /// <returns>The requested resource. If no control with the provided name was found,
        /// an exception is thrown.</returns>
        /// <exception cref="InvalidOperationException">A control with the provided name was
        /// found.</exception>
        public UIElement FindElement(string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            return TryFindElement(name) ?? throw new InvalidOperationException(
                $"Element with name '{name}' was not found.");
        }

        internal static bool GetFrameworkParent(FrameworkElement current, out FrameworkElement feParent)
        {
            FrameworkObject fo = new(current);

            fo = fo.FrameworkParent;

            feParent = fo.FE;

            return fo.IsValid;
        }

        internal static DependencyObject GetFrameworkParent(object current)
        {
            FrameworkObject fo = new(current as DependencyObject);

            fo = fo.FrameworkParent;

            return fo.DO;
        }

        internal static void AddIntermediateElementsToRoute(
            DependencyObject mergePoint,
            EventRoute route,
            RoutedEventArgs args,
            DependencyObject modelTreeNode)
        {
            while (modelTreeNode != null && modelTreeNode != mergePoint)
            {
                if (modelTreeNode is UIElement uiElement)
                {
                    uiElement.AddToEventRoute(route, args);

                    // FrameworkElement fe = uiElement as FrameworkElement;
                    // if (fe != null)
                    // {
                    //    AddStyleHandlersToEventRoute(fe, null, route, args);
                    // }
                }

                // Get model parent
                modelTreeNode = LogicalTreeHelper.GetParent(modelTreeNode);
            }
        }

        // Like GetValueCore, except it returns the expression (if any) instead of its value
        internal Expression? GetExpressionCore(DependencyProperty dp, PropertyMetadata metadata)
        {
            // this.IsRequestingExpression = true; yezo
            EffectiveValueEntry entry = new(dp)
            {
                Value = DependencyProperty.UnsetValue,
            };
            this.EvaluateBaseValueCore(dp, metadata, ref entry);

            // this.IsRequestingExpression = false; yezo
            return entry.Value as Expression;
        }

        internal override DependencyObject? GetUIParentCore()
        {
            return LogicalParent;
        }

        /// <summary>
        ///     Invoked when logical parent is changed.  This just
        ///     sets the parent pointer.
        /// </summary>
        /// <remarks>
        ///     A parent change is considered catastrohpic and results in a large
        ///     amount of invalidations and tree traversals. <cref see="DependencyFastBuild"/>
        ///     is recommended to reduce the work necessary to build a tree
        /// </remarks>
        internal void ChangeLogicalParent(DependencyObject? oldParent, DependencyObject? newParent)
        {
            ///////////////////
            // OnNewParent:
            ///////////////////

            // -- Approved By The Core Team --
            // Do not allow foreign threads to change the tree.
            // (This is a noop if this object is not assigned to a Dispatcher.)
            //
            // We also need to ensure that the tree is homogenous with respect
            // to the dispatchers that the elements belong to.
            this.VerifyAccess();
            newParent?.VerifyAccess();

            // Logical Parent must first be dropped before you are attached to a newParent
            // This mitigates illegal tree state caused by logical child stealing as illustrated
            // in bug 970706
            // if (_parent != null && newParent != null && _parent != newParent)
            // {
            //    throw new System.InvalidOperationException(SR.Get(SRID.HasLogicalParent));
            // }

            // Trivial check to avoid loops
            // if (newParent == this)
            // {
            //    throw new System.InvalidOperationException(SR.Get(SRID.CannotBeSelfParent));
            // }

            // invalid during a VisualTreeChanged event
            // VisualDiagnostics.VerifyVisualTreeChange(this);

            // Logical Parent implies no InheritanceContext
            if (newParent != null)
            {
                ClearInheritanceContext();
            }

            IsParentAnFE = newParent is FrameworkElement;

            OnNewParent(oldParent, newParent);

            // Update Has[Loaded/Unloaded]Handler Flags
            // BroadcastEventHelper.AddOrRemoveHasLoadedChangeHandlerFlag(this, oldParent, newParent);

            ///////////////////
            // OnParentChanged:
            ///////////////////

            // Invalidate relevant properties for this subtree
            var parent = newParent ?? oldParent;
            TreeWalkHelper.InvalidateOnTreeChange(/* fe = */ this, parent, newParent != null);

            // If no one has called BeginInit then mark the element initialized and fire
            // Initialized event
            // (non-parser programmatic tree building scenario)
            // TryFireInitialized();
        }

        internal bool ReadInternalFlag(InternalFlags reqFlag)
        {
            return (internalFlags & reqFlag) != 0;
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

        internal bool BuildRouteCoreHelper(
            EventRoute route,
            RoutedEventArgs args,
            bool shouldAddIntermediateElementsToRoute)
        {
            bool continuePastCoreTree = false;

            // DependencyObject visualParent = VisualTreeHelper.GetParent(this);
            var modelParent = GetUIParentCore();

            // FrameworkElement extends the basic event routing strategy by
            // introducing the concept of a logical tree.  When an event
            // passes through an element in a logical tree, the source of
            // the event needs to change to the leaf-most node in the same
            // logical tree that is in the route.

            // Check the route to see if we are returning into a logical tree
            // that we left before.  If so, restore the source of the event to
            // be the source that it was when we left the logical tree.
            if (route.PeekBranchNode() is DependencyObject branchNode
                && IsLogicalDescendent(branchNode))
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
                    FrameworkElement.AddIntermediateElementsToRoute(
                        this,
                        route,
                        args,
                        LogicalTreeHelper.GetParent(branchNode));
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
                else */
                if (modelParent != null)
                {
                    // Visual visualParentAsVisual = visualParent as Visual;
                    // if (visualParentAsVisual != null)
                    // {
                    //    if (visualParentAsVisual.CheckFlagsAnd(VisualFlags.IsLayoutIslandRoot))
                    //    {
                    //        continuePastCoreTree = true;
                    //    }
                    // }
                    // else
                    // {
                    //    if (((Visual3D)visualParent).CheckFlagsAnd(VisualFlags.IsLayoutIslandRoot))
                    //    {
                    //        continuePastCoreTree = true;
                    //    }
                    // }

                    // If there is a model parent, record the branch node.
                    route.PushBranchNode(this, args.Source);

                    // The source is going to be the visual parent, which
                    // could live in a different logical tree.
                    // args.Source = visualParent;
                    args.Source = LogicalParent;
                }
            }

            return continuePastCoreTree;
        }

        internal virtual bool IgnoreModelParentBuildRoute(RoutedEventArgs args)
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
        internal override object? AdjustEventSource(RoutedEventArgs args)
        {
            object? source = null;

            // As part of routing events through logical trees, we have
            // to be careful about events that come to us from "foreign"
            // trees.  For example, the event could come from an element
            // in our "implementation" visual tree, or from an element
            // in a different logical tree all together.
            //
            // Note that we consider ourselves to be part of a logical tree
            // if we have either a logical parent, or any logical children.
            // BUGBUG: this misses "trees" that have only one logical node.  No parents, no children.
            if (LogicalParent != null || HasLogicalChildren)
            {
                if (args.Source is not DependencyObject logicalSource
                    || !IsLogicalDescendent(logicalSource))
                {
                    args.Source = this;
                    source = this;
                }
            }

            return source;
        }

        internal void EventHandlersStoreAdd(EventPrivateKey key, Delegate handler)
        {
            EnsureEventHandlersStore();
            EventHandlersStore!.Add(key, handler);
        }

        internal void EventHandlersStoreRemove(EventPrivateKey key, Delegate handler)
        {
            var store = EventHandlersStore;
            store?.Remove(key, handler);
        }

        // Sets or Unsets the required flag based on
        // the bool argument
        internal void WriteInternalFlag(InternalFlags reqFlag, bool set)
        {
            if (set)
            {
                internalFlags |= reqFlag;
            }
            else
            {
                internalFlags &= ~reqFlag;
            }
        }

        /// <summary>
        ///     Called before the parent is chanded to the new value.
        /// </summary>
        internal virtual void OnNewParent(DependencyObject? oldParent, DependencyObject? newParent)
        {
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
                // HasStyleChanged = false;
                // HasStyleInvalidated = false;
                // HasTemplateChanged = false;
            }

            // If this is a tree add operation update the ShouldLookupImplicitStyles
            // flag with respect to your parent.
            if (parentTreeState.IsAddOperation)
            {
                FrameworkObject fo = new(this);

                fo.SetShouldLookupImplicitStyles();
            }

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

        // Helper method to retrieve and fire Clr Event handlers
        internal void RaiseClrEvent(EventPrivateKey key, EventArgs args)
        {
            var store = EventHandlersStore;
            if (store != null)
            {
                Delegate handler = store.Get(key);
                if (handler != null)
                {
                    ((EventHandler)handler)(this, args);
                }
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

        // Helper method to retrieve and fire the InheritedPropertyChanged event
        internal void RaiseInheritedPropertyChangedEvent(ref InheritablePropertyChangeInfo info)
        {
            var store = EventHandlersStore;
            if (store != null)
            {
                Delegate handler = store.Get(FrameworkElement.InheritedPropertyChangedKey);
                if (handler != null)
                {
                    InheritedPropertyChangedEventArgs args = new(ref info);
                    ((InheritedPropertyChangedEventHandler)handler)(this, args);
                }
            }
        }

        // Invalidate all the properties that may have changed as a result of
        //  changing this element's parent in the logical (and sometimes visual tree.)
        internal FrugalObjectList<DependencyProperty> InvalidateTreeDependentProperties(
            TreeChangeInfo parentTreeState,
            bool isSelfInheritanceParent,
            bool wasSelfInheritanceParent)
        {
            AncestorChangeInProgress = true;

            // False == we don't know whether we're in a visibility collapsed tree.
            InVisibilityCollapsedTree = false;

            if (parentTreeState.TopmostCollapsedParentNode == null)
            {
            }
            else
            {
                // There is an ancestor node somewhere above us with
                //  Visibility=Collapsed.  We're in a visibility collapsed subtree.
                InVisibilityCollapsedTree = true;
            }

            try
            {
                bool hasParent = FrameworkElement.GetFrameworkParent(
                    this,
                    out FrameworkElement parentFE);

                DependencyObject? parent = null;
                InheritanceBehavior parentInheritanceBehavior = InheritanceBehavior.Default;
                if (hasParent)
                {
                    if (parentFE != null)
                    {
                        parent = parentFE;
                        parentInheritanceBehavior = parentFE.InheritanceBehavior;
                    }
                }

                if (!TreeWalkHelper.SkipNext(InheritanceBehavior) &&
                    !TreeWalkHelper.SkipNow(parentInheritanceBehavior))
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

                // Loop through all cached inheritable properties for the parent to see if they
                // should be invalidated.
                return TreeWalkHelper.InvalidateTreeDependentProperties(
                    parentTreeState,
                    this,
                    isSelfInheritanceParent,
                    wasSelfInheritanceParent);
            }
            finally
            {
                AncestorChangeInProgress = false;

                // 'false' just means 'we don't know' - see comment at definition of the flag.
                InVisibilityCollapsedTree = false;
            }
        }

        /// <summary>
        ///     Notification that a specified property has been changed
        /// </summary>
        /// <param name="e">EventArgs that contains the property, metadata, old value,
        /// and new value for this change</param>
        internal override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            DependencyProperty dp = e.Property;

            // invalid during a VisualTreeChanged event
            // VisualDiagnostics.VerifyVisualTreeChange(this);
            base.OnPropertyChanged(e);

            // Invalidation propagation for Groups and Inheritance

            // Metadata must exist specifically stating propagate invalidation
            // due to group or inheritance
            if (e.Metadata is FrameworkPropertyMetadata fmetadata)
            {
                if (fmetadata.Inherits)
                {
                    // Invalidate Inheritable descendents only if instance is not a TreeSeparator
                    // or fmetadata.OverridesInheritanceBehavior is set to override separated
                    // tree behavior
                    if ((InheritanceBehavior == InheritanceBehavior.Default
                        || fmetadata.OverridesInheritanceBehavior) &&
                        (!DependencyObject.IsTreeWalkOperation(e.OperationType)
                        || PotentiallyHasMentees))
                    {
                        EffectiveValueEntry newEntry = e.NewEntry;
                        EffectiveValueEntry oldEntry = e.OldEntry;
                        if (oldEntry.BaseValueSourceInternal > newEntry.BaseValueSourceInternal)
                        {
                            // valuesource == Inherited && value == UnsetValue indicates that we
                            // are clearing the inherited value
                            newEntry = new EffectiveValueEntry(dp, BaseValueSourceInternal.Inherited);
                        }
                        else
                        {
                            newEntry = newEntry.GetFlattenedEntry(RequestFlags.FullyResolved);
                            newEntry.BaseValueSourceInternal = BaseValueSourceInternal.Inherited;
                        }

                        if (oldEntry.BaseValueSourceInternal != BaseValueSourceInternal.Default
                            || oldEntry.HasModifiers)
                        {
                            oldEntry = oldEntry.GetFlattenedEntry(RequestFlags.FullyResolved);
                            oldEntry.BaseValueSourceInternal = BaseValueSourceInternal.Inherited;
                        }
                        else
                        {
                            // we use an empty EffectiveValueEntry as a signal that the old entry
                            // was the default value
                            oldEntry = new EffectiveValueEntry();
                        }

                        InheritablePropertyChangeInfo info =
                                new(this, dp, oldEntry, newEntry);

                        // Don't InvalidateTree if we're in the middle of doing it.
                        if (!DependencyObject.IsTreeWalkOperation(e.OperationType))
                        {
                            TreeWalkHelper.InvalidateOnInheritablePropertyChange(this, info, true);
                        }

                        // Notify mentees if they exist
                        if (PotentiallyHasMentees)
                        {
                            TreeWalkHelper.OnInheritedPropertyChanged(
                                this,
                                ref info,
                                InheritanceBehavior);
                        }
                    }
                }

                if (e.IsAValueChange || e.IsASubPropertyChange)
                {
                    if (fmetadata.AffectsLayout)
                    {
                        if (this is IControl c)
                            c.PerformLayout();
                    }

                    if (fmetadata.AffectsPaint)
                    {
                        if (this is IControl c)
                            c.Invalidate();
                    }
                }
            }
        }

        private static void OnDataContextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == BindingExpressionBase.DisconnectedItem)
                return;

            ((FrameworkElement)d).RaiseDependencyPropertyChanged(DataContextChangedKey, e);
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

        // Clear the inheritance context (called when the element
        // gets a real parent
        private void ClearInheritanceContext()
        {
            if (InheritanceContext != null)
            {
                InheritanceContextField.ClearValue(this);
                OnInheritanceContextChanged(EventArgs.Empty);
            }
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
    }
}