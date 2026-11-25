taskkill /F /IM explorer.exe

del /F /Q "%LocalAppData%\IconCache.db"
del /F /Q "%LocalAppData%\Microsoft\Windows\Explorer\iconcache*"
del /F /Q "%LocalAppData%\Microsoft\Windows\Explorer\thumbcache*"

shutdown /r /t 0