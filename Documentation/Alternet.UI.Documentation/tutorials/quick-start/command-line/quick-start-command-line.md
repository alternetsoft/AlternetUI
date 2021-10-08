#  Quick Start with Command Line and Visual Studio Code

In this tutorial you will create a cross-platform desktop application using C#, .NET command line tools and Visual Studio Code.
The application will display a message box in response to a button click.

### Prerequisites

1. Download and install [.NET SDK](https://dotnet.microsoft.com/download/dotnet). Minimum supported SDK version is .NET Core 3.1.
1. Download and install [Visual Studio Code](https://code.visualstudio.com/download)
1. In Visual Studio Code, make sure the [C# extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp) is installed. For
   information about how to install extensions on Visual Studio Code, see [VS Code Extension Marketplace](https://code.visualstudio.com/docs/editor/extension-gallery).

### Create New Project

1. Create a new directory for your application, name it `HelloWorld`
1. Open the **Command Prompt** window (**Terminal** on macOS or Linux)
1. Navigate the terminal to the created directory:
    ```dos
    cd path/to/directory
    ```
1. Enter the following command to create a new project in the current directory:
    ```dos
    dotnet new alternet-ui
    ```
1. The following files will be created:
    ```
    HelloWorld.csproj
    MainWindow.uixml
    MainWindow.uixml.cs
    Program.cs
    ```

> [!NOTE]
> By default the created project will use .NET Core 3.1 as a target framework. If .NET Core 3.1 runtime is not installed on your machine you
> will be prompted to do so on the first application run.

