# Routed Events

This topic describes the concept of routed events in AlterNET UI. The topic
defines routed events terminology, describes how routed events are routed
through a tree of elements, summarizes how you handle routed events, and
introduces how to create custom routed events.

## Prerequisites

This topic assumes that you have basic knowledge of the common language runtime
(CLR) and object-oriented programming, as well as the concept of how the
relationships between AlterNET UI elements can be conceptualized as a tree. 
To follow the examples in this topic, you should also understand AlterNET UI applications or pages.

## What Is a Routed Event?

You can think about routed events either from a functional or implementation
perspective. Both definitions are presented here because some people find one
or the other definition more useful.

Functional definition: A routed event is a type of event that can invoke
handlers on multiple listeners in an element tree rather than just on an 
object that raised the event.

Implementation definition: A routed event is a CLR event that is backed by an
instance of the <xref:Alternet.UI.RoutedEvent> class and is processed by the
AlterNET UI event system.

A typical AlterNET UI application contains many elements. Whether created in code or
declared in UIXML, these elements exist in an element tree relationship to each
other. The event route can travel in one of two directions depending on the
event definition, but generally, the route travels from the source element and
then "bubbles" upward through the element tree until it reaches the element tree
root (typically a page or a window). This bubbling concept might be familiar to
you if you have worked with the DHTML object model previously.

### Top-level Scenarios for Routed Events

The following is a brief summary of the scenarios that motivated the routed
event concept, and why a typical CLR event was not adequate for these scenarios:

**Singular handler attachment points:** In Windows Forms, you would have to
attach the same handler multiple times to process events that could be raised
from multiple elements. Routed events enable you to attach that handler only
once, as was shown in the previous example, and use handler logic to determine
where the event came from if necessary.

**Class handling:** Routed events permit a static handler that is defined by the
class. This class handler has the opportunity to handle an event before any
attached instance handlers can.

**Referencing an event without reflection:** Certain code and markup techniques
require a way to identify a specific event. A routed event creates a
<xref:Alternet.UI.RoutedEvent> field as an identifier, which provides a
robust event identification technique that does not require static or runtime reflection.

### How Routed Events Are Implemented

A routed event is a CLR event that is backed by an instance of the
<xref:Alternet.UI.RoutedEvent> class and registered with the AlterNET UI event
system. The <xref:Alternet.UI.RoutedEvent> instance obtained from
registration is typically retained as a `public` `static` `readonly` field
member of the class that registers and thus "owns" the routed event. The
connection to the identically named CLR event (which is sometimes termed the
"wrapper" event) is accomplished by overriding the `add` and `remove`
implementations for the CLR event. Ordinarily, the `add` and `remove` are left
as an implicit default that uses the appropriate language-specific event syntax
for adding and removing handlers of that event. The routed event backing and
connection mechanism is conceptually similar to how a dependency property is a
CLR property that is backed by the <xref:Alternet.UI.DependencyProperty>
class and registered with the AlterNET UI property system.

The following example shows the declaration for a custom `Tap` routed event,
including the registration and exposure of the <xref:Alternet.UI.RoutedEvent>
identifier field and the `add` and `remove` implementations for the `Tap` CLR
event.

[!code-csharp[](snippets/ExampleWindow.uixml.cs#CSharpCreation)]

## Routing Strategies

Routed events use one of three routing strategies:

- **Bubbling:** Event handlers on the event source are invoked. The routed event
  then routes to successive parent elements until reaching the element tree
  root. Most routed events use the bubbling routing strategy. Bubbling routed
  events are generally used to report input or state changes from distinct
  controls or other UI elements.

- **Direct:** Only the source element itself is given the opportunity to invoke
  handlers in response. This is analogous to the "routing" that Windows Forms
  uses for events. However, unlike a standard CLR event, direct routed events
  support class handling.

- **Tunneling:** Initially, event handlers at the element tree root are invoked.
  The routed event then travels a route through successive child elements along
  the route towards the node element that is the routed event source (the
  element that raised the routed event). Tunneling routed events are often used
  or handled as part of the compositing for a control, such that events from
  composite parts can be deliberately suppressed or replaced by events that are
  specific to the complete control. Input events provided in AlterNET UI often come
  implemented as a tunneling/bubbling pair. Tunneling events are also sometimes
  referred to as Preview events because of a naming convention that is used for
  the pairs.

## Why Use Routed Events?

As an application developer, you do not always need to know or care that the
event you are handling is implemented as a routed event. Routed events have
special behavior, but that behavior is largely invisible if you are handling an
event on the element where it is raised.

Where routed events become powerful is if you use any of the suggested
scenarios: defining common handlers at a common root, compositing your own
control, or defining your own custom control class.

Routed event listeners and routed event sources do not need to share a common
event in their hierarchy. Any <xref:Alternet.UI.UIElement> can be an event listener for any routed
event. Therefore, you can use the full set of routed events available throughout
the working API set as a conceptual "interface" whereby disparate elements in
the application can exchange event information. This "interface" concept for
routed events is particularly applicable to input events.

Routed events can also be used to communicate through the element tree because
the event data for the event is perpetuated to each element in the route. One
element could change something in the event data, and that change would be
available to the next element in the route.

Other than the routing aspect, routed events support a class-handling mechanism whereby 
the class can specify
static methods that have the opportunity to handle routed events before any
registered instance handlers can access them. This is very useful in the control
design because your class can enforce event-driven class behaviors that
cannot be accidentally suppressed by handling an event on an instance.

Each of the above considerations is discussed in a separate section of this
topic.

## Adding and Implementing an Event Handler for a Routed Event

To add a handler for an event using UIXML, you declare the event name as an
attribute on the element that is an event listener. The value of the attribute
is the name of your implemented handler method, which must exist in the partial
class of the code-behind file.

```xml
<SampleControl Tap="Control_Tap" Name="sampleControl2" />
```

`Control_Tap` is the name of the implemented handler that contains the code that
handles the event.

[!code-csharp[](snippets/ExampleWindow.uixml.cs#CSharpCreation2)]

`Control_Tap` must have the same signature as the
<xref:Alternet.UI.RoutedEventHandler> delegate, which is the event handler
delegate for the event. The first parameter of all routed event handler delegates specifies the
element to which the event handler is added, and the second parameter specifies
the data for the event.

<xref:Alternet.UI.RoutedEventHandler> is the basic routed event handler
delegate. For routed events that are specialized for certain controls or
scenarios, the delegates to use for the routed event handlers also might become
more specialized so that they can transmit specialized event data. 

Adding a handler for a routed event in an application that is created in code is
straightforward. Routed event handlers can always be added through a helper
method <xref:Alternet.UI.UIElement.AddHandler%2A> (which is the same method
that the existing backing calls for `add`.) However, existing AlterNET UI routed events
generally have backing implementations of `add` and `remove` logic that allows
the handlers for routed events to be added by a language-specific event syntax,
which is more intuitive syntax than the helper method. The following is an
example usage of the helper method:

[!code-csharp[](snippets/ExampleWindow.uixml.cs#AddHandlerCode)]

The next example shows the C# operator syntax:

[!code-csharp[](snippets/ExampleWindow.uixml.cs#AddHandlerPlusEquals)]

### The Concept of Handled

All routed events share a common event database class,
<xref:Alternet.UI.RoutedEventArgs>. <xref:Alternet.UI.RoutedEventArgs>
defines the <xref:Alternet.UI.RoutedEventArgs.Handled%2A> property, which
takes a Boolean value. The purpose of the
<xref:Alternet.UI.RoutedEventArgs.Handled%2A> property is to enable any event
handler along the route to mark the routed event as *handled*, by setting the
value of <xref:Alternet.UI.RoutedEventArgs.Handled%2A> to `true`. After being
processed by the handler at one element along the route, the shared event data
is again reported to each listener along the route.

The value of <xref:Alternet.UI.RoutedEventArgs.Handled%2A> affects how a
routed event is reported or processed as it travels further along the route. If
<xref:Alternet.UI.RoutedEventArgs.Handled%2A> is `true` in the event data for
a routed event, then handlers that listen for that routed event on other
elements are generally no longer invoked for that particular event instance.
This is true both for handlers attached in UIXML and for handlers added by
language-specific event handler attachment syntaxes such as `+=` or `Handles`.
For most common handler scenarios, marking an event as handled by setting
<xref:Alternet.UI.RoutedEventArgs.Handled%2A> to `true` will "stop" routing
for either a tunneling route or a bubbling route and also for any event that is
handled at a point in the route by a class handler.

In addition to the behavior that
<xref:Alternet.UI.RoutedEventArgs.Handled%2A> state produces in routed
events, the concept of <xref:Alternet.UI.RoutedEventArgs.Handled%2A> has
implications for how you should design your application and write the event
handler code. You can conceptualize
<xref:Alternet.UI.RoutedEventArgs.Handled%2A> as being a simple protocol that
is exposed by routed events. Exactly how you use this protocol is up to you, but
the conceptual design for how the value of
<xref:Alternet.UI.RoutedEventArgs.Handled%2A> is intended to be used is as
follows:

- If a routed event is marked as handled, then it does not need to be handled
  again by other elements along that route.

- If a routed event is not marked as handled, then other listeners that were 
earlier along the route have chosen either not to register a handler or the 
handlers that were registered chose not to manipulate the event data and set
  <xref:Alternet.UI.RoutedEventArgs.Handled%2A> to `true`. (Or, it is, of
  course, possible that the current listener is the first point in the route.)
  Handlers on the current listener now have three possible courses of action:

  - Take no action at all; the event remains unhandled, and the event routes to
    the next listener.

  - Execute code in response to the event, but make the determination that the
    action taken was not substantial enough to warrant marking the event as
    handled. The event routes to the next listener.

  - Execute code in response to the event. Mark the event as handled in the
    event data passed to the handler because the action taken was deemed
    substantial enough to warrant marking as handled. The event still routes to
    the next listener, but with
    <xref:Alternet.UI.RoutedEventArgs.Handled%2A>=`true` in its event data,
    so only `handledEventsToo` listeners have the opportunity to invoke further
    handlers.

This conceptual design is reinforced by the routing behavior mentioned earlier:
it is more difficult (although still possible in code or styles) to attach
handlers for routed events that are invoked even if a previous handler 
along the route has already set <xref:Alternet.UI.RoutedEventArgs.Handled%2A> to
`true`.

In applications, it is quite common to just handle a bubbling routed event on 
the object that raised it, and not be concerned with the event's routing 
characteristics at all. However, it is still a good practice to mark the routed event 
as handled in the event data to prevent unanticipated side effects just
in case an element that is further up the element tree also has a handler
attached for that same routed event.

## Class Handlers

If you are defining a class that derives in some way from
<xref:Alternet.UI.DependencyObject>, you can also define and attach a class
handler for a routed event that is a declared or inherited event member of your
class. Class handlers are invoked before any instance listener handlers that are
attached to an instance of that class, whenever a routed event reaches an
element instance in its route.

Some AlterNET UI controls have inherent class handling for certain routed events. This
might give the outward appearance that the routed event is not ever raised, 
but in reality, it is being class handled, and the routed event can potentially still
be handled by your instance handlers if you use certain techniques. Also, many
base classes and controls expose virtual methods that can be used to override
class handling behavior.

## Attached Events in AlterNET UI

The UIXML language also defines a special type of event called an *attached
event*. An attached event enables you to add a handler for a particular event to
an arbitrary element. The element handling the event need not define or inherit
the attached event, and neither the object potentially raising the event nor the
destination handling instance must define or otherwise "own" that event as a
class member.
