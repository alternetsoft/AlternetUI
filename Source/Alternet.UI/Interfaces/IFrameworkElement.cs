using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace Alternet.UI
{
    internal interface IFrameworkElement : IUIElement
    {
        /* public event DependencyPropertyChangedEventHandler DataContextChanged;*/

        string? Name { get; set; }

        /* IReadOnlyList<IFrameworkElement> ContentElements { get; }*/

        public object? DataContext { get; set; }

        /* IFrameworkElement? TryFindElement(string name);*/

        /* IUIElement FindElement(string name);*/
    }

    internal interface IFrameworkElementInternalStatic
    {
        EventPrivateKey ResourcesChangedKey { get; }

        EventPrivateKey DataContextChangedKey { get; }

        EventPrivateKey InheritedPropertyChangedKey { get; }

        bool GetFrameworkParent(
            IFrameworkElement current,
            out IFrameworkElement feParent);

        DependencyObject GetFrameworkParent(object current);

        void AddIntermediateElementsToRoute(
            DependencyObject mergePoint,
            EventRoute route,
            RoutedEventArgs args,
            DependencyObject modelTreeNode);
    }

    internal interface IFrameworkElementProtected
    {
        IEnumerable<FrameworkElement> LogicalChildrenCollection { get; }
    }

    internal interface IFrameworkElementInternal
    {
        bool IsInitialized { get; set; }

        IFrameworkElement? LogicalParent { get; set; }

        bool IsParentAnFE { get; set; }

        bool IsLogicalChildrenIterationInProgress { get; set; }

        InheritanceBehavior InheritanceBehavior { get; set; }

        IEnumerator LogicalChildren { get; }

        bool ShouldLookupImplicitStyles { get; set; }

        bool HasLogicalChildren { get; }

        bool AncestorChangeInProgress { get; set; }

        bool InVisibilityCollapsedTree { get; set; }

        bool PotentiallyHasMentees { get; set; }

        void WriteInternalFlag(InternalFlags reqFlag, bool set);

        Expression? GetExpressionCore(
            DependencyProperty dp,
            PropertyMetadata metadata);

        void OnNewParent(DependencyObject? oldParent, DependencyObject? newParent);

        void OnAncestorChangedInternal(TreeChangeInfo parentTreeState);

        void RaiseClrEvent(EventPrivateKey key, EventArgs args);

        void OnAncestorChanged();

        void RaiseInheritedPropertyChangedEvent(
            ref InheritablePropertyChangeInfo info);

        FrugalObjectList<DependencyProperty> InvalidateTreeDependentProperties(
            TreeChangeInfo parentTreeState,
            bool isSelfInheritanceParent,
            bool wasSelfInheritanceParent);

        void ChangeLogicalParent(
            DependencyObject? oldParent,
            DependencyObject? newParent);

        bool ReadInternalFlag(InternalFlags reqFlag);

        void AdjustBranchSource(RoutedEventArgs args);

        bool BuildRouteCoreHelper(
            EventRoute route,
            RoutedEventArgs args,
            bool shouldAddIntermediateElementsToRoute);

        bool IgnoreModelParentBuildRoute(RoutedEventArgs args);

        void EventHandlersStoreAdd(EventPrivateKey key, Delegate handler);

        void EventHandlersStoreRemove(EventPrivateKey key, Delegate handler);
    }
}