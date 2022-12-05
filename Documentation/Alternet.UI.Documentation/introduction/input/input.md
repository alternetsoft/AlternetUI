# Input Overview
This article explains the architecture of the input systems in AlteNET UI.

## Input API
 The primary input API exposure is found on the base element classes:
 <xref:Alternet.UI.UIElement> and
 <xref:Alternet.UI.FrameworkElement>. These
 classes provide functionality for input events related to key presses, mouse
 buttons, mouse-wheel, mouse movement, focus management, and mouse capture, to
 name a few. By placing the input API on the base elements, rather than treating
 all input events as a service, the input architecture enables the input events
 to be sourced by a particular object in the UI and to support an event routing
 scheme whereby more than one element has an opportunity to handle an input
 event. Many input events have a pair of events associated with them. For
 example, the key-down event is associated with the
 <xref:Alternet.UI.Keyboard.KeyDownEvent> and
 <xref:Alternet.UI.Keyboard.PreviewKeyDownEvent> events. The difference in
 these events is in how they are routed to the target element. Preview events
 tunnel down the element tree from the root element to the target element.
 Bubbling events bubble up from the target element to the root element.

### Keyboard and Mouse Classes
 In addition to the input API on the base element classes, the
 <xref:Alternet.UI.Keyboard> class and
 <xref:Alternet.UI.Mouse> classes provide additional API for working
 with keyboard and mouse input.

 Examples of input API on the <xref:Alternet.UI.Keyboard> class are the
 <xref:Alternet.UI.Keyboard.Modifiers%2A> property, which returns the
 <xref:Alternet.UI.ModifierKeys> currently pressed, and the
 <xref:Alternet.UI.Keyboard.IsKeyDown%2A> method, which determines
 whether a specified key is pressed.

 The following example uses the
 <xref:Alternet.UI.Keyboard.GetKeyStates%2A> method to determine if a
 <xref:Alternet.UI.Key> is in the down state.

 [!code-csharp[](./snippets/KeyEventArgsKeyBoardGetKeyStates.cs)]

 An example of input API on the <xref:Alternet.UI.Mouse> class is
 <xref:Alternet.UI.Mouse.MiddleButton%2A>, which obtains the state of
 the middle mouse button.

 The following example determines whether the
 <xref:Alternet.UI.Mouse.LeftButton%2A> on the mouse is in the
 <xref:Alternet.UI.MouseButtonState.Pressed> state.

[!code-csharp[](./snippets/MouseRelatedSnippetsGetLeftButtonMouse.cs)]

 The <xref:Alternet.UI.Mouse> and <xref:Alternet.UI.Keyboard>
 classes are covered in more detail throughout this overview.

## Event Routing
 A <xref:Alternet.UI.FrameworkElement> can contain other elements as child
 elements in its content model, forming a tree of elements. In AlterNET UI, the parent
 element can participate in input directed to its child elements or other
 descendants by handing events. This is especially useful for building controls
 out of smaller controls, a process known as "control composition" or
 "compositing."

 Event routing is the process of forwarding events to multiple elements so that
 a particular object or element along the route can choose to offer a
 significant response (through handling) to an event that might have been
 sourced by a different element. Routed events use one of three routing
 mechanisms: direct, bubbling, and tunneling. In direct routing, the source
 element is the only element notified, and the event is not routed to any other
 elements. However, the direct routed event still offers some additional
 capabilities that are only present for routed events as opposed to standard CLR
 events. Bubbling works up the element tree by first notifying the element that
 sourced the event, then the parent element, and so on. Tunneling starts at the
 root of the element tree and works down, ending with the original source
 element. For more information about routed events, see [Routed Events](../routed-events/routed-events.md).

 AlterNET UI input events generally come in pairs that consist of a tunneling event and
 a bubbling event. Tunneling events are distinguished from bubbling events with
 the "Preview" prefix. For instance,
 <xref:Alternet.UI.Mouse.PreviewMouseMoveEvent> is the tunneling version of
 a mouse move event and <xref:Alternet.UI.Mouse.MouseMoveEvent> is the
 bubbling version of this event. This event pairing is a convention that is
 implemented at the element level and is not an inherent capability of the AlterNET UI
 event system.

## Handling Input Events
 To handle an element's input, an event handler must be associated with that
 particular event. In UIXML this is straightforward: you reference the name of
 the event as an attribute of the element that will be listening for this event.
 Then, you set the value of the attribute to the name of the event handler that
 you define, based on a delegate. The event handler must be written in code
 such as C# and can be included in a code-behind file.

 Keyboard events occur when the operating system reports key actions that occur
 while the keyboard focus is on an element. Mouse and stylus events each fall into
 two categories: events that report changes in pointer position relative to the
 element and events that report changes in the state of device buttons.

### Keyboard Input Event Example
 The following example listens for a left arrow key press. A
 <xref:Alternet.UI.StackPanel> is created that has a
 <xref:Alternet.UI.Button>. An event handler to listen for the left
 arrow key press is attached to the <xref:Alternet.UI.Button>
 instance.

 The first section of the example creates the
 <xref:Alternet.UI.StackPanel> and the
 <xref:Alternet.UI.Button> and attaches the event handler for the
 <xref:Alternet.UI.UIElement.KeyDown>.

 [!code-xml[](./snippets/Input_OvwKeyboardExampleUIXML.uixml)]

 [!code-csharp[](./snippets/Input_OvwKeyboardExampleUICodeBehind.cs)]

 The second section is written in code and defines the event handler. When the
 left arrow key is pressed, and the <xref:Alternet.UI.Button> has
 keyboard focus, the handler runs and the
 <xref:Alternet.UI.Control.Background%2A> color of the
 <xref:Alternet.UI.Button> is changed. If the key is pressed, but
 it is not the left arrow key, the
 <xref:Alternet.UI.Control.Background%2A> color of the
 <xref:Alternet.UI.Button> is changed back to its starting color.

  [!code-csharp[](./snippets/Input_OvwKeyboardExampleHandlerCodeBehind.cs)]

### Mouse Input Event Example
 In the following example, the
 <xref:Alternet.UI.Control.Background%2A> color of a
 <xref:Alternet.UI.Button> is changed when the mouse pointer enters
 the <xref:Alternet.UI.Button>. The
 <xref:Alternet.UI.Control.Background%2A> color is restored when the
 mouse leaves the <xref:Alternet.UI.Button>.

 The first section of the example creates the
 <xref:Alternet.UI.StackPanel> and the
 <xref:Alternet.UI.Button> control and attaches the event handlers
 for the <xref:Alternet.UI.Control.MouseEnter> and
 <xref:Alternet.UI.Control.MouseLeave> events to the
 <xref:Alternet.UI.Button>.

  [!code-xml[](./snippets/Input_OvwMouseExampleUIXML.uixml)]
  [!code-csharp[](./snippets/Input_OvwMouseExampleUICodeBehind.cs)]

 The second section of the example is written in code and defines the event
 handlers. When the mouse enters the <xref:Alternet.UI.Button>, the
 <xref:Alternet.UI.Control.Background%2A> color of the
 <xref:Alternet.UI.Button> is changed to
 <xref:Alternet.Drawing.Brushes.SlateGray%2A>.  When the mouse leaves the
 <xref:Alternet.UI.Button>, the
 <xref:Alternet.UI.Control.Background%2A> color of the
 <xref:Alternet.UI.Button> is changed back to
 <xref:Alternet.Drawing.Brushes.AliceBlue%2A>.

   [!code-csharp[](./snippets/Input_OvwMouseExampleEneterHandler.cs)]

   [!code-csharp[](./snippets/Input_OvwMouseExampleLeaveHandler.cs)]

## Text Input
 The <xref:Alternet.UI.UIElement.TextInput> event enables you to listen
 for text input in a device-independent manner. The keyboard is the primary
 means of text input, but speech, handwriting, and other input devices can
 generate text input also.

 For keyboard input, AlterNET UI first sends the appropriate
 <xref:Alternet.UI.UIElement.KeyDown>/<xref:Alternet.UI.UIElement.KeyUp>
 events. If those events are not handled, and the key is textual (rather than a
 control key such as directional arrows or function keys), then a
 <xref:Alternet.UI.UIElement.TextInput> event is raised. There is not
 always a simple one-to-one mapping between
 <xref:Alternet.UI.UIElement.KeyDown>/<xref:Alternet.UI.UIElement.KeyUp>
 and <xref:Alternet.UI.UIElement.TextInput> events because multiple
 keystrokes can generate a single character of text input, and single keystrokes
 can generate multi-character strings. This is especially true for languages
 such as Chinese, Japanese, and Korean, which use Input Method Editors (IMEs) to
 generate the thousands of possible characters in their corresponding alphabets.

 The following example defines a handler for the
 <xref:Alternet.UI.Control.Click> event and a handler
 for the <xref:Alternet.UI.UIElement.KeyDown> event.

 The first segment of code or markup creates the user interface.

 [!code-xml[](./snippets/Input_OvwTextInputUIXML.uixml)]

 [!code-csharp[](./snippets/Input_OvwTextInputUICodeBehind.cs)]

 The second segment of the code contains the event handlers.

 [!code-csharp[](./snippets/Input_OvwTextInputHandlersCodeBehind.cs)]

 Because input events bubble up the event route, the
 <xref:Alternet.UI.StackPanel> receives the input regardless of
 which element has keyboard focus. The <xref:Alternet.UI.TextBox>
 control is notified first, and the `OnTextInputKeyDown` handler is called only
 if the <xref:Alternet.UI.TextBox> did not handle the input. If the
 <xref:Alternet.UI.UIElement.PreviewKeyDown> event is used instead of the
 <xref:Alternet.UI.UIElement.KeyDown> event, the `OnTextInputKeyDown` handler
 is called first.

## Mouse Position
 The AlterNET UI input API provides helpful information with regard to coordinate
 spaces. For example, coordinate `(0,0)` is the upper-left coordinate, but the
 upper-left of which element in the tree? The element that is the input target?
 The element you attached your event handler to? Or something else? To avoid
 confusion, the AlterNET UI input API requires that you specify your frame of reference
 when you work with coordinates obtained through the mouse. The
 <xref:Alternet.UI.Mouse.GetPosition%2A> method returns the coordinate
 of the mouse pointer relative to the specified element.

## Mouse Capture
 Mouse devices specifically hold a modal characteristic known as mouse capture.
 Mouse capture is used to maintain a transitional input state when a
 drag-and-drop operation is started so that other operations involving the
 nominal on-screen position of the mouse pointer do not necessarily occur.
 During the drag, the user cannot click without aborting the drag-and-drop,
 which makes most mouseover cues inappropriate while the mouse capture is held
 by the drag origin. The input system exposes APIs that can determine the mouse
 capture state, as well as APIs that can force mouse capture to a specific
 element or clear the mouse capture state.

## The Input System and Base Elements
 Input events such as the attached events defined by the
 <xref:Alternet.UI.Mouse> and <xref:Alternet.UI.Keyboard> classes are raised by the input system and
 injected into a particular position in the object model based on hit testing
 the visual tree at run time.

 Each of the events that <xref:Alternet.UI.Mouse> and
 <xref:Alternet.UI.Keyboard>
 define as an attached event is also re-exposed by the base element class
 <xref:Alternet.UI.UIElement> as a
 new routed event. The base element routed events are generated by classes
 handling the original attached event and reusing the event data.

 When the input event becomes associated with a particular source element
 through its base element input event implementation, it can be routed through
 the remainder of an event route that is based on a combination of logical and
 visual tree objects, and be handled by application code. Generally, it is more
 convenient to handle these device-related input events using the routed events
 on <xref:Alternet.UI.UIElement>,
 because you can use more intuitive event handler syntax both in UIXML and in
 code. You could choose to handle the attached event that initiated the process
 instead, but you would face several issues: the attached event may be marked
 handled by the base element class handling, and you need to use the accessor
 methods rather than true event syntax in order to attach handlers for attached
 events.