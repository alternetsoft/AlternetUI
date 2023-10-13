using System;
using System.Diagnostics;
using Alternet.UI.Internal.KnownBoxes;

namespace Alternet.UI
{
    internal interface IUIElement : /*DependencyObject,*/ IInputElement
    {
        event TextInputEventHandler PreviewTextInput;

        event TextInputEventHandler TextInput;

        event MouseButtonEventHandler PreviewMouseDoubleClick;

        event MouseButtonEventHandler MouseDoubleClick;

        event DependencyPropertyChangedEventHandler FocusableChanged;

        event RoutedEventHandler GotFocus;

        event RoutedEventHandler LostFocus;

        event EventHandler? LayoutUpdated;

        bool Focusable { get; set; }

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

        void OnPreviewKeyDown(KeyEventArgs e);

        void OnTextInput(TextInputEventArgs e);

        void OnPreviewTextInput(TextInputEventArgs e);

        void OnPreviewKeyUp(KeyEventArgs e);

        void OnKeyUp(KeyEventArgs e);

        void OnPreviewMouseDown(MouseButtonEventArgs e);

        void OnMouseDown(MouseButtonEventArgs e);

        void OnPreviewMouseDoubleClick(MouseButtonEventArgs e);

        void OnMouseRightButtonDown(MouseButtonEventArgs e);

        void OnPreviewMouseRightButtonUp(MouseButtonEventArgs e);

        void OnMouseRightButtonUp(MouseButtonEventArgs e);

        void OnPreviewMouseWheel(MouseWheelEventArgs e);

        void OnMouseWheel(MouseWheelEventArgs e);

        void OnMouseDoubleClick(MouseButtonEventArgs e);

        void OnPreviewMouseUp(MouseButtonEventArgs e);

        void OnMouseUp(MouseButtonEventArgs e);

        void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e);

        void OnMouseLeftButtonDown(MouseButtonEventArgs e);

        void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e);

        void OnMouseLeftButtonUp(MouseButtonEventArgs e);

        void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e);

        void OnMouseMove(MouseEventArgs e);

        void OnPreviewMouseMove(MouseEventArgs e);

        void OnGotFocus(RoutedEventArgs e);

        void OnLostFocus(RoutedEventArgs e);
    }
}