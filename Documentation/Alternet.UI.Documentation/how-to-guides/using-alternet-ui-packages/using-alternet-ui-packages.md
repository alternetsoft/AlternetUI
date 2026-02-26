# Using AlterNET UI NuGet Packages

The easiest way to create an AlterNET UI application project is to use the project templates that come with [AlterNET UI Visual Studio
extension](https://marketplace.visualstudio.com/items?itemName=AlternetSoftwarePTYLTD.AlternetUIForVS2022) or can be [installed separately using
command line](http://docs.alternet-ui.com/tutorials/hello-world/command-line/hello-world-command-line.html#prerequisites).

However, [AlterNET UI Nuget Packages](https://www.nuget.org/packages/Alternet.UI) can be referenced manually in a .NET project.
To reference AlterNET UI framework in your project, add the following lines to your `.csproj` file:
```xml
<ItemGroup>
    <PackageReference Include="Alternet.UI" Version="1.0.1" />
</ItemGroup>
```
The `Version` value can be set to one of the published versions of the AlterNET UI packages.

After you have added the packages, you can start a GUI application in your code.
You can use the following code to show an empty window in a .NET console application:
```csharp
class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        var application = new Alternet.UI.Application();
        var window = new Alternet.UI.Window();

        application.Run(window);

        window.Dispose();
        application.Dispose();
    }
}
```
If you would like to hide the console created by the console application, change its `OutputType` to `WinExe` like so:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>WinExe</OutputType>
  </PropertyGroup>
```