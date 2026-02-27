# How to Use `.snupkg` Files with NuGet Packages in Visual Studio

The `.snupkg` files ("symbol packages") let you **debug and step into** the source code of NuGet packages, 
such as `Alternet.UI`, `Alternet.UI.Common`, and `Alternet.UI.Maui`, directly from Visual Studio.

These instructions explain how to configure Visual Studio to use our `.snupkg` files for a better debugging experience.

In this instruction, we use version 1.0.1 of the `Alternet.UI` NuGet packages. 
In practice, use the latest available version of our NuGet packages.

---

## Prerequisites

- **Visual Studio 2022** or newer is recommended (for full symbol package support).
- Project references the relevant NuGet packages (e.g., `Alternet.UI.1.0.1.nupkg`).
- You have the corresponding `.snupkg` files for these packages:
    - `Alternet.UI.1.0.1.snupkg`
    - `Alternet.UI.Common.1.0.1.snupkg`
    - `Alternet.UI.Maui.1.0.1.snupkg`

---

## Install NuGet Packages Normally

Add the packages to your project as usual (via NuGet Package Manager or CLI):

```shell
dotnet add package Alternet.UI --version 1.0.1
dotnet add package Alternet.UI.Common --version 1.0.1
dotnet add package Alternet.UI.Maui --version 1.0.1
```

---

## Configure Visual Studio to Use Local Symbol Packages

### Option 1: Using a Local Symbol Server Folder

1. **Put Your `.snupkg` Files in a Folder**
    - For example, `C:\snupkgs\`

2. **Extract `.snupkg` Files**
    - Visual Studio does not read `.snupkg` directly, but tools like [NuGet Symbol Server](https://github.com/NuGet/Symbols) 
or [dotnet-symbol](https://github.com/dotnet/symstore) can help. 
    - As a quick workaround, you can host them on a local file share, or (best) publish your NuGet packages to a 
[NuGet Symbol Server](https://docs.microsoft.com/en-us/nuget/create-packages/symbol-packages-snupkg#publishing-a-symbol-package).

### Option 2: Let Visual Studio Download from a Public Symbol Server

If your `.snupkg` files are published on [nuget.org](https://www.nuget.org/), Visual Studio will automatically find and download them 
if you have configured symbol settings.

---

## Set Up Visual Studio Debugging Options

1. **Go to** `Tools` > `Options` > `Debugging` > `Symbols`
2. **Add a symbol file location:**
    - Click the folder icon and add your local `.snupkg` folder **or** ensure Microsoft Symbol Servers are checked if using nuget.org.

      Example: `C:\snupkgs\`
3. **[Optional] Enable "All Modules" Search:** 
    - Check "Search Microsoft Symbol Servers" and "NuGet.org Symbol Server" as needed
4. **Enable "Source Link support":**
    - Go to `Tools` > `Options` > `Debugging` > `General`
    - Check **Enable source server support** and **Enable Source Link support**

---

## Start Debugging

- Press **F5** or **F11** to start debugging and Step Into code.
- When hitting code from `Alternet.UI` or related packages, Visual Studio will try to download/load matching symbols and sources.
- For Step Into to work, the .snupkg file must match the package version.

---

## Troubleshooting Tips

- If Stepping In does not work, ensure package versions (nupkg and snupkg) **match exactly**.
- Make sure that all symbol servers are enabled and priority is correct (local before remote for faster lookup).
- "No source available" may mean the snupkg is missing, corrupted, or not accessible.
- You can clear the symbol cache (`Tools` > `Options` > `Debugging` > `Symbols` > "Empty Symbol Cache") and try again.

---

## Useful Resources

- [NuGet Symbol Packages docs (nuget.org)](https://docs.microsoft.com/en-us/nuget/create-packages/symbol-packages-snupkg)
- [Microsoft Docs: Debugging with Source Link](https://docs.microsoft.com/en-us/dotnet/standard/library-guidance/sourcelink)
- [Publish symbols with your NuGet package](https://devblogs.microsoft.com/nuget/publish-symbols-with-your-package/)

---

**You are now ready to step into and debug `Alternet.UI` code from your app using Visual Studio!**

If you encounter problems, double-check symbol settings, file versions, and Source Link support.
