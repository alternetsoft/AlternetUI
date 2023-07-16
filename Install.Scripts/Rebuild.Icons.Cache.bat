cd %homepath%\AppData\Local\Microsoft\Windows\Explorer
taskkill /f /im explorer.exe
del iconcache*
dir iconcache*
explorer.exe