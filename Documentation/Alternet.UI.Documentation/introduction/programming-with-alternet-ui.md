# Programming with AlterNET UI

The main class of AlterNET UI is <xref:Alternet.UI.Application>, which provides methods and properties to manage an application, such as methods to start and stop an
application, and properties to get information about an application, and a Window, which represents a top-level form control. Window is defined by a
UIXML markup code and C# or Visual Basic file with event handlers and programming logic.

UIXML markup code which is very similar to XAML and C# or Visual Basic code for programming. It follows WPF approach of separating visual layout which
is defined by declarative XML document and code-behind files where the behaviour is implemented.

### UIXML Markup

UIXML is a declarative markup language, very similar to XAML. UIXML simplifies creating an UI for a .NET applications. You can create visible UI
elements in the declarative UIXML markup, and then separate the UI definition from the run-time logic by using code-behind files that are joined to
the markup through partial class definitions. UIXML directly represents the instantiation of objects in a specific set of backing types defined in
assemblies. UIXML enables a workflow where separate parties can work on the UI and the logic of an app, using potentially 

When represented as text, UIXML files are XML files that generally have the .uixml extension. The files can be encoded by any XML encoding, but
encoding as UTF-8 is typical.

The following example shows how you can create a panel with few elements as part of a UI:

[!code-xml[](examples/uixml-sample.uixml)]

UIXML document is paired with C# or Visual Basic file with the same name and .cs or .vb extension (such as MainWindow.uixml.cs) which represents a
partial class used to implement initialization logic, handle events, etc.

Very much like in WPF the following rules apply:

- The partial class must derive from the type that backs the root element.
- The event handlers you define in UIXML document should be matched by instance methods in code-behind file.
- The event handlers must match the delegate for the appropriate event in the backing type system.

[!code-csharp[](examples/code-behind-csharp.cs)]

### Layout and Controls

In AlterNET UI, each element is defined within a rectangle that represents the boundaries of an element. The actual size of this rectangle is
calculated by the layout system at runtime after calculations based on the screen size, parent properties and element properties such as border,
width, height, margin and padding.

When you create a user interface, you arrange your controls by location and size to form a layout. A key requirement of any layout is to adapt to
changes in window size and display settings. Rather than forcing you to write the code to adapt a layout in these circumstances, the layout engine
does it for you.

AlterNET UI provides the following core controls:

**Containers**: <xref:Alternet.UI.StackPanel>, <xref:Alternet.UI.GroupBox>, <xref:Alternet.UI.Border> and <xref:Alternet.UI.TabControl>.

These are one of the most important control types in UI. They act as containers for other controls and provide different kind of layouts of your windows.

**Inputs controls**: <xref:Alternet.UI.Button>, <xref:Alternet.UI.CheckBox>, <xref:Alternet.UI.ComboBox>, <xref:Alternet.UI.RadioButton>,
<xref:Alternet.UI.NumericUpDown>, <xref:Alternet.UI.TextBox> and <xref:Alternet.UI.Slider>.

Controls most often detect and respond to user input. AlterNET provides both direct and routed events to support text input, focus management, and mouse positioning.

**Data display**: <xref:Alternet.UI.ListBox>, <xref:Alternet.UI.ListView>, <xref:Alternet.UI.TreeView>.

These controls provide a visual representation of a data elements in different layouts or views.

**Informational**: <xref:Alternet.UI.Label>, <xref:Alternet.UI.ProgressBar>.

These controls are designed to present an information to the user in a visual form.

### Data Binding

Data binding is the process that establishes a connection between the app UI and the data it displays. It allows to get data into your application's
UI without having to set properties on each control each time a value changes.

### Rendering Graphics 

AlterNET UI incudes set of resolution-independent graphics features that use native rendering on every supported platform.
It supports rendering graphic primitives such as text, images, graphic shapes with different fonts and brushes.

[!code-csharp[](examples/rendering-graphics.cs)]