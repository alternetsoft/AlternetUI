---
uid: Alternet.UI.Button
remarks: *content
---
TODO: THIS IS A TEST COPY FROM WINFORMS, WRITE A PROPER BUTTON REMARKS.

Member cross-reference test: <xref:Alternet.UI.Control.Visible>, <xref:Alternet.UI.Control.Visible?text=Control.Visible>
A Button can be clicked by using the mouse, ENTER key, or SPACEBAR if the button has focus.

Set the AcceptButton or CancelButton property of a Form to allow users to click a button by pressing the ENTER or ESC keys even if the button does not have focus. This gives the form the behavior of a dialog box.

When you display a form using the ShowDialog method, you can use the DialogResult property of a button to specify the return value of ShowDialog.

You can change the button's appearance. For example, to make it appear flat for a Web look, set the FlatStyle property to FlatStyle.Flat. The FlatStyle property can also be set to FlatStyle.Popup, which appears flat until the mouse pointer passes over the button; then the button takes on the standard Windows button appearance.

> [!NOTE]
> If the control that has focus accepts and processes the ENTER key press, the Button does not process it. For example, if a multiline TextBox or another button has focus, that control processes the ENTER key press instead of the accept button.

See also: @Control_concepts