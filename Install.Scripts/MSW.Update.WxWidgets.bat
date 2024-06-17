:: STEPS: 
:: 1. Modify WxWidgetsDownloadUrlLocalHost in Source\Build\Alternet.UI.Pal\Alternet.UI.Pal.props 
:: 2. Copy new WxWidgets from External to localhost and cloud


call MSW.Update.WxWidgets.SubTool.1.DownloadAndExtract.bat
call MSW.Update.WxWidgets.SubTool.2.Defines.bat
call MSW.Update.WxWidgets.SubTool.3.BuildWithoutDownload.bat
call MSW.Update.WxWidgets.SubTool.4.WinRar.bat
