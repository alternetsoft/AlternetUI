# Framework Overview

The design of the AlterNET UI framework is influenced by **Windows Forms** and **Windows Presentation Foundation (WPF)** frameworks from Microsoft. While
incorporating WPF-inspired features like UIXML, AlterNET UI design aims to bring the developers the ease of use of Windows Forms, as well as a
platform-native look and feel.

The basic building blocks of a typical AlterNET UI application are simple. One such class is <xref:Alternet.UI.Application>, which allows to start and stop an
application, and a <xref:Alternet.UI.Window>, which represents an on-screen window to display UI elements inside of it.
A UI inside a <xref:Alternet.UI.Window> is usually defined by a pair
of the UIXML markup code file and C# or Visual Basic (**code-behind**) file with event handlers and programming logic.

UIXML markup code is very similar to XAML. Using UIXML follows an approach of separating visual layout from code. The visual layout is then defined by
a declarative UIXML document, and the code-behind files which are written in C# or Visual Basic. The application logic is implemented in these
code-behind files. This approach is inspired by WPF design and is proven by a widespread industry use.

The following example shows how you can create a window with a few controls as part of a user interface. 

[!code-xml[](examples/uixml-sample.uixml)]

[!code-csharp[](examples/code-behind-csharp.cs)]

[Explore more examples on GitHub](https://github.com/todo).

### Native controls look and feel

AlterNET UI provides a set of standard controls which use native API and look and feel exactly like native elements on all platforms and different screen resolutions.

AlterNET UI provides the following core controls:

**Containers**: <xref:Alternet.UI.Grid>, <xref:Alternet.UI.StackPanel>, <xref:Alternet.UI.GroupBox>, <xref:Alternet.UI.Border> and <xref:Alternet.UI.TabControl>.

These controls act as containers for other controls and provide different kind of layouts in your windows.

**Inputs controls**: <xref:Alternet.UI.Button>, <xref:Alternet.UI.CheckBox>, <xref:Alternet.UI.ComboBox>, <xref:Alternet.UI.RadioButton>,
<xref:Alternet.UI.NumericUpDown>, <xref:Alternet.UI.TextBox> and <xref:Alternet.UI.Slider>.

These controls most often detect and respond to user input. The control classes expose API to handle text and mouse input, focus management, and more.

**Data display**: <xref:Alternet.UI.ListBox>, <xref:Alternet.UI.ListView>, <xref:Alternet.UI.TreeView>.

These controls provide a visual representation of a data elements in different layouts or views.

**Informational**: <xref:Alternet.UI.Label>, <xref:Alternet.UI.ProgressBar>.

These controls are designed to present an information to the user in a visual form.

In AlterNET UI, each control is defined within a rectangle that represents its boundaries. The actual size of this rectangle is calculated by the
layout system at runtime using automatic measurements based on the available screen size, parent properties and element properties such as border,
width, height, margin and padding.

### Platform-independent graphics device interface

AlterNET UI incudes set of resolution-independent graphics features that use native rendering on every supported platform.

It supports rendering graphic primitives such as text, images, graphic shapes with different fonts, pens and brushes.

The following code example illustrates how graphics can be drawn in a UI element:

[!code-csharp[](../tutorials/drawing-context/examples/DrawingContextTutorial/DrawingControl-Step4.cs)]

Refer to our [blog post](https://www.alternet-ui.com/blog/drawing-context-tutorial) to see it in action.