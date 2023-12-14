using System;
using System.Diagnostics;
using Alternet.UI.Internal.KnownBoxes;

namespace Alternet.UI
{
    internal interface IUIElement : /*DependencyObject,*/ IInputElement
    {
        event TextInputEventHandler PreviewTextInput;

        event TextInputEventHandler TextInput;

        event MouseEventHandler PreviewMouseDoubleClick;

        event MouseEventHandler MouseDoubleClick;

        event EventHandler? LayoutUpdated;

        /*event DependencyPropertyChangedEventHandler FocusableChanged;*/

        /*bool Focusable { get; set; }*/

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

        void OnPreviewMouseDown(MouseEventArgs e);

        void OnMouseDown(MouseEventArgs e);

        void OnPreviewMouseDoubleClick(MouseEventArgs e);

        void OnMouseRightButtonDown(MouseEventArgs e);

        void OnPreviewMouseRightButtonUp(MouseEventArgs e);

        void OnMouseRightButtonUp(MouseEventArgs e);

        void OnPreviewMouseWheel(MouseEventArgs e);

        void OnMouseWheel(MouseEventArgs e);

        void OnMouseDoubleClick(MouseEventArgs e);

        void OnPreviewMouseUp(MouseEventArgs e);

        void OnMouseUp(MouseEventArgs e);

        void OnPreviewMouseLeftButtonDown(MouseEventArgs e);

        void OnMouseLeftButtonDown(MouseEventArgs e);

        void OnPreviewMouseLeftButtonUp(MouseEventArgs e);

        void OnMouseLeftButtonUp(MouseEventArgs e);

        void OnPreviewMouseRightButtonDown(MouseEventArgs e);

        void OnMouseMove(MouseEventArgs e);

        void OnPreviewMouseMove(MouseEventArgs e);

        void OnGotFocus(RoutedEventArgs e);

        void OnLostFocus(RoutedEventArgs e);
    }
}