---
uid: Alternet.UI.Button
remarks: *content
---
A button is one of the most commonly used UI controls. It is designed to mimic a real-world push button. When the user clicks on a button,
the <xref:Alternet.UI.AbstractControl.Click> event is raised.
This event is then handled to execute some code in response to the user clicking a button.

Examples of how a button can look on different platforms:

# [Windows](#tab/screenshot-windows)
![Button on Windows](images/button-windows.png)
# [macOS](#tab/screenshot-macos)
![Button on macOS](images/button-macos.png)
# [Linux](#tab/screenshot-linux)
![Button on Linux](images/button-linux.png)
***

Set <xref:Alternet.UI.ButtonBase.Text> property to specify the text displayed on the control.
A button, like any other <xref:Alternet.UI.Control>, can be disabled by setting its <xref:Alternet.UI.AbstractControl.Enabled> property to `false`.