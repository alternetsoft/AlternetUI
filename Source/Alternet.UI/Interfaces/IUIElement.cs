using System;
using System.Diagnostics;
using Alternet.UI.Internal.KnownBoxes;

namespace Alternet.UI
{
    internal interface IUIElement : /*DependencyObject,*/ IInputElement
    {
        event MouseEventHandler MouseDoubleClick;

        event EventHandler? LayoutUpdated;

        void AddToEventRoute(EventRoute route, RoutedEventArgs e);

        void AddHandler(
            RoutedEvent routedEvent,
            Delegate handler,
            bool handledEventsToo);
    }

    internal interface IUIElementInternalStatic
    {
        void RaiseEventImpl(DependencyObject sender, RoutedEventArgs args);

        void AddHandler(DependencyObject d, RoutedEvent routedEvent, Delegate handler);

        void RemoveHandler(DependencyObject d, RoutedEvent routedEvent, Delegate handler);

        void BuildRouteHelper(DependencyObject? e, EventRoute route, RoutedEventArgs args);
    }

    internal interface IUIElementInternal
    {
        EventHandlersStore? EventHandlersStore { get; }

        void OnRemoveHandler(RoutedEvent routedEvent, Delegate handler);

        void EnsureEventHandlersStore();

        void WriteFlag(CoreFlags field, bool value);

        bool ReadFlag(CoreFlags field);

        void OnAddHandler(RoutedEvent routedEvent, Delegate handler);

        void RaiseLayoutUpdated();

        void AddToEventRouteCore(EventRoute route, RoutedEventArgs args);

        bool BuildRouteCore(EventRoute route, RoutedEventArgs args);

        object? AdjustEventSource(RoutedEventArgs args);

        DependencyObject? GetUIParentCore();

        DependencyObject? GetUIParent(bool v);
    }

    internal interface IUIElementProtected
    {
        void OnKeyDown(KeyEventArgs e);

        void OnTextInput(KeyPressEventArgs e);

        void OnKeyUp(KeyEventArgs e);

        void OnMouseDown(MouseEventArgs e);

        void OnMouseRightButtonDown(MouseEventArgs e);

        void OnMouseRightButtonUp(MouseEventArgs e);

        void OnMouseWheel(MouseEventArgs e);

        void OnMouseDoubleClick(MouseEventArgs e);

        void OnMouseUp(MouseEventArgs e);

        void OnMouseLeftButtonDown(MouseEventArgs e);

        void OnMouseLeftButtonUp(MouseEventArgs e);

        void OnMouseMove(MouseEventArgs e);

        void OnGotFocus(RoutedEventArgs e);

        void OnLostFocus(RoutedEventArgs e);
    }
}