:: STEPS: 
:: 1. Modify WxWidgetsDownloadUrl and WebViewDownloadUrl in Source\Build\Alternet.UI.Pal\Alternet.UI.Pal.props 
:: 2. Copy microsoft.web.webview2.1.0.*.zip or other version to localhost (this is just nuget with renamed extension to zip)
:: 3. Copy new WxWidgets from External to localhost
:: 4. call MSW.Update.WxWidgets.bat
:: 5. upload wxWidgets-bin-noobjpch-*.7z to cloud and modify urls


call MSW.Update.WxWidgets.SubTool.1.DownloadAndExtract.bat
call MSW.Update.WxWidgets.SubTool.2.Defines.bat
call MSW.Update.WxWidgets.SubTool.3.BuildWithoutDownload.bat
:: MSW.Update.WxWidgets.SubTool.4.WinRar.bat
call MSW.Update.WxWidgets.SubTool.4.7z.bat
