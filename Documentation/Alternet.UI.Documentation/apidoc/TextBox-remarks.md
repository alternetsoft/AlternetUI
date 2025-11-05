---
uid: Alternet.UI.TextBox
remarks: *content
---
With the <xref:Alternet.UI.TextBox> control, the user can enter text in an application.

Examples of how a <xref:Alternet.UI.TextBox> can look on different platforms:

# [Windows](#tab/screenshot-windows)
![TextBox on Windows](images/textbox-windows.png)
# [macOS](#tab/screenshot-macos)
![TextBox on macOS](images/textbox-macos.png)
# [Linux](#tab/screenshot-linux)
![TextBox on Linux](images/textbox-linux.png)
***

Set <xref:Alternet.UI.AbstractControl.Text> property to specify the text displayed on the control.
A <xref:Alternet.UI.TextBox>, like any other <xref:Alternet.UI.Control>, can be disabled by
 setting its <xref:Alternet.UI.AbstractControl.Enabled> property to `false`.

## TextBox Text Format

The multiline text controls always store the text as a sequence of lines separated by '\n' characters, 
i.e. in the Unix text format even on non-Unix platforms. This allows the user code to ignore the 
differences between the platforms but at a price: the indices in the control such as those 
returned by <xref:Alternet.UI.TextBox.GetInsertionPoint> or <xref:Alternet.UI.TextBox.GetSelectionStart> can not 
be used as indices into the string returned 
by <xref:Alternet.UI.AbstractControl.Text> as they're going to be slightly off for platforms using "\\r\\n" 
as separator (as Windows does).

Instead, if you need to obtain a substring between the 2 indices obtained from the control with the help 
of the functions mentioned above, you should use GetRange(). And the indices themselves
can only be passed to 
other methods, for example SetInsertionPoint() or SetSelection().

To summarize: never use the indices returned by (multiline) TextBox as indices into the string it contains, 
but only as arguments to be passed back to the other TextBox methods. This problem doesn't arise for 
single-line platforms however where the indices in the control do correspond to the positions in the value string.
