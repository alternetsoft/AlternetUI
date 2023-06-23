# Edge backend in WebBrowser

The Edge backend uses Microsoft's Edge WebView2. 
https://www.nuget.org/packages/Microsoft.Web.WebView2

It is available for Windows 7 and newer. This backend does not support 
custom schemes and virtual file systems. 

Instead of the installation process described in this file,
you can download wxWidgets binary by running 
**"AlternetUI/Source/Build/Scripts/Download.WxWidgets.Bin.Windows.bat"**.

This backend is not enabled by default. 
For the installation, instead of running "Build WxWidgets.bat", you need 
to follow these steps:

**STEP 1**. Run **"AlternetUI/Source/Build/Scripts/MSW.Download.WxWidgets.bat"**.
It downloads the wxWidgets library and the WebView2 SDK nuget package 
(Version 1.0.1774.30 or newer) and extracts the package (it's a zip archive) to 
"AlternetUI/External/wxWidgets/3rdparty/webview2" (you should have 
"3rdparty/webview2/build/native/include/WebView2.h" and other files after unpacking it).

**STEP 2**. Run **"AlternetUI/Source/Build/Scripts/MSW.Update.WxWidgets.Defines.bat"**.
It will edit file "\AlternetUI\External\wxWidgets\include\wx\msw\setup.h" and 
changes defines wxUSE_WEBVIEW_EDGE and wxUSE_WEBVIEW_EDGE_STATIC like this:
	#define wxUSE_WEBVIEW_EDGE 1
	#define wxUSE_WEBVIEW_EDGE_STATIC 1
You can do these changes manually, wihtout running the bat file.

**STEP 3**. Run **"AlternetUI/Source/Build/Scripts/MSW.Build.WxWidgets.NoDownload.bat"**. 

## Remarks

- Make sure to add a note about using the WebView2 SDK to your application documentation, 
as required by it's license.

- If you did not set wxUSE_WEBVIEW_EDGE_STATIC define to 1, you will need to copy 
WebView2Loader.dll from the subdirectory corresponding to the architecture used 
(x86 or x64) of "wxWidgets/3rdparty/webview2/build/" to your applications executable.

- When wxUSE_WEBVIEW_EDGE_STATIC is defined, it removes the dependency on 
WebView2Loader.dll at runtime.

- At runtime you can use WebBrowser.IsBackendAvailable() to check if the backend can be 
used (it will be available if WebView2Loader.dll can be loaded and Edge (Chromium) 
is installed).

- Your application can be configured to use a fixed version of the WebView2 runtime. 
You must download a fixed version of the runtime and place it in the subfolder of your 
application installation path. Also you need to use WebBrowser.SetBackendPath() 
to specify its usage before creating WebBrowser with the Edge backend. You can 
download the WebView2 Runtime from this url:
https://developer.microsoft.com/en-us/microsoft-edge/webview2/

- If you do not want to use a fixed version of the WebView2 runtime, you need to install a 
global version of the WebView2 Runtime to the client computer before using 
Edge backend. You can do it with Standalone Installer or Bootstrapper, which are 
also available from the download page of WebView2.

