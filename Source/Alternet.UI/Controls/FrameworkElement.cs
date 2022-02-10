using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

        internal static DependencyObject GetFrameworkParent(object current)
        {
            FrameworkObject fo = new FrameworkObject(current as DependencyObject);

            fo = fo.FrameworkParent;

            return fo.DO;
        }
    }
}