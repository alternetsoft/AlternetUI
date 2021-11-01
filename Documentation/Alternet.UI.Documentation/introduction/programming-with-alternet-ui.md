# Programming With AlterNET UI

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