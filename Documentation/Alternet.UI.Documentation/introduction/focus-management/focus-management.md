# Focus Management

AlterNET UI includes several API elements to control input focus.

Keyboard focus refers to the object that is receiving keyboard input. The element with keyboard focus has
<xref:Alternet.UI.Control.Focused> set to true. There can be only one element with keyboard focus on the entire desktop.

Keyboard focus can be obtained through user interaction with the UI, such as tabbing to an element or clicking the mouse
on certain elements. Keyboard focus can also be obtained programmatically by using the <xref:Alternet.UI.Control.SetFocus%2A> method.

The <xref:Alternet.UI.Control.SetFocus%2A> method returns true if the control successfully received input focus. The control can have the input focus
while not displaying any visual cues of having the focus. This behavior is primarily observed by the nonselectable
controls listed below, or any controls derived from them.

When the user presses the TAB key, the input focus is set to the next control in the tab order. Controls with the
<xref:Alternet.UI.Control.TabStop> property value of false are not included in the collection of controls in the tab order.

When you change the focus by using the keyboard (TAB, SHIFT+TAB, and so on), by calling the <xref:Alternet.UI.Control.SetFocus%2A> or
<xref:Alternet.UI.Control.FocusNextControl%2A>
methods focus events occur in the following order:

- <xref:Alternet.UI.Control.GotFocus>
- <xref:Alternet.UI.Control.LostFocus>